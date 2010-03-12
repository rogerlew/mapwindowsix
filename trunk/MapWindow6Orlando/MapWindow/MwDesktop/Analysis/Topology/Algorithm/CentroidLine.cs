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

using System.Collections.Generic;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Algorithm
{
    /// <summary> 
    /// Computes the centroid of a linear point.
    /// Algorithm:
    /// Compute the average of the midpoints
    /// of all line segments weighted by the segment length.
    /// </summary>
    public class CentroidLine
    {
        private readonly Coordinate _centSum = new Coordinate(0D, 0D, 0D, 0D);
        private double _totalLength;



        /// <summary> 
        /// Adds the linestring(s) defined by a Geometry to the centroid total.
        /// If the point is not linear it does not contribute to the centroid.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        public virtual void Add(IGeometry geom)
        {
            if(geom is LineString)             
                Add(geom.Coordinates);            

            else if (geom is GeometryCollection) 
            {
                GeometryCollection gc = (GeometryCollection)geom;
                foreach (Geometry geometry in gc.Geometries)
                    Add(geometry);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate Centroid
        {
            get
            {
                Coordinate cent = new Coordinate();
                cent.X = _centSum.X / _totalLength;
                cent.Y = _centSum.Y / _totalLength;
                return cent;
            }
        }

        /// <summary> 
        /// Adds the length defined by an array of coordinates.
        /// </summary>
        /// <param name="pts">An array of <c>Coordinates</c>.</param>
        public virtual void Add(IList<Coordinate> pts)
        {
            for (int i = 0; i < pts.Count - 1; i++)
            {

                double segmentLen = pts[i].Distance(pts[i + 1]);
                _totalLength += segmentLen;

                double midx = (pts[i].X + pts[i + 1].X) / 2;
                _centSum.X += segmentLen * midx;
                double midy = (pts[i].Y + pts[i + 1].Y) / 2;
                _centSum.Y += segmentLen * midy;
            }
        }
    }
}
