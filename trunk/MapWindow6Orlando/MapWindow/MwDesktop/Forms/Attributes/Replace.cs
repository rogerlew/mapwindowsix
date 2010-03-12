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
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/17/09.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
//********************************************************************************************************
using System;
using System.Windows.Forms;

namespace MapWindow.Forms
{
    /// <summary>
    /// This form diplay to user to find perticular value or string in DataGridView and replace.
    /// </summary>
    public partial class Replace : Form
    {
        #region Variable

        private string _find;
        private string _replace;

        /// <summary>
        /// get the Find String 
        /// </summary>
        public string FindString
        {
            //set { _find = value; }
            get { return _find; }
        }

        /// <summary>
        /// get the ReplaceString
        /// </summary>
        public string ReplaceString
        {
            //set { _replace = value; }
            get { return _replace; }
        }

        #endregion
        
        /// <summary>
        /// Creates a new instance of the replace form.
        /// </summary>
        public Replace()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _find = txtFind.Text;
            _replace = txtReplace.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
