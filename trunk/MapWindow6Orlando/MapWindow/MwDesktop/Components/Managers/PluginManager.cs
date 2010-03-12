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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 2:13:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MapWindow.Components
{
    /// <summary>
    /// PluginManager for dealing with additional plugins
    /// </summary>
    [ToolboxBitmap("PluginManager.ico")]
    public partial class PluginManager 
    {
        #region Private Variables
        IBasicMap _map;
        IBasicMap _previewMap;
        ILegend _legend;
        ToolStrip _mapToolStrip;
        MenuStrip _mapMenuStrip;
        mwStatusStrip _mwStatusStrip;
        PanelManager _panelManager;
        bool _pluginMenuIsVisible;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Plugin Manager
        /// </summary>
        public PluginManager()
        {
            
        }

       

        #endregion

        



        #region Properties

        /// <summary>
        /// Gets or sets the Legend associated with this plugin manager
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set { _legend = value; }
        }


        /// <summary>
        /// Gets or sets the Map associated with this plugin manager
        /// </summary>
        public IBasicMap Map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }

        /// <summary>
        /// Gets or sets the MapMenuStrip associated with this plugin manager
        /// </summary>
        public MenuStrip MapMenuStrip
        {
            get { return _mapMenuStrip; }
            set 
            {
                if (_pluginMenuIsVisible == true)
                {
                    AddPluginMenu();
                }
                if (_pluginMenuIsVisible == false)
                {
                    RemovePluginMenu();
                }
                _mapMenuStrip = value; 
            }
        }

        /// <summary>
        /// Gets or sets the MapToolStrip associated with this plugin manager
        /// </summary>
        public ToolStrip MapToolstrip
        {
            get { return _mapToolStrip; }
            set { _mapToolStrip = value; }
        }


        /// <summary>
        /// Gets or sets the Preview Map associated with this plugin manager
        /// </summary>
        public IBasicMap PreviewMap
        {
            get { return _previewMap; }
            set { _previewMap = value; }
        }

        /// <summary>
        /// Gets or sets the Status Strip
        /// </summary>
        public mwStatusStrip StatusStrip
        {
            get { return _mwStatusStrip; }
            set { _mwStatusStrip = value; }
        }

        /// <summary>
        /// Controls whether or not a Plugin menu will be added to the MapMenuStrip
        /// specified by this Plugin Manager
        /// </summary>
        public bool PluginMenuIsVisible
        {
            get { return _pluginMenuIsVisible; }
            set
            {
                if (_pluginMenuIsVisible == false)
                {
                    if (value == true)
                    {
                        AddPluginMenu();
                    }
                }
                else
                {
                    if (value == false)
                    {
                        RemovePluginMenu();
                    }
                }
                _pluginMenuIsVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the panel manager. The panel manager can
        /// be used to work with tabs and dockable panels
        /// </summary>
        public PanelManager PanelManager
        {
            get { return _panelManager; }
            set { _panelManager = value; }
        }


        #endregion


        #region Methods





        /// <summary>
        /// Checks the dlls in the Plugins folder or any subfolder and
        /// adds a new checked menu item for each one that it finds.
        /// This can also be controlled using the PluginMenuIsVisible property.
        /// </summary>
        public void AddPluginMenu()
        {
            if (_mapMenuStrip == null) return;
            if (_pluginMenuIsVisible == true) return;
            _pluginMenuIsVisible = true;
            //MenuItem mi = _mapMenuStrip.Items[MessageStrings.Plugins] as MenuItem;
            //if (mi == null)
            //{
            //    MenuItem mi = _mapMenuStrip.Items.Add(MessageStrings.Plugins) as MenuItem;
            //}
            //UpdateStore();
           
        }

        /// <summary>
        /// Looks for a menu named Plug-ins and removes it.
        /// Control this through the PluginMenuIsVisible property.
        /// This can also be controlled using the PluginMenuIsVisible property.
        /// </summary>
        public void RemovePluginMenu()
        {
            if (_mapMenuStrip == null) return;
            if (_pluginMenuIsVisible == false) return;
            // The Find method is not supported by Mono 2.0
            // ToolStripItem[] tsList = _mapMenuStrip.Items.Find(MessageStrings.Plugins, false);
            List<ToolStripItem> tsList = new List<ToolStripItem>();
            foreach (ToolStripItem item in _mapMenuStrip.Items)
            {
                if (item.Text == MessageStrings.Plugins)
                {
                    tsList.Add(item);
                }
            }
            foreach(ToolStripItem item in tsList)
            {
                _mapMenuStrip.Items.Remove(item);
            }
            _pluginMenuIsVisible = false;
        }

       

       

        #endregion

      
    }


}
