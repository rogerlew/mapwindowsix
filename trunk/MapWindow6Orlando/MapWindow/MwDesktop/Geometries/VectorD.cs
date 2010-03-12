//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2008 1:51:53 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Geometries
{


    /// <summary>
    /// Creates a mathematical vector of doubles of the arbitrary length N.  
    /// </summary>
    public class VectorD : Coordinate, VectorD
    {
        #region Private Variables

       
        #endregion

        #region Constructors

        /// <summary>
        /// If no indicator is given for the number of elements in the vector,
        /// a 3D vector is assumed.
        /// </summary>
        public VectorD():base(3)
        {
           
        }

        /// <summary>
        /// Creates a new instance of VectorN.
        /// </summary>
        public VectorD(int n):base(n)
        {
     
        }
        /// <summary>
        /// creates an N dimensional vector from the ordinates specified in the array
        /// </summary>
        /// <param name="values">The double values to use for the sequential vector values.</param>
        public VectorD(double[] values)
            : base(values)
        {
        }

        /// <summary>
        /// Creates a 2D vector from ordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public VectorD(double x, double y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Creates a 3D vector from the specified values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public VectorD(double x, double y, double z):base(x, y, z)
        {

        }

        


        #endregion

        #region Methods

        #endregion

        #region Properties

     


        #endregion




        #region VectorD Members

        /// <summary>
        /// Adds vectors
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public VectorD Add(VectorD v)
        {
            if (v.NumOrdinates != NumOrdinates) throw new ArgumentException("The specified vector to add must have the same number of elements (N) as this vector.");
            double[] inVals = v.Values;
            double[] outVals = new double[NumOrdinates];
            for (int I = 0; I < NumOrdinates; I++)
            {
                outVals[I] = base.Values[I] + inVals[I];
            }
            return new VectorD(outVals);
        }

        /// <summary>
        /// Currently this is only supported as a 3 dimensional opperation.  The two dimensional
        /// version where you are looking for an angle is not implemented here, and higher dimensions
        /// are (other than some special cases like 7) don't become degenerate to a vector of the 
        /// same dimension that you started with, but rather end up with a matrix.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public VectorD Cross(VectorD v)
        {
            if (base.NumOrdinates > 3)
            {
                throw new NotImplementedException("Support for N - dimensional cross product for vectors where N > 3 is not implemented.");
            }
            
            double[] result = new double[3];
            result[0] = (Y * v.Z - Z * v.Y);
            result[1] = (Z * v.Z - X * v.Z);
            result[2] = (X * v.Y - Y * v.X);
            return new VectorD(result);
        }

        /// <summary>
        /// The dot product
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Dot(VectorD v)
        {
            if (v.NumOrdinates != NumOrdinates) throw new ArgumentException("The specified vector to add must have the same number of elements (N) as this vector.");
            double[] inVals = v.Values;
            double result = 0;
            for (int I = 0; I < NumOrdinates; I++)
            {
                result += base.Values[I] + inVals[I];
            }
            return result;
        }

        /// <summary>
        /// Multiplies this vector by the specified scalar.
        /// </summary>
        /// <param name="scalar">A double value to multiply against all of the terms</param>
        /// <returns>Another vector</returns>
        public VectorD Multiply(double scalar)
        {
           
            double[] outVals = new double[NumOrdinates];
            for (int I = 0; I < NumOrdinates; I++)
            {
                outVals[I] = base.Values[I] * scalar;
            }
            return new VectorD(outVals);
        }

        /// <summary>
        /// Divides every term by the length of the vector, so that the length of the new
        /// vector will be 1, but the direction will be the same as the original.
        /// </summary>
        public void Normalize()
        {
            double l = Length;
            Coordinate c = this;
            double[] vals = c.ToArray();
            for (int I = 0; I < NumOrdinates; I++)
            {
                this[I] = vals[I] / l;
            }
            
        }

        /// <summary>
        /// Subtracts the specified vector from this vector
        /// </summary>
        /// <param name="v">The vector to subtract from this vector</param>
        /// <returns>A new vector</returns>
        public VectorD Subtract(VectorD v)
        {
            if (v.NumOrdinates != NumOrdinates) throw new ArgumentException("The specified vector to add must have the same number of elements (N) as this vector.");
            double[] inVals = v.Values;
            double[] outVals = new double[NumOrdinates];
            for (int I = 0; I < NumOrdinates; I++)
            {
                outVals[I] = base.Values[I] - inVals[I];
            }
            return new VectorD(outVals);
        }

        /// <summary>
        /// Gets the euclidean length of this vector.  This includes all of the ordinates.
        /// </summary>
        public double Length
        {
            get
            {

                double result = 0;
                for (int I = 0; I < NumOrdinates; I++)
                {
                   result += base.Values[I] * base.Values[I];
                }
                return Math.Sqrt(result);
            }
            
        }


        #endregion

       

       
    }
}
