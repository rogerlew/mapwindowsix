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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 8:21:20 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Data
{

    /// <summary>
    /// Data types specific to grids
    /// </summary>
    public enum RasterDataTypes
    {
        /// <summary>
        /// An invalid data type
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// Short 16 Bit integers
        /// </summary>
        SHORT = 0,

        /// <summary>
        /// 32 Bit Integers (old style long)
        /// </summary>
        INTEGER = 1,

        /// <summary>
        /// Float or Single
        /// </summary>
        SINGLE = 2,

        /// <summary>
        /// Double
        /// </summary>
        DOUBLE = 3,

        /// <summary>
        /// Unknown
        /// </summary>
        UNKNOWN = 4,

        /// <summary>
        /// Byte
        /// </summary>
        BYTE = 5,

        /// <summary>
        /// Specified as the CustomType string
        /// </summary>
        CUSTOM = 6,
    }
    
}
