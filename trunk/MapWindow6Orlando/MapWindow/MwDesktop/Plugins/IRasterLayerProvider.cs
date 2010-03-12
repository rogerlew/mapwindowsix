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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/08/2008 10:54:10 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using MapWindow.Drawing;


namespace MapWindow.Plugins
{


    /// <summary>
    /// A DataProvider that is specific to raster formats.
    /// </summary>
    public interface IRasterLayerProvider : ILayerProvider
    {
        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the filename
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="name">The string filename for the new instance</param>
        /// <param name="dataType">The data type to use for the raster</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster</param>
        /// <param name="numBands">The number of bands to create in the raster</param>
        /// <param name="xSize">The number of columns in the raster</param>
        /// <param name="ySize">The number of rows in the raster</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>An IRaster</returns>
        IRasterLayer Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options);


        
    }
}
