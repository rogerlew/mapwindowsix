//********************************************************************************************************
// Product Name: MapWindow.Tools.ModelerToolStrip
// Description:  A tool strip designed to work along with the modeler
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Apr, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapWindow.Tools
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(false)]
    public class ModelerToolStrip: ToolStrip
    {
        #region "Private Variables"

        private Modeler _modeler;
        private ToolStripButton _btnNewModel;
        private ToolStripButton _btnSaveModel;
        private ToolStripButton _btnLoadModel;

        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripButton _btnZoomFullExtent;

        private ToolStripButton _btnAddData;
        private ToolStripButton _btnLink;
        private ToolStripButton _btnDelete;

        private ToolStripButton _btnRun;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public ModelerToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// Gets or sets the modeler currently associated with the toolstrip
        /// </summary>
        public Modeler Modeler
        {
            get { return _modeler; }
            set { _modeler = value; }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();

            //New model button
            _btnNewModel = new ToolStripButton();
            _btnNewModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnNewModel.Image = Images.file_new;
            _btnNewModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnNewModel.Name = "btnNewModel";
            _btnNewModel.Size = new System.Drawing.Size(23, 22);
            _btnNewModel.Text = MessageStrings.ModelTipNew;
            //_btnNewModel.Click 
            
            //save model button
            _btnSaveModel = new ToolStripButton();
            _btnSaveModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnSaveModel.Image = Images.file_saveas;
            _btnSaveModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnSaveModel.Name = "btnSaveModel";
            _btnSaveModel.Size = new System.Drawing.Size(23, 22);
            _btnSaveModel.Text = MessageStrings.ModelTipSave;
            _btnSaveModel.Click += new EventHandler(_btnSaveModel_Click);
            
            //Load model button
            _btnLoadModel = new ToolStripButton();
            _btnLoadModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnLoadModel.Image = Images.FolderOpen;
            _btnLoadModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnLoadModel.Name = "btnLoadModel";
            _btnLoadModel.Size = new System.Drawing.Size(23, 22);
            _btnLoadModel.Text = MessageStrings.ModelTipLoad;
            _btnLoadModel.Click += new EventHandler(_btnLoadModel_Click);

            //Zoom In button
            _btnZoomIn = new ToolStripButton();
            _btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnZoomIn.Image = Images.zoom_in.ToBitmap();
            _btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnZoomIn.Name = "btnZoomIn";
            _btnZoomIn.Size = new System.Drawing.Size(23, 22);
            _btnZoomIn.Text = MessageStrings.ModelTipZoonIn;
            _btnZoomIn.Click += new EventHandler(_btnZoomIn_Click);

            //Zoom out button
            _btnZoomOut = new ToolStripButton();
            _btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnZoomOut.Image = Images.zoom_out.ToBitmap();
            _btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnZoomOut.Name = "btnZoomOut";
            _btnZoomOut.Size = new System.Drawing.Size(23, 22);
            _btnZoomOut.Text = MessageStrings.ModelTipZoomOut;
            _btnZoomOut.Click += new EventHandler(_btnZoomOut_Click);

            //Zoom full extent
            _btnZoomFullExtent = new ToolStripButton();
            _btnZoomFullExtent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnZoomFullExtent.Image = Images.zoom_full_extent.ToBitmap();
            _btnZoomFullExtent.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnZoomFullExtent.Name = "btnZoomOut";
            _btnZoomFullExtent.Size = new System.Drawing.Size(23, 22);
            _btnZoomFullExtent.Text = MessageStrings.ModelTipFullExtent;
            _btnZoomFullExtent.Click += new EventHandler(_btnZoomFullExtent_Click);

            //Add data button
            _btnAddData = new ToolStripButton();
            _btnAddData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnAddData.Image = Images.AddLayer;
            _btnAddData.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnAddData.Name = "btnLink";
            _btnAddData.Size = new System.Drawing.Size(23, 22);
            _btnAddData.Text = MessageStrings.ModelTipAddData;
           // _btnAddData.Click += new EventHandler(_btnLink_Click);

            //Zoom link tools
            _btnLink = new ToolStripButton();
            _btnLink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnLink.Image = Images.LinkData;
            _btnLink.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnLink.Name = "btnLink";
            _btnLink.Size = new System.Drawing.Size(23, 22);
            _btnLink.Text = MessageStrings.ModelTipLink;
            _btnLink.Click += new EventHandler(_btnLink_Click);

            //delete stuff
            _btnDelete = new ToolStripButton();
            _btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnDelete.Image = Images.mnuLayerClear;
            _btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnDelete.Name = "btnLink";
            _btnDelete.Size = new System.Drawing.Size(23, 22);
            _btnDelete.Text = MessageStrings.ModelTipDelete;
            _btnDelete.Click += new EventHandler(_btnDelete_Click);

            //delete stuff
            _btnRun = new ToolStripButton();
            _btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _btnRun.Image = Images.RunModel;
            _btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnRun.Name = "btnLink";
            _btnRun.Size = new System.Drawing.Size(23, 22);
            _btnRun.Text = MessageStrings.ModelTipRunModel;
            _btnRun.Click += new EventHandler(_btnRun_Click);

            //Adds all the buttons to the toolstrip
            this.Items.Add(_btnNewModel);
            this.Items.Add(_btnLoadModel);
            this.Items.Add(_btnSaveModel);
            this.Items.Add(new ToolStripSeparator());
            this.Items.Add(_btnZoomIn);
            this.Items.Add(_btnZoomOut);
            this.Items.Add(_btnZoomFullExtent);
            this.Items.Add(new ToolStripSeparator());
            this.Items.Add(_btnAddData);
            this.Items.Add(_btnLink);
            this.Items.Add(_btnDelete);
            this.Items.Add(new ToolStripSeparator());
            this.Items.Add(_btnRun);


            this.ResumeLayout();
        }

        void _btnRun_Click(object sender, EventArgs e)
        {
            this.Parent.Enabled = false;
            string error;
            _modeler.ExecuteModel(out error);
            this.Parent.Enabled = true;
        }

        void _btnLoadModel_Click(object sender, EventArgs e)
        {
            _modeler.LoadModel();
        }

        void _btnSaveModel_Click(object sender, EventArgs e)
        {
            _modeler.SaveModel();
        }

        #region "Envent Handlers"

        //Fires when the user clicks the delete button
        void _btnDelete_Click(object sender, EventArgs e)
        {
            _modeler.DeleteSelectedElements();
        }

        //Fires when the user clicks the link button
        void _btnLink_Click(object sender, EventArgs e)
        {
            if (_modeler.EnableLinking == true)
            {
                _btnLink.Checked = false;
                _modeler.EnableLinking = false;
            }
            else
            {
                _btnLink.Checked = true;
                _modeler.EnableLinking = true;
            }
        }

        //Fires when the user clicks the zoom to full extent button
        void _btnZoomFullExtent_Click(object sender, EventArgs e)
        {
            _modeler.ZoomFullExtent();
        }

        //Fires the zoom in control on the modeler
        void _btnZoomIn_Click(object sender, EventArgs e)
        {
            _modeler.ZoomIn();
        }

        //Fires the zoom out control on the modeler
        void _btnZoomOut_Click(object sender, EventArgs e)
        {
            _modeler.ZoomOut();
        }

        #endregion
    }
}
