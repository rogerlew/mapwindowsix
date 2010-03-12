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
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/26/2009 12:49:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MapWindow.Drawing;

namespace MapWindow.Forms
{
    /// <summary>
    /// ExportFeature
    /// </summary>
    public class AddCustomSymbolDialog : Form
    {
        private Button _btnCancel;
        private Button _btnOk;
        private Label _lblName;
        private Label _lblCategory;
        private ComboBox _cmbSymbolCategory;
        private TextBox _txtSymbolName;

        #region Private Variables

        

        private readonly List<string> _categories = new List<string>();
        private readonly IFeatureSymbolizer _symbolizer;
        private CustomSymbolizer _customSymbolizer;

        #endregion


        #region Windows Form Designer generated code
        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected System.ComponentModel.IContainer components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomSymbolDialog));
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnOk = new System.Windows.Forms.Button();
            this._lblName = new System.Windows.Forms.Label();
            this._lblCategory = new System.Windows.Forms.Label();
            this._cmbSymbolCategory = new System.Windows.Forms.ComboBox();
            this._txtSymbolName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _btnCancel
            // 
            this._btnCancel.AccessibleDescription = null;
            this._btnCancel.AccessibleName = null;
            resources.ApplyResources(this._btnCancel, "_btnCancel");
            this._btnCancel.BackgroundImage = null;
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Font = null;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _btnOk
            // 
            this._btnOk.AccessibleDescription = null;
            this._btnOk.AccessibleName = null;
            resources.ApplyResources(this._btnOk, "_btnOk");
            this._btnOk.BackgroundImage = null;
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Font = null;
            this._btnOk.Name = "_btnOk";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // _lblName
            // 
            this._lblName.AccessibleDescription = null;
            this._lblName.AccessibleName = null;
            resources.ApplyResources(this._lblName, "_lblName");
            this._lblName.Font = null;
            this._lblName.Name = "_lblName";
            this._lblName.Click += new System.EventHandler(this._lblName_Click);
            // 
            // _lblCategory
            // 
            this._lblCategory.AccessibleDescription = null;
            this._lblCategory.AccessibleName = null;
            resources.ApplyResources(this._lblCategory, "_lblCategory");
            this._lblCategory.Font = null;
            this._lblCategory.Name = "_lblCategory";
            // 
            // _cmbSymbolCategory
            // 
            this._cmbSymbolCategory.AccessibleDescription = null;
            this._cmbSymbolCategory.AccessibleName = null;
            resources.ApplyResources(this._cmbSymbolCategory, "_cmbSymbolCategory");
            this._cmbSymbolCategory.BackgroundImage = null;
            this._cmbSymbolCategory.Font = null;
            this._cmbSymbolCategory.FormattingEnabled = true;
            this._cmbSymbolCategory.Name = "_cmbSymbolCategory";
            // 
            // _txtSymbolName
            // 
            this._txtSymbolName.AccessibleDescription = null;
            this._txtSymbolName.AccessibleName = null;
            resources.ApplyResources(this._txtSymbolName, "_txtSymbolName");
            this._txtSymbolName.BackgroundImage = null;
            this._txtSymbolName.Font = null;
            this._txtSymbolName.Name = "_txtSymbolName";
            // 
            // AddCustomSymbolDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this._txtSymbolName);
            this.Controls.Add(this._cmbSymbolCategory);
            this.Controls.Add(this._lblCategory);
            this.Controls.Add(this._lblName);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._btnCancel);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCustomSymbolDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// This looks like a function created by Mr. Jiri and not commented on.
        /// </summary>
        /// <param name="symbolCategories"></param>
        /// <param name="symbolizer"></param>
        public AddCustomSymbolDialog(List<string> symbolCategories, IFeatureSymbolizer symbolizer)
        {
            InitializeComponent();
            _symbolizer = symbolizer;
            _categories = symbolCategories;

            UpdateCategories();

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// The custom symbolizer edited by this form
        /// </summary>
        public CustomSymbolizer CustomSymbolizer
        {
            get { return _customSymbolizer; }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

        private void UpdateCategories()
        {
            //the default new category will be called 'My Symbols'.
            if (!_categories.Contains("My Symbols"))
            {
                _categories.Insert(0, "My Symbols");
            }
            
            _cmbSymbolCategory.SuspendLayout();
            _cmbSymbolCategory.Items.Clear();
            foreach (string cat in _categories)
            {
                _cmbSymbolCategory.Items.Add(cat);
            }
            _cmbSymbolCategory.SelectedIndex = 0;
            _cmbSymbolCategory.ResumeLayout();
        }

        /// <summary>
        /// Creates the new custom symbolizer with the specified name and category
        /// </summary>
        /// <returns>the custom symbolizer</returns>
        private CustomSymbolizer CreateCustomSymbolizer()
        {
            CustomSymbolizer custSym = null;
            if (_symbolizer is PointSymbolizer)
            {
                custSym = new CustomPointSymbolizer();
            }
            else if (_symbolizer is LineSymbolizer)
            {
                custSym = new CustomLineSymbolizer();
            }
            else if (_symbolizer is PolygonSymbolizer)
            {
                custSym = new CustomPolygonSymbolizer();
            }
            if (custSym != null)
            {
                custSym.Symbolizer = _symbolizer;
                custSym.Name = _txtSymbolName.Text;
                custSym.Category = _cmbSymbolCategory.Text;
                return custSym;
            }
            return null;
        }

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
            //creates the new custom symbolizer
            CustomSymbolizer newCustSym = CreateCustomSymbolizer();
            if (newCustSym != null)
            {
                _customSymbolizer = newCustSym;
            }
            
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _lblName_Click(object sender, EventArgs e)
        {

        }

        
       
    }
}