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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/5/2009 12:49:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Windows.Forms;

namespace MapWindow.Forms
{
    /// <summary>
    /// ExportFeature
    /// </summary>
    public class ExportFeature : Form
    {
        private ComboBox cmbFeatureSpecification;
        private Label label1;
        private Label label2;
        private TextBox txtOutput;
        private Button btnBrowse;
        private Button btnCancel;
        private Button btnOK;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportFeature));
            this.cmbFeatureSpecification = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbFeatureSpecification
            // 
            this.cmbFeatureSpecification.AccessibleDescription = null;
            this.cmbFeatureSpecification.AccessibleName = null;
            resources.ApplyResources(this.cmbFeatureSpecification, "cmbFeatureSpecification");
            this.cmbFeatureSpecification.BackgroundImage = null;
            this.cmbFeatureSpecification.Font = null;
            this.cmbFeatureSpecification.FormattingEnabled = true;
            this.cmbFeatureSpecification.Name = "cmbFeatureSpecification";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // txtOutput
            // 
            this.txtOutput.AccessibleDescription = null;
            this.txtOutput.AccessibleName = null;
            resources.ApplyResources(this.txtOutput, "txtOutput");
            this.txtOutput.BackgroundImage = null;
            this.txtOutput.Font = null;
            this.txtOutput.Name = "txtOutput";
            // 
            // btnBrowse
            // 
            this.btnBrowse.AccessibleDescription = null;
            this.btnBrowse.AccessibleName = null;
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.BackgroundImage = null;
            this.btnBrowse.Font = null;
            this.btnBrowse.Image = global::MapWindow.Images.FolderOpen;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            // ExportFeature
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbFeatureSpecification);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportFeature";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ExportFeature
        /// </summary>
        public ExportFeature()
        {
            InitializeComponent();
            cmbFeatureSpecification.Items.Add(MessageStrings.FeaturesAll);
            cmbFeatureSpecification.Items.Add(MessageStrings.FeaturesSelected);
            cmbFeatureSpecification.Items.Add(MessageStrings.FeaturesExtent);
            cmbFeatureSpecification.SelectedIndex = 0;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = MapWindow.Components.DataManager.DefaultDataManager.VectorWriteFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            txtOutput.Text = ofd.FileName;

            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Gets the zero based integer index
        /// </summary>
        public int FeaturesIndex
        {
            get { return cmbFeatureSpecification.SelectedIndex; }
        }

        /// <summary>
        /// Gets or sets the string filename.  Setting this will not actually use this value,
        /// but will make up a new value based on the entered value.
        /// </summary>
        public string Filename
        {
            get { return txtOutput.Text; }
            set 
            {
                string baseFile = System.IO.Path.GetDirectoryName(value) +
                    System.IO.Path.DirectorySeparatorChar +
                    System.IO.Path.GetFileNameWithoutExtension(value);
                int i = 1;
                string outFile = value;
                string ext = System.IO.Path.GetExtension(value);
               
                while (System.IO.File.Exists(outFile))
                {
                   outFile = baseFile + i.ToString() + ext;
                   i++;
                }
                txtOutput.Text = outFile;
            }
        }
       
    }
}