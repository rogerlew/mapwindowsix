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
using System.ComponentModel;
namespace MapWindow.Geometries
{

    /// <summary>
    /// A lightweight class used to store coordinates
    /// on the 2-dimensional Cartesian plane.
    /// It is distinct from <c>Point</c>, which is a subclass of <c>Geometry</c>.
    /// Unlike objects of type <c>Point</c> (which contain additional
    /// information such as an envelope, a precision model, and spatial reference
    /// system information), a <c>SlowCoordinate</c> only contains ordinate values
    /// and accessor methods.
    /// <c>SlowCoordinate</c>s are two-dimensional points, with an additional
    /// z-ordinate. NTS does not support any operations on the z-ordinate except
    /// the basic accessor functions. Constructed coordinates will have a
    /// z-ordinate of <c>NaN</c>.  The standard comparison functions will ignore
    /// the z-ordinate.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SlowCoordinate : ICoordinate
    {
        private readonly int _numOrdinates;
        private double[] _values;
        private double _m;

        /// <summary>
        /// Creates an empty coordinate with the specified number of ordinates
        /// </summary>
        /// <param name="numOrdinates"></param>
        public SlowCoordinate(int numOrdinates)
        {
            _numOrdinates = numOrdinates;
            _values = new double[numOrdinates];
        }

        /// <summary>
        /// Creates a new coordinate with the same number of ordinates as the number of values in the specified double.
        /// </summary>
        /// <param name="values"></param>
        public SlowCoordinate(double[] values)
        {
            _numOrdinates = values.GetLength(0);
            _values = values;
        }

        /// <summary>
        /// Constructs a <c>SlowCoordinate</c> at (x,y,z).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        /// <param name="m">Measure value.</param>
        public SlowCoordinate(double x, double y, double z, double m)
        {
            _numOrdinates = 3;
            _values = new[]{ x, y, z };
            _m = m;
        }

        /// <summary>
        /// Constructs a <c>SlowCoordinate</c> at (x,y,z).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public SlowCoordinate(double x, double y, double z)
        {
            _numOrdinates = 3;
            _values = new[] { x, y, z };
        }

        /// <summary>
        /// Creates a SlowCoordinate from any ICoordinate Interface
        /// </summary>
        /// <param name="coordinate">The Vector.IPoint interface to construct a coordinate from</param>
        public SlowCoordinate(Coordinate coordinate)
        {
            _numOrdinates = coordinate.NumOrdinates;
            _values = new double[_numOrdinates];
            double[] inVals = coordinate.ToArray();
            for (int I = 0; I < _numOrdinates; I++)
            {
                _values[I] = inVals[I];
            }
        }

      

      

        /// <summary>
        ///  By default, this constructs a 3D coordinate with X, Y and Z values set to 0.
        /// </summary>
        public SlowCoordinate()
        {
            _numOrdinates = 3;
            _values = new double[3];
        }
        

        /// <summary>
        /// Constructs a 2D <c>SlowCoordinate</c> at (x,y).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public SlowCoordinate(double x, double y)
        {
            _numOrdinates = 2;
            _values = new[] { x, y };
        }

 
        /// <summary>
        /// This tests each of the ordinates to see if they are the same.  This will work for any dimension,
        /// but the test for equality will return true even if the dimensions are not equal as long as the
        /// terms match for all the members of the coordinate with the smaller dimension.  In this way,
        /// a 3D coordinate will be equal to a 2D coordinate, even if the Z values are not equal.
        /// </summary>
        /// <param name="other"><c>SlowCoordinate</c> with which to do the comparison.</param>
        /// <returns><c>true</c> the terms are equal up to the extent of dimensions for the smaller dimension.</returns>
        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            Coordinate c = other as Coordinate;
            if (c == null) return false;
            int m = Math.Min(NumOrdinates, c.NumOrdinates);
            for (int I = 0; I < m; I++)
            {
                if (c[I] != _values[I]) return false;
            }
            return true;
        }


        #region Properties

        /// <summary>
        /// Gets the number of ordinates.  This should be a positive integer, but does not include the measure
        /// term (M), which is considered independant of the standard dimension.
        /// </summary>
        public int NumOrdinates
        {
            get { return _numOrdinates; }
        }

        /// <summary>
        /// Gets a double value for the specified zero based ordinate.
        /// </summary>
        /// <param name="index">The zero based integer ordinate.</param>
        /// <returns>A double value.</returns>
        public double this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }



        /// <summary>
        /// X coordinate.
        /// </summary>
        public virtual double X
        {
            get
            {
                if (_numOrdinates > 0) return _values[0];
                return 0;
            }
            set
            {
                if (_numOrdinates > 0) _values[0] = value;
            }
        }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public virtual double Y
        {
            get
            {
                if (_numOrdinates > 1) return _values[1];
                return 0;
            }
            set
            {
                if (_numOrdinates > 1) _values[1] = value;
            }
        }

        /// <summary>
        /// Z coordinate.
        /// </summary>
        public virtual double Z
        {
            get
            {
                if (_numOrdinates > 2) return _values[2];
                return 0;
            }
            set
            {
                if (_numOrdinates > 2) _values[2] = value;
            }
        }

        /// <summary>
        /// A Measure value
        /// </summary>
        public virtual double M
        {
            get
            {
                return _m;
            }
            set
            {
                _m = value;
            }
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(SlowCoordinate obj1, Coordinate obj2)
        {
            return Equals(obj1, obj2);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(SlowCoordinate obj1, Coordinate obj2)
        {
            return !(obj1 == obj2);
        }
       
        /// <summary>
        /// Compares this object with the specified object for order.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// Returns
        ///    -1 : this.x lowerthan other.x || ((this.x == other.x) AND (this.y lowerthan other.y))
        ///    0  : this.x == other.x AND this.y = other.y 
        ///    1  : this.x greaterthan other.x || ((this.x == other.x) AND (this.y greaterthan other.y)) 
        /// </summary>
        /// <param name="other"><c>SlowCoordinate</c> with which this <c>SlowCoordinate</c> is being compared.</param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <c>SlowCoordinate</c>
        ///         is less than, equal to, or greater than the specified <c>SlowCoordinate</c>.
        /// </returns>
        public virtual int CompareTo(object other)
        {
            Coordinate otherCoord = other as Coordinate;
            if (otherCoord == null) throw new ArgumentException(MessageStrings.ArgumentCouldNotBeCast_S1_S2.Replace("%S1","other").Replace("%S2","ICoordinate"));
            if (_values[0] < otherCoord.X)
                return -1;
            if (_values[0] > otherCoord.X)
                return 1;
            if (_values[1] < otherCoord.Y)
                return -1;
            if (_values[1] > otherCoord.Y)
                return 1;
            return 0;
        }

        /// <summary>
        /// Gets or sets the values as a one dimensional array of doubles.
        /// </summary>
        public double[] Values
        {
            get { return _values; }
            set { _values = value; }
        }

       


        

        /// <summary>
        /// Create a new object as copy of this instance.
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            Coordinate c = MemberwiseClone() as Coordinate;
            OnCopy(c);
            return c;
        }

        /// <summary>
        /// Occurs during the copy process and allows for overriding the default behaviors.
        /// </summary>
        /// <param name="copy"></param>
        public virtual void OnCopy(Coordinate copy)
        {
            // this is so that child classes can override the copy behavior if desired.
        }
        

        /// <summary>
        /// Return HashCode.
        /// </summary>
        public override int GetHashCode()
        {
            int result = 17;            
            result = 37 * result + GetHashCode(X);
            result = 37 * result + GetHashCode(Y);
            return result;
        }

        /// <summary>
        /// Return HashCode.
        /// </summary>
        /// <param name="x">Value from HashCode computation.</param>
        public static int GetHashCode(double x)
        {
            long f = BitConverter.DoubleToInt64Bits(x);
            return (int)(f^(f>>32));
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static SlowCoordinate operator +(SlowCoordinate coord1, Coordinate coord2)
        {
            // returns SlowCoordinate as a specific implementatino of ICoordinate
            return new SlowCoordinate(coord1.X + coord2.X, coord1.Y + coord2.Y, coord1.Z + coord2.Z);
        }

       

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static SlowCoordinate operator +(SlowCoordinate coord1, double d)
        {
            return new SlowCoordinate(coord1.X + d, coord1.Y + d, coord1.Z + d);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static SlowCoordinate operator +(double d, SlowCoordinate coord1)
        {
            return coord1 + d;
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static SlowCoordinate operator *(SlowCoordinate coord1, Coordinate coord2)
        {
            return new SlowCoordinate(coord1.X * coord2.X, coord1.Y * coord2.Y, coord1.Z * coord2.Z);
        }
       

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static SlowCoordinate operator *(SlowCoordinate coord1, double d)
        {
            return new SlowCoordinate(coord1.X * d, coord1.Y * d, coord1.Z * d);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static SlowCoordinate operator *(double d, SlowCoordinate coord1)
        {
            return coord1 * d;
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static SlowCoordinate operator -(SlowCoordinate coord1, Coordinate coord2)
        {
            return new SlowCoordinate(coord1.X - coord2.X, coord1.Y - coord2.Y, coord1.Z - coord2.Z);
        }
       

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static SlowCoordinate operator -(SlowCoordinate coord1, double d)
        {
            return new SlowCoordinate(coord1.X - d, coord1.Y - d, coord1.Z - d);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static SlowCoordinate operator -(double d, SlowCoordinate coord1)
        {
            return coord1 - d;
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static SlowCoordinate operator /(SlowCoordinate coord1, Coordinate coord2)
        {
            return new SlowCoordinate(coord1.X / coord2.X, coord1.Y / coord2.Y, coord1.Z / coord2.Z);
        }
        
     
        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static SlowCoordinate operator /(SlowCoordinate coord1, double d)
        {
            return new SlowCoordinate(coord1.X / d, coord1.Y / d, coord1.Z / d);
        }  

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static SlowCoordinate operator /(double d, SlowCoordinate coord1)
        {
            return coord1 / d;
        }

        /* END ADDED BY MPAUL42: monoGIS team */


        
    }
} 
