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

using System.Collections;
using System.Collections.Generic;
using MapWindow.GeometriesGraph;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Algorithm
{
    /// <summary> 
    /// Computes the topological relationship (Location) of a single point to a Geometry.
    /// The algorithm obeys the SFS boundaryDetermination rule to correctly determine
    /// whether the point lies on the boundary or not.
    /// Notice that instances of this class are not reentrant.
    /// </summary>
    public class PointLocator
    {
        private bool isIn;            // true if the point lies in or on any Geometry element
        private int numBoundaries;    // the number of sub-elements whose boundaries the point lies in

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLocator"/> class.
        /// </summary>
        public PointLocator() { }

        /// <summary> 
        /// Convenience method to test a point for intersection with a Geometry
        /// </summary>
        /// <param name="p">The coordinate to test.</param>
        /// <param name="geom">The Geometry to test.</param>
        /// <returns><c>true</c> if the point is in the interior or boundary of the Geometry.</returns>
        public virtual bool Intersects(Coordinate p, IGeometry geom)
        {
            return Locate(p, geom) != Locations.Exterior;
        }

        /// <summary> 
        /// Computes the topological relationship ({Location}) of a single point to a Geometry.
        /// It handles both single-element and multi-element Geometries.
        /// The algorithm for multi-part Geometries takes into account the boundaryDetermination rule.
        /// </summary>
        /// <returns>The Location of the point relative to the input Geometry.</returns>
        public virtual Locations Locate(Coordinate p, IGeometry geom)
        {
            if(geom.IsEmpty)
                return Locations.Exterior;
            if(geom is ILineString) 
                return LocateInLineString(p, (ILineString)geom);                        
            else if (geom is IPolygon) 
                return LocateInPolygon(p, (IPolygon)geom);
        
            isIn = false;
            numBoundaries = 0;
            ComputeLocation(p, geom);
            if(GeometryGraph.IsInBoundary(numBoundaries))
                return Locations.Boundary;
            if(numBoundaries > 0 || isIn)
                return Locations.Interior;
            return Locations.Exterior;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="geom"></param>
        private void ComputeLocation(Coordinate p, IGeometry geom)
        {
            if(geom is ILineString) 
                UpdateLocationInfo(Locate(p, (ILineString)geom));                                  
            else if(geom is IPolygon) 
                UpdateLocationInfo(Locate(p, (IPolygon)geom));            
            else if(geom is IMultiLineString) 
            {
                IMultiLineString ml = (IMultiLineString)geom;
                foreach(ILineString l in ml.Geometries)                     
                    UpdateLocationInfo(Locate(p, l));                
            }
            else if(geom is IMultiPolygon)
            {
                IMultiPolygon mpoly = (IMultiPolygon)geom;
                foreach(IPolygon poly in mpoly.Geometries) 
                    UpdateLocationInfo(Locate(p, poly));
            }
            else if (geom is IGeometryCollection) 
            {
                IEnumerator geomi = new GeometryCollectionEnumerator((IGeometryCollection)geom);
                while(geomi.MoveNext()) 
                {
                    IGeometry g2 = (IGeometry)geomi.Current;
                    if (g2 != geom)
                        ComputeLocation(p, g2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        private void UpdateLocationInfo(Locations loc)
        {
            if(loc == Locations.Interior) 
                isIn = true;
            if(loc == Locations.Boundary) 
                numBoundaries++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private Locations LocateInLineString(Coordinate p, ILineString l)
        {
            IList<Coordinate> pt = l.Coordinates;
            if(!l.IsClosed)
                if(p.Equals(pt[0]) || p.Equals(pt[pt.Count - 1]))
                    return Locations.Boundary;                            
            if (CGAlgorithms.IsOnLine(p, pt))
                return Locations.Interior;
            return Locations.Exterior;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ring"></param>
        /// <returns></returns>
        private Locations LocateInPolygonRing(Coordinate p, IBasicGeometry ring)
        {
            // can this test be folded into IsPointInRing?
            if(CGAlgorithms.IsOnLine(p, ring.Coordinates))
                return Locations.Boundary;
            if(CGAlgorithms.IsPointInRing(p, ring.Coordinates))
                return Locations.Interior;
            return Locations.Exterior;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="poly"></param>
        /// <returns></returns>
        private Locations LocateInPolygon(Coordinate p, IPolygon poly)
        {
            if (poly.IsEmpty) 
                return Locations.Exterior;
            LinearRing shell = (LinearRing)poly.Shell;
            Locations shellLoc = LocateInPolygonRing(p, shell);
            if (shellLoc == Locations.Exterior) 
                return Locations.Exterior;
            if (shellLoc == Locations.Boundary) 
                return Locations.Boundary;
            // now test if the point lies in or on the holes
            foreach(LinearRing hole in poly.Holes)
            {
                Locations holeLoc = LocateInPolygonRing(p, hole);
                if(holeLoc == Locations.Interior) 
                    return Locations.Exterior;
                if(holeLoc == Locations.Boundary) 
                    return Locations.Boundary;
            }
            return Locations.Interior;
        }
    }
}
