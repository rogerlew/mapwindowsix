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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 9:28:29 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Plugins;

namespace MapWindow.Components
{
    /// <summary>
    /// This can be used as a component to work as a DataManager.  This also provides the
    /// very important DefaultDataManager property, which is where the developer controls
    /// what DataManager should be used for their project.
    /// </summary>
    public class DataManager : IDataManager
    {
        #region Private Variables

        // If this doesn't exist, a new one is created when you "get" this data manager.
        private static IDataManager _defaultDataManager;
        private IContainer components; //Required designer variable.
        private List<IDataProvider> _dataProviders;

        private string _dialogReadFilter;
        private string _rasterReadFilter;
        private string _vectorReadFilter;
        private string _imageReadFilter;

        private string _dialogWriteFilter;
        private string _rasterWriteFilter;
        private string _vectorWriteFilter;
        private string _imageWriteFilter;
        private bool _defaultDirectoryAdded;

        private IProgressHandler _progressHandler;

        private Dictionary<string, IDataProvider> _preferredProviders;

        private List<string> _dataProviderDirectories;

        private bool _loadInRam = true;

        /// <summary>
        /// Gets the componetns for this manager
        /// </summary>
        public IContainer Components
        {
            get { return components; }
        }

        /// <summary>
        /// Gets or sets the implemenation of IDataManager for the project to use when
        /// accessing data.  This is THE place where the DataManager can be replaced
        /// by a different data manager.  If you add this data manager to your
        /// project, this will automatically set itself as the DefaultDataManager.
        /// However, since each DM will do this, you may have to control this manually
        /// if you add more than one DataManager to the project in order to set the
        /// one that will be chosen.
        /// </summary>
        public static IDataManager DefaultDataManager
        {
            get
            {
                if (_defaultDataManager == null)
                    _defaultDataManager = new DataManager();
                return _defaultDataManager;
            }
            set { _defaultDataManager = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whether or not the default directory has been added
        /// </summary>
        public bool DefaultDirectoryAdded
        {
            get { return _defaultDirectoryAdded;  }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the DataManager class.  A data manager is more or less
        /// just a list of data providers to use.  The very important 
        /// DataManager.DefaultDataManager property controls which DataManager will be used
        /// to load data.  By default, each DataManager sets itself as the default in its
        /// constructor.
        /// </summary>
        public DataManager()
        {
            Configure();
        }

        private void Configure()
        {
            InitializeComponent();
            _defaultDataManager = this;
            _dataProviders = new List<IDataProvider>();
            _dataProviders.Add(new ShapefileDataProvider()); // .shp files
            _dataProviders.Add(new BinaryRasterProvider()); // .bgd files
            _dataProviders.Add(new DotNetImageProvider()); // simple image files supported by dot net.
            _preferredProviders = new Dictionary<string, IDataProvider>();
            _dataProviderDirectories = new List<string>();
            
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the "Plugins" directory
        /// </summary>
        public void LoadDefaultDirectories()
        {

            string path = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "Plugins";
            if (!Directory.Exists(path)) return;
            OperatingSystem os = Environment.OSVersion;
           
            if (os.Platform != PlatformID.Unix)
            {
                foreach (var s in Directory.GetDirectories(path))
                {
                    _dataProviderDirectories.Add(s);
                }
                LoadProvidersFromDirectories();
            }
            _defaultDirectoryAdded = true;
        }

        /// <summary>
        /// Creates a new class of vector that matches the given filename.
        /// </summary>
        /// <param name="filename">The string filename from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        public IFeatureSet CreateVector(string filename, FeatureTypes featureType)
        {
            return CreateVector(filename, featureType, _progressHandler);
        }

        /// <summary>
        /// Creates a new class of vector that matches the given filename.
        /// </summary>
        /// <param name="filename">The string filename from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file</param>
        /// <param name="progHandler">Overrides the default progress handler with the specified progress handler</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        public IFeatureSet CreateVector(string filename, FeatureTypes featureType, IProgressHandler progHandler)
        {
            // To Do: Add Customization that allows users to specify which plugins to use in priority order.
            IFeatureSet result;

            // First check for the extension in the preferred plugins list

            string ext = Path.GetExtension(filename);
            if (_preferredProviders.ContainsKey(ext))
            {
                VectorProvider vp = _preferredProviders[ext] as VectorProvider;
                if (vp != null)
                {
                    result = vp.CreateNew(filename, featureType, true, progHandler);
                    if (result != null)
                        return result;
                }
                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers

            foreach (IDataProvider dp in _dataProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    VectorProvider vp = dp as VectorProvider;
                    if (vp != null)
                    {
                        // attempt to open with the filename.
                        result = vp.CreateNew(filename, featureType, true, progHandler);
                        if (result != null)
                            return result;
                    }
                }
            }
            MessageBox.Show(MessageStrings.FileTypeNotSupported);
            return null;
        }


        /// <summary>
        /// Checks a dialog filter and returns a list of just the extensions.
        /// </summary>
        /// <param name="DialogFilter">The Dialog Filter to read extensions from</param>
        /// <returns>A list of extensions</returns>
        public virtual List<string> GetSupportedExtensions(string DialogFilter)
        {
            List<string> extensions = new List<string>();
            string[] formats = DialogFilter.Split('|');
            char[] wild = {'*'};
            // We don't care about the description strings, just the extensions.
            for (int i = 1; i < formats.Length; i += 2)
            {
                // Multiple extension types are separated by semicolons
                string[] potentialExtensions = formats[i].Split(';');
                foreach (string potentialExtension in potentialExtensions)
                {
                    string ext = potentialExtension.TrimStart(wild);
                    if (extensions.Contains(ext) == false)
                        extensions.Add(ext);
                }
            }
            return extensions;
        }

        /// <summary>
        /// This can help determine what kind of file format a file is, without actually opening the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public virtual DataFormats GetFileFormat(string filename)
        {
            string ext = Path.GetExtension(filename);
            foreach (IDataProvider dp in _dataProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    VectorProvider vp = dp as VectorProvider;
                    if (vp != null) return DataFormats.Vector;
                    IRasterProvider rp = dp as IRasterProvider;
                    if (rp != null) return DataFormats.Raster;
                    IImageDataProvider ip = dp as IImageDataProvider;
                    if (ip != null) return DataFormats.Image;
                    return DataFormats.Custom;
                }
            }
            return DataFormats.Custom;
        }

        /// <summary>
        /// Instead of opening the specified file, this simply determines the correct 
        /// provider, and requests that the provider check the feature type for vector
        /// formats.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public virtual FeatureTypes GetFeatureType(string filename)
        {
            string ext = Path.GetExtension(filename);
            if (GetFileFormat(filename) != DataFormats.Vector) return FeatureTypes.Unspecified;
            foreach (IDataProvider dp in _dataProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    VectorProvider vp = dp as VectorProvider;
                    if (vp == null) continue;
                    return vp.GetFeatureType(filename);
                }
            }
            return FeatureTypes.Unspecified;
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public virtual IRaster OpenRaster()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = RasterReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenFile(ofd.FileName, LoadInRam, ProgressHandler) as IRaster;
        }

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <returns>The list or rasters</returns>
        public virtual List<IRaster> OpenRasters()
        {
            List<IRaster> result = new List<IRaster>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = RasterReadFilter;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IRaster ds = OpenRaster(name, LoadInRam, ProgressHandler);
                if (ds != null) result.Add(ds);
            }
            return result;
        }


        /// <summary>
        /// Opens the specified filename, returning an IRaster.  This will return null if a manager 
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns>An IRaster loaded from the specified file.</returns>
        public virtual IRaster OpenRaster(string filename)
        {
            return OpenFile(filename, true, ProgressHandler) as IRaster;
        }

        /// <summary>
        /// Opens the specified filename, returning an IRaster.  This will return null if a manager 
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram</param>
        /// <param name="prog">a progress interface</param>
        /// <returns>An IRaster loaded from the specified file</returns>
        public virtual IRaster OpenRaster(string filename, bool inRam, IProgressHandler prog)
        {
            return OpenFile(filename, inRam, prog) as IRaster;
        }

        /// <summary>
        /// Opens a specified file as an IFeatureSet
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram</param>
        /// <param name="prog">a progress interface</param>
        /// <returns>An IFeatureSet loaded from the specified file</returns>
        public virtual IFeatureSet OpenVector(string filename, bool inRam, IProgressHandler prog)
        {
            return OpenFile(filename, inRam, prog) as IFeatureSet;
        }


        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        public virtual IFeatureSet OpenVector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = VectorReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenFile(ofd.FileName, LoadInRam, ProgressHandler) as IFeatureSet;
        }

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <returns>The list or rasters</returns>
        public virtual List<IFeatureSet> OpenVectors()
        {
            List<IFeatureSet> result = new List<IFeatureSet>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = VectorReadFilter;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IFeatureSet ds = OpenVector(name, LoadInRam, ProgressHandler);
                if (ds != null) result.Add(ds);
            }
            return result;
        }


        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>for now an IDataSet</returns>
        public virtual IImageData OpenImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ImageReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenFile(ofd.FileName, LoadInRam, ProgressHandler) as IImageData;
        }

        /// <summary>
        /// This uses an open dialog filter with only image extensions for supported image formats,
        /// but where multi-select is enabled, and so allowing multiple images to be returned at once.
        /// </summary>
        /// <returns></returns>
        public virtual List<IImageData> OpenImages()
        {
            List<IImageData> result = new List<IImageData>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ImageReadFilter;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IImageData id = OpenImage(name, ProgressHandler);
                if(id != null)result.Add(id);
            }
            return result;
        }


        /// <summary>
        /// Opens the file as an Image and returns an IImageData object for interacting with the file.
        /// </summary>
        /// <param name="filename">The string filename</param>
        /// <returns>An IImageData object</returns>
        public virtual IImageData OpenImage(string filename)
        {
            return OpenFile(filename, LoadInRam, ProgressHandler) as IImageData;
        }

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <param name="progressHandler">The progressHandler to receive progress updates</param>
        /// <returns>An IImageData</returns>
        public virtual IImageData OpenImage(string filename, IProgressHandler progressHandler)
        {
            return OpenFile(filename, LoadInRam, progressHandler) as IImageData;
        }


        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        public virtual IDataSet OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenFile(ofd.FileName, LoadInRam, ProgressHandler);
        }

        /// <summary>
        /// This launches an open file dialog that allows loading of several files at once 
        /// and returns the datasets in a list.
        /// </summary>
        /// <returns>A list of all the files that were opened</returns>
        public virtual List<IDataSet> OpenFiles()
        {
            List<IDataSet> result = new List<IDataSet>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IDataSet ds = OpenFile(name, LoadInRam, ProgressHandler);
                if (ds != null) result.Add(ds);
              
            }
            return result;
        }


        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        public virtual IDataSet OpenFile(string filename)
        {
            return OpenFile(filename, LoadInRam, ProgressHandler);
        }

        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        public virtual IDataSet OpenFile(string filename, bool inRam)
        {
            return OpenFile(filename, inRam, ProgressHandler);
        }


        /// <summary>
        /// Attempts to call the open filename method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        public virtual IDataSet OpenFile(string filename, bool inRam, IProgressHandler progressHandler)
        {
            // To Do: Add Customization that allows users to specify which plugins to use in priority order.
            IDataSet result;

            // First check for the extension in the preferred plugins list
            filename = filename.ToLower();
            string ext = Path.GetExtension(filename).ToLower();
            if (_preferredProviders.ContainsKey(ext))
            {
                result = _preferredProviders[ext].Open(filename);
                if (result != null)
                    return result;
                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers


            foreach (IDataProvider dp in _dataProviders)
            {
                if (!GetSupportedExtensions(dp.DialogReadFilter).Contains(ext)) continue;
                // attempt to open with the filename.
                dp.ProgressHandler = ProgressHandler;
                result = dp.Open(filename);
                if (result != null)
                    return result;
            }

            throw new ApplicationException(MessageStrings.FileTypeNotSupported);
        }

        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="filename">The string filename to open an image for</param>
        /// <param name="width">The integer width in pixels</param>
        /// <param name="height">The integer height in pixels</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        public virtual IImageData CreateImage(string filename, int width, int height)
        {
            return CreateImage(filename, width, height, LoadInRam, ProgressHandler);
        }


        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="filename">The string filename to open an image for</param>
        /// <param name="width">The integer width in pixels</param>
        /// <param name="height">The integer height in pixels</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        public virtual IImageData CreateImage(string filename, int width, int height, bool inRam)
        {
            return CreateImage(filename, width, height, inRam, ProgressHandler);
        }

        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="filename">The string filename to open an image for</param>
        /// <param name="width">The integer width in pixels</param>
        /// <param name="height">The integer height in pixels</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory</param>
        /// <param name="progHandler">A Progress handler</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        public virtual IImageData CreateImage(string filename, int width, int height, bool inRam,
                                              IProgressHandler progHandler)
        {
            IImageData result;

            // First check for the extension in the preferred plugins list
            filename = filename.ToLower();
            string ext = Path.GetExtension(filename);
            if (_preferredProviders.ContainsKey(ext))
            {
                IImageDataProvider rp = _preferredProviders[ext] as IImageDataProvider;
                if (rp != null)
                {
                    result = rp.Create(filename, width, height, inRam, progHandler);
                    if (result != null)
                        return result;
                }

                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers

            foreach (IDataProvider dp in _dataProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    IImageDataProvider rp = dp as IImageDataProvider;
                    if (rp != null)
                    {
                        // attempt to open with the filename.
                        result = rp.Create(filename, width, height, inRam, progHandler);
                        if (result != null)
                            return result;
                    }
                }
            }

            throw new ApplicationException(MessageStrings.FileTypeNotSupported);
        }

       

        /// <summary>
        /// Creates a new raster using the specified raster provider and the Data Manager's Progress Handler,
        /// as well as its LoadInRam property.
        /// </summary>
        /// <param name="name">The filename of the new file to create.</param>
        /// <param name="driverCode">The string code identifying the driver to use to create the raster.  If no code is specified
        /// the manager will attempt to match the extension with a code specified in the Dialog write filter.</param>
        /// <param name="xSize">The number of columns in the raster</param>
        /// <param name="ySize">The number of rows in the raster</param>
        /// <param name="numBands">The number of bands in the raster</param>
        /// <param name="dataType">The data type for the raster</param>
        /// <param name="options">Any additional, driver specific options for creation</param>
        /// <returns>An IRaster representing the created raster.</returns>
        public virtual IRaster CreateRaster(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            IRaster result;

            // First check for the extension in the preferred plugins list
            name = name.ToLower();
            string ext = Path.GetExtension(name);
            if (_preferredProviders.ContainsKey(ext))
            {
                IRasterProvider rp = _preferredProviders[ext] as IRasterProvider;
                if (rp != null)
                {
                    result = rp.Create(name,driverCode, xSize, ySize, numBands, dataType, options);
                    if (result != null)
                        return result;
                }

                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers

            foreach (IDataProvider dp in _dataProviders)
            {
                if (GetSupportedExtensions(dp.DialogWriteFilter).Contains(ext))
                {
                    IRasterProvider rp = dp as IRasterProvider;
                    if (rp != null)
                    {
                        // attempt to open with the filename.
                        result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                        if (result != null)
                            return result;
                    }
                }
            }

            throw new ApplicationException(MessageStrings.FileTypeNotSupported);
        }

        #endregion

        private string GetDriverFromExtension(string extension, string dialogWriteFilter)
        {
            string[] allnames = dialogWriteFilter.Split('|');
            for(int i = 0; i < allnames.Length; i+=2)
            {
                if(allnames[i+1].Contains(extension))
                {
                    return allnames[i];
                }
            }
            return "";
        }


        #region Properties

        // May make this invisible if we can
        /// <summary>
        /// Gets or sets the list of IDataProviders that should be used in the project.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual List<IDataProvider> DataProviders
        {
            get { return _dataProviders; }
            set { _dataProviders = value; }
        }


        /// <summary>
        /// Gets or sets the path (either as a full path or as a path relative to
        /// the MapWindow.dll) to search for plugins that implement the IDataProvider interface.
        /// </summary>
        [Category("Providers"),
         Description(
             "Gets or sets the list of string path names that should be used to search for IDataProvider interfaces.")]
        public virtual List<string> DataProviderDirectories
        {
            get { return _dataProviderDirectories; }
            set { _dataProviderDirectories = value; }
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open files."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string DialogReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_dialogReadFilter != null) return _dialogReadFilter;

                List<string> extensions = new List<string>();
                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    extensions.Add(item.Key);
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                extensions.Add(potentialExtension);
                                if (dp is IRasterProvider)
                                    rasterExtensions.Add(potentialExtension);
                                if (dp is VectorProvider)
                                    vectorExtensions.Add(potentialExtension);
                                if (dp is IImageDataProvider)
                                    imageExtensions.Add(potentialExtension);
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                string result = "All Supported Formats|" + String.Join(";", extensions.ToArray());
                if (vectorExtensions.Count > 0)
                    result += "|Vectors|" + String.Join(";", vectorExtensions.ToArray());
                if (rasterExtensions.Count > 0)
                    result += "|Rasters|" + String.Join(";", rasterExtensions.ToArray());
                if (imageExtensions.Count > 0)
                    result += "|Images|" + String.Join(";", imageExtensions.ToArray());

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat = "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            // don't add this format if it was already added by a "preferred data provider"
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                result += potentialFormat;
                                result += "|" + formats[i];
                            }
                        }
                    }
                }
                result += "|All Files (*.*) |*.*";
                _dialogReadFilter = result;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save files."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string DialogWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_dialogWriteFilter != null) return _dialogWriteFilter;

                List<string> extensions = new List<string>();
                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');

                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                extensions.Add(potentialExtension);
                                if (dp is IRasterProvider)
                                    rasterExtensions.Add(potentialExtension);
                                if (dp is VectorProvider)
                                    vectorExtensions.Add(potentialExtension);
                                if (dp is IImageDataProvider)
                                    imageExtensions.Add(potentialExtension);
                            }
                        }
                    }
                }


                // We now have a list of all the file extensions supported
                string result = "All Supported Formats|" + String.Join(";", extensions.ToArray());

                if (vectorExtensions.Count > 0)
                    result += "|Vectors|" + String.Join(";", vectorExtensions.ToArray());
                if (rasterExtensions.Count > 0)
                    result += "|Rasters|" + String.Join(";", rasterExtensions.ToArray());
                if (imageExtensions.Count > 0)
                    result += "|Images|" + String.Join(";", imageExtensions.ToArray());


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat += "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                result += potentialFormat;
                                result += "|" + formats[i];
                            }
                        }
                    }
                }

                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _dialogWriteFilter = value; }
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files that are specifically raster formats.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open rasters."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string RasterReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_rasterReadFilter != null) return _rasterReadFilter;

                List<string> extensions = new List<string>();


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        extensions.Add(item.Key);
                    }
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                if (dp is IRasterProvider)
                                    extensions.Add(potentialExtension);
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                string result = "Rasters|" + String.Join(";", extensions.ToArray());

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat = "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            // don't add this format if it was already added by a "preferred data provider"
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                if (dp is IRasterProvider)
                                {
                                    result += potentialFormat;
                                    result += "|" + formats[i];
                                }
                            }
                        }
                    }
                }
                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _rasterReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save rasters."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string RasterWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_rasterWriteFilter != null) return _rasterWriteFilter;

                string result = null;
                List<string> extensions = new List<string>();


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                        extensions.Add(item.Key);
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');

                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                if (dp is IRasterProvider)
                                    extensions.Add(potentialExtension);
                            }
                        }
                    }
                }


                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                    result += "Rasters|" + String.Join(";", extensions.ToArray());


                //foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                //{
                //    if (item.Value is IRasterProvider)
                //    {
                //        // we don't have to check for uniqueness here because it is enforced by the HashTable
                //        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                //    }
                //}


                //// Now add each of the individual lines, prepended with the provider name
                //foreach (IDataProvider dp in _dataProviders)
                //{
                //    string[] formats = dp.DialogWriteFilter.Split('|');
                //    string potentialFormat = null;
                //    for (int i = 0; i < formats.Length; i++)
                //    {
                //        if (i%2 == 0)
                //        {
                //            // For descriptions, prepend the name:
                //            potentialFormat += "|" + dp.Name + " - " + formats[i];
                //        }
                //        else
                //        {
                //            if (_preferredProviders.ContainsKey(formats[i]) == false)
                //            {
                //                if (dp is IRasterProvider)
                //                {
                //                    result += potentialFormat;
                //                    result += "|" + formats[i];
                //                }
                //            }
                //        }
                //    }
                //}

                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _rasterWriteFilter = value; }
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open vectors."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string VectorReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_vectorReadFilter != null) return _vectorReadFilter;

                string result = null;
                List<string> extensions = new List<string>();


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is VectorProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        extensions.Add(item.Key);
                    }
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                if (dp is VectorProvider)
                                    extensions.Add(potentialExtension);
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                    result += "Vectors|" + String.Join(";", extensions.ToArray());


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is VectorProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat = "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            // don't add this format if it was already added by a "preferred data provider"
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                if (dp is VectorProvider)
                                {
                                    result += potentialFormat;
                                    result += "|" + formats[i];
                                }
                            }
                        }
                    }
                }
                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _vectorReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save vectors."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string VectorWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_vectorWriteFilter != null) return _vectorWriteFilter;

                List<string> extensions = new List<string>();
  

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');

                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                if (dp is VectorProvider)
                                    extensions.Add(potentialExtension);
                            }
                        }
                    }
                }


                // We now have a list of all the file extensions supported
                string result = "All Supported Formats|" + String.Join(";", extensions.ToArray());

                if (extensions.Count > 0)
                    result += "|Vectors|" + String.Join(";", extensions.ToArray());

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is VectorProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }


                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat += "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                if (dp is VectorProvider)
                                {
                                    result += potentialFormat;
                                    result += "|" + formats[i];
                                }
                            }
                        }
                    }
                }

                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _vectorWriteFilter = value; }
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open images."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public virtual string ImageReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_imageReadFilter != null) return _imageReadFilter;

                string result = null;
                List<string> extensions = new List<string>();


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is IImageDataProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        extensions.Add(item.Key);
                    }
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                if (dp is IImageDataProvider)
                                    extensions.Add(potentialExtension);
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                    result += "Images|" + String.Join(";", extensions.ToArray());

                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is IImageDataProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat = "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            // don't add this format if it was already added by a "preferred data provider"
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                if (dp is IImageDataProvider)
                                {
                                    result += potentialFormat;
                                    result += "|" + formats[i];
                                }
                            }
                        }
                    }
                }
                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _imageReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save images."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string ImageWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_imageWriteFilter != null) return _imageWriteFilter;

                string result = null;
                List<string> extensions = new List<string>();


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');

                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                if (dp is IImageDataProvider)
                                    extensions.Add(potentialExtension);
                            }
                        }
                    }
                }


                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                    result += "Images|" + String.Join(";", extensions.ToArray());


                foreach (KeyValuePair<string, IDataProvider> item in _preferredProviders)
                {
                    if (item.Value is IImageDataProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in _dataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat += "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                if (dp is IImageDataProvider)
                                {
                                    result += potentialFormat;
                                    result += "|" + formats[i];
                                }
                            }
                        }
                    }
                }

                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _imageWriteFilter = value; }
        }


        /// <summary>
        /// Sets the default condition for how this data manager should try to load layers.
        /// This will be overridden if the inRam property is specified as a parameter.
        /// </summary>
        [Category("Behavior"),
         Description(
             "Gets or sets the default condition for subsequent load operations which may be overridden by specifying inRam in the parameters."
             )]
        public bool LoadInRam
        {
            get { return _loadInRam; }
            set { _loadInRam = value; }
        }


        /// <summary>
        /// Gets or sets a dictionary of IDataProviders with corresponding extensions.  The 
        /// standard order is to try to load the data using a PreferredProvider.  If that
        /// fails, then it will check the list of dataProviders, and finally, if that fails,
        /// it will check the plugin Data Providers in directories.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Dictionary<string, IDataProvider> PreferredProviders
        {
            get { return _preferredProviders; }
            set { _preferredProviders = value; }
        }

        /// <summary>
        /// Gets or sets a progress handler for any open operations that are intiated by this 
        /// DataManager and don't override this value with an IProgressHandler specified in the parameters.
        /// </summary>
        [Category("Handlers"),
         Description(
             "Gets or sets the object that implements the IProgressHandler interface for recieving status messages."),
         TypeConverter(typeof (mwStatusStrip))] // if the designer wants something here, should work.
            public virtual IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the directory providers have been loaded into the project.
        /// </summary>
        public virtual event EventHandler<DataProviders> DirectoryProvidersLoaded;

        /// <summary>
        /// Triggers the DirectoryProvidersLoaded event
        /// </summary>
        protected virtual void OnProvidersLoaded(List<IDataProvider> list)
        {
            if (DirectoryProvidersLoaded != null)
                DirectoryProvidersLoaded(this, new DataProviders(list));
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }


        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// This will not affect the PreferredProviders or the general list of Providers.
        /// These automatically have the lowest priority and will only be used if nothing
        /// else works.  Use the PreferredProviders to force preferential loading of 
        /// a plugin DataProvider.
        /// </summary>
        /// <returns>A list of just the newly added DataProviders from this method.</returns>
        public virtual List<IDataProvider> LoadProvidersFromDirectories()
        {
            Assembly asm;
            List<IDataProvider> result = new List<IDataProvider>();
            foreach (string directory in _dataProviderDirectories)
            {
                foreach (string file in Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories))
                {
                    if (file.Contains("Interop")) continue;
                    if (Path.GetFileName(file) == "MapWindow.dll")
                        continue; // If they forget to turn "copy local" to false.
                    try
                    {
                        AssemblyName.GetAssemblyName(file);
                    }
                    catch (BadImageFormatException)
                    {
                        System.Diagnostics.Debug.WriteLine("Skipping Non-Dot Net Dll: " + file);
                        continue;
                    }
                    try
                    {
                        
                        asm = Assembly.LoadFrom(file);
                        var attributes = asm.GetCustomAttributes(typeof(MapWindowPluginAssembly),true);
                        if (attributes.Length == 0) continue; // don't bother with a dll unless it marks itself as a MW plugin assembly
                        Type[] coClassList = asm.GetTypes();
                        foreach (Type coClass in coClassList)
                        {
                            if (coClass.ContainsGenericParameters || !coClass.IsClass ||
                                coClass.IsAbstract) continue;
                            if (coClass.IsCOMObject && Type.GetType("Mono.Runtime") != null)
                            {
                                System.Diagnostics.Debug.WriteLine("Mono cannot work with COM objects.");
                                break; // if one is a com object, they will all be
                            }
                            try
                            {
                             
                                object obj = asm.CreateInstance(coClass.FullName);
                                IDataProvider dp = obj as IDataProvider;
                                if (dp != null)
                                {
                                    _dataProviders.Insert(0, dp);
                                    result.Insert(0, dp);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex);
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }
            _imageReadFilter = null;
            _imageWriteFilter = null;
            _rasterReadFilter = null;
            _rasterWriteFilter = null;
            _vectorReadFilter = null;
            _vectorWriteFilter = null;
            OnProvidersLoaded(result);
            return result;
        }

        /// <summary>
        /// Given a string filename for the "*.dll" file, this will attempt to load any classes that implement the
        /// IDataProvder interface.
        /// </summary>
        /// <param name="filename">The string path of the assembly to load from.</param>
        /// <returns>A list that contains only the providers that were just loaded.  This may be a list of count 0, but shouldn't return null.</returns>
        public virtual List<IDataProvider> LoadProvidersFromAssembly(string filename)
        {
            List<IDataProvider> result = new List<IDataProvider>();
            if (Path.GetExtension(filename) != ".dll") return result;
            if (filename.Contains("Interop")) return result;

            Assembly asm = Assembly.LoadFrom(filename);
            Type[] coClassList = asm.GetTypes();
            try
            {
                foreach (Type coClass in coClassList)
                {
                    try
                    {
                        object obj = asm.CreateInstance(coClass.FullName);
                        IDataProvider dp = obj as IDataProvider;
                        if (dp != null)
                        {
                            _dataProviders.Add(dp);
                            result.Insert(0, dp);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            OnProvidersLoaded(result);
            return result;
        }

        #endregion
    }
}