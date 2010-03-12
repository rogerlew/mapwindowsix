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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/18/2010 3:45:51 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Data
{


    /// <summary>
    /// ColorType
    /// </summary>
    public enum ColorTypes :byte
    {
        /// <summary>
        /// Each pixel is a greyscale sample
        /// </summary>
        Greyscale = 0,
        /// <summary>
        /// Each pixel is an RGB triple
        /// </summary>
        Truecolor = 2,
        /// <summary>
        /// Each pixel is a palette index
        /// </summary>
        Indexed = 3,
        /// <summary>
        /// Each pixel is a greyscale sample followed by an alpha sample
        /// </summary>
        GreyscaleAlpha = 4,
        /// <summary>
        /// EAch pixel is an RGB triple followed by an alhpa sample
        /// </summary>
        TruecolorAlpha = 6


    }
}
