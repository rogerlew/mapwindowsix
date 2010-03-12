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
// The Initial Developer of this Original Code is Brian Marchionni. Created 9/08/2009
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapWindow.Drawing
{
    /// <summary>
    /// Methods used in calculating the placement of a label
    /// </summary>
    public enum LabelMethod
    {
        /// <summary>
        /// Use the centroid of the feature
        /// </summary>
        Centroid,

        /// <summary>
        /// Use the center of the extents of the feature
        /// </summary>
        Center,

        /// <summary>
        /// Use the closest point to the centroid that is in the feature
        /// </summary>
        InteriorPoint,
    }

    /// <summary>
    /// Determins if all parts should be labeled or just the largest
    /// </summary>
    public enum LabelParts
    {
        /// <summary>
        /// Label all parts
        /// </summary>
        LabelAllParts,

        /// <summary>
        /// Only label the largest part
        /// </summary>
        LabelLargestPart
    }
}
