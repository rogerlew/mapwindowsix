//********************************************************************************************************
// Product Name: MapWindow.Layout.Elements.FilteredFileNameEditor
// Description:  Enumerations used by the layout elements
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
// The Original Code is MapWindow.dll version 6.0 
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MapWindow.Forms
{

    /// <summary>
    ///  A standard open file dialog editor, but one that is custom tailored to open image path extension
    /// </summary>
    public class BitmapFileNameEditor : FileNameEditor
    {
        /// <summary>
        /// Initializes the dialog that will appear when editing the value
        /// </summary>
        /// <param name="openFileDialog"></param>
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            openFileDialog.CheckFileExists = false;
            openFileDialog.Filter = "Image (*.jpg, *.bmp, *.png)|*.jpg;*.bmp;*.png|All files (*.*)|*.*";
        }

    }
}
