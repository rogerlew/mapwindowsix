//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutMapToolStrip
// Description:  A tool strip designed to work along with the layout engine
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
// The Original Code is MapWindow.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace MapWindow.Layout
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(false)]
    public class LayoutMapToolStrip : ToolStrip
    {
        #region "Private Variables"

        private LayoutControl _layoutControl;
        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripButton _btnPan;
        private ToolStripButton _btnZoomFullExtent;
        private ToolStripButton _btnZoomViewExtent;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutMapToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// The layout control associated with this toolstrip
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set 
            {
                _layoutControl = value;
                if (_layoutControl == null) return;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutZoomToolStrip));
            this._btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this._btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this._btnZoomFullExtent = new System.Windows.Forms.ToolStripButton();
            this._btnZoomViewExtent = new System.Windows.Forms.ToolStripButton();
            this._btnPan = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // _btnZoomIn
            // 
            this._btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomIn.Image = Images.ZoomInMap;
            this._btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomIn.Name = "_btnZoomIn";
            this._btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this._btnZoomIn.Text = global::MapWindow.MessageStrings.LayoutMapToolStripZoomIn;
            this._btnZoomIn.Click += new System.EventHandler(this._btnZoomIn_Click);
            // 
            // _btnZoomOut
            // 
            this._btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomOut.Image = Images.ZoomOutMap;
            this._btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomOut.Name = "_btnZoomOut";
            this._btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this._btnZoomOut.Text = global::MapWindow.MessageStrings.LayoutMapToolStripZoomOut;
            this._btnZoomOut.Click += new System.EventHandler(this._btnZoomOut_Click);
            // 
            // _btnZoomFullExtent
            // 
            this._btnZoomFullExtent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomFullExtent.Image = Images.ZoomFullMap;
            this._btnZoomFullExtent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomFullExtent.Name = "_btnZoomFullExtent";
            this._btnZoomFullExtent.Size = new System.Drawing.Size(23, 22);
            this._btnZoomFullExtent.Text = global::MapWindow.MessageStrings.LayoutMapToolStripMaxExtent;
            this._btnZoomFullExtent.Click += new System.EventHandler(this._btnZoomFullExtent_Click);
            // 
            // _btnZoomFullExtent
            // 
            this._btnZoomViewExtent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomViewExtent.Image = Images.ZoomFullView;
            this._btnZoomViewExtent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomViewExtent.Name = "_btnZoomViewExtent";
            this._btnZoomViewExtent.Size = new System.Drawing.Size(23, 22);
            this._btnZoomViewExtent.Text = global::MapWindow.MessageStrings.LayoutMapToolStripViewExtent;
            this._btnZoomViewExtent.Click += new EventHandler(_btnZoomViewExtent_Click);          
            // 
            // _btnPan
            // 
            this._btnPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPan.Image = Images.PanMap;
            this._btnPan.CheckOnClick = true;
            this._btnPan.Checked = false;
            this._btnPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnPan.Name = "_btnZoomFullExtent";
            this._btnPan.Size = new System.Drawing.Size(23, 22);
            this._btnPan.Text = global::MapWindow.MessageStrings.LayoutMapToolStripPan;
            this._btnPan.Click +=new EventHandler(_btnPan_Click);
            // 
            // LayoutToolStrip
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnZoomIn,
            this._btnZoomOut,
            this._btnZoomFullExtent,
            this._btnZoomViewExtent,
            this._btnPan});
            this.EnabledChanged += new EventHandler(LayoutMapToolStrip_EnabledChanged);
            this.Enabled = false;
            this.ResumeLayout(false);

        }

        #region "Envent Handlers"

        //Fires when the user clicks the zoom to full extent button
        void _btnZoomFullExtent_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomFullExtentMap(_layoutControl.SelectedLayoutElements[0] as Elements.LayoutMap);
        }

        //Fires the zoom in control on the modeler
        void _btnZoomIn_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomInMap(_layoutControl.SelectedLayoutElements[0] as Elements.LayoutMap);
        }

        //Fires the zoom out control on the modeler
        void _btnZoomOut_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomOutMap(_layoutControl.SelectedLayoutElements[0] as Elements.LayoutMap);
        }

        //Fires when the user clicks the pan button
        void  _btnPan_Click(object sender, EventArgs e)
        {
            if (_btnPan.Checked == true)
                _layoutControl.MapPanMode = true;
            else
                _layoutControl.MapPanMode = false;
        }

        //If the toolbar is disabled we disable the pan checked button state
        void LayoutMapToolStrip_EnabledChanged(object sender, EventArgs e)
        {
            _btnPan.Checked = false;
        }

        //Zoom the map to the extent of the layout
        void _btnZoomViewExtent_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomFullViewExtentMap(_layoutControl.SelectedLayoutElements[0] as Elements.LayoutMap);
        }

        #endregion
    }
}

