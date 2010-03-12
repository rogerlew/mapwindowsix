//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;
using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.Operation.Distance
{
    /// <summary>
    /// Represents the location of a point on a Geometry.
    /// Maintains both the actual point location (which of course
    /// may not be exact) as well as information about the component
    /// and segment index where the point occurs.
    /// Locations inside area Geometrys will not have an associated segment index,
    /// so in this case the segment index will have the sentinel value of InsideArea.
    /// </summary>
    public class GeometryLocation
    {
        /// <summary>
        /// Special value of segment-index for locations inside area geometries. These
        /// locations do not have an associated segment index.
        /// </summary>
        public const int InsideArea = -1;

        private IGeometry component = null;
        private int segIndex;
        private Coordinate pt = null;

        /// <summary>
        /// Constructs a GeometryLocation specifying a point on a point, as well as the 
        /// segment that the point is on (or InsideArea if the point is not on a segment).
        /// </summary>
        /// <param name="component"></param>
        /// <param name="segIndex"></param>
        /// <param name="pt"></param>
        public GeometryLocation(IGeometry component, int segIndex, Coordinate pt)
        {
            this.component = component;
            this.segIndex = segIndex;
            this.pt = new Coordinate(pt);
        }

        /// <summary> 
        /// Constructs a GeometryLocation specifying a point inside an area point.
        /// </summary>
        public GeometryLocation(IGeometry component, Coordinate pt) : this(component, InsideArea, pt) { }

        /// <summary>
        /// Returns the point associated with this location.
        /// </summary>
        public virtual IGeometry GeometryComponent
        {
            get
            {
                return component;
            }
        }

        /// <summary>
        /// Returns the segment index for this location. If the location is inside an
        /// area, the index will have the value InsideArea;
        /// </summary>
        public virtual int SegmentIndex
        {
            get
            {
                return segIndex;
            }
        }

        /// <summary>
        /// Returns the location.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return pt;
            }
        }

        /// <summary>
        /// Returns whether this GeometryLocation represents a point inside an area point.
        /// </summary>
        public virtual bool IsInsideArea
        {
            get
            {
                return segIndex == InsideArea;
            }
        }
    }
}
