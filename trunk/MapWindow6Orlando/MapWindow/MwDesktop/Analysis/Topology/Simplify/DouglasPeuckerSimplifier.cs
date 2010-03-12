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
using MapWindow.Geometries.Utilities;

namespace MapWindow.Analysis.Topology.Simplify
{
    /// <summary>
    /// Simplifies a <c>Geometry</c> using the standard Douglas-Peucker algorithm.
    /// Ensures that any polygonal geometries returned are valid.
    /// Simple lines are not guaranteed to remain simple after simplification.
    /// Notice that in general D-P does not preserve topology -
    /// e.g. polygons can be split, collapse to lines or disappear
    /// holes can be created or disappear,
    /// and lines can cross.
    /// To simplify point while preserving topology use TopologySafeSimplifier.
    /// (However, using D-P is significantly faster).
    /// </summary>
    public class DouglasPeuckerSimplifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="distanceTolerance"></param>
        /// <returns></returns>
        public static IGeometry Simplify(Geometry geom, double distanceTolerance)
        {
            DouglasPeuckerSimplifier tss = new DouglasPeuckerSimplifier(geom);
            tss.DistanceTolerance = distanceTolerance;
            return tss.GetResultGeometry();
        }

        private readonly Geometry _inputGeom;
        private double _distanceTolerance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputGeom"></param>
        public DouglasPeuckerSimplifier(Geometry inputGeom)
        {
            _inputGeom = inputGeom;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double DistanceTolerance
        {
            get
            {
                return _distanceTolerance; 
            }
            set
            {
                _distanceTolerance = value; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IGeometry GetResultGeometry()
        {
            return (new DPTransformer(this)).Transform(_inputGeom);
        }

        /// <summary>
        /// 
        /// </summary>
        private class DPTransformer : GeometryTransformer
        {
            private readonly DouglasPeuckerSimplifier _container;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="container"></param>
            public DPTransformer(DouglasPeuckerSimplifier container)
            {
                _container = container;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="coords"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override IList<Coordinate> TransformCoordinates(IList<Coordinate> coords, IGeometry parent)
            {
                return DouglasPeuckerLineSimplifier.Simplify(coords, _container.DistanceTolerance);

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="geom"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override IGeometry TransformPolygon(IPolygon geom, IGeometry parent)
            {
                IGeometry roughGeom = base.TransformPolygon(geom, parent);
                // don't try and correct if the parent is going to do this
                if (parent is MultiPolygon) 
                    return roughGeom;            
                return CreateValidArea(roughGeom);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="geom"></param>
            /// <returns></returns>
            protected override IGeometry TransformMultiPolygon(IMultiPolygon geom)
            {
                IGeometry roughGeom = base.TransformMultiPolygon(geom);
                return CreateValidArea(roughGeom);
            }

            /// <summary>
            /// Creates a valid area point from one that possibly has
            /// bad topology (i.e. self-intersections).
            /// Since buffer can handle invalid topology, but always returns
            /// valid point, constructing a 0-width buffer "corrects" the
            /// topology.
            /// Notice this only works for area geometries, since buffer always returns
            /// areas.  This also may return empty geometries, if the input
            /// has no actual area.
            /// </summary>
            /// <param name="roughAreaGeom">An area point possibly containing self-intersections.</param>
            /// <returns>A valid area point.</returns>
            private static IGeometry CreateValidArea(IGeometry roughAreaGeom)
            {
                return roughAreaGeom.Buffer(0.0);
            }
        }
    }
}
