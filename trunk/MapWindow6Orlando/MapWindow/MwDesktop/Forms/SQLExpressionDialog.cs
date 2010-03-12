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
//********************************************************************************************************
using System;
using System.Data;
using System.Windows.Forms;
using MapWindow.Data;
using MapWindow.Components;

namespace MapWindow.Forms
{
    /// <summary>
    /// SQLExpressionDialog
    /// </summary>
    public class SQLExpressionDialog : Form
    {
        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        private Panel panel1;
        private Button btnCancel;
        private Button cmdOk;
        private SQLQueryControl sqlQueryControl1;
        private Button btnApply;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLExpressionDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
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
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.cmdOk);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // btnApply
            // 
            this.btnApply.AccessibleDescription = null;
            this.btnApply.AccessibleName = null;
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.BackgroundImage = null;
            this.btnApply.Font = null;
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
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
            // cmdOk
            // 
            this.cmdOk.AccessibleDescription = null;
            this.cmdOk.AccessibleName = null;
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.BackgroundImage = null;
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Font = null;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
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
            // SQLExpressionDialog
            // 
            this.AcceptButton = this.cmdOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.sqlQueryControl1);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.HelpButton = true;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SQLExpressionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public SQLExpressionDialog()
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
        /// Gets or sets the DataTable that the expression dialog uses.
        /// </summary>
        public DataTable Table
        {
            get { return sqlQueryControl1.Table; }
            set { sqlQueryControl1.Table = value; }
        }

        /// <summary>
        /// Gets or sets the Attribute source instead of the table
        /// </summary>
        public IAttributeSource AttributeSource
        {
            get { return sqlQueryControl1.AttributeSource; }
            set { sqlQueryControl1.AttributeSource = value; }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void cmdOk_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
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