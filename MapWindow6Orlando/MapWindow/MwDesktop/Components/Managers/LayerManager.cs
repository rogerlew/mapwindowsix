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
using System.Drawing;
using MapWindow.Main;
using MapWindow.Drawing;
using MapWindow.Plugins;
using System.Windows.Forms;
namespace MapWindow.Components
{
    /// <summary>
    /// This can be used as a component to work as a LayerManager.  This also provides the
    /// very important DefaultLayerManager property, which is where the developer controls
    /// what LayerManager should be used for their project.
    /// </summary>
    [ToolboxBitmap("LayerManager.ico")]
    public class LayerManager : ILayerManager
    {

        #region Private Variables

        // If this doesn't exist, a new one is created when you "get" this data manager.
        private static ILayerManager _defaultLayerManager;
        private System.ComponentModel.IContainer components = null; //Required designer variable.
        private List<ILayerProvider> _layerProviders;
        private IList<ILayer> _activeProjectLayers;

        /// <summary>
        /// Gets or sets a temporary list of active project layers.  This is designed to house 
        /// the layers from a map frame when the property grids are shown for layers in that
        /// map frame.  This list on the DefaultLayerManager is what is used to create the
        /// list that populates dropdowns that can take a Layer as a parameter.
        /// </summary>
        public IList<ILayer> ActiveProjectLayers
        {
            get { return _activeProjectLayers; }
            set { _activeProjectLayers = value; }
        }
        
        



        private string _dialogReadFilter;
        private string _rasterReadFilter;
        private string _vectorReadFilter;
        private string _imageReadFilter;

        private string _dialogWriteFilter;
        private string _rasterWriteFilter;
        private string _vectorWriteFilter;
        private string _imageWriteFilter;


        private IProgressHandler _progressHandler;
        
        private System.Collections.Generic.Dictionary<string, ILayerProvider> _preferredProviders;

        private List<string> _layerProviderDirectories;

        private bool _loadInRam = true;

       
        
        /// <summary>
        /// Gets or sets the implemenation of ILayerManager for the project to use when
        /// accessing data.  This is THE place where the LayerManager can be replaced
        /// by a different data manager.  If you add this data manager to your
        /// project, this will automatically set itself as the DefaultLayerManager.
        /// However, since each DM will do this, you may have to control this manually
        /// if you add more than one LayerManager to the project in order to set the
        /// one that will be chosen.
        /// </summary>
        public static ILayerManager DefaultLayerManager
        {
            get 
            {
                if (LayerManager._defaultLayerManager == null)
                {
                    _defaultLayerManager = new LayerManager();
                }
                return LayerManager._defaultLayerManager;

            }
            set 
            {
                LayerManager._defaultLayerManager = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the LayerManager class.  A data manager is more or less
        /// just a list of data providers to use.  The very important 
        /// LayerManager.DefaultLayerManager property controls which LayerManager will be used
        /// to load data.  By default, each LayerManager sets itself as the default in its
        /// constructor.
        /// </summary>
        public LayerManager()
        {
           // InitializeComponent();

            _defaultLayerManager = this;
            _layerProviders = new List<ILayerProvider>();
            _layerProviders.Add(new ShapefileLayerProvider()); // .shp files
            _layerProviders.Add(new BinaryLayerProvider()); // .bgd files
          

            string path = System.Windows.Forms.Application.ExecutablePath;
            _layerProviderDirectories = new List<string>();
            _layerProviderDirectories.Add(System.IO.Path.GetDirectoryName(path) + "\\Plugins");
            _preferredProviders = new Dictionary<string, ILayerProvider>();
            
        }

       
        #endregion


        #region Methods

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
                    {
                        extensions.Add(ext);
                    }
                }


            }
            return extensions;

        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public virtual IRasterLayer OpenRasterLayer()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = RasterReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenLayer(ofd.FileName, this.LoadInRam, null, this.ProgressHandler) as IRasterLayer;

        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        public virtual IFeatureLayer OpenVectorLayer()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = VectorReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenLayer(ofd.FileName, this.LoadInRam, null, this.ProgressHandler) as IFeatureLayer;

        }

        /// <summary>
        /// This attempts to open the specified raster file and returns an associated layer
        /// </summary>
        /// <param name="filename">The string filename to open</param>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public virtual IRasterLayer OpenRasterLayer(string filename)
        {
            return OpenLayer(filename, this.LoadInRam, null, this.ProgressHandler) as IRasterLayer;

        }

        /// <summary>
        /// This attempts to open the specified vector file and returns an associated layer
        /// </summary>
        /// <param name="filename">the string filename to open</param>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        public virtual IFeatureLayer OpenVectorLayer(string filename)
        {
            return OpenLayer(filename, this.LoadInRam, null, this.ProgressHandler) as IFeatureLayer;
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>for now an ILayerSet</returns>
        public virtual ILayer OpenImageLayer()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ImageReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenLayer(ofd.FileName, this.LoadInRam, null, this.ProgressHandler);

        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        public virtual ILayer OpenLayer(ICollection<ILayer> container)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenLayer(ofd.FileName, this.LoadInRam, container, this.ProgressHandler);
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        /// <returns>A Layer</returns>
        public virtual ILayer OpenLayer(IProgressHandler progressHandler)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenLayer(ofd.FileName, this.LoadInRam, null, progressHandler);
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <returns>A Layer created from the file</returns>
        public virtual ILayer OpenLayer()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return OpenLayer(ofd.FileName, this.LoadInRam, null, this.ProgressHandler);
        }

       
        

        /// <summary>
        /// Attempts to call the open filename method for any ILayerProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        public virtual ILayer OpenLayer(string filename)
        {
            return OpenLayer(filename, this.LoadInRam, null, this.ProgressHandler);
            
        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        public virtual ILayer OpenLayer(string filename, ICollection<ILayer> container)
        {
            return OpenLayer(filename, this.LoadInRam, container, this.ProgressHandler);
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        /// <returns>A Layer</returns>
        public virtual ILayer OpenLayer(string filename, IProgressHandler progressHandler)
        {
            return OpenLayer(filename, this.LoadInRam, null, progressHandler);
        }

        /// <summary>
        /// Attempts to call the open filename method for any ILayerProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this LayerManager.</param>
        /// <param name="container">A container to open this layer in</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this LayerManager.</param>
        /// <returns>An ILayer</returns>
        public virtual ILayer OpenLayer(string filename, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            // To Do: Add Customization that allows users to specify which plugins to use in priority order.
            ILayer result;

            // First check for the extension in the preferred plugins list

            string ext = System.IO.Path.GetExtension(filename);
            if (_preferredProviders.ContainsKey(ext))
            {
                result = _preferredProviders[ext].OpenLayer(filename, inRam, container, progressHandler);
                if (result != null)
                {
                    return result;
                }
                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers
          
            foreach (ILayerProvider dp in _layerProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    // attempt to open with the filename.
                    result = dp.OpenLayer(filename, inRam, container, progressHandler);
                    if (result != null)
                    {
                        return result;
                       
                    }
                }
            }

            throw new ApplicationException(MessageStrings.FileTypeNotSupported);

        }




       


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
        public virtual IRasterLayer CreateRasterLayer(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            IRasterLayer result;

            // First check for the extension in the preferred plugins list

            string ext = System.IO.Path.GetExtension(name);
            if (_preferredProviders.ContainsKey(ext))
            {
                IRasterLayerProvider rp = _preferredProviders[ext] as IRasterLayerProvider;
                if (rp != null)
                {
                    result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                    if (result != null)
                    {
                        return result;
                    }
                }
               
                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers

            foreach (ILayerProvider dp in _layerProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    IRasterLayerProvider rp = dp as IRasterLayerProvider;
                    if (rp != null)
                    {
                        // attempt to open with the filename.
                        result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                        if (result != null)
                        {
                            return result;

                        }

                    }
                    
                }
            }

            throw new ApplicationException(MessageStrings.FileTypeNotSupported);

        }


        #endregion

        #region Properties

        

        // May make this invisible if we can
        /// <summary>
        /// Gets or sets the list of ILayerProviders that should be used in the project.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual List<ILayerProvider> LayerProviders
        {
            get { return _layerProviders; }
            set 
            {
                _layerProviders = value; 
            }
        }



        /// <summary>
        /// Gets or sets the path (either as a full path or as a path relative to
        /// the MapWindow.dll) to search for plugins that implement the ILayerProvider interface.
        /// </summary>
        [Category("Providers"),
         Description("Gets or sets the list of string path names that should be used to search for ILayerProvider interfaces.")]
        public virtual List<string> LayerProviderDirectories
        {
            get { return _layerProviderDirectories; }
            set { _layerProviderDirectories = value; }
        }

        


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open files.")]
        public virtual string DialogReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_dialogReadFilter != null) return _dialogReadFilter;

                string result;
                List<string> extensions = new List<string>();
                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    extensions.Add(item.Key);
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    rasterExtensions.Add(potentialExtension);
                                }
                                if (dp is VectorProvider)
                                {
                                    vectorExtensions.Add(potentialExtension);
                                }
                                if (dp is IImageDataProvider)
                                {
                                    imageExtensions.Add(potentialExtension);
                                }
                            }
                        }
                    }
                }
                
                // We now have a list of all the file extensions supported
                result = "All Supported Formats|" + String.Join(";", extensions.ToArray());
                if (vectorExtensions.Count > 0)
                {
                    result += "|Vectors|" + String.Join(";", vectorExtensions.ToArray());
                }
                if (rasterExtensions.Count > 0)
                {
                    result += "|Rasters|" + String.Join(";", rasterExtensions.ToArray());
                }
                if (imageExtensions.Count > 0)
                {
                    result += "|Images|" + String.Join(";", imageExtensions.ToArray());
                }

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        
                        if (i % 2 == 0)
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
                return result;
            }
            set { _dialogReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save files.")]
        public virtual string DialogWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_dialogWriteFilter != null) return _dialogWriteFilter;

                string result = null;
                List<string> extensions = new List<string>();
                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    rasterExtensions.Add(potentialExtension);
                                }
                                if (dp is VectorProvider)
                                {
                                    vectorExtensions.Add(potentialExtension);
                                }
                                if (dp is IImageDataProvider)
                                {
                                    imageExtensions.Add(potentialExtension);
                                }
                            }
                        }


                    }
                }
               

                // We now have a list of all the file extensions supported
                result = "All Supported Formats|" + String.Join(";", extensions.ToArray());

                if (vectorExtensions.Count > 0)
                {
                    result += "|Vectors|" + String.Join(";", vectorExtensions.ToArray());
                }
                if (rasterExtensions.Count > 0)
                {
                    result += "|Rasters|" + String.Join(";", rasterExtensions.ToArray());
                }
                if (imageExtensions.Count > 0)
                {
                    result += "|Images|" + String.Join(";", imageExtensions.ToArray());
                }


                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i % 2 == 0)
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
         Description("Gets or sets the string that should be used when using this data manager is used to open rasters.")]
        public virtual string RasterReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_rasterReadFilter != null) return _rasterReadFilter;

                string result;
                List<string> extensions = new List<string>();
               

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        extensions.Add(item.Key);
                    }
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    extensions.Add(potentialExtension);
                                }
                                
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                result = "Rasters|" + String.Join(";", extensions.ToArray());
                
                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {

                        if (i % 2 == 0)
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
         Description("Gets or sets the string that should be used when this data manager is used to save rasters.")]
        public virtual string RasterWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_rasterWriteFilter != null) return _rasterWriteFilter;

                string result = null;
                List<string> extensions = new List<string>();
               

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                    {
                        extensions.Add(item.Key);
                    }
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    extensions.Add(potentialExtension);
                                }
                               
                            }
                        }


                    }
                }


                // We now have a list of all the file extensions supported
               
                if (extensions.Count > 0)
                {
                    result += "Rasters|" + String.Join(";", extensions.ToArray());
                }


                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IRasterProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }


                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat += "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
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
            set { _rasterWriteFilter = value; }
        }


        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open vectors.")]
        public virtual string VectorReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_vectorReadFilter != null) return _vectorReadFilter;

                string result = null;
                List<string> extensions = new List<string>();
               

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is VectorProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        extensions.Add(item.Key);
                    }
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    extensions.Add(potentialExtension);
                                }
                               
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                {
                    result += "Vectors|" + String.Join(";", extensions.ToArray());
                }
                

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is VectorProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {

                        if (i % 2 == 0)
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
         Description("Gets or sets the string that should be used when this data manager is used to save vectors.")]
        public virtual string VectorWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_vectorWriteFilter != null) return _vectorWriteFilter;

                string result = null;
                List<string> extensions = new List<string>();
                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    extensions.Add(potentialExtension);
                                }
                               
                            }
                        }


                    }
                }


                // We now have a list of all the file extensions supported
                result = "All Supported Formats|" + String.Join(";", extensions.ToArray());

                if (extensions.Count > 0)
                {
                    result += "|Vectors|" + String.Join(";", extensions.ToArray());
                }

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is VectorProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }


                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i % 2 == 0)
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
         Description("Gets or sets the string that should be used when using this data manager is used to open images.")]
        public virtual string ImageReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_imageReadFilter != null) return _imageReadFilter;

                string result = null;
                List<string> extensions = new List<string>();
               

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IImageDataProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        extensions.Add(item.Key);
                    }
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    extensions.Add(potentialExtension);
                                }
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                {
                    result += "Images|" + String.Join(";", extensions.ToArray());
                }

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IImageDataProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {

                        if (i % 2 == 0)
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
         Description("Gets or sets the string that should be used when this data manager is used to save images.")]
        public virtual string ImageWriteFilter
        {
            get
            {
                // Setting this to something overrides the default 
                if (_imageWriteFilter != null) return _imageWriteFilter;

                string result = null;
                List<string> extensions = new List<string>();
                

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (ILayerProvider dp in _layerProviders)
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
                                {
                                    extensions.Add(potentialExtension);
                                }
                            }
                        }


                    }
                }


                // We now have a list of all the file extensions supported

                if (extensions.Count > 0)
                {
                    result += "Images|" + String.Join(";", extensions.ToArray());
                }


                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    if (item.Value is IImageDataProvider)
                    {
                        // we don't have to check for uniqueness here because it is enforced by the HashTable
                        result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                    }
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i % 2 == 0)
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
         Description("Gets or sets the default condition for subsequent load operations which may be overridden by specifying inRam in the parameters.")]
        public bool LoadInRam
        {
            get { return _loadInRam; }
            set { _loadInRam = value; }
        }


        /// <summary>
        /// Gets or sets a dictionary of ILayerProviders with corresponding extensions.  The 
        /// standard order is to try to load the data using a PreferredProvider.  If that
        /// fails, then it will check the list of dataProviders, and finally, if that fails,
        /// it will check the plugin Layer Providers in directories.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Collections.Generic.Dictionary<string, ILayerProvider> PreferredProviders
        {
            get { return _preferredProviders; }
            set { _preferredProviders = value; }
        }

        /// <summary>
        /// Gets or sets a progress handler for any open operations that are intiated by this 
        /// LayerManager and don't override this value with an IProgressHandler specified in the parameters.
        /// </summary>
        [Category("Handlers"),
         Description("Gets or sets the object that implements the IProgressHandler interface for recieving status messages."),
         TypeConverter(typeof(mwStatusStrip))] // if the designer wants something here, should work.
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
        public virtual event EventHandler<MapWindow.Main.LayerProviders> DirectoryProvidersLoaded;

        /// <summary>
        /// Triggers the DirectoryProvidersLoaded event
        /// </summary>
        protected virtual void OnProvidersLoaded(List<ILayerProvider> list)
        {
            if (DirectoryProvidersLoaded != null)
            {
                DirectoryProvidersLoaded(this, new MapWindow.Main.LayerProviders(list));
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

       

        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// This will not affect the PreferredProviders or the general list of Providers.
        /// These automatically have the lowest priority and will only be used if nothing
        /// else works.  Use the PreferredProviders to force preferential loading of 
        /// a plugin LayerProvider.
        /// </summary>
        /// <returns>A list of just the newly added LayerProviders from this method.</returns>
        public virtual List<ILayerProvider> LoadProvidersFromDirectories()
        {

            Type[] CoClassList;
            Type[] InfcList;
            System.Reflection.Assembly asm;
            List<ILayerProvider> result = new List<ILayerProvider>();
            foreach (string directory in _layerProviderDirectories)
            {
                foreach (string file in System.IO.Directory.GetFiles(directory, "*.dll", System.IO.SearchOption.AllDirectories))
                {
                    if (file.Contains("Interop")) continue;
                    if (System.IO.Path.GetFileName(file) == "MapWindow.dll") continue; // If they forget to turn "copy local" to false.

                    asm = System.Reflection.Assembly.LoadFrom(file);
                    try
                    {
                        CoClassList = asm.GetTypes();
                        foreach (Type CoClass in CoClassList)
                        {
                            InfcList = CoClass.GetInterfaces();

                            foreach (Type Infc in InfcList)
                            {

                                try
                                {
                                    object obj = asm.CreateInstance(CoClass.FullName);
                                    ILayerProvider dp = obj as ILayerProvider;
                                    if (dp != null)
                                    {
                                        _layerProviders.Add(dp);
                                        result.Add(dp);
                                    }
                                }
                                catch
                                {
                                    // this object didn't work, but keep looking
                                }

                            }
                        }
                    }
                    catch
                    {
                        // We will fail frequently.
                    }
                }
            }
            OnProvidersLoaded(result);
            return result;

        }

        /// <summary>
        /// Given a string filename for the "*.dll" file, this will attempt to load any classes that implement the
        /// ILayerProvder interface.
        /// </summary>
        /// <param name="filename">The string path of the assembly to load from.</param>
        /// <returns>A list that contains only the providers that were just loaded.  This may be a list of count 0, but shouldn't return null.</returns>
        public virtual List<ILayerProvider> LoadProvidersFromAssembly(string filename)
        {
            
            Type[] CoClassList;
            Type[] InfcList;
            System.Reflection.Assembly asm;
            List<ILayerProvider> result = new List<ILayerProvider>();
            if (System.IO.Path.GetExtension(filename) != ".dll") return result;
            if (filename.Contains("Interop")) return result;

            asm = System.Reflection.Assembly.LoadFrom(filename);
            try
            {
                CoClassList = asm.GetTypes();
                foreach (Type CoClass in CoClassList)
                {
                    InfcList = CoClass.GetInterfaces();

                    foreach (Type Infc in InfcList)
                    {

                        try
                        {
                            object obj = asm.CreateInstance(CoClass.FullName);
                            ILayerProvider dp = obj as ILayerProvider;
                            if (dp != null)
                            {
                                _layerProviders.Add(dp);
                                result.Add(dp);
                            }
                        }
                        catch
                        {
                            // this object didn't work, but keep looking
                        }

                    }
                }
            }
            catch
            {
                // We will fail frequently.
            }
               
            OnProvidersLoaded(result);
            return result;
        }

      

        #endregion
    }
}
