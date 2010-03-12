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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/6/2008 5:59:02 PM
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

namespace MapWindow
{


    /// <summary>
    /// Command
    /// </summary>
    public static class Command
    {
       

        #region Methods

        /// <summary>
        /// Launches the open file dialog.
        /// </summary>
        /// <returns>An IDataSet</returns>
        public static IDataSet OpenFile()
        {
            return MapWindow.Components.DataManager.DefaultDataManager.OpenFile();
        }

        /// <summary>
        /// Opens a specific file.
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns>An IDataSet</returns>
        public static IDataSet OpenFile(string filename)
        {
            return MapWindow.Components.DataManager.DefaultDataManager.OpenFile(filename);
        }

        /// <summary>
        /// Launches the open file dialog in order to add a raster
        /// </summary>
        /// <returns>An IRaster</returns>
        public static IRaster OpenRaster()
        {
            return MapWindow.Components.DataManager.DefaultDataManager.OpenRaster();
        }

        /// <summary>
        /// Opens the specified file and returns an IRaster
        /// </summary>
        /// <param name="filename">A string filename</param>
        /// <returns>The string to return</returns>
        public static IRaster OpenRaster(string filename)
        {
            return Raster.OpenFile(filename);
        }

        /// <summary>
        /// Launches the open file dialog in order to add a vector
        /// </summary>
        /// <returns>An IFeatureSet</returns>
        public static IFeatureSet OpenFeatureSet()
        {
            return MapWindow.Components.DataManager.DefaultDataManager.OpenVector();
        }


        /// <summary>
        /// Opens the specified file
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns></returns>
        public static IFeatureSet OpenFeatureSet(string filename)
        {
           return FeatureSet.OpenFile(filename);
        }

        
        /// <summary>
        /// Opens a file dialog to allow the user to open a layer and returns the result as a layer
        /// </summary>
        /// <returns>Returns an ILayer</returns>
        public static ILayer OpenLayer()
        {
            return MapWindow.Components.LayerManager.DefaultLayerManager.OpenLayer();
        }

        /// <summary>
        /// Opens the specified file as a layer
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns>Returns an ILayer</returns>
        public static ILayer OpenLayer(string filename)
        {
            return MapWindow.Components.LayerManager.DefaultLayerManager.OpenLayer(filename);
        }

        /// <summary>
        /// Opens an open file dialog to allow users to open the file of their choice
        /// </summary>
        /// <returns>An IRasterLayer</returns>
        public static IRasterLayer OpenRasterLayer()
        {
            return MapWindow.Components.LayerManager.DefaultLayerManager.OpenRasterLayer();
        }

        /// <summary>
        /// Opens an open file dialog to allow users to open the file of their choice
        /// </summary>
        /// <returns>An IFeatureLayer</returns>
        public static IFeatureLayer OpenVectorLayer()
        {
            return MapWindow.Components.LayerManager.DefaultLayerManager.OpenVectorLayer();
        }

        /// <summary>
        /// Opens an open file dialog to allow users to open the file of their choice
        /// </summary>
        /// <returns>An IRasterLayer</returns>
        /// <param name="filename">The string filename to open</param>
        public static IRasterLayer OpenRasterLayer(string filename)
        {
            return MapWindow.Components.LayerManager.DefaultLayerManager.OpenRasterLayer(filename);
        }

        /// <summary>
        /// Opens an open file dialog to allow users to open the file of their choice
        /// </summary>
        /// <returns>An IFeatureLayer</returns>
        /// <param name="filename">The string filename to open</param>
        public static IFeatureLayer OpenVectorLayer(string filename)
        {
            return MapWindow.Components.LayerManager.DefaultLayerManager.OpenVectorLayer(filename);
        }


       

        #endregion

        #region Properties



        #endregion



    }
}
