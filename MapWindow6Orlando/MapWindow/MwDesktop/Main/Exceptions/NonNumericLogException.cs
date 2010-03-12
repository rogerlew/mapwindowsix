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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/29/2008 2:32:41 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Components;
namespace MapWindow.Main
{


    /// <summary>
    /// NonNumericLogException
    /// </summary>
    public class NonNumericLogException: InvalidOperationException, ILogException
    {
        #region Private Variables



        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NonNumericLogException
        /// </summary>
        /// <param name="invalidVariable">The string name of the variable, or value that cannot be parsed as a number.</param>
        public NonNumericLogException(string invalidVariable): base(ExceptionMessages.NotNumeric_S.Replace("%S", invalidVariable))
        {
            Log();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs the exception
        /// </summary>
        public void Log()
        {
            LogManager.DefaultLogManager.Exception(this);
        }

        #endregion

        #region Properties



        #endregion




    }
}
