//********************************************************************************************************
// Product Name: MapWindow.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for MapWindow.Drawing.PredefinedSymbols version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/15/2009 9:56:44 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{
    
    /// <summary>
    /// The interface IPredefinedSymbolProvider defines methods to save and load predefined 
    /// line, point or polygon symbolizers from an xml file or from another source.
    /// </summary>
    public interface ICustomSymbolProvider
    {

        #region Methods

        /// <summary>
        /// Loads all available predefined feature symbolizers
        /// </summary>
        /// <param name="symbolType">The type of symbolizer to be loaded (point, line, polygon)</param>
        /// <returns>The list of all available symbolizers</returns>
        List<CustomSymbolizer> LoadAllSymbols(SymbolizerTypes symbolType);

        /// <summary>
        /// Loads all predefined symbolizers which belong to the specific group (map type name)
        /// </summary>
        /// <param name="groupName">The group name describes the style of map where the symbolizers are used</param>
        /// <param name="symbolType">The type of symbolizer to be loaded (point, line, polygon)</param>
        /// <returns>The list of available symbolizers</returns>
        List<CustomSymbolizer> LoadSymbolsByCategory(string groupName, SymbolizerTypes symbolType);

        #endregion

        #region Properties



        #endregion

    }
}
