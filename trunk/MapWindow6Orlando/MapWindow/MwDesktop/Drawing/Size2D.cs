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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 10:52:13 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using MapWindow.Forms;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// Size2D
    /// </summary>
    [Serializable, TypeConverter(typeof(Size2DConverter))]
    public class Size2D : Descriptor
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets the width
        /// </summary>
		[Serialize("Width")]
		public double Width;

        /// <summary>
        /// Gets or sets the height
        /// </summary>
		[Serialize("Height")]
		public double Height;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Size2D
        /// </summary>
        public Size2D()
        {

        }

        /// <summary>
        /// Creates a new instance of a Size2D
        /// </summary>
        /// <param name="width">The double width</param>
        /// <param name="height">The double height</param>
        public Size2D(double width, double height)
        {
            Width = width;
            Height = height;
        }

        #endregion

        #region Methods

      
       
       
        /// <summary>
        /// Tests for equality against an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Size2D other = obj as Size2D;
            if (((object)other) == null) return false;
            return Equals(other);
        }

        /// <summary>
        /// Tests for equality against another size.  
        /// </summary>
        /// <param name="size">the size to compare this size to</param>
        /// <returns>boolean, true if the height and width are the same in each case.</returns>
        public bool Equals(Size2D size)
        {
            if (((object)size) == null) return false;
            return (size.Width == Width && size.Height == Height);
        }


        



       


        /// <summary>
        /// Gets the string equivalent of this object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Width + ", " + Height;
        }

        #endregion


        /// <summary>
        /// Determines if the height and width are both equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Size2D a, Size2D b)
        {
            if (((object)a) == null && ((object)b) == null) return true;
            if ((object)a == null) return false;
            if ((object)b == null) return false;
            return (a.Width == b.Width && a.Height == b.Height);
        }

        /// <summary>
        /// Determiens if the height and width are not equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Size2D a, Size2D b)
        {
            if (((object)a) == null && ((object)b) == null) return false;
            if (((object)a) == null) return true;
            if (((object)b) == null) return true;
            return !(a.Width == b.Width && a.Height == b.Height);
        }

        /// <summary>
        /// Returns the basic hash code for the object. 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region protected methods


        /// <summary>
        /// Generates random doubles for the size from 1 to 100 for both the width and height
        /// </summary>
        protected override void OnRandomize(Random generator)
        {
            Width = generator.NextDouble() * 100;
            Height = generator.NextDouble() * 100;
        }

        

        #endregion

    }
}
