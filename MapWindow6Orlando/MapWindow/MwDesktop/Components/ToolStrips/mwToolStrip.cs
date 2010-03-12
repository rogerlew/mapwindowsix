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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/8/2008 2:13:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        |  2/18/2010         |  Added zoom out button and custom mouse cursors
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using MapWindow.Drawing;
using MapWindow.Map;
using MapWindow.XML;

namespace MapWindow.Components
{


    /// <summary>
    /// mwToolBar
    /// </summary>
    [ToolboxBitmap(typeof(mwToolStrip), "ToolStrips.mwToolStrip.ico")]
    public class mwToolStrip : ToolStrip
    {

        #region Events

        /// <summary>
        /// Occurs when the print button is clicked
        /// </summary>
        public event EventHandler PrintClicked;

        #endregion

        #region Private Variables

        private ToolStripButton cmdNew;
        private ToolStripButton cmdOpen;
        private ToolStripButton cmdSave;
        private ToolStripButton cmdPrint;
        private ToolStripButton cmdAddData;
        private ToolStripButton cmdPan;
        private ToolStripButton cmdSelect;
        private ToolStripButton cmdZoom;
        private ToolStripButton cmdZoomOut;
        private ToolStripButton cmdInfo;
        private ToolStripButton cmdTable;
        private System.ComponentModel.IContainer components;
        private ToolStripButton cmdMaxExtents;
        private ToolStripButton cmdLabel;
        private ToolStripButton cmdMeasure;
        private IBasicMap _basicMap;

        //private bool boolTableIsChecked = false;
        //private bool boolInfoIsChecked = false;
        //private bool boolZoomIsChecked = false;
        //private bool boolSelectIsChecked = false;
        //private bool boolPanIsChecked = false;
        private Color checkedButtonBackColor = Color.LightYellow;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of mwToolBar
        /// </summary>
        public mwToolStrip()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Constructs and initializes this toolbar using the specified IBasicMap
        /// </summary>
        /// <param name="map">The map for the toolbar to interact with</param>
        public mwToolStrip(IBasicMap map)
        {
            Init(map);
            InitializeComponent();
        }

        
        private void Configure()
        {
          
            
        }



        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Warning: Using the normal resource manager that is used by default by the program will cause
        /// the entire application to crash in mono.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmdNew = new System.Windows.Forms.ToolStripButton();
            this.cmdOpen = new System.Windows.Forms.ToolStripButton();
            this.cmdSave = new System.Windows.Forms.ToolStripButton();
            this.cmdPrint = new System.Windows.Forms.ToolStripButton();
            this.cmdAddData = new System.Windows.Forms.ToolStripButton();
            this.cmdPan = new System.Windows.Forms.ToolStripButton();
            this.cmdSelect = new System.Windows.Forms.ToolStripButton();
            this.cmdZoom = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomOut = new System.Windows.Forms.ToolStripButton();
            this.cmdInfo = new System.Windows.Forms.ToolStripButton();
            this.cmdTable = new System.Windows.Forms.ToolStripButton();
            this.cmdMaxExtents = new System.Windows.Forms.ToolStripButton();
            this.cmdLabel = new System.Windows.Forms.ToolStripButton();
            this.cmdMeasure = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // cmdNew
            // 
            this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdNew.Image = Images.file_new;
            this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(23, 69);
            this.cmdNew.ToolTipText = "New";
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmdOpen
            // 
            this.cmdOpen.Image = Images.FolderOpen;
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(23, 69);
            this.cmdOpen.ToolTipText = "Open Project";
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Image = Images.file_saveas;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(23, 69);
            this.cmdSave.ToolTipText = "Save Project";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdPrint
            // 
            this.cmdPrint.Image = Images.printer;
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(23, 20);
            this.cmdPrint.ToolTipText = "Print";
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdAddData
            //
            this.cmdAddData.Image = Images.mnuLayerAdd;
            this.cmdAddData.Name = "cmdAddData";
            this.cmdAddData.Size = new System.Drawing.Size(23, 20);
            this.cmdAddData.ToolTipText = "Add Data";
            this.cmdAddData.Click += new System.EventHandler(this.cmdAddData_Click);
            // 
            // cmdPan
            // 
            this.cmdPan.Image = Images.Pan.ToBitmap();
            this.cmdPan.Name = "cmdPan";
            this.cmdPan.Size = new System.Drawing.Size(23, 20);
            this.cmdPan.ToolTipText = "Pan";
            this.cmdPan.Click += new System.EventHandler(this.cmdPan_Click);
            this.cmdPan.CheckedChanged += new EventHandler(cmdPan_CheckedChanged);
            // 
            // cmdSelect
            // 
            this.cmdSelect.Image = Images.select.ToBitmap();
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(23, 20);
            this.cmdSelect.ToolTipText = "Select";
            //this.cmdSelect.CheckOnClick = true;
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            this.cmdSelect.CheckedChanged += new EventHandler(cmdSelect_CheckedChanged);
            // 
            // cmdZoom
            //
            this.cmdZoom.Image = Images.zoom_in.ToBitmap();
            this.cmdZoom.Name = "cmdZoom";
            this.cmdZoom.Size = new System.Drawing.Size(23, 20);
            this.cmdZoom.ToolTipText = "Zoom In";
            //this.cmdZoom.CheckOnClick = true;
            this.cmdZoom.Click += new System.EventHandler(this.cmdZoom_Click);
            this.cmdZoom.CheckedChanged += new EventHandler(cmdZoom_CheckedChanged);
            // 
            // cmdZoomOut
            //
            this.cmdZoomOut.Image = Images.zoom_out.ToBitmap();
            this.cmdZoomOut.Name = "cmdZoomOut";
            this.cmdZoomOut.Size = new System.Drawing.Size(23, 20);
            this.cmdZoomOut.ToolTipText = "Zoom Out";
            //this.cmdZoomOut.CheckOnClick = true;
            this.cmdZoomOut.Click += new EventHandler(cmdZoomOut_Click);
            this.cmdZoomOut.CheckedChanged += new EventHandler(cmdZoom_CheckedChanged);

            // 
            // cmdInfo
            // 
            this.cmdInfo.Image = Images.info;
            this.cmdInfo.Name = "cmdInfo";
            this.cmdInfo.Size = new System.Drawing.Size(23, 20);
            this.cmdInfo.BackColor = Color.Transparent;
            this.cmdInfo.ToolTipText = "Identifier";
            //this.cmdInfo.CheckOnClick = true;
            this.cmdInfo.Click += new System.EventHandler(this.cmdInfo_Click);
            this.cmdInfo.CheckedChanged += new EventHandler(cmdInfo_CheckedChanged);
            // 
            // cmdTable
            // 
            this.cmdTable.Image = Images.Table;
            this.cmdTable.Name = "cmdTable";
            this.cmdTable.Size = new System.Drawing.Size(23, 20);
            this.cmdTable.ToolTipText = "Attribute Table";
            //this.cmdTable.CheckOnClick = true;
            this.cmdTable.Click += new System.EventHandler(this.cmdTable_Click);
            this.cmdTable.CheckedChanged += new EventHandler(cmdTable_CheckedChanged);
            // 
            // cmdMaxExtents
            // 
            this.cmdMaxExtents.Image = Images.zoom_full_extent.ToBitmap();
            this.cmdMaxExtents.Name = "cmdMaxExtents";
            this.cmdMaxExtents.Size = new System.Drawing.Size(23, 20);
            this.cmdMaxExtents.ToolTipText = "Zoom to Maximum Extents";
            this.cmdMaxExtents.Click += new System.EventHandler(this.cmdMaxExtents_Click);
        
            // 
            // cmdLabel
            // 
            this.cmdLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdLabel.Image = Images.Label;
            this.cmdLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdLabel.Name = "cmdLabel";
            this.cmdLabel.Size = new System.Drawing.Size(23, 23);
            this.cmdLabel.Click += new EventHandler(cmdLabel_Click);
            //
            // cmdMeasure
            //
            this.cmdMeasure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdMeasure.Image = Images.ScaleBar;
            this.cmdMeasure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdMeasure.Name = "cmdMeasure";
            this.cmdMeasure.Size = new System.Drawing.Size(23, 23);
            this.cmdMeasure.ToolTipText = "Measure Distance";
            //this.cmdMeasure.CheckOnClick = true;
            this.cmdMeasure.Click += new EventHandler(cmdMeasure_Click);

            // 
            // mwToolStrip
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNew,
            this.cmdOpen,
            this.cmdSave,
            this.cmdPrint,
            this.cmdAddData,
            this.cmdPan,
            this.cmdSelect,
            this.cmdZoom,
            this.cmdZoomOut,
            this.cmdInfo,
            this.cmdTable,
            this.cmdMaxExtents,
            //this.cmdLabel,
            this.cmdMeasure});
            this.Size = new System.Drawing.Size(100, 72);
            this.ResumeLayout(false);

        }

        

        /// <summary>
        /// Unchecks all toolstrip buttons except the current button
        /// </summary>
        /// <param name="checkedButton">The toolstrip button which should
        /// stay checked</param>
        void UncheckOtherButtonsButMe(ToolStripButton checkedButton)
        {
            foreach(ToolStripItem item in this.Items)
            {
                ToolStripButton buttonItem = item as ToolStripButton;
                if (buttonItem != null)
                {
                    if (buttonItem.Name != checkedButton.Name)
                    {
                        buttonItem.Checked = false;
                    }
                }
            }
        }

        void cmdTable_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void cmdInfo_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        void cmdZoom_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void cmdSelect_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void cmdPan_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }


        #endregion

        /// <summary>
        /// Allows the editing of labels
        /// </summary>
        void cmdLabel_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;
            _basicMap.FunctionMode = FunctionModes.Label;
        }

        /// <summary>
        /// Zoom to maximum extents
        /// </summary>
        void cmdMaxExtents_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            UncheckOtherButtonsButMe(cmdMaxExtents);

            _basicMap.ZoomToMaxExtent();

            _basicMap.FunctionMode = FunctionModes.None;
        }

        /// <summary>
        /// Open attribute table
        /// </summary>
        void cmdTable_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;
           
            UncheckOtherButtonsButMe(cmdTable);

            foreach (ILayer layer in _basicMap.MapFrame)
            {
                IFeatureLayer fl = layer as IFeatureLayer;

                //TODO Does this work for layers within groups??
                if (fl == null) continue;
                if (fl.IsSelected == false) continue;
                fl.ShowAttributes();
            }  
        }

        /// <summary>
        /// Identifier Tool
        /// </summary>
        void cmdInfo_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdInfo.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdInfo);
                cmdInfo.Checked = true;   

                _basicMap.FunctionMode = FunctionModes.Info;
            }
            else
            {
                cmdInfo.Checked = false;
                _basicMap.FunctionMode = FunctionModes.None;
            }
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        void cmdZoom_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdZoom.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdZoom);
                cmdZoom.Checked = true;
                _basicMap.FunctionMode = FunctionModes.Zoom;
            }
        }

        void cmdZoomOut_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdZoomOut.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdZoomOut);
                cmdZoomOut.Checked = true;
                _basicMap.FunctionMode = FunctionModes.ZoomOut;
            }
        }

        /// <summary>
        /// Select or deselect Features
        /// </summary>
        void cmdSelect_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdSelect.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdSelect);
                cmdSelect.Checked = true;
                _basicMap.FunctionMode = FunctionModes.Select;
            }
        }

        /// <summary>
        /// Move (Pan) the map
        /// </summary>
        void cmdPan_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdPan.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdPan);
                cmdPan.Checked = true;

                _basicMap.FunctionMode = FunctionModes.Pan;
            }
        }

        /// <summary>
        /// Measure Distance
        /// </summary>
        void cmdMeasure_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdMeasure.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdMeasure);
                cmdMeasure.Checked = true;
                _basicMap.FunctionMode = FunctionModes.Measure;
            }
        }

        /// <summary>
        /// Add Data to the Map
        /// </summary>
        void cmdAddData_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;
            _basicMap.AddLayer();
        }

        /// <summary>
        /// Print Map
        /// </summary>
        void cmdPrint_Click(object sender, EventArgs e)
        {
            OnPrintClicked();   
        }

        /// <summary>
        /// Fires the PrintClicked event
        /// </summary>
        protected virtual void OnPrintClicked()
        {
            if (PrintClicked != null) PrintClicked(this, new EventArgs());
        }

       
        /// <summary>
        /// Save Map
        /// </summary>
        void cmdSave_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Map Files|*.map.xml";
            dlg.SupportMultiDottedExtensions = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    XmlSerializer s = new XmlSerializer();
                    string xml = s.Serialize(Map);
                    File.WriteAllText(dlg.FileName, xml);
                }
                catch (XmlException)
                {
                    MessageBox.Show(this, "Failed to write the specified map file " + dlg.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException)
                {
                    MessageBox.Show(this, "Could not write to the specified map file " + dlg.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Open Map
        /// </summary>
        void cmdOpen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to clear all data and open a different map?", "Confirm new map", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            var dlg = new OpenFileDialog();
            dlg.Filter = "Map Files|*.map.xml";
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                XmlDeserializer d = new XmlDeserializer();
                d.Deserialize(Map, File.ReadAllText(dlg.FileName));
                Map.Invalidate();
            }
            catch (IOException)
            {
                MessageBox.Show(this, "Could not open the specified map file " + dlg.FileName, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (XmlException)
            {
                MessageBox.Show(this, "Failed to read the specified map file " + dlg.FileName, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// New Map
        /// </summary>
        void cmdNew_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(this, "Are you sure you want to clear all data and start a new map?", "Confirm new map",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
               Map.ClearLayers();
            }
        }

        #region Methods

        /// <summary>
        /// Initializes the map tool, telling it what map that it will be working with.
        /// </summary>
        /// <param name="map">Any implementation of IBasicMap that the tool can work with</param>
        public virtual void Init(IBasicMap map)
        {
            _basicMap = map;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the basic map that this toolbar will interact with by default
        /// </summary>
        public virtual IBasicMap Map
        {
            get { return _basicMap; }
            set { _basicMap = value; }
        }

        #endregion

     
    }
}
