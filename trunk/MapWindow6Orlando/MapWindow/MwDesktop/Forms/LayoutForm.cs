//********************************************************************************************************
// Product Name: MapWindow.Forms.LayoutForm
// Description:  A form that shows the mapwindow layout
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is by Brian Marchionni Aug 2009
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
//********************************************************************************************************

using System;
using System.Windows.Forms;

namespace MapWindow.Forms
{
    /// <summary>
    /// This is the primary form where the print layout content is organized before printing
    /// </summary>
    public class LayoutForm : Form
    {

        private ToolStripContainer _toolStripContainer1;
        private SplitContainer _splitContainer1;
        private Layout.LayoutPropertyGrid _layoutPropertyGrid1;
        private SplitContainer _splitContainer2;
        private Layout.LayoutDocToolStrip _layoutDocToolStrip1;
        private Layout.LayoutControl _layoutControl1;
        private Layout.LayoutListBox _layoutListBox1;
        private Layout.LayoutMenuStrip _layoutMenuStrip1;
        private Layout.LayoutInsertToolStrip _layoutInsertToolStrip1;
        private Layout.LayoutZoomToolStrip _layoutZoomToolStrip1;
        private Layout.LayoutMapToolStrip _layoutMapToolStrip1;

        /// <summary>
        /// Default constructor for creating a new instance of hte Layout form
        /// </summary>
        public LayoutForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the map that will be used in the layout
        /// </summary>
        public Map.Map MapControl
        {
            get { return _layoutControl1.MapControl; }
            set { _layoutControl1.MapControl = value; }
        }

        #region Windows Form Designer generated code

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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutForm));
            this._toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._layoutControl1 = new MapWindow.Layout.LayoutControl();
            this._layoutDocToolStrip1 = new MapWindow.Layout.LayoutDocToolStrip();
            this._layoutInsertToolStrip1 = new MapWindow.Layout.LayoutInsertToolStrip();
            this._layoutListBox1 = new MapWindow.Layout.LayoutListBox();
            this._layoutMapToolStrip1 = new MapWindow.Layout.LayoutMapToolStrip();
            this._layoutMenuStrip1 = new MapWindow.Layout.LayoutMenuStrip();
            this._layoutPropertyGrid1 = new MapWindow.Layout.LayoutPropertyGrid();
            this._layoutZoomToolStrip1 = new MapWindow.Layout.LayoutZoomToolStrip();
            this._splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._toolStripContainer1.ContentPanel.SuspendLayout();
            this._toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer1.SuspendLayout();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            this._splitContainer2.Panel1.SuspendLayout();
            this._splitContainer2.Panel2.SuspendLayout();
            this._splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStripContainer1
            // 
            this._toolStripContainer1.AccessibleDescription = null;
            this._toolStripContainer1.AccessibleName = null;
            resources.ApplyResources(this._toolStripContainer1, "_toolStripContainer1");
            // 
            // _toolStripContainer1.BottomToolStripPanel
            // 
            this._toolStripContainer1.BottomToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer1.BottomToolStripPanel.AccessibleName = null;
            this._toolStripContainer1.BottomToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer1.BottomToolStripPanel, "_toolStripContainer1.BottomToolStripPanel");
            this._toolStripContainer1.BottomToolStripPanel.Font = null;
            // 
            // _toolStripContainer1.ContentPanel
            // 
            this._toolStripContainer1.ContentPanel.AccessibleDescription = null;
            this._toolStripContainer1.ContentPanel.AccessibleName = null;
            resources.ApplyResources(this._toolStripContainer1.ContentPanel, "_toolStripContainer1.ContentPanel");
            this._toolStripContainer1.ContentPanel.BackgroundImage = null;
            this._toolStripContainer1.ContentPanel.Controls.Add(this._splitContainer1);
            this._toolStripContainer1.ContentPanel.Font = null;
            this._toolStripContainer1.Font = null;
            // 
            // _toolStripContainer1.LeftToolStripPanel
            // 
            this._toolStripContainer1.LeftToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer1.LeftToolStripPanel.AccessibleName = null;
            this._toolStripContainer1.LeftToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer1.LeftToolStripPanel, "_toolStripContainer1.LeftToolStripPanel");
            this._toolStripContainer1.LeftToolStripPanel.Font = null;
            this._toolStripContainer1.Name = "_toolStripContainer1";
            // 
            // _toolStripContainer1.RightToolStripPanel
            // 
            this._toolStripContainer1.RightToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer1.RightToolStripPanel.AccessibleName = null;
            this._toolStripContainer1.RightToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer1.RightToolStripPanel, "_toolStripContainer1.RightToolStripPanel");
            this._toolStripContainer1.RightToolStripPanel.Font = null;
            // 
            // _toolStripContainer1.TopToolStripPanel
            // 
            this._toolStripContainer1.TopToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer1.TopToolStripPanel.AccessibleName = null;
            this._toolStripContainer1.TopToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer1.TopToolStripPanel, "_toolStripContainer1.TopToolStripPanel");
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutDocToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutInsertToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutZoomToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutMapToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Font = null;
            // 
            // _splitContainer1
            // 
            this._splitContainer1.AccessibleDescription = null;
            this._splitContainer1.AccessibleName = null;
            resources.ApplyResources(this._splitContainer1, "_splitContainer1");
            this._splitContainer1.BackgroundImage = null;
            this._splitContainer1.Font = null;
            this._splitContainer1.Name = "_splitContainer1";
            // 
            // _splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.AccessibleDescription = null;
            this._splitContainer1.Panel1.AccessibleName = null;
            resources.ApplyResources(this._splitContainer1.Panel1, "_splitContainer1.Panel1");
            this._splitContainer1.Panel1.BackgroundImage = null;
            this._splitContainer1.Panel1.Controls.Add(this._layoutControl1);
            this._splitContainer1.Panel1.Font = null;
            // 
            // _splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.AccessibleDescription = null;
            this._splitContainer1.Panel2.AccessibleName = null;
            resources.ApplyResources(this._splitContainer1.Panel2, "_splitContainer1.Panel2");
            this._splitContainer1.Panel2.BackgroundImage = null;
            this._splitContainer1.Panel2.Controls.Add(this._splitContainer2);
            this._splitContainer1.Panel2.Font = null;
            // 
            // _layoutControl1
            // 
            this._layoutControl1.AccessibleDescription = null;
            this._layoutControl1.AccessibleName = null;
            resources.ApplyResources(this._layoutControl1, "_layoutControl1");
            this._layoutControl1.BackgroundImage = null;
            this._layoutControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this._layoutControl1.DrawingQuality = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this._layoutControl1.Filename = "";
            this._layoutControl1.Font = null;
            this._layoutControl1.LayoutDocToolStrip = this._layoutDocToolStrip1;
            this._layoutControl1.LayoutInsertToolStrip = this._layoutInsertToolStrip1;
            this._layoutControl1.LayoutListBox = this._layoutListBox1;
            this._layoutControl1.LayoutMapToolStrip = this._layoutMapToolStrip1;
            this._layoutControl1.LayoutMenuStrip = this._layoutMenuStrip1;
            this._layoutControl1.LayoutPropertyGrip = this._layoutPropertyGrid1;
            this._layoutControl1.LayoutZoomToolStrip = this._layoutZoomToolStrip1;
            this._layoutControl1.MapControl = null;
            this._layoutControl1.MapPanMode = false;
            this._layoutControl1.Name = "_layoutControl1";
            this._layoutControl1.PrinterSettings = ((System.Drawing.Printing.PrinterSettings)(resources.GetObject("_layoutControl1.PrinterSettings")));
            this._layoutControl1.ShowMargin = false;
            this._layoutControl1.Zoom = 0.3541667F;
            this._layoutControl1.FilenameChanged += new System.EventHandler(this.layoutControl1_FilenameChanged);
            // 
            // _layoutDocToolStrip1
            // 
            this._layoutDocToolStrip1.AccessibleDescription = null;
            this._layoutDocToolStrip1.AccessibleName = null;
            resources.ApplyResources(this._layoutDocToolStrip1, "_layoutDocToolStrip1");
            this._layoutDocToolStrip1.BackgroundImage = null;
            this._layoutDocToolStrip1.Font = null;
            this._layoutDocToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutDocToolStrip1.Name = "_layoutDocToolStrip1";
            // 
            // _layoutInsertToolStrip1
            // 
            this._layoutInsertToolStrip1.AccessibleDescription = null;
            this._layoutInsertToolStrip1.AccessibleName = null;
            resources.ApplyResources(this._layoutInsertToolStrip1, "_layoutInsertToolStrip1");
            this._layoutInsertToolStrip1.BackgroundImage = null;
            this._layoutInsertToolStrip1.Font = null;
            this._layoutInsertToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutInsertToolStrip1.Name = "_layoutInsertToolStrip1";
            // 
            // _layoutListBox1
            // 
            this._layoutListBox1.AccessibleDescription = null;
            this._layoutListBox1.AccessibleName = null;
            resources.ApplyResources(this._layoutListBox1, "_layoutListBox1");
            this._layoutListBox1.BackgroundImage = null;
            this._layoutListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._layoutListBox1.Font = null;
            this._layoutListBox1.LayoutControl = this._layoutControl1;
            this._layoutListBox1.Name = "_layoutListBox1";
            // 
            // _layoutMapToolStrip1
            // 
            this._layoutMapToolStrip1.AccessibleDescription = null;
            this._layoutMapToolStrip1.AccessibleName = null;
            resources.ApplyResources(this._layoutMapToolStrip1, "_layoutMapToolStrip1");
            this._layoutMapToolStrip1.BackgroundImage = null;
            this._layoutMapToolStrip1.Font = null;
            this._layoutMapToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutMapToolStrip1.Name = "_layoutMapToolStrip1";
            // 
            // _layoutMenuStrip1
            // 
            this._layoutMenuStrip1.AccessibleDescription = null;
            this._layoutMenuStrip1.AccessibleName = null;
            resources.ApplyResources(this._layoutMenuStrip1, "_layoutMenuStrip1");
            this._layoutMenuStrip1.BackgroundImage = null;
            this._layoutMenuStrip1.Font = null;
            this._layoutMenuStrip1.LayoutControl = this._layoutControl1;
            this._layoutMenuStrip1.Name = "_layoutMenuStrip1";
            this._layoutMenuStrip1.CloseClicked += new System.EventHandler(this.layoutMenuStrip1_CloseClicked);
            // 
            // _layoutPropertyGrid1
            // 
            this._layoutPropertyGrid1.AccessibleDescription = null;
            this._layoutPropertyGrid1.AccessibleName = null;
            resources.ApplyResources(this._layoutPropertyGrid1, "_layoutPropertyGrid1");
            this._layoutPropertyGrid1.BackgroundImage = null;
            this._layoutPropertyGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._layoutPropertyGrid1.Font = null;
            this._layoutPropertyGrid1.LayoutControl = this._layoutControl1;
            this._layoutPropertyGrid1.Name = "_layoutPropertyGrid1";
            // 
            // _layoutZoomToolStrip1
            // 
            this._layoutZoomToolStrip1.AccessibleDescription = null;
            this._layoutZoomToolStrip1.AccessibleName = null;
            resources.ApplyResources(this._layoutZoomToolStrip1, "_layoutZoomToolStrip1");
            this._layoutZoomToolStrip1.BackgroundImage = null;
            this._layoutZoomToolStrip1.Font = null;
            this._layoutZoomToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutZoomToolStrip1.Name = "_layoutZoomToolStrip1";
            // 
            // _splitContainer2
            // 
            this._splitContainer2.AccessibleDescription = null;
            this._splitContainer2.AccessibleName = null;
            resources.ApplyResources(this._splitContainer2, "_splitContainer2");
            this._splitContainer2.BackgroundImage = null;
            this._splitContainer2.Font = null;
            this._splitContainer2.Name = "_splitContainer2";
            // 
            // _splitContainer2.Panel1
            // 
            this._splitContainer2.Panel1.AccessibleDescription = null;
            this._splitContainer2.Panel1.AccessibleName = null;
            resources.ApplyResources(this._splitContainer2.Panel1, "_splitContainer2.Panel1");
            this._splitContainer2.Panel1.BackgroundImage = null;
            this._splitContainer2.Panel1.Controls.Add(this._layoutListBox1);
            this._splitContainer2.Panel1.Font = null;
            // 
            // _splitContainer2.Panel2
            // 
            this._splitContainer2.Panel2.AccessibleDescription = null;
            this._splitContainer2.Panel2.AccessibleName = null;
            resources.ApplyResources(this._splitContainer2.Panel2, "_splitContainer2.Panel2");
            this._splitContainer2.Panel2.BackgroundImage = null;
            this._splitContainer2.Panel2.Controls.Add(this._layoutPropertyGrid1);
            this._splitContainer2.Panel2.Font = null;
            // 
            // LayoutForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this._toolStripContainer1);
            this.Controls.Add(this._layoutMenuStrip1);
            this.Font = null;
            this.ForeColor = System.Drawing.Color.Coral;
            this.Name = "LayoutForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LayoutForm_FormClosing);
            this._toolStripContainer1.ContentPanel.ResumeLayout(false);
            this._toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer1.TopToolStripPanel.PerformLayout();
            this._toolStripContainer1.ResumeLayout(false);
            this._toolStripContainer1.PerformLayout();
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel2.ResumeLayout(false);
            this._splitContainer1.ResumeLayout(false);
            this._splitContainer2.Panel1.ResumeLayout(false);
            this._splitContainer2.Panel2.ResumeLayout(false);
            this._splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void layoutMenuStrip1_CloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        void LayoutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _layoutControl1.ClearLayout();
        }

        void layoutControl1_FilenameChanged(object sender, EventArgs e)
        {
            if (this._layoutControl1.Filename.Length > 0) 
                this.Text = "MapWindow Print Layout - " + System.IO.Path.GetFileName(this._layoutControl1.Filename);
            else
                this.Text = "MapWindow Print Layout";
        }

        #endregion

    }
}
