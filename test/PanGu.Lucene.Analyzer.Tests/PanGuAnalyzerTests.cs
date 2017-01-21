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
        private PanGuAnalyzer _analyzer;

        private string[] _samples;

        public PanGuAnalyzerTests()
        {
            Console.OutputEncoding = Encoding.UTF8;

            this._indexDir = new DirectoryInfo("bin");
            this._analyzer = this.CreateAnalyzer();

            var asm = this.GetType().GetTypeInfo().Assembly;
            var resourceNames = asm.GetManifestResourceNames();
            var sampleResourceName = resourceNames.First(x => x.EndsWith("Sample.json"));

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

        private PanGuAnalyzer CreateAnalyzer()
        {
            return new PanGuAnalyzer(false, new Match.MatchOptions()
            {
                ChineseNameIdentify = true,
                FilterStopWords = true,
                FrequencyFirst = true,
                EnglishMultiDimensionality = true,
                EnglishSegment = true
            }, new Match.MatchParameter());
        }

        private void BuidIndex()
        {
            var codecs = Codec.AvailableCodecs();
            var options = new IndexWriterConfig(LVERSION.LUCENE_48, null);
            options.SetOpenMode(IndexWriterConfig.OpenMode_e.CREATE);
            //options.SetCodec(Codec.ForName(""));
            using (var iw = new IndexWriter(FSDirectory.Open(this._indexDir), options))
            {
                iw.DeleteAll();
                iw.Commit();
                iw.Flush(true, true);
                foreach (string text in this._samples)
                {
                    if (!text.StartsWith(@"//"))
                    {
                        var doc = new Document();
                        //doc.Add(new Field("body", text, Field.Store.YES, Field.Index.ANALYZED));
                        doc.Add(new TextField("body", text, Field.Store.YES));
                        iw.AddDocument(doc, this.CreateAnalyzer()); // TODO: If not create a new analyzer, result will be empty.
                        Console.WriteLine("Indexed doc: {0}", text);
                        var keywords = this.InvokeAnalyzer(text);
                        Console.WriteLine($"Keywords: {string.Join(", ", keywords)}.");
                    }
                }
                iw.Commit();
                Console.WriteLine("Building index done!");
            }
        }

        private List<string> InvokeAnalyzer(string useCase)
        {
            var list = new List<string>();
            using (var analyzer = this.CreateAnalyzer())
            {
                var input = useCase;
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
            return list;
        }

        private void TestAnalyzer(string useCase, IEnumerable<string> expectedResult)
        {
            var list = this.InvokeAnalyzer(useCase);
            Assert.True(list.Count > 0);
            Assert.All(list, x => expectedResult.Contains(x));
        }

        [Fact]
        public void PanGuAnalyzerTest()
        {
            foreach (string text in this._samples)
            {
                if (!text.StartsWith(@"//"))
                {
                    var keywords = this.InvokeAnalyzer(text);
                    Console.WriteLine($"Keywords: {string.Join(", ", keywords)}.");
                }
            }
        }

        [Fact]
        public void PanGuAnalyzerTest1()
        {
            this.TestAnalyzer("同义词输出功能一般用于对搜索字符串的分词，不建议在索引时使用", new List<string>() {
                "同义词", "输出", "功能", "一般", "用于",
                "对", "搜索", "字符串", "的", "分词",
                "不", "建议", "在", "索引", "时", "使用"
            });
        }

        [Fact]
        public void PanGuAnalyzerTest2()
        {
            // See output, the PanGu lib not reproduce the result as expected.
            this.TestAnalyzer("上海东方明珠", new List<string>() {
                "上海", "东方", "明珠",
                //"上海东方", "东方明珠",
            });
        }

        [Fact]
        public void PanGuAnalyzerTest3()
        {
            this.TestAnalyzer("【AppsFlyer：社交平台口碑营销效果最佳", new List<string>() {
                "AppsFlyer", "社交", "平台", "口碑",
                "营销", "效果", "最佳",
            });
        }

        [Fact]
        public void SearchTest()
        {
            this.BuidIndex();
            var keyword = "社交"; // why "社交" failed? see line 83.
            using (var indexer = DirectoryReader.Open(FSDirectory.Open(this._indexDir)))
            {
                var searcher = new IndexSearcher(indexer);
                var qp = new QueryParser(LVERSION.LUCENE_48, "body", new PanGuAnalyzer(true));
                var query = qp.Parse(keyword);
                Console.WriteLine(string.Format("query> {0}", query.ToString()).Trim());
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