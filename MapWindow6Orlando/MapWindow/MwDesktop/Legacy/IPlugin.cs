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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using MapWindow.Components;
namespace MapWindow.Legacy
{
    /// <summary>
    /// During the Initialize method, the 
    /// </summary>
    public interface IPlugin 
    {
        #region Methods

        /// <summary>
        /// This method is called by the MapWindow when the plugin is loaded.
        /// </summary>
        /// <param name="MapWin">The <c>IMapWin</c> interface to use when interacting with the MapWindow.</param>
        /// <param name="ParentHandle">The windows handle of the MapWindow form.  This can be used to make the MapWindow the parent of any forms in the plugin.</param>
        void Initialize(IMapWin MapWin, int ParentHandle);

        ///// <summary>
        ///// Occurs when the plugin manager is being intialized.
        ///// </summary>
        ///// <param name="sender">The object reference of the caller</param>
        ///// <param name="e">Any EventArgs that are necessary</param>
        //void Initialize(object sender, PluginInitializeEventArgs e);

        /// <summary>
        /// This method is called by the MapWindow when a toolbar or menu item is clicked.
        /// </summary>
        /// <param name="ItemName">Name of the item that was clicked.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void ItemClicked(string ItemName, ref bool Handled);


        /// <summary>
        /// This method is called by the MapWindow when one or more layer(s) is/are added.
        /// </summary>
        /// <param name="Layers">An array of <c>Layer</c> objects that were added.</param>
        void LayersAdded(ILayerOld[] Layers);

        /// <summary>
        /// This method is called by the MapWindow when all layers are cleared from the map.
        /// </summary>
        void LayersCleared();

        /// <summary>
        /// This method is called by the MapWindow when a layer is removed from the map.
        /// </summary>
        /// <param name="Handle">Handle of the layer that was removed.</param>
        void LayerRemoved(int Handle);

        /// <summary>
        /// This method is called by the MapWindow when a layer is selected in code or by the legend.
        /// </summary>
        /// <param name="Handle">Handle of the newly selected layer.</param>
        void LayerSelected(int Handle);

        

        /// <summary>
        /// This method is called by the MapWindow when the user double-clicks on the legend.
        /// </summary>
        /// <param name="Handle">Handle of the layer or group that was double-clicked</param>
        /// <param name="Location">Location that was clicked.  Either a layer or a group.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void LegendDoubleClick(int Handle, ClickLocations Location, ref bool Handled);

        /// <summary>
        /// This method is called by the MapWindow when the user presses a mouse button on the legend.
        /// </summary>
        /// <param name="Handle">Handle of the layer or group that was under the cursor.</param>
        /// <param name="Button">The mouse button that was pressed.  You can use the <c>vb6Buttons</c> enumeration to determine which button was pressed.</param>
        /// <param name="Location">Location that was clicked.  Either a layer or a group</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void LegendMouseDown(int Handle, int Button, ClickLocations Location, ref bool Handled);

        /// <summary>
        /// This method is called by the MapWindow when the user releases a mouse button on the legend.
        /// </summary>
        /// <param name="Handle">Handle of the layer or group that was under the cursor.</param>
        /// <param name="Button">The mouse button that was released.  You can use the <c>vb6Buttons</c> enumeration to determine which button it was.</param>
        /// <param name="Location">Location that was clicked.  Either a layer or a group</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void LegendMouseUp(int Handle, int Button, ClickLocations Location, ref bool Handled);

        /// <summary>
        /// This method is called by the MapWindow when the extents of the map have changed.
        /// </summary>
        void MapExtentsChanged();

        /// <summary>
        /// This method is called by the MapWindow when the user presses a mouse button over the map.
        /// </summary>
        /// <param name="Button">The button that was pressed.  These values can be compared to the <c>vb6Butttons</c> enumerations.</param>
        /// <param name="Shift">This value represents the state of the <c>ctrl</c>, <c>alt</c> and <c>shift</c> keyboard buttons.  The values use VB6 values.</param>
        /// <param name="x">The x coordinate in pixels.</param>
        /// <param name="y">The y coordinate in pixels.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled);

        /// <summary>
        /// This method is called by the MapWindow when the user moves the mouse over the map display.
        /// </summary>
        /// <param name="ScreenX">The x coordinate in pixels.</param>
        /// <param name="ScreenY">The y coordinate in pixels.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled);

        /// <summary>
        /// This method is called by the MapWindow when the user releases a mouse button over the map.
        /// </summary>
        /// <param name="Button">The button that was released.  These values can be compared to the <c>vb6Butttons</c> enumerations.</param>
        /// <param name="Shift">This value represents the state of the <c>ctrl</c>, <c>alt</c> and <c>shift</c> keyboard buttons.  The values use VB6 values.</param>
        /// <param name="x">The x coordinate in pixels.</param>
        /// <param name="y">The y coordinate in pixels.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled);

        /// <summary>
        /// This method is called by the MapWindow when the user completes a dragging operation on the map.
        /// </summary>
        /// <param name="Bounds">The rectangle that was selected, in pixel coordinates.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled);

        

        /// <summary>
        /// This method is called by the MapWindow when a project is being loaded.
        /// </summary>
        /// <param name="ProjectFile">Filename of the project file.</param>
        /// <param name="SettingsString">Settings string for this plugin from the project file.</param>
        void ProjectLoading(string ProjectFile, string SettingsString);

        /// <summary>
        /// This method is called by the MapWindow when a project is being saved.
        /// </summary>
        /// <param name="ProjectFile">Filename of the project file.</param>
        /// <param name="SettingsString">Reference parameter.  Set this value in order to save your plugin's settings string in the project file.</param>
        void ProjectSaving(string ProjectFile, ref string SettingsString);



        /// <summary>
        /// This method is called by the MapWindow when the plugin is unloaded.
        /// </summary>
        void Terminate();

        /// <summary>
        /// This method is called by the MapWindow when shapes are selected by the user.
        /// </summary>
        /// <param name="Handle">Handle of the shapefile layer that was selected on.</param>
        /// <param name="SelectInfo">The <c>SelectInfo</c> object containing information about the selected shapes.</param>
        void ShapesSelected(int Handle, ISelectInfo SelectInfo);

       
        /// <summary>
        /// This message is relayed by the MapWindow when another plugin broadcasts a message.  Messages can be used to send messages between plugins.
        /// </summary>
        /// <param name="msg">The message being relayed.</param>
        /// <param name="Handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void Message(string msg, ref bool Handled);

        #endregion

        #region Properties

        /// <summary>
		/// Author of the plugin.
		/// </summary>
		string Author {get;}

		/// <summary>
		/// Short description of the plugin.
		/// </summary>
		string Description {get;}

		/// <summary>
		/// Plugin serial number.  NO LONGER NEEDED; kept for backward compatibility.
		/// </summary>
		string SerialNumber {get;}

		/// <summary>
		/// Name of the plugin.
		/// </summary>
		string Name {get;}

		/// <summary>
		/// Build date.
		/// </summary>
		string BuildDate {get;}

		/// <summary>
		/// Plugin version.
		/// </summary>
		string Version {get;}

			
		

       

        #endregion

    }
}
