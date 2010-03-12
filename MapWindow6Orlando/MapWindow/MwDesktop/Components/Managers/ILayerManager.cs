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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 9:30:47 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Drawing;
using MapWindow.Main;
using MapWindow.Plugins;
namespace MapWindow.Components
{


    /// <summary>
    /// IDataManager
    /// </summary>
    public interface ILayerManager
    {
        #region Events

        /// <summary>
        /// Occurs after the directory providers have been loaded into the project.
        /// </summary>
        event EventHandler<LayerProviders> DirectoryProvidersLoaded;

        #endregion


        #region Methods

        #region OpenLayer

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        ILayer OpenLayer();

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="container">The layer will be created in the specified collection</param>
        /// <returns>A Layer</returns>
        ILayer OpenLayer(ICollection<ILayer> container);

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        /// <returns>A Layer</returns>
        ILayer OpenLayer(IProgressHandler progressHandler);

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        ILayer OpenLayer(string filename, ICollection<ILayer> container);
      
        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        /// <returns>A Layer</returns>
        ILayer OpenLayer(string filename, IProgressHandler progressHandler);
       

        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        ILayer OpenLayer(string filename);

        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        /// <param name="container">any valid IContainer that this should be added to</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        ILayer OpenLayer(string filename, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler);

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        IRasterLayer OpenRasterLayer();
      

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        IFeatureLayer OpenVectorLayer();

        /// <summary>
        /// This attempts to open the specified raster file and returns an associated layer
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        IRasterLayer OpenRasterLayer(string filename);
        

        /// <summary>
        /// This attempts to open the specified vector file and returns an associated layer
        /// </summary>
        /// <param name="filename">the string filename to open</param>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        IFeatureLayer OpenVectorLayer(string filename);
       
      
       


        #endregion


        #region CreateNew

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
        IRasterLayer CreateRasterLayer(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType,
                                       string[] options);
        
 

        #endregion

        /// <summary>
        /// Checks a dialog filter and returns a list of just the extensions.
        /// </summary>
        /// <param name="DialogFilter">The Dialog Filter to read extensions from</param>
        /// <returns>A list of extensions</returns>
        List<string> GetSupportedExtensions(string DialogFilter);

        /// <summary>
        /// Given a string filename for the "*.dll" file, this will attempt to load any classes that implement the
        /// IDataProvder interface.
        /// </summary>
        /// <param name="filename">The string path of the assembly to load from.</param>
        /// <returns>A list that contains only the providers that were just loaded.  This may be a list of count 0, but shouldn't return null.</returns>
        List<ILayerProvider> LoadProvidersFromAssembly(string filename);

        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// This will not affect the PreferredProviders or the general list of Providers.
        /// These automatically have the lowest priority and will only be used if nothing
        /// else works.  Use the PreferredProviders to force preferential loading of 
        /// a plugin DataProvider.
        /// </summary>
        /// <returns>A list of just the newly added DataProviders from this method.</returns>
        List<ILayerProvider> LoadProvidersFromDirectories();

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets a temporary list of active project layers.  This is designed to house 
        /// the layers from a map frame when the property grids are shown for layers in that
        /// map frame.  This list on the DefaultLayerManager is what is used to create the
        /// list that populates dropdowns that can take a Layer as a parameter.
        /// </summary>
        IList<ILayer> ActiveProjectLayers
        {
            get;
            set;
        }
     
        /// <summary>
        /// Gets or sets the list of IDataProviders that should be used in the project.
        /// </summary>
        List<ILayerProvider> LayerProviders
        {
            get;
            set;
        }

        

        /// <summary>
        /// Gets or sets the path (either as a full path or as a path relative to
        /// the MapWindow.dll) to search for plugins that implement the IDataProvider interface.
        /// </summary>
        List<string> LayerProviderDirectories
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        string DialogReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        string DialogWriteFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the default condition for how this data manager should try to load layers.
        /// This will be overridden if the inRam property is specified as a parameter.
        /// </summary>
        bool LoadInRam
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files that are specifically raster formats.
        /// </summary>
        string RasterReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving raster files.
        /// </summary>
        string RasterWriteFilter
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening vector files.
        /// </summary>
        string VectorReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving vector files.
        /// </summary>
        string VectorWriteFilter
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets a dictionary of IDataProviders keyed by the extension.  The 
        /// standard order is to try to load the data using a PreferredProvider.  If that
        /// fails, then it will check the list of dataProviders, and finally, if that fails,
        /// it will check the plugin Data Providers in directories.
        /// </summary>
        Dictionary<string, ILayerProvider> PreferredProviders
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a progress handler for any open operations that are intiated by this 
        /// DataManager.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }


        #endregion

       


        
        

    }
}
