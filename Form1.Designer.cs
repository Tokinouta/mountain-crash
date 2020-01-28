namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.generation = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.combatForceOptions = new System.Windows.Forms.ComboBox();
            this.killOptions = new System.Windows.Forms.ComboBox();
            this.ozoneExists = new System.Windows.Forms.CheckBox();
            this.elfExists = new System.Windows.Forms.CheckBox();
            this.eggExists = new System.Windows.Forms.CheckBox();
            this.hatExists = new System.Windows.Forms.CheckBox();
            this.proprietorExists = new System.Windows.Forms.CheckBox();
            this.PitNumber = new System.Windows.Forms.TextBox();
            this.ClinicNumber = new System.Windows.Forms.TextBox();
            this.RiverNumber = new System.Windows.Forms.TextBox();
            this.MountainNumber = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.pause = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.playerRemained = new System.Windows.Forms.Label();
            this.gamingTime = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerForBattle = new System.Windows.Forms.Timer(this.components);
            this.UpdateSpeed = new System.Windows.Forms.Timer(this.components);
            this.BattleField = new System.Windows.Forms.GroupBox();
            this.Result = new System.Windows.Forms.DataGridView();
            this.玩家名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.Label();
            this.RandomGeneration = new System.Windows.Forms.Button();
            this.TeamNumber = new System.Windows.Forms.NumericUpDown();
            this.TeamNumberPrompt = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TeamNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // generation
            // 
            this.generation.Location = new System.Drawing.Point(30, 54);
            this.generation.Name = "generation";
            this.generation.Size = new System.Drawing.Size(104, 34);
            this.generation.TabIndex = 0;
            this.generation.Text = "Generation";
            this.generation.UseVisualStyleBackColor = true;
            this.generation.Click += new System.EventHandler(this.Generation_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.TeamNumberPrompt);
            this.groupBox1.Controls.Add(this.TeamNumber);
            this.groupBox1.Controls.Add(this.combatForceOptions);
            this.groupBox1.Controls.Add(this.killOptions);
            this.groupBox1.Controls.Add(this.ozoneExists);
            this.groupBox1.Controls.Add(this.elfExists);
            this.groupBox1.Controls.Add(this.eggExists);
            this.groupBox1.Controls.Add(this.hatExists);
            this.groupBox1.Controls.Add(this.proprietorExists);
            this.groupBox1.Controls.Add(this.PitNumber);
            this.groupBox1.Controls.Add(this.ClinicNumber);
            this.groupBox1.Controls.Add(this.RiverNumber);
            this.groupBox1.Controls.Add(this.MountainNumber);
            this.groupBox1.Location = new System.Drawing.Point(140, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 464);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configurations";
            // 
            // combatForceOptions
            // 
            this.combatForceOptions.FormattingEnabled = true;
            this.combatForceOptions.Items.AddRange(new object[] {
            "战力相同随机",
            "战力相同轮回",
            "战力不相同"});
            this.combatForceOptions.Location = new System.Drawing.Point(31, 237);
            this.combatForceOptions.Name = "combatForceOptions";
            this.combatForceOptions.Size = new System.Drawing.Size(191, 23);
            this.combatForceOptions.TabIndex = 11;
            this.combatForceOptions.Text = "Combat Force Options";
            // 
            // killOptions
            // 
            this.killOptions.FormattingEnabled = true;
            this.killOptions.Items.AddRange(new object[] {
            "单向等量伤害模式",
            "单向比例伤害模式",
            "双向伤害模式",
            "回血模式"});
            this.killOptions.Location = new System.Drawing.Point(31, 208);
            this.killOptions.Name = "killOptions";
            this.killOptions.Size = new System.Drawing.Size(191, 23);
            this.killOptions.TabIndex = 10;
            this.killOptions.Text = "Kill Options";
            // 
            // ozoneExists
            // 
            this.ozoneExists.AutoSize = true;
            this.ozoneExists.Location = new System.Drawing.Point(29, 168);
            this.ozoneExists.Name = "ozoneExists";
            this.ozoneExists.Size = new System.Drawing.Size(119, 19);
            this.ozoneExists.TabIndex = 8;
            this.ozoneExists.Text = "有臭氧加速器";
            this.ozoneExists.UseVisualStyleBackColor = true;
            // 
            // elfExists
            // 
            this.elfExists.AutoSize = true;
            this.elfExists.Location = new System.Drawing.Point(148, 143);
            this.elfExists.Name = "elfExists";
            this.elfExists.Size = new System.Drawing.Size(74, 19);
            this.elfExists.TabIndex = 7;
            this.elfExists.Text = "有精灵";
            this.elfExists.UseVisualStyleBackColor = true;
            // 
            // eggExists
            // 
            this.eggExists.AutoSize = true;
            this.eggExists.Location = new System.Drawing.Point(29, 143);
            this.eggExists.Name = "eggExists";
            this.eggExists.Size = new System.Drawing.Size(74, 19);
            this.eggExists.TabIndex = 6;
            this.eggExists.Text = "有弱蛋";
            this.eggExists.UseVisualStyleBackColor = true;
            // 
            // hatExists
            // 
            this.hatExists.AutoSize = true;
            this.hatExists.Location = new System.Drawing.Point(148, 118);
            this.hatExists.Name = "hatExists";
            this.hatExists.Size = new System.Drawing.Size(104, 19);
            this.hatExists.TabIndex = 5;
            this.hatExists.Text = "有草帽大叔";
            this.hatExists.UseVisualStyleBackColor = true;
            // 
            // proprietorExists
            // 
            this.proprietorExists.AutoSize = true;
            this.proprietorExists.Location = new System.Drawing.Point(29, 118);
            this.proprietorExists.Name = "proprietorExists";
            this.proprietorExists.Size = new System.Drawing.Size(74, 19);
            this.proprietorExists.TabIndex = 4;
            this.proprietorExists.Text = "有社长";
            this.proprietorExists.UseVisualStyleBackColor = true;
            // 
            // PitNumber
            // 
            this.PitNumber.Location = new System.Drawing.Point(148, 81);
            this.PitNumber.Name = "PitNumber";
            this.PitNumber.Size = new System.Drawing.Size(103, 25);
            this.PitNumber.TabIndex = 3;
            this.PitNumber.Text = "坑的数量";
            this.PitNumber.Click += new System.EventHandler(this.Textbox_Click);
            // 
            // ClinicNumber
            // 
            this.ClinicNumber.Location = new System.Drawing.Point(29, 81);
            this.ClinicNumber.Name = "ClinicNumber";
            this.ClinicNumber.Size = new System.Drawing.Size(96, 25);
            this.ClinicNumber.TabIndex = 2;
            this.ClinicNumber.Text = "医疗站数量";
            this.ClinicNumber.Click += new System.EventHandler(this.Textbox_Click);
            // 
            // RiverNumber
            // 
            this.RiverNumber.Location = new System.Drawing.Point(148, 40);
            this.RiverNumber.Name = "RiverNumber";
            this.RiverNumber.Size = new System.Drawing.Size(104, 25);
            this.RiverNumber.TabIndex = 1;
            this.RiverNumber.Text = "河的数量";
            this.RiverNumber.Click += new System.EventHandler(this.Textbox_Click);
            // 
            // MountainNumber
            // 
            this.MountainNumber.Location = new System.Drawing.Point(29, 40);
            this.MountainNumber.Name = "MountainNumber";
            this.MountainNumber.Size = new System.Drawing.Size(97, 25);
            this.MountainNumber.TabIndex = 0;
            this.MountainNumber.Text = "山的数量";
            this.MountainNumber.Click += new System.EventHandler(this.Textbox_Click);
            // 
            // start
            // 
            this.start.Enabled = false;
            this.start.Location = new System.Drawing.Point(30, 94);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(103, 33);
            this.start.TabIndex = 4;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // pause
            // 
            this.pause.Enabled = false;
            this.pause.Location = new System.Drawing.Point(30, 133);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(103, 33);
            this.pause.TabIndex = 5;
            this.pause.Text = "Pause";
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.pause_Click);
            // 
            // clear
            // 
            this.clear.Enabled = false;
            this.clear.Location = new System.Drawing.Point(30, 172);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(103, 33);
            this.clear.TabIndex = 6;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // playerRemained
            // 
            this.playerRemained.AutoSize = true;
            this.playerRemained.Location = new System.Drawing.Point(27, 240);
            this.playerRemained.Name = "playerRemained";
            this.playerRemained.Size = new System.Drawing.Size(97, 15);
            this.playerRemained.TabIndex = 7;
            this.playerRemained.Text = "剩余玩家数量";
            // 
            // gamingTime
            // 
            this.gamingTime.AutoSize = true;
            this.gamingTime.Location = new System.Drawing.Point(48, 296);
            this.gamingTime.Name = "gamingTime";
            this.gamingTime.Size = new System.Drawing.Size(55, 15);
            this.gamingTime.TabIndex = 8;
            this.gamingTime.Text = "000:00";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerForBattle
            // 
            this.timerForBattle.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // UpdateSpeed
            // 
            this.UpdateSpeed.Interval = 15000;
            this.UpdateSpeed.Tick += new System.EventHandler(this.UpdateSpeed_Tick);
            // 
            // BattleField
            // 
            this.BattleField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BattleField.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BattleField.BackgroundImage = global::WindowsFormsApp1.Properties.Resources.Screenshot_2019_10_16_at_11_50_12_1024x613;
            this.BattleField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BattleField.Location = new System.Drawing.Point(12, 349);
            this.BattleField.Name = "BattleField";
            this.BattleField.Size = new System.Drawing.Size(111, 48);
            this.BattleField.TabIndex = 0;
            this.BattleField.TabStop = false;
            this.BattleField.Text = "BattleField";
            this.BattleField.Visible = false;
            this.BattleField.SizeChanged += new System.EventHandler(this.BattleField_SizeChanged);
            this.BattleField.Paint += new System.Windows.Forms.PaintEventHandler(this.BattleField_Paint);
            // 
            // Result
            // 
            this.Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Result.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.玩家名称,
            this.Column8,
            this.Column3,
            this.Column2,
            this.Column4,
            this.Column5,
            this.Column7,
            this.Column6,
            this.Column9});
            this.Result.Location = new System.Drawing.Point(449, 12);
            this.Result.Name = "Result";
            this.Result.RowHeadersWidth = 5;
            this.Result.RowTemplate.Height = 20;
            this.Result.Size = new System.Drawing.Size(499, 464);
            this.Result.TabIndex = 9;
            // 
            // 玩家名称
            // 
            this.玩家名称.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.玩家名称.HeaderText = "玩家名称";
            this.玩家名称.MinimumWidth = 6;
            this.玩家名称.Name = "玩家名称";
            this.玩家名称.ReadOnly = true;
            this.玩家名称.Width = 96;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column8.HeaderText = "幸存排名";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 96;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column3.HeaderText = "幸存时间";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 96;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column2.HeaderText = "玩家得分";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 96;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column4.HeaderText = "时间得分";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 96;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column5.HeaderText = "交战得分";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 96;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column7.HeaderText = "加分";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 66;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column6.HeaderText = "殉职人数";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 96;
            // 
            // Column9
            // 
            this.Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column9.HeaderText = "被殉职";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 81;
            // 
            // Number
            // 
            this.Number.AutoSize = true;
            this.Number.Location = new System.Drawing.Point(27, 220);
            this.Number.Name = "Number";
            this.Number.Size = new System.Drawing.Size(67, 15);
            this.Number.TabIndex = 10;
            this.Number.Text = "玩家数量";
            // 
            // RandomGeneration
            // 
            this.RandomGeneration.Location = new System.Drawing.Point(30, 14);
            this.RandomGeneration.Name = "RandomGeneration";
            this.RandomGeneration.Size = new System.Drawing.Size(103, 34);
            this.RandomGeneration.TabIndex = 11;
            this.RandomGeneration.Text = "Randomize!";
            this.RandomGeneration.UseVisualStyleBackColor = true;
            this.RandomGeneration.Click += new System.EventHandler(this.RandomGeneration_Click);
            // 
            // TeamNumber
            // 
            this.TeamNumber.Location = new System.Drawing.Point(31, 294);
            this.TeamNumber.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.TeamNumber.Name = "TeamNumber";
            this.TeamNumber.Size = new System.Drawing.Size(191, 25);
            this.TeamNumber.TabIndex = 12;
            // 
            // TeamNumberPrompt
            // 
            this.TeamNumberPrompt.AutoSize = true;
            this.TeamNumberPrompt.Location = new System.Drawing.Point(31, 267);
            this.TeamNumberPrompt.Name = "TeamNumberPrompt";
            this.TeamNumberPrompt.Size = new System.Drawing.Size(187, 15);
            this.TeamNumberPrompt.TabIndex = 13;
            this.TeamNumberPrompt.Text = "战队数量（点击进入帮助）";
            this.TeamNumberPrompt.Click += new System.EventHandler(this.TeamNumberPrompt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 488);
            this.Controls.Add(this.RandomGeneration);
            this.Controls.Add(this.Number);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.gamingTime);
            this.Controls.Add(this.playerRemained);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.start);
            this.Controls.Add(this.BattleField);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.generation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TeamNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generation;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button pause;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox BattleField;
        private System.Windows.Forms.TextBox PitNumber;
        private System.Windows.Forms.TextBox ClinicNumber;
        private System.Windows.Forms.TextBox RiverNumber;
        private System.Windows.Forms.TextBox MountainNumber;
        private System.Windows.Forms.CheckBox ozoneExists;
        private System.Windows.Forms.CheckBox elfExists;
        private System.Windows.Forms.CheckBox eggExists;
        private System.Windows.Forms.CheckBox hatExists;
        private System.Windows.Forms.CheckBox proprietorExists;
        private System.Windows.Forms.Label playerRemained;
        private System.Windows.Forms.Label gamingTime;
        private System.Windows.Forms.ComboBox combatForceOptions;
        private System.Windows.Forms.ComboBox killOptions;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerForBattle;
        private System.Windows.Forms.Timer UpdateSpeed;
        private System.Windows.Forms.DataGridView Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn 玩家名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.Label Number;
        private System.Windows.Forms.Button RandomGeneration;
        private System.Windows.Forms.Label TeamNumberPrompt;
        private System.Windows.Forms.NumericUpDown TeamNumber;
    }
}

