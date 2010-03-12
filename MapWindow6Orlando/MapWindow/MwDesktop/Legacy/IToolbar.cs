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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:54:30 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Legacy
{

    /// <summary>
    /// Used to add/remove Toolbars, buttons, combo boxes, etc. to/from the application
    /// </summary>
    public interface IToolbar
    {
        /// <summary>
        /// Adds a Toolbar group to the Main Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the Toolbar item</param>
        /// <returns>true on success, false on failure</returns>
        bool AddToolbar(string Name);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ToolbarButton</param>
        IToolbarButton AddButton(string Name);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ToolbarButton</param>
        /// <param name="IsDropDown">Should this button support drop-down items?</param>
        IToolbarButton AddButton(string Name, bool IsDropDown);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ToolbarButton</param>
        /// /// <param name="Toolbar">The name of the Toolbar to which this ToolbarButton should belong (if null or empty, then the default Toolbar will be used</param>
        /// <param name="IsDropDown">Should this button support drop-down items?</param>
        IToolbarButton AddButton(string Name, string Toolbar, bool IsDropDown);

        /// <summary>
        /// Adds a separator to a toolstrip dropdown button.
        /// </summary>
        /// <param name="Name">The name to give to the new separator.</param>
        /// <param name="ParentButton">The name of the ToolbarButton to which this new separator should be added as a subitem</param>
        /// <param name="Toolbar">The name of the Toolbar to which this separator should belong (if null or empty, then the default Toolbar will be used</param>
        void AddButtonDropDownSeparator(string Name, string Toolbar, string ParentButton);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ToolbarButton</param>
        /// <param name="Picture">The Icon/Bitmap to use as a picture on the ToolbarButton face</param>
        IToolbarButton AddButton(string Name, object Picture);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ToolbarButton</param>
        /// <param name="Picture">The Icon/Bitmap to use as a picture on the ToolbarButton face</param>
        /// <param name="Text">The text name for the ToolbarButton.  This is the text the user will see if customizing the Toolbar</param>
        IToolbarButton AddButton(string Name, object Picture, string Text);

        /// <summary>
        /// Adds a button to a specified to the specified Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ToolbarButton</param>
        /// <param name="After">The name of the ToolbarButton after which this new ToolbarButton should be added</param>
        /// <param name="ParentButton">The name of the ToolbarButton to which this new ToolbarButton should be added as a subitem</param>
        /// <param name="Toolbar">The name of the Toolbar to which this ToolbarButton should belong (if null or empty, then the default Toolbar will be used</param>
        IToolbarButton AddButton(string Name, string Toolbar, string ParentButton, string After);

        /// <summary>
        /// Adds a ComboBoxItem to a specified to the default Toolbar
        /// </summary>
        /// <param name="Name">The name to give to the new ComboBoxItem</param>
        /// <param name="After">The name of the ToolbarButton/ComboBoxItem afterwhich this new item should be added</param>
        /// <param name="Toolbar">The name of the Toolbar to which this ToolbarButton should belong</param>
        IComboBoxItem AddComboBox(string Name, string Toolbar, string After);

        /// <summary>
        /// returns the specified ToolbarButton (null on failure)
        /// </summary>
        /// <param name="Name">The name of the ToolbarButton to retrieve</param>
        IToolbarButton ButtonItem(string Name);

        /// <summary>
        /// returns the specified ComboBoxItem
        /// </summary>
        /// <param name="Name">Name of the item to retrieve</param>
        IComboBoxItem ComboBoxItem(string Name);

        /// <summary>
        /// Returns the number of buttons on the specified toolbar, or 0 if the toolbar can't be found.
        /// </summary>
        /// <param name="ToolbarName">The name of the toolbar.</param>
        /// <returns>The number of buttons on the toolbar.</returns>
        int NumToolbarButtons(string ToolbarName);

        /// <summary>
        /// Presses the specified ToolBar button (if it's enabled) as if a user
        /// had pressed it.
        /// </summary>
        /// <param name="Name">The name of the toolbar button to press.</param>
        /// <returns>true on success, false on failure (i.e. bad toolbar button name)</returns>
        bool PressToolbarButton(string Name);

        /// <summary>
        /// Removes the specified Toolbar and any ToolbarButton/ComboBoxItems contained within the control
        /// </summary>
        /// <param name="Name">The name of the Toolbar to be removed</param>
        /// <returns>true on success, false on failure</returns>
        bool RemoveToolbar(string Name);
        /// <summary>
        /// Removes the specified ToolbarButton item
        /// </summary>
        /// <param name="Name">The name of the ToolbarButton to be removed</param>
        /// <returns>true on success, false on failure</returns>
        bool RemoveButton(string Name);

        /// <summary>
        /// Removes the specified ComboBoxItem
        /// </summary>
        /// <param name="Name">The name of the ComboBoxItem to be removed</param>
        /// <returns>true on success, false on failure</returns>
        bool RemoveComboBox(string Name);

       
        

    }
}
