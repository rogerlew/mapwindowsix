using MapWindow.Components;
namespace MapWindow
{
    partial class frmMapWindow6
    {
        

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapWindow6));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLegend = new System.Windows.Forms.TabPage();
            this.legend1 = new MapWindow.Components.Legend();
            this.mwStatusStrip1 = new MapWindow.Components.mwStatusStrip();
            this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabToolbox = new System.Windows.Forms.TabPage();
            this.toolManager1 = new MapWindow.Tools.ToolManager();
            this.toolManagerToolStrip1 = new MapWindow.Tools.ToolManagerToolStrip();
            this.geoMap1 = new MapWindow.Map.Map();
            this.mwToolStrip1 = new MapWindow.Components.mwToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectByAttributes = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationManager1 = new MapWindow.Components.ApplicationManager();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabLegend.SuspendLayout();
            this.mwStatusStrip1.SuspendLayout();
            this.tabToolbox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.AccessibleDescription = null;
            this.toolStripContainer1.AccessibleName = null;
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.BottomToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.BottomToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.BottomToolStripPanel, "toolStripContainer1.BottomToolStripPanel");
            this.toolStripContainer1.BottomToolStripPanel.Font = null;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AccessibleDescription = null;
            this.toolStripContainer1.ContentPanel.AccessibleName = null;
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            this.toolStripContainer1.ContentPanel.BackgroundImage = null;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Font = null;
            this.toolStripContainer1.Font = null;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.LeftToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.LeftToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.LeftToolStripPanel, "toolStripContainer1.LeftToolStripPanel");
            this.toolStripContainer1.LeftToolStripPanel.Font = null;
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.RightToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.RightToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.RightToolStripPanel, "toolStripContainer1.RightToolStripPanel");
            this.toolStripContainer1.RightToolStripPanel.Font = null;
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.TopToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.TopToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.TopToolStripPanel, "toolStripContainer1.TopToolStripPanel");
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.mwToolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Font = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.AccessibleDescription = null;
            this.splitContainer1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackgroundImage = null;
            this.splitContainer1.Font = null;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AccessibleDescription = null;
            this.splitContainer1.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackgroundImage = null;
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Font = null;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AccessibleDescription = null;
            this.splitContainer1.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackgroundImage = null;
            this.splitContainer1.Panel2.Controls.Add(this.geoMap1);
            this.splitContainer1.Panel2.Font = null;
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleDescription = null;
            this.tabControl1.AccessibleName = null;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.BackgroundImage = null;
            this.tabControl1.Controls.Add(this.tabLegend);
            this.tabControl1.Controls.Add(this.tabToolbox);
            this.tabControl1.Font = null;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabLegend
            // 
            this.tabLegend.AccessibleDescription = null;
            this.tabLegend.AccessibleName = null;
            resources.ApplyResources(this.tabLegend, "tabLegend");
            this.tabLegend.BackgroundImage = null;
            this.tabLegend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabLegend.Controls.Add(this.legend1);
            this.tabLegend.Font = null;
            this.tabLegend.Name = "tabLegend";
            this.tabLegend.UseVisualStyleBackColor = true;
            // 
            // legend1
            // 
            this.legend1.AccessibleDescription = null;
            this.legend1.AccessibleName = null;
            resources.ApplyResources(this.legend1, "legend1");
            this.legend1.BackColor = System.Drawing.Color.White;
            this.legend1.BackgroundImage = null;
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 275, 575);
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 346, 573);
            this.legend1.Font = null;
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = this.mwStatusStrip1;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.VerticalScrollEnabled = true;
            // 
            // mwStatusStrip1
            // 
            this.mwStatusStrip1.AccessibleDescription = null;
            this.mwStatusStrip1.AccessibleName = null;
            resources.ApplyResources(this.mwStatusStrip1, "mwStatusStrip1");
            this.mwStatusStrip1.BackgroundImage = null;
            this.mwStatusStrip1.Font = null;
            this.mwStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusProgress,
            this.statusLocation});
            this.mwStatusStrip1.Name = "mwStatusStrip1";
            // 
            // statusMessage
            // 
            this.statusMessage.AccessibleDescription = null;
            this.statusMessage.AccessibleName = null;
            resources.ApplyResources(this.statusMessage, "statusMessage");
            this.statusMessage.BackgroundImage = null;
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Spring = true;
            // 
            // statusProgress
            // 
            this.statusProgress.AccessibleDescription = null;
            this.statusProgress.AccessibleName = null;
            resources.ApplyResources(this.statusProgress, "statusProgress");
            this.statusProgress.Name = "statusProgress";
            // 
            // statusLocation
            // 
            this.statusLocation.AccessibleDescription = null;
            this.statusLocation.AccessibleName = null;
            resources.ApplyResources(this.statusLocation, "statusLocation");
            this.statusLocation.BackgroundImage = null;
            this.statusLocation.Name = "statusLocation";
            this.statusLocation.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // tabToolbox
            // 
            this.tabToolbox.AccessibleDescription = null;
            this.tabToolbox.AccessibleName = null;
            resources.ApplyResources(this.tabToolbox, "tabToolbox");
            this.tabToolbox.BackgroundImage = null;
            this.tabToolbox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabToolbox.Controls.Add(this.toolManager1);
            this.tabToolbox.Controls.Add(this.toolManagerToolStrip1);
            this.tabToolbox.Font = null;
            this.tabToolbox.Name = "tabToolbox";
            this.tabToolbox.UseVisualStyleBackColor = true;
            // 
            // toolManager1
            // 
            this.toolManager1.AccessibleDescription = null;
            this.toolManager1.AccessibleName = null;
            resources.ApplyResources(this.toolManager1, "toolManager1");
            this.toolManager1.BackgroundImage = null;
            this.toolManager1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolManager1.Font = null;
            this.toolManager1.HideSelection = false;
            this.toolManager1.Legend = this.legend1;
            this.toolManager1.Name = "toolManager1";
            this.toolManager1.TempPath = "c:\\temp\\";
            this.toolManager1.ToolDirectories = ((System.Collections.Generic.List<string>)(resources.GetObject("toolManager1.ToolDirectories")));
            // 
            // toolManagerToolStrip1
            // 
            this.toolManagerToolStrip1.AccessibleDescription = null;
            this.toolManagerToolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolManagerToolStrip1, "toolManagerToolStrip1");
            this.toolManagerToolStrip1.BackgroundImage = null;
            this.toolManagerToolStrip1.Font = null;
            this.toolManagerToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolManagerToolStrip1.Name = "toolManagerToolStrip1";
            this.toolManagerToolStrip1.ToolManager = this.toolManager1;
            // 
            // geoMap1
            // 
            this.geoMap1.AccessibleDescription = null;
            this.geoMap1.AccessibleName = null;
            this.geoMap1.AllowDrop = true;
            resources.ApplyResources(this.geoMap1, "geoMap1");
            this.geoMap1.BackColor = System.Drawing.Color.White;
            this.geoMap1.BackgroundImage = null;
            this.geoMap1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.geoMap1.CollectAfterDraw = false;
            this.geoMap1.CollisionDetection = true;
            this.geoMap1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.geoMap1.CursorMode = MapWindow.Components.FunctionModes.None;
            this.geoMap1.ExtendBuffer = false;
            this.geoMap1.Font = null;
            this.geoMap1.FunctionMode = MapWindow.Components.FunctionModes.None;
            this.geoMap1.Legend = this.legend1;
            this.geoMap1.Name = "geoMap1";
            this.geoMap1.ProgressHandler = this.mwStatusStrip1;
            this.geoMap1.SelectionEnabled = true;
            // 
            // mwToolStrip1
            // 
            this.mwToolStrip1.AccessibleDescription = null;
            this.mwToolStrip1.AccessibleName = null;
            resources.ApplyResources(this.mwToolStrip1, "mwToolStrip1");
            this.mwToolStrip1.BackgroundImage = null;
            this.mwToolStrip1.Font = null;
            this.mwToolStrip1.Map = this.geoMap1;
            this.mwToolStrip1.Name = "mwToolStrip1";
            this.mwToolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mwToolStrip1_ItemClicked);
            // 
            // menuStrip1
            // 
            this.menuStrip1.AccessibleDescription = null;
            this.menuStrip1.AccessibleName = null;
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.BackgroundImage = null;
            this.menuStrip1.Font = null;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.selectionToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.AccessibleDescription = null;
            this.fileToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.BackgroundImage = null;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMapMenuItem,
            this.openMapMenuItem,
            this.saveMapMenuItem,
            this.toolStripSeparator1,
            this.mnuAddLayer,
            this.toolStripSeparator2,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.layoutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // newMapMenuItem
            // 
            this.newMapMenuItem.AccessibleDescription = null;
            this.newMapMenuItem.AccessibleName = null;
            resources.ApplyResources(this.newMapMenuItem, "newMapMenuItem");
            this.newMapMenuItem.BackgroundImage = null;
            this.newMapMenuItem.Name = "newMapMenuItem";
            this.newMapMenuItem.ShortcutKeyDisplayString = null;
            this.newMapMenuItem.Click += new System.EventHandler(this.newMapMenuItem_Click);
            // 
            // openMapMenuItem
            // 
            this.openMapMenuItem.AccessibleDescription = null;
            this.openMapMenuItem.AccessibleName = null;
            resources.ApplyResources(this.openMapMenuItem, "openMapMenuItem");
            this.openMapMenuItem.BackgroundImage = null;
            this.openMapMenuItem.Name = "openMapMenuItem";
            this.openMapMenuItem.ShortcutKeyDisplayString = null;
            this.openMapMenuItem.Click += new System.EventHandler(this.openMapMenuItem_Click);
            // 
            // saveMapMenuItem
            // 
            this.saveMapMenuItem.AccessibleDescription = null;
            this.saveMapMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveMapMenuItem, "saveMapMenuItem");
            this.saveMapMenuItem.BackgroundImage = null;
            this.saveMapMenuItem.Name = "saveMapMenuItem";
            this.saveMapMenuItem.ShortcutKeyDisplayString = null;
            this.saveMapMenuItem.Click += new System.EventHandler(this.saveMapMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // mnuAddLayer
            // 
            this.mnuAddLayer.AccessibleDescription = null;
            this.mnuAddLayer.AccessibleName = null;
            resources.ApplyResources(this.mnuAddLayer, "mnuAddLayer");
            this.mnuAddLayer.BackgroundImage = null;
            this.mnuAddLayer.Name = "mnuAddLayer";
            this.mnuAddLayer.ShortcutKeyDisplayString = null;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.AccessibleDescription = null;
            this.settingsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.BackgroundImage = null;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.AccessibleDescription = null;
            this.toolStripMenuItem2.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.AccessibleDescription = null;
            this.layoutToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.layoutToolStripMenuItem, "layoutToolStripMenuItem");
            this.layoutToolStripMenuItem.BackgroundImage = null;
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.layoutToolStripMenuItem.Click += new System.EventHandler(this.layoutToolStripMenuItem_Click);
            // 
            // selectionToolStripMenuItem
            // 
            this.selectionToolStripMenuItem.AccessibleDescription = null;
            this.selectionToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.selectionToolStripMenuItem, "selectionToolStripMenuItem");
            this.selectionToolStripMenuItem.BackgroundImage = null;
            this.selectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSelectByAttributes});
            this.selectionToolStripMenuItem.Name = "selectionToolStripMenuItem";
            this.selectionToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // mnuSelectByAttributes
            // 
            this.mnuSelectByAttributes.AccessibleDescription = null;
            this.mnuSelectByAttributes.AccessibleName = null;
            resources.ApplyResources(this.mnuSelectByAttributes, "mnuSelectByAttributes");
            this.mnuSelectByAttributes.BackgroundImage = null;
            this.mnuSelectByAttributes.Name = "mnuSelectByAttributes";
            this.mnuSelectByAttributes.ShortcutKeyDisplayString = null;
            this.mnuSelectByAttributes.Click += new System.EventHandler(this.mnuSelectByAttributes_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.AccessibleDescription = null;
            this.helpToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.BackgroundImage = null;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // applicationManager1
            // 
            this.applicationManager1.DataManager.DataProviderDirectories = ((System.Collections.Generic.List<string>)(resources.GetObject("applicationManager1.DataManager.DataProviderDirectories")));
            this.applicationManager1.DataManager.LoadInRam = true;
            this.applicationManager1.DataManager.ProgressHandler = null;
            this.applicationManager1.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("applicationManager1.Directories")));
            this.applicationManager1.LayoutControl = null;
            this.applicationManager1.Legend = this.legend1;
            this.applicationManager1.MainMenu = this.menuStrip1;
            this.applicationManager1.MainToolStrip = this.mwToolStrip1;
            this.applicationManager1.Map = this.geoMap1;
            this.applicationManager1.ProgressHandler = this.mwStatusStrip1;
            this.applicationManager1.ToolManager = null;
            this.applicationManager1.ToolStripContainer = this.toolStripContainer1;
            // 
            // frmMapWindow6
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.mwStatusStrip1);
            this.Font = null;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMapWindow6";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabLegend.ResumeLayout(false);
            this.mwStatusStrip1.ResumeLayout(false);
            this.mwStatusStrip1.PerformLayout();
            this.tabToolbox.ResumeLayout(false);
            this.tabToolbox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

       

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem mnuAddLayer;
        private mwStatusStrip mwStatusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusMessage;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.ToolStripStatusLabel statusLocation;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private Map.Map geoMap1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLegend;
        private System.Windows.Forms.TabPage tabToolbox;
        private mwToolStrip mwToolStrip1;
        private Legend legend1;
        private System.Windows.Forms.ToolStripMenuItem selectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectByAttributes;
        private ApplicationManager applicationManager1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private Tools.ToolManager toolManager1;
        private Tools.ToolManagerToolStrip toolManagerToolStrip1;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem openMapMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveMapMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem newMapMenuItem;
        private System.ComponentModel.IContainer components;

    }
}

