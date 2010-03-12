//********************************************************************************************************
// Product Name: MapWindow.Main.Exceptions.Raster.dll Alpha
// Description:  The basic module for MapWindow.Main.Exceptions.Raster version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Main.Exceptions.Raster.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/28/2008 3:10:58 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using MapWindow.Components;
namespace MapWindow.Main
{


    /// <summary>
    /// RangeRequiredBivalueException
    /// </summary>
    public class RangeRequiredBivalueException : InvalidOperationException, ILogException
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RangeRequiredBivalueException
        /// </summary>
        public RangeRequiredBivalueException():base(ExceptionMessages.Raster_RangeRequiresBivalue)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs this exception with any Loggers
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
