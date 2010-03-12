//********************************************************************************************************
// Product Name: MapWindow.Tools.doubleElement
// Description:  Double Element for use in the tool dialog
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    internal class DoubleElement : DialogElement
    {
        #region Class Variables

        private System.Windows.Forms.TextBox textBox1;
        private string _oldText = "";
        private Boolean _enableUpdate = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public DoubleElement(DoubleParam param)
        {
            //Needed by the designer
            InitializeializeializeComponent();
            this.GroupBox.Text = param.Name;
            
            //We save the Parameter passed in 
            this.Param = param;

            Refresh();
        }

        public override void Refresh()
        {
            if (_enableUpdate == false) return;

            _enableUpdate = false;

            //We load the default Parameter
            if (this.Param.DefaultSpecified)
            {
                double value = this.Param.Value;
                if ((value >= this.Param.Min) && (value <= this.Param.Max))
                {
                    this.Status = ToolStatus.Ok;
                    this.LightTipText = MessageStrings.ParameterValid;
                    textBox1.Text = value.ToString();
                }
                else
                {
                    this.Status = ToolStatus.Empty;
                    this.LightTipText = MessageStrings.InvalidDouble.Replace("%min", this.Param.Min.ToString()).Replace("%max", this.Param.Max.ToString());
                }
            }
            else
            {
                this.Status = ToolStatus.Empty;
                this.LightTipText = MessageStrings.ParameterInvalid;
            }

            _enableUpdate = true;
        }

        #endregion

        #region Events
        
        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (IsDecimal(textBox1.Text) == true)
                {
                    _oldText = textBox1.Text;
                    this.Param.Value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    textBox1.Text = _oldText;
                }
            }
        }
 
        /// <summary>
        /// Checks if text contains a value double
        /// </summary>
        /// <param name="theValue">The text to text</param>
        /// <returns>Returns true if it is a valid double</returns>
        private bool IsDecimal(string theValue)
        {
            try
            {
                Convert.ToDouble(theValue);
                return true;
            } 
            catch 
            {
                return false;
            }

        }

        /// <summary>
        /// When the text box is clicked this event fires
        /// </summary>
        private void textBox1_Click(object sender, EventArgs e)  
        {
            this.OnClick(e);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new DoubleParam Param
        {
            get { return (DoubleParam)base.Param; }
            set { base.Param = (Parameter)value; }
        }

        #endregion
        
        #endregion

        #region Generate by the designer
        private void InitializeializeializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);
            // 
            // lblStatus
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 20);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(440, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // DoubleElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "DoubleElement";
            this.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
