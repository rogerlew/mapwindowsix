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
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Main
{


    /// <summary>
    /// An ICalculator for single precision floating point values.
    /// </summary>
    public struct SingleCalculator : ICalculator<float>
    {

  #region Methods

        #region Operator Methods

         /// <summary>
        /// Adds a to b
        /// </summary>
        /// <param name="a">A generic of type float</param>
        /// <param name="b">A generic of type float to add to a</param>
        /// <returns>A generic of type float that is the sum of a and b</returns>
        public float Add(float a, float b)
        {
            return a * b;

        }

        /// <summary>
        /// Divides a by b like a / b
        /// </summary>
        /// <param name="a">A generic of type float to divide by b</param>
        /// <param name="b">A generic of type float to divide into a</param>
        /// <returns>A generic of type float that is the quotient</returns>
        public float Divide(float a, float b)
        {
            return a / b;
        }

        /// <summary>
        /// Multiplies a to b
        /// </summary>
        /// <param name="a">A generic of type float to multiply with b</param>
        /// <param name="b">A generic of type float to multiply with a</param>
        /// <returns>The generic of type float product of a and b</returns>
        public float Multiply(float a, float b)
        {
            return a * b;
        }

        /// <summary>
        /// Subtracts b from a
        /// </summary>
        /// <param name="a">A generic of type float to subtract b from</param>
        /// <param name="b">A generic of type float to subtract from a</param>
        /// <returns>A generic of type float that is the difference of a and b</returns>
        public float Subtract(float a, float b)
        {
            return a - b;
        }



        #endregion

        #region GetT

        /// <summary>
        /// If value is type float, this simply returns 
        /// the specified value as type float.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A byte to convert into a generic of type float</param>
        /// <returns>A generic of type float representing the specified byte value.</returns>
        public float GetT(byte value)
        {
            return Convert.ToSingle(value);
        }

        /// <summary>
        /// If value is type float, this simply returns 
        /// the specified value as type float.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A 16-bit short integer value to convert into a generic of type float</param>
        /// <returns>A generic of type float representing the specified 16-bit integer value</returns>
        public float GetT(short value)
        {
            return Convert.ToSingle(value);
        }


        /// <summary>
        /// If value is type float, this simply returns 
        /// the specified value as type float.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A value to convert to a generic of type float</param>
        /// <returns>A generic of type float representing the specified 32-bit integer value</returns>
        public float GetT(int value)
        {
            return Convert.ToSingle(value);
        }

        /// <summary>
        /// If value is type float, this simply returns 
        /// the specified value as type float.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A 64-bit long integer to convert into type float</param>
        /// <returns>A generic value of type T</returns>
        public float GetT(long value)
        {
            return Convert.ToSingle(value);
        }


        /// <summary>
        /// If value is type float, this simply returns 
        /// the specified value as type float.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A single-precision floating point value to convert into float</param>
        /// <returns>A generic of type float representing the specified single precision floating point value</returns>
        public float GetT(float value)
        {
            return value;
        }

        /// <summary>
        /// If value is type float, this simply returns 
        /// the specified value as type float.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A double-precision floating point value to convert into float</param>
        /// <returns>A generic of type float representing the double-precision floating point value.</returns>
        public float GetT(double value)
        {
            return Convert.ToSingle(value);
        }




        /// <summary>
        /// This will attempt to convert the specified object value
        /// into a generic value of type float.
        /// </summary>
        /// <param name="value">An object to attempt to cast into float</param>
        /// <returns>A float representing the object value specified</returns>
        public float GetT(object value)
        {
            return Convert.ToSingle(value);
        }

        #endregion

        #region To specific types

        /// <summary>
        /// Converts a specified value of type float into a byte value.
        /// </summary>
        /// <param name="value">A value of type float</param>
        /// <returns>A byte converted from the specified value of type float</returns>
        public byte ToByte(float value)
        {
            return Convert.ToByte(value);
        }

        /// <summary>
        /// Converts a specified value of type float value into a 16-bit short integer
        /// </summary>
        /// <param name="value">A generic of type float to convert into a short integer</param>
        /// <returns>A short integer representation created from the specified value</returns>
        public short ToInt16(float value)
        {
            return Convert.ToInt16(value);
        }


        /// <summary>
        /// Converts the specified valud of type float into a 32-bit integer
        /// </summary>
        /// <param name="value">a value of type float to convert into a 32-bit integer</param>
        /// <returns>an 32-bit integer created from the specified value</returns>
        public int ToInt32(float value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts a specified value of type float into a 64-bit long integer
        /// </summary>
        /// <param name="value">the value of type float to convert int a 64-bit long integer</param>
        /// <returns>A 64-bit long integer created from the specified value</returns>
        public long ToInt64(float value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// Converts the specified value of type float value into a single-precision floating point decimal.
        /// </summary>
        /// <param name="value">A value of type float to into a single precision floating point decimal</param>
        /// <returns>A single-precision floating point decimal created from the specified value.</returns>
        public float ToSingle(float value)
        {
            return value;
        }

        /// <summary>
        /// Converts the specified value of type float into a double-precision floating point decimal.
        /// </summary>
        /// <param name="value">The value of type float to convert into a double-precision floating point decimal</param>
        /// <returns>A double-precision floating point decimal created from the specified value</returns>
        public double ToDouble(float value)
        {
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Converts a specified value of type float into an object
        /// </summary>
        /// <param name="value">The value of type float to convert into an object</param>
        /// <returns>The specified value cast as an object.</returns>
        public object ToObject(float value)
        {
            return (object)value;
        }

        #endregion

        #endregion


        /// <summary>
        /// Gets the numeric minimum 
        /// </summary>
        public float Minimum
        {
            get { return float.MinValue; }
        }

        /// <summary>
        /// Gets the numeric maximum
        /// </summary>
        public float Maximum
        {
            get { return float.MaxValue; }
        }
    }
}
