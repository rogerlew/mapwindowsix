//********************************************************************************************************
// Product Name: MapWindow.Tools.frmModeler
// Description:  A form which contains the modeler component
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Apr, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapWindow
{
    /// <summary>
    /// A form used in Brian's toolkit code
    /// </summary>
    public class frmModeler : Form
    {
        #region "Private variables"

        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private MapWindow.Tools.ModelerMenuStrip _modelerMenuStrip;
        private MapWindow.Tools.ModelerToolStrip _modelerToolStrip;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItem;
        private MapWindow.Tools.Modeler _modeler;

        #endregion

        /// <summary>
        /// Creates a new instance of the modeler's form
        /// </summary>
        public frmModeler()
        {
            InitializeComponent();

            _modeler.ModelFilenameChanged += new EventHandler(_modeler_ModelFilenameChanged);
        }

        #region properties

        /// <summary>
        /// Gets modeler in the form
        /// </summary>
        public MapWindow.Tools.Modeler Modeler
        {
            get { return _modeler; }
        }

        #endregion

        #region Events

            void _modeler_ModelFilenameChanged(object sender, EventArgs e)
            {
                this.Text = "MapWindow Modeler - " + System.IO.Path.GetFileNameWithoutExtension(_modeler.ModelFilename);
            }

        #endregion

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModeler));
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._modeler = new MapWindow.Tools.Modeler();
            this._modelerMenuStrip = new MapWindow.Tools.ModelerMenuStrip();
            this._modelerToolStrip = new MapWindow.Tools.ModelerToolStrip();
            this._toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripContainer.ContentPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStripContainer
            // 
            this._toolStripContainer.AccessibleDescription = null;
            this._toolStripContainer.AccessibleName = null;
            resources.ApplyResources(this._toolStripContainer, "_toolStripContainer");
            // 
            // _toolStripContainer.BottomToolStripPanel
            // 
            this._toolStripContainer.BottomToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer.BottomToolStripPanel.AccessibleName = null;
            this._toolStripContainer.BottomToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer.BottomToolStripPanel, "_toolStripContainer.BottomToolStripPanel");
            this._toolStripContainer.BottomToolStripPanel.Font = null;
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.AccessibleDescription = null;
            this._toolStripContainer.ContentPanel.AccessibleName = null;
            resources.ApplyResources(this._toolStripContainer.ContentPanel, "_toolStripContainer.ContentPanel");
            this._toolStripContainer.ContentPanel.BackgroundImage = null;
            this._toolStripContainer.ContentPanel.Controls.Add(this._modeler);
            this._toolStripContainer.ContentPanel.Font = null;
            this._toolStripContainer.Font = null;
            // 
            // _toolStripContainer.LeftToolStripPanel
            // 
            this._toolStripContainer.LeftToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer.LeftToolStripPanel.AccessibleName = null;
            this._toolStripContainer.LeftToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer.LeftToolStripPanel, "_toolStripContainer.LeftToolStripPanel");
            this._toolStripContainer.LeftToolStripPanel.Font = null;
            this._toolStripContainer.Name = "_toolStripContainer";
            // 
            // _toolStripContainer.RightToolStripPanel
            // 
            this._toolStripContainer.RightToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer.RightToolStripPanel.AccessibleName = null;
            this._toolStripContainer.RightToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer.RightToolStripPanel, "_toolStripContainer.RightToolStripPanel");
            this._toolStripContainer.RightToolStripPanel.Font = null;
            // 
            // _toolStripContainer.TopToolStripPanel
            // 
            this._toolStripContainer.TopToolStripPanel.AccessibleDescription = null;
            this._toolStripContainer.TopToolStripPanel.AccessibleName = null;
            this._toolStripContainer.TopToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this._toolStripContainer.TopToolStripPanel, "_toolStripContainer.TopToolStripPanel");
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this._modelerToolStrip);
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this._modelerMenuStrip);
            this._toolStripContainer.TopToolStripPanel.Font = null;
            // 
            // _modeler
            // 
            this._modeler.AccessibleDescription = null;
            this._modeler.AccessibleName = null;
            this._modeler.AllowDrop = true;
            resources.ApplyResources(this._modeler, "_modeler");
            this._modeler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this._modeler.BackgroundImage = null;
            this._modeler.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._modeler.Cursor = System.Windows.Forms.Cursors.Default;
            this._modeler.DataColor = System.Drawing.Color.LightGreen;
            this._modeler.DataFont = new System.Drawing.Font("Tahoma", 8F);
            this._modeler.DataShape = MapWindow.Tools.ModelShapes.Ellipse;
            this._modeler.DefaultFileExtension = "mwm";
            this._modeler.DrawingQuality = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this._modeler.EnableLinking = false;
            this._modeler.Font = null;
            this._modeler.IsInitialized = true;
            this._modeler.MaxExecutionThreads = 2;
            this._modeler.ModelFilename = null;
            this._modeler.Name = "_modeler";
            this._modeler.ShowWaterMark = true;
            this._modeler.ToolColor = System.Drawing.Color.Khaki;
            this._modeler.ToolFont = new System.Drawing.Font("Tahoma", 8F);
            this._modeler.ToolManager = null;
            this._modeler.ToolShape = MapWindow.Tools.ModelShapes.Rectangle;
            this._modeler.WorkingPath = null;
            this._modeler.ZoomFactor = 1F;
            // 
            // _modelerMenuStrip
            // 
            this._modelerMenuStrip.AccessibleDescription = null;
            this._modelerMenuStrip.AccessibleName = null;
            resources.ApplyResources(this._modelerMenuStrip, "_modelerMenuStrip");
            this._modelerMenuStrip.BackgroundImage = null;
            this._modelerMenuStrip.Font = null;
            this._modelerMenuStrip.Name = "_modelerMenuStrip";
            // 
            // _modelerToolStrip
            // 
            this._modelerToolStrip.AccessibleDescription = null;
            this._modelerToolStrip.AccessibleName = null;
            resources.ApplyResources(this._modelerToolStrip, "_modelerToolStrip");
            this._modelerToolStrip.BackgroundImage = null;
            this._modelerToolStrip.Font = null;
            this._modelerToolStrip.Modeler = this._modeler;
            this._modelerToolStrip.Name = "_modelerToolStrip";
            // 
            // _toolStripMenuItem
            // 
            this._toolStripMenuItem.AccessibleDescription = null;
            this._toolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this._toolStripMenuItem, "_toolStripMenuItem");
            this._toolStripMenuItem.BackgroundImage = null;
            this._toolStripMenuItem.Name = "_toolStripMenuItem";
            this._toolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // frmModeler
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this._toolStripContainer);
            this.Font = null;
            this.Icon = global::MapWindow.Images.NewModel;
            this.Name = "frmModeler";
            this._toolStripContainer.ContentPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
