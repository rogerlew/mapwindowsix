//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core assembly for the MapWindow 6.0 distribution.
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/5/2009 1:29:17 PM
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
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Components;

namespace MapWindow.Forms
{
    /// <summary>
    /// AddToMapDialog
    /// </summary>
    public class AddToMapDialog : Form
    {
        private RadioButton radYes;
        private Label lblExport;
        private RadioButton radNo;
        private Button btnOK;
        private Button Cancel;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #endregion


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddToMapDialog));
            this.radYes = new System.Windows.Forms.RadioButton();
            this.lblExport = new System.Windows.Forms.Label();
            this.radNo = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radYes
            // 
            this.radYes.AccessibleDescription = null;
            this.radYes.AccessibleName = null;
            resources.ApplyResources(this.radYes, "radYes");
            this.radYes.BackgroundImage = null;
            this.radYes.Checked = true;
            this.radYes.Font = null;
            this.radYes.Name = "radYes";
            this.radYes.TabStop = true;
            this.radYes.UseVisualStyleBackColor = true;
            // 
            // lblExport
            // 
            this.lblExport.AccessibleDescription = null;
            this.lblExport.AccessibleName = null;
            resources.ApplyResources(this.lblExport, "lblExport");
            this.lblExport.Font = null;
            this.lblExport.Name = "lblExport";
            // 
            // radNo
            // 
            this.radNo.AccessibleDescription = null;
            this.radNo.AccessibleName = null;
            resources.ApplyResources(this.radNo, "radNo");
            this.radNo.BackgroundImage = null;
            this.radNo.Font = null;
            this.radNo.Name = "radNo";
            this.radNo.TabStop = true;
            this.radNo.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Cancel
            // 
            this.Cancel.AccessibleDescription = null;
            this.Cancel.AccessibleName = null;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.BackgroundImage = null;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = null;
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // AddToMapDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.radNo);
            this.Controls.Add(this.lblExport);
            this.Controls.Add(this.radYes);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddToMapDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AddToMapDialog
        /// </summary>
        public AddToMapDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Boolean, true if a new layer should be loaded from the recently exported data.
        /// </summary>
        public bool AddLayer
        {
            get
            {
                return radYes.Checked;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

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

        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}