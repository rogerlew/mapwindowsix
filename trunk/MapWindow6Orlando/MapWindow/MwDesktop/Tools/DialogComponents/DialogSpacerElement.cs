//********************************************************************************************************
// Product Name: MapWindow.Tools.DialogSpacerElement
// Description:  DialogSpacerElement Element for use in the tool dialog
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
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Tools
{
    internal class DialogSpacerElement : DialogElement
    {
        private System.Windows.Forms.Label label1;

        /// <summary>
        /// Creates an instance of a dialogspacerelement
        /// </summary>
        public DialogSpacerElement()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the text of the spacer
        /// </summary>
        override public String Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        /// <summary>
        /// Gets the current status the input
        /// </summary>
        override public ToolStatus Status
        {
            set { }
            get {return ToolStatus.Ok;}
        }

        private void label1_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Size = new System.Drawing.Size(492, 33);
            this.GroupBox.Visible = false;
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            // 
            // lblStatus
            // 
            this.StatusLabel.Visible = false;
            // 
            // label1
            // 
            this.Controls.Add(this.label1);
            this.Controls.SetChildIndex(this.label1, 0);
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "asdfsdfs";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // DialogSpacerElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "DialogSpacerElement";
            this.Size = new System.Drawing.Size(492, 33);
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
