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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 6:17:22 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.ComponentModel;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Map;
namespace MapWindow.Map
{


    /// <summary>
    /// GeoPluginManager
    /// </summary>
    public class PluginManager
    {
        private IMap _map;
        private ILegend _legend;
        private MenuStrip _mainMenu;
        private ToolStrip _toolStrip;
        private IProgressHandler _progressHandler;
        private List<IMapPlugin> _geoPlugins;
        private List<string> _directories;
        private IContainer components;
        private TokenManager _tokenManager;
        private ToolStripContainer _toolStripContainer;
        private PanelManager _panelManager;
        private Dictionary<PluginToken, IMapPlugin> _tokenPlugins;

        /// <summary>
        /// Creates a new instance of the GeoPluginManager
        /// </summary>
        public PluginManager()
        {
            InitializeComponent();
            _directories = new List<string>();
            _geoPlugins = new List<IMapPlugin>();
            _directories.Add("Plugins");
            _tokenPlugins = new Dictionary<PluginToken, IMapPlugin>();
        }

        /// <summary>
        /// Forces this component to add its plugin members
        /// </summary>
        public void Update()
        {
            _tokenManager = new TokenManager();
            _tokenManager.Directories = _directories;
            _tokenManager.UpdateStore();
            if (_mainMenu != null)
            {
                ToolStripItem[] items = _mainMenu.Items.Find(MessageStrings.Plugins, true);
                ToolStripMenuItem mi = null;
                if (items.Length > 0) mi = items[0] as ToolStripMenuItem;
                if (mi == null) mi = _mainMenu.Items.Add(MessageStrings.Plugins) as ToolStripMenuItem;
                IMapPluginArgs args = new GeoPluginArgs(_map, _legend, _mainMenu, _toolStrip, _progressHandler, _geoPlugins, _toolStripContainer, _panelManager);
                foreach (PluginToken token in _tokenManager.Tokens)
                {
                    bool isGeoPlugin = false;
                    Type[] interfaces = token.PluginType.GetInterfaces();
                    foreach (Type t in interfaces)
                    {
                        if (t == typeof(IMapPlugin))
                        {
                            isGeoPlugin = true;
                            break;
                        }
                    }
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(token.Name);
                    tsmi.Click += new EventHandler(tsmi_Click);
                    tsmi.Image = Images.InactivePlugin.ToBitmap();
                    tsmi.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    tsmi.Tag = token;
                    mi.DropDownItems.Add(tsmi);
                    if (token.Enabled == true && isGeoPlugin)
                    {
                        IMapPlugin gp = token.CreateInstance<IMapPlugin>();
                        if(gp != null)
                        {
                            gp.Initialize(args);
                            _geoPlugins.Add(gp);
                            mi.Checked = true;  
                        }
                       
                        
                    }
                   
                }

            }
        }

     

        void tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            tsmi.Checked = !tsmi.Checked;
            PluginToken token = tsmi.Tag as PluginToken;
            if (tsmi.Checked)
            {
                tsmi.Image = Images.ActivePlugin.ToBitmap();
                token.Enabled = true;
                ActivateToken(token);
            }
            else
            {
                tsmi.Image = Images.InactivePlugin.ToBitmap();
                token.Enabled = false;
                DeactivateToken(token);
            }

        }

        /// <summary>
        /// Activates the specified token
        /// </summary>
        /// <param name="token">The token to activate</param>
        public void ActivateToken(PluginToken token)
        {
            IMapPlugin gp = null;
            if (_tokenPlugins.ContainsKey(token))
            {
                gp = _tokenPlugins[token];
            }
            else
            {
                gp = token.CreateInstance<IMapPlugin>();
                IMapPluginArgs args = new GeoPluginArgs(_map, _legend, _mainMenu, _toolStrip, _progressHandler, _geoPlugins, _toolStripContainer, _panelManager);
                gp.Initialize(args);
                _tokenPlugins.Add(token, gp);
            }
            
            _geoPlugins.Add(gp);
            gp.Activate();
            
        }

        /// <summary>
        /// Deactivates the current token, removing the plugin from the active plugins list.
        /// This isn't quite the same as unloading the plugin.
        /// </summary>
        /// <param name="token"></param>
        public void DeactivateToken(PluginToken token)
        {
            token.Enabled = false;
            IMapPlugin gp = _tokenPlugins[token];
            gp.Deactivate();
            // Remove from the "loaded plugins" list
            _geoPlugins.Remove(_tokenPlugins[token]);
        }

        /// <summary>
        /// Gets the Map associated with the plugin manager
        /// </summary>
        public IMap Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// Gets the Legend (Table of Contents) associated with the plugin manager
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set { _legend = value; }
        }

        /// <summary>
        /// Gets or sets the control that should become the parent of the
        /// plugin toolstrips
        /// </summary>
        public ToolStripContainer ToolStripContainer
        {
            get { return _toolStripContainer; }
            set { _toolStripContainer = value; }
        }

        /// <summary>
        /// Gets the Main Menu, if any, associated with the plugin manager
        /// </summary>
        public MenuStrip MainMenu
        {
            get { return _mainMenu; }
            set { _mainMenu = value; }
        }

        /// <summary>
        /// Gets the ToolStrip if any associated with the plugin manager,
        /// if any.
        /// </summary>
        public ToolStrip MainToolStrip
        {
            get { return _toolStrip; }
            set { _toolStrip = value; }
        }

        /// <summary>
        /// Gets the panel manager associated with the plugin manager, if any
        /// </summary>
        public PanelManager PanelManager
        {
            get { return _panelManager; }
            set { _panelManager = value; }
        }

        /// <summary>
        /// Gets the progress handler that is being used to display status messages.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        /// <summary>
        /// Gets the list of plugins
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<IMapPlugin> Plugins
        {
            get { return _geoPlugins; }
            set { _geoPlugins = value; }
        }

        /// <summary>
        /// Gets or sets the list of string paths (relative to this one) to search for plugins.
        /// </summary>
        public List<string> Directories
        {
            get { return _directories; }
            set { _directories = value; }
        }


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

       
    }
}
