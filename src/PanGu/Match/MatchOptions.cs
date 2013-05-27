using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace PanGu.Match
{
    [Serializable]
    public class MatchOptions
    {
        /// <summary>
        /// 中文人名识别
        /// </summary>
        public bool ChineseNameIdentify = false;

        /// <summary>
        /// 词频优先
        /// </summary>
        public bool FrequencyFirst = false;

        /// <summary>
        /// 多元分词
        /// </summary>
        public bool MultiDimensionality = true;

        /// <summary>
        /// 英文多元分词，这个开关，会将英文中的字母和数字分开。
        /// </summary>
        public bool EnglishMultiDimensionality = false;

        /// <summary>
        /// 过滤停用词
        /// </summary>
        public bool FilterStopWords = true;

        /// <summary>
        /// 忽略空格、回车、Tab
        /// </summary>
        public bool IgnoreSpace = true;

        /// <summary>
        /// 强制一元分词
        /// </summary>
        public bool ForceSingleWord = false;

        /// <summary>
        /// 繁体中文开关
        /// </summary>
        public bool TraditionalChineseEnabled = false;

        /// <summary>
        /// 同时输出简体和繁体
        /// </summary>
        public bool OutputSimplifiedTraditional = false;

        /// <summary>
        /// 未登录词识别
        /// </summary>
        public bool UnknownWordIdentify = true;

        /// <summary>
        /// 过滤英文，这个选项只有在过滤停用词选项生效时才有效
        /// </summary>
        public bool FilterEnglish = false;

        /// <summary>
        /// 过滤数字，这个选项只有在过滤停用词选项生效时才有效
        /// </summary>
        public bool FilterNumeric = false;


        /// <summary>
        /// 忽略英文大小写
        /// </summary>
        public bool IgnoreCapital = false;

        /// <summary>
        /// 英文分词
        /// </summary>
        public bool EnglishSegment = false;

        /// <summary>
        /// 同义词输出
        /// </summary>
        /// <remarks>
        /// 同义词输出功能一般用于对搜索字符串的分词，不建议在索引时使用
        /// </remarks>
        public bool SynonymOutput = false;

        /// <summary>
        /// 通配符匹配输出
        /// </summary>
        /// <remarks>
        /// 同义词输出功能一般用于对搜索字符串的分词，不建议在索引时使用
        /// </remarks>
        public bool WildcardOutput = false;

        /// <summary>
        /// 对通配符匹配的结果分词
        /// </summary>
        public bool WildcardSegment = false;

        /// <summary>
        /// 是否进行用户自定义规则匹配
        /// </summary>
        public bool CustomRule = false;

        public MatchOptions Clone()
        {
            MatchOptions result = new MatchOptions();

            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                object value = fi.GetValue(this);
                fi.SetValue(result, value);
            }

            return result;
        }
    }
}
