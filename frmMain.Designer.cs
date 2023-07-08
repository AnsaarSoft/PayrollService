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
            this.rotatingRingsWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.RotatingRingsWaitingBarIndicatorElement();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.lblAction = new Telerik.WinControls.UI.RadLabel();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.Exit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblVersion = new Telerik.WinControls.UI.RadLabel();
            this.tmrFSEmployee = new System.Windows.Forms.Timer(this.components);
            this.tmrProbation = new System.Windows.Forms.Timer(this.components);
            this.tmrEOBI = new System.Windows.Forms.Timer(this.components);
            this.btnTest = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.wb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAction)).BeginInit();
            this.Exit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTest)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrEmployeeSync
            // 
            this.tmrEmployeeSync.Tick += new System.EventHandler(this.tmrEmployeeSync_Tick);
            // 
            // wb
            // 
            this.wb.Location = new System.Drawing.Point(9, 352);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(130, 49);
            this.wb.TabIndex = 0;
            this.wb.Text = "Hi";
            this.wb.ThemeName = "Fluent";
            this.wb.Visible = false;
            this.wb.WaitingIndicators.Add(this.rotatingRingsWaitingBarIndicatorElement1);
            this.wb.WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            this.wb.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.RotatingRings;
            // 
            // rotatingRingsWaitingBarIndicatorElement1
            // 
            this.rotatingRingsWaitingBarIndicatorElement1.Name = "rotatingRingsWaitingBarIndicatorElement1";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.radLabel1.Location = new System.Drawing.Point(9, 304);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(50, 23);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Action";
            // 
            // lblAction
            // 
            this.lblAction.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAction.Location = new System.Drawing.Point(9, 327);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(69, 23);
            this.lblAction.TabIndex = 2;
            this.lblAction.Text = "Waiting...";
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
            this.lblVersion.Location = new System.Drawing.Point(9, 9);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(38, 18);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Action";
            // 
            // tmrFSEmployee
            // 
            this.tmrFSEmployee.Tick += new System.EventHandler(this.tmrFSEmployee_Tick);
            // 
            // tmrProbation
            // 
            this.tmrProbation.Tick += new System.EventHandler(this.tmrProbation_Tick);
            // 
            // tmrEOBI
            // 
            this.tmrEOBI.Tick += new System.EventHandler(this.tmrEOBI_Tick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(336, 251);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(110, 24);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Test Probation";
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(474, 419);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.wb);
            this.DoubleBuffered = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.btnTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrEmployeeSync;
        private Telerik.WinControls.UI.RadWaitingBar wb;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel lblAction;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.ContextMenuStrip Exit;
        private System.Windows.Forms.ToolStripMenuItem cmsExit;
        private Telerik.WinControls.UI.RadLabel lblVersion;
        private System.Windows.Forms.Timer tmrFSEmployee;
        private Telerik.WinControls.UI.RotatingRingsWaitingBarIndicatorElement rotatingRingsWaitingBarIndicatorElement1;
        private System.Windows.Forms.Timer tmrProbation;
        private System.Windows.Forms.Timer tmrEOBI;
        private Telerik.WinControls.UI.RadButton btnTest;
    }
}

