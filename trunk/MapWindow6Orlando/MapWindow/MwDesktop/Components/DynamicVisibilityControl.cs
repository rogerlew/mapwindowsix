//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2009 4:59:03 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using System.Drawing;
using MapWindow.Drawing;
using System.Windows.Forms.Design;
using MapWindow.Geometries;
namespace MapWindow.Components
{


    /// <summary>
    /// DynamicVisibilityControl
    /// </summary>
    [ToolboxBitmap(typeof(DoubleBox), "UserControl.ico")]
    public class DynamicVisibilityControl : UserControl
    {
        

        
        #region Private Variables

        private bool _useDynamicVisibility;
        private double _dynamicVisibilityWidth;
        private ILayer _layer;
        private IEnvelope _grabExtents;

        private CheckBox chkUseDynamicVisibility;
        private Button btnGrabExtents;
        private IWindowsFormsEditorService _dialogProvider;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DynamicVisibilityControl.  Note,
        /// this default constructor won't be able to grab the extents
        /// from a layer, but instead will use the "grab extents"
        /// </summary>
        public DynamicVisibilityControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The true constructor
        /// </summary>
        /// <param name="dialogProvider">Service that may have launched this control</param>
        /// <param name="layer">the layer that this property is being adjusted on</param>
        public DynamicVisibilityControl(IWindowsFormsEditorService dialogProvider, ILayer layer)
        {
            _dialogProvider = dialogProvider;
            _useDynamicVisibility = layer.UseDynamicVisibility;
            _dynamicVisibilityWidth = layer.DynamicVisibilityWidth;
            _layer = layer;
            InitializeComponent();
        }

        #endregion


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicVisibilityControl));
            this.chkUseDynamicVisibility = new System.Windows.Forms.CheckBox();
            this.btnGrabExtents = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkUseDynamicVisibility
            // 
            this.chkUseDynamicVisibility.AccessibleDescription = null;
            this.chkUseDynamicVisibility.AccessibleName = null;
            resources.ApplyResources(this.chkUseDynamicVisibility, "chkUseDynamicVisibility");
            this.chkUseDynamicVisibility.BackgroundImage = null;
            this.chkUseDynamicVisibility.Font = null;
            this.chkUseDynamicVisibility.Name = "chkUseDynamicVisibility";
            this.chkUseDynamicVisibility.UseVisualStyleBackColor = true;
            this.chkUseDynamicVisibility.CheckedChanged += new System.EventHandler(this.chkUseDynamicVisibility_CheckedChanged);
            // 
            // btnGrabExtents
            // 
            this.btnGrabExtents.AccessibleDescription = null;
            this.btnGrabExtents.AccessibleName = null;
            resources.ApplyResources(this.btnGrabExtents, "btnGrabExtents");
            this.btnGrabExtents.BackgroundImage = null;
            this.btnGrabExtents.Font = null;
            this.btnGrabExtents.Name = "btnGrabExtents";
            this.btnGrabExtents.UseVisualStyleBackColor = true;
            this.btnGrabExtents.Click += new System.EventHandler(this.btnGrabExtents_Click);
            // 
            // DynamicVisibilityControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnGrabExtents);
            this.Controls.Add(this.chkUseDynamicVisibility);
            this.Font = null;
            this.Name = "DynamicVisibilityControl";
            this.ResumeLayout(false);

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

       

        /// <summary>
        /// Gets or sets the geographic width where the layer content becomes visible again.
        /// </summary>
        public double DynamicVisibilityWidth
        {
            get { return _dynamicVisibilityWidth; }
            set { _dynamicVisibilityWidth = value; }
        }

        /// <summary>
        /// If a layer is not provided, the DynamicVisibilityExtents
        /// will be set to the grab extents instead.
        /// </summary>
        public IEnvelope GrabExtents
        {
            get { return _grabExtents; }
            set { _grabExtents = value; }
        }

        /// <summary>
        /// Gets or sets a boolean corresponding 
        /// </summary>
        public bool UseDynamicVisibility
        {
            get { return _useDynamicVisibility; }
            set { _useDynamicVisibility = value; }
        }

        #endregion

        #region Protected Methods

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

        private void chkUseDynamicVisibility_CheckedChanged(object sender, EventArgs e)
        {
            _useDynamicVisibility = chkUseDynamicVisibility.Checked;
            if (_layer != null) _layer.UseDynamicVisibility = chkUseDynamicVisibility.Checked;
        }

        private void btnGrabExtents_Click(object sender, EventArgs e)
        {
            if (_layer != null)
            {
                
                _dynamicVisibilityWidth = _layer.MapFrame.Extents.Width;
                _layer.DynamicVisibilityWidth = _dynamicVisibilityWidth;
                _layer.UseDynamicVisibility = true;
            }
            else
            {
                _dynamicVisibilityWidth = _grabExtents.Width;
            }
            if (_dialogProvider != null) _dialogProvider.CloseDropDown();
            this.Hide();
        }




    }
}
