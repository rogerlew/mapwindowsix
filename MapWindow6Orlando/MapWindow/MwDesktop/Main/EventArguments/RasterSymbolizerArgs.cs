//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description: The core library for the MapWindow6.0 project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/28/2008 9:46:04 AM
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

namespace MapWindow.Main
{


    /// <summary>
    /// RasterSymbolizerArgs
    /// </summary>
    public class RasterSymbolizerArgs : EventArgs
    {
        #region Private Variables

        IRasterSymbolizer _rasterSymbolizer;

       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RasterSymbolizerArgs
        /// </summary>
        public RasterSymbolizerArgs(IRasterSymbolizer rasterSymbolizer)
        {
            _rasterSymbolizer = rasterSymbolizer;
        }

        #endregion

      

        #region Properties

        /// <summary>
        /// Gets the RasterSymbolizer that is involved in this event.
        /// </summary>
        public IRasterSymbolizer RasterSymbolizer
        {
            get { return _rasterSymbolizer; }
            protected set { _rasterSymbolizer = value; }
        }


        #endregion



    }
}
