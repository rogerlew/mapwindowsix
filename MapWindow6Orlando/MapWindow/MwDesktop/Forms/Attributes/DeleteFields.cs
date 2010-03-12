//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/15/09.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MapWindow.Forms
{
    /// <summary>
    /// This will load to user select field to remove.
    /// </summary>
    public partial class frmDeleteField : Form
    {
        #region Private Variables

        private readonly List<string> _fields = new List<string>();
        private List<string> _fieldSelected = new List<string>();

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the form for deleting a field
        /// </summary>
        public frmDeleteField()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of the form for deleting a field while passing in a new field.
        /// </summary>
        /// <param name="fields"></param>
        public frmDeleteField(List<string> fields)
        {
            InitializeComponent();
            _fields=fields;
            foreach (string st in _fields)
            {
                clb.Items.Add(st, false);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// get or set the list of selected Fields
        /// </summary>
        public List<String> SelectedFieldIdList
        {
            get { return _fieldSelected; }
            set { _fieldSelected = value; }
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _fieldSelected.Clear();
            CheckedListBox.CheckedItemCollection sItems = clb.CheckedItems;
            foreach(string st in sItems)
                _fieldSelected.Add(st);
            DialogResult = DialogResult.OK;
        }

        private void frmDeleteField_Load(object sender, EventArgs e)
        {

        }
    }
}
