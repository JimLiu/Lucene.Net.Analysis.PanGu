using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PanGu.Match
{
    [Serializable]
    public class MatchParameter
    {
 
        private int _Redundancy;

        /// <summary>
        /// 多元分词冗余度
        /// </summary>
        public int Redundancy
        {
            get
            {
                return _Redundancy;
            }

            set
            {
                if (value < 0)
                {
                    _Redundancy = 0;
                }
                else if (value >= 3)
                {
                    _Redundancy = 2;
                }
                else
                {
                    _Redundancy = value;
                }
            }
        }

        /// <summary>
        /// 未登录词权值
        /// </summary>
        public int UnknowRank = 1;

        /// <summary>
        /// 最匹配词权值
        /// </summary>
        public int BestRank = 5;

        /// <summary>
        /// 次匹配词权值
        /// </summary>
        public int SecRank = 3;

        /// <summary>
        /// 再次匹配词权值
        /// </summary>
        public int ThirdRank = 2;

        /// <summary>
        /// 强行输出的单字的权值
        /// </summary>
        public int SingleRank = 1;

        /// <summary>
        /// 数字的权值
        /// </summary>
        public int NumericRank = 1;

        /// <summary>
        /// 英文词汇权值
        /// </summary>
        public int EnglishRank = 5;

        /// <summary>
        /// 英文词汇小写的权值
        /// </summary>
        public int EnglishLowerRank = 3;

        /// <summary>
        /// 英文词汇词根的权值
        /// </summary>
        public int EnglishStemRank = 2;


        /// <summary>
        /// 符号的权值
        /// </summary>
        public int SymbolRank = 1;

        /// <summary>
        /// 强制同时输出简繁汉字时，非原来文本的汉字输出权值。
        /// 比如原来文本是简体，这里就是输出的繁体字的权值，反之亦然。
        /// </summary>
        public int SimplifiedTraditionalRank = 1;

        /// <summary>
        /// 同义词权值
        /// </summary>
        public int SynonymRank = 1;

        /// <summary>
        /// 通配符匹配结果的权值
        /// </summary>
        public int WildcardRank = 1;

        /// <summary>
        /// 过滤英文选项生效时，过滤大于这个长度的英文。
        /// </summary>
        public int FilterEnglishLength = 0;

        /// <summary>
        /// 过滤数字选项生效时，过滤大于这个长度的数字。
        /// </summary>
        public int FilterNumericLength = 0;

        /// <summary>
        /// 用户自定义规则的配件文件名
        /// </summary>
        public string CustomRuleAssemblyFileName = "";

        /// <summary>
        /// 用户自定义规则的类的完整名，即带名字空间的名称
        /// </summary>
        public string CustomRuleFullClassName = "";

        public MatchParameter Clone()
        {
            MatchParameter result = new MatchParameter();

            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                object value = fi.GetValue(this);
                fi.SetValue(result, value);
            }

            return result;
        }
    }
}
