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
namespace MapWindow.GeometriesGraph
{
    /// <summary> 
    /// A TopologyLocation is the labelling of a
    /// GraphComponent's topological relationship to a single Geometry.
    /// If the parent component is an area edge, each side and the edge itself
    /// have a topological location.  These locations are named:
    ///  On: on the edge
    ///  Left: left-hand side of the edge
    ///  Right: right-hand side
    /// If the parent component is a line edge or node, there is a single
    /// topological relationship attribute, On.
    /// The possible values of a topological location are
    /// { Location.Null, Location.Exterior, Location.Boundary, Location.Interior } 
    /// The labelling is stored in an array location[j] where
    /// where j has the values On, Left, Right.
    /// </summary>
    public class TopologyLocation 
    {
        private Locations[] location;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        public TopologyLocation(Locations[] location)
        {
            Init(location.Length);
        }

        /// <summary> 
        /// Constructs a TopologyLocation specifying how points on, to the left of, and to the
        /// right of some GraphComponent relate to some Geometry. Possible values for the
        /// parameters are Location.Null, Location.Exterior, Location.Boundary, 
        /// and Location.Interior.
        /// </summary>        
        /// <param name="on"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public TopologyLocation(Locations on, Locations left, Locations right) 
        {
            Init(3);
            location[(int)Positions.On] = on;
            location[(int)Positions.Left] = left;
            location[(int)Positions.Right] = right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="on"></param>
        public TopologyLocation(Locations on) 
        {
            Init(1);
            location[(int)Positions.On] = on;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gl"></param>
        public TopologyLocation(TopologyLocation gl) 
        {
            Init(gl.location.Length);
            if (gl != null) 
            {
                for (int i = 0; i < location.Length; i++) 
                    location[i] = gl.location[i];                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        private void Init(int size)
        {
            location = new Locations[size];
            SetAllLocations(Locations.Null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual Locations Get(Positions posIndex)
        {
            int index = (int)posIndex;
            if (index < location.Length)
                return location[index];
            return Locations.Null;
        }

        /// <summary>
        /// Get calls Get(Positions posIndex),
        /// Set calls SetLocation(Positions locIndex, Locations locValue)
        /// </summary>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual Locations this[Positions posIndex]
        {
            get
            {
                return Get(posIndex);
            }
            set
            {
                SetLocation(posIndex, value);
            }
        }

        /// <returns>
        /// <c>true</c> if all locations are Null.
        /// </returns>
        public virtual bool IsNull
        {
            get
            {
                for (int i = 0; i < location.Length; i++)
                    if (location[i] != Locations.Null) 
                        return false;
                return true;
            }
        }

        /// <returns> 
        /// <c>true</c> if any locations are Null.
        /// </returns>
        public virtual bool IsAnyNull
        {
            get
            {
                for (int i = 0; i < location.Length; i++)
                    if (location[i] == Locations.Null) 
                        return true;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="le"></param>
        /// <param name="locIndex"></param>
        /// <returns></returns>
        public virtual bool IsEqualOnSide(TopologyLocation le, int locIndex)
        {
            return location[locIndex] == le.location[locIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsArea
        {
            get
            {
                return location.Length > 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsLine
        {
            get
            {
                return location.Length == 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Flip()
        {
            if (location.Length <= 1) 
                return;
            Locations temp = location[(int)Positions.Left];
            location[(int)Positions.Left] = location[(int)Positions.Right];
            location[(int)Positions.Right] = temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locValue"></param>
        public virtual void SetAllLocations(Locations locValue)
        {
            for (int i = 0; i < location.Length; i++) 
                location[i] = locValue;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locValue"></param>
        public virtual void SetAllLocationsIfNull(Locations locValue)
        {
            for (int i = 0; i < location.Length; i++) 
                if (location[i] == Locations.Null) 
                    location[i] = locValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locIndex"></param>
        /// <param name="locValue"></param>
        public virtual void SetLocation(Positions locIndex, Locations locValue)
        {
            location[(int)locIndex] = locValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locValue"></param>
        public virtual void SetLocation(Locations locValue)
        {
            SetLocation(Positions.On, locValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Locations[] GetLocations() 
        {
            return location; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="on"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public virtual void SetLocations(Locations on, Locations left, Locations right) 
        {
            location[(int)Positions.On] = on;
            location[(int)Positions.Left] = left;
            location[(int)Positions.Right] = right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gl"></param>
        public virtual void SetLocations(TopologyLocation gl) 
        {
            for (int i = 0; i < gl.location.Length; i++) 
                location[i] = gl.location[i];            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public virtual bool AllPositionsEqual(Locations loc)
        {
            for (int i = 0; i < location.Length; i++) 
                if (location[i] != loc) 
                    return false;
            return true;
        }

        /// <summary>
        /// Merge updates only the Null attributes of this object
        /// with the attributes of another.
        /// </summary>
        public virtual void Merge(TopologyLocation gl)
        {
            // if the src is an Area label & and the dest is not, increase the dest to be an Area
            if (gl.location.Length > location.Length) 
            {
                Locations[] newLoc = new Locations[3];
                newLoc[(int)Positions.On] = location[(int)Positions.On];
                newLoc[(int)Positions.Left] = Locations.Null;
                newLoc[(int)Positions.Right] = Locations.Null;
                location = newLoc;
            }
            for (int i = 0; i < location.Length; i++) 
                if (location[i] == Locations.Null && i < gl.location.Length)
                    location[i] = gl.location[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (location.Length > 1)
                sb.Append(Location.ToLocationSymbol(location[(int)Positions.Left]));
            sb.Append(Location.ToLocationSymbol(location[(int)Positions.On]));
            if (location.Length > 1)
                sb.Append(Location.ToLocationSymbol(location[(int)Positions.Right]));
            return sb.ToString();
        }
    }
}
