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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:42:09 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Legacy
{

    /// <summary>
    /// Docking styles for MapWindow UIPanels
    /// </summary>
    public enum MapWindowDockStyles
    {
        /// <summary>Floating</summary>
        None = -1,
        /// <summary>Dock Left</summary>
        Left = 0,
        /// <summary>Dock Right</summary>
        Right = 1,
        /// <summary>Dock Top</summary>
        Top = 2,
        /// <summary>Dock Bottom</summary>
        Bottom = 3,
        /// <summary>Dock Left Autohidden</summary>
        LeftAutoHide = 4,
        /// <summary>Dock Right Autohidden</summary>
        RightAutoHide = 5,
        /// <summary>Dock Top Autohidden</summary>
        TopAutoHide = 6,
        /// <summary>Dock Bottom Autohidden</summary>
        BottomAutoHide = 7
    }

}
