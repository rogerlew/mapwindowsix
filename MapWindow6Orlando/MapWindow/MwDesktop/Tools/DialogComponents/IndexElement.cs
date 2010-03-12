//********************************************************************************************************
// Product Name: MapWindow.Tools.IntegerElement
// Description:  Integer Element for use in the tool dialog
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name                 |    Date              |   Comments
// ---------------------|----------------------|----------------------------------------------------
// Ted Dunsford         |  8/28/2009           |  Cleaned up some formatting content using re-sharper.
//********************************************************************************************************
using System;
using System.Windows.Forms;
using MapWindow.Forms;
using MapWindow.Tools.Param;

namespace MapWindow.Tools.DialogComponents
{
    /// <summary>
    ///  /// A modular component that can be inherited to retrieve parameters for functions.
    /// </summary>
    internal class IndexElement : DialogElement
    {
        
        #region Class Variables

        private TextBox textBox1;
        private Button btnSelect;
        private Boolean _enableUpdate = true;
        private string _expression;
        private bool _click;

        #endregion

        public string Expression
        {
            get { return _expression; }
        }

        #region Constructor
        /// <summary>
        /// Create a new instant.
        /// </summary>
        /// <param name="param"></param>
        public IndexElement(IndexParam param)
        {
            InitializeComponent();
            GroupBox.Text = param.Name;

            //We save the parameters passed in 
            base.Param = param;
            HandleStatusLight();
        }

        public override void Refresh()
        {
            HandleStatusLight();
        }
        private void HandleStatusLight()
        {
            _enableUpdate = false;

            //base.Status = ToolStatus.Ok;
            // base.LightTipText = MapWindow.MessageStrings.FeaturesSelected;

            //We load the default parameters
            if (_click)
            {
                textBox1.Text = _expression;
                base.Status = ToolStatus.Ok;
                LightTipText = MessageStrings.FeaturesSelected;
            }
            else
            {
                textBox1.Text = "Expression";
            }

            _enableUpdate = true;
        }

        #endregion
     
  
        //User Click the Select Button
        private void BtnSelectClick(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (((IndexParam) base.Param).Fs == null || ((IndexParam) base.Param).Fs.DataTable == null) return;

                if (((IndexParam) base.Param).Fs.DataTable.Rows.Count < 1) return;
                
                SQLExpressionDialog dlgExpression = new SQLExpressionDialog();
                dlgExpression.Table = ((IndexParam) base.Param).Fs.DataTable;
                dlgExpression.ShowDialog();
                
                if (dlgExpression.DialogResult == DialogResult.OK)
                {
                    _expression = dlgExpression.Expression;
                    textBox1.Text = _expression;
                    base.Status = ToolStatus.Ok;
                    _click = true;
                    LightTipText = MessageStrings.FeaturesSelected;
                }

                
            }
        }


        /// <summary>
        /// When the text box is clicked this event fires
        /// </summary>
        private void TextBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new IndexParam Param
        {
            get { return (IndexParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Controls.Add(this.btnSelect);
            this.GroupBox.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);
            this.GroupBox.Controls.SetChildIndex(this.btnSelect, 0);
            // 
            // lblStatus
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 20);
            //
            // btnSelect
            // 
            
            this.btnSelect.Image = global::MapWindow.Images.AddLayer;
            this.btnSelect.Location = new System.Drawing.Point(460, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(26, 26);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelectClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(400, 20);
            this.textBox1.TabIndex = 3;
            //this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Click += new System.EventHandler(this.TextBox1Click);
            // 
            // IndexElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "IndexElement";
            this.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

    }
}
