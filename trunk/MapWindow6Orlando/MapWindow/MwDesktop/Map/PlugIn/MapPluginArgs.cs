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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 5:56:54 PM
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
using System.Windows.Forms;
using MapWindow.Components;
namespace MapWindow.Map
{


    /// <summary>
    /// GeoPluginArgs
    /// </summary>
    public class GeoPluginArgs : IMapPluginArgs
    {
        #region Private Variables

        private IMap _map;
        private ILegend _legend;
        private IProgressHandler _progressHandler;
        private MenuStrip _mainMenu;
        private ToolStrip _mainToolStrip;
        private List<IMapPlugin> _plugins;
        private ToolStripContainer _toolStripContainer;
        private PanelManager _panelManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoPluginArgs
        /// </summary>
        public GeoPluginArgs()
        {

        }
        /// <summary>
        /// Creates a new instance of the GeoPluginArgs
        /// </summary>
        /// <param name="map">Each Manager is associated with a single map</param>
        /// <param name="legend">The legend</param>
        /// <param name="mainMenu">The main menu</param>
        /// <param name="mainToolStrip">The main toolstrip</param>
        /// <param name="progressHandler">The progress handler</param>
        /// <param name="plugins">The list of plugins controlled by the manager</param>
        /// <param name="toolStripContainer">The container where any toolstrips should be added</param>
        public GeoPluginArgs(IMap map, ILegend legend, MenuStrip mainMenu, ToolStrip mainToolStrip, IProgressHandler progressHandler, List<IMapPlugin> plugins, ToolStripContainer toolStripContainer)
        {
            _toolStripContainer = toolStripContainer;
            _map = map;
            _legend = legend;
            _mainMenu = mainMenu;
            _mainToolStrip = mainToolStrip;
            _plugins = plugins;
            _progressHandler = progressHandler;
        }

        /// <summary>
        /// Creates a new instance of the GeoPluginArgs
        /// </summary>
        /// <param name="map">Each Manager is associated with a single map</param>
        /// <param name="legend">The legend</param>
        /// <param name="mainMenu">The main menu</param>
        /// <param name="mainToolStrip">The main toolstrip</param>
        /// <param name="progressHandler">The progress handler</param>
        /// <param name="plugins">The list of plugins controlled by the manager</param>
        /// <param name="toolStripContainer">The container where any toolstrips should be added</param>
        /// <param name="panelManager">The panel manager for adding tabs and panels</param>
        public GeoPluginArgs(IMap map, ILegend legend, MenuStrip mainMenu, ToolStrip mainToolStrip, IProgressHandler progressHandler, List<IMapPlugin> plugins, ToolStripContainer toolStripContainer, PanelManager panelManager)
        {
            _toolStripContainer = toolStripContainer;
            _map = map;
            _legend = legend;
            _mainMenu = mainMenu;
            _mainToolStrip = mainToolStrip;
            _plugins = plugins;
            _progressHandler = progressHandler;
            _panelManager = panelManager;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Map associated with the plugin manager
        /// </summary>
        public IMap Map
        {
            get { return _map; }
        }

        /// <summary>
        /// Gets the Legend (Table of Contents) associated with the plugin manager
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
        }

        /// <summary>
        /// Gets the Main Menu, if any, associated with the plugin manager
        /// </summary>
        public MenuStrip MainMenu
        {
            get { return _mainMenu; }
        }

        /// <summary>
        /// Gets the ToolStrip if any associated with the plugin manager,
        /// if any.
        /// </summary>
        public ToolStrip MainToolStrip
        {
            get { return _mainToolStrip; }
        }

        /// <summary>
        /// Gets the progress handler that is being used to display status messages.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
        }

        /// <summary>
        /// Gets the list of plugins
        /// </summary>
        public List<IMapPlugin> Plugins
        {
            get { return _plugins; }
        }

        /// <summary>
        /// Gets the actual container for the tool strips
        /// </summary>
        public ToolStripContainer ToolStripContainer
        {
            get { return _toolStripContainer; }
        }

        public PanelManager PanelManager
        {
            get { return _panelManager; }
        }

        #endregion



    }
}
