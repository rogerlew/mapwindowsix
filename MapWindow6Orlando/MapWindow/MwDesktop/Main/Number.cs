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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/3/2008 5:21:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Text;
using System;
namespace MapWindow.Main
{


    /// <summary>
    /// NumberFormat
    /// </summary>
    public struct Number : IComparable, IComparable<Number>, IComparable<double>
    {

        #region Private methods

        private int _decimalCount;
        private int _significantFigures;
        private NumberFormats _format;   
        private double _value;
        private bool _hasValue;

        #endregion



        /// <summary>
        /// Creates a value from an object
        /// </summary>
        /// <param name="value">A numeric value that is, or can be parsed to a numeric value.</param>
        /// <exception cref="MapWindow.Main.NonNumericLogException">Not Numeric</exception>
        public Number(object value)
        {
            if (Global.IsShort(value))
            {
                _value = Global.GetDouble(value);
                _significantFigures = 5;
                _decimalCount = 0;
                _format = NumberFormats.General;
                _hasValue = true;
                return;
            }
            else if (Global.IsInteger(value))
            {
                _value = Global.GetDouble(value);
                _significantFigures = 10;
                _decimalCount = 0;
                _format = NumberFormats.General;
                _hasValue = true;
                return;
            }
            else if (Global.IsFloat(value))
            {
                _value = Global.GetDouble(value);
                _significantFigures = 8;
                _decimalCount = 7;
                _format = NumberFormats.General;
                _hasValue = true;
                return;
            }
            else if (Global.IsDouble(value))
            {
                // doubles can have 16 digits, so in scientific notation
                _value = Global.GetDouble(value);
                _significantFigures = 16;
                _decimalCount = 15;
                _format = NumberFormats.General;
                _hasValue = true;
                return;
            }
            throw new MapWindow.Main.NonNumericLogException("value");
        }

        /// <summary>
        /// Creates a number to fit the specified double value.
        /// </summary>
        /// <param name="value">A double</param>
        public Number(double value)
        {
            // doubles can have 16 digits, so in scientific notation
            _value = value;
            _significantFigures = 16;
            _decimalCount = 15;
            _format = NumberFormats.General;
            _hasValue = true;
        }

        /// <summary>
        /// Creates a number to fit the specified int value
        /// </summary>
        /// <param name="value">An integer</param>
        public Number(int value)
        {
            _value = value;
            _significantFigures = 10;
            _decimalCount = 0;
            _format = NumberFormats.General;
            _hasValue = true;
        }

        /// <summary>
        /// Creates a number to fit the specified float value
        /// </summary>
        /// <param name="value">The value to work with</param>
        public Number(float value)
        {
            _value = value;
            _significantFigures = 8;
            _decimalCount = 7;
            _format = NumberFormats.General;
            _hasValue = true;
        }

        /// <summary>
        /// Creates a number from a short
        /// </summary>
        /// <param name="value">A short</param>
        public Number(short value)
        {
            _value = value;
            _significantFigures = 5;
            _decimalCount = 0;
            _format = NumberFormats.General;
            _hasValue = true;
        }

        /// <summary>
        /// Gets the alphabetical letter code for ToString(-Code-)
        /// </summary>
        public string Code
        {
            get
            {
                switch (_format)
                {
                    case NumberFormats.Currency: return "C";
                    case NumberFormats.Exponential: return "E";
                    case NumberFormats.FixedPoint: return "F";
                    case NumberFormats.General: return "G";
                    case NumberFormats.Number: return "N";
                    case NumberFormats.Percent: return "P";
                    case NumberFormats.Unspecified: return "G";
                }
                return "G";
            }

        }

        /// <summary>
        /// A NumberFormats enumeration giving the various formatting options
        /// </summary>
        public NumberFormats Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Tests to see if a value has been assigned to this Number
        /// </summary>
        public bool IsEmpty
        {
            get { return !_hasValue; }
        }

        /// <summary>
        /// Gets or sets the number of digits that folow the decimal 
        /// when converting this value to string notation.
        /// </summary>
        public int DecimalsCount
        {
            get { return _decimalCount; }
            set { _decimalCount = value; }
        }

        /// <summary>
        /// Gets the precision (determined by the data type) which
        /// effectively describes how many significant figures there are.
        /// </summary>
        public int SignificantFigures
        {
            get { return _significantFigures; }
        }

        /// <summary>
        /// Gets this number as a double
        /// </summary>
        /// <returns>a double</returns>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _hasValue = true;
            }
        }

       

       

        /// <summary>
        /// Returns this Number as a string.
        /// </summary>
        /// <returns>The string created using the specified number format and precision.</returns>
        public override string ToString()
        {
            return _value.ToString(Code + DecimalsCount.ToString(), MapWindow.Globalization.CulturePreferences.CultureInformation.NumberFormat);
        }

        /// <summary>
        /// Returns this Number as a string.
        /// </summary>
        /// <param name="provider">An IFormatProvider that provides culture specific formatting information.</param>
        /// <returns>A string with the formatted number.</returns>
        public string ToString(IFormatProvider provider)
        {
            return _value.ToString(Code + DecimalsCount.ToString(), provider);
        }

        /// <summary>
        /// Returns this Number as a string.
        /// </summary>
        /// <param name="format">A string that controls how this value should be formatted.</param>
        /// <returns>A string with the formatted number.</returns>
        public string ToString(string format)
        {
            return _value.ToString(format, MapWindow.Globalization.CulturePreferences.CultureInformation.NumberFormat);
        }

        /// <summary>
        ///  Returns this Number as a string.
        /// </summary>
        /// <param name="format">A string that controls how this value should be formatted.</param>
        /// <param name="provider">An IFormatProvider that provides culture specific formatting information.</param>
        /// <returns>A string with the formatted number.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return _value.ToString(format, provider);
        }


        /// <summary>
        /// Gets this number as an int 32
        /// </summary>
        /// <returns>An integer</returns>
        public int ToInt32()
        {
            if (_value > int.MaxValue) return int.MaxValue;
            if (_value < int.MinValue) return int.MinValue;
            return Convert.ToInt32(_value);
        }

        /// <summary>
        /// Gets this number as a short, or a short.MaxValue/short.MinValue
        /// </summary>
        /// <returns>A short</returns>
        public short ToInt16()
        {
            if (_value > short.MaxValue) return short.MaxValue;
            if (_value < short.MinValue) return short.MinValue;
            return Convert.ToInt16(_value);
        }

        /// <summary>
        /// Gets this number as a float
        /// </summary>
        /// <returns>A float </returns>
        public float ToFloat()
        {
            if (_value > float.MaxValue) return float.MaxValue;
            if (_value > float.MinValue) return float.MinValue;
            return Convert.ToSingle(_value);
        }

       


        /// <summary>
        /// Compares this with the specified value.  Returns 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value.
        /// </summary>
        /// <param name="other">The value to be tested.</param>
        /// <returns>An integer: 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value. </returns>
        public int CompareTo(object other)
        {
            if (Global.IsDouble(other) == false)
            {
                throw new MapWindow.Main.NonNumericLogException("other");
            }
            return _value.CompareTo(Global.GetDouble(other));
        }


        /// <summary>
        /// Compares this with the specified value.  Returns 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value.
        /// </summary>
        /// <param name="other">The value to be tested.</param>
        /// <returns>An integer: 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value. </returns>
        public int CompareTo(double other)
        {
            return _value.CompareTo(other);
        }


        /// <summary>
        /// Compares this with the specified value.  Returns 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value.
        /// </summary>
        /// <param name="other">The value to be tested.</param>
        /// <returns>An integer: 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value. </returns>
        public int CompareTo(Number other)
        {
            return _value.CompareTo(other.Value);
        }

    }
}
