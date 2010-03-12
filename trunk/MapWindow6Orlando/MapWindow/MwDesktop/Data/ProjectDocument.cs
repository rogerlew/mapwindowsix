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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:18:50 PM
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
using System.Xml;
using System.IO;
namespace MapWindow.Data
{


    /// <summary>
    /// ProjectDocument
    /// </summary>
    public class ProjectDocument
    {
        #region Private Variables

        private Envelope _extents;
        private string _name;
        private string _type;
        private string _version;
        private string _configPath;
        private string _projection;
        private string _mapUnits;
        private ScaleBarInfo _scaleBar;
        private ResizeBehaviors _resizeBehavior;
        // private bool _showStatusBarCoords;
        private StatusBarInfo _statusBar;
        private List<PluginInfo> _plugins;
        private List<PluginInfo> _applicationPlugins;
        private TryXmlDocument _doc;
        private Color _backColor;
        private bool _useDefaultBackColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectDocument
        /// </summary>
        public ProjectDocument()
        {
            _plugins = new List<PluginInfo>();
            _applicationPlugins = new List<PluginInfo>();
            _doc = new TryXmlDocument();
           
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a *.mwprj file.
        /// </summary>
        /// <param name="filename">The string filename of the mwprj file to parse.</param>
        public virtual void Open(string filename)
        {

            _doc.Open(filename);

            Directory.SetCurrentDirectory(Path.GetDirectoryName(filename));

            // Main document Properties
            _name = _doc.ReadText("name");
            _type = _doc.ReadText("type");
            _version = _doc.ReadText("version");
            _configPath = _doc.ReadText("ConfigurationPath");
            _projection = _doc.ReadText("ProjectProjection");
            _mapUnits = _doc.ReadText("MapUnits");
            _backColor = _doc.ReadColor("ViewBackColor");
            _useDefaultBackColor = _doc.ReadBool("ViewBackColor_UseDefault");
            _scaleBar = new ScaleBarInfo();
            _scaleBar.Visible = _doc.ReadBool("ShowFloatingScaleBar");
            _scaleBar.Position = (ScaleBarPositions)Enum.Parse(typeof(ScaleBarPositions), _doc.ReadText("FloatingScaleBarPosition"));
            _scaleBar.Unit = _doc.ReadText("FloatingScaleBarUnit");
            _scaleBar.ForeColor = _doc.ReadColor("FloatingScaleBarForecolor");
            _scaleBar.BackColor = _doc.ReadColor("FloatingScaleBarBackcolor");
            _resizeBehavior = (ResizeBehaviors)_doc.ReadInteger("MapResizeBehavior");
            _statusBar.GetFromProjection = _doc.ReadBool("ShowStatusBarCoords_Projected");
            _statusBar.Alternate = _doc.ReadText("ShowStatusBarCoords_Alternate");

            if (_doc.NavigateToChild("Plugins"))
            {
                foreach (XmlElement plugin in _doc.CurrentElement.ChildNodes)
                {
                    PluginInfo pi = new PluginInfo(plugin.GetAttribute("SettingsString"), plugin.GetAttribute("Key"));
                    _plugins.Add(pi);
                }
                _doc.NavigateToParent();
            }
            if (_doc.NavigateToChild("ApplicationPlugins"))
            {
                foreach (XmlElement plugin in _doc.CurrentElement.ChildNodes)
                {
                    PluginInfo pi = new PluginInfo(plugin.GetAttribute("SettingsString"), plugin.GetAttribute("Key"));
                    _plugins.Add(pi);
                }
                _doc.NavigateToParent();
            }
            if (_doc.NavigateToChild("Extents"))
            {
                double xMin = _doc.ReadDouble("xMin");
                double xMax = _doc.ReadDouble("xMax");
                double yMin = _doc.ReadDouble("yMin");
                double yMax = _doc.ReadDouble("yMax");
                _extents = new Envelope(xMin, xMax, yMin, yMax);
            }
        }

       


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of application plugins
        /// </summary>
        public List<PluginInfo> ApplicationPlugins
        {
            get { return _applicationPlugins; }
            set { _applicationPlugins = value; }
        }

        /// <summary>
        /// Gets or sets the color to use as the background color for the map in the case where UseDefaultBackColor is false. 
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the string path for the configuration file to use when organizing the layout for mapwindow
        /// </summary>
        public string ConfigurationPath
        {
            get { return _configPath; }
            set { _configPath = value; }
        }


        /// <summary>
        /// gets or sets the extents of the project.
        /// </summary>
        public Envelope Extents
        {
            get { return _extents; }
            set { _extents = value; }
        }

        /// <summary>
        /// Gets or sets the MapUnits for this project.
        /// </summary>
        public string MapUnits
        {
            get { return _mapUnits; }
            set { _mapUnits = value; }
        }

        /// <summary>
        /// Gets or sets the name that should appear in the caption of the project.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the list of information controling the regular plugins.
        /// </summary>
        public List<PluginInfo> Plugins
        {
            get { return _plugins; }
            set { _plugins = value; }
        }

        /// <summary>
        /// Gets or sets the projection string that should be used for the project
        /// </summary>
        public string Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// Gets or sets the resize behavior.
        /// </summary>
        public ResizeBehaviors ResizeBehavior
        {
            get { return _resizeBehavior; }
            set { _resizeBehavior = value; }
        }

        /// <summary>
        /// Gets or sets the information that controls the positioning and information about the scale bar.
        /// </summary>
        public ScaleBarInfo ScaleBar
        {
            get { return _scaleBar; }
            set { _scaleBar = value; }
        }

        /// <summary>
        /// Gets or sets the information that controls the structuring of the units in the status bar.
        /// </summary>
        public StatusBarInfo StatusBar
        {
            get { return _statusBar; }
            set { _statusBar = value; }
        }
      

        /// <summary>
        /// Gets a description of what type of project file this is
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets a boolean controling whether the default background color is used, or if the background
        /// color specified by this document class should be used.
        /// </summary>
        public bool UseDefaultBackColor
        {
            get { return _useDefaultBackColor; }
            set { _useDefaultBackColor = value; }
        }

        /// <summary>
        /// Gets or sets the version number of MapWindow used to create the project.
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        #endregion



    }
}
