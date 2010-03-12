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
using System.Collections.Generic;
using System.Linq;

namespace MapWindow.Geometries
{
    /// <summary>  
    /// Basic implementation of <c>LinearRing</c>.
    /// The first and last point in the coordinate sequence must be equal.
    /// Either orientation of the ring is allowed.
    /// A valid ring must not self-intersect.
    /// </summary>
    [Serializable]
    public class LinearRing : LineString, ILinearRing
    {

       

        /// <summary>
        /// Creates a new instance of a linear ring where the enumerable collection of
        /// coordinates represents the set of coordinates to add to the ring.
        /// </summary>
        /// <param name="coordinates"></param>
        public LinearRing(IEnumerable<Coordinate> coordinates):base(coordinates)
        {
            ValidateConstruction();
        }

        /// <summary>
        /// Creates a new instance of a linear ring where the enumerable collection of
        /// coordinates represents the set of coordinates to add to the ring.
        /// </summary>
        /// <param name="coordinates"></param>
        public LinearRing(IEnumerable<ICoordinate> coordinates)
            : base(coordinates)
        {
            ValidateConstruction();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="linestringbase"></param>
        public LinearRing(IBasicLineString linestringbase)
            : base(linestringbase)
        {
            
        }

        private void ValidateConstruction() 
        {
            if (!IsEmpty && !base.IsClosed)
            {
                IList<Coordinate> lCoords = Coordinates.ToList();
                lCoords.Add(Coordinates[0].Copy());
                // The sequence is not closed, so add the first point again to close it.
                Coordinates = lCoords;
            }         
            if (Coordinates.Count >= 1 && Coordinates.Count < 3) 
                throw new ArgumentException("Number of points must be 0 or >= 3");            
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsSimple
        {
            get
            {
                return true;
            }
        }

       
        /// <summary>
        /// Geometry Type
        /// </summary>
        public override string GeometryType
        {
            get
            {
                return "LinearRing";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsClosed
        {
            get
            {
                return true;
            }
        }


        
        /* END ADDED BY MPAUL42: monoGIS team */
        /// <summary>
        /// This will always contain Line, even if it is technically empty
        /// </summary>
        public override FeatureTypes FeatureType
        {
            get
            {
                return FeatureTypes.Line;
            }
        }
       
    }
}
