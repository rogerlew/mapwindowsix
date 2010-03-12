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
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        | 03/08/2010         |  Added the PanelManager to work with tabs and panels
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using MapWindow.Main;
using MapWindow.Map;
using MapWindow.Plugins;
using MapWindow.Tools;

namespace MapWindow.Components
{


    /// <summary>
    /// GeoPluginManager
    /// </summary>
    public class ApplicationManager : Component
    {
        private IMap _map;
        private ILegend _legend;
        private MenuStrip _mainMenu;
        private ToolStrip _toolStrip;
        private IProgressHandler _progressHandler;
        private List<IMapPlugin> _geoPlugins;
        private List<string> _directories;
        private ToolStripContainer _toolStripContainer;
        private ToolManager _toolManager;
        private TokenManager _tokenManager;
        private PanelManager _panelManager;
        private readonly DataManager _dataManager;

        private Dictionary<PluginToken, IExtension> _activeTokens; // plugins/providers etc.
        private Layout.LayoutControl _layoutControl;
        private Timer loadTimer;

        /// <summary>
        /// Gets or sets the print layout control
        /// </summary>
        public Layout.LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set { _layoutControl = value; }
        }

        /// <summary>
        /// Creates a new instance of the GeoPluginManager
        /// </summary>
        public ApplicationManager()
        {
            InitializeComponent();
            _directories = new List<string>();
            _geoPlugins = new List<IMapPlugin>();
            _directories.Add("Plugins");
            _dataManager = new DataManager();
            _panelManager = new PanelManager(this);
            loadTimer = new Timer();
            loadTimer.Interval = 10;
            loadTimer.Start();
            loadTimer.Tick += loadTimer_Tick;       
        }

        void loadTimer_Tick(object sender, EventArgs e)
        {
            loadTimer.Stop();
            OnLoad();   
        }

        /// <summary>
        /// Occurs after the constructor has been called, but not in the constructor itself.
        /// </summary>
        protected virtual void OnLoad()
        {
            Update();     
        }

  
   

		/// <summary>
		/// Component.DesignMode doesn't always work correctly from constructors because the ISite isn't always assigned at that point.
		/// </summary>
		private bool DesignModeEx
		{
			get
			{
				return (GetService(typeof(IDesignerHost)) != null) || (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			}
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
                //ToolStripItem[] items = _mainMenu.Items.Find("Extensions", true);
                //ToolStripMenuItem mi = null;
                //if (items.Length > 0) mi = items[0] as ToolStripMenuItem;
                ToolStripMenuItem mi = _mainMenu.Items.Add(MessageStrings.Extensions) as ToolStripMenuItem;
                ToolStripMenuItem pluginMenu = null;
                if (mi != null)
                {
                    if (mi.DropDownItems.ContainsKey(MessageStrings.Plugins))
                    {
                        pluginMenu = mi.DropDownItems[MessageStrings.Plugins] as ToolStripMenuItem;
                    }
                    else
                    {
                        pluginMenu = mi.DropDownItems.Add(MessageStrings.Plugins) as ToolStripMenuItem;
                        if (pluginMenu != null)
                        {
                            pluginMenu.Image = Images.PluginSubmenu.ToBitmap();
                            pluginMenu.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        }
                    }
                }


                //IMapPluginArgs args = new GeoPluginArgs(_map, _legend, _mainMenu, _toolStrip, _progressHandler, _geoPlugins, _toolStripContainer);
                foreach (PluginToken token in _tokenManager.Tokens)
                {
                    if (token.CanBecome(typeof(IMapPlugin)))
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem(token.Name);
                        tsmi.Click += tsmi_Click;
                        tsmi.Image = Images.InactivePlugin.ToBitmap();
                        tsmi.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        tsmi.Tag = token;
                        if (pluginMenu != null) pluginMenu.DropDownItems.Add(tsmi);
                        // Plugins don't activate right away.
                    }
                    else
                    {
                        // If it isn't a plug-in, it should be a provider, which should load.
                        ActivateToken(token);
                    }
                }

            }
        }

        void tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi == null) return;
            tsmi.Checked = !tsmi.Checked;
            PluginToken token = tsmi.Tag as PluginToken;
            if (tsmi.Checked)
            {
                tsmi.Image = Images.ActivePlugin.ToBitmap();
                if (token != null)
                {
                    token.Enabled = true;
                    ActivateToken(token);
                }
            }
            else
            {
                tsmi.Image = Images.InactivePlugin.ToBitmap();
                if (token != null)
                {
                    token.Enabled = false;
                    DeactivateToken(token);
                }
            }
        }

       
        /// <summary>
        /// Activates the specified token
        /// </summary>
        /// <param name="token">The token to activate</param>
        public void ActivateToken(PluginToken token)
        {
            IExtension ext;
            if (_activeTokens == null) _activeTokens = new Dictionary<PluginToken, IExtension>();
            if (_activeTokens.ContainsKey(token))
            {
                ext = _activeTokens[token];
            }
            else
            {
                ext = token.CreateInstance<IExtension>();
                if (ext == null) return;
                _activeTokens.Add(token, ext);
            }


            IMapPlugin gp = ext as IMapPlugin;
            if (gp != null)
            {
                IMapPluginArgs args = new GeoPluginArgs(_map, _legend, _mainMenu, _toolStrip, _progressHandler, _geoPlugins, _toolStripContainer, _panelManager);
                gp.Initialize(args);
                _geoPlugins.Add(gp);
            }

            ext.Activate();     
          
        }

        /// <summary>
        /// Deactivates the current token, removing the plugin from the active plugins list.
        /// This isn't quite the same as unloading the plugin.
        /// </summary>
        /// <param name="token"></param>
        public void DeactivateToken(PluginToken token)
        {
            if (_activeTokens.ContainsKey(token))
            {
                IExtension ext = _activeTokens[token];
                ext.Deactivate();
                IMapPlugin pg = ext as IMapPlugin;
                if (pg != null)
                {
                    _geoPlugins.Remove(pg);
                }
            }
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
        /// Gets or sets the control that is the ToolManager
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
            set { _toolManager = value; }
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
        /// Gets or sets the main panel manager, if any, associated with the
        /// plugin manager
        /// </summary>
        public PanelManager MainPanelManager
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
        /// Gets the list of plugins that are currently active
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
        /// Gets or sets the DataManager for the application
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter)),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataManager DataManager
        {
            get
            {
                if(!_dataManager.DefaultDirectoryAdded && !DesignMode)_dataManager.LoadDefaultDirectories();
                return _dataManager;
            }
        }

       

        private IContainer components;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
