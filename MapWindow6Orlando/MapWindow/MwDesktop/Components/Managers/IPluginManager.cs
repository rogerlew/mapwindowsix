using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace MapWindow.Components
{

    /// <summary>
    /// This is a read-only style interface that gives a plugin developer access
    /// to the major components.
    /// </summary>
    public interface IPluginManager
    {
        


        /// <summary>
        /// The Legend (if any) associated with the plugin manager
        /// </summary>
        ILegend Legend
        {
            get;
        }

        /// <summary>
        /// The main map associated with the plugin manager
        /// </summary>
        IBasicMap Map
        {
            get;

        }



        /// <summary>
        /// The Menu Strip (if any) associated with the plugin manager
        /// </summary>
        MenuStrip MenuStrip
        {
            get;

        }

        /// <summary>
        /// Controls whether or not a Plugin menu will be added to the MapMenuStrip
        /// specified by this Plugin Manager
        /// </summary>
        bool PluginMenuIsVisible
        {
            get;
        }

        /// <summary>
        /// The Preview Map (if any) associated with the plugin manager
        /// </summary>
        PreviewMap.IPreviewMap PreviewMap
        {
            get;

        }

        /// <summary>
        /// The StatusBar (if any) associated with the plugin manager
        /// </summary>
        mwStatusStrip StatusBar
        {
            get;

        }
      

        /// <summary>
        /// The ToolStrip (if any) associated with the plugin manager
        /// </summary>
        ToolStripContainer ToolStripContainer
        {
            get;

        }

        /// <summary>
        /// The PanelManager (if any) associated with the plugin manager
        /// The panel manager is used to add tabs and panels to the main form
        /// </summary>
        PanelManager PanelManager
        {
            get;
        }

       

    }
}
