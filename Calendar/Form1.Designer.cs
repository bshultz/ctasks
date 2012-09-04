namespace CalendarTest
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
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			Calendar.DrawTool drawTool2 = new Calendar.DrawTool();
			this.dayView1 = new Calendar.DayView();
			this.m_pnlContainer = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label5 = new System.Windows.Forms.Label();
			this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
			this.button8 = new System.Windows.Forms.Button();
			this.chkbxEnableShadows = new System.Windows.Forms.CheckBox();
			this.chkbxUseRoundedCorners = new System.Windows.Forms.CheckBox();
			this.cmbbxInterval = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.m_sslMemory = new System.Windows.Forms.ToolStripStatusLabel();
			this.m_sslGdi = new System.Windows.Forms.ToolStripStatusLabel();
			this.m_sslObjectCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.m_lblSummary = new System.Windows.Forms.Label();
			this.m_btnStartStop = new System.Windows.Forms.Button();
			this.m_timer = new System.Windows.Forms.Timer(this.components);
			this.m_pnlContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dayView1
			// 
			drawTool2.DayView = this.dayView1;
			this.dayView1.ActiveTool = drawTool2;
			this.dayView1.AllowInplaceEditing = false;
			this.dayView1.AmPmDisplay = true;
			this.dayView1.AppointmentHeightMode = Calendar.AppHeightDrawMode.EndHalfHourBlocksAll;
			this.dayView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dayView1.DrawAllAppBorder = true;
			this.dayView1.EnableDurationDisplay = false;
			this.dayView1.EnableRoundedCorners = false;
			this.dayView1.EnableShadows = true;
			this.dayView1.EnableTimeIndicator = true;
			this.dayView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			this.dayView1.Location = new System.Drawing.Point(0, 0);
			this.dayView1.MinHalfHourApp = false;
			this.dayView1.Name = "dayView1";
			this.dayView1.SelectionEnd = new System.DateTime(((long)(0)));
			this.dayView1.SelectionStart = new System.DateTime(((long)(0)));
			this.dayView1.Size = new System.Drawing.Size(798, 283);
			this.dayView1.StartDate = new System.DateTime(((long)(0)));
			this.dayView1.TabIndex = 0;
			this.dayView1.Text = "dayView1";
			this.dayView1.WorkingHourEnd = 19;
			this.dayView1.WorkingHourStart = 9;
			this.dayView1.WorkingMinuteEnd = 0;
			this.dayView1.WorkingMinuteStart = 0;
			// 
			// m_pnlContainer
			// 
			this.m_pnlContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_pnlContainer.AutoScroll = true;
			this.m_pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_pnlContainer.Controls.Add(this.dayView1);
			this.m_pnlContainer.Location = new System.Drawing.Point(13, 179);
			this.m_pnlContainer.Name = "m_pnlContainer";
			this.m_pnlContainer.Size = new System.Drawing.Size(800, 285);
			this.m_pnlContainer.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(256, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "[Pointed date]";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(256, 119);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "[selection]";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(738, 151);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 7;
			this.button2.Text = "create";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(337, 16);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 8;
			this.button3.Text = "3";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(418, 16);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 9;
			this.button4.Text = "5";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(499, 16);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 10;
			this.button5.Text = "7";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(256, 16);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 11;
			this.button6.Text = "1";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "Office 11",
            "Office 12"});
			this.comboBox1.Location = new System.Drawing.Point(691, 14);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 13;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(646, 17);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 13);
			this.label4.TabIndex = 14;
			this.label4.Text = "Theme";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(657, 151);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 15;
			this.button1.Text = "color";
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// button7
			// 
			this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button7.Location = new System.Drawing.Point(576, 150);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(75, 23);
			this.button7.TabIndex = 16;
			this.button7.Text = "border";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(297, 45);
			this.trackBar1.Maximum = 80;
			this.trackBar1.Minimum = 5;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(277, 45);
			this.trackBar1.TabIndex = 17;
			this.trackBar1.Value = 18;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(258, 49);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(34, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Zoom";
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(16, 8);
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.TabIndex = 19;
			this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
			// 
			// button8
			// 
			this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button8.Location = new System.Drawing.Point(495, 150);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(75, 23);
			this.button8.TabIndex = 20;
			this.button8.Text = "scrollbar";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.button8_Click);
			// 
			// chkbxEnableShadows
			// 
			this.chkbxEnableShadows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkbxEnableShadows.AutoSize = true;
			this.chkbxEnableShadows.Checked = true;
			this.chkbxEnableShadows.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkbxEnableShadows.Location = new System.Drawing.Point(496, 128);
			this.chkbxEnableShadows.Name = "chkbxEnableShadows";
			this.chkbxEnableShadows.Size = new System.Drawing.Size(106, 17);
			this.chkbxEnableShadows.TabIndex = 21;
			this.chkbxEnableShadows.Text = "Enable Shadows";
			this.chkbxEnableShadows.UseVisualStyleBackColor = true;
			this.chkbxEnableShadows.CheckedChanged += new System.EventHandler(this.chkbxEnableShadows_CheckedChanged);
			// 
			// chkbxUseRoundedCorners
			// 
			this.chkbxUseRoundedCorners.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkbxUseRoundedCorners.AutoSize = true;
			this.chkbxUseRoundedCorners.Location = new System.Drawing.Point(616, 128);
			this.chkbxUseRoundedCorners.Name = "chkbxUseRoundedCorners";
			this.chkbxUseRoundedCorners.Size = new System.Drawing.Size(131, 17);
			this.chkbxUseRoundedCorners.TabIndex = 22;
			this.chkbxUseRoundedCorners.Text = "Use Rounded Corners";
			this.chkbxUseRoundedCorners.UseVisualStyleBackColor = true;
			this.chkbxUseRoundedCorners.CheckedChanged += new System.EventHandler(this.chkbxUseRoundedCorners_CheckedChanged);
			// 
			// cmbbxInterval
			// 
			this.cmbbxInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbbxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbbxInterval.FormattingEnabled = true;
			this.cmbbxInterval.Location = new System.Drawing.Point(691, 48);
			this.cmbbxInterval.Name = "cmbbxInterval";
			this.cmbbxInterval.Size = new System.Drawing.Size(121, 21);
			this.cmbbxInterval.TabIndex = 23;
			this.cmbbxInterval.SelectedIndexChanged += new System.EventHandler(this.cmbbxInterval_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(646, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 24;
			this.label1.Text = "Interval";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_sslMemory,
            this.m_sslGdi,
            this.m_sslObjectCount});
			this.statusStrip1.Location = new System.Drawing.Point(0, 499);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(825, 22);
			this.statusStrip1.TabIndex = 25;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// m_sslMemory
			// 
			this.m_sslMemory.Name = "m_sslMemory";
			this.m_sslMemory.Size = new System.Drawing.Size(59, 17);
			this.m_sslMemory.Text = "[GDI Test]";
			// 
			// m_sslGdi
			// 
			this.m_sslGdi.Name = "m_sslGdi";
			this.m_sslGdi.Size = new System.Drawing.Size(118, 17);
			this.m_sslGdi.Text = "toolStripStatusLabel2";
			// 
			// m_sslObjectCount
			// 
			this.m_sslObjectCount.Name = "m_sslObjectCount";
			this.m_sslObjectCount.Size = new System.Drawing.Size(118, 17);
			this.m_sslObjectCount.Text = "toolStripStatusLabel3";
			// 
			// m_lblSummary
			// 
			this.m_lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_lblSummary.AutoSize = true;
			this.m_lblSummary.Location = new System.Drawing.Point(16, 480);
			this.m_lblSummary.Name = "m_lblSummary";
			this.m_lblSummary.Size = new System.Drawing.Size(56, 13);
			this.m_lblSummary.TabIndex = 26;
			this.m_lblSummary.Text = "[Summary]";
			// 
			// m_btnStartStop
			// 
			this.m_btnStartStop.AutoSize = true;
			this.m_btnStartStop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_btnStartStop.Location = new System.Drawing.Point(256, 144);
			this.m_btnStartStop.Name = "m_btnStartStop";
			this.m_btnStartStop.Size = new System.Drawing.Size(142, 23);
			this.m_btnStartStop.TabIndex = 27;
			this.m_btnStartStop.Text = "Start create / dispose loop";
			this.m_btnStartStop.UseVisualStyleBackColor = true;
			this.m_btnStartStop.Click += new System.EventHandler(this.OnStartStop_Click);
			// 
			// m_timer
			// 
			this.m_timer.Enabled = true;
			this.m_timer.Tick += new System.EventHandler(this.OnTimer_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(825, 521);
			this.Controls.Add(this.m_btnStartStop);
			this.Controls.Add(this.m_lblSummary);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbbxInterval);
			this.Controls.Add(this.chkbxUseRoundedCorners);
			this.Controls.Add(this.chkbxEnableShadows);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.monthCalendar1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_pnlContainer);
			this.MinimumSize = new System.Drawing.Size(841, 557);
			this.Name = "Form1";
			this.Text = "DayView";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.m_pnlContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel m_pnlContainer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private Calendar.DayView dayView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Button button8;
		private System.Windows.Forms.CheckBox chkbxEnableShadows;
		private System.Windows.Forms.CheckBox chkbxUseRoundedCorners;
		private System.Windows.Forms.ComboBox cmbbxInterval;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel m_sslMemory;
		private System.Windows.Forms.ToolStripStatusLabel m_sslGdi;
		private System.Windows.Forms.ToolStripStatusLabel m_sslObjectCount;
		private System.Windows.Forms.Label m_lblSummary;
		private System.Windows.Forms.Button m_btnStartStop;
		private System.Windows.Forms.Timer m_timer;
    }
}

