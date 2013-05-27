/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.Match
{
    public class ChsFullTextMatch: IChsFullTextMatch
    {
        class NodeComparer : IComparer<Node>
        {
            bool _FreqFirst;

            internal NodeComparer(bool frequencyFirst)
            {
                _FreqFirst = frequencyFirst;
            }

            #region IComparer<Node> Members

            public int Compare(Node x, Node y)
            {
                if (x.SpaceCount < y.SpaceCount)
                {
                    return -1;
                }
                else if (x.SpaceCount > y.SpaceCount)
                {
                    return 1;
                }
                else
                {
                    if (x.AboveCount < y.AboveCount)
                    {
                        return -1;
                    }
                    else if (x.AboveCount > y.AboveCount)
                    {
                        return 1;
                    }
                    else
                    {
                        if (_FreqFirst)
                        {
                            if (x.FreqSum > y.FreqSum)
                            {
                                return -1;
                            }
                            else if (x.FreqSum < y.FreqSum)
                            {
                                return 1;
                            }
                            else
                            {
                                if (x.SingleWordCount < y.SingleWordCount)
                                {
                                    return -1;
                                }
                                else if (x.SingleWordCount > y.SingleWordCount)
                                {
                                    return 1;
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                        }
                        else
                        {
                            if (x.SingleWordCount < y.SingleWordCount)
                            {
                                return -1;
                            }
                            else if (x.SingleWordCount > y.SingleWordCount)
                            {
                                return 1;
                            }
                            else
                            {
                                if (x.FreqSum > y.FreqSum)
                                {
                                    return -1;
                                }
                                else if (x.FreqSum < y.FreqSum)
                                {
                                    return 1;
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                }

            }

            #endregion
        }

        class Node : IComparable<Node>
        {
            public int AboveCount;
            public int SpaceCount;
            public double FreqSum;
            public int SingleWordCount;
            
            public Dict.PositionLength PositionLength;
            public Node Parent;

            public Node()
            {
                AboveCount = 0;
            }

            public Node(Node node)
            {
                this.AboveCount = node.AboveCount;
                this.SpaceCount = node.SpaceCount;
                this.FreqSum = node.FreqSum;
                this.SingleWordCount = node.SingleWordCount;
                this.PositionLength = node.PositionLength;
                this.Parent = null;
            }

            public Node(Dict.PositionLength pl, Node parent, int aboveCount, 
                int spaceCount, int singleWordCount, double freqSum)
            {
                PositionLength = pl;
                Parent = parent;
                AboveCount = aboveCount;
                SpaceCount = spaceCount;
                SingleWordCount = singleWordCount;
                FreqSum = freqSum;
            }


            #region IComparable<Node> Members

            public int CompareTo(Node other)
            {
                if (this.SpaceCount < other.SpaceCount)
                {
                    return -1;
                }
                else if (this.SpaceCount > other.SpaceCount)
                {
                    return 1;
                }
                else
                {
                    if (this.AboveCount < other.AboveCount)
                    {
                        return -1;
                    }
                    else if (this.AboveCount > other.AboveCount)
                    {
                        return 1;
                    }
                    else
                    {
                        if (this.FreqSum > other.FreqSum)
                        {
                            return -1;
                        }
                        else if (this.FreqSum < other.FreqSum)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }

            #endregion
        }

        struct NodeForTree
        {
            public Node Node;
            public int CurIndex;

            public NodeForTree(Node node, int curIndex)
            {
                Node = node;
                CurIndex = curIndex;
            }
        }

        Node _Root = new Node();

        Framework.AppendList<Node> _LeafNodeList = new PanGu.Framework.AppendList<Node>();
        Dict.PositionLength[] _PositionLengthArr;
        int _InputStringLength;
        int _PositionLengthArrCount;

        List<Dict.PositionLength[]> _AllCombinations = new List<PanGu.Dict.PositionLength[]>();
        Dict.WordDictionary _WordDict;

        const int TopRecord = 3;
        const POS SingleWordMask = POS.POS_D_C | POS.POS_D_P | POS.POS_D_R | POS.POS_D_U;

        /// <summary>
        /// Build tree 
        /// </summary>
        /// <param name="pl">position length list</param>
        /// <param name="count">position length list count</param>
        /// <param name="parent">parent node</param>
        /// <param name="curIndex">current index of position length list</param>
        private void BuildTree(Node parent, int curIndex)
        {
            //嵌套太多的情况一般很少发生，如果发生，强行中断，以免造成博弈树遍历层次过多
            //降低系统效率
            if (_LeafNodeList.Count > 8192)
            {
                return;
            }

            if (curIndex < _PositionLengthArrCount - 1)
            {
                if (_PositionLengthArr[curIndex + 1].Position == _PositionLengthArr[curIndex].Position)
                {
                    BuildTree(parent, curIndex + 1);
                }
            }

            int spaceCount = parent.SpaceCount + _PositionLengthArr[curIndex].Position - (parent.PositionLength.Position + parent.PositionLength.Length);

            int singleWordCount = parent.SingleWordCount + (_PositionLengthArr[curIndex].Length == 1 ? 1 : 0);
            double freqSum = 0;

            if (_Options != null)
            {
                if (_Options.FrequencyFirst)
                {
                    freqSum = parent.FreqSum + _PositionLengthArr[curIndex].WordAttr.Frequency;
                }
            }

            Node curNode = new Node(_PositionLengthArr[curIndex], parent, parent.AboveCount + 1, spaceCount, singleWordCount, freqSum);

            int cur = curIndex + 1;
            while (cur < _PositionLengthArrCount)
            {
                if (_PositionLengthArr[cur].Position >= _PositionLengthArr[curIndex].Position + _PositionLengthArr[curIndex].Length)
                {
                    BuildTree(curNode, cur);
                    break;
                }

                cur++;
            }

            if (cur >= _PositionLengthArrCount)
            {
                curNode.SpaceCount += _InputStringLength - curNode.PositionLength.Position - curNode.PositionLength.Length;
                _LeafNodeList.Add(curNode);
            }

        }


        private void BuildTreeStack(Node parent, int curIndex)
        {
            NodeForTree[] stack = new NodeForTree[_PositionLengthArrCount];
            int stackPoint = -1;

            //Stack<NodeForTree> stack = new Stack<NodeForTree>(_PositionLengthArrCount);

            stackPoint++;
            stack[stackPoint] = new NodeForTree(parent, curIndex);

            while (stackPoint >= 0)
            {
                NodeForTree curNodeForTree = stack[stackPoint];
                stackPoint--;
                parent = curNodeForTree.Node;
                curIndex = curNodeForTree.CurIndex;

                //嵌套太多的情况一般很少发生，如果发生，强行中断，以免造成博弈树遍历层次过多
                //降低系统效率
                if (_LeafNodeList.Count > 8192)
                {
                    return;
                }

                if (curIndex < _PositionLengthArrCount - 1)
                {
                    if (_PositionLengthArr[curIndex + 1].Position == _PositionLengthArr[curIndex].Position)
                    {
                        //BuildTree(parent, curIndex + 1);
                        stackPoint++;
                        stack[stackPoint] = new NodeForTree(parent, curIndex + 1);
                    }
                }

                int spaceCount = parent.SpaceCount + _PositionLengthArr[curIndex].Position - (parent.PositionLength.Position + parent.PositionLength.Length);

                int singleWordCount = parent.SingleWordCount + (_PositionLengthArr[curIndex].Length == 1 ? 1 : 0);
                double freqSum = 0;

                if (_Options != null)
                {
                    if (_Options.FrequencyFirst)
                    {
                        freqSum = parent.FreqSum + _PositionLengthArr[curIndex].WordAttr.Frequency;
                    }
                }

                Node curNode = new Node(_PositionLengthArr[curIndex], parent, parent.AboveCount + 1, spaceCount, singleWordCount, freqSum);

                int cur = curIndex + 1;
                bool find = false;
                while (cur < _PositionLengthArrCount)
                {
                    if (_PositionLengthArr[cur].Position >= _PositionLengthArr[curIndex].Position + _PositionLengthArr[curIndex].Length)
                    {
                        //BuildTree(curNode, cur);
                        stackPoint++;
                        stack[stackPoint] = new NodeForTree(curNode, cur);
                        find = true;
                        break;
                    }

                    cur++;
                }

                if (find)
                {
                    continue;
                }

                if (cur >= _PositionLengthArrCount)
                {
                    curNode.SpaceCount += _InputStringLength - curNode.PositionLength.Position - curNode.PositionLength.Length;
                    _LeafNodeList.Add(curNode);
                }
            }
        }

        #region IChsFullTextMatch Members

        private MatchOptions _Options = null;
        public MatchOptions Options
        {
            get
            {
                return _Options;
            }
            set
            {
                _Options = value;
            }
        }


        private MatchParameter _Parameters = null;
        public MatchParameter Parameters
        {
            get
            {
                return _Parameters;
            }
            set
            {
                _Parameters = value;
            }
        }

        private ICollection<Dict.PositionLength> MergeAllCombinations(int redundancy)
        {
            LinkedList<Dict.PositionLength> result = new LinkedList<PanGu.Dict.PositionLength>();

            if ((redundancy == 0 || !_Options.MultiDimensionality) && !_Options.ForceSingleWord)
            {
                return _AllCombinations[0];
            }

            int i = 0;

            LinkedListNode<Dict.PositionLength> cur;

            bool forceOnce = false;

            Loop:

            while (i <= redundancy && i < _AllCombinations.Count)
            {
                cur = result.First;

                for (int j = 0; j < _AllCombinations[i].Length; j++)
                {
                    _AllCombinations[i][j].Level = i;

                    if (cur != null)
                    {
                        while (cur.Value.Position < _AllCombinations[i][j].Position)
                        {
                            cur = cur.Next;

                            if (cur == null)
                            {
                                break;
                            }
                        }

                        if (cur != null)
                        {
                            if (cur.Value.Position != _AllCombinations[i][j].Position ||
                                cur.Value.Length != _AllCombinations[i][j].Length)
                            {
                                result.AddBefore(cur, _AllCombinations[i][j]);
                            }
                        }
                        else
                        {
                            result.AddLast(_AllCombinations[i][j]);
                        }
                    }
                    else
                    {
                        result.AddLast(_AllCombinations[i][j]);
                    }
                }

                i++;
            }

            if (_Options.ForceSingleWord && !forceOnce)
            {
                i = _AllCombinations.Count - 1;
                redundancy = i;
                forceOnce = true;
                goto Loop;
            }

            return result;
        }

        private bool IsKnownSingleWord(int[] masks, int index, string orginalText)
        {

            int state = masks[index];
            if (state == 2)
            {
                return false;
            }

            if (state == 1)
            {
                if (!_Options.UnknownWordIdentify)
                {
                    return false;
                }

                //如果单字是连词、助词、介词、代词
                WordAttribute wa = _WordDict.GetWordAttr(orginalText[index].ToString());

                if (wa != null)
                {
                    if ((wa.Pos & SingleWordMask) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        private List<WordInfo> GetUnknowWords(int[] masks, string orginalText, out bool needRemoveSingleWord)
        {
            List<WordInfo> unknownWords = new List<WordInfo>();

            //找到所有未登录词
            needRemoveSingleWord = false;

            int j = 0;
            bool begin = false;
            int beginPosition = 0;
            while (j < masks.Length)
            {
                if (_Options.UnknownWordIdentify)
                {

                    if (!begin)
                    {
                        if (IsKnownSingleWord(masks, j, orginalText))
                        {
                            begin = true;
                            beginPosition = j;
                        }
                    }
                    else
                    {
                        bool mergeUnknownWord = true;

                        if (!IsKnownSingleWord(masks, j, orginalText))
                        {
                            if (j - beginPosition <= 2)
                            {
                                for (int k = beginPosition; k < j; k++)
                                {
                                    mergeUnknownWord = false;

                                    if (masks[k] != 1)
                                    {
                                        string word = orginalText.Substring(k, 1);
                                        WordInfo wi = new WordInfo();
                                        wi.Word = word;
                                        wi.Position = k;
                                        wi.WordType = WordType.None;
                                        wi.Rank = _Parameters.UnknowRank;
                                        unknownWords.Add(wi);
                                    }
                                }
                            }
                            else
                            {
                                for (int k = beginPosition; k < j; k++)
                                {
                                    if (masks[k] == 1)
                                    {
                                        masks[k] = 11;
                                        needRemoveSingleWord = true;
                                    }
                                }
                            }

                            begin = false;

                            if (mergeUnknownWord)
                            {
                                string word = orginalText.Substring(beginPosition,
                                    j - beginPosition);
                                WordInfo wi = new WordInfo();
                                wi.Word = word;
                                wi.Position = beginPosition;
                                wi.WordType = WordType.None;
                                wi.Rank = _Parameters.UnknowRank;
                                unknownWords.Add(wi);
                            }
                        }
                    }
                }
                else
                {
                    if (IsKnownSingleWord(masks, j, orginalText))
                    {
                        WordInfo wi = new WordInfo();
                        wi.Word = orginalText[j].ToString();
                        wi.Position = j;
                        wi.WordType = WordType.None;
                        wi.Rank = _Parameters.UnknowRank;
                        unknownWords.Add(wi);
                    }
                }

                j++;
            }

            if (begin && _Options.UnknownWordIdentify)
            {
                bool mergeUnknownWord = true;

                if (j - beginPosition <= 2)
                {
                    for (int k = beginPosition; k < j; k++)
                    {
                        mergeUnknownWord = false;

                        if (masks[k] != 1)
                        {
                            string word = orginalText.Substring(k, 1);
                            WordInfo wi = new WordInfo();
                            wi.Word = word;
                            wi.Position = k;
                            wi.WordType = WordType.None;
                            wi.Rank = _Parameters.UnknowRank;
                            unknownWords.Add(wi);
                        }
                    }
                }
                else
                {
                    for (int k = beginPosition; k < j; k++)
                    {
                        if (masks[k] == 1)
                        {
                            masks[k] = 11;
                            needRemoveSingleWord = true;
                        }
                    }
                }

                begin = false;

                if (mergeUnknownWord)
                {

                    string word = orginalText.Substring(beginPosition,
                        j - beginPosition);
                    WordInfo wi = new WordInfo();
                    wi.Word = word;
                    wi.Position = beginPosition;
                    wi.WordType = WordType.None;
                    wi.Rank = _Parameters.UnknowRank;
                    unknownWords.Add(wi);
                }
            }

            return unknownWords;
        }

        public ChsFullTextMatch(Dict.WordDictionary wordDict)
        {
            _WordDict = wordDict;
        }

        private void CombineNodeArr(Node[] result, Node[] arr)
        {
            if (arr.Length < result.Length)
            {
                Node[] newArr = new Node[result.Length];
                Array.Copy(arr, newArr, arr.Length);
            }


            //复制 arr 链表
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    if (arr[i] == null)
                    {
                        return;
                    }

                    continue;
                }

                if (i >= result.Length)
                {
                    break;
                }

                if (arr[i] == null)
                {
                    arr[i] = arr[i - 1];
                }

                Node fst = new Node(arr[i]);
                Node node = fst;
                Node n = arr[i];

                n = n.Parent;
                for (int j = 1; j < arr[i].AboveCount; j++)
                {
                    node.Parent = new Node(n);
                    node = node.Parent;
                    n = n.Parent;
                }

                arr[i] = fst;
            }


            //如果result 的有效值数量少于 arr,将result 的有效值填充到和arr相等
            //如果result 没有一个有效值，则不做处理
            for (int i = 0; i < result.Length; i++)
            {
                if (i >= arr.Length)
                {
                    break;
                }

                if (result[i] == null && arr[i] != null)
                {
                    if (i > 0)
                    {
                        result[i] = result[i - 1];
                    }
                }
            }

            for (int i = 0; i < result.Length; i++)
            {
                int j = i;
                if (arr.Length <= i)
                {
                    j = arr.Length - 1;
                }

                if (arr[j] == null)
                {
                    if (result[i] == null)
                    {
                        return;
                    }
                    else
                    {
                        while (arr[j] == null)
                        {
                            j--;
                        }
                    }
                }

                if (result[i] == null)
                {
                    //只有在result 没有一个有效值时才会到这个分支
                    result[i] = arr[j];
                }
                else
                {
                    Node n = arr[j];
                    for (int k = 0; k < arr[j].AboveCount - 1; k++)
                    {
                        n = n.Parent;
                    }

                    n.Parent = result[i];
                    int aboveCount = arr[j].AboveCount + result[i].AboveCount;
                    result[i] = arr[j];
                    result[i].AboveCount = aboveCount;
                }
            }


        }

        /// <summary>
        ///根据孤立点拆分长句，然后再分别对各个句子的片段进行分词.
        ///长中文句子的分词困扰了我3年，一直没有好的解决方案。没想到在观看
        ///2010年世界杯开幕式时，我突发灵感，想出了这个孤立点分割拆分长句的
        ///算法，彻底解决的这个长期困扰我的难题. 
        ///eaglet 11th Jun 2010 注释留念
        /// </summary>
        /// <param name="positionLenArr">保护位置和长度信息的单词分量数组</param>
        /// <param name="orginalTextLength">原始字符串长度</param>
        /// <param name="count">positionLenArr 的 count</param>
        /// <returns></returns>
        private Node[] GetLeafNodeArray(PanGu.Dict.PositionLength[] positionLenArr, int orginalTextLength, int count)
        {
            //Split by isolated point

            Node[] result = new Node[TopRecord];

            int lastRightBoundary = positionLenArr[0].Position + positionLenArr[0].Length;
            int lastIndex = 0;

            for (int i = 1; i < count; i++)
            {
                if (positionLenArr[i].Position >= lastRightBoundary)
                {
                    //last is isolated point
                    int c = i - lastIndex;
                    PanGu.Dict.PositionLength[] arr = new PanGu.Dict.PositionLength[c];
                    Array.Copy(positionLenArr, lastIndex, arr, 0, c);
                    Node[] leafNodeArray = GetLeafNodeArrayCore(arr, lastRightBoundary - positionLenArr[lastIndex].Position, c);
                    Framework.QuickSort<Node>.TopSort(leafNodeArray, _LeafNodeList.Count, (int)Math.Min(TopRecord, _LeafNodeList.Count), new NodeComparer(_Options.FrequencyFirst));
                    CombineNodeArr(result, leafNodeArray);

                    lastIndex = i;
                }

                int newRightBoundary = positionLenArr[i].Position + positionLenArr[i].Length;

                if (newRightBoundary > lastRightBoundary)
                {
                    lastRightBoundary = newRightBoundary;
                }

            }

            if (lastIndex < count)
            {
                //last is isolated point
                int c = count - lastIndex;

                PanGu.Dict.PositionLength[] arr = new PanGu.Dict.PositionLength[c];
                Array.Copy(positionLenArr, lastIndex, arr, 0, c);
                Node[] leafNodeArray = GetLeafNodeArrayCore(arr, lastRightBoundary - positionLenArr[lastIndex].Position, c);
                Framework.QuickSort<Node>.TopSort(leafNodeArray, _LeafNodeList.Count, (int)Math.Min(TopRecord, _LeafNodeList.Count), new NodeComparer(_Options.FrequencyFirst));
                CombineNodeArr(result, leafNodeArray);
            }


            return result;

        }

        /// <summary>
        /// 最底层算法，获取叶子节点集合
        /// </summary>
        /// <param name="positionLenArr"></param>
        /// <param name="orginalText"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Node[] GetLeafNodeArrayCore(PanGu.Dict.PositionLength[] positionLenArr, int orginalTextLength, int count)
        {
            _LeafNodeList.Clear();
            _PositionLengthArr = positionLenArr;
            _InputStringLength = orginalTextLength;
            _PositionLengthArrCount = count;

            BuildTree(_Root, 0);

            for (int i = _LeafNodeList.Count; i < _LeafNodeList.Items.Length; i++)
            {
                _LeafNodeList.Items[i] = null;
            }

            return _LeafNodeList.Items;

        }

        public SuperLinkedList<WordInfo> Match(PanGu.Dict.PositionLength[] positionLenArr, string orginalText, int count)
        {
            if (_Options == null)
            {
                _Options = Setting.PanGuSettings.Config.MatchOptions;
            }

            if (_Parameters == null)
            {
                _Parameters = Setting.PanGuSettings.Config.Parameters;
            }

            int[] masks = new int[orginalText.Length];
            int redundancy = _Parameters.Redundancy;

            SuperLinkedList<WordInfo> result = new SuperLinkedList<WordInfo>();

            if (count == 0)
            {
                if (_Options.UnknownWordIdentify)
                {
                    WordInfo wi = new WordInfo();
                    wi.Word = orginalText;
                    wi.Position = 0;
                    wi.WordType = WordType.None;
                    wi.Rank = 1;
                    result.AddFirst(wi);
                    return result;
                }
                else
                {
                    int position = 0;
                    foreach (char c in orginalText)
                    {
                        WordInfo wi = new WordInfo();
                        wi.Word = c.ToString();
                        wi.Position = position++;
                        wi.WordType = WordType.None;
                        wi.Rank = 1;
                        result.AddLast(wi);
                    }

                    return result;
                }
            }

            Node[] leafNodeArray = GetLeafNodeArray(positionLenArr, orginalText.Length, count);

            //下面两句是不采用孤立点分割算法的老算法
            //Node[] leafNodeArray = GetLeafNodeArrayCore(positionLenArr, orginalText.Length, count);
            //Framework.QuickSort<Node>.TopSort(leafNodeArray,
            //    _LeafNodeList.Count, (int)Math.Min(TopRecord, _LeafNodeList.Count), new NodeComparer());

            int j = 0;
            // 获取前TopRecord个单词序列
            foreach (Node node in leafNodeArray)
            {
                if (leafNodeArray[j] == null)
                {
                    break;
                }

                if (j >= TopRecord || j >= leafNodeArray.Length)
                {
                    break;
                }

                Dict.PositionLength[] comb = new PanGu.Dict.PositionLength[node.AboveCount];

                int i = node.AboveCount - 1;
                Node cur = node;

                while (i >= 0)
                {
                    comb[i] = cur.PositionLength;
                    cur = cur.Parent;
                    i--;
                }

                _AllCombinations.Add(comb);

                j++;
            }

            //Force single word
            //强制一元分词
            if (_Options.ForceSingleWord)
            {
                Dict.PositionLength[] comb = new PanGu.Dict.PositionLength[orginalText.Length];

                for (int i = 0; i < comb.Length; i++)
                {
                    PanGu.Dict.PositionLength pl = new PanGu.Dict.PositionLength(i, 1, new WordAttribute(orginalText[i].ToString(), POS.POS_UNK, 0));
                    pl.Level = 3;
                    comb[i] = pl;
                }

                _AllCombinations.Add(comb);
            }

            if (_AllCombinations.Count > 0)
            {
                ICollection<Dict.PositionLength> positionCollection = MergeAllCombinations(redundancy);

                foreach (Dict.PositionLength pl in positionCollection)
                //for (int i = 0; i < _AllCombinations[0].Length; i++)
                {
                    //result.AddLast(new WordInfo(_AllCombinations[0][i], orginalText));
                    result.AddLast(new WordInfo(pl, orginalText, _Parameters));
                    if (pl.Length > 1)
                    {
                        for (int k = pl.Position;
                            k < pl.Position + pl.Length; k++)
                        {
                            masks[k] = 2;
                        }
                    }
                    else
                    {
                        masks[pl.Position] = 1;
                    }
                }
            }

            #region 合并未登录词

            bool needRemoveSingleWord;
            List<WordInfo> unknownWords = GetUnknowWords(masks, orginalText, out needRemoveSingleWord);

            //合并到结果序列的对应位置中
            if (unknownWords.Count > 0)
            {
                SuperLinkedListNode<WordInfo> cur = result.First;

                if (needRemoveSingleWord && !_Options.ForceSingleWord)
                {
                    //Remove single word need be remvoed

                    while (cur != null)
                    {
                        if (cur.Value.Word.Length == 1)
                        {
                            if (masks[cur.Value.Position] == 11)
                            {
                                SuperLinkedListNode<WordInfo> removeItem = cur;

                                cur = cur.Next;

                                result.Remove(removeItem);

                                continue;
                            }
                        }

                        cur = cur.Next;
                    }
                }

                cur = result.First;

                j = 0;

                while (cur != null)
                {
                    if (cur.Value.Position >= unknownWords[j].Position)
                    {
                        result.AddBefore(cur, unknownWords[j]);
                        j++;
                        if (j >= unknownWords.Count)
                        {
                            break;
                        }
                    }

                    if (cur.Value.Position < unknownWords[j].Position)
                    {
                        cur = cur.Next;
                    }
                }

                while (j < unknownWords.Count)
                {
                    result.AddLast(unknownWords[j]);
                    j++;
                }
            }


            #endregion



            return result;
        }

        #endregion
    }
}
