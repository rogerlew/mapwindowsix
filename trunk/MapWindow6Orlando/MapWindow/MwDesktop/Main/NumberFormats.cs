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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/3/2008 5:15:54 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Main
{

    // We don't support hex or round-trip
    /// <summary>
    /// NumberFormats
    /// </summary>
    public enum NumberFormats
    {
        /// <summary>
        /// Currency - C
        /// </summary>
        Currency,
        
        /// <summary>
        /// Scientific Notation Exponential - E
        /// </summary>
        Exponential,

        /// <summary>
        /// Fixed point - F
        /// The number is converted to a string of the form "-ddd.ddd…" where each 'd'
        /// indicates a digit (0-9). The string starts with a minus sign if the number
        /// is negative.
        /// </summary>
        FixedPoint,

        /// <summary>
        /// Shortest text - G
        /// </summary>
        General,

        /// <summary>
        /// Number - N, The number is converted to a string of the form "-d,ddd,ddd.ddd…", 
        /// where '-' indicates a negative number symbol if required, 'd' indicates a digit
        /// (0-9), ',' indicates a thousand separator between number groups, and '.' indicates
        /// a decimal point symbol
        /// </summary>
        Number,

        /// <summary>
        /// Percent, value is multiplied by 100 and shown with a % symbol (cultural specific)
        /// </summary>
        Percent,

        /// <summary>
        /// No format specified.
        /// </summary>
        Unspecified,
    }
}
