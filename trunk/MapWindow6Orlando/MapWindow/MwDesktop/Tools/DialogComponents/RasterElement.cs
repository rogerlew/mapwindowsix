//********************************************************************************************************
// Product Name: MapWindow.Tools.RasterElement
// Description:  Raster Element for use in the tool dialog
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
using System.Windows.Forms;
using MapWindow.Tools.Param;
using MapWindow.Data;

namespace MapWindow.Tools
{
    internal class RasterElement : DialogElement
    {
        
        #region Class Variables
        
        private System.Windows.Forms.ComboBox comboRaster;
        private System.Windows.Forms.Button btnAddData;
        private DataSetArray _addedRasterSet;
        private List<DataSetArray> _dataSets;
        private Boolean _refreshCombo = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public RasterElement(RasterParam inputParam, List<DataSetArray> dataSets)
        {
            //Needed by the designer
            InitializeComponent();

            //We save the parameters passed in 
            base.Param = inputParam;

            _dataSets = dataSets;

            //Saves the label
            this.GroupBox.Text = Param.Name;

            Refresh();
        }

        #endregion

        #region methods

        /// <summary>
        /// updates the param if something's been changed
        /// </summary>
        public override void Refresh()
        {
            //Disable the combo box temporarily
            _refreshCombo = false;

            //We set the combo boxes status to empty to start
            base.Status = ToolStatus.Empty;
            base.LightTipText = MapWindow.MessageStrings.FeaturesetMissing;
            comboRaster.Items.Clear();

            //If the user added a raster set
            if (_addedRasterSet != null)
            {
                comboRaster.Items.Add(_addedRasterSet);
                if (base.Param.Value != null && base.Param.DefaultSpecified == true)
                {
                    if (_addedRasterSet.DataSet == base.Param.Value)
                    {
                        comboRaster.SelectedItem = _addedRasterSet;
                        base.Status = ToolStatus.Ok;
                        base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
                    }
                }
            }

            //Add all the dataSets back to the combo box
            if (_dataSets != null)
            {
                foreach (DataSetArray dsa in _dataSets)
                {
                    IRaster aRasterSet = (dsa.DataSet as IRaster);
                    if (aRasterSet != null)
                    {
                        //If the featureset is the correct type and isn't already in the combo box we add it
                        if (comboRaster.Items.Contains(dsa) == false)
                        {
                            comboRaster.Items.Add(dsa);
                            if (base.Param.Value != null && base.Param.DefaultSpecified == true)
                            {
                                if (dsa.DataSet == base.Param.Value)
                                {
                                    comboRaster.SelectedItem = dsa;
                                    base.Status = ToolStatus.Ok;
                                    base.LightTipText = MapWindow.MessageStrings.FeaturesetValid;
                                }
                            }
                        }
                    }
                }
            }

            _refreshCombo = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        void comboRaster_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_refreshCombo)
            {
                DataSetArray dsa = comboRaster.SelectedItem as DataSetArray;
                if (dsa != null)
                {
                    base.Param.ModelName = dsa.Name;
                    base.Param.Value = dsa.DataSet;
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a new entry to the drop down list from data provider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddData_Click(object sender, EventArgs e)
        {

            /////////////////////////////////
            //Replace with something that uses the default data provider
            IRaster tempRaster = MapWindow.Components.DataManager.DefaultDataManager.OpenRaster();
            
            //If the feature is null don't do anything the user probably hit cancel on the dialog
            if (tempRaster == null)
                return;

            //If its good add the feature set and save it
            else
            {
                _addedRasterSet = new DataSetArray(System.IO.Path.GetFileNameWithoutExtension(tempRaster.Filename), tempRaster);
                base.Param.ModelName = _addedRasterSet.Name;
                base.Param.Value = _addedRasterSet.DataSet as object;
            }
        }

        #endregion

        #region Generate by the designer

        private void InitializeComponent()
        {
            this.btnAddData = new System.Windows.Forms.Button();
            this.comboRaster = new System.Windows.Forms.ComboBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.GroupBox.Controls.Add(this.comboRaster);
            this.GroupBox.Controls.Add(this.btnAddData);
            this.GroupBox.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.btnAddData, 0);
            this.GroupBox.Controls.SetChildIndex(this.comboRaster, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            // 
            // btnAddData
            // 
            this.btnAddData.Image = global::MapWindow.Images.AddLayer;
            this.btnAddData.Location = new System.Drawing.Point(460, 14);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(26, 26);
            this.btnAddData.TabIndex = 4;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // comboFeatures
            // 
            this.comboRaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRaster.FormattingEnabled = true;
            this.comboRaster.Location = new System.Drawing.Point(44, 17);
            this.comboRaster.Name = "comboFeatures";
            this.comboRaster.Size = new System.Drawing.Size(410, 21);
            this.comboRaster.TabIndex = 5;
            this.comboRaster.SelectedValueChanged +=new EventHandler(comboRaster_SelectedValueChanged);
            this.comboRaster.Click += new EventHandler(base.DialogElement_Click);
            // 
            // RasterElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "RasterElement";
            this.Size = new System.Drawing.Size(492, 46);
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
