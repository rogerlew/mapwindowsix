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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 10:24:02 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// Category
    /// </summary>
    public class Category : LegendItem
    {
        #region Private Variables

        object _tag;
        private Range _range;
        private string _status;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Category
        /// </summary>
        public Category()
        {
            
        }

        /// <summary>
        /// Creaates a new instance of this category, and tailors the range to the specifeid values.
        /// </summary>
        /// <param name="startValue">The start value</param>
        /// <param name="endValue">The end value</param>
        public Category(double? startValue, double? endValue)
        {
            _range = new Range(startValue, endValue);
        }
        /// <summary>
        /// Creates a category that has the same value for both minimum and maximum
        /// </summary>
        /// <param name="value">The value to use</param>
        public Category(double value)
        {
            _range = new Range(value, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the snapping rule directly to the categories, instead of the breaks.
        /// </summary>
        public void ApplySnapping(IntervalSnapMethods method, int numDigits, List<double> values)
        {
            switch (method)
            {
                case IntervalSnapMethods.None:
                    break;
                case IntervalSnapMethods.SignificantFigures:
                    if (Maximum != null)
                    {
                        int digits = numDigits;
                        double max = (double)Maximum;
                        int md = (int)Math.Ceiling(Math.Log10(max));
                        md -= digits;
                        double norm = Math.Pow(10, md);
                        double val = (double)Maximum;
                        Maximum = norm * Math.Round(val / norm);
                    }
                    if (Minimum != null)
                    {
                        int digits = numDigits;
                        double min = (double)Minimum;
                        int md = (int)Math.Ceiling(Math.Log10(min));
                        md -= digits;
                        double norm = Math.Pow(10, md);
                        double val = (double)Minimum;
                        Minimum = norm * Math.Round(val / norm);
                    }
                    break;
                case IntervalSnapMethods.Rounding:
                    if (Maximum != null)
                    {
                        Maximum = Math.Round((double)Maximum, numDigits);
                    }
                    if (Minimum != null)
                    {
                        Minimum = Math.Round((double)Minimum, numDigits);
                    }
                    break;
                case IntervalSnapMethods.DataValue:
                    if (Maximum != null)
                    {
                        Maximum = NearestValue((double)Maximum, values);
                    }
                    if (Minimum != null)
                    {
                        Minimum = NearestValue((double)Minimum, values);
                    }
                    break;
            }
        }

        /// <summary>
        /// Searches the list and returns the nearest value in the list to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static double NearestValue(double value, List<double> values)
        {
            if (values == null || values.Count == 0) return 0;
            int indx = values.BinarySearch(value);
            if (indx >= 0)
            {
                return values[indx];
            }
            int iHigh = -indx;
            if (iHigh >= 0 && iHigh < values.Count)
            {
                double high = values[iHigh];
                int iLow = -indx - 1;
                if (iLow >= 0 && iLow < values.Count && iLow != iHigh)
                {
                    double low = values[iLow];
                    return value - low < high - value ? low : high;
                }
            }
            int iLow2 = -indx - 1;
            if (iLow2 >= 0 && iLow2 < values.Count)
            {
                return values[iLow2];
            }
            return 0;
        }

        /// <summary>
        /// Since rasters are numeric and not relying on an SQL expression, this allows
        /// this only sets the legend text using the method and digits to help with
        /// formatting.
        /// </summary>
        /// <param name="settings">An EditorSettings from either a feature scheme or color scheme.</param>
        public virtual void ApplyMinMax(EditorSettings settings)
        {
            LegendText = Range.ToString(settings.IntervalSnapMethod, settings.IntervalRoundingDigits);
        }

        /// <summary>
        /// Tests to see if the specified value falls in the range specified by this ColorCategory
        /// </summary>
        /// <param name="value">The value of type int to test</param>
        /// <returns>Boolean, true if the value was found in the range</returns>
        public bool Contains(double value)
        {
            return _range == null || _range.Contains(value);
        }

        /// <summary>
        /// Returns this Number as a string.  This uses the MapWindow.Globalization.CulturePreferences and
        /// Controls the number type using the NumberFormat enumeration plus the DecimalCount to create
        /// a number format.
        /// </summary>
        /// <returns>The string created using the specified number format and precision.</returns>
        public override string ToString()
        {
            return _range.ToString();
        }

        /// <summary>
        ///  Returns this Number as a string.
        /// </summary>
        /// <param name="method">Specifies how the numbers are modified so that the numeric text can be cleaned up.</param>
        /// <param name="digits">An integer clarifying digits for rounding or significant figure situations.</param>
        /// <returns>A string with the formatted number.</returns>
        public virtual string ToString(IntervalSnapMethods method, int digits)
        {
            return _range.ToString(method, digits);
        }

        #endregion

        #region Properties



        /// <summary>
        /// Maximum this is a convenient caching tool only, and doesn't control the filter expression at all.
        /// Use ApplyMinMax after setting this to update the filter expression.
        /// </summary>
        [Description("Gets or sets the maximum value for this category using the scheme field.")]
        [Serialize("Maximum")]
        public double? Maximum
        {
            get
            {
                return _range != null ? _range.Maximum : null;
            }
            set
            {
                if (_range == null)
                {
                    _range = new Range(null, value);
                    return;
                }
                _range.Maximum = value;

            }
        }


        /// <summary>
        /// Gets or sets the color to be used for this break.  For
        /// BiValued breaks, this only sets one of the colors.  If
        /// this is higher than the high value, both are set to this.
        /// If this equals the high value, IsBiValue will be false.
        /// </summary>
        [Description("Gets or sets a minimum value for this category using the scheme field.")]
        [Serialize("Minimum")]
        public double? Minimum
        {
            get
            {
                return _range != null ? _range.Minimum : null;
            }
            set
            {
                if (_range == null)
                {
                    _range = new Range(value, null);
                    return;
                }
                _range.Minimum = value;
            }
        }


        /// <summary>
        /// Gets the numeric Range for this color break.
        /// </summary>
        public Range Range
        {
            get { return _range; }
            set { _range = value; }
        }

        /// <summary>
        /// Gets or sets a status message for this string.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }


        /// <summary>
        /// This is not used by MapWindow, but is provided for convenient linking for this object
        /// in plugins or other applications.
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
       

        #endregion



    }
}
