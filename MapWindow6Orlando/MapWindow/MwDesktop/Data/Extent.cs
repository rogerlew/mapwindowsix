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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/11/2009 2:34:48 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.ComponentModel;
using MapWindow.Geometries;
using MapWindow.Serialization;

namespace MapWindow.Data
{


    /// <summary>
    /// Extent works like an envelope but is faster acting, has a minimum memory profile, only works in 2D and has no events.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Extent : ICloneable
    {
        #region Private Variables
        /// <summary>
        /// Min X
        /// </summary>
        [Serialize("XMin")]
        public double XMin;
        /// <summary>
        /// Max X
        /// </summary>
        [Serialize("XMax")]
        public double XMax;
        /// <summary>
        /// Min Y
        /// </summary>
        [Serialize("YMin")]
        public double YMin;
        /// <summary>
        /// Max Y
        /// </summary>
        [Serialize("YMax")]
        public double YMax;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Extent.  This introduces no error checking and assumes
        /// that the user knows what they are doing when working with this.
        /// </summary>
        public Extent()
        {
            XMin = double.MaxValue;
            XMax = double.MinValue;
            YMin = double.MaxValue;
            YMax = double.MinValue;
        }

        /// <summary>
        /// Creates a new extent from the specified ordinates
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMax"></param>
        public Extent(double xMin, double yMin, double xMax, double yMax)
        {
            XMin = xMin;
            YMin = yMin;
            XMax = xMax;
            YMax = yMax;
        }

        /// <summary>
        /// Given a long array of doubles, this builds an extent from a small part of that
        /// xmin, ymin, xmax, ymax
        /// </summary>
        /// <param name="values"></param>
        /// <param name="offset"></param>
        public Extent(double[] values, int offset)
        {
            XMin = values[0 + offset];
            YMin = values[1 + offset];
            XMax = values[2 + offset];
            YMax = values[3 + offset];
        }

        /// <summary>
        /// XMin, YMin, XMax, YMax order
        /// </summary>
        /// <param name="values"></param>
        public Extent(double[] values)
        {
            XMin = values[0];
            YMin = values[1];
            XMax = values[2];
            YMax = values[3];
        }

        /// <summary>
        /// Creates a new extent from the specified envelope
        /// </summary>
        /// <param name="env"></param>
        public Extent(IEnvelope env)
        {
            XMin = env.Minimum.X;
            YMin = env.Minimum.Y;
            XMax = env.Maximum.X;
            YMax = env.Maximum.Y;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Creates a geometric envelope interface from this
        /// </summary>
        /// <returns></returns>
        public IEnvelope ToEnvelope()
        {
            return new Envelope(XMin, XMax, YMin, YMax);
        }

        #endregion

        /// <summary>
        /// Returns true if the coordinate exists anywhere within this envelope
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool Intersects(Coordinate c)
        {
            if (double.IsNaN(c.X) || double.IsNaN(c.Y)) return false;
            return c.X >= XMin && c.X <= XMax && c.Y >= YMin && c.Y <= YMax;
        }

        /// <summary>
        /// Tests for intersection with the specified coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Intersects(double x, double y)
        {
            if (double.IsNaN(x) || double.IsNaN(y)) return false;
            return x >= XMin && x <= XMax && y >= YMin && y <= YMax;
        }

        /// <summary>
        /// Tests to see if the point is inside or on the boundary of this extent.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public bool Intersects(Vertex vert)
        {
            if (vert.X < XMin) return false;
            if (vert.X > XMax) return false;
            if (vert.Y < YMin) return false;
            if (vert.Y > YMax) return false;
            return true;
        }

        /// <summary>
        /// Tests for an intersection with the specified extent
        /// </summary>
        /// <param name="ext">The other extent</param>
        /// <returns>Boolean, true if they overlap anywhere, or even touch</returns>
        public bool Intersects(Extent ext)
        {
            if (ext.XMax < XMin) return false;
            if (ext.YMax < YMin) return false;
            if (ext.XMin > XMax) return false;
            if (ext.YMin > YMax) return false;
            return true;
        }

        /// <summary>
        /// Tests with the specified envelope for a collision
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public bool Intersects(IEnvelope env)
        {
            if (env.Maximum.X < XMin) return false;
            if (env.Maximum.Y < YMin) return false;
            if (env.Minimum.X > XMax) return false;
            if (env.Minimum.Y > YMax) return false;
            return true;
        }
        /// <summary>
        /// Expands this extent to include the domain of the specified extent
        /// </summary>
        /// <param name="ext">The extent to expand to include</param>
        public void ExpandToInclude(Extent ext)
        {
            if (ext.XMin < XMin) XMin = ext.XMin;
            if (ext.YMin < YMin) YMin = ext.YMin;
            if (ext.XMax > XMax) XMax = ext.XMax;
            if (ext.YMax > YMax) YMax = ext.YMax;
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ExpandToInclude(double x, double y)
        {
            if (x < XMin) XMin = x;
            if (y < YMin) YMin = y;
            if (x > XMax) XMax = x;
            if (y > YMax) YMax = y;
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public bool Within(Extent ext)
        {
            if(XMin < ext.XMin) return false;
            if (XMax > ext.XMax) return false;
            if (YMin < ext.YMin) return false;
            if (YMax > ext.YMax) return false;
            return true;
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public bool Contains(Extent ext)
        {
            if (XMin > ext.XMin) return false;
            if (XMax < ext.XMax) return false;
            if (YMin > ext.YMin) return false;
            if (YMax < ext.YMax) return false;
            return true;
        }

       

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public bool Within(IEnvelope env)
        {
            if (XMin < env.Minimum.X) return false;
            if (XMax > env.Maximum.X) return false;
            if (YMin < env.Minimum.Y) return false;
            if (YMax > env.Maximum.Y) return false;
            return true;
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public bool Contains(IEnvelope env)
        {
            if (XMin > env.Minimum.X) return false;
            if (XMax < env.Maximum.X) return false;
            if (YMin > env.Minimum.Y) return false;
            if (YMax < env.Maximum.Y) return false;
            return true;
        }

        /// <summary>
        /// If this is undefined, it will have a min that is larger than the max.
        /// </summary>
        /// <returns>Boolean, true if the envelope has not had values set for it yet.</returns>
        public bool IsEmpty()
        {
            if (XMin > XMax || YMin > YMax) return true;
            return false;
        }

        #region Properties



        #endregion




        #region ICloneable Members

        /// <summary>
        /// Produces a clone, rather than using this same object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Extent(XMin, YMin, XMax, YMax);
        }

        #endregion
    }
}
