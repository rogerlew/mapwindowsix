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
    /// This will Pop when user want to rename the field
    /// </summary>
    public partial class ReNameField : Form
    {
        private readonly List<string> _field = new List<string>();
        private string[] _compinationName={"",""};

        /// <summary>
        /// get or set ResultCombination
        /// </summary>
        public string[] ResultCombination
        {
            set { _compinationName = value; }
            get { return _compinationName; }
        }
        /// <summary>
        /// Constructs a new instance of the form for renaming a field
        /// </summary>
        public ReNameField()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of the form for renaming a field
        /// </summary>
        /// <param name="field"></param>
        public ReNameField(List<string> field)
        {
            InitializeComponent();
            _field = field;
            foreach (string st in _field)
                cmbField.Items.Add(st);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _compinationName[0] = cmbField.SelectedItem as string;
            _compinationName[1] = txtName.Text;
            DialogResult = DialogResult.OK;
        }

    }
}
