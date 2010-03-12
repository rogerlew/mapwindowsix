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
using MapWindow.Analysis.Topology.Operation;
using MapWindow.GeometriesGraph;
using System.Linq;


namespace MapWindow.Geometries
{
    /// <summary>
    /// Basic implementation of <c>MultiLineString</c>.
    /// </summary>    
    public class MultiLineString : GeometryCollection, IMultiLineString
    {
        /// <summary>
        /// Represents an empty <c>MultiLineString</c>.
        /// </summary>
        public static new readonly IMultiLineString Empty = new MultiLineString();

      
        /// <summary>
        /// Constructs a multiLineString from the list of IBasicLineStrings, creating new full geometries where necessary.
        /// </summary>
        /// <param name="lineStrings">An IBasicLineString that is either a full IGeometry itself, or will be cast into one by this step</param>
        public MultiLineString(IEnumerable<IBasicLineString> lineStrings) 
        {
            int count = lineStrings.Count();
            IGeometry[] g = new IGeometry[count];
            int index = 0;
            foreach (IBasicLineString basicLineString in lineStrings)
            {
                g[index] = FromBasicGeometry(basicLineString);
                index = index + 1;
            }
            base.Geometries = g;
        }

        /// <summary>
        /// Constructs a <c>MultiLineString</c>.
        /// </summary>
        /// <param name="lineStrings">
        /// The <c>LineString</c>s for this <c>MultiLineString</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>LineString</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public MultiLineString(IBasicLineString[] lineStrings, IGeometryFactory factory) 
            : base(lineStrings, factory)
        {
        }

        /// <summary>
        /// This will attempt to create a new MultiLineString from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Basic geometry that shoule be a LineString or MultiLineString</param>
        public MultiLineString(IBasicGeometry inBasicGeometry)
            : base(inBasicGeometry, DefaultFactory)
        {
        }

        /// <summary>
        /// This will attempt to create a new MultiLineString from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Basic geometry that shoule be a LineString or MultiLineString</param>
        /// <param name="inFactory">Any valid Geometry Factory</param>
        public MultiLineString(IBasicGeometry inBasicGeometry, IGeometryFactory inFactory)
            : base(inBasicGeometry, inFactory)
        {

        }

        /// <summary>
        /// Constructs a <c>MultiLineString</c>.
        /// </summary>
        /// <param name="lineStrings">
        /// The <c>LineString</c>s for this <c>MultiLineString</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>LineString</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public MultiLineString(IBasicLineString[] lineStrings) : this(lineStrings, DefaultFactory) { }

        /// <summary>
        /// Constructor for a MultiLineString that is empty
        /// </summary>
        public MultiLineString()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override Dimensions Dimension
        {
            get
            {
                return Dimensions.Curve;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override Dimensions BoundaryDimension
        {
            get
            {
                if (IsClosed)
                    return Dimensions.False;
                return Dimensions.Point;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override string GeometryType
        {
            get
            {
                return "MultiLineString";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public virtual bool IsClosed
        {
            get
            {
                if (IsEmpty) 
                    return false;
                for (int i = 0; i < Geometries.Length; i++)
                    if (!((LineString)Geometries[i]).IsClosed)
                        return false;                
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override bool IsSimple
        {
            get
            {
                return (new IsSimpleOp()).IsSimple(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override IGeometry Boundary
        {
            get
            {
                if(IsEmpty)
                    return Factory.CreateGeometryCollection(null);
                GeometryGraph g = new GeometryGraph(0, this);
                Coordinate[] pts = g.GetBoundaryPoints();
                return Factory.CreateMultiPoint(pts);
            }
        }

        /// <summary>
        /// Creates a <see cref="MultiLineString" /> in the reverse order to this object.
        /// Both the order of the component LineStrings
        /// and the order of their coordinate sequences are reversed.
        /// </summary>
        /// <returns>a <see cref="MultiLineString" /> in the reverse order.</returns>
        public virtual IMultiLineString Reverse()
        {
            int nLines = Geometries.Length;
            ILineString[] revLines = new LineString[nLines];
            for (int i = 0; i < Geometries.Length; i++)
                revLines[nLines - 1 - i] = new LineString(((ILineString)Geometries[i]).Reverse());            
            return Factory.CreateMultiLineString(revLines);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance) 
        {
            if (!IsEquivalentClass(other)) 
                return false;            
            return base.EqualsExact(other, tolerance);
        }

        /// <summary>
        /// Always returns FeatureTypes.Line
        /// </summary>
        public override FeatureTypes FeatureType
        {
            get
            {
                return FeatureTypes.Line;
            }
        }

        /// <summary>
        /// Gets the ILineString that corresponds to the specified index
        /// </summary>
        /// <param name="index">The integer index to get a linestring</param>
        /// <returns>An ILineString</returns>
        public new ILineString this[int index]
        {
            get
            {
                return base[index] as ILineString;
            }
            set
            {
                base[index] = value;
            }
        }
        
    }
}
