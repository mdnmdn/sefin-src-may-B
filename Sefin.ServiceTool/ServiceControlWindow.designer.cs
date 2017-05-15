namespace Sefin.ServiceTool {
    partial class ServiceControlWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceControlWindow));
            this.BoxInfo = new System.Windows.Forms.GroupBox();
            this.LblInfo = new System.Windows.Forms.Label();
            this.BoxStatus = new System.Windows.Forms.GroupBox();
            this.LblStatus = new System.Windows.Forms.Label();
            this.BoxInstall = new System.Windows.Forms.GroupBox();
            this.LblInstall = new System.Windows.Forms.Label();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.BtnStartStop = new System.Windows.Forms.Button();
            this.BtnInstall = new System.Windows.Forms.Button();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.LblStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.LblTitleService = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TxtLog = new System.Windows.Forms.TextBox();
            this.BtnClearLog = new System.Windows.Forms.Button();
            this.BoxInfo.SuspendLayout();
            this.BoxStatus.SuspendLayout();
            this.BoxInstall.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoxInfo
            // 
            this.BoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxInfo.Controls.Add(this.LblInfo);
            this.BoxInfo.Location = new System.Drawing.Point(12, 25);
            this.BoxInfo.Name = "BoxInfo";
            this.BoxInfo.Size = new System.Drawing.Size(268, 61);
            this.BoxInfo.TabIndex = 0;
            this.BoxInfo.TabStop = false;
            this.BoxInfo.Text = "Info";
            // 
            // LblInfo
            // 
            this.LblInfo.AutoSize = true;
            this.LblInfo.Location = new System.Drawing.Point(7, 20);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(58, 13);
            this.LblInfo.TabIndex = 0;
            this.LblInfo.Text = "                 ";
            // 
            // BoxStatus
            // 
            this.BoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxStatus.Controls.Add(this.LblStatus);
            this.BoxStatus.Location = new System.Drawing.Point(12, 92);
            this.BoxStatus.Name = "BoxStatus";
            this.BoxStatus.Size = new System.Drawing.Size(268, 39);
            this.BoxStatus.TabIndex = 1;
            this.BoxStatus.TabStop = false;
            this.BoxStatus.Text = "Status";
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Location = new System.Drawing.Point(7, 16);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(58, 13);
            this.LblStatus.TabIndex = 1;
            this.LblStatus.Text = "                 ";
            // 
            // BoxInstall
            // 
            this.BoxInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxInstall.Controls.Add(this.LblInstall);
            this.BoxInstall.Location = new System.Drawing.Point(12, 138);
            this.BoxInstall.Name = "BoxInstall";
            this.BoxInstall.Size = new System.Drawing.Size(268, 38);
            this.BoxInstall.TabIndex = 2;
            this.BoxInstall.TabStop = false;
            this.BoxInstall.Text = "Installation";
            // 
            // LblInstall
            // 
            this.LblInstall.AutoSize = true;
            this.LblInstall.Location = new System.Drawing.Point(7, 16);
            this.LblInstall.Name = "LblInstall";
            this.LblInstall.Size = new System.Drawing.Size(58, 13);
            this.LblInstall.TabIndex = 2;
            this.LblInstall.Text = "                 ";
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRefresh.Enabled = false;
            this.BtnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRefresh.Location = new System.Drawing.Point(295, 32);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(75, 23);
            this.BtnRefresh.TabIndex = 0;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // BtnStartStop
            // 
            this.BtnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStartStop.Enabled = false;
            this.BtnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStartStop.Location = new System.Drawing.Point(295, 61);
            this.BtnStartStop.Name = "BtnStartStop";
            this.BtnStartStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStartStop.TabIndex = 3;
            this.BtnStartStop.Text = "Start";
            this.BtnStartStop.UseVisualStyleBackColor = true;
            this.BtnStartStop.Click += new System.EventHandler(this.BtnStartStop_Click);
            // 
            // BtnInstall
            // 
            this.BtnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnInstall.Enabled = false;
            this.BtnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInstall.Location = new System.Drawing.Point(295, 90);
            this.BtnInstall.Name = "BtnInstall";
            this.BtnInstall.Size = new System.Drawing.Size(75, 23);
            this.BtnInstall.TabIndex = 4;
            this.BtnInstall.Text = "Install";
            this.BtnInstall.UseVisualStyleBackColor = true;
            this.BtnInstall.Click += new System.EventHandler(this.BtnInstall_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblStatusBar});
            this.StatusBar.Location = new System.Drawing.Point(0, 339);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(384, 22);
            this.StatusBar.SizingGrip = false;
            this.StatusBar.TabIndex = 5;
            // 
            // LblStatusBar
            // 
            this.LblStatusBar.Name = "LblStatusBar";
            this.LblStatusBar.Size = new System.Drawing.Size(0, 17);
            // 
            // LblTitleService
            // 
            this.LblTitleService.AutoSize = true;
            this.LblTitleService.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LblTitleService.Location = new System.Drawing.Point(12, 9);
            this.LblTitleService.Name = "LblTitleService";
            this.LblTitleService.Size = new System.Drawing.Size(50, 13);
            this.LblTitleService.TabIndex = 6;
            this.LblTitleService.Text = "Service";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.TxtLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 182);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 154);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // TxtLog
            // 
            this.TxtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtLog.Location = new System.Drawing.Point(6, 19);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.ReadOnly = true;
            this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtLog.Size = new System.Drawing.Size(348, 129);
            this.TxtLog.TabIndex = 0;
            // 
            // BtnClearLog
            // 
            this.BtnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClearLog.Location = new System.Drawing.Point(295, 149);
            this.BtnClearLog.Name = "BtnClearLog";
            this.BtnClearLog.Size = new System.Drawing.Size(75, 23);
            this.BtnClearLog.TabIndex = 7;
            this.BtnClearLog.Text = "Clear log";
            this.BtnClearLog.UseVisualStyleBackColor = true;
            this.BtnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);
            // 
            // ServiceControlWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.BtnClearLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LblTitleService);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.BtnInstall);
            this.Controls.Add(this.BtnStartStop);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.BoxInstall);
            this.Controls.Add(this.BoxStatus);
            this.Controls.Add(this.BoxInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "ServiceControlWindow";
            this.Text = "Sefin Service Tool";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServiceControlWindow_FormClosed);
            this.Load += new System.EventHandler(this.ServiceControlWindow_Load);
            this.BoxInfo.ResumeLayout(false);
            this.BoxInfo.PerformLayout();
            this.BoxStatus.ResumeLayout(false);
            this.BoxStatus.PerformLayout();
            this.BoxInstall.ResumeLayout(false);
            this.BoxInstall.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox BoxInfo;
        private System.Windows.Forms.GroupBox BoxStatus;
        private System.Windows.Forms.GroupBox BoxInstall;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.Button BtnStartStop;
        private System.Windows.Forms.Button BtnInstall;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel LblStatusBar;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label LblInstall;
        private System.Windows.Forms.Label LblTitleService;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtLog;
        private System.Windows.Forms.Button BtnClearLog;
    }
}