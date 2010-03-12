//********************************************************************************************************
// Product Name: MapWindow.Tools.ToolManagerToolStrip
// Description:  A tool strip designed to work along with the tool manager
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
    /// A Tool strip to use with the ToolManager when used as a graphical control
    /// </summary>
    [ToolboxItem(false)]
    public class ToolManagerToolStrip: ToolStrip
    {
        #region "Private Variables"

        private ToolManager _toolManager;
        private ToolStripTextBox _txtBoxSearch;
        private ToolStripButton _btnNewModel;
        private bool _enableFind = false;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public ToolManagerToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// Gets or sets the ToolManager currently associated with the toolstrip
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
            set { _toolManager = value; }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();

            _btnNewModel = new ToolStripButton();
            _btnNewModel.ToolTipText = MessageStrings.NewModel;
            _btnNewModel.Image = Images.NewModel.ToBitmap();
            _btnNewModel.Click += new EventHandler(_btnNewModel_Click);
            this.Items.Add(_btnNewModel);

            _txtBoxSearch = new ToolStripTextBox();
            _txtBoxSearch.Text = MapWindow.MessageStrings.FindToolByName;
            _txtBoxSearch.TextChanged +=new EventHandler(txtBoxSearch_TextChanged);
            _txtBoxSearch.Leave += new EventHandler(_txtBoxSearch_Leave);
            _txtBoxSearch.Enter += new EventHandler(_txtBoxSearch_Enter);
            _txtBoxSearch.KeyPress += new KeyPressEventHandler(_txtBoxSearch_KeyPress);
            this.Items.Add(_txtBoxSearch);

            this.ResumeLayout();
        }

        #region "Envent Handlers"

        // Fires when the user click on the find tool text box
        void _txtBoxSearch_Enter(object sender, EventArgs e)
        {
            _txtBoxSearch.Text = "";
            _enableFind = true;
        }

        // Fires when the user leaves the find tool text box
        void _txtBoxSearch_Leave(object sender, EventArgs e)
        {
            _enableFind = false;
            _txtBoxSearch.Text = MapWindow.MessageStrings.FindToolByName;
        }
        
        //Fires when the text is changed in the find tool text box and calls the toolmanager to highlight a relevant tool
        void  txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (_enableFind)
                _toolManager.HighlightTool(_txtBoxSearch.Text);
        }

        //Fires when the user hits a new, and handles it if its enter
        void _txtBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                _toolManager.HighlightNextTool(_txtBoxSearch.Text);
        }

        //Fires when the user clicks the new model tool
        void  _btnNewModel_Click(object sender, EventArgs e)
        {
           
                frmModeler aModelerForm = new frmModeler();
                aModelerForm.Modeler.ToolManager = _toolManager;
                aModelerForm.Modeler.CreateNewModel(false);
                aModelerForm.Show(this);
            
        }

        #endregion
    }
}
