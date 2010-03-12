//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in Fall 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
// Jiri Kadlec (2009-10-30) The attribute Table editor has been moved to a separate user control
//********************************************************************************************************

using System;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Map;

namespace MapWindow.Forms
{
    /// <summary>
    /// Atrribute Table editor form
    /// </summary>
    public class AttributeDialog : Form
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttributeDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.tableEditorControl1 = new MapWindow.Components.AttributeTable.TableEditorControl();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.Font = null;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // tableEditorControl1
            // 
            this.tableEditorControl1.AccessibleDescription = null;
            this.tableEditorControl1.AccessibleName = null;
            resources.ApplyResources(this.tableEditorControl1, "tableEditorControl1");
            this.tableEditorControl1.BackgroundImage = null;
            this.tableEditorControl1.Font = null;
            this.tableEditorControl1.IgnoreSelectionChanged = false;
            this.tableEditorControl1.IsEditable = true;
            this.tableEditorControl1.Name = "tableEditorControl1";
            this.tableEditorControl1.ShowFileName = true;
            this.tableEditorControl1.ShowMenuStrip = true;
            this.tableEditorControl1.ShowProgressBar = false;
            this.tableEditorControl1.ShowSelectedRowsOnly = false;
            this.tableEditorControl1.ShowToolStrip = true;
            // 
            // AttributeDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tableEditorControl1);
            this.Font = null;
            this.Name = "AttributeDialog";
            this.ResumeLayout(false);

        }

        #endregion

        #region Variables

        private MapWindow.Components.AttributeTable.TableEditorControl tableEditorControl1;
        private System.Windows.Forms.Button btnClose;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Creates a new instance of the attribute Table editor form
        /// <param name="featureLayer">The feature layer associated with
        /// this instance and displayed in the editor</param>
        /// </summary>
        public AttributeDialog(IFeatureLayer featureLayer)
        {
            InitializeComponent();
            IMapFeatureLayer mapLayer = featureLayer as IMapFeatureLayer;
            if (mapLayer != null)
            {
                tableEditorControl1.FeatureLayer = mapLayer;
            }
        }

        #endregion

        #region Event Handlers
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Protected Methods
        
        #endregion

        
    }
}
