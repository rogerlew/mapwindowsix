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
using System.Windows.Forms;

namespace MapWindow.Main
{
    /// <summary>
    /// An EventArgs for controling actions that need to pass an object
    /// </summary>
    public class EntityArgs : System.EventArgs
    {
        #region Private Variables

        private object _entity;
        private MouseEventArgs _mouseArgs;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor for EntityArgs
        /// </summary>
        /// <param name="inEntity">Any object</param>
        /// <param name="inMouseArgs">The usual MouseEventArgs</param>
        public EntityArgs(object inEntity, MouseEventArgs inMouseArgs)
        {
            _entity = inEntity;
            _mouseArgs = inMouseArgs;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// An object to track
        /// </summary>
        public object Entity
        {
            get { return _entity; }
            protected set { _entity = value; }
        }

        /// <summary>
        /// The mouse arguments
        /// </summary>
        public MouseEventArgs MouseArgs
        {
            get { return _mouseArgs; }
            protected set { _mouseArgs = value; }
        }

        #endregion
    }
    
}
