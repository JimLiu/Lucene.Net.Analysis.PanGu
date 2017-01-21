using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Codecs;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

using LVERSION = global::Lucene.Net.Util.LuceneVersion;

namespace PanGu.Lucene.Analyzer.Tests
{

    public class PanGuAnalyzerTests
    {

        private DirectoryInfo _indexDir;
        private global::Lucene.Net.Analysis.Analyzer _analyzer;

        private string[] _samples;

        public PanGuAnalyzerTests()
        {
            Console.OutputEncoding = Encoding.UTF8;

            this._indexDir = new DirectoryInfo("bin");
            this._analyzer = new PanGuAnalyzer(false);

            var asm = this.GetType().GetTypeInfo().Assembly;
            var resourceNames = asm.GetManifestResourceNames();
            var sampleResourceName = resourceNames.First(x => x.EndsWith("Sample.txt"));

            using (var stream = asm.GetManifestResourceStream(sampleResourceName))
            {
                using (TextReader tr = new StreamReader(stream))
                {
                    var allText = tr.ReadToEnd();
                    this._samples = allText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            // Optional
            //PanGuTokenizer.InitPanGuSegment();
        }

        private void BuidIndex()
        {
            var codecs = Codec.AvailableCodecs();
            var options = new IndexWriterConfig(LVERSION.LUCENE_48, this._analyzer);
            options.SetOpenMode(IndexWriterConfig.OpenMode_e.CREATE);
            //options.SetCodec(Codec.ForName(""));
            using (var iw = new IndexWriter(FSDirectory.Open(this._indexDir), options))
            {
                iw.DeleteAll();
                iw.Commit();
                iw.Flush(true, true);
                foreach (string text in this._samples)
                {
                    var doc = new Document();
                    doc.Add(new Field("body", text, Field.Store.YES, Field.Index.ANALYZED));
                    iw.AddDocument(doc);
                    Console.WriteLine("Indexed doc: {0}", text);
                }
                iw.Commit();
                Console.WriteLine("Building index done!");
            }
        }

        [Fact]
        public void PanGuAnalyzerTest()
        {
            var list = new List<string>();
            var expected = new List<string>() { "上海", "东方", "明珠", "上海东方", "东方明珠" };
            using (var analyzer = new PanGuAnalyzer())
            {
                var input = "上海东方明珠";
                using (var reader = new StringReader(input))
                {
                    var ts = analyzer.TokenStream(input, reader);
                    ts.Reset();
                    var hasNext = ts.IncrementToken();
                    while (hasNext)
                    {
                        var ita = ts.GetAttribute<ICharTermAttribute>();
                        var term = ita.ToString();
                        Console.WriteLine(term);
                        list.Add(term);
                        hasNext = ts.IncrementToken();
                    }
                    ts.CloneAttributes();
                }
            }
            Assert.True(list.Count > 0);
            Assert.All(list, x => expected.Contains(x));
        }

        [Fact]
        public void SearchTest()
        {
            this.BuidIndex();
            var keyword = "社交";
            using (var indexer = DirectoryReader.Open(FSDirectory.Open(this._indexDir)))
            {
                var searcher = new IndexSearcher(indexer);
                var qp = new QueryParser(LVERSION.LUCENE_48, "body", new PanGuAnalyzer(true));
                var query = qp.Parse(keyword);
                Console.WriteLine("query> {0}", query.ToString());
                var tds = searcher.Search(query, 10);
                Console.WriteLine("TotalHits: " + tds.TotalHits);
                Assert.True(tds.TotalHits > 0);
                foreach (var sd in tds.ScoreDocs)
                {
                    Console.WriteLine(sd.Score);
                    var doc = searcher.Doc(sd.Doc);
                    var body = doc.Get("body");
                    Console.WriteLine(body);
                    Assert.False(string.IsNullOrWhiteSpace(body));
                }
            }
        }

    }

}