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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 3:24:43 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Drawing
{


    /// <summary>
    /// ChangedObjectEvent
    /// </summary>
    public class ChangedObjectEventArgs : EventArgs
    {
        #region Private Variables

        private readonly object _item;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ChangedObjectEvent
        /// </summary>
        public ChangedObjectEventArgs(object changedItem)
        {
            _item = changedItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that has been changed
        /// </summary>
        public object Item
        {
            get { return _item; }
        }

        #endregion



    }
}
