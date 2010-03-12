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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/17/2008 1:39:02 PM
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
    /// ICalculator exists to allow mathematics with Generic types.  This only supports cases where
    /// a subclass calculator has been written for the specified type, however.  The fastest
    /// way to use one is to specify it in a second type parameter because it avoids unnecessary
    /// virtual message dispatching.
    /// </summary>
    public interface ICalculator<T>
    {
        #region Methods

        #region Operator Methods

        /// <summary>
        /// Adds a and b
        /// </summary>
        /// <param name="a">A value to add</param>
        /// <param name="b">the value to add to a</param>
        /// <returns></returns>
        T Add(T a, T b);

        /// <summary>
        /// Divides b into a like a/b
        /// </summary>
        /// <param name="a">The numerator of the division</param>
        /// <param name="b">The denominator of the divsion</param>
        /// <returns></returns>
        T Divide(T a, T b);

        /// <summary>
        /// Multiplies a with b
        /// </summary>
        /// <param name="a">A value to multiply</param>
        /// <param name="b">The second value to multiply</param>
        /// <returns>A product of type T</returns>
        T Multiply(T a, T b);


        /// <summary>
        /// Subtracts b from a
        /// </summary>
        /// <param name="a">The initial value to subtract from</param>
        /// <param name="b">The value to subtract from a</param>
        /// <returns></returns>
        T Subtract(T a, T b);

        #endregion

        #region GetT

        /// <summary>
        /// If value is the same type as T, this simply returns 
        /// the specified value as a T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T GetT(byte value);

        /// <summary>
        /// If value is the same type as T, this simply returns 
        /// the specified value as a T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T GetT(short value);

        /// <summary>
        /// If value is the same type as T, this simply returns 
        /// the specified value as a T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A value to convert to a generic type</param>
        /// <returns>A generic of type T</returns>
        T GetT(int value);

        /// <summary>
        /// Gets a generic T from the specified 64-bit long integer.
        /// If T is not long, this will apply System.Convert
        /// </summary>
        /// <param name="value">A 64-bit long integer</param>
        /// <returns>A generic value of type T</returns>
        T GetT(long value);

        /// <summary>
        /// If value is the same type as T, this simply returns 
        /// the specified value as a T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A double-precision floating point value</param>
        /// <returns>A generic value of type T</returns>
        T GetT(double value);

        /// <summary>
        /// If value is the same type as T, this simply returns 
        /// the specified value as a T.  Otherwise System.Convert
        /// will be used, and exceptions may be thrown in some cases.
        /// </summary>
        /// <param name="value">A single-precision floating point value</param>
        /// <returns>A generic value of type T</returns>
        T GetT(float value);

        /// <summary>
        /// This will attempt to convert the specified object value
        /// into the the type for this calculator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T GetT(object value);

        #endregion

        #region Convert

        /// <summary>
        /// Converts the specified value into a double.  System.Convert
        /// will be used if the type is not correct, otherwise the value
        /// is simply passed through.
        /// </summary>
        /// <param name="value">The value to convert into a double</param>
        /// <returns>A double value created from the value parameter</returns>
        double ToDouble(T value);

        /// <summary>
        /// Converts the specified value into an Integer
        /// </summary>
        /// <param name="value">a value of type T</param>
        /// <returns>an Integer created from value </returns>
        int ToInt32(T value);

        /// <summary>
        /// Converts the specified value into a float
        /// </summary>
        /// <param name="value">A value to convert into a Float</param>
        /// <returns>A float value</returns>
        float ToSingle(T value);

        /// <summary>
        /// Converts a specified value int a byte
        /// </summary>
        /// <param name="value">A value of type T</param>
        /// <returns>A byte</returns>
        byte ToByte(T value);

        /// <summary>
        /// Converts a specified value into a 16-bit short integer
        /// </summary>
        /// <param name="value">A value to convert into a 16-bit short integer</param>
        /// <returns>A 16-bit short integer </returns>
        short ToInt16(T value);

        /// <summary>
        /// Converts a specified value into a 64-bit long integer
        /// </summary>
        /// <param name="value">the value to convert int a 64-bit long integer</param>
        /// <returns>A 64-bit long integer</returns>
        long ToInt64(T value);

        /// <summary>
        /// Converts a specified value into an Object
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>An object version of T</returns>
        object ToObject(T value);

        #endregion

        #endregion

        /// <summary>
        /// Gets the minimum value for the T type
        /// </summary>
        T Minimum
        {
            get;
        }

        /// <summary>
        /// Gets the maximum value for the T type
        /// </summary>
        T Maximum
        {
            get;
        }
    }
}
