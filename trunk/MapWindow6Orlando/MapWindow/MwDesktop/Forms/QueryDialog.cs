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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2009 11:29:27 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//Kandasamy Prasanna: contribute to modify the Execute Button in 09/17/2009
//
//********************************************************************************************************
using System;
using System.Data;
using System.Windows.Forms;
using MapWindow.Components;

namespace MapWindow.Forms
{
    /// <summary>
    /// This will load to get query from user
    /// </summary>
    public class QueryDialog : Form
    {
       #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        private Panel panel1;
        private Button btnCancel;
        private SQLQueryControl sqlQueryControl1;
        private Button btnExecute;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.sqlQueryControl1 = new MapWindow.Components.SQLQueryControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.btnExecute);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // btnExecute
            // 
            this.btnExecute.AccessibleDescription = null;
            this.btnExecute.AccessibleName = null;
            resources.ApplyResources(this.btnExecute, "btnExecute");
            this.btnExecute.BackgroundImage = null;
            this.btnExecute.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExecute.Font = null;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
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
            // 
            // QueryDialog
            // 
            this.AcceptButton = this.btnExecute;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.sqlQueryControl1);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueryDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Query dialog
        /// </summary>
        public QueryDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string expression.  Setting this will set the initial
        /// text content in the dialog.
        /// </summary>
        public string Expression
        {
            get { return sqlQueryControl1.ExpressionText; }
            set { sqlQueryControl1.ExpressionText = value; }
        }

        /// <summary>
        /// Gets or sets the DataTable that hte expression dialog uses.
        /// </summary>
        public DataTable Table
        {
            get { return sqlQueryControl1.Table; }
            set { sqlQueryControl1.Table = value; }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            
            OnApplyChanges();
            DialogResult = DialogResult.OK;
            Close();
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

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            if (ChangesApplied != null) ChangesApplied(this, new EventArgs());
        }

        #endregion

       

    }
}
