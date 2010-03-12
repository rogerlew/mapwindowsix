//********************************************************************************************************
// Product Name: MapWindow.Layout.LayoutDocToolStrip
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
    public class LayoutDocToolStrip : ToolStrip
    {
        #region "Private Variables"

        private LayoutControl _layoutControl;
        private ToolStripButton _btnNew;
        private ToolStripButton _btnOpen;
        private ToolStripButton _btnSave;
        private ToolStripButton _btnSaveAs;
        private ToolStripButton _btnPrint;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutDocToolStrip()
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
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnOpen = new System.Windows.Forms.ToolStripButton();
            this._btnSave = new System.Windows.Forms.ToolStripButton();
            this._btnSaveAs = new System.Windows.Forms.ToolStripButton();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // _btnZoomIn
            // 
            this._btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNew.Image = Images.file_new;
            this._btnNew.Size = new System.Drawing.Size(23, 22);
            this._btnNew.Text = MessageStrings.LayoutMenuStripNew;
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _btnZoomOut
            // 
            this._btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpen.Image = Images.FolderOpen;
            this._btnOpen.Size = new System.Drawing.Size(23, 22);
            this._btnOpen.Text = MessageStrings.LayoutMenuStripOpen;
            this._btnOpen.Click += new System.EventHandler(this._btnOpen_Click);
            // 
            // _btnZoomFullExtent
            // 
            this._btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSave.Image = Images.save.ToBitmap();
            this._btnSave.Size = new System.Drawing.Size(23, 22);
            this._btnSave.Text = MessageStrings.LayoutMenuStripSave;
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // _comboZoom
            // 
            this._btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSaveAs.Image = Images.file_saveas;
            this._btnSaveAs.Size = new System.Drawing.Size(23, 22);
            this._btnSaveAs.Text = MessageStrings.LayoutMenuStripSaveAs;
            this._btnSaveAs.Click += new EventHandler(_btnSaveAs_Click);

            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = Images.printer;
            this._btnPrint.Size = new System.Drawing.Size(23, 22);
            this._btnPrint.Text = MessageStrings.LayoutMenuStripPrint;
            this._btnPrint.Click += new EventHandler(_btnPrint_Click);

            // 
            // LayoutToolStrip
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnOpen,
            this._btnSave,
            this._btnSaveAs,
            new ToolStripSeparator(),
            this._btnPrint});
            this.ResumeLayout(false);

        }


        #region "Envent Handlers"

        //Fires the print method on the layoutcontrol
        void _btnPrint_Click(object sender, EventArgs e)
        {
            _layoutControl.Print();
        }
 
        //Fires the saveas method on the layoutcontrol
        void _btnSaveAs_Click(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(true);
        }

        //Fires the save method on the layoutcontrol
        void _btnSave_Click(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(false);
        }

        //Fires the new method on the layoutcontrol
        void _btnNew_Click(object sender, EventArgs e)
        {
            _layoutControl.NewLayout(true);
        }

        //Fires the open method on the layoutcontrol
        void _btnOpen_Click(object sender, EventArgs e)
        {
            _layoutControl.LoadLayout(true);
        }

        #endregion
    }
}

