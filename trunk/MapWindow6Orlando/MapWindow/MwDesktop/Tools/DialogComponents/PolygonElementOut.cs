//********************************************************************************************************
// Product Name: MapWindow.Tools.PolygonElement
// Description:  Polygon Element for use in the tool dialog
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
using MapWindow.Data;

namespace MapWindow.Tools
{
    internal class PolygonElementOut : DialogElement
    {
        #region Class Variables

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAddData;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="outputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public PolygonElementOut(PolygonFeatureSetParam outputParam, List<DataSetArray> dataSets)
        {
            //Needed by the designer
            InitializeComponent();

            //We save the parameters passed in 
            base.Param = outputParam;

            //Saves the label
            base.GroupBox.Text = Param.Name;

            //Sets up the initial status light indicator
            base.Status = ToolStatus.Empty;
            base.LightTipText = MapWindow.MessageStrings.FeaturesetMissing;

            //Populates the dialog with the default parameter value
            if (outputParam.Value != null && outputParam.DefaultSpecified == true)
            {
                textBox1.Text = outputParam.ModelName;
                base.Status = ToolStatus.Ok;
                base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Events

        private void btnAddData_Click(object sender, EventArgs e)
        {
            /////////////////////////////////
            //Replace with something that uses the default data provider
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.Filter = "Shape Files|*.shp";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IFeatureSet addedFeatureSet = new MapWindow.Data.PolygonShapefile();
                addedFeatureSet.Filename = sfd.FileName;

                //If the features set is null do nothing the user probably hit cancel
                if (addedFeatureSet == null)
                    return;

                //If the feature type is good save it
                else
                {
                    //This inserts the new featureset into the list
                    textBox1.Text = System.IO.Path.GetFileNameWithoutExtension(addedFeatureSet.Filename);
                    base.Param.Value = addedFeatureSet;
                    base.Status = ToolStatus.Ok;
                    base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
                }
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.btnAddData = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Controls.Add(this.btnAddData);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.btnAddData, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);
            // 
            // lblStatus
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 20);
            // 
            // btnAddData
            // 
            this.btnAddData.Image = global::MapWindow.Images.AddLayer;
            this.btnAddData.Location = new System.Drawing.Point(460, 14);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(26, 26);
            this.btnAddData.TabIndex = 5;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(410, 20);
            this.textBox1.TabIndex = 6;
            // 
            // PolygonElementOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "PolygonElementOut";
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }



    }
}
