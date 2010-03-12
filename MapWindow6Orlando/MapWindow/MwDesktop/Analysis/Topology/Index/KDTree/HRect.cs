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

namespace MapWindow.Analysis.Topology.KDTree
{
    /// <summary>
    /// Hyper-Rectangle class supporting KDTree class
    /// </summary>
    public class HRect: ICloneable
    {
        // Internal is being used for performance for KD calculations, where these would normally be private.
        #region Internal Variables
        
        /// <summary>
        /// Minimum values
        /// </summary>
        internal HPoint _min;

        /// <summary>
        /// Maximum values
        /// </summary>
        internal HPoint _max;

        #endregion

        /// <summary>
        /// Constructs a new instance of a rectangle binding structure based on a specified number of dimensions
        /// </summary>
        /// <param name="numDimensions">An integer representing the number of dimensions.  For X, Y coordinates, this should be 2.</param>
        public HRect(int numDimensions)
        {
            _min = new HPoint(numDimensions);
            _max = new HPoint(numDimensions);
        }

        /// <summary>
        /// Creates a new bounding rectangle based on the two coordinates specified.  It is assumed that
        /// the vmin and vmax coordinates have already been correctly calculated.
        /// </summary>
        /// <param name="vmin"></param>
        /// <param name="vmax"></param>
        public HRect(HPoint vmin, HPoint vmax)
        {

            _min = vmin.Copy();
            _max = vmax.Copy();
        }

        /// <summary>
        /// Creates a duplicate of this object
        /// </summary>
        /// <returns>An object duplicate of this object</returns>
        public object Clone()
        {

            return new HRect(_min, _max);
        }

        /// <summary>
        /// Creates a duplicate of this bounding box using the existing minimum and maximum.
        /// </summary>
        /// <returns>An HRect duplicate of this object</returns>
        public HRect Copy()
        {
            return new HRect(_min, _max);
        }

        
        /// <summary>
        /// Calculates the closest point on the hyper-extent to the specified point.
        /// from Moore's eqn. 6.6
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public HPoint Closest(HPoint t)
        {
            int len = t.NumOrdinates;
            HPoint p = new HPoint(len);
            
            for (int i = 0; i < len; ++i)
            {
                if (t[i] <= _min[i])
                {
                    p[i] = _min[i];
                }
                else if (t[i] >= _max[i])
                {
                    p[i] = _max[i];
                }
                else
                {
                    p[i] = t[i];
                }
            }
            return p;
        }

        /// <summary>
        /// This method calculates the furthest point on the rectangle
        /// from the specified point.  This is to determine if it is
        /// possible for any of the members of the closer rectangle
        /// to be positioned further away from the test point than
        /// the points in the hyper-extent that is further from the point.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public HPoint Farthest(HPoint t)
        {
            int len = t.NumOrdinates;
            HPoint p = new HPoint(len);

            for (int i = 0; i < len; ++i)
            {
                if (t[i] <= _min[i])
                {
                    p[i] = _max[i];
                }
                else if (t[i] >= _max[i])
                {
                    p[i] = _min[i];
                }
                else
                {
                    // Calculating the closest position always uses the point, but
                    // to calculate the furthest position, we actually want to
                    // pick the furhter of two extremes, in order to pick up the
                    // diagonal distance.
                    // p[i] = t[i];
                    if (t[i] - _min[i] > _max[i] - t[i])
                    {
                        p[i] = _min[i];
                    }
                    else
                    {
                        p[i] = _max[i];
                    }
                }
            }
            return p;
        }

       
        /// <summary>
        /// Calculates a new HRect object that has a nearly infinite bounds.
        /// </summary>
        /// <param name="d">THe number of dimensions to use</param>
        /// <returns>A new HRect where the minimum is negative infinity, and the maximum is positive infinity</returns>
        /// <remarks>Used in initial conditions of KDTree.nearest()</remarks>
        public static HRect InfiniteHRect(int d)
        {

            HPoint vmin = new HPoint(d);
            HPoint vmax = new HPoint(d);

            for (int i = 0; i < d; ++i)
            {
                vmin[i] = Double.NegativeInfinity;
                vmax[i] = Double.PositiveInfinity;
            }

            return new HRect(vmin, vmax);
        }

        /// <summary>
        /// If the specified HRect does not intersect this HRect, this returns null.  Otherwise,
        /// this will return a smaller rectangular region that represents the intersection
        /// of the two bounding regions.
        /// </summary>
        /// <param name="region">Another HRect object to intersect with this one.</param>
        /// <returns>The HRect that represents the intersection area for the two bounding boxes.</returns>
        /// <remarks>currently unused</remarks>
        public HRect Intersection(HRect region)
        {

            HPoint newmin = new HPoint(_min.NumOrdinates);
            HPoint newmax = new HPoint(_min.NumOrdinates);

            for (int i = 0; i < _min.NumOrdinates; ++i)
            {
                newmin[i] = Math.Max(_min[i], region._min[i]);
                newmax[i] = Math.Min(_max[i], region._max[i]);
                if (newmin[i] >= newmax[i]) return null;
            }

            return new HRect(newmin, newmax);
        }

        /// <summary>
        /// Gets the current hyper-volume.  For 1D, this is Length.  For 2D this is Area.  For 3D this is Volume.
        /// </summary>
        public double HyperVolume
        {
            get
            {

                double a = 1;

                for (int i = 0; i < _min.NumOrdinates; ++i)
                {
                    a *= (_max[i] - _min[i]);
                }

                return a;
            }
        }

        /// <summary>
        /// Creates a string that represents this bounding box
        /// </summary>
        /// <returns>A String</returns>
        public override string ToString()
        {
            return _min + "\n" + _max + "\n";
        }

        /// <summary>
        /// Gets or sets the minimum coordinate (containing the smaller value) in all dimensions.
        /// </summary>
        public HPoint Minimum
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Gets or sets the maximum coordinate
        /// </summary>
        public HPoint Maximum
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Gets the number of ordinates for this rectangular structure (based on the minimum HPoint)
        /// </summary>
        public int NumOrdinates
        {
            get { return _min.NumOrdinates; }
        }
    }
}
