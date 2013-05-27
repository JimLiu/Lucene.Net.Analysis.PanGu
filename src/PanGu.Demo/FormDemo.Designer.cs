namespace Demo
{
    partial class FormDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDemo));
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.numericUpDownRedundancy = new System.Windows.Forms.NumericUpDown();
            this.checkBoxMultiSelect = new System.Windows.Forms.CheckBox();
            this.buttonSaveConfig = new System.Windows.Forms.Button();
            this.numericUpDownUnknownWordsThreshold = new System.Windows.Forms.NumericUpDown();
            this.checkBoxFreqFirst = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayPosition = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterStopWords = new System.Windows.Forms.CheckBox();
            this.checkBoxMatchName = new System.Windows.Forms.CheckBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelSrcLength = new System.Windows.Forms.Label();
            this.labelRegRate = new System.Windows.Forms.Label();
            this.labelSegTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSegment = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSegwords = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.checkBoxForceSingleWord = new System.Windows.Forms.CheckBox();
            this.checkBoxTraditionalChs = new System.Windows.Forms.CheckBox();
            this.checkBoxST = new System.Windows.Forms.CheckBox();
            this.checkBoxUnknownWord = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterEnglish = new System.Windows.Forms.CheckBox();
            this.numericUpDownFilterEnglishLength = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.labelFilterNumericLength = new System.Windows.Forms.Label();
            this.numericUpDownFilterNumericLength = new System.Windows.Forms.NumericUpDown();
            this.checkBoxFilterNumeric = new System.Windows.Forms.CheckBox();
            this.checkBoxIgnoreCapital = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTimeOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxEnglishSegment = new System.Windows.Forms.CheckBox();
            this.checkBoxSynonymOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxWildcard = new System.Windows.Forms.CheckBox();
            this.checkBoxWildcardSegment = new System.Windows.Forms.CheckBox();
            this.checkBoxCustomRule = new System.Windows.Forms.CheckBox();
            this.checkBoxEnglishMultiSelect = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRedundancy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnknownWordsThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterEnglishLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterNumericLength)).BeginInit();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 88;
            this.label10.Text = "项目首页";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(294, 593);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 87;
            this.label14.Text = "冗余度";
            // 
            // numericUpDownRedundancy
            // 
            this.numericUpDownRedundancy.Location = new System.Drawing.Point(387, 590);
            this.numericUpDownRedundancy.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownRedundancy.Name = "numericUpDownRedundancy";
            this.numericUpDownRedundancy.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownRedundancy.TabIndex = 86;
            this.numericUpDownRedundancy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxMultiSelect
            // 
            this.checkBoxMultiSelect.AutoSize = true;
            this.checkBoxMultiSelect.Location = new System.Drawing.Point(191, 593);
            this.checkBoxMultiSelect.Name = "checkBoxMultiSelect";
            this.checkBoxMultiSelect.Size = new System.Drawing.Size(74, 17);
            this.checkBoxMultiSelect.TabIndex = 85;
            this.checkBoxMultiSelect.Text = "多元分词";
            this.checkBoxMultiSelect.UseVisualStyleBackColor = true;
            // 
            // buttonSaveConfig
            // 
            this.buttonSaveConfig.Location = new System.Drawing.Point(91, 567);
            this.buttonSaveConfig.Name = "buttonSaveConfig";
            this.buttonSaveConfig.Size = new System.Drawing.Size(75, 25);
            this.buttonSaveConfig.TabIndex = 84;
            this.buttonSaveConfig.Text = "保存配置";
            this.buttonSaveConfig.UseVisualStyleBackColor = true;
            this.buttonSaveConfig.Click += new System.EventHandler(this.buttonSaveConfig_Click);
            // 
            // numericUpDownUnknownWordsThreshold
            // 
            this.numericUpDownUnknownWordsThreshold.Location = new System.Drawing.Point(865, 697);
            this.numericUpDownUnknownWordsThreshold.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownUnknownWordsThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownUnknownWordsThreshold.Name = "numericUpDownUnknownWordsThreshold";
            this.numericUpDownUnknownWordsThreshold.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownUnknownWordsThreshold.TabIndex = 80;
            this.numericUpDownUnknownWordsThreshold.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // checkBoxFreqFirst
            // 
            this.checkBoxFreqFirst.AutoSize = true;
            this.checkBoxFreqFirst.Checked = true;
            this.checkBoxFreqFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFreqFirst.Location = new System.Drawing.Point(494, 570);
            this.checkBoxFreqFirst.Name = "checkBoxFreqFirst";
            this.checkBoxFreqFirst.Size = new System.Drawing.Size(98, 17);
            this.checkBoxFreqFirst.TabIndex = 78;
            this.checkBoxFreqFirst.Text = "优先判断词频";
            this.checkBoxFreqFirst.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayPosition
            // 
            this.checkBoxDisplayPosition.AutoSize = true;
            this.checkBoxDisplayPosition.Location = new System.Drawing.Point(387, 570);
            this.checkBoxDisplayPosition.Name = "checkBoxDisplayPosition";
            this.checkBoxDisplayPosition.Size = new System.Drawing.Size(98, 17);
            this.checkBoxDisplayPosition.TabIndex = 76;
            this.checkBoxDisplayPosition.Text = "显示单词位置";
            this.checkBoxDisplayPosition.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterStopWords
            // 
            this.checkBoxFilterStopWords.AutoSize = true;
            this.checkBoxFilterStopWords.Location = new System.Drawing.Point(298, 570);
            this.checkBoxFilterStopWords.Name = "checkBoxFilterStopWords";
            this.checkBoxFilterStopWords.Size = new System.Drawing.Size(86, 17);
            this.checkBoxFilterStopWords.TabIndex = 74;
            this.checkBoxFilterStopWords.Text = "过滤停用词";
            this.checkBoxFilterStopWords.UseVisualStyleBackColor = true;
            // 
            // checkBoxMatchName
            // 
            this.checkBoxMatchName.AutoSize = true;
            this.checkBoxMatchName.Checked = true;
            this.checkBoxMatchName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMatchName.Location = new System.Drawing.Point(191, 570);
            this.checkBoxMatchName.Name = "checkBoxMatchName";
            this.checkBoxMatchName.Size = new System.Drawing.Size(98, 17);
            this.checkBoxMatchName.TabIndex = 73;
            this.checkBoxMatchName.Text = "识别中文人名";
            this.checkBoxMatchName.UseVisualStyleBackColor = true;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(67, 41);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(206, 13);
            this.linkLabel2.TabIndex = 72;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "http://www.codeplex.com/pangusegment";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(444, 13);
            this.label9.TabIndex = 70;
            this.label9.Text = "盘古中英文分词组件是由eaglet开发的一款基于 .Net Framework 2.0的轻量级分词组件";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(189, 539);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "chars";
            // 
            // labelSrcLength
            // 
            this.labelSrcLength.AutoSize = true;
            this.labelSrcLength.Location = new System.Drawing.Point(112, 540);
            this.labelSrcLength.Name = "labelSrcLength";
            this.labelSrcLength.Size = new System.Drawing.Size(13, 13);
            this.labelSrcLength.TabIndex = 68;
            this.labelSrcLength.Text = "0";
            // 
            // labelRegRate
            // 
            this.labelRegRate.AutoSize = true;
            this.labelRegRate.Location = new System.Drawing.Point(502, 540);
            this.labelRegRate.Name = "labelRegRate";
            this.labelRegRate.Size = new System.Drawing.Size(13, 13);
            this.labelRegRate.TabIndex = 67;
            this.labelRegRate.Text = "0";
            // 
            // labelSegTime
            // 
            this.labelSegTime.AutoSize = true;
            this.labelSegTime.Location = new System.Drawing.Point(296, 540);
            this.labelSegTime.Name = "labelSegTime";
            this.labelSegTime.Size = new System.Drawing.Size(13, 13);
            this.labelSegTime.TabIndex = 66;
            this.labelSegTime.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(616, 539);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 65;
            this.label8.Text = "chars/s";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(416, 540);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "s";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(433, 540);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 63;
            this.label6.Text = "分词速度";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 540);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "分词时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 540);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "源字符串长度";
            // 
            // buttonSegment
            // 
            this.buttonSegment.Location = new System.Drawing.Point(10, 567);
            this.buttonSegment.Name = "buttonSegment";
            this.buttonSegment.Size = new System.Drawing.Size(75, 25);
            this.buttonSegment.TabIndex = 60;
            this.buttonSegment.Text = "分词";
            this.buttonSegment.UseVisualStyleBackColor = true;
            this.buttonSegment.Click += new System.EventHandler(this.buttonSegment_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "分词结果";
            // 
            // textBoxSegwords
            // 
            this.textBoxSegwords.Location = new System.Drawing.Point(1, 315);
            this.textBoxSegwords.Multiline = true;
            this.textBoxSegwords.Name = "textBoxSegwords";
            this.textBoxSegwords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSegwords.Size = new System.Drawing.Size(906, 212);
            this.textBoxSegwords.TabIndex = 58;
            this.textBoxSegwords.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "源文";
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(1, 84);
            this.textBoxSource.MaxLength = 327670000;
            this.textBoxSource.Multiline = true;
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSource.Size = new System.Drawing.Size(906, 187);
            this.textBoxSource.TabIndex = 56;
            // 
            // checkBoxForceSingleWord
            // 
            this.checkBoxForceSingleWord.AutoSize = true;
            this.checkBoxForceSingleWord.Location = new System.Drawing.Point(494, 591);
            this.checkBoxForceSingleWord.Name = "checkBoxForceSingleWord";
            this.checkBoxForceSingleWord.Size = new System.Drawing.Size(98, 17);
            this.checkBoxForceSingleWord.TabIndex = 89;
            this.checkBoxForceSingleWord.Text = "强制一元分词";
            this.checkBoxForceSingleWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxTraditionalChs
            // 
            this.checkBoxTraditionalChs.AutoSize = true;
            this.checkBoxTraditionalChs.Checked = true;
            this.checkBoxTraditionalChs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTraditionalChs.Location = new System.Drawing.Point(599, 568);
            this.checkBoxTraditionalChs.Name = "checkBoxTraditionalChs";
            this.checkBoxTraditionalChs.Size = new System.Drawing.Size(86, 17);
            this.checkBoxTraditionalChs.TabIndex = 90;
            this.checkBoxTraditionalChs.Text = "繁体字分词";
            this.checkBoxTraditionalChs.UseVisualStyleBackColor = true;
            // 
            // checkBoxST
            // 
            this.checkBoxST.AutoSize = true;
            this.checkBoxST.Checked = true;
            this.checkBoxST.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxST.Location = new System.Drawing.Point(702, 567);
            this.checkBoxST.Name = "checkBoxST";
            this.checkBoxST.Size = new System.Drawing.Size(134, 17);
            this.checkBoxST.TabIndex = 91;
            this.checkBoxST.Text = "同时输出简体和繁体";
            this.checkBoxST.UseVisualStyleBackColor = true;
            // 
            // checkBoxUnknownWord
            // 
            this.checkBoxUnknownWord.AutoSize = true;
            this.checkBoxUnknownWord.Location = new System.Drawing.Point(599, 590);
            this.checkBoxUnknownWord.Name = "checkBoxUnknownWord";
            this.checkBoxUnknownWord.Size = new System.Drawing.Size(98, 17);
            this.checkBoxUnknownWord.TabIndex = 92;
            this.checkBoxUnknownWord.Text = "未登录词识别";
            this.checkBoxUnknownWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterEnglish
            // 
            this.checkBoxFilterEnglish.AutoSize = true;
            this.checkBoxFilterEnglish.Location = new System.Drawing.Point(191, 619);
            this.checkBoxFilterEnglish.Name = "checkBoxFilterEnglish";
            this.checkBoxFilterEnglish.Size = new System.Drawing.Size(74, 17);
            this.checkBoxFilterEnglish.TabIndex = 93;
            this.checkBoxFilterEnglish.Text = "过滤英文";
            this.checkBoxFilterEnglish.UseVisualStyleBackColor = true;
            // 
            // numericUpDownFilterEnglishLength
            // 
            this.numericUpDownFilterEnglishLength.Location = new System.Drawing.Point(387, 615);
            this.numericUpDownFilterEnglishLength.Name = "numericUpDownFilterEnglishLength";
            this.numericUpDownFilterEnglishLength.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownFilterEnglishLength.TabIndex = 94;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(294, 620);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 95;
            this.label11.Text = "过滤英文长度";
            // 
            // labelFilterNumericLength
            // 
            this.labelFilterNumericLength.AutoSize = true;
            this.labelFilterNumericLength.Location = new System.Drawing.Point(599, 618);
            this.labelFilterNumericLength.Name = "labelFilterNumericLength";
            this.labelFilterNumericLength.Size = new System.Drawing.Size(79, 13);
            this.labelFilterNumericLength.TabIndex = 98;
            this.labelFilterNumericLength.Text = "过滤数字长度";
            // 
            // numericUpDownFilterNumericLength
            // 
            this.numericUpDownFilterNumericLength.Location = new System.Drawing.Point(692, 614);
            this.numericUpDownFilterNumericLength.Name = "numericUpDownFilterNumericLength";
            this.numericUpDownFilterNumericLength.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownFilterNumericLength.TabIndex = 97;
            // 
            // checkBoxFilterNumeric
            // 
            this.checkBoxFilterNumeric.AutoSize = true;
            this.checkBoxFilterNumeric.Location = new System.Drawing.Point(494, 617);
            this.checkBoxFilterNumeric.Name = "checkBoxFilterNumeric";
            this.checkBoxFilterNumeric.Size = new System.Drawing.Size(74, 17);
            this.checkBoxFilterNumeric.TabIndex = 96;
            this.checkBoxFilterNumeric.Text = "过滤数字";
            this.checkBoxFilterNumeric.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreCapital
            // 
            this.checkBoxIgnoreCapital.AutoSize = true;
            this.checkBoxIgnoreCapital.Location = new System.Drawing.Point(702, 589);
            this.checkBoxIgnoreCapital.Name = "checkBoxIgnoreCapital";
            this.checkBoxIgnoreCapital.Size = new System.Drawing.Size(110, 17);
            this.checkBoxIgnoreCapital.TabIndex = 99;
            this.checkBoxIgnoreCapital.Text = "忽略英文大小写";
            this.checkBoxIgnoreCapital.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTimeOnly
            // 
            this.checkBoxShowTimeOnly.AutoSize = true;
            this.checkBoxShowTimeOnly.Location = new System.Drawing.Point(10, 616);
            this.checkBoxShowTimeOnly.Name = "checkBoxShowTimeOnly";
            this.checkBoxShowTimeOnly.Size = new System.Drawing.Size(110, 17);
            this.checkBoxShowTimeOnly.TabIndex = 100;
            this.checkBoxShowTimeOnly.Text = "仅显示分词时间";
            this.checkBoxShowTimeOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnglishSegment
            // 
            this.checkBoxEnglishSegment.AutoSize = true;
            this.checkBoxEnglishSegment.Location = new System.Drawing.Point(297, 645);
            this.checkBoxEnglishSegment.Name = "checkBoxEnglishSegment";
            this.checkBoxEnglishSegment.Size = new System.Drawing.Size(74, 17);
            this.checkBoxEnglishSegment.TabIndex = 101;
            this.checkBoxEnglishSegment.Text = "英文分词";
            this.checkBoxEnglishSegment.UseVisualStyleBackColor = true;
            // 
            // checkBoxSynonymOutput
            // 
            this.checkBoxSynonymOutput.AutoSize = true;
            this.checkBoxSynonymOutput.Location = new System.Drawing.Point(191, 668);
            this.checkBoxSynonymOutput.Name = "checkBoxSynonymOutput";
            this.checkBoxSynonymOutput.Size = new System.Drawing.Size(86, 17);
            this.checkBoxSynonymOutput.TabIndex = 102;
            this.checkBoxSynonymOutput.Text = "输出同义词";
            this.checkBoxSynonymOutput.UseVisualStyleBackColor = true;
            // 
            // checkBoxWildcard
            // 
            this.checkBoxWildcard.AutoSize = true;
            this.checkBoxWildcard.Location = new System.Drawing.Point(283, 668);
            this.checkBoxWildcard.Name = "checkBoxWildcard";
            this.checkBoxWildcard.Size = new System.Drawing.Size(86, 17);
            this.checkBoxWildcard.TabIndex = 103;
            this.checkBoxWildcard.Text = "通配符匹配";
            this.checkBoxWildcard.UseVisualStyleBackColor = true;
            // 
            // checkBoxWildcardSegment
            // 
            this.checkBoxWildcardSegment.AutoSize = true;
            this.checkBoxWildcardSegment.Location = new System.Drawing.Point(388, 668);
            this.checkBoxWildcardSegment.Name = "checkBoxWildcardSegment";
            this.checkBoxWildcardSegment.Size = new System.Drawing.Size(182, 17);
            this.checkBoxWildcardSegment.TabIndex = 104;
            this.checkBoxWildcardSegment.Text = "对通配符匹配出来的词再分词";
            this.checkBoxWildcardSegment.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomRule
            // 
            this.checkBoxCustomRule.AutoSize = true;
            this.checkBoxCustomRule.Location = new System.Drawing.Point(586, 667);
            this.checkBoxCustomRule.Name = "checkBoxCustomRule";
            this.checkBoxCustomRule.Size = new System.Drawing.Size(86, 17);
            this.checkBoxCustomRule.TabIndex = 105;
            this.checkBoxCustomRule.Text = "自定义规则";
            this.checkBoxCustomRule.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnglishMultiSelect
            // 
            this.checkBoxEnglishMultiSelect.AutoSize = true;
            this.checkBoxEnglishMultiSelect.Location = new System.Drawing.Point(191, 645);
            this.checkBoxEnglishMultiSelect.Name = "checkBoxEnglishMultiSelect";
            this.checkBoxEnglishMultiSelect.Size = new System.Drawing.Size(98, 17);
            this.checkBoxEnglishMultiSelect.TabIndex = 106;
            this.checkBoxEnglishMultiSelect.Text = "英文多元分词";
            this.checkBoxEnglishMultiSelect.UseVisualStyleBackColor = true;
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 691);
            this.Controls.Add(this.checkBoxEnglishMultiSelect);
            this.Controls.Add(this.checkBoxCustomRule);
            this.Controls.Add(this.checkBoxWildcardSegment);
            this.Controls.Add(this.checkBoxWildcard);
            this.Controls.Add(this.checkBoxSynonymOutput);
            this.Controls.Add(this.checkBoxEnglishSegment);
            this.Controls.Add(this.checkBoxShowTimeOnly);
            this.Controls.Add(this.checkBoxIgnoreCapital);
            this.Controls.Add(this.labelFilterNumericLength);
            this.Controls.Add(this.numericUpDownFilterNumericLength);
            this.Controls.Add(this.checkBoxFilterNumeric);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericUpDownFilterEnglishLength);
            this.Controls.Add(this.checkBoxFilterEnglish);
            this.Controls.Add(this.checkBoxUnknownWord);
            this.Controls.Add(this.checkBoxST);
            this.Controls.Add(this.checkBoxTraditionalChs);
            this.Controls.Add(this.checkBoxForceSingleWord);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.numericUpDownRedundancy);
            this.Controls.Add(this.checkBoxMultiSelect);
            this.Controls.Add(this.buttonSaveConfig);
            this.Controls.Add(this.numericUpDownUnknownWordsThreshold);
            this.Controls.Add(this.checkBoxFreqFirst);
            this.Controls.Add(this.checkBoxDisplayPosition);
            this.Controls.Add(this.checkBoxFilterStopWords);
            this.Controls.Add(this.checkBoxMatchName);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelSrcLength);
            this.Controls.Add(this.labelRegRate);
            this.Controls.Add(this.labelSegTime);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonSegment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSegwords);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSource);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDemo";
            this.Text = "PanGu Demo";
            this.Load += new System.EventHandler(this.FormDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRedundancy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnknownWordsThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterEnglishLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterNumericLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numericUpDownRedundancy;
        private System.Windows.Forms.CheckBox checkBoxMultiSelect;
        private System.Windows.Forms.Button buttonSaveConfig;
        private System.Windows.Forms.NumericUpDown numericUpDownUnknownWordsThreshold;
        private System.Windows.Forms.CheckBox checkBoxFreqFirst;
        private System.Windows.Forms.CheckBox checkBoxDisplayPosition;
        private System.Windows.Forms.CheckBox checkBoxFilterStopWords;
        private System.Windows.Forms.CheckBox checkBoxMatchName;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSrcLength;
        private System.Windows.Forms.Label labelRegRate;
        private System.Windows.Forms.Label labelSegTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSegment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSegwords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.CheckBox checkBoxForceSingleWord;
        private System.Windows.Forms.CheckBox checkBoxTraditionalChs;
        private System.Windows.Forms.CheckBox checkBoxST;
        private System.Windows.Forms.CheckBox checkBoxUnknownWord;
        private System.Windows.Forms.CheckBox checkBoxFilterEnglish;
        private System.Windows.Forms.NumericUpDown numericUpDownFilterEnglishLength;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelFilterNumericLength;
        private System.Windows.Forms.NumericUpDown numericUpDownFilterNumericLength;
        private System.Windows.Forms.CheckBox checkBoxFilterNumeric;
        private System.Windows.Forms.CheckBox checkBoxIgnoreCapital;
        private System.Windows.Forms.CheckBox checkBoxShowTimeOnly;
        private System.Windows.Forms.CheckBox checkBoxEnglishSegment;
        private System.Windows.Forms.CheckBox checkBoxSynonymOutput;
        private System.Windows.Forms.CheckBox checkBoxWildcard;
        private System.Windows.Forms.CheckBox checkBoxWildcardSegment;
        private System.Windows.Forms.CheckBox checkBoxCustomRule;
        private System.Windows.Forms.CheckBox checkBoxEnglishMultiSelect;

    }
}

