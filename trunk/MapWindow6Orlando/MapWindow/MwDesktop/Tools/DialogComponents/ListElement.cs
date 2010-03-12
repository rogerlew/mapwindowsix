//********************************************************************************************************
// Product Name: MapWindow.Tools.ListElement
// Description:  List Element for use in the tool dialog
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
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    internal class ListElement : DialogElement
    {

        #region Class Variables

        private System.Windows.Forms.ComboBox comboBox1;
        private Boolean _disableComboRefresh = false;
        
        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public ListElement(ListParam param)
        {
            //Needed by the designer
            InitializeComponent();
            this.GroupBox.Text = param.Name;
            
            //We save the parameters passed in 
            Param = param;

            Refresh();

            //Update the state of the status light
            comboBox1_SelectedValueChanged(null, null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Refresh()
        {
            _disableComboRefresh = true;

            comboBox1.Items.Clear();

            //We load the items in the list
            for (int i = 0; i < Param.ValueList.Count; i++)
            {
                comboBox1.Items.Insert(i, Param.ValueList[i]);
            }

            //We set the default value
            if ((base.Param.DefaultSpecified) && (Param.Value >= 0))
            {
                comboBox1.SelectedIndex = Param.Value;
                this.Status = ToolStatus.Ok;
                this.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
            }

            _disableComboRefresh = false;
        }

        #endregion

        #region Events
        
        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_disableComboRefresh == true) return;

            if (comboBox1.SelectedItem == null)
            {
                this.Status = ToolStatus.Empty;
                this.LightTipText = "The seletion is invalid, select and item from the drop down list.";
            }
            else
            {
                this.Status = ToolStatus.Ok;
                this.LightTipText = "The selection is valid";
                this.Param.Value = comboBox1.SelectedIndex;
                return;
            }
        }
 
        /// <summary>
        /// When the control is clicked this event fires
        /// </summary>
        void comboBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new ListParam Param
        {
            get { return (ListParam)base.Param; }
            set { base.Param = (Parameter)value; }
        }

        #endregion

        #region Generate by the designer
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.comboBox1);
            this.GroupBox.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.comboBox1, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            // 
            // lblStatus
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 20);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(44, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(440, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            this.comboBox1.Click += new System.EventHandler(this.comboBox1_Click);
            // 
            // ListElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ListElement";
            this.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }




        #endregion
    }
}
