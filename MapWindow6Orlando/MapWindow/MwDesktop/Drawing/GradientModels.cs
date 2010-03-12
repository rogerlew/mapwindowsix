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
// The Initial Developer of this Original Code is Ted Dunsford. 2/17/2008 12:28:12 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MapWindow.Drawing
{
    /// <summary>
    /// An enumeration specifying the way that a gradient of color is attributed to the values in the specified range.
    /// </summary>
    public enum GradientModels
    {
        /// <summary>
        /// The values are colored in even steps in each of the Red, Green and Blue bands.
        /// </summary>
        Linear,
        /// <summary>
        /// The even steps between values are used as powers of two, greatly increasing the impact of higher values.
        /// </summary>
        Exponential,

        /// <summary>
        /// The log of the values is used, reducing the relative impact of the higher values in the range.
        /// </summary>
        Logarithmic
    }


}
