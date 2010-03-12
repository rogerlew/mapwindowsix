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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 5:10:05 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Windows.Forms;
using MapWindow.Data;
using MapWindow.Geometries;
namespace MapWindow.ShapeEditor
{
    /// <summary>
    /// FeatureTypeDialog
    /// </summary>
    public class FeatureTypeDialog : Form
    {
        private Button btnOk;
        private Button btnCancel;
        private ComboBox cmbFeatureType;
        private CheckBox chkM;
        private CheckBox chkZ;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureTypeDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbFeatureType = new System.Windows.Forms.ComboBox();
            this.chkM = new System.Windows.Forms.CheckBox();
            this.chkZ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbFeatureType
            // 
            resources.ApplyResources(this.cmbFeatureType, "cmbFeatureType");
            this.cmbFeatureType.FormattingEnabled = true;
            this.cmbFeatureType.Items.AddRange(new object[] {
            resources.GetString("cmbFeatureType.Items"),
            resources.GetString("cmbFeatureType.Items1"),
            resources.GetString("cmbFeatureType.Items2"),
            resources.GetString("cmbFeatureType.Items3")});
            this.cmbFeatureType.Name = "cmbFeatureType";
            this.cmbFeatureType.SelectedIndexChanged += new System.EventHandler(this.cmbFeatureType_SelectedIndexChanged);
            // 
            // chkM
            // 
            resources.ApplyResources(this.chkM, "chkM");
            this.chkM.Name = "chkM";
            this.chkM.UseVisualStyleBackColor = true;
            // 
            // chkZ
            // 
            resources.ApplyResources(this.chkZ, "chkZ");
            this.chkZ.Name = "chkZ";
            this.chkZ.UseVisualStyleBackColor = true;
            // 
            // FeatureTypeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkZ);
            this.Controls.Add(this.chkM);
            this.Controls.Add(this.cmbFeatureType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureTypeDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureTypeDialog
        /// </summary>
        public FeatureTypeDialog()
        {
            InitializeComponent();
            cmbFeatureType.SelectedIndex = 0;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the feature type chosen by this dialog
        /// </summary>
        public FeatureTypes FeatureType
        {
            get
            {
                switch (cmbFeatureType.SelectedIndex)
                {
                    case 0: return FeatureTypes.Point;
                    case 1: return FeatureTypes.Line;
                    case 2: return FeatureTypes.Polygon;
                    case 3:
                        return FeatureTypes.MultiPoint;
                }
                return FeatureTypes.Unspecified;
            }
        }

        /// <summary>
        /// Gets the Coordinate type for this dialog
        /// </summary>
        public CoordinateTypes CoordinateType
        {
            get
            {
                if (chkZ.Checked) return CoordinateTypes.Z;
                if (chkM.Checked) return CoordinateTypes.M;
                return CoordinateTypes.Regular;
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbFeatureType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}