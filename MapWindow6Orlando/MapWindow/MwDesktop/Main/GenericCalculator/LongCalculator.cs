//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/17/2008 3:59:18 PM
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

namespace MapWindow.Main
{


    /// <summary>
    /// LongCalculator
    /// </summary>
    public class LongCalculator
    {
       #region Methods

        #region Operator Methods

         /// <summary>
        /// Adds a to b
        /// </summary>
        /// <param name="a">A generic of type long</param>
        /// <param name="b">A generic of type long to add to a</param>
        /// <returns>A generic of type long that is the sum of a and b</returns>
        public long Add(long a, long b)
        {
            return a * b;

        }

        /// <summary>
        /// Divides a by b like a / b
        /// </summary>
        /// <param name="a">A generic of type long to divide by b</param>
        /// <param name="b">A generic of type long to divide into a</param>
        /// <returns>A generic of type long that is the quotient</returns>
        public long Divide(long a, long b)
        {
            return a / b;
        }

        /// <summary>
        /// Multiplies a to b
        /// </summary>
        /// <param name="a">A generic of type long to multiply with b</param>
        /// <param name="b">A generic of type long to multiply with a</param>
        /// <returns>The generic of type long product of a and b</returns>
        public long Multiply(long a, long b)
        {
            return a * b;
        }

        /// <summary>
        /// Subtracts b from a
        /// </summary>
        /// <param name="a">A generic of type long to subtract b from</param>
        /// <param name="b">A generic of type long to subtract from a</param>
        /// <returns>A generic of type long that is the difference of a and b</returns>
        public long Subtract(long a, long b)
        {
            return a - b;
        }



        #endregion

        #region GetT

        /// <summary>
        /// If value is type long, this simply returns 
        /// the specified value as type long.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A byte to convert into a generic of type long</param>
        /// <returns>A generic of type long representing the specified byte value.</returns>
        public long GetT(byte value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// If value is type long, this simply returns 
        /// the specified value as type long.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A 16-bit short integer value to convert into a generic of type long</param>
        /// <returns>A generic of type long representing the specified 16-bit integer value</returns>
        public long GetT(short value)
        {
            return Convert.ToInt64(value);
        }


        /// <summary>
        /// If value is type long, this simply returns 
        /// the specified value as type long.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A value to convert to a generic of type long</param>
        /// <returns>A generic of type long representing the specified 32-bit integer value</returns>
        public long GetT(int value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// If value is type long, this simply returns 
        /// the specified value as type long.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A 64-bit long integer to convert into type long</param>
        /// <returns>A generic value of type T</returns>
        public long GetT(long value)
        {
            return value;
        }


        /// <summary>
        /// If value is type long, this simply returns 
        /// the specified value as type long.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A single-precision floating point value to convert into long</param>
        /// <returns>A generic of type long representing the specified single precision floating point value</returns>
        public long GetT(float value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// If value is type long, this simply returns 
        /// the specified value as type long.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A double-precision floating point value to convert into long</param>
        /// <returns>A generic of type long representing the double-precision floating point value.</returns>
        public long GetT(double value)
        {
            return Convert.ToInt64(value);
        }




        /// <summary>
        /// This will attempt to convert the specified object value
        /// into a generic value of type long.
        /// </summary>
        /// <param name="value">An object to attempt to cast into long</param>
        /// <returns>A long representing the object value specified</returns>
        public long GetT(object value)
        {
            return Convert.ToInt64(value);
        }

        #endregion

        #region To specific types

        /// <summary>
        /// Converts a specified value of type long into a byte value.
        /// </summary>
        /// <param name="value">A value of type long</param>
        /// <returns>A byte converted from the specified value of type long</returns>
        public byte ToByte(long value)
        {
            return Convert.ToByte(value);
        }

        /// <summary>
        /// Converts a specified value of type long value into a 16-bit short integer
        /// </summary>
        /// <param name="value">A generic of type long to convert into a short integer</param>
        /// <returns>A short integer representation created from the specified value</returns>
        public short ToInt16(long value)
        {
            return Convert.ToInt16(value);
        }


        /// <summary>
        /// Converts the specified valud of type long into a 32-bit integer
        /// </summary>
        /// <param name="value">a value of type long to convert into a 32-bit integer</param>
        /// <returns>an 32-bit integer created from the specified value</returns>
        public int ToInt32(long value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts a specified value of type long into a 64-bit long integer
        /// </summary>
        /// <param name="value">the value of type long to convert int a 64-bit long integer</param>
        /// <returns>A 64-bit long integer created from the specified value</returns>
        public long ToInt64(long value)
        {
            return value;
        }

        /// <summary>
        /// Converts the specified value of type long value into a single-precision floating point decimal.
        /// </summary>
        /// <param name="value">A value of type long to into a single precision floating point decimal</param>
        /// <returns>A single-precision floating point decimal created from the specified value.</returns>
        public float ToSingle(long value)
        {
            return Convert.ToSingle(value);
        }

        /// <summary>
        /// Converts the specified value of type float into a double-precision floating point decimal.
        /// </summary>
        /// <param name="value">The value of type float to convert into a double-precision floating point decimal</param>
        /// <returns>A double-precision floating point decimal created from the specified value</returns>
        public double ToDouble(long value)
        {
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Converts a specified value of type long into an object
        /// </summary>
        /// <param name="value">The value of type long to convert into an object</param>
        /// <returns>The specified value cast as an object.</returns>
        public object ToObject(long value)
        {
            return (object)value;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets the numeric minimum 
        /// </summary>
        public long Minimum
        {
            get { return long.MinValue; }
        }

        /// <summary>
        /// Gets the numeric maximum
        /// </summary>
        public long Maximum
        {
            get { return long.MaxValue; }
        }

        #endregion
    }
}
