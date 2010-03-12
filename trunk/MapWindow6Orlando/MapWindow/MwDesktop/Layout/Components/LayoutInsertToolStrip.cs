//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutInsertToolStrip
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Aug, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//************************************************+********************************************************


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace MapWindow.Layout
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(false)]
    public class LayoutInsertToolStrip : ToolStrip
    {
        #region "Private Variables"

        private LayoutControl _layoutControl;
        private ToolStripButton _btnMap;
        private ToolStripButton _btnText;
        private ToolStripButton _btnRectangle;
        private ToolStripButton _btnNorthArrow;
        private ToolStripButton _btnBitmap;
        private ToolStripButton _btnScaleBar;
        private ToolStripButton _btnLegend;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutInsertToolStrip()
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
            set { _layoutControl = value; if (_layoutControl == null) return; }
        }

        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutZoomToolStrip));
            this._btnMap = new System.Windows.Forms.ToolStripButton();
            this._btnText = new System.Windows.Forms.ToolStripButton();
            this._btnRectangle = new System.Windows.Forms.ToolStripButton();
            this._btnNorthArrow = new System.Windows.Forms.ToolStripButton();
            this._btnBitmap = new System.Windows.Forms.ToolStripButton();
            this._btnScaleBar = new System.Windows.Forms.ToolStripButton();
            this._btnLegend = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // _btnMap
            // 
            this._btnMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnMap.Image = Images.map;
            this._btnMap.Size = new System.Drawing.Size(23, 22);
            this._btnMap.Text = MessageStrings.LayoutInsertToolStripMap;
            this._btnMap.Click += new System.EventHandler(this._btnMap_Click);
            // 
            // _btnText
            // 
            this._btnText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnText.Image = Images.text;
            this._btnText.Size = new System.Drawing.Size(23, 22);
            this._btnText.Text = MessageStrings.LayoutInsertToolStripText;
            this._btnText.Click += new System.EventHandler(this._btnText_Click);
            // 
            // _btnRectangle
            // 
            this._btnRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnRectangle.Image = Images.Rectangle;
            this._btnRectangle.Size = new System.Drawing.Size(23, 22);
            this._btnRectangle.Text = MessageStrings.LayoutInsertToolStripRectangle;
            this._btnRectangle.Click += new System.EventHandler(this._btnRectangle_Click);
            // 
            // _comboNorthArrow
            // 
            this._btnNorthArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNorthArrow.Image = Images.NorthArrow;
            this._btnNorthArrow.Size = new System.Drawing.Size(23, 22);
            this._btnNorthArrow.Text = MessageStrings.LayoutInsertToolStripNorthArrow;
            this._btnNorthArrow.Click += new EventHandler(_btnNorthArrow_Click);

            //_Insert Scale bar
            this._btnScaleBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnScaleBar.Image = Images.ScaleBar;
            this._btnScaleBar.Size = new System.Drawing.Size(23, 22);
            this._btnScaleBar.Text = MessageStrings.LayoutInsertMenuStripScaleBar;
            this._btnScaleBar.Click += new EventHandler(_btnScaleBar_Click);

            //_Insert Legend
            this._btnLegend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnLegend.Image = Images.Legend;
            this._btnLegend.Size = new System.Drawing.Size(23, 22);
            this._btnLegend.Text = MessageStrings.LayoutInsertMenuStripLegend;
            this._btnLegend.Click += new EventHandler(_btnLegend_Click);

            //_Insert Bitmap
            this._btnBitmap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnBitmap.Image = Images.Apple;
            this._btnBitmap.Size = new System.Drawing.Size(23, 22);
            this._btnBitmap.Text = MessageStrings.LayoutInsertToolStripBitmap;
            this._btnBitmap.Click += new EventHandler(_btnBitmap_Click);

            // 
            // LayoutToolStrip
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnMap,
            this._btnNorthArrow,
            this._btnLegend,
            this._btnScaleBar,
            this._btnText,
            this._btnRectangle,
            this._btnBitmap});
            this.ResumeLayout(false);

        }

        #region "Envent Handlers"

        void _btnLegend_Click(object sender, EventArgs e)
        {
            Elements.LayoutLegend lsb = _layoutControl.CreateLegendElement() as Elements.LayoutLegend;
            List<Elements.LayoutElement> mapElements = _layoutControl.LayoutElements.FindAll(delegate(Elements.LayoutElement o) { return (o is Elements.LayoutMap); });
            if (mapElements.Count > 0)
                lsb.Map = mapElements[0] as Elements.LayoutMap;
            lsb.LayoutControl = _layoutControl;
            _layoutControl.AddElementWithMouse(lsb);
        }

        //Adds a scale bar element to the layout and if there is already a map on the form we link it to the first one
        void _btnScaleBar_Click(object sender, EventArgs e)
        {
            Elements.LayoutScaleBar lsb = _layoutControl.CreateScaleBarElement() as Elements.LayoutScaleBar;
            List<Elements.LayoutElement> mapElements = _layoutControl.LayoutElements.FindAll(delegate(Elements.LayoutElement o) { return (o is Elements.LayoutMap); });
            if (mapElements.Count > 0)
                lsb.Map = mapElements[0] as Elements.LayoutMap;
            lsb.LayoutControl = _layoutControl;
            _layoutControl.AddElementWithMouse(lsb);
        }

        //Fires the print method on the layoutcontrol
        void _btnMap_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(_layoutControl.CreateMapElement());
        }

        //Fires the saveas method on the layoutcontrol
        void _btnText_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new Layout.Elements.LayoutText());
        }

        //Fires the save method on the layoutcontrol
        void _btnRectangle_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new Layout.Elements.LayoutRectangle());
        }

        //Fires the new method on the layoutcontrol
        void _btnNorthArrow_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new Layout.Elements.LayoutNorthArrow());
        }

        //Fires the open method on the layoutcontrol
        void _btnBitmap_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.png, *.jpg, *.bmp, *.gif, *.tif)|*.png;*.jpg;*.bmp;*.gif;*.tif";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Layout.Elements.LayoutBitmap newBitmap = new Layout.Elements.LayoutBitmap();
                newBitmap.Size = new System.Drawing.SizeF(100, 100);
                newBitmap.Filename = ofd.FileName;
                _layoutControl.AddElementWithMouse(newBitmap);
            }
        }

        #endregion
    }
}

