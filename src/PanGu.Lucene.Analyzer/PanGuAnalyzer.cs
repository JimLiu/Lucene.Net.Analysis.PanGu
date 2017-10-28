using Lucene.Net.Analysis.Core;
using PanGu.Match;
using System.IO;

using LVERSION = global::Lucene.Net.Util.LuceneVersion;

namespace Lucene.Net.Analysis.PanGu
{

    public class PanGuAnalyzer
        : Analyzer
    {

        private bool _originalResult = false;
        private MatchOptions _options;
        private MatchParameter _parameters;

        public PanGuAnalyzer()
            : this(false, null, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="originalResult">
        /// Return original string.
        /// Don't use when you are doing segments.
        /// </param>
        public PanGuAnalyzer(bool originalResult)
          : this(originalResult, null, null)
        {
        }

        public PanGuAnalyzer(MatchOptions options, MatchParameter parameters)
            : this(false, options, parameters, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="originalResult">
        /// Return original string.
        /// Don't use when you are doing segments.
        /// </param>
        public PanGuAnalyzer(bool originalResult, MatchOptions options, MatchParameter parameters)
            : base()
        {
            this.Initialize(originalResult, options, parameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="originalResult">
        /// Return original string.
        /// Don't use when you are doing segments.
        /// </param>
        public PanGuAnalyzer(bool originalResult, MatchOptions options, MatchParameter parameters, ReuseStrategy reuseStrategy)
            : base(reuseStrategy)
        {
            this.Initialize(originalResult, options, parameters);
        }

        protected virtual void Initialize(bool originalResult, MatchOptions options, MatchParameter parameters)
        {
            _originalResult = originalResult;
            _options = options;
            _parameters = parameters;
        }

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var result = new PanGuTokenizer(reader, _originalResult, _options, _parameters);
            var finalStream = (TokenStream)new LowerCaseFilter(LVERSION.LUCENE_48, result);
            return new TokenStreamComponents(result, finalStream);
        }

    }


}
