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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 5:50:21 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapWindow;
using MapWindow.Components;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
namespace MapWindow.Map
{


    /// <summary>
    /// IGeoPluginArgs
    /// </summary>
    public interface IMapPluginArgs 
    {
       
        #region Properties

        /// <summary>
        /// Gets the Map associated with the plugin manager
        /// </summary>
        IMap Map
        {
            get;
        }

        /// <summary>
        /// Gets the Legend (Table of Contents) associated with the plugin manager
        /// </summary>
        ILegend Legend
        {
            get;
        }

        /// <summary>
        /// Gets the Main Menu, if any, associated with the plugin manager
        /// </summary>
        MenuStrip MainMenu
        {
            get;
        }

        /// <summary>
        /// Gets the ToolStrip if any associated with the plugin manager,
        /// if any.
        /// </summary>
        ToolStrip MainToolStrip
        {
            get;
        }

        /// <summary>
        /// Gets the progress handler that is being used to display status messages.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
        }

        /// <summary>
        /// Gets the list of plugins
        /// </summary>
        List<IMapPlugin> Plugins
        {
            get;
        }

        /// <summary>
        /// Gets the actual container for the tool strips
        /// </summary>
        ToolStripContainer ToolStripContainer
        {
            get;
        }

        /// <summary>
        /// Gets the actual panel manager for adding tabs and panels
        /// </summary>
        PanelManager PanelManager
        {
            get;
        }


        #endregion



    }
}
