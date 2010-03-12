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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2008 2:23:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{


    /// <summary>
    /// This describes the three permitted states for legend items.
    /// Items can have a checkbox or a symbol, but not both.
    /// </summary>
    public enum SymbolModes
    {
        /// <summary>
        /// Display a checkbox next to the legend item
        /// </summary>
        Checkbox,

        /// <summary>
        /// Draws a symbol, but also allows collapsing.
        /// </summary>
        GroupSymbol,

        /// <summary>
        /// Display a symbol next to the legend item
        /// </summary>
        Symbol,

        

        /// <summary>
        /// Display only legend text
        /// </summary>
        None

    }
}
