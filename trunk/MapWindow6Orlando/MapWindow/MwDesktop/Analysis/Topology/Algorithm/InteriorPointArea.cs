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

using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.Algorithm
{
    /// <summary> 
    /// Computes a point in the interior of an area point.
    /// Algorithm:
    /// Find the intersections between the point
    /// and the horizontal bisector of the area's envelope
    /// Pick the midpoint of the largest intersection (the intersections
    /// will be lines and points)
    /// Note: If a fixed precision model is used,
    /// in some cases this method may return a point
    /// which does not lie in the interior.
    /// </summary>
    public class InteriorPointArea
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double Avg(double a, double b)
        {
            return (a + b) / 2.0;
        }

        private readonly IGeometryFactory _factory;
        private Coordinate _interiorPoint = Coordinate.Empty;
        private double _maxWidth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public InteriorPointArea(IGeometry g)
        {
            _factory = g.Factory;
            Add(g);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate InteriorPoint
        {
            get
            {
                return _interiorPoint;
            }
        }

        /// <summary> 
        /// Tests the interior vertices (if any)
        /// defined by a linear Geometry for the best inside point.
        /// If a Geometry is not of dimension 1 it is not tested.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        private void Add(IGeometry geom)
        {
            if (geom is IPolygon) 
                AddPolygon(geom);            
            else if (geom is IGeometryCollection) 
            {
                IGeometryCollection gc = (IGeometryCollection)geom;
                foreach (IGeometry geometry in gc.Geometries)
                    Add(geometry);
            }
        }

        /// <summary> 
        /// Finds a reasonable point at which to label a Geometry.
        /// </summary>
        /// <param name="geometry">The point to analyze.</param>
        /// <returns> 
        /// The midpoint of the largest intersection between the point and
        /// a line halfway down its envelope.
        /// </returns>
        public virtual void AddPolygon(IGeometry geometry)
        {
            ILineString bisector = HorizontalBisector(geometry);

            IGeometry intersections = bisector.Intersection(geometry);
            IGeometry widestIntersection = WidestGeometry(intersections);

            double width = widestIntersection.EnvelopeInternal.Width;
            if (_interiorPoint.IsEmpty() || width > _maxWidth)
            {
                _interiorPoint = Centre(widestIntersection.EnvelopeInternal);
                _maxWidth = width;
            }
        }
           
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns>
        /// If point is a collection, the widest sub-point; otherwise,
        /// the point itself.
        /// </returns>
        protected virtual IGeometry WidestGeometry(IGeometry geometry) 
        {
            if (!(geometry is GeometryCollection)) 
                return geometry;        
            return WidestGeometry((GeometryCollection)geometry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gc"></param>
        /// <returns></returns>
        private static IGeometry WidestGeometry(IGeometryCollection gc)
        {
            if (gc.IsEmpty) 
                return gc;
            
            IGeometry widestGeometry = gc.GetGeometryN(0);
            for (int i = 1; i < gc.NumGeometries; i++) //Start at 1        
                if (gc.GetGeometryN(i).EnvelopeInternal.Width > widestGeometry.EnvelopeInternal.Width)                
                    widestGeometry = gc.GetGeometryN(i);                            
            return widestGeometry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual ILineString HorizontalBisector(IGeometry geometry)
        {
            IEnvelope envelope = geometry.EnvelopeInternal;

            // Assert: for areas, minx <> maxx
            double avgY = Avg(envelope.Minimum.Y, envelope.Maximum.Y);
            return _factory.CreateLineString(new[] { new Coordinate(envelope.Minimum.X, avgY), new Coordinate(envelope.Maximum.X, avgY) });
        }

        /// <summary> 
        /// Returns the centre point of the envelope.
        /// </summary>
        /// <param name="envelope">The envelope to analyze.</param>
        /// <returns> The centre of the envelope.</returns>
        public virtual Coordinate Centre(IEnvelope envelope)
        {
            return new Coordinate(Avg(envelope.Minimum.X, envelope.Maximum.X), Avg(envelope.Minimum.Y, envelope.Maximum.Y));
        }
    }
}
