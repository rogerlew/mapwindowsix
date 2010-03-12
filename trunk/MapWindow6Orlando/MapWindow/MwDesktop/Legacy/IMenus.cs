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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:44:15 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;

namespace MapWindow.Legacy
{


    /// <summary>
    /// Used for manipulation of the menu system for the application
    /// </summary>
    public interface IMenus
    {
        /// <summary>
        /// Adds a menu with the specified name
        /// </summary>
        IMenuItem AddMenu(string Name);
        /// <summary>
        /// Adds a menu with the specified name and icon
        /// </summary>
        IMenuItem AddMenu(string Name, Image Picture);

        /// <summary>
        /// Adds a menu with the specified name, icon and text
        /// </summary>
        IMenuItem AddMenu(string Name, Image Picture, string Text);

        /// <summary>
        /// Adds a menu with the specified name to the menu indicated by ParentMenu
        /// </summary>
        IMenuItem AddMenu(string Name, string ParentMenu);

        /// <summary>
        /// Adds a menu with the specified name and icon to the menu indicated by ParentMenu
        /// </summary>
        IMenuItem AddMenu(string Name, string ParentMenu, Image Picture);
        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu
        /// </summary>
        IMenuItem AddMenu(string Name, string ParentMenu, Image Picture, string Text);

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu and after the specifed item
        /// </summary>
        IMenuItem AddMenu(string Name, string ParentMenu, Image Picture, string Text, string After);

        /// <summary>
        /// Adds a menu with the specified name and text to the specified ParentMenu and before the specifed item
        /// </summary>
        IMenuItem AddMenu(string Name, string ParentMenu, string Text, string Before);

        /// <summary>
        /// Gets a MenuItem by its name
        /// </summary>
        IMenuItem this[string MenuName] { get; }

        /// <summary>
        /// Removes a MenuItem
        /// </summary>
        /// <param name="Name">Name of the item to remove</param>
        /// <returns>true on success, false otherwise</returns>
        bool Remove(string Name);
    }
}
