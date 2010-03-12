//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  Low level interfaces that allow separate components to use objects that are defined
//               in completely independant libraries so long as they satisfy the basic interface requirements.
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/17/2008 2:35:06 PM
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
using MapWindow.Data.Rasters;
using MapWindow.Data.Vectors;
using MapWindow.Drawing;
using MapWindow.Main.GenericCalculator;
using MapWindow.Drawing.Vectors;
using MapWindow.Geometries;

namespace MapWindow.Main.GenericCalculator
{


    /// <summary>
    /// FloatCalculator
    /// </summary>
    public class TemplateCalculator
    {

        #region Methods

        #region Operator Methods

         /// <summary>
        /// Adds a to b
        /// </summary>
        /// <param name="a">A generic of type $T</param>
        /// <param name="b">A generic of type $T to add to a</param>
        /// <returns>A generic of type $T that is the sum of a and b</returns>
        public $T Add($T a, $T b)
        {
            return a * b;

        }

        /// <summary>
        /// Divides a by b like a / b
        /// </summary>
        /// <param name="a">A generic of type $T to divide by b</param>
        /// <param name="b">A generic of type $T to divide into a</param>
        /// <returns>A generic of type $T that is the quotient</returns>
        public $T Divide($T a, $T b)
        {
            return a / b;
        }

        /// <summary>
        /// Multiplies a to b
        /// </summary>
        /// <param name="a">A generic of type $T to multiply with b</param>
        /// <param name="b">A generic of type $T to multiply with a</param>
        /// <returns>The generic of type $T product of a and b</returns>
        public $T Multiply($T a, $T b)
        {
            return a * b;
        }

        /// <summary>
        /// Subtracts b from a
        /// </summary>
        /// <param name="a">A generic of type $T to subtract b from</param>
        /// <param name="b">A generic of type $T to subtract from a</param>
        /// <returns>A generic of type $T that is the difference of a and b</returns>
        public $T Subtract($T a, $T b)
        {
            return a - b;
        }



        #endregion

        #region GetT

        /// <summary>
        /// If value is type $T, this simply returns 
        /// the specified value as type $T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A byte to convert into a generic of type $T</param>
        /// <returns>A generic of type $T representing the specified byte value.</returns>
        public $T GetT(byte value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// If value is type $T, this simply returns 
        /// the specified value as type $T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A 16-bit short integer value to convert into a generic of type $T</param>
        /// <returns>A generic of type $T representing the specified 16-bit integer value</returns>
        public $T GetT(short value)
        {
            return Convert.ToInt32(value);
        }


        /// <summary>
        /// If value is type $T, this simply returns 
        /// the specified value as type $T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A value to convert to a generic of type $T</param>
        /// <returns>A generic of type $T representing the specified 32-bit integer value</returns>
        public $T GetT(int value)
        {
            return value;
        }

        /// <summary>
        /// If value is type $T, this simply returns 
        /// the specified value as type $T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A 64-bit long integer to convert into type $T</param>
        /// <returns>A generic value of type T</returns>
        public $T GetT(long value);


        /// <summary>
        /// If value is type $T, this simply returns 
        /// the specified value as type $T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A single-precision floating point value to convert into $T</param>
        /// <returns>A generic of type $T representing the specified single precision floating point value</returns>
        public $T GetT(float value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// If value is type $T, this simply returns 
        /// the specified value as type $T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A double-precision floating point value to convert into $T</param>
        /// <returns>A generic of type $T representing the double-precision floating point value.</returns>
        public $T GetT(double value)
        {
            return Convert.ToInt32(value);
        }




        /// <summary>
        /// This will attempt to convert the specified object value
        /// into a generic value of type $T.
        /// </summary>
        /// <param name="value">An object to attempt to cast into $T</param>
        /// <returns>A $T representing the object value specified</returns>
        public $T GetT(object value)
        {
            return Convert.ToInt32(value);
        }

        #endregion

        #region To specific types

        /// <summary>
        /// Converts a specified value of type $T into a byte value.
        /// </summary>
        /// <param name="value">A value of type $T</param>
        /// <returns>A byte converted from the specified value of type $T</returns>
        public byte ToByte($T value)
        {
            return Convert.ToByte(value);
        }

        /// <summary>
        /// Converts a specified value of type $T value into a 16-bit short integer
        /// </summary>
        /// <param name="value">A generic of type $T to convert into a short integer</param>
        /// <returns>A short integer representation created from the specified value</returns>
        public short ToInt16($T value)
        {
            return Convert.ToInt16(value);
        }


        /// <summary>
        /// Converts the specified valud of type $T into a 32-bit integer
        /// </summary>
        /// <param name="value">a value of type $T to convert into a 32-bit integer</param>
        /// <returns>an 32-bit integer created from the specified value</returns>
        public int ToInt32($T value)
        {
            return value;
        }

        /// <summary>
        /// Converts a specified value of type $T into a 64-bit long integer
        /// </summary>
        /// <param name="value">the value of type $T to convert int a 64-bit long integer</param>
        /// <returns>A 64-bit long integer created from the specified value</returns>
        public long ToInt64($T value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// Converts the specified value of type $T value into a single-precision floating point decimal.
        /// </summary>
        /// <param name="value">A value of type $T to into a single precision floating point decimal</param>
        /// <returns>A single-precision floating point decimal created from the specified value.</returns>
        public float ToSingle($T value)
        {
            return Convert.ToSingle(value);
        }

        /// <summary>
        /// Converts the specified value of type float into a double-precision floating point decimal.
        /// </summary>
        /// <param name="value">The value of type float to convert into a double-precision floating point decimal</param>
        /// <returns>A double-precision floating point decimal created from the specified value</returns>
        public double ToDouble($T value)
        {
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Converts a specified value of type $T into an object
        /// </summary>
        /// <param name="value">The value of type $T to convert into an object</param>
        /// <returns>The specified value cast as an object.</returns>
        public object ToObject($T value)
        {
            return (object)value;
        }

        #endregion

        #endregion

    }
}
