//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the the original author of the file, which is an adaptation of the Java KDTree library implemented by Levy 
// and Heckel. This simplified version is written by Marco A. Alvarez
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the KDTreeDll
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford, shortly after 
// Allen Anselmo first added it to MapWinGeoProc in August 2008.  
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.KDTree
{
    /// <summary>
    /// Hyper-Point class supporting KDTree class
    /// </summary>
    public class HPoint : Coordinate
    {

        private readonly int _numDimensions;

        #region Constructors

        /// <summary>
        /// Constructs a new HyperPoint where numDimensions indicates the size of the array for the coordinates
        /// </summary>
        /// <param name="numDimensions">The number of dimensions</param>
        public HPoint(int numDimensions)
        {
            _numDimensions = numDimensions;
        }

        /// <summary>
        /// Constructs a new HyperPoint where the specified array of doubles defines the internal coordinate values.
        /// </summary>
        /// <param name="inCoord">The in coordinate to use when constructing this hyper point.</param>
        public HPoint(double[] inCoord)
        {
            _numDimensions = inCoord.Length;
            if(_numDimensions>0)X = inCoord[0];
            if(_numDimensions>1)Y = inCoord[1];
            if(_numDimensions>2)Z = inCoord[2];
        }

        #endregion

        #region Methods

       
       


       
        #endregion

       

        #region ICoordinate Members

        

       
       

        /// <summary>
        /// Calculates the distance in comparison with any coordinate.  The coordinate with fewer dimensions
        /// will determine the dimensionality for the comparison.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double HyperDistance(HPoint p)
        {
            return Math.Sqrt(SquareHyperDistance(p));
        }
       
        /// <summary>
        /// Calculates the square of the Euclidean distance between this point and the other point.
        /// </summary>
        /// <param name="p">Any valid implementation of ICoordinate</param>
        /// <returns>A double representing the distances.</returns>
        public double SquareHyperDistance(HPoint p)
        {
            int maxOrdinate = Math.Min(p.NumOrdinates, NumOrdinates);
            double dist = 0;
            for (int i = 0; i < maxOrdinate; i++)
            {
                double diff = (p[i] - this[i]);
                dist += (diff * diff);
            }
            return dist;
        }

       

        /// <summary>
        /// Gets or sets the actual values for this class.
        /// </summary>
        public double[] Values
        {
            get
            {
                return ToArray();
            }
            set
            {
                if(value.Length > 0)X = value[0];
                if (value.Length > 1) Y = value[1];
                if (value.Length > 2) Z = value[2];
            }
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// A Static method for returning the square distance between two coordinates
        /// </summary>
        /// <param name="x">One coordinate</param>
        /// <param name="y">A second coordinate</param>
        /// <returns>A double representing the square distance</returns>
        public static double SquareDistance(HPoint x, HPoint y)
        {


            HPoint a = x;
            HPoint b = y;

            int minOrdinate = Math.Min(x.NumOrdinates, y.NumOrdinates);
            double dist = 0;

            if (a != null && b != null)
            {
                // if we can calculate using direct access to the internal coord variable,
                // it will be slightly faster.
                for (int i = 0; i < minOrdinate; ++i)
                {
                    double diff = (a[i] - b[i]);
                    dist += (diff * diff);
                }
            }
            else
            {
                // otherwise, just use the standard ICoordinate accessors
                for (int i = 0; i < minOrdinate; ++i)
                {
                    double diff = (x[i] - y[i]);
                    dist += (diff * diff);
                }
            }

            return dist;

        }

        /// <summary>
        /// Calculates the Euclidean distance between the two coordinates
        /// </summary>
        /// <param name="x">An ICoordinate</param>
        /// <param name="y">Another ICoordinate</param>
        /// <returns>A double representing the distance</returns>
        public static double EuclideanDistance(HPoint x, HPoint y)
        {

            return Math.Sqrt(SquareDistance(x, y));
        }


        #endregion

    }
}
