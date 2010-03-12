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
namespace MapWindow.Analysis.Topology.Operation.Valid
{
    /// <summary>
    /// Contains information about the nature and location of 
    /// a <see cref="Geometry" /> validation error.
    /// </summary>
    public enum TopologyValidationErrors
    {     
        /// <summary>
        /// Not used.
        /// </summary>
        [Obsolete("Not used")]
        Error = 0,

        /// <summary>
        /// No longer used: 
        /// repeated points are considered valid as per the SFS.
        /// </summary>
        [Obsolete("No longer used: repeated points are considered valid as per the SFS")]
        RepeatedPoint = 1,

        /// <summary>
        /// Indicates that a hole of a polygon lies partially 
        /// or completely in the exterior of the shell.
        /// </summary>
        HoleOutsideShell = 2,

        /// <summary>
        /// Indicates that a hole lies 
        /// in the interior of another hole in the same polygon.
        /// </summary>
        NestedHoles = 3,

        /// <summary>
        /// Indicates that the interior of a polygon is disjoint
        /// (often caused by set of contiguous holes splitting 
        /// the polygon into two parts).
        /// </summary>
        DisconnectedInteriors = 4,

        /// <summary>
        /// Indicates that two rings of a polygonal geometry intersect.
        /// </summary>
        SelfIntersection = 5,

        /// <summary>
        /// Indicates that a ring self-intersects.
        /// </summary>
        RingSelfIntersection = 6,

        /// <summary>
        /// Indicates that a polygon component of a 
        /// <see cref="MultiPolygon" /> lies inside another polygonal component.
        /// </summary>
        NestedShells = 7,

        /// <summary>
        /// Indicates that a polygonal geometry 
        /// contains two rings which are identical.
        /// </summary>
        DuplicateRings = 8,

        /// <summary>
        /// Indicates that either:
        /// - A <see cref="LineString" /> contains a single point.
        /// - A <see cref="LinearRing" /> contains 2 or 3 points.
        /// </summary>
        TooFewPoints = 9,

        /// <summary>
        /// Indicates that the <c>X</c> or <c>Y</c> ordinate of
        /// a <see cref="Coordinate" /> is not a valid 
        /// numeric value (e.g. <see cref="Double.NaN" />).
        /// </summary>
        InvalidCoordinate = 10,

        /// <summary>
        /// Indicates that a ring is not correctly closed
        /// (the first and the last coordinate are different).
        /// </summary>
        RingNotClosed = 11,
    }

    /// <summary>
    /// Contains information about the nature and location of a <c>Geometry</c>
    /// validation error.
    /// </summary>
    public class TopologyValidationError 
    {        
        // NOTE: modified for "safe" assembly in Sql 2005
        // Added readonly!

        /// <summary>
        /// These messages must synch up with the indexes above
        /// </summary>
        private static readonly string[] errMsg = 
        {
            "Topology Validation Error",
            "Repeated Point",
            "Hole lies outside shell",
            "Holes are nested",
            "Interior is disconnected",
            "Self-intersection",
            "Ring Self-intersection",
            "Nested shells",
            "Duplicate Rings",
            "Too few points in geometry component",
            "Invalid Coordinate"
        };

        private TopologyValidationErrors errorType;
        private Coordinate pt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="pt"></param>
        public TopologyValidationError(TopologyValidationErrors errorType, Coordinate pt)
        {
            this.errorType = errorType;
            if(pt != null)
                this.pt = (Coordinate)pt.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        public TopologyValidationError(TopologyValidationErrors errorType) : this(errorType, null) { }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return pt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual TopologyValidationErrors ErrorType
        {
            get
            {
                return errorType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual String Message
        {
            get
            {
                return errMsg[(int)errorType];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Message + " at or near point " + pt;
        }
    }
}
