//********************************************************************************************************
// Product Name: MapWindow.Tools.BooleanElement
// Description:  Boolean Element for use in the tool dialog
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    internal class BooleanElement : DialogElement
    {
        #region Class Variables
       
        private System.Windows.Forms.CheckBox checkBox1;
        private Boolean _updateBox = true;
        
        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public BooleanElement(BooleanParam param)
        {
            //Needed by the designer
            InitializeComponent();

            this.Param = param;
            checkBox1.Text = param.CheckBoxText;
            GroupBox.Text = param.Name;

            Refresh();
        }

        public override void Refresh()
        {
            _updateBox = false;

            //This stuff loads the default value
            if (Param.DefaultSpecified == false)
            {
                this.Status = ToolStatus.Empty;
                this.LightTipText = MessageStrings.ParameterInvalid;
                checkBox1.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            }
            else if (Param.Value == true)
            {
                this.Status = ToolStatus.Ok;
                this.LightTipText = MessageStrings.ParameterValid;
                checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else if (Param.Value == false)
            {
                this.Status = ToolStatus.Ok;
                this.LightTipText = MessageStrings.ParameterValid;
                checkBox1.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }

            _updateBox = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the checkbox
        /// </summary>
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (_updateBox)
            {
                if (checkBox1.CheckState == System.Windows.Forms.CheckState.Checked)
                    this.Param.Value = true;
                else if (checkBox1.CheckState == System.Windows.Forms.CheckState.Unchecked)
                    this.Param.Value = false;
            }
        }

        /// <summary>
        /// When the check box it clicked this event fires
        /// </summary>
        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new BooleanParam Param
        {
            get { return (BooleanParam)base.Param; }
            set { base.Param = (Parameter)value; }
        }

        #endregion

        #region Generate by the designer
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.checkBox1);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.checkBox1, 0);
            // 
            // lblStatus
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 20);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(44, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // BooleanElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "BooleanElement";
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
       #endregion
    }
}
