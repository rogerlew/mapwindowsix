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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 3:34:26 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using MapWindow.Data;

namespace MapWindow.Components
{
    /// <summary>
    /// Creates a new instance of the SQLQueryControl
    /// </summary>
    [ToolboxBitmap(typeof(SQLQueryControl), "UserControl.ico")]
    public class SQLQueryControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the text in the expression textbox has changed.
        /// </summary>
        public event EventHandler ExpressionTextChanged;

        #endregion

        private DataTable _table;
        private IAttributeSource _attributeSource;
        private ToolTip ttHelp;
        private Button btnNull;
        private Button btnNotNull;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

       
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLQueryControl));
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFields = new System.Windows.Forms.Label();
            this.lblUniqueValues = new System.Windows.Forms.Label();
            this.lblSelectPrecursor = new System.Windows.Forms.Label();
            this.rtbFilterText = new System.Windows.Forms.RichTextBox();
            this.btnGetUniqueValues = new System.Windows.Forms.Button();
            this.btnNot = new System.Windows.Forms.Button();
            this.btnParenthasis = new System.Windows.Forms.Button();
            this.btnAsterix = new System.Windows.Forms.Button();
            this.btnOr = new System.Windows.Forms.Button();
            this.btnLessThanOrEqual = new System.Windows.Forms.Button();
            this.btnLessThan = new System.Windows.Forms.Button();
            this.btnAnd = new System.Windows.Forms.Button();
            this.btnGreaterThanOrEqual = new System.Windows.Forms.Button();
            this.btnGreaterThan = new System.Windows.Forms.Button();
            this.btnLike = new System.Windows.Forms.Button();
            this.btnNotEqual = new System.Windows.Forms.Button();
            this.btnEquals = new System.Windows.Forms.Button();
            this.lbxUniqueValues = new System.Windows.Forms.ListBox();
            this.lbxFields = new System.Windows.Forms.ListBox();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnNull = new System.Windows.Forms.Button();
            this.btnNotNull = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMax
            // 
            this.lblMax.AccessibleDescription = null;
            this.lblMax.AccessibleName = null;
            resources.ApplyResources(this.lblMax, "lblMax");
            this.lblMax.BackColor = System.Drawing.Color.White;
            this.lblMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMax.Font = null;
            this.lblMax.Name = "lblMax";
            this.ttHelp.SetToolTip(this.lblMax, resources.GetString("lblMax.ToolTip"));
            // 
            // lblMin
            // 
            this.lblMin.AccessibleDescription = null;
            this.lblMin.AccessibleName = null;
            resources.ApplyResources(this.lblMin, "lblMin");
            this.lblMin.BackColor = System.Drawing.Color.White;
            this.lblMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMin.Font = null;
            this.lblMin.Name = "lblMin";
            this.ttHelp.SetToolTip(this.lblMin, resources.GetString("lblMin.ToolTip"));
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            this.ttHelp.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            this.ttHelp.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // lblFields
            // 
            this.lblFields.AccessibleDescription = null;
            this.lblFields.AccessibleName = null;
            resources.ApplyResources(this.lblFields, "lblFields");
            this.lblFields.Font = null;
            this.lblFields.Name = "lblFields";
            this.ttHelp.SetToolTip(this.lblFields, resources.GetString("lblFields.ToolTip"));
            // 
            // lblUniqueValues
            // 
            this.lblUniqueValues.AccessibleDescription = null;
            this.lblUniqueValues.AccessibleName = null;
            resources.ApplyResources(this.lblUniqueValues, "lblUniqueValues");
            this.lblUniqueValues.Font = null;
            this.lblUniqueValues.Name = "lblUniqueValues";
            this.ttHelp.SetToolTip(this.lblUniqueValues, resources.GetString("lblUniqueValues.ToolTip"));
            // 
            // lblSelectPrecursor
            // 
            this.lblSelectPrecursor.AccessibleDescription = null;
            this.lblSelectPrecursor.AccessibleName = null;
            resources.ApplyResources(this.lblSelectPrecursor, "lblSelectPrecursor");
            this.lblSelectPrecursor.Font = null;
            this.lblSelectPrecursor.Name = "lblSelectPrecursor";
            this.ttHelp.SetToolTip(this.lblSelectPrecursor, resources.GetString("lblSelectPrecursor.ToolTip"));
            // 
            // rtbFilterText
            // 
            this.rtbFilterText.AccessibleDescription = null;
            this.rtbFilterText.AccessibleName = null;
            resources.ApplyResources(this.rtbFilterText, "rtbFilterText");
            this.rtbFilterText.BackgroundImage = null;
            this.rtbFilterText.Font = null;
            this.rtbFilterText.Name = "rtbFilterText";
            this.ttHelp.SetToolTip(this.rtbFilterText, resources.GetString("rtbFilterText.ToolTip"));
            this.rtbFilterText.TextChanged += new System.EventHandler(this.rtbFilterText_TextChanged);
            // 
            // btnGetUniqueValues
            // 
            this.btnGetUniqueValues.AccessibleDescription = null;
            this.btnGetUniqueValues.AccessibleName = null;
            resources.ApplyResources(this.btnGetUniqueValues, "btnGetUniqueValues");
            this.btnGetUniqueValues.BackgroundImage = null;
            this.btnGetUniqueValues.Font = null;
            this.btnGetUniqueValues.Name = "btnGetUniqueValues";
            this.ttHelp.SetToolTip(this.btnGetUniqueValues, resources.GetString("btnGetUniqueValues.ToolTip"));
            this.btnGetUniqueValues.UseVisualStyleBackColor = true;
            this.btnGetUniqueValues.Click += new System.EventHandler(this.btnGetUniqueValues_Click);
            // 
            // btnNot
            // 
            this.btnNot.AccessibleDescription = null;
            this.btnNot.AccessibleName = null;
            resources.ApplyResources(this.btnNot, "btnNot");
            this.btnNot.BackgroundImage = null;
            this.btnNot.Font = null;
            this.btnNot.Name = "btnNot";
            this.ttHelp.SetToolTip(this.btnNot, resources.GetString("btnNot.ToolTip"));
            this.btnNot.UseVisualStyleBackColor = true;
            this.btnNot.Click += new System.EventHandler(this.btnNot_Click);
            // 
            // btnParenthasis
            // 
            this.btnParenthasis.AccessibleDescription = null;
            this.btnParenthasis.AccessibleName = null;
            resources.ApplyResources(this.btnParenthasis, "btnParenthasis");
            this.btnParenthasis.BackgroundImage = null;
            this.btnParenthasis.Font = null;
            this.btnParenthasis.Name = "btnParenthasis";
            this.ttHelp.SetToolTip(this.btnParenthasis, resources.GetString("btnParenthasis.ToolTip"));
            this.btnParenthasis.UseVisualStyleBackColor = true;
            this.btnParenthasis.Click += new System.EventHandler(this.btnParenthasis_Click);
            // 
            // btnAsterix
            // 
            this.btnAsterix.AccessibleDescription = null;
            this.btnAsterix.AccessibleName = null;
            resources.ApplyResources(this.btnAsterix, "btnAsterix");
            this.btnAsterix.BackgroundImage = null;
            this.btnAsterix.Font = null;
            this.btnAsterix.Name = "btnAsterix";
            this.ttHelp.SetToolTip(this.btnAsterix, resources.GetString("btnAsterix.ToolTip"));
            this.btnAsterix.UseVisualStyleBackColor = true;
            this.btnAsterix.Click += new System.EventHandler(this.btnAsterix_Click);
            // 
            // btnOr
            // 
            this.btnOr.AccessibleDescription = null;
            this.btnOr.AccessibleName = null;
            resources.ApplyResources(this.btnOr, "btnOr");
            this.btnOr.BackgroundImage = null;
            this.btnOr.Font = null;
            this.btnOr.Name = "btnOr";
            this.ttHelp.SetToolTip(this.btnOr, resources.GetString("btnOr.ToolTip"));
            this.btnOr.UseVisualStyleBackColor = true;
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            // 
            // btnLessThanOrEqual
            // 
            this.btnLessThanOrEqual.AccessibleDescription = null;
            this.btnLessThanOrEqual.AccessibleName = null;
            resources.ApplyResources(this.btnLessThanOrEqual, "btnLessThanOrEqual");
            this.btnLessThanOrEqual.BackgroundImage = null;
            this.btnLessThanOrEqual.Font = null;
            this.btnLessThanOrEqual.Name = "btnLessThanOrEqual";
            this.ttHelp.SetToolTip(this.btnLessThanOrEqual, resources.GetString("btnLessThanOrEqual.ToolTip"));
            this.btnLessThanOrEqual.UseVisualStyleBackColor = true;
            this.btnLessThanOrEqual.Click += new System.EventHandler(this.btnLessThanOrEqual_Click);
            // 
            // btnLessThan
            // 
            this.btnLessThan.AccessibleDescription = null;
            this.btnLessThan.AccessibleName = null;
            resources.ApplyResources(this.btnLessThan, "btnLessThan");
            this.btnLessThan.BackgroundImage = null;
            this.btnLessThan.Font = null;
            this.btnLessThan.Name = "btnLessThan";
            this.ttHelp.SetToolTip(this.btnLessThan, resources.GetString("btnLessThan.ToolTip"));
            this.btnLessThan.UseVisualStyleBackColor = true;
            this.btnLessThan.Click += new System.EventHandler(this.btnLessThan_Click);
            // 
            // btnAnd
            // 
            this.btnAnd.AccessibleDescription = null;
            this.btnAnd.AccessibleName = null;
            resources.ApplyResources(this.btnAnd, "btnAnd");
            this.btnAnd.BackgroundImage = null;
            this.btnAnd.Font = null;
            this.btnAnd.Name = "btnAnd";
            this.ttHelp.SetToolTip(this.btnAnd, resources.GetString("btnAnd.ToolTip"));
            this.btnAnd.UseVisualStyleBackColor = true;
            this.btnAnd.Click += new System.EventHandler(this.btnAnd_Click);
            // 
            // btnGreaterThanOrEqual
            // 
            this.btnGreaterThanOrEqual.AccessibleDescription = null;
            this.btnGreaterThanOrEqual.AccessibleName = null;
            resources.ApplyResources(this.btnGreaterThanOrEqual, "btnGreaterThanOrEqual");
            this.btnGreaterThanOrEqual.BackgroundImage = null;
            this.btnGreaterThanOrEqual.Font = null;
            this.btnGreaterThanOrEqual.Name = "btnGreaterThanOrEqual";
            this.ttHelp.SetToolTip(this.btnGreaterThanOrEqual, resources.GetString("btnGreaterThanOrEqual.ToolTip"));
            this.btnGreaterThanOrEqual.UseVisualStyleBackColor = true;
            this.btnGreaterThanOrEqual.Click += new System.EventHandler(this.btnGreaterThanOrEqual_Click);
            // 
            // btnGreaterThan
            // 
            this.btnGreaterThan.AccessibleDescription = null;
            this.btnGreaterThan.AccessibleName = null;
            resources.ApplyResources(this.btnGreaterThan, "btnGreaterThan");
            this.btnGreaterThan.BackgroundImage = null;
            this.btnGreaterThan.Font = null;
            this.btnGreaterThan.Name = "btnGreaterThan";
            this.ttHelp.SetToolTip(this.btnGreaterThan, resources.GetString("btnGreaterThan.ToolTip"));
            this.btnGreaterThan.UseVisualStyleBackColor = true;
            this.btnGreaterThan.Click += new System.EventHandler(this.btnGreaterThan_Click);
            // 
            // btnLike
            // 
            this.btnLike.AccessibleDescription = null;
            this.btnLike.AccessibleName = null;
            resources.ApplyResources(this.btnLike, "btnLike");
            this.btnLike.BackgroundImage = null;
            this.btnLike.Font = null;
            this.btnLike.Name = "btnLike";
            this.ttHelp.SetToolTip(this.btnLike, resources.GetString("btnLike.ToolTip"));
            this.btnLike.UseVisualStyleBackColor = true;
            this.btnLike.Click += new System.EventHandler(this.btnLike_Click);
            // 
            // btnNotEqual
            // 
            this.btnNotEqual.AccessibleDescription = null;
            this.btnNotEqual.AccessibleName = null;
            resources.ApplyResources(this.btnNotEqual, "btnNotEqual");
            this.btnNotEqual.BackgroundImage = null;
            this.btnNotEqual.Font = null;
            this.btnNotEqual.Name = "btnNotEqual";
            this.ttHelp.SetToolTip(this.btnNotEqual, resources.GetString("btnNotEqual.ToolTip"));
            this.btnNotEqual.UseVisualStyleBackColor = true;
            this.btnNotEqual.Click += new System.EventHandler(this.btnNotEqual_Click);
            // 
            // btnEquals
            // 
            this.btnEquals.AccessibleDescription = null;
            this.btnEquals.AccessibleName = null;
            resources.ApplyResources(this.btnEquals, "btnEquals");
            this.btnEquals.BackgroundImage = null;
            this.btnEquals.Font = null;
            this.btnEquals.Name = "btnEquals";
            this.ttHelp.SetToolTip(this.btnEquals, resources.GetString("btnEquals.ToolTip"));
            this.btnEquals.UseVisualStyleBackColor = true;
            this.btnEquals.Click += new System.EventHandler(this.btnEquals_Click);
            // 
            // lbxUniqueValues
            // 
            this.lbxUniqueValues.AccessibleDescription = null;
            this.lbxUniqueValues.AccessibleName = null;
            resources.ApplyResources(this.lbxUniqueValues, "lbxUniqueValues");
            this.lbxUniqueValues.BackColor = System.Drawing.SystemColors.Control;
            this.lbxUniqueValues.BackgroundImage = null;
            this.lbxUniqueValues.Font = null;
            this.lbxUniqueValues.FormattingEnabled = true;
            this.lbxUniqueValues.Name = "lbxUniqueValues";
            this.ttHelp.SetToolTip(this.lbxUniqueValues, resources.GetString("lbxUniqueValues.ToolTip"));
            this.lbxUniqueValues.DoubleClick += new System.EventHandler(this.lbxUniqueValues_DoubleClick);
            // 
            // lbxFields
            // 
            this.lbxFields.AccessibleDescription = null;
            this.lbxFields.AccessibleName = null;
            resources.ApplyResources(this.lbxFields, "lbxFields");
            this.lbxFields.BackgroundImage = null;
            this.lbxFields.Font = null;
            this.lbxFields.FormattingEnabled = true;
            this.lbxFields.Name = "lbxFields";
            this.ttHelp.SetToolTip(this.lbxFields, resources.GetString("lbxFields.ToolTip"));
            this.lbxFields.SelectedIndexChanged += new System.EventHandler(this.lbxFields_SelectedIndexChanged);
            this.lbxFields.DoubleClick += new System.EventHandler(this.lbxFields_DoubleClick);
            // 
            // btnNull
            // 
            this.btnNull.AccessibleDescription = null;
            this.btnNull.AccessibleName = null;
            resources.ApplyResources(this.btnNull, "btnNull");
            this.btnNull.BackgroundImage = null;
            this.btnNull.Font = null;
            this.btnNull.Name = "btnNull";
            this.ttHelp.SetToolTip(this.btnNull, resources.GetString("btnNull.ToolTip"));
            this.btnNull.UseVisualStyleBackColor = true;
            this.btnNull.Click += new System.EventHandler(this.btnNull_Click);
            // 
            // btnNotNull
            // 
            this.btnNotNull.AccessibleDescription = null;
            this.btnNotNull.AccessibleName = null;
            resources.ApplyResources(this.btnNotNull, "btnNotNull");
            this.btnNotNull.BackgroundImage = null;
            this.btnNotNull.Font = null;
            this.btnNotNull.Name = "btnNotNull";
            this.ttHelp.SetToolTip(this.btnNotNull, resources.GetString("btnNotNull.ToolTip"));
            this.btnNotNull.UseVisualStyleBackColor = true;
            this.btnNotNull.Click += new System.EventHandler(this.btnNotNull_Click);
            // 
            // SQLQueryControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnNotNull);
            this.Controls.Add(this.btnNull);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFields);
            this.Controls.Add(this.lblUniqueValues);
            this.Controls.Add(this.lblSelectPrecursor);
            this.Controls.Add(this.rtbFilterText);
            this.Controls.Add(this.btnGetUniqueValues);
            this.Controls.Add(this.btnNot);
            this.Controls.Add(this.btnParenthasis);
            this.Controls.Add(this.btnAsterix);
            this.Controls.Add(this.btnOr);
            this.Controls.Add(this.btnLessThanOrEqual);
            this.Controls.Add(this.btnLessThan);
            this.Controls.Add(this.btnAnd);
            this.Controls.Add(this.btnGreaterThanOrEqual);
            this.Controls.Add(this.btnGreaterThan);
            this.Controls.Add(this.btnLike);
            this.Controls.Add(this.btnNotEqual);
            this.Controls.Add(this.btnEquals);
            this.Controls.Add(this.lbxUniqueValues);
            this.Controls.Add(this.lbxFields);
            this.Font = null;
            this.Name = "SQLQueryControl";
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void rtbFilterText_TextChanged(object sender, EventArgs e)
        {
            if (ExpressionTextChanged != null) ExpressionTextChanged(this, new EventArgs());
        }

        /// <summary>
        /// Gets or sets the string expression text
        /// </summary>
        public string ExpressionText
        {
            get { return rtbFilterText.Text; }
            set { rtbFilterText.Text = value; }
        }

        void lbxUniqueValues_DoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = lbxUniqueValues.SelectedItem.ToString() + " ";
        }

        #endregion

        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFields;
        private System.Windows.Forms.Label lblUniqueValues;
        private System.Windows.Forms.Label lblSelectPrecursor;
        private System.Windows.Forms.RichTextBox rtbFilterText;
        private System.Windows.Forms.Button btnGetUniqueValues;
        private System.Windows.Forms.Button btnNot;
        private System.Windows.Forms.Button btnParenthasis;
        private System.Windows.Forms.Button btnAsterix;
        private System.Windows.Forms.Button btnOr;
        private System.Windows.Forms.Button btnLessThanOrEqual;
        private System.Windows.Forms.Button btnLessThan;
        private System.Windows.Forms.Button btnAnd;
        private System.Windows.Forms.Button btnGreaterThanOrEqual;
        private System.Windows.Forms.Button btnGreaterThan;
        private System.Windows.Forms.Button btnLike;
        private System.Windows.Forms.Button btnNotEqual;
        private System.Windows.Forms.Button btnEquals;
        private System.Windows.Forms.ListBox lbxUniqueValues;
        private System.Windows.Forms.ListBox lbxFields;

        /// <summary>
        /// Creates a new instance of the control with no fields specified.
        /// </summary>
        public SQLQueryControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the data Table for this control. Setting this will
        /// automatically update the fields shown in the list.
        /// </summary>
        public DataTable Table
        {
            get { return _table; }
            set
            {
                _table = value;
                if (_table != null)
                {
                    UpdateFields();
                }
            }
        }

        /// <summary>
        /// Setting this is an alternative to specifying the table.  This allows the 
        /// query control to build a query using pages of data instead of the whole
        /// table at once.
        /// </summary>
        public IAttributeSource AttributeSource
        {
            get { return _attributeSource; }
            set
            {
                _attributeSource = value;
                UpdateFields();
            }
        }

        private void UpdateFields()
        {
            lbxFields.SuspendLayout();
            lbxFields.Items.Clear();
            if(_attributeSource != null)
            {
                DataColumn[] columns = _attributeSource.GetColumns();
                foreach (Field dc in columns)
                {
                    lbxFields.Items.Add(dc.ColumnName);
                }
            }
            else if (_table != null)
            {
                foreach (DataColumn dc in _table.Columns)
                {
                    lbxFields.Items.Add(dc.ColumnName);
                }
            }
            
            lbxFields.ResumeLayout();
        }

        /// <summary>
        /// Gets or sets the string that appears in the filter text.
        /// </summary>
        public override string Text
        {
            get { return rtbFilterText.Text; }
            set { rtbFilterText.Text = value; }
        }

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

        #region Event Handlers

        private void lbxFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGetUniqueValues.Enabled = true;
            string field = lbxFields.SelectedItem as string;
            if (field == null) return;
            IComparable min = null;
            lblMin.Text = "";
            IComparable max = null;
            lblMax.Text = "";
            if(_attributeSource != null)
            {
                int numPages = (int)Math.Ceiling((double)_attributeSource.NumRows() / 10000);
                for(int page = 0; page < numPages; page ++)
                {
                    DataTable table = _attributeSource.GetAttributes(page*10000, 10000);
                    foreach (DataRow dr in table.Rows)
                    {
                        if ((dr[field] is DBNull)) continue;
                        if(min == null)
                        {
                            min = dr[field] as IComparable;
                        }
                        else
                        {
                            if (min.CompareTo(dr[field]) > 0) min = dr[field] as IComparable;
                        }
                        if (max == null)
                        {
                            max = dr[field] as IComparable;
                        }
                        else
                        {
                            if (max.CompareTo(dr[field]) < 0) max = dr[field] as IComparable;
                        }
                    }
                }
            }
            if(_table != null)
            {
                foreach (DataRow dr in _table.Rows)
                {
                    if ((dr[field] is DBNull)) continue;
                    if (min == null)
                    {
                        min = dr[field] as IComparable;
                    }
                    else
                    {
                        if (min.CompareTo(dr[field]) > 0) min = dr[field] as IComparable;
                    }
                    if (max == null)
                    {
                        max = dr[field] as IComparable;
                    }
                    else
                    {
                        if (max.CompareTo(dr[field]) < 0) max = dr[field] as IComparable;
                    }
                }
            }
            if(min != null)lblMin.Text = min.ToString();
            if(max != null)lblMax.Text = max.ToString();

        }

        private void btnGetUniqueValues_Click(object sender, EventArgs e)
        {
            // Sorting should be done as the original objects, not as strings.
            ArrayList lst = new ArrayList();
            string fieldName = lbxFields.SelectedItem.ToString();
            bool useAll = false;
            bool isString = true;
            if (_attributeSource != null)
            {
                isString = (_attributeSource.GetColumn(fieldName).DataType == typeof(string));
                int numPages = (int)Math.Ceiling((double)_attributeSource.NumRows() / 10000);
                for (int page = 0; page < numPages; page++)
                {
                    DataTable table = _attributeSource.GetAttributes(page * 10000, 10000);
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr[fieldName] is DBNull) continue;
                        if (lst.Contains(dr[fieldName])) continue;
                        lst.Add(dr[fieldName]);
                    }
                    if (lst.Count <= 10000 || useAll) continue;
                    if(MessageBox.Show("There are more than 10,000 unique values... do you wish to show all of them?",
                                       "Large Number of Unique Values", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        useAll = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (_table != null)
            {
                isString = (_table.Columns[fieldName].DataType == typeof(string));
                foreach (DataRow dr in _table.Rows)
                {
                    if (dr[fieldName] is DBNull) continue;
                    if (lst.Contains(dr[fieldName])) continue;
                    lst.Add(dr[fieldName]);
                }
            }

            
            lst.Sort();
            List<string> text = new List<string>();
            
            foreach (object o in lst)
            {
                if(isString)
                {
                    text.Add("'" + ((string)o).Replace("'", "''") + "'");
                }
                else
                {
                    text.Add(o.ToString());
                }
            }
            lbxUniqueValues.SuspendLayout();
            lbxUniqueValues.Items.Clear();
            lbxUniqueValues.Items.AddRange(text.ToArray());
            lbxUniqueValues.ResumeLayout();
            lbxUniqueValues.Enabled = true;
            lbxUniqueValues.BackColor = Color.White;
            btnGetUniqueValues.Enabled = false;

        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "= ";
        }

        private void btnNotEqual_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "<> ";
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "LIKE ";
        }

        private void btnGreaterThan_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "> ";
        }

        private void btnGreaterThanOrEqual_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = ">= ";
        }

        private void btnAnd_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "AND ";
        }

        private void btnLessThan_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "< ";
        }

        private void btnLessThanOrEqual_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "<= ";
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "OR ";
        }

        private void btnAsterix_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "*";
        }

        private void btnParenthasis_Click(object sender, EventArgs e)
        {
            string str = rtbFilterText.SelectedText;
            str = "(" + str + ")";
            rtbFilterText.SelectedText = str;
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "NOT ";
        }

        void lbxFields_DoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "[" + lbxFields.SelectedItem + "] ";
        }

        private void btnNull_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = " is Null";
        }

       

        #endregion

        private void btnNotNull_Click(object sender, EventArgs e)
        {
            string str = rtbFilterText.SelectedText;
            if(str.Length == 0)
            {
                str = rtbFilterText.Text;
                str = "NOT ((" + str + ") is Null)";
                rtbFilterText.Text = str;
            }
            else
            {
                str = "NOT ((" + str + ") is Null)";
                rtbFilterText.SelectedText = str;
            }
            
            
        }
       
    }
}
