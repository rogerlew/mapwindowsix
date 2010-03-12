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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/6/2009 12:19:18 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Drawing
{


    /// <summary>
    /// StringEventArgs
    /// </summary>
    public class ExpressionEventArgs : EventArgs
    {
        #region Private Variables

        private string _expression;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StringEventArgs
        /// </summary>
        /// <param name="expression">The string expression for this event args</param>
        public ExpressionEventArgs(string expression)
        {
            _expression = expression;
        }

        #endregion

 
        #region Properties

        /// <summary>
        /// The string expression for this event.
        /// </summary>
        public string Expression
        {
            get { return _expression;}
            protected set { _expression = value;}
        }


        #endregion



    }
}
