//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    public class IndividualCancel<T> : System.ComponentModel.CancelEventArgs
    {

         /// <summary>
        /// The protected object internal to this list event args class
        /// </summary>
        private T _listItem;
        
      
        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public IndividualCancel(T inListItem)
        {
            _listItem = ListItem;
        }

     

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list item being referenced by this event
        /// </summary>
        T ListItem
        {
            get
            {
                return _listItem;
            }
        }

      
        #endregion
    }
}
