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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 9:45:17 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using MapWindow.Drawing;
using MapWindow.Components;

namespace MapWindow.Forms
{
    /// <summary>
    /// SelectByAttributes
    /// </summary>
    public class SelectByAttributes : Form
    {
        private Label lblLayer;
        private ComboBox cmbLayers;
        private Label lblMethod;
        private ComboBox cmbMethod;
        private Button btnOk;
        private Button btnApply;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private IFrame _mapFrame;
        private ToolTip ttHelp;
        private Button btnClose;
        private SQLQueryControl sqlQueryControl1;
        private IFeatureLayer _activeLayer;

        #endregion


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectByAttributes));
            this.lblLayer = new System.Windows.Forms.Label();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.sqlQueryControl1 = new MapWindow.Components.SQLQueryControl();
            this.SuspendLayout();
            // 
            // lblLayer
            // 
            this.lblLayer.AccessibleDescription = null;
            this.lblLayer.AccessibleName = null;
            resources.ApplyResources(this.lblLayer, "lblLayer");
            this.lblLayer.Font = null;
            this.lblLayer.Name = "lblLayer";
            this.ttHelp.SetToolTip(this.lblLayer, resources.GetString("lblLayer.ToolTip"));
            // 
            // cmbLayers
            // 
            this.cmbLayers.AccessibleDescription = null;
            this.cmbLayers.AccessibleName = null;
            resources.ApplyResources(this.cmbLayers, "cmbLayers");
            this.cmbLayers.BackgroundImage = null;
            this.cmbLayers.Font = null;
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Name = "cmbLayers";
            this.ttHelp.SetToolTip(this.cmbLayers, resources.GetString("cmbLayers.ToolTip"));
            this.cmbLayers.SelectedIndexChanged += new System.EventHandler(this.cmbLayers_SelectedIndexChanged);
            // 
            // lblMethod
            // 
            this.lblMethod.AccessibleDescription = null;
            this.lblMethod.AccessibleName = null;
            resources.ApplyResources(this.lblMethod, "lblMethod");
            this.lblMethod.Font = null;
            this.lblMethod.Name = "lblMethod";
            this.ttHelp.SetToolTip(this.lblMethod, resources.GetString("lblMethod.ToolTip"));
            // 
            // cmbMethod
            // 
            this.cmbMethod.AccessibleDescription = null;
            this.cmbMethod.AccessibleName = null;
            resources.ApplyResources(this.cmbMethod, "cmbMethod");
            this.cmbMethod.BackgroundImage = null;
            this.cmbMethod.Font = null;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            resources.GetString("cmbMethod.Items"),
            resources.GetString("cmbMethod.Items1"),
            resources.GetString("cmbMethod.Items2"),
            resources.GetString("cmbMethod.Items3")});
            this.cmbMethod.Name = "cmbMethod";
            this.ttHelp.SetToolTip(this.cmbMethod, resources.GetString("cmbMethod.ToolTip"));
            // 
            // btnOk
            // 
            this.btnOk.AccessibleDescription = null;
            this.btnOk.AccessibleName = null;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.BackgroundImage = null;
            this.btnOk.Font = null;
            this.btnOk.Name = "btnOk";
            this.ttHelp.SetToolTip(this.btnOk, resources.GetString("btnOk.ToolTip"));
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnApply
            // 
            this.btnApply.AccessibleDescription = null;
            this.btnApply.AccessibleName = null;
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.BackgroundImage = null;
            this.btnApply.Font = null;
            this.btnApply.Name = "btnApply";
            this.ttHelp.SetToolTip(this.btnApply, resources.GetString("btnApply.ToolTip"));
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = null;
            this.btnClose.Name = "btnClose";
            this.ttHelp.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // sqlQueryControl1
            // 
            this.sqlQueryControl1.AccessibleDescription = null;
            this.sqlQueryControl1.AccessibleName = null;
            resources.ApplyResources(this.sqlQueryControl1, "sqlQueryControl1");
            this.sqlQueryControl1.AttributeSource = null;
            this.sqlQueryControl1.BackgroundImage = null;
            this.sqlQueryControl1.ExpressionText = "";
            this.sqlQueryControl1.Font = null;
            this.sqlQueryControl1.Name = "sqlQueryControl1";
            this.sqlQueryControl1.Table = null;
            this.ttHelp.SetToolTip(this.sqlQueryControl1, resources.GetString("sqlQueryControl1.ToolTip"));
            // 
            // SelectByAttributes
            // 
            this.AcceptButton = this.btnOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.sqlQueryControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.lblLayer);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectByAttributes";
            this.ShowIcon = false;
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectByAttributes
        /// </summary>
        public SelectByAttributes()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of SelectByAttributes
        /// </summary>
        /// <param name="mapFrame">the MapFrame containing the layers</param>
        public SelectByAttributes(IFrame mapFrame)
        {
            _mapFrame = mapFrame;
            InitializeComponent();
            Configure();
        }

        private void Configure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(IFeatureLayer));

            foreach(ILayer layer in _mapFrame)
            {
                IFeatureLayer fl = layer as IFeatureLayer;
                if (fl != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = fl.LegendText;
                    dr["Value"] = fl;
                    dt.Rows.Add(dr);
                }
            }
            cmbLayers.DataSource = dt;
            cmbLayers.DisplayMember = "Name";
            cmbLayers.ValueMember = "Value";
            cmbMethod.SelectedIndex = 0;
            if(cmbLayers.Items.Count > 0) cmbLayers.SelectedIndex = 0;
     
            
        }


        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map frame to use for this control
        /// </summary>
        public IFrame MapFrame
        {
            get { return _mapFrame; }
            set 
            { 
                _mapFrame = value;
                Configure();
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

        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = cmbLayers.SelectedValue as DataRowView;
            if (drv != null)
            {
                _activeLayer = drv.Row["Value"] as IFeatureLayer;
            }
            else
            {
                _activeLayer = cmbLayers.SelectedValue as IFeatureLayer;
            }
            if (_activeLayer == null) return;
            if(!_activeLayer.DataSet.AttributesPopulated && _activeLayer.DataSet.NumRows() < 50000)
            {
                _activeLayer.DataSet.FillAttributes();
            }
            if(_activeLayer.EditMode || _activeLayer.DataSet.AttributesPopulated)
            {
                sqlQueryControl1.Table = _activeLayer.DataSet.DataTable;
            }
            else
            {
                sqlQueryControl1.AttributeSource = _activeLayer.DataSet;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string filter = sqlQueryControl1.ExpressionText;
            if (_activeLayer != null)
            {
                try
                {
                    _activeLayer.SelectByAttribute(filter, GetSelectMode());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    MessageBox.Show("There was an error attempting to apply this expression.");
                }
            }
        }


        ModifySelectionModes GetSelectMode()
        {
            switch (cmbMethod.SelectedIndex)
            {
                case 0: return ModifySelectionModes.Replace;
                case 1: return ModifySelectionModes.Append;
                case 2: return ModifySelectionModes.Subtract;
                case 3: return ModifySelectionModes.SelectFrom;
            }
            return ModifySelectionModes.Replace;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ApplyFilter();
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

       

       


      
        
        

    }
}