//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

namespace MapWindow.Geometries
{
    /// <summary>
    /// This is a low-level place holder for a linestring with only two points.
    /// This does not inherit geometry (Use ILineString for those features).
    /// The Idea is that this provides just enough information to communicate
    /// the definition of a LineSegment.
    /// </summary>
    public interface ILineSegmentBase
    {
        /// <summary>
        /// The first of two coordinates that defines the segment
        /// </summary>
        Coordinate P1
        {
            get; set;
        }

        /// <summary>
        /// The second of two endpoints that defines the segment
        /// </summary>
        Coordinate P0
        {
            get; set;
        }
    }
}
