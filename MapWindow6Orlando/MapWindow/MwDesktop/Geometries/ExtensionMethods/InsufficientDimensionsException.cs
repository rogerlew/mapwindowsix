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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/13/2009 6:09:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MapWindow.Geometries
{


    /// <summary>
    /// InsufficientDimensionsException
    /// </summary>
    public class InsufficientDimensionsException : ArgumentException
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of InsufficientDimensionsException
        /// </summary>
        public InsufficientDimensionsException() : base(MessageStrings.InsufficientDimensions_S.Replace("%S","at least one argument")) 
        {

        }

        /// <summary>
        /// Creates a new instance of InsufficientDimesionsException, but specifies the name of the argument,
        /// or a similar string like "both envelopes".
        /// </summary>
        /// <param name="argumentName">The string name of the argument to introduce in to exception message</param>
        public InsufficientDimensionsException(string argumentName) : base(MessageStrings.InsufficientDimensions_S.Replace("%S",argumentName)) 
        {
        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
