using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
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
            this._analyzer = new PanGuAnalyzer(true);

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

            this.BuidIndex();
        }

        private void BuidIndex()
        {
            using (var iw = new IndexWriter(FSDirectory.Open(this._indexDir),
               new IndexWriterConfig(LVERSION.LUCENE_48, this._analyzer)))
            {
                iw.DeleteAll();
                iw.Commit();
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
        public void SearchTest()
        {
            var keyword = "社交";
            using (var indexer = DirectoryReader.Open(FSDirectory.Open(this._indexDir)))
            {
                var searcher = new IndexSearcher(indexer);
                var qp = new QueryParser(LVERSION.LUCENE_CURRENT, "body", this._analyzer);
                var query = qp.Parse(keyword);
                Console.WriteLine("query> {0}", query.ToString());
                var tds = searcher.Search(query, 10);
                Console.WriteLine("TotalHits: " + tds.TotalHits);
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