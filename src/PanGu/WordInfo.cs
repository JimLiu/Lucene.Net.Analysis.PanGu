using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu
{
    public enum WordType
    {
        None = 0,
        English = 1,
        SimplifiedChinese = 2,
        TraditionalChinese = 3,
        Numeric = 4,
        Symbol = 5,
        Space = 6,
        Synonym = 7, //同义词
    }


    public class WordInfo : WordAttribute, IComparable<WordInfo>
    {
        /// <summary>
        /// Current word type
        /// </summary>
        public WordType WordType;

        /// <summary>
        /// Original word type
        /// </summary>
        public WordType OriginalWordType;

        /// <summary>
        /// Word position
        /// </summary>
        public int Position;

        /// <summary>
        /// Rank for this word
        /// 单词权重
        /// </summary>
        public int Rank;

        public WordInfo()
        {
        }

        public WordInfo(string word, int position, POS pos, double frequency, int rank, WordType wordTye, WordType originalWordType)
            : base(word, pos, frequency)
        {
            Position = position;
            WordType = wordTye;
            OriginalWordType = originalWordType;
            Rank = rank;
        }

        public WordInfo(string word, POS pos, double frequency)
            :base(word, pos, frequency)
        {
        }

        public WordInfo(WordAttribute wordAttr)
        {
            this.Word = wordAttr.Word;
            this.Pos = wordAttr.Pos;
            this.Frequency = wordAttr.Frequency;
        }

        public WordInfo(Dict.PositionLength pl, string oringinalText, Match.MatchParameter parameters)
        {
            this.Word = oringinalText.Substring(pl.Position, pl.Length);
            this.Pos = pl.WordAttr.Pos;
            this.Frequency = pl.WordAttr.Frequency;
            this.WordType = WordType.SimplifiedChinese;
            this.Position = pl.Position;

            switch (pl.Level)
            {
                case 0:
                    this.Rank = parameters.BestRank;
                    break;
                case 1:
                    this.Rank = parameters.SecRank;
                    break;
                case 2:
                    this.Rank = parameters.ThirdRank;
                    break;
                case 3:
                    this.Rank = parameters.SingleRank;
                    break;
                default:
                    this.Rank = parameters.BestRank;
                    break;
            }

        }

        public int GetEndPositon()
        {
            return this.Position + this.Word.Length;
        }

        #region IComparable<WordInfo> Members

        public int CompareTo(WordInfo other)
        {
            if (other == null)
            {
                return -1;
            }

            if (this.Position != other.Position)
            {
                return this.Position.CompareTo(other.Position);
            }

            if (other.Word == null)
            {
                return -1;
            }

            return this.Word.Length.CompareTo(other.Word.Length);
        }

        #endregion
    }
}
