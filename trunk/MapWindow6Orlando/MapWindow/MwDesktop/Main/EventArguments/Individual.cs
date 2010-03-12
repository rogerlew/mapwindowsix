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
    /// Carries event arguments for the generic IEventList
    /// </summary>
    public class Individual<T> : System.EventArgs
    {

        private T _listItem;
        
      

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public Individual(T inListItem)
        {
            _listItem = inListItem;
        }

       

     
      
        #endregion

        #region Properties

        /// <summary>
        /// Gets the list item being referenced by this event
        /// </summary>
        public T ListItem
        {
            get { return _listItem; }
            protected set { _listItem = value; }
        }

       
        #endregion

    }
}
