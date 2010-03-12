//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutToolStrip
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
    public class LayoutZoomToolStrip : ToolStrip
    {
        #region "Private Variables"

        private LayoutControl _layoutControl;
        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripButton _btnZoomFullExtent;
        private ToolStripComboBox _comboZoom;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutZoomToolStrip()
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
                _layoutControl.ZoomChanged += new EventHandler(_layoutControl_ZoomChanged);
                _layoutControl_ZoomChanged(null, null);
            }
        }

        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutZoomToolStrip));
            this._btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this._btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this._btnZoomFullExtent = new System.Windows.Forms.ToolStripButton();
            this._comboZoom = new System.Windows.Forms.ToolStripComboBox();
            this.SuspendLayout();
            // 
            // _btnZoomIn
            // 
            this._btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomIn.Image = Images.layout_zoom_in.ToBitmap();
            this._btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomIn.Name = "_btnZoomIn";
            this._btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this._btnZoomIn.Text = global::MapWindow.MessageStrings.LayoutToolStripZoomIn;
            this._btnZoomIn.Click += new System.EventHandler(this._btnZoomIn_Click);
            // 
            // _btnZoomOut
            // 
            this._btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomOut.Image = Images.layout_zoom_out.ToBitmap();
            this._btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomOut.Name = "_btnZoomOut";
            this._btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this._btnZoomOut.Text = global::MapWindow.MessageStrings.LayoutToolStripZoomOut;
            this._btnZoomOut.Click += new System.EventHandler(this._btnZoomOut_Click);
            // 
            // _btnZoomFullExtent
            // 
            this._btnZoomFullExtent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnZoomFullExtent.Image = Images.layout_zoom_full_extent.ToBitmap();
            this._btnZoomFullExtent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoomFullExtent.Name = "_btnZoomFullExtent";
            this._btnZoomFullExtent.Size = new System.Drawing.Size(23, 22);
            this._btnZoomFullExtent.Text = global::MapWindow.MessageStrings.LayoutToolStripZoomFull;
            this._btnZoomFullExtent.Click += new System.EventHandler(this._btnZoomFullExtent_Click);
            // 
            // _comboZoom
            // 
            this._comboZoom.Items.AddRange(new object[] {
            "50%",
            "75%",
            "100%",
            "150%",
            "200%",
            "300%"});
            this._comboZoom.Name = "_comboZoom";
            this._comboZoom.Size = new System.Drawing.Size(75, 21);
            this._comboZoom.SelectedIndexChanged += new System.EventHandler(this._comboZoom_SelectedIndexChanged);
            this._comboZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._comboZoom_KeyPress);
            // 
            // LayoutToolStrip
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnZoomIn,
            this._btnZoomOut,
            this._btnZoomFullExtent,
            this._comboZoom});
            this.ResumeLayout(false);

        }

        #region "Envent Handlers"

        void _comboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = _comboZoom.Text.Replace("%", "");
            _layoutControl.Zoom = Convert.ToInt32(input) / 100F;
        }

        void _comboZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                try
                {
                    string input = _comboZoom.Text.Replace("%","");
                    float newZoom = Convert.ToInt32(input) / 100F;
                    _layoutControl.Zoom = newZoom;
                }
                catch
                {
                    _comboZoom.Text = (_layoutControl.Zoom * 100).ToString() + "%";
                }
            }
        }

        void _layoutControl_ZoomChanged(object sender, EventArgs e)
        {
            _comboZoom.Text = String.Format("{0:0}", _layoutControl.Zoom * 100) + "%";
        }

        //Fires when the user clicks the zoom to full extent button
        void _btnZoomFullExtent_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomFitToScreen();
        }

        //Fires the zoom in control on the modeler
        void _btnZoomIn_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomIn();
        }

        //Fires the zoom out control on the modeler
        void _btnZoomOut_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomOut();
        }

        #endregion
    }
}

