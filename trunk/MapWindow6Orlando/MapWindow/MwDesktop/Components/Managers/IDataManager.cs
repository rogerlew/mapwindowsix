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
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Plugins;
namespace MapWindow.Components
{


    /// <summary>
    /// IDataManager
    /// </summary>
    public interface IDataManager
    {

        #region Methods

        /// <summary>
        /// This can help determine what kind of file format a file is, without actually opening the file.
        /// </summary>
        /// <param name="filename">The string filename to test</param>
        /// <returns>A DataFormats enum</returns>
        DataFormats GetFileFormat(string filename);

        /// <summary>
        /// Instead of opening the specified file, this simply determines the correct 
        /// provider, and requests that the provider check the feature type for vector
        /// formats.
        /// </summary>
        /// <param name="filename">The string filename to test</param>
        /// <returns>A FeatureTypes enum</returns>
        FeatureTypes GetFeatureType(string filename);

        #region OpenFile


        /// <summary>
        /// This launches an open file dialog that allows loading of several files at once 
        /// and returns the datasets in a list.
        /// </summary>
        /// <returns>A list of all the files that were opened</returns>
        List<IDataSet> OpenFiles();
        

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        IDataSet OpenFile();

        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        IDataSet OpenFile(string filename);

        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        IDataSet OpenFile(string filename, bool inRam);


        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        IDataSet OpenFile(string filename, bool inRam, IProgressHandler progressHandler);

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        IRaster OpenRaster();

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <returns>The list or rasters</returns>
        List<IRaster> OpenRasters();

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        IFeatureSet OpenVector();

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <returns>The list or rasters</returns>
        List<IFeatureSet> OpenVectors();


        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>for now an IDataSet</returns>
        IImageData OpenImage();

        /// <summary>
        /// This uses an open dialog filter with only image extensions for supported image formats,
        /// but where multi-select is enabled, and so allowing multiple images to be returned at once.
        /// </summary>
        /// <returns></returns>
        List<IImageData> OpenImages();
        

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object for interacting with the file.
        /// </summary>
        /// <param name="filename">The string filename</param>
        /// <returns>An IImageData object</returns>
        IImageData OpenImage(string filename);

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <param name="progressHandler">The progressHandler to receive progress updates</param>
        /// <returns>An IImageData</returns>
        IImageData OpenImage(string filename, IProgressHandler progressHandler);
      


        /// <summary>
        /// Opens the specified filename, returning an IRaster.  This will return null if a manager 
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns>An IRaster loaded from the specified file.</returns>
        IRaster OpenRaster(string filename);
       

        /// <summary>
        /// Opens the specified filename, returning an IRaster.  This will return null if a manager 
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram</param>
        /// <param name="prog">a progress interface</param>
        /// <returns>An IRaster loaded from the specified file</returns>
        IRaster OpenRaster(string filename, bool inRam, IProgressHandler prog);
        
        /// <summary>
        /// Opens a specified file as an IFeatureSet
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram</param>
        /// <param name="prog">a progress interface</param>
        /// <returns>An IFeatureSet loaded from the specified file</returns>
        IFeatureSet OpenVector(string filename, bool inRam, IProgressHandler prog);
      


        #endregion


        #region CreateNew


        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="filename">The string filename to open an image for</param>
        /// <param name="numRows">The integer number of rows</param>
        /// <param name="numColumns">The integer number of columns</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        IImageData CreateImage(string filename, int numRows, int numColumns);
       

        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="filename">The string filename to open an image for</param>
        /// <param name="numRows">The integer number of rows</param>
        /// <param name="numColumns">The integer number of columns</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        IImageData CreateImage(string filename, int numRows, int numColumns, bool inRam);
     
        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="filename">The string filename to open an image for</param>
        /// <param name="numRows">The integer number of rows</param>
        /// <param name="numColumns">The integer number of columns</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory</param>
        /// <param name="progHandler">A Progress handler</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        IImageData CreateImage(string filename, int numRows, int numColumns, bool inRam, IProgressHandler progHandler);
        

        /// <summary>
        /// Creates a new class of vector that matches the given filename.
        /// </summary>
        /// <param name="filename">The string filename from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        IFeatureSet CreateVector(string filename, FeatureTypes featureType);
       

        /// <summary>
        /// Creates a new class of vector that matches the given filename.
        /// </summary>
        /// <param name="filename">The string filename from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file</param>
        /// <param name="progHandler">Overrides the default progress handler with the specified progress handler</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        IFeatureSet CreateVector(string filename, FeatureTypes featureType, IProgressHandler progHandler);
      


        /// <summary>
        /// Creates a new raster using the specified raster provider and the Data Manager's Progress Handler,
        /// as well as its LoadInRam property.
        /// </summary>
        /// <param name="name">The filename of the new file to create.</param>
        /// <param name="driverCode">The string code identifying the driver to use to create the raster</param>
        /// <param name="xSize">The number of columns in the raster</param>
        /// <param name="ySize">The number of rows in the raster</param>
        /// <param name="numBands">The number of bands in the raster</param>
        /// <param name="dataType">The data type for the raster</param>
        /// <param name="options">Any additional, driver specific options for creation</param>
        /// <returns>An IRaster representing the created raster.</returns>
        IRaster CreateRaster(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options);
       

       
        


        #endregion

        /// <summary>
        /// Checks a dialog filter and returns a list of just the extensions.
        /// </summary>
        /// <param name="DialogFilter">The Dialog Filter to read extensions from</param>
        /// <returns>A list of extensions</returns>
        List<string> GetSupportedExtensions(string DialogFilter);

        #endregion

        #region Properties

     
        /// <summary>
        /// Gets or sets the list of IDataProviders that should be used in the project.
        /// </summary>
        List<IDataProvider> DataProviders
        {
            get;
            set;
        }

        

        /// <summary>
        /// Gets or sets the path (either as a full path or as a path relative to
        /// the MapWindow.dll) to search for plugins that implement the IDataProvider interface.
        /// </summary>
        List<string> DataProviderDirectories
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
        /// Gets or sets the dialog read filter to use for opening image files.
        /// </summary>
        string ImageReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving image files.
        /// </summary>
        string ImageWriteFilter
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
        Dictionary<string, IDataProvider> PreferredProviders
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

        /// <summary>
        /// Occurs after the directory providers have been loaded into the project.
        /// </summary>
        event EventHandler<DataProviders> DirectoryProvidersLoaded;


        /// <summary>
        /// Given a string filename for the "*.dll" file, this will attempt to load any classes that implement the
        /// IDataProvder interface.
        /// </summary>
        /// <param name="filename">The string path of the assembly to load from.</param>
        /// <returns>A list that contains only the providers that were just loaded.  This may be a list of count 0, but shouldn't return null.</returns>
        List<IDataProvider> LoadProvidersFromAssembly(string filename);

        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// This will not affect the PreferredProviders or the general list of Providers.
        /// These automatically have the lowest priority and will only be used if nothing
        /// else works.  Use the PreferredProviders to force preferential loading of 
        /// a plugin DataProvider.
        /// </summary>
        /// <returns>A list of just the newly added DataProviders from this method.</returns>
        List<IDataProvider> LoadProvidersFromDirectories();
        

    }
}
