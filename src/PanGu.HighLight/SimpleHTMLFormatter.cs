using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.HighLight
{
    public class SimpleHTMLFormatter : Formatter   
    {
        string _PreTag = "<font color=\"red\">";
        string _PostTag = "</font>";

        #region Public properties

        /// <summary>
        /// Get or set prefix gag.
        /// Default: "<font color=\"red\">"
        /// </summary>
        public string PreTag
        {
            get
            {
                return _PreTag;
            }

            set
            {
                _PreTag = value;
            }
        }

        /// <summary>
        /// Get or set postfix tag.
        /// Default:"</font>"
        /// </summary>
        public string PostTag
        {
            get
            {
                return _PostTag;
            }

            set
            {
                _PostTag = value;
            }
        }

        #endregion

        public SimpleHTMLFormatter()
        {
           
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preTag">prefix tag</param>
        /// <param name="postTag">postfix tag</param>
        public SimpleHTMLFormatter(string preTag, string postTag)
        {
            _PreTag = preTag;
            _PostTag = postTag;
        }

        #region Formatter Members

        public string HighlightTerm(string originalText)
        {
            return PreTag + originalText + PostTag;
        }

        #endregion
    }
}
