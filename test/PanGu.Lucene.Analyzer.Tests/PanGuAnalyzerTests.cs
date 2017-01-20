using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Diagnostics;
using System.IO;
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
            this._indexDir = new DirectoryInfo("bin");
            this._analyzer = new PanGuAnalyzer();
            this._samples = File.ReadAllLines(@"Resources/Sample.txt");
            this.BuidIndex();
        }

        private void BuidIndex()
        {
            using (var iw = new IndexWriter(FSDirectory.Open(this._indexDir),
               new IndexWriterConfig(LVERSION.LUCENE_48, this._analyzer)))
            {
                foreach (string text in this._samples)
                {
                    var doc = new Document();
                    doc.Add(new StringField("body", text, Field.Store.YES));
                    iw.AddDocument(doc);
                    Debug.WriteLine("Indexed doc: {0}", text);
                }
                iw.Commit();
                Debug.WriteLine("Building index done!\r\n\r\n");
            }
        }

        [Fact]
        public void SearchTest()
        {
            var keyword = "社交";
            using (var indexer = DirectoryReader.Open(FSDirectory.Open(this._indexDir)))
            {
                var searcher = new IndexSearcher(indexer);
                var qp = new QueryParser(LVERSION.LUCENE_48, "body", this._analyzer);
                var query = qp.Parse(keyword);
                Debug.WriteLine("query> {0}", query);
                var tds = searcher.Search(query, 10);
                Debug.WriteLine("TotalHits: " + tds.TotalHits);
                foreach (var sd in tds.ScoreDocs)
                {
                    Debug.WriteLine(sd.Score);
                    var doc = searcher.Doc(sd.Doc);
                    var body = doc.Get("body");
                    Debug.WriteLine(body);
                    Assert.False(string.IsNullOrWhiteSpace(body));
                }
            }
        }

    }

}