using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Analysis.PanGu;

namespace PanGuLucene.Demo
{
    class Program
    {
        static DirectoryInfo INDEX_DIR = new DirectoryInfo("index");
        static Analyzer analyzer = new PanGuAnalyzer(); //MMSegAnalyzer //StandardAnalyzer

        static void Main(string[] args)
        {
            string[] texts = new string[] { 
                "京华时报1月23日报道 昨天，受一股来自中西伯利亚的强冷空气影响，本市出现大风降温天气，白天最高气温只有零下7摄氏度，同时伴有6到7级的偏北风。",
                "【AppsFlyer：社交平台口碑营销效果最佳http://t.cn/zTHEQRM】社交网络分享应用的方式，在新应用获取用户非常有效率。搜索方式可为移动应用带来最高玩家质量，但玩家量和转化率较低。广告可带来最大用户量，但用户质量却相对不高，转化率也不够高。刺激性流量的转化率最高，但是平均玩家质量是最低",
                "Server Core省去了Windows Server的图形界面，改为命令行的方式来管理服务器。它不仅拥有更精简的体积与更优化的性能，还可缩短50%-60%的系统更新时间。现在，SQL Server已经支持Windows Server Core，计划内停机时间的大幅缩减让企业关键数据库应用获得更高的可用性。",
                "【各移动游戏分发渠道的优势和分成比例】联通沃商店：线下资源和话费支付能力（分成比例3:7），触控：技术和运营能力（分成比例5:5），91无线：评测、运营、数据等服务能力（分成比例4:6），UC：用户接入点、WebApp的支持（分成比例5:5），斯凯网络：硬件厂商资源（分成比例3:7）http://t.cn/zTHnwJk",
                "iPod之父创办的Nest收购家居能源数据监测服务MyEnergy，将从小小恒温器进展为家居节能整套方案 |Nest公司要做的并不只是一个小小温控器，而是提供智能家居节能整套方案。而MyEnergy积累的数据能对Nest起到很大帮助，并且也为Nest带来更多能源服务商方面的联系： http://t.cn/zTHs8qQ",
                "今日，58同城将正式与支付宝达成战略合作。这既是支付宝首次为阿里系外的企业提供担保支付服务，也是58同城推动消费者保障服务在支付和结算方面迈出的重要一步，此番合作将对整个行业产生颠覆性的影响。58要做的就是不断的了解用户痛点，不断的与虚假信息斗争，建立一个人人信赖的生活服务平台。",
                "【iPhone如何征服日本】虽然日本身为现代移动技术的摇篮，智能手机和触屏设备的普及也领先于其他地区，但iPhone仍然顺利地征服这个岛国，成为该国最畅销的手机。一方面得益于女性用户的追捧，但更多地，还是源自日本移动行业的内在问题。http://t.cn/zTHENrI",
                "【东方体育中心游泳馆今起重新开放，成人票20元/场】#爱体育#“立夏”过了，夏天近了，喜欢游泳的筒子心痒难耐了吧！@965365上海体育发布 说，经过一个多月的内装修，东方体育中心室内游泳馆今天起重新对外开放，开放时间为13:00-21:00，票价详情点大图了解~今夏挥洒汗水，“玉兰桥”走起！",
                "【深圳地铁免费伞 一年借出2000把归还70把】深圳地铁站摆放了“红雨伞”，下雨时可免费借给乘客使用。但一年来，地铁借给市民2000多把雨伞，只还回来70把，有的站甚至已经没有雨伞了。工作人员尝试联系部分借伞人，发现登记电话号码常常显示是空号……地铁站的红雨伞，你借了会还吗？（南方都市报）",
                "【银行的速度，移动互联网的速度】招商银行信用卡副总经理彭千在GMIC上分享招商银行移动互联网尝试案例：先后和开心和人人推出联名信用卡，但银行动作太慢了，推出是开心网已经不开心了，人人网已经没有人了！",
                "【黑石超级公关】4月21日的新闻联播上，黑石集团主席施瓦茨曼向清华大学捐赠1亿美元，并牵头筹集2亿美元，投资3亿美元与清华大学合作筹建“苏世民书院”的新闻被列为头条。很明显“未来中国不再是选修课，而是必修课。”1亿美元投资清华，背后是扭转坑中投形象的战略公关…",
                "【传谷歌将效仿苹果开设谷歌眼镜零售店】科技博客Business Insider今天援引消息人士说法称，谷歌正计划开设零售店，销售谷歌眼镜。谷歌门店或将专为眼镜产品服务，即只展示各类品牌、型号的“谷歌眼镜”产品。早前的消息指出，谷歌拟效仿苹果和微软，计划推出自主品牌的零售门店，以展示旗下各类产品。",
                "【武汉一高中禁止学生校内用手机 现场砸毁】近期在武昌东亭二路，一所学校收缴并砸毁学生手机24部，其中包括iPhone5等较昂贵的智能手机，也有价值数百元的普通手机，并设“手机尸体展示台”展出近期砸毁的部分手机残骸，均已经无法使用。",
                "【小偷慌不择路当街撒钱 警民携手完璧归赵】日前，一男子来到青浦一小作坊佯装购买商品，后借机溜进卧室行窃。老板娘在周围群众的帮助下将男子扭获，男子见势不妙，掏出一沓钞票当街抛撒。民警到达现场后，将男子抛撒的钱一一清点，共计6600元。警察蜀黍真心想为当天帮忙捡钱的群众竖起大拇指！",
                "#瓜大活动预告#【风起云涌南山下，群雄逐鹿辩工大】经过层层奋战，软件与微电子学院和理学院最终杀入了决赛，巅峰对决，即将展开。智慧的火花，头脑的竞技，唇舌的交战，精彩，一触即发。5月13日，周一晚七点，翱翔学生中心，我们与你不见不散！via人人网@西北工业大学_学生会",
                "#GMIC#尚伦律师事务所合伙人张明若律师：在中国，发生了很多起创业者因为法律意识淡薄，在天使投融资期间甚至没有签订法律文件的创业悲剧。这份文件的目的是帮助暂时还请不起律师的创业者。这份法律文件模板简单、对买卖双方公平、且免费！",
                "【金蝶随手记创始人谷风：先自我否定，再创新！】当创业者们把目光聚焦在娱乐、社交、手游、电商这些热门品类时，相信没有多少人会料到记账这一细分领域里也有产品能做到6000万级别的用户规模，堪称“屌丝逆袭”。http://t.cn/zTQvB16",
                "【陕西回应省纪委人员开车打架致死案：车辆是私家车 车主是纪委临时工】乾县青仁村发生斗殴，一死两伤，嫌犯开的显示单位为陕西省纪委的轿车引起质疑。陕西公安厅称，陕VHA088克莱斯勒轿车系嫌犯付某借用朋友的私家车。乾县公安局此前通报，陕VHA088车主是陕西省纪委临时工范小勇http://t.cn/zTQP5kC",
                "【经典干货！25个强大的PS炫光特效教程】这些经典的特效教程是很多教PS老师们的课堂案例，更被很多出版物摘录其中。那么今天毫无保留的全盘托出，同学们一定要好好练习。完成的同学也可以到优设群交作业哟，给大家分享你的设计过程和经验心得：）@尼拉贡戈小麦穗 →http://t.cn/zTHdOCK",
                "【树莓派的三个另类“武装”玩法】树莓派（Raspberry Pi）一直以来以极低的价格和“信用卡大小”的尺寸为人追捧。对于爱折腾的发烧友来说，永远可以在常人意想不到的地方发挥出自己折腾的功力。当一般人仅仅研究其编程玩法时，另一帮人已经琢磨着要把树莓派“武装”成另一个样子。http://t.cn/zTHFxIS",
                "【媒体札记：白宫信访办】@徐达内：19年前铊中毒的清华女生朱令的同情者，找到了“白宫请愿”这个易于操作又声势浩大的方法，要求美国将朱当年的室友孙维驱逐出境。随着意见领袖和各大媒体的加入，这一“跨国抗议” 的景观搅动了对官方公信力，冤假错案判断标准的全民讨论。http://t.cn/zTHsLIC",
                "【传第七大道页游海外月流水近1亿元http://t.cn/zTQPnnv】根据消息人士的透露，第七大道目前旗下网页游戏海外月流水收入已达近1亿元人民币，实质已是国内游戏公司海外收入第一，已超过大家所熟知的端游上市公司。孟治昀如是表示：“谁能告诉我，中国网游企业出口收入哪家公司高于第七大道？”",
                "【简介：他废掉了一切不服者】弗格森执教曼联26年，夺得13个英超冠军，4个联赛杯冠军，5个足总杯冠军，2个欧冠冠军，1个世俱杯冠军，1个优胜者杯冠军，1个欧洲超级杯。如果非要用一句话来总结他的伟大，小编个人的总结是：他废掉了一切敢于“不服者”，包括小贝同学",
                "这个世界干啥最赚钱？历史证明，持续保持对一个国家进行专制统治，通过无节制的赋税和滥发货币来掠夺全体国民的私人财富是唯一的标准答案。历史在进步，这种商业模式也在改头换面，于是，党专制替代家族专制，集体世袭权利代替个体世袭权力。既然改头换面，理论体系也得改变，这个理论体系就是特色论。",
                "【拥有“全球最美海滩”的塞舌尔将对中国游客免签！】#便民提示#准备出国白相的筒子冒个泡吧~你们有福啦。拥有“全球最美丽的海滩”和“最洁净的海水”美誉的塞舌尔，将可凭我国有效护照免签入境，最多停留30天这里还是英国威廉王子和王妃的蜜月地~~所以，别再只盯着马尔代夫一处啦",
                "【用数据告诉你手游有多热】今天，作为本届GMIC 的一部分，GGS全球移动游戏峰会召开。嘉宾和游戏开发者们探讨了移动游戏的现状与发展趋势。手游则是最为重要的一大关键词。盛大游戏总裁钱东海分享了日本最大手游公司CEO预测的数据：2015年全球游戏产业的格局中80%都是手机游戏。http://t.cn/zTHdkFY"
            };

            IndexWriter iw = new IndexWriter(FSDirectory.Open(INDEX_DIR), analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            int i = 0;
            foreach (string text in texts)
            {
                Document doc = new Document();
                doc.Add(new Field("body", text, Field.Store.YES, Field.Index.ANALYZED));
                iw.AddDocument(doc);
                Console.WriteLine("Indexed doc: {0}", text);
            }
            iw.Commit();
            iw.Optimize();
            iw.Dispose();

            //Analyzer a = new PanGuAnalyzer();
            //string s = "上海东方明珠";
            //System.IO.StringReader reader = new System.IO.StringReader(s);
            //Lucene.Net.Analysis.TokenStream ts = a.TokenStream(s, reader);
            //bool hasnext = ts.IncrementToken();
            //Lucene.Net.Analysis.Tokenattributes.ITermAttribute ita;
            //while (hasnext)
            //{
            //    ita = ts.GetAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();
            //    Console.WriteLine(ita.Term);
            //    hasnext = ts.IncrementToken();
            //}
            //ts.CloneAttributes();
            //reader.Close();
            //a.Close();
            //Console.ReadKey();



            Console.WriteLine();

            Console.WriteLine("Building index done!\r\n\r\n");

            while (true)
            {
                Console.Write("Enter the keyword: ");
                string keyword = Console.ReadLine();
                Search(keyword);
                Console.WriteLine();
            }

        }


        static void Search(string keyword)
        {
            IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(INDEX_DIR), true);
            QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "body", analyzer);
            Query query = qp.Parse(keyword); //2008年底  
            Console.WriteLine("query> {0}", query);


            TopDocs tds = searcher.Search(query, 10);
            Console.WriteLine("TotalHits: " + tds.TotalHits);
            foreach (ScoreDoc sd in tds.ScoreDocs)
            {
                Console.WriteLine(sd.Score);
                Document doc = searcher.Doc(sd.Doc);
                Console.WriteLine(doc.Get("body"));
            }

            searcher.Dispose();
        }
    }
}
