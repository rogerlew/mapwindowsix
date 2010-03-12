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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/6/2008 9:06:14 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Algorithm
{


    /// <summary>
    /// IntersectionTypes
    /// </summary>
    public enum IntersectionTypes
    {
        /// <summary>
        /// No intersection occurs 
        /// </summary>
        NoIntersection = 0,
        
        /// <summary>
        /// The lines intersect in a single point
        /// </summary>
        PointIntersection = 1,

        /// <summary>
        /// The lines intersect by overlapping
        /// </summary>
        Collinear = 2,
        



    }
}
