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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/18/2009 9:06:46 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MapWindow.Drawing;

namespace MapWindow.Forms
{


    /// <summary>
    /// Initial dialog for selecting a predefined line symbol
    /// </summary>
    public class LineSymbolDialog : Form
    {
        #region Events

        /// <summary>
        /// Fires an event indicating that changes should be applied.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion
        
        
        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private ILineSymbolizer _symbolizer;
        private ILineSymbolizer _symbolizer2;
        private ILineSymbolizer _original;
        private List<string> _categories;
        
        private Label lblSymbologyType;
        private Label lblPredefinedSymbol;
        private Label lblSymbolPreview;
        private Button btnSymbolDetails;
        private Components.SymbolPreview symbolPreview1;
        private Components.PredefinedLineSymbolControl predefinedLineSymbolControl1;
        private MapWindow.Components.DialogButtons dialogButtons1;
        private ComboBox cmbCategories;
        

        #endregion
        

        #region Constructors

        /// <summary>
        /// Creates a new instance of DetailedLineSymbolDialog
        /// </summary>
        public LineSymbolDialog()
        {
            InitializeComponent();
            
            _original = new LineSymbolizer();
            _symbolizer = new LineSymbolizer();
            _symbolizer2 = new LineSymbolizer();
            Configure();
        }

        /// <summary>
        /// Creates a new Detailed Line Symbol Dialog
        /// </summary>
        /// <param name="symbolizer"></param>
        public LineSymbolDialog(ILineSymbolizer symbolizer)
        {
            InitializeComponent();
            
            _original = symbolizer;
            _symbolizer = symbolizer.Copy();
            Configure();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += btnOK_Click;
            dialogButtons1.CancelClicked += btnCancel_Click;
            dialogButtons1.ApplyClicked += btnApply_Click;
           

            LoadDefaultSymbols();
            UpdatePreview();

            predefinedLineSymbolControl1.IsSelected = false;
            predefinedLineSymbolControl1.SymbolSelected +=new EventHandler(predefinedSymbolControl_SymbolSelected);
            predefinedLineSymbolControl1.DoubleClick += new EventHandler(predefinedLineSymbolControl1_DoubleClick);
            symbolPreview1.DoubleClick += new EventHandler(SymbolPreview_DoubleClick);
        }
  
        #endregion


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineSymbolDialog));
            this.lblSymbologyType = new System.Windows.Forms.Label();
            this.lblPredefinedSymbol = new System.Windows.Forms.Label();
            this.lblSymbolPreview = new System.Windows.Forms.Label();
            this.btnSymbolDetails = new System.Windows.Forms.Button();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.predefinedLineSymbolControl1 = new MapWindow.Components.PredefinedLineSymbolControl();
            this.symbolPreview1 = new MapWindow.Components.SymbolPreview();
            this.dialogButtons1 = new MapWindow.Components.DialogButtons();
            this.SuspendLayout();
            // 
            // lblSymbologyType
            // 
            this.lblSymbologyType.AccessibleDescription = null;
            this.lblSymbologyType.AccessibleName = null;
            resources.ApplyResources(this.lblSymbologyType, "lblSymbologyType");
            this.lblSymbologyType.Font = null;
            this.lblSymbologyType.Name = "lblSymbologyType";
            // 
            // lblPredefinedSymbol
            // 
            this.lblPredefinedSymbol.AccessibleDescription = null;
            this.lblPredefinedSymbol.AccessibleName = null;
            resources.ApplyResources(this.lblPredefinedSymbol, "lblPredefinedSymbol");
            this.lblPredefinedSymbol.Font = null;
            this.lblPredefinedSymbol.Name = "lblPredefinedSymbol";
            // 
            // lblSymbolPreview
            // 
            this.lblSymbolPreview.AccessibleDescription = null;
            this.lblSymbolPreview.AccessibleName = null;
            resources.ApplyResources(this.lblSymbolPreview, "lblSymbolPreview");
            this.lblSymbolPreview.Font = null;
            this.lblSymbolPreview.Name = "lblSymbolPreview";
            // 
            // btnSymbolDetails
            // 
            this.btnSymbolDetails.AccessibleDescription = null;
            this.btnSymbolDetails.AccessibleName = null;
            resources.ApplyResources(this.btnSymbolDetails, "btnSymbolDetails");
            this.btnSymbolDetails.BackgroundImage = null;
            this.btnSymbolDetails.Font = null;
            this.btnSymbolDetails.Name = "btnSymbolDetails";
            this.btnSymbolDetails.UseVisualStyleBackColor = true;
            this.btnSymbolDetails.Click += new System.EventHandler(this.btnSymbolDetails_Click);
            // 
            // cmbCategories
            // 
            this.cmbCategories.AccessibleDescription = null;
            this.cmbCategories.AccessibleName = null;
            resources.ApplyResources(this.cmbCategories, "cmbCategories");
            this.cmbCategories.BackgroundImage = null;
            this.cmbCategories.Font = null;
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            // 
            // predefinedLineSymbolControl1
            // 
            this.predefinedLineSymbolControl1.AccessibleDescription = null;
            this.predefinedLineSymbolControl1.AccessibleName = null;
            resources.ApplyResources(this.predefinedLineSymbolControl1, "predefinedLineSymbolControl1");
            this.predefinedLineSymbolControl1.BackColor = System.Drawing.Color.White;
            this.predefinedLineSymbolControl1.BackgroundImage = null;
            this.predefinedLineSymbolControl1.CategoryFilter = "";
            this.predefinedLineSymbolControl1.CellMargin = 8;
            this.predefinedLineSymbolControl1.CellSize = new System.Drawing.Size(62, 62);
            this.predefinedLineSymbolControl1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 272, 253);
            this.predefinedLineSymbolControl1.DefaultCategoryFilter = "All";
            this.predefinedLineSymbolControl1.DynamicColumns = true;
            this.predefinedLineSymbolControl1.Font = null;
            this.predefinedLineSymbolControl1.IsInitialized = false;
            this.predefinedLineSymbolControl1.IsSelected = true;
            this.predefinedLineSymbolControl1.Name = "predefinedLineSymbolControl1";
            this.predefinedLineSymbolControl1.SelectedIndex = -1;
            this.predefinedLineSymbolControl1.SelectionBackColor = System.Drawing.Color.LightGray;
            this.predefinedLineSymbolControl1.SelectionForeColor = System.Drawing.Color.White;
            this.predefinedLineSymbolControl1.ShowSymbolNames = true;
            this.predefinedLineSymbolControl1.TextFont = new System.Drawing.Font("Arial", 8F);
            this.predefinedLineSymbolControl1.VerticalScrollEnabled = true;
            // 
            // symbolPreview1
            // 
            this.symbolPreview1.AccessibleDescription = null;
            this.symbolPreview1.AccessibleName = null;
            resources.ApplyResources(this.symbolPreview1, "symbolPreview1");
            this.symbolPreview1.BackColor = System.Drawing.Color.White;
            this.symbolPreview1.BackgroundImage = null;
            this.symbolPreview1.Font = null;
            this.symbolPreview1.Name = "symbolPreview1";
            // 
            // dialogButtons1
            // 
            this.dialogButtons1.AccessibleDescription = null;
            this.dialogButtons1.AccessibleName = null;
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.BackgroundImage = null;
            this.dialogButtons1.Font = null;
            this.dialogButtons1.Name = "dialogButtons1";
            // 
            // LineSymbolDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.dialogButtons1);
            this.Controls.Add(this.predefinedLineSymbolControl1);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.symbolPreview1);
            this.Controls.Add(this.btnSymbolDetails);
            this.Controls.Add(this.lblSymbolPreview);
            this.Controls.Add(this.lblPredefinedSymbol);
            this.Controls.Add(this.lblSymbologyType);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LineSymbolDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        #region Event Handlers



        void predefinedSymbolControl_SymbolSelected(object sender, EventArgs e)
        {
            CustomLineSymbolizer customSymbol = predefinedLineSymbolControl1.SelectedSymbolizer;

            if (customSymbol != null)
            {
                _symbolizer = customSymbol.Symbolizer;
                UpdatePreview();
            }
        }

        void SymbolPreview_DoubleClick(object sender, EventArgs e)
        {
            if (_symbolizer != null && _original != null)
            {
               ShowDetailsDialog();              
            } 
        }

        void predefinedLineSymbolControl1_DoubleClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            DialogResult = DialogResult.OK;
            Close();
        }

        void btnSymbolDetails_Click(object sender, EventArgs e)
        {

            if (_symbolizer != null && _original != null)
            {
                ShowDetailsDialog();
            }
        }

        void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            predefinedLineSymbolControl1.CategoryFilter = cmbCategories.SelectedItem.ToString();
        }

        //when the user clicks 'Add to custom symbols' on the details dialog
        void detailsDialog_AddToCustomSymbols(object sender, LineSymbolizerEventArgs e)
        {
            // Here a dialog is displayed. The user can enter the custom symbol name and category
            // in the dialog.
            AddCustomSymbolDialog dlg = new AddCustomSymbolDialog(_categories, e.Symbolizer);
            dlg.ShowDialog();

            CustomLineSymbolizer newSym = dlg.CustomSymbolizer as CustomLineSymbolizer;
            if (newSym != null)
            {
                //check if user added a new category
                if (!_categories.Contains(newSym.Category))
                {
                    _categories.Add(newSym.Category);                
                }
                UpdateCategories();

                predefinedLineSymbolControl1.SymbolizerList.Insert(0, newSym);
                predefinedLineSymbolControl1.Invalidate();
            }
            
            //TODO: save the custom symbolizer to xml / serialized file.
            //predefinedLineSymbolControl1.SaveToXml("test.xml");
        }

        void detailsDialog_ChangesApplied(object sender, EventArgs e)
        {
            //unselect any symbolizers in the control
            predefinedLineSymbolControl1.IsSelected = false;
            
            UpdatePreview(_symbolizer2);
        }

        #endregion


        #region Methods

        private void UpdatePreview()
        {
            symbolPreview1.UpdatePreview(_symbolizer);
        }

        private void UpdatePreview(IFeatureSymbolizer symbolizer)
        {
            symbolPreview1.UpdatePreview(symbolizer);
        }

        /// <summary>
        /// Shows the 'Symbol Details' dialog
        /// </summary>
        private void ShowDetailsDialog()
        {

            DetailedLineSymbolDialog detailsDialog = new DetailedLineSymbolDialog(_original);
            detailsDialog.ChangesApplied += new EventHandler(detailsDialog_ChangesApplied);
            detailsDialog.ShowDialog();
        }


        //this loads the default symbols and initializes the control
        //as well as the available categories
        private void LoadDefaultSymbols()
        {
            CustomLineSymbolProvider prov = new CustomLineSymbolProvider();

            _categories = prov.GetAvailableCategories();
            UpdateCategories();
        }

        private void UpdateCategories()
        {
            cmbCategories.SuspendLayout();
            cmbCategories.Items.Clear();
            cmbCategories.Items.Add("All");
            foreach (string cat in _categories)
            {
                cmbCategories.Items.Add(cat);
            }
            cmbCategories.SelectedIndex = 0;
            cmbCategories.ResumeLayout();
        }

        #endregion


        #region Properties

        

        #endregion


        #region Protected Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected void OnApplyChanges()
        {
            UpdatePreview();
            _original.CopyProperties(_symbolizer);
            if (ChangesApplied != null) ChangesApplied(this, new EventArgs());
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

    }
}
