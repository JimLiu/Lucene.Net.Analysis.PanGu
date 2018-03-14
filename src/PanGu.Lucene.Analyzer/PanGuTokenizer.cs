using Lucene.Net.Analysis.TokenAttributes;
using PanGu;
using PanGu.Match;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;

namespace Lucene.Net.Analysis.PanGu
{

    public class PanGuTokenizer
        : Tokenizer
    {

        private static object _LockObj = new object();
        private static bool _Inited = false;

        private readonly MatchOptions _options;
        private readonly MatchParameter _parameters;

        private WordInfo[] _WordList;
        private int _Position = -1; //词汇在缓冲中的位置.
        private bool _OriginalResult = false;
        private string _InputText;

        // this tokenizer generates three attributes:
        // offset, positionIncrement and type
        private ICharTermAttribute termAtt;
        private IOffsetAttribute offsetAtt;
        private IPositionIncrementAttribute posIncrAtt;
        private ITypeAttribute typeAtt;

        /// <summary>
        /// Initialize PanGu Segment
        /// </summary>
        /// <param name="fileName">PanGu.xml file path</param>
        public static void InitPanGuSegment(string fileName = null)
        {
            lock (_LockObj)
            {
                if (!_Inited)
                {
                    Segment.Init( );
                    _Inited = true;
                }
            }
        }

        private void Init()
        {
            InitPanGuSegment();
            termAtt = AddAttribute<ICharTermAttribute>();
            offsetAtt = AddAttribute<IOffsetAttribute>();
            posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            typeAtt = AddAttribute<ITypeAttribute>();
        }

        public PanGuTokenizer(TextReader input, bool originalResult)
            : this(input, originalResult, null, null)
        {
        }

        public PanGuTokenizer(TextReader input, bool originalResult, MatchOptions options, MatchParameter parameters)
            : this(input, options, parameters)
        {
            _OriginalResult = originalResult;
        }

        public PanGuTokenizer(TextReader input, MatchOptions options, MatchParameter parameters)
            : this(AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, input, options, parameters)
        {
        }

        public PanGuTokenizer(AttributeFactory factory, TextReader input, MatchOptions options, MatchParameter parameters)
            : base(factory, input)
        {
            lock (_LockObj)
            {
                Init();
            }
            this._options = options;
            this._parameters = parameters;
        }

        public sealed override bool IncrementToken()
        {
            ClearAttributes();
            Token word = Next();
            if (word != null)
            {
                var buffer = word.ToString();
                termAtt.SetEmpty().Append(buffer);
                offsetAtt.SetOffset(word.StartOffset, word.EndOffset);
                typeAtt.Type = word.Type;
                return true;
            }
            End();
            this.Dispose();
            return false;
        }

        //DotLucene的分词器简单来说，就是实现Tokenizer的Next方法，把分解出来的每一个词构造为一个Token，因为Token是DotLucene分词的基本单位。
        public Token Next()
        {
            if (_OriginalResult)
            {
                string retStr = _InputText;

                _InputText = null;

                if (retStr == null)
                {
                    return null;
                }

                return new Token(retStr, 0, retStr.Length);
            }

            int length = 0;    //词汇的长度.
            int start = 0;     //开始偏移量.

            while (true)
            {
                _Position++;
                if (_Position < _WordList.Length)
                {
                    if (_WordList[_Position] != null)
                    {
                        length = _WordList[_Position].Word.Length;
                        start = _WordList[_Position].Position;
                        return new Token(_WordList[_Position].Word, start, start + length);
                    }
                }
                else
                {
                    break;
                }
            }

            _InputText = null;
            return null;
        }

        public override void Reset()
        {
            base.Reset();

            this._InputText = this.ReadToEnd(base.m_input);
            this._WordList = this.DoSegement(this._InputText);
            this._Position = -1;
        }

        public ICollection<WordInfo> SegmentToWordInfos(String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new LinkedList<WordInfo>();
            }
            var segment = new Segment();
            return segment.DoSegment(str);
        }

        private string ReadToEnd(TextReader input)
        {
            return input.ReadToEnd();

            //char[] readBuf = new char[1024];
            //int relCount = base.input.Read(readBuf, 0, readBuf.Length);
            //var inputStr = new StringBuilder(readBuf.Length);

            //while (relCount > 0)
            //{
            //    inputStr.Append(readBuf, 0, relCount);
            //    relCount = input.Read(readBuf, 0, readBuf.Length);
            //}

            //if (inputStr.Length > 0)
            //{
            //    return inputStr.ToString();
            //}

            //return null;
        }

        private WordInfo[] DoSegement(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new WordInfo[0];
            }
            else
            {
                var segment = new Segment();
                var wordInfos = segment.DoSegment(input, this._options, this._parameters);
                return wordInfos.ToArray();
            }
        }

    }

}