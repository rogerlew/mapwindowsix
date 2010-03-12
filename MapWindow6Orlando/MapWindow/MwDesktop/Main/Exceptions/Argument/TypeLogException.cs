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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 9:00:10 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Components;
namespace MapWindow.Main
{
    /// <summary>
    /// CastingException
    /// </summary>
    public class TypeLogException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of the TypeLogException, but does not set the message or
        /// throw the Log() method.
        /// </summary>
        public TypeLogException()
        {

        }

        /// <summary>
        /// invalidItem was of the wrong type.
        /// </summary>
        /// <param name="invalidItem">String describing the invalidly cast item</param>
        public TypeLogException(string invalidItem):
            base( ExceptionMessages.Argument_IncorrectType_S.Replace("%S", invalidItem))
        {
   
            Log();
        }

        /// <summary>
        /// invalidItem was of the wrong type for forItem
        /// </summary>
        /// <param name="invalidItem">a string describing the invalid item</param>
        /// <param name="forItem">a string describing the context that caused the invalid cast</param>
        public TypeLogException(string invalidItem, string forItem):
            base(ExceptionMessages.Argument_IncorrectType_S1_for_S2.Replace("%S1", invalidItem).Replace("%S2", invalidItem))
        {
            Log();
        }

        #region Methods
        /// <summary>
        /// Actually logs the exception.  This happens after the message has been set in the constructors of the classes of this exception.
        /// </summary>
        public virtual void Log()
        {
            LogManager.DefaultLogManager.Exception(this);
        }
        #endregion


    }
}
