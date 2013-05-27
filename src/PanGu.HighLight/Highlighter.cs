using System;
using System.Collections.Generic;
using System.Text;
using PanGu;

namespace PanGu.HighLight
{
    public class Highlighter
    {
        #region Private fields

        private int _FragmentSize = 50;
        private Formatter _Formatter;
        private PanGu.Segment _PanGuSegment;
        List<WordInfo> _Selection;
        string _Content;

        #endregion

        #region Public properties
        /// <summary>
        /// set or get fragment size.
        /// </summary>
        public int FragmentSize
        {
            get
            {
                return _FragmentSize;
            }

            set
            {
                _FragmentSize = value;
            }
        }
        #endregion

        #region Private methods
        private List<WordInfo> PickupKeywords(ICollection<WordInfo> keywordsWordInfos, ICollection<WordInfo> contentWordInfos)
        {
            List<WordInfo> ret = new List<WordInfo>();

            Dictionary<string, bool> dict = new Dictionary<string, bool>();

            foreach (WordInfo wordInfo in keywordsWordInfos)
            {
                if (wordInfo == null)
                {
                    continue;
                }

                if (!dict.ContainsKey(wordInfo.Word))
                {
                    dict.Add(wordInfo.Word, true);
                }
            }

            foreach (WordInfo wordInfo in contentWordInfos)
            {
                if (wordInfo == null)
                {
                    continue;
                }

                if (dict.ContainsKey(wordInfo.Word))
                {
                    ret.Add(wordInfo);
                }
            }

            ret.Sort();
            return ret;
        }

        private IEnumerable<WordInfo> Optimize(List<WordInfo> selection, int start, int end)
        {
            int maxRank = 0;
            WordInfo lst = null;

            for(int i = start; i < end; i++)
            {
                if (selection[i].Rank > maxRank)
                {
                    maxRank = selection[i].Rank;
                }
            }

            int rankSum = 0;
            int endPosition = selection[start].GetEndPositon();

            for (int i = start; i < end; i++)
            {
                if (endPosition < selection[i].GetEndPositon())
                {
                    endPosition = selection[i].GetEndPositon();
                }

                if (selection[i].Rank == maxRank)
                {
                    if (lst == null)
                    {
                        rankSum += (int)Math.Pow(3, selection[i].Rank);
                        //yield return selection[i];
                    }
                    else if (selection[i].Position >= lst.Position + lst.Word.Length)
                    {
                        rankSum += (int)Math.Pow(3, selection[i].Rank);
                        //yield return selection[i];
                    }

                    lst = selection[i];
                }
            }

            WordInfo word = new WordInfo();

            word.Position = selection[start].Position;
            word.Word = _Content.Substring(word.Position, endPosition - word.Position);
            word.Rank = rankSum == 0 ? (int)Math.Pow(3, maxRank) : rankSum;
            yield return word;
        }

        private List<WordInfo> Optimize(List<WordInfo> selection)
        {
            int start = 0;
            int end = 0;
            List<WordInfo> ret = new List<WordInfo>();

            while (start < selection.Count)
            {
                int endCharPos = selection[start].Position + selection[start].Word.Length;

                while (end < selection.Count)
                {
                    if (selection[end].Position >= endCharPos)
                    {
                        foreach (WordInfo wordInfo in Optimize(selection, start, end))
                        {
                            ret.Add(wordInfo);
                        }

                        start = end;
                        break;
                    }

                    endCharPos = Math.Max(endCharPos, selection[end].Position + selection[end].Word.Length);
                    end++;
                }

                if (start != end)
                {
                    //end point to the last word in the list
                    foreach (WordInfo wordInfo in Optimize(selection, start, end))
                    {
                        ret.Add(wordInfo);
                    }

                    break;
                }
            }

            return ret;
        }

        private List<Fragment> GetFragments(List<WordInfo> selection)
        {
            List<Fragment> fragments = new List<Fragment>();

            if (selection.Count == 0)
            {
                return fragments;
            }

            int start = 0;
            int end = 0;

            while (start < selection.Count)
            {
                while (end < selection.Count)
                {
                    if (selection[end].GetEndPositon() - selection[start].Position > FragmentSize)
                    {
                        fragments.Add(new Fragment(start, end, selection));
                        start = end;
                    }

                    end++;
                }

                if (start != end)
                {
                    //end point to the last word in the list
                    foreach (WordInfo wordInfo in Optimize(selection, start, end))
                    {
                        fragments.Add(new Fragment(start, end, selection));
                    }

                    break;
                }

            }

            fragments.Sort();
            return fragments;
        }

        private List<Fragment> GetFragments(string keywords, string content)
        {
            ICollection<WordInfo> keywordsWordInfos = _PanGuSegment.DoSegment(keywords);

            //Make lower
            foreach (WordInfo wordInfo in keywordsWordInfos)
            {
                if (wordInfo == null)
                {
                    continue;
                }

                if (wordInfo.Word == null)
                {
                    continue;
                }

                wordInfo.Word = wordInfo.Word.ToLower();
            }

            ICollection<WordInfo> contentWordInfos = _PanGuSegment.DoSegment(content);

            //Make lower
            foreach (WordInfo wordInfo in contentWordInfos)
            {
                if (wordInfo == null)
                {
                    continue;
                }

                if (wordInfo.Word == null)
                {
                    continue;
                }

                wordInfo.Word = wordInfo.Word.ToLower();
            }

            _Content = content;

            _Selection = PickupKeywords(keywordsWordInfos, contentWordInfos);

            _Selection = Optimize(_Selection);

            return GetFragments(_Selection);
        }

        private string FragmentToString(Fragment fragment)
        {
            StringBuilder str = new StringBuilder();

            int width = _Selection[fragment.End - 1].GetEndPositon() - _Selection[fragment.Start].Position;

            int remain = (_FragmentSize - width) / 2;

            if (remain < 0)
            {
                remain = 0;
            }

            int fst = Math.Max(0, _Selection[fragment.Start].Position - remain);

            int lst = Math.Min(_Content.Length, _Selection[fragment.End - 1].GetEndPositon() + remain);

            if (fst == 0)
            {
                lst += remain - _Selection[fragment.Start].Position;
                lst = Math.Min(_Content.Length, lst);
            }
            else if (lst == _Content.Length)
            {
                fst -= _Selection[fragment.End - 1].GetEndPositon() + remain - lst;
                fst = Math.Max(0, fst);
            }
            

            for (int i = fragment.Start; i < fragment.End; i++)
            {
                str.AppendFormat("{0}{1}", 
                    _Content.Substring(fst, _Selection[i].Position - fst),
                    _Formatter.HighlightTerm(_Content.Substring(_Selection[i].Position, _Selection[i].Word.Length)));
                fst = _Selection[i].GetEndPositon();
            }

            str.Append(_Content.Substring(_Selection[fragment.End - 1].GetEndPositon(), 
                lst - _Selection[fragment.End - 1].GetEndPositon()));

            return str.ToString();
        }

        #endregion

        public Highlighter(Formatter formatter, PanGu.Segment segment)
        {
            _Formatter = formatter;
            _PanGuSegment = segment;
        }

        /// <summary>
        /// Get best fragment
        /// </summary>
        /// <param name="keywords">keywords</param>
        /// <param name="content">content</param>
        /// <returns></returns>
        public string GetBestFragment(string keywords, string content)
        {
            List<Fragment> fragments = GetFragments(keywords, content);

            if (fragments.Count > 0)
            {
                return FragmentToString(fragments[0]);
            }

            return "";
        }

        /// <summary>
        /// Get the fragments 
        /// </summary>
        /// <param name="keywords">keywords</param>
        /// <param name="content">content</param>
        /// <param name="maxFragments">max fragments will be outputed</param>
        /// <returns></returns>
        public IEnumerable<string> GetFragments(string keywords, string content, int maxFragments)
        {
            List<Fragment> fragments = GetFragments(keywords, content);
            int index = 0;

            foreach(Fragment fragment in fragments)
            {
                if (index >= maxFragments)
                {
                    break;
                }

                index++;
                yield return FragmentToString(fragment);
            }
        }
    }
}
