//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 3:35:43 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IPropertyDialogProvider
    /// </summary>
    public interface IPropertyDialogProvider
    {
        #region Events

        /// <summary>
        /// Fires an event signifying that the item has been updated
        /// </summary>
        event EventHandler<ChangedObjectEventArgs> ChangesApplied;

        #endregion

        #region Methods

        /// <summary>
        /// Shows a PropertyGrid Dialog and uses the specified object as the edit copy.
        /// </summary>
        /// <param name="editCopy">A clone of the actual object to be edited.  This should not be the original object.</param>
        void ShowDialog(object editCopy);
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that was changed by this operation.
        /// </summary>
        object ChangeItem
        {
            get;
        }

        #endregion

        
    }
}
