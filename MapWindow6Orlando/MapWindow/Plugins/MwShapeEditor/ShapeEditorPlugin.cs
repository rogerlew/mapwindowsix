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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 6:44:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using MapWindow.Map;
using MapWindow.Plugins;
using MapWindow.Components;
namespace MapWindow.ShapeEditor
{


    /// <summary>
    /// GeoShapeEditorPlugin
    /// </summary>
    [MapWindowPlugin("ShapeEditor", Author="MapWindow", UniqueName="mw_ShapeEditor_1", Version="1")]
    public class ShapeEditorPlugin : Extension, IMapPlugin
    {
        #region Private Variables

        private IMapPluginArgs _host; // caches useful information about the host application
        private ShapeEditorToolStrip _myToolStrip;


        #endregion

        #region Constructors

      
        #endregion
    

        /// <summary>
        /// The initialization is where this plugin can cache the necessary information about the MapWindow
        /// project.  This includes, for instance, the Map or Legend that is associated with the GeoPlugin
        /// Manager.  This can be useful for trapping events or accessing member values like the MapFrame
        /// or figuring out what layers are currently in the map, or even getting to the dataset for 
        /// each layer.
        /// </summary>
        /// <param name="args">The IGeoPluginArgs replaces the g_MapWin concept and has the necessary application info </param>
        public void Initialize(IMapPluginArgs args)
        {
            _host = args;
            // When this plugin is activated, a shape editor toolstrip appears with the shape editor functions.
            // Create a new list of toolstrips that can be provided to the host application
            _myToolStrip = new ShapeEditorToolStrip();
            _myToolStrip.Map = _host.Map;
            if (_host.ToolStripContainer != null)
            {
                _host.ToolStripContainer.TopToolStripPanel.Controls.Add(_myToolStrip);
            }
           
        
        }

       

        /// <summary>
        /// Fires when the plugin should become inactive
        /// </summary>
        protected override void OnDeactivate()
        {
            if(_myToolStrip != null)
            {
                _myToolStrip.Deactivate();
                _host.ToolStripContainer.TopToolStripPanel.Controls.Remove(_myToolStrip);
            }
            
        }

        
    }
}
