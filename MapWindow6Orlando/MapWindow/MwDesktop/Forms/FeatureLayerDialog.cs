//********************************************************************************************************
// Product Name: MapWindow.Forms.dll Alpha
// Description:  The basic module for MapWindow.Forms version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Forms.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/18/2009 3:36:37 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Drawing;

namespace MapWindow.Forms
{


    /// <summary>
    /// This is a basic form which is displayed when the user double-clicks on a layer name
    /// in the legend
    /// </summary>
    public class FeatureLayerDialog : Form
    {
        private TabControl tabControl1;
        private TabPage tabSymbology;
        private Panel pnlContent;
        private PropertyGrid propertyGrid1;
        private TabPage tabDetails;
        private FeatureCategoryControl _featureCategoryControl;
        private Panel panel1;
        private DialogButtons dialogButtons1;
       

        #region Events
        /// <summary>
        /// Occurs when the apply changes situation forces the symbology to become updated.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Private Variables

        private IFeatureLayer _layer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LayerDialog
        /// </summary>
        public FeatureLayerDialog()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of LayerDialog form to display the symbology and
        /// other properties of the specified feature layer
        /// </summary>
        /// <param name="selectedLayer">the specified feature layer that is
        /// modified using this form</param>
        public FeatureLayerDialog(IFeatureLayer selectedLayer)
        {
            InitializeComponent();

            _layer = selectedLayer;
            propertyGrid1.SelectedObject = _layer;
            Configure();
        }

        private void Configure()
        {
            _featureCategoryControl = new FeatureCategoryControl();
            _featureCategoryControl.Parent = pnlContent;
            _featureCategoryControl.Visible = true;
            _featureCategoryControl.Initialize(_layer);
        }

       

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion

        #region Event Handlers


        #endregion

        #region Protected Methods

       

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureLayerDialog));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSymbology = new System.Windows.Forms.TabPage();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dialogButtons1 = new MapWindow.Components.DialogButtons();
            this.tabControl1.SuspendLayout();
            this.tabSymbology.SuspendLayout();
            this.tabDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleDescription = null;
            this.tabControl1.AccessibleName = null;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.BackgroundImage = null;
            this.tabControl1.Controls.Add(this.tabSymbology);
            this.tabControl1.Controls.Add(this.tabDetails);
            this.tabControl1.Font = null;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabSymbology
            // 
            this.tabSymbology.AccessibleDescription = null;
            this.tabSymbology.AccessibleName = null;
            resources.ApplyResources(this.tabSymbology, "tabSymbology");
            this.tabSymbology.BackgroundImage = null;
            this.tabSymbology.Controls.Add(this.pnlContent);
            this.tabSymbology.Font = null;
            this.tabSymbology.Name = "tabSymbology";
            this.tabSymbology.UseVisualStyleBackColor = true;
            // 
            // pnlContent
            // 
            this.pnlContent.AccessibleDescription = null;
            this.pnlContent.AccessibleName = null;
            resources.ApplyResources(this.pnlContent, "pnlContent");
            this.pnlContent.BackgroundImage = null;
            this.pnlContent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlContent.Font = null;
            this.pnlContent.Name = "pnlContent";
            // 
            // tabDetails
            // 
            this.tabDetails.AccessibleDescription = null;
            this.tabDetails.AccessibleName = null;
            resources.ApplyResources(this.tabDetails, "tabDetails");
            this.tabDetails.BackgroundImage = null;
            this.tabDetails.Controls.Add(this.propertyGrid1);
            this.tabDetails.Font = null;
            this.tabDetails.Name = "tabDetails";
            this.tabDetails.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleDescription = null;
            this.propertyGrid1.AccessibleName = null;
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.BackgroundImage = null;
            this.propertyGrid1.Font = null;
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.dialogButtons1);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // dialogButtons1
            // 
            this.dialogButtons1.AccessibleDescription = null;
            this.dialogButtons1.AccessibleName = null;
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.BackgroundImage = null;
            this.dialogButtons1.Font = null;
            this.dialogButtons1.Name = "dialogButtons1";
            this.dialogButtons1.OkClicked += new System.EventHandler(this.dialogButtons1_OkClicked);
            this.dialogButtons1.ApplyClicked += new System.EventHandler(this.dialogButtons1_ApplyClicked);
            this.dialogButtons1.CancelClicked += new System.EventHandler(this.dialogButtons1_CancelClicked);
            // 
            // FeatureLayerDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureLayerDialog";
            this.ShowInTaskbar = false;
            this.tabControl1.ResumeLayout(false);
            this.tabSymbology.ResumeLayout(false);
            this.tabDetails.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void dialogButtons1_ApplyClicked(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        void dialogButtons1_CancelClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion

      
       

       
        /// <summary>
        /// Forces changes to be written from the copy symbology to
        /// the original, updating the map display.
        /// </summary>
        public void ApplyChanges()
        {
            OnApplyChanges();
        }

        /// <summary>
        /// Occurs during apply changes operations and is overrideable in subclasses
        /// </summary>
        protected virtual void OnApplyChanges()
        {
             _featureCategoryControl.ApplyChanges();
            if(ChangesApplied != null)ChangesApplied(_layer, new EventArgs());
        }

        private void dialogButtons1_OkClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            OnApplyChanges();
            Close();
        }

    }
}
