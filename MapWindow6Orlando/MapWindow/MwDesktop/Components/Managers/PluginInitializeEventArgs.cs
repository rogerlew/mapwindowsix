using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MapWindow.Components;
using System.Collections;
namespace MapWindow.Components
{
    /// <summary>
    /// Event arguments for a plugin initialization
    /// </summary>
    public class PluginInitializeEventArgs : EventArgs, IPluginManager
    {
        #region Private Variables

        private IBasicMap _map;
        private PreviewMap.IPreviewMap _previewMap;
        private ILegend _legend;
        private MenuStrip _menuStrip;
        private mwStatusStrip _mwStatusStrip;
        private bool _pluginMenuIsVisible;
        private ToolStripContainer _toolStripContainer;
        private PanelManager _panelManager;

        #endregion

        #region Constructors

      

        #endregion




        /// <summary>
        /// The Legend (if any) associated with the plugin manager
        /// </summary>
        public ILegend Legend
        {
            get
            {
                return _legend;
            }
            protected set
            {
                _legend = value;
            }
        }

        /// <summary>
        /// The main map associated with this plugin manager
        /// </summary>
        public IBasicMap Map
        {
            get
            {
                return _map;
            }
            protected set
            {
                _map = value;
            }
        }

        /// <summary>
        /// The Menu Strip (if any) associated with the plugin manager
        /// </summary>
        public MenuStrip MenuStrip
        {
            get
            {
                return _menuStrip;
            }
            protected set
            {
                _menuStrip = value;
            }
        }

        /// <summary>
        /// The Preview Map (if any) associated with the plugin manager
        /// </summary>
        public PreviewMap.IPreviewMap PreviewMap
        {
            get
            {
                return _previewMap;
            }
            protected set
            {
                _previewMap = value;
            }
        }

        /// <summary>
        /// Gets or sets whether or not the plugin Menu item is visible
        /// </summary>
        public bool PluginMenuIsVisible
        {
            get { return _pluginMenuIsVisible; }
            protected set { _pluginMenuIsVisible = value; }
        }

        /// <summary>
        /// The StatusBar (if any) associated with the plugin manager
        /// </summary>
        public mwStatusStrip StatusBar
        {
            get { return _mwStatusStrip; }
            protected set
            {
                _mwStatusStrip = value;
            }
        }
      

        /// <summary>
        /// The ToolStrip (if any) associated with the plugin manager
        /// </summary>
        public ToolStripContainer ToolStripContainer
        {
            get
            {
                return _toolStripContainer;
            }
            protected set
            {
                _toolStripContainer = value;
            }
        }

        /// <summary>
        /// The ToolStrip (if any) associated with the plugin manager
        /// </summary>
        public PanelManager PanelManager
        {
            get
            {
                return _panelManager;
            }
            protected set
            {
                _panelManager = value;
            }
        }





        
    }
}
