/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using PanGu;
using PanGu.Dict;

namespace Demo
{
    public partial class FormDemo : Form
    {
        String _InitSource = "盘古分词 简介: 盘古分词 是由eaglet 开发的一款基于字典的中英文分词组件\r\n" +
            "主要功能: 中英文分词，未登录词识别,多元歧义自动识别,全角字符识别能力\r\n" +
            "主要性能指标:\r\n" +
            "分词准确度:90%以上\r\n" +
            "处理速度: 300-600KBytes/s Core Duo 1.8GHz\r\n" +
            "用于测试的句子:\r\n" +
            "长春市长春节致词\r\n" +
            "长春市长春药店\r\n" +
            "IＢM的技术和服务都不错\r\n" +
            "张三在一月份工作会议上说的确实在理\r\n" +
            "于北京时间5月10日举行运动会\r\n" +
            "我的和服务必在明天做好";

        private PanGu.Match.MatchOptions _Options;
        private PanGu.Match.MatchParameter _Parameters;

        public FormDemo()
        {
            InitializeComponent();
        }

        private void FormDemo_Load(object sender, EventArgs e)
        {
            textBoxSource.Text = _InitSource;
            PanGu.Segment.Init();

            PanGu.Match.MatchOptions options = PanGu.Setting.PanGuSettings.Config.MatchOptions;
            checkBoxFreqFirst.Checked = options.FrequencyFirst;
            checkBoxFilterStopWords.Checked = options.FilterStopWords;
            checkBoxMatchName.Checked = options.ChineseNameIdentify;
            checkBoxMultiSelect.Checked = options.MultiDimensionality;
            checkBoxEnglishMultiSelect.Checked = options.EnglishMultiDimensionality;
            checkBoxForceSingleWord.Checked = options.ForceSingleWord;
            checkBoxTraditionalChs.Checked = options.TraditionalChineseEnabled;
            checkBoxST.Checked = options.OutputSimplifiedTraditional;
            checkBoxUnknownWord.Checked = options.UnknownWordIdentify;
            checkBoxFilterEnglish.Checked = options.FilterEnglish;
            checkBoxFilterNumeric.Checked = options.FilterNumeric;
            checkBoxIgnoreCapital.Checked = options.IgnoreCapital;
            checkBoxEnglishSegment.Checked = options.EnglishSegment;
            checkBoxSynonymOutput.Checked = options.SynonymOutput;
            checkBoxWildcard.Checked = options.WildcardOutput;
            checkBoxWildcardSegment.Checked = options.WildcardSegment;
            checkBoxCustomRule.Checked = options.CustomRule;

            if (checkBoxMultiSelect.Checked)
            {
                checkBoxDisplayPosition.Checked = true;
            }

            PanGu.Match.MatchParameter parameters = PanGu.Setting.PanGuSettings.Config.Parameters;

            numericUpDownRedundancy.Value = parameters.Redundancy;
            numericUpDownFilterEnglishLength.Value = parameters.FilterEnglishLength;
            numericUpDownFilterNumericLength.Value = parameters.FilterNumericLength;

            //str = Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);

        }

        private void DisplaySegment()
        {
            DisplaySegment(false);
        }

        private void DisplaySegment(bool showPosition)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Segment segment = new Segment();
            ICollection<WordInfo> words = segment.DoSegment(textBoxSource.Text, _Options, _Parameters);

            watch.Stop();

            labelSrcLength.Text = textBoxSource.Text.Length.ToString();

            labelSegTime.Text = watch.Elapsed.ToString();
            if (watch.ElapsedMilliseconds == 0)
            {
                labelRegRate.Text = "无穷大";
            }
            else
            {
                labelRegRate.Text = ((double)(textBoxSource.Text.Length / watch.ElapsedMilliseconds) * 1000).ToString();
            }


            if (checkBoxShowTimeOnly.Checked)
            {
                return;
            }

            StringBuilder wordsString = new StringBuilder();
            foreach (WordInfo wordInfo in words)
            {
                if (wordInfo == null)
                {
                    continue;
                }

                if (showPosition)
                {

                    wordsString.AppendFormat("{0}({1},{2})/", wordInfo.Word, wordInfo.Position, wordInfo.Rank);
                    //if (_Options.MultiDimensionality)
                    //{
                    //}
                    //else
                    //{
                    //    wordsString.AppendFormat("{0}({1})/", wordInfo.Word, wordInfo.Position);
                    //}
                }
                else
                {
                    wordsString.AppendFormat("{0}/", wordInfo.Word);
                }
            }

            textBoxSegwords.Text = wordsString.ToString();


        }

        private void DisplaySegmentAndPostion()
        {
            DisplaySegment(true);
        }

        private void UpdateSettings()
        {
            _Options.FrequencyFirst = checkBoxFreqFirst.Checked;
            _Options.FilterStopWords = checkBoxFilterStopWords.Checked;
            _Options.ChineseNameIdentify = checkBoxMatchName.Checked;
            _Options.MultiDimensionality = checkBoxMultiSelect.Checked;
            _Options.EnglishMultiDimensionality = checkBoxEnglishMultiSelect.Checked;
            _Options.ForceSingleWord = checkBoxForceSingleWord.Checked;
            _Options.TraditionalChineseEnabled = checkBoxTraditionalChs.Checked;
            _Options.OutputSimplifiedTraditional = checkBoxST.Checked;
            _Options.UnknownWordIdentify = checkBoxUnknownWord.Checked;
            _Options.FilterEnglish = checkBoxFilterEnglish.Checked;
            _Options.FilterNumeric = checkBoxFilterNumeric.Checked;
            _Options.IgnoreCapital = checkBoxIgnoreCapital.Checked;
            _Options.EnglishSegment = checkBoxEnglishSegment.Checked;
            _Options.SynonymOutput = checkBoxSynonymOutput.Checked;
            _Options.WildcardOutput = checkBoxWildcard.Checked;
            _Options.WildcardSegment = checkBoxWildcardSegment.Checked;
            _Options.CustomRule = checkBoxCustomRule.Checked;

            _Parameters.Redundancy = (int)numericUpDownRedundancy.Value;
            _Parameters.FilterEnglishLength = (int)numericUpDownFilterEnglishLength.Value;
            _Parameters.FilterNumericLength = (int)numericUpDownFilterNumericLength.Value;

        }

        private void buttonSegment_Click(object sender, EventArgs e)
        {
            _Options = PanGu.Setting.PanGuSettings.Config.MatchOptions.Clone();
            _Parameters = PanGu.Setting.PanGuSettings.Config.Parameters.Clone();

            UpdateSettings();

            if (checkBoxDisplayPosition.Checked)
            {
                DisplaySegmentAndPostion();
            }
            else
            {
                DisplaySegment();
            }
        }

        private void buttonSaveConfig_Click(object sender, EventArgs e)
        {
            _Options = PanGu.Setting.PanGuSettings.Config.MatchOptions;
            _Parameters = PanGu.Setting.PanGuSettings.Config.Parameters;

            UpdateSettings();

            PanGu.Setting.PanGuSettings.Save("PanGu.xml");
        }
    }
}
