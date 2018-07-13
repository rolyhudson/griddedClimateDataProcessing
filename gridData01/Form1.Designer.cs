namespace gridData01
{
    partial class Form1
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
            this.gridButton = new System.Windows.Forms.Button();
            this.weaButton = new System.Windows.Forms.Button();
            this.demButton = new System.Windows.Forms.Button();
            this.getGrid = new System.Windows.Forms.Button();
            this.averageDataBtn = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.writeCountryBtn = new System.Windows.Forms.Button();
            this.writeAllBTn = new System.Windows.Forms.Button();
            this.readclimaBtn = new System.Windows.Forms.Button();
            this.kgBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.endYrUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.startYrUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.classAvOnly = new System.Windows.Forms.CheckBox();
            this.statsBtn = new System.Windows.Forms.Button();
            this.holdridgeBtn = new System.Windows.Forms.Button();
            this.HVBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.classificationLbl = new System.Windows.Forms.Label();
            this.yearsLbl = new System.Windows.Forms.Label();
            this.gridPtsLbl = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.resetBtn = new System.Windows.Forms.Button();
            this.indentClimaChk = new System.Windows.Forms.CheckBox();
            this.averagesOnlyChk = new System.Windows.Forms.CheckBox();
            this.Output = new System.Windows.Forms.GroupBox();
            this.classOnlyChkBx = new System.Windows.Forms.CheckBox();
            this.compactOutputChkbx = new System.Windows.Forms.CheckBox();
            this.windBtn = new System.Windows.Forms.Button();
            this.clusterBtn = new System.Windows.Forms.Button();
            this.pcaChkBx = new System.Windows.Forms.CheckBox();
            this.normaliseChkBx = new System.Windows.Forms.CheckBox();
            this.logscaleChkBx = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endYrUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYrUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.Output.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridButton
            // 
            this.gridButton.Location = new System.Drawing.Point(6, 21);
            this.gridButton.Name = "gridButton";
            this.gridButton.Size = new System.Drawing.Size(263, 35);
            this.gridButton.TabIndex = 0;
            this.gridButton.Text = "generate grid";
            this.gridButton.UseVisualStyleBackColor = true;
            this.gridButton.Click += new System.EventHandler(this.makeGrid);
            // 
            // weaButton
            // 
            this.weaButton.Location = new System.Drawing.Point(9, 103);
            this.weaButton.Name = "weaButton";
            this.weaButton.Size = new System.Drawing.Size(262, 35);
            this.weaButton.TabIndex = 1;
            this.weaButton.Text = "get climate data";
            this.weaButton.UseVisualStyleBackColor = true;
            this.weaButton.Click += new System.EventHandler(this.readWeather);
            // 
            // demButton
            // 
            this.demButton.Location = new System.Drawing.Point(7, 62);
            this.demButton.Name = "demButton";
            this.demButton.Size = new System.Drawing.Size(262, 35);
            this.demButton.TabIndex = 2;
            this.demButton.Text = "get elevation data";
            this.demButton.UseVisualStyleBackColor = true;
            this.demButton.Click += new System.EventHandler(this.elevation);
            // 
            // getGrid
            // 
            this.getGrid.Location = new System.Drawing.Point(7, 103);
            this.getGrid.Name = "getGrid";
            this.getGrid.Size = new System.Drawing.Size(262, 35);
            this.getGrid.TabIndex = 3;
            this.getGrid.Text = "read exisiting grid";
            this.getGrid.UseVisualStyleBackColor = true;
            this.getGrid.Click += new System.EventHandler(this.readAGrid);
            // 
            // averageDataBtn
            // 
            this.averageDataBtn.Location = new System.Drawing.Point(21, 21);
            this.averageDataBtn.Name = "averageDataBtn";
            this.averageDataBtn.Size = new System.Drawing.Size(262, 35);
            this.averageDataBtn.TabIndex = 4;
            this.averageDataBtn.Text = "average all years ";
            this.averageDataBtn.UseVisualStyleBackColor = true;
            this.averageDataBtn.Click += new System.EventHandler(this.averageData);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(338, 692);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(260, 35);
            this.buttonTest.TabIndex = 5;
            this.buttonTest.Text = "Bogota, Cali, Medellin test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.testWeather);
            // 
            // writeCountryBtn
            // 
            this.writeCountryBtn.Location = new System.Drawing.Point(12, 21);
            this.writeCountryBtn.Name = "writeCountryBtn";
            this.writeCountryBtn.Size = new System.Drawing.Size(262, 35);
            this.writeCountryBtn.TabIndex = 6;
            this.writeCountryBtn.Text = "write grid with altitude";
            this.writeCountryBtn.UseVisualStyleBackColor = true;
            this.writeCountryBtn.Click += new System.EventHandler(this.writeCountryBtn_Click);
            // 
            // writeAllBTn
            // 
            this.writeAllBTn.Location = new System.Drawing.Point(12, 208);
            this.writeAllBTn.Name = "writeAllBTn";
            this.writeAllBTn.Size = new System.Drawing.Size(262, 35);
            this.writeAllBTn.TabIndex = 7;
            this.writeAllBTn.Text = "write all data to clima json";
            this.writeAllBTn.UseVisualStyleBackColor = true;
            this.writeAllBTn.Click += new System.EventHandler(this.writeClimateData);
            // 
            // readclimaBtn
            // 
            this.readclimaBtn.Location = new System.Drawing.Point(23, 331);
            this.readclimaBtn.Name = "readclimaBtn";
            this.readclimaBtn.Size = new System.Drawing.Size(262, 35);
            this.readclimaBtn.TabIndex = 8;
            this.readclimaBtn.Text = "read data from clima json";
            this.readclimaBtn.UseVisualStyleBackColor = true;
            this.readclimaBtn.Click += new System.EventHandler(this.readclimaBtn_Click);
            // 
            // kgBtn
            // 
            this.kgBtn.Location = new System.Drawing.Point(21, 98);
            this.kgBtn.Name = "kgBtn";
            this.kgBtn.Size = new System.Drawing.Size(262, 35);
            this.kgBtn.TabIndex = 9;
            this.kgBtn.Text = "Koppen Geiger";
            this.kgBtn.UseVisualStyleBackColor = true;
            this.kgBtn.Click += new System.EventHandler(this.generateKG);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.readclimaBtn);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 374);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "pre processing";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.endYrUpDown);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.startYrUpDown);
            this.groupBox5.Controls.Add(this.weaButton);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(14, 176);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(278, 149);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "get climate data";
            // 
            // endYrUpDown
            // 
            this.endYrUpDown.Location = new System.Drawing.Point(86, 54);
            this.endYrUpDown.Maximum = new decimal(new int[] {
            2009,
            0,
            0,
            0});
            this.endYrUpDown.Minimum = new decimal(new int[] {
            1902,
            0,
            0,
            0});
            this.endYrUpDown.Name = "endYrUpDown";
            this.endYrUpDown.Size = new System.Drawing.Size(120, 22);
            this.endYrUpDown.TabIndex = 15;
            this.endYrUpDown.Value = new decimal(new int[] {
            2009,
            0,
            0,
            0});
            this.endYrUpDown.ValueChanged += new System.EventHandler(this.yearChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "End year:";
            // 
            // startYrUpDown
            // 
            this.startYrUpDown.Location = new System.Drawing.Point(86, 26);
            this.startYrUpDown.Maximum = new decimal(new int[] {
            2008,
            0,
            0,
            0});
            this.startYrUpDown.Minimum = new decimal(new int[] {
            1901,
            0,
            0,
            0});
            this.startYrUpDown.Name = "startYrUpDown";
            this.startYrUpDown.Size = new System.Drawing.Size(120, 22);
            this.startYrUpDown.TabIndex = 13;
            this.startYrUpDown.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.startYrUpDown.ValueChanged += new System.EventHandler(this.yearChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Start year:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gridButton);
            this.groupBox4.Controls.Add(this.getGrid);
            this.groupBox4.Controls.Add(this.demButton);
            this.groupBox4.Location = new System.Drawing.Point(14, 21);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(278, 149);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "get grid and elevation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.classAvOnly);
            this.groupBox2.Controls.Add(this.statsBtn);
            this.groupBox2.Controls.Add(this.holdridgeBtn);
            this.groupBox2.Controls.Add(this.averageDataBtn);
            this.groupBox2.Controls.Add(this.kgBtn);
            this.groupBox2.Location = new System.Drawing.Point(12, 392);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 284);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "post processing / classification";
            // 
            // classAvOnly
            // 
            this.classAvOnly.AutoSize = true;
            this.classAvOnly.Checked = true;
            this.classAvOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.classAvOnly.Location = new System.Drawing.Point(23, 70);
            this.classAvOnly.Name = "classAvOnly";
            this.classAvOnly.Size = new System.Drawing.Size(171, 21);
            this.classAvOnly.TabIndex = 17;
            this.classAvOnly.Text = "Classify averages only";
            this.classAvOnly.UseVisualStyleBackColor = true;
            // 
            // statsBtn
            // 
            this.statsBtn.Enabled = false;
            this.statsBtn.Location = new System.Drawing.Point(21, 221);
            this.statsBtn.Name = "statsBtn";
            this.statsBtn.Size = new System.Drawing.Size(262, 35);
            this.statsBtn.TabIndex = 14;
            this.statsBtn.Text = "Climate Statisitics";
            this.statsBtn.UseVisualStyleBackColor = true;
            this.statsBtn.Click += new System.EventHandler(this.statsBtn_Click);
            // 
            // holdridgeBtn
            // 
            this.holdridgeBtn.Enabled = false;
            this.holdridgeBtn.Location = new System.Drawing.Point(21, 139);
            this.holdridgeBtn.Name = "holdridgeBtn";
            this.holdridgeBtn.Size = new System.Drawing.Size(262, 35);
            this.holdridgeBtn.TabIndex = 11;
            this.holdridgeBtn.Text = "Holdridge";
            this.holdridgeBtn.UseVisualStyleBackColor = true;
            this.holdridgeBtn.Click += new System.EventHandler(this.holdridgeBtn_Click);
            // 
            // HVBtn
            // 
            this.HVBtn.Location = new System.Drawing.Point(32, 774);
            this.HVBtn.Name = "HVBtn";
            this.HVBtn.Size = new System.Drawing.Size(262, 35);
            this.HVBtn.TabIndex = 15;
            this.HVBtn.Text = "HudsonVelasco";
            this.HVBtn.UseVisualStyleBackColor = true;
            this.HVBtn.Click += new System.EventHandler(this.hvBtnClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.classificationLbl);
            this.groupBox3.Controls.Add(this.yearsLbl);
            this.groupBox3.Controls.Add(this.gridPtsLbl);
            this.groupBox3.Location = new System.Drawing.Point(326, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(287, 374);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "status";
            // 
            // classificationLbl
            // 
            this.classificationLbl.AutoSize = true;
            this.classificationLbl.Location = new System.Drawing.Point(9, 102);
            this.classificationLbl.Name = "classificationLbl";
            this.classificationLbl.Size = new System.Drawing.Size(141, 17);
            this.classificationLbl.TabIndex = 3;
            this.classificationLbl.Text = "Data sets generated:";
            // 
            // yearsLbl
            // 
            this.yearsLbl.AutoSize = true;
            this.yearsLbl.Location = new System.Drawing.Point(6, 69);
            this.yearsLbl.Name = "yearsLbl";
            this.yearsLbl.Size = new System.Drawing.Size(165, 17);
            this.yearsLbl.TabIndex = 2;
            this.yearsLbl.Text = "Years of climate data = 0";
            // 
            // gridPtsLbl
            // 
            this.gridPtsLbl.AutoSize = true;
            this.gridPtsLbl.Location = new System.Drawing.Point(6, 36);
            this.gridPtsLbl.Name = "gridPtsLbl";
            this.gridPtsLbl.Size = new System.Drawing.Size(134, 17);
            this.gridPtsLbl.TabIndex = 0;
            this.gridPtsLbl.Text = "Total grid points = 0";
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(338, 733);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(260, 35);
            this.resetBtn.TabIndex = 13;
            this.resetBtn.Text = "Reset all";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // indentClimaChk
            // 
            this.indentClimaChk.AutoSize = true;
            this.indentClimaChk.Location = new System.Drawing.Point(12, 97);
            this.indentClimaChk.Name = "indentClimaChk";
            this.indentClimaChk.Size = new System.Drawing.Size(219, 38);
            this.indentClimaChk.TabIndex = 4;
            this.indentClimaChk.Text = "Indent clima json \r\n(for a readable but bigger file)";
            this.indentClimaChk.UseVisualStyleBackColor = true;
            // 
            // averagesOnlyChk
            // 
            this.averagesOnlyChk.AutoSize = true;
            this.averagesOnlyChk.Location = new System.Drawing.Point(12, 70);
            this.averagesOnlyChk.Name = "averagesOnlyChk";
            this.averagesOnlyChk.Size = new System.Drawing.Size(249, 21);
            this.averagesOnlyChk.TabIndex = 14;
            this.averagesOnlyChk.Text = "Output averages only (single year)";
            this.averagesOnlyChk.UseVisualStyleBackColor = true;
            // 
            // Output
            // 
            this.Output.Controls.Add(this.classOnlyChkBx);
            this.Output.Controls.Add(this.compactOutputChkbx);
            this.Output.Controls.Add(this.writeCountryBtn);
            this.Output.Controls.Add(this.averagesOnlyChk);
            this.Output.Controls.Add(this.indentClimaChk);
            this.Output.Controls.Add(this.writeAllBTn);
            this.Output.Location = new System.Drawing.Point(326, 392);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(287, 284);
            this.Output.TabIndex = 15;
            this.Output.TabStop = false;
            this.Output.Text = "output";
            // 
            // classOnlyChkBx
            // 
            this.classOnlyChkBx.AutoSize = true;
            this.classOnlyChkBx.Checked = true;
            this.classOnlyChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.classOnlyChkBx.Location = new System.Drawing.Point(12, 172);
            this.classOnlyChkBx.Name = "classOnlyChkBx";
            this.classOnlyChkBx.Size = new System.Drawing.Size(194, 21);
            this.classOnlyChkBx.TabIndex = 16;
            this.classOnlyChkBx.Text = "Output classifications only";
            this.classOnlyChkBx.UseVisualStyleBackColor = true;
            // 
            // compactOutputChkbx
            // 
            this.compactOutputChkbx.AutoSize = true;
            this.compactOutputChkbx.Location = new System.Drawing.Point(12, 141);
            this.compactOutputChkbx.Name = "compactOutputChkbx";
            this.compactOutputChkbx.Size = new System.Drawing.Size(162, 21);
            this.compactOutputChkbx.TabIndex = 15;
            this.compactOutputChkbx.Text = "Output compact form";
            this.compactOutputChkbx.UseVisualStyleBackColor = true;
            // 
            // windBtn
            // 
            this.windBtn.Location = new System.Drawing.Point(338, 774);
            this.windBtn.Name = "windBtn";
            this.windBtn.Size = new System.Drawing.Size(260, 35);
            this.windBtn.TabIndex = 16;
            this.windBtn.Text = "wind";
            this.windBtn.UseVisualStyleBackColor = true;
            this.windBtn.Click += new System.EventHandler(this.windBtn_Click);
            // 
            // clusterBtn
            // 
            this.clusterBtn.Location = new System.Drawing.Point(338, 815);
            this.clusterBtn.Name = "clusterBtn";
            this.clusterBtn.Size = new System.Drawing.Size(260, 35);
            this.clusterBtn.TabIndex = 17;
            this.clusterBtn.Text = "Cluster";
            this.clusterBtn.UseVisualStyleBackColor = true;
            this.clusterBtn.Click += new System.EventHandler(this.clusterBtn_Click);
            // 
            // pcaChkBx
            // 
            this.pcaChkBx.AutoSize = true;
            this.pcaChkBx.Checked = true;
            this.pcaChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pcaChkBx.Location = new System.Drawing.Point(34, 733);
            this.pcaChkBx.Name = "pcaChkBx";
            this.pcaChkBx.Size = new System.Drawing.Size(57, 21);
            this.pcaChkBx.TabIndex = 18;
            this.pcaChkBx.Text = "PCA";
            this.pcaChkBx.UseVisualStyleBackColor = true;
            // 
            // normaliseChkBx
            // 
            this.normaliseChkBx.AutoSize = true;
            this.normaliseChkBx.Enabled = false;
            this.normaliseChkBx.Location = new System.Drawing.Point(34, 706);
            this.normaliseChkBx.Name = "normaliseChkBx";
            this.normaliseChkBx.Size = new System.Drawing.Size(125, 21);
            this.normaliseChkBx.TabIndex = 19;
            this.normaliseChkBx.Text = "Normalise data";
            this.normaliseChkBx.UseVisualStyleBackColor = true;
            // 
            // logscaleChkBx
            // 
            this.logscaleChkBx.AutoSize = true;
            this.logscaleChkBx.Enabled = false;
            this.logscaleChkBx.Location = new System.Drawing.Point(34, 679);
            this.logscaleChkBx.Name = "logscaleChkBx";
            this.logscaleChkBx.Size = new System.Drawing.Size(115, 21);
            this.logscaleChkBx.TabIndex = 20;
            this.logscaleChkBx.Text = "Use log scale";
            this.logscaleChkBx.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 815);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(262, 35);
            this.button1.TabIndex = 21;
            this.button1.Text = "HudsonVelasco1D";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.hv1DBtnClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 879);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logscaleChkBx);
            this.Controls.Add(this.normaliseChkBx);
            this.Controls.Add(this.pcaChkBx);
            this.Controls.Add(this.clusterBtn);
            this.Controls.Add(this.HVBtn);
            this.Controls.Add(this.windBtn);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endYrUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startYrUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.Output.ResumeLayout(false);
            this.Output.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button gridButton;
        private System.Windows.Forms.Button weaButton;
        private System.Windows.Forms.Button demButton;
        private System.Windows.Forms.Button getGrid;
        private System.Windows.Forms.Button averageDataBtn;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button writeCountryBtn;
        private System.Windows.Forms.Button writeAllBTn;
        private System.Windows.Forms.Button readclimaBtn;
        private System.Windows.Forms.Button kgBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label yearsLbl;
        private System.Windows.Forms.Label gridPtsLbl;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown endYrUpDown;
        private System.Windows.Forms.NumericUpDown startYrUpDown;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label classificationLbl;
        private System.Windows.Forms.Button holdridgeBtn;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button statsBtn;
        private System.Windows.Forms.CheckBox indentClimaChk;
        private System.Windows.Forms.CheckBox averagesOnlyChk;
        private System.Windows.Forms.GroupBox Output;
        private System.Windows.Forms.CheckBox compactOutputChkbx;
        private System.Windows.Forms.CheckBox classOnlyChkBx;
        private System.Windows.Forms.Button windBtn;
        private System.Windows.Forms.Button HVBtn;
        private System.Windows.Forms.Button clusterBtn;
        private System.Windows.Forms.CheckBox classAvOnly;
        private System.Windows.Forms.CheckBox pcaChkBx;
        private System.Windows.Forms.CheckBox normaliseChkBx;
        private System.Windows.Forms.CheckBox logscaleChkBx;
        private System.Windows.Forms.Button button1;
    }
}

