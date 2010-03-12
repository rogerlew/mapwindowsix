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

using System;
using System.Collections.Generic;
using System.Linq;
using MapWindow.Main;
namespace MapWindow.Geometries
{       
    /// <summary>
    /// A list of Coordinates, which may
    /// be set to prevent repeated coordinates from occuring in the list.
    /// </summary>
    public class CoordinateList : BaseList<Coordinate>, ICloneable
    {
     
        /// <summary>
        /// Creates a new instance of a coordiante list
        /// </summary>
        public CoordinateList()
        {
            InnerList = new List<Coordinate>();
        }

        /// <summary>
        /// The basic constructor for a CoordinateArray allows repeated points
        /// (i.e produces a CoordinateList with exactly the same set of points).
        /// </summary>
        /// <param name="coords">Initial coordinates</param>
        public CoordinateList(IEnumerable<Coordinate> coords)
        {
            DoAddRange(coords, true);
        }

      

        /// <summary>
        /// Constructs a new list from an array of Coordinates,
        /// allowing caller to specify if repeated points are to be removed.
        /// </summary>
        /// <param name="coords">Array of coordinates to load into the list.</param>
        /// <param name="allowRepeated">If <c>false</c>, repeated points are removed.</param>
        public CoordinateList(IEnumerable<Coordinate> coords, bool allowRepeated)
        {
            DoAddRange(coords, allowRepeated);
        }


        /// <summary>
        /// Add an array of coordinates.
        /// </summary>
        /// <param name="coords">Coordinates to be inserted.</param>
        /// <param name="allowRepeated">If set to false, repeated coordinates are collapsed.</param>
        /// <param name="direction">If false, the array is added in reverse order.</param>
        /// <returns>Return true.</returns>
        public virtual void Add(IEnumerable<Coordinate> coords, bool allowRepeated, bool direction)
        {
            if (!direction) coords.Reverse();
            DoAddRange(coords, allowRepeated);
        }

        /// <summary>
        /// Add an array of coordinates.
        /// </summary>
        /// <param name="coord">Coordinates to be inserted.</param>
        /// <param name="allowRepeated">If set to false, repeated coordinates are collapsed.</param>
        /// <returns>Return true.</returns>
        public virtual void Add(IEnumerable<Coordinate> coord, bool allowRepeated)
        {
            Add(coord, allowRepeated, true);
        }


        /// <summary>
        /// Add a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate to be inserted.</param>
        /// <param name="allowRepeated">If set to false, repeated coordinates are collapsed.</param>
        /// <returns>Return true if all ok.</returns>
        public virtual void Add(Coordinate coord, bool allowRepeated)
        {
            // don't add duplicate coordinates
            DoAdd(coord, allowRepeated);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="allowRepeated"></param>
        protected void DoAdd(Coordinate coord, bool allowRepeated)
        {
            if (!allowRepeated)
            {
                if (Count >= 1)
                {
                    Coordinate last = this[Count - 1];
                    if (last.Equals2D(coord))
                        return;
                }
            }
            Add(coord);
            return;
        }

        /// <summary>
        /// Adds the entire range of coordinates, preventing coordinates with the identical 2D signiture from
        /// being added immediately after it's duplicate.
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="allowRepeated"></param>
        protected void DoAddRange(IEnumerable<Coordinate> coords, bool allowRepeated)
        {
            foreach (Coordinate coord in coords)
            {
                DoAdd(coord, allowRepeated);
            }
        }


        /// <summary>
        /// Ensure this coordList is a ring, by adding the start point if necessary.
        /// </summary>
        public virtual void CloseRing()
        {
            if(Count > 0)
                Add(this[0], false);
        }

        /// <summary>
        /// Returns the Coordinates in this collection.
        /// </summary>
        /// <returns>Coordinater as <c>Coordinate[]</c> array.</returns>
        public virtual Coordinate[] ToCoordinateArray()
        {
            return InnerList.ToArray();
        }

        /// <summary>
        /// Returns a deep copy of this collection.
        /// </summary>
        /// <returns>The copied object.</returns>
        public object Clone()
        {         
            CoordinateList copy = new CoordinateList();
            foreach (Coordinate c in this)
                copy.Add(c.Copy());
            return copy;
        }

        
    } 
} 
