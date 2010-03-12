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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 4:35:27 PM
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
    /// OutOfRangeException
    /// </summary>
    public class OutOfRangeLogException : ArgumentOutOfRangeException, ILogException
    {
      
        #region Constructors

        /// <summary>
        /// Creates a new instance of the OutOfRangeLogException but does not set the message
        /// or fire the Log method.
        /// </summary>
        public OutOfRangeLogException()
        {
        }


        /// <summary>
        /// Creates a new instance of OutOfRangeException and logs the creation.
        /// </summary>
        public OutOfRangeLogException(string parameterName):base(ExceptionMessages.Argument_OutOfRange_S.Replace("%S", parameterName))
        {
            Log();
        }

        #endregion

      
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
