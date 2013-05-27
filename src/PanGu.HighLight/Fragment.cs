using System;
using System.Collections.Generic;
using System.Text;
using PanGu;

namespace PanGu.HighLight
{
    class Fragment : IComparable<Fragment>
    {
        int _Start;
        int _End;
        int _Rank;

        /// <summary>
        /// Get or set the start position in wordinfo list
        /// </summary>
        public int Start
        {
            get
            {
                return _Start;
            }

            set
            {
                _Start = value;
            }
        }

        /// <summary>
        /// Get or set the end position in wordinfo list
        /// </summary>
        public int End
        {
            get
            {
                return _End;
            }

            set
            {
                _End = value;
            }
        }

        /// <summary>
        /// Get or set the rank of this fragment
        /// </summary>
        public int Rank
        {
            get
            {
                return _Rank;
            }

            set
            {
                _Rank = value;
            }
        }

        public Fragment(int start, int end, List<WordInfo> wordInfos)
        {
            _Start = start;
            _End = end;
            _Rank = 0;

            for (int i = start; i < end; i++)
            {
                //_Rank += (int)Math.Pow(3, wordInfos[i].Rank);
                _Rank += wordInfos[i].Rank;
            }
        }

        #region IComparable<Fragment> Members

        public int CompareTo(Fragment other)
        {
            return other.Rank.CompareTo(this.Rank);
        }

        #endregion
    }
}
