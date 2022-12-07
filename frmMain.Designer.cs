namespace PayrollService
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tmrEmployeeSync = new System.Windows.Forms.Timer(this.components);
            this.wb = new Telerik.WinControls.UI.RadWaitingBar();
            this.waitingBarIndicatorElement2 = new Telerik.WinControls.UI.WaitingBarIndicatorElement();
            this.waitingBarIndicatorElement1 = new Telerik.WinControls.UI.WaitingBarIndicatorElement();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.lblAction = new Telerik.WinControls.UI.RadLabel();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.Exit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblVersion = new Telerik.WinControls.UI.RadLabel();
            this.tmrFSEmployee = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.wb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAction)).BeginInit();
            this.Exit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrEmployeeSync
            // 
            this.tmrEmployeeSync.Tick += new System.EventHandler(this.tmrEmployeeSync_Tick);
            // 
            // wb
            // 
            this.wb.Location = new System.Drawing.Point(179, 464);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(130, 24);
            this.wb.TabIndex = 0;
            this.wb.Text = "Hi";
            this.wb.ThemeName = "Fluent";
            this.wb.Visible = false;
            this.wb.WaitingIndicators.Add(this.waitingBarIndicatorElement2);
            this.wb.WaitingIndicators.Add(this.waitingBarIndicatorElement1);
            this.wb.WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            // 
            // waitingBarIndicatorElement2
            // 
            this.waitingBarIndicatorElement2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(102)))), ((int)(((byte)(229)))));
            this.waitingBarIndicatorElement2.BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(106)))), ((int)(((byte)(228)))));
            this.waitingBarIndicatorElement2.Name = "waitingBarIndicatorElement2";
            this.waitingBarIndicatorElement2.StretchHorizontally = false;
            // 
            // waitingBarIndicatorElement1
            // 
            this.waitingBarIndicatorElement1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(102)))), ((int)(((byte)(229)))));
            this.waitingBarIndicatorElement1.BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(106)))), ((int)(((byte)(228)))));
            this.waitingBarIndicatorElement1.Name = "waitingBarIndicatorElement1";
            this.waitingBarIndicatorElement1.StretchHorizontally = false;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 470);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(38, 18);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Action";
            this.radLabel1.Visible = false;
            // 
            // lblAction
            // 
            this.lblAction.Location = new System.Drawing.Point(56, 470);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(52, 18);
            this.lblAction.TabIndex = 2;
            this.lblAction.Text = "Waiting...";
            this.lblAction.Visible = false;
            // 
            // ni
            // 
            this.ni.ContextMenuStrip = this.Exit;
            this.ni.Icon = ((System.Drawing.Icon)(resources.GetObject("ni.Icon")));
            this.ni.Text = "Payroll Service";
            this.ni.Visible = true;
            // 
            // Exit
            // 
            this.Exit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsExit});
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(94, 26);
            // 
            // cmsExit
            // 
            this.cmsExit.Name = "cmsExit";
            this.cmsExit.Size = new System.Drawing.Size(93, 22);
            this.cmsExit.Text = "Exit";
            this.cmsExit.Click += new System.EventHandler(this.cmsExit_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(387, 12);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(38, 18);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Action";
            // 
            // tmrFSEmployee
            // 
            this.tmrFSEmployee.Tick += new System.EventHandler(this.tmrFSEmployee_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.wb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAction)).EndInit();
            this.Exit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrEmployeeSync;
        private Telerik.WinControls.UI.RadWaitingBar wb;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.WaitingBarIndicatorElement waitingBarIndicatorElement2;
        private Telerik.WinControls.UI.WaitingBarIndicatorElement waitingBarIndicatorElement1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel lblAction;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.ContextMenuStrip Exit;
        private System.Windows.Forms.ToolStripMenuItem cmsExit;
        private Telerik.WinControls.UI.RadLabel lblVersion;
        private System.Windows.Forms.Timer tmrFSEmployee;
    }
}

