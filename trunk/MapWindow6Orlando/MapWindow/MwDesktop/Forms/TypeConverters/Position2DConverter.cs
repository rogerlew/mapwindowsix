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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/1/2009 3:10:20 PM
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
using System.Drawing.Design;
using System.ComponentModel;
namespace MapWindow.Forms
{


    /// <summary>
    /// PointFConverter
    /// </summary>
    public class Position2DConverter : ExpandableObjectConverter
    {
    
        #region Constructors

        /// <summary>
        /// Creates a new instance of PointFConverter
        /// </summary>
        public Position2DConverter()
        {

        }

        #endregion

     
        #region Protected Methods

        /// <summary>
        /// True if the source type is a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns>Boolean, true if source type is a string</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Uses a string to return a new Position2D
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns>A String</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    string[] converterParts = s.Split(',');
                    double x = 0;
                    double y = 0;
                    if (converterParts.Length > 1)
                    {
                        x = double.Parse(converterParts[0].Trim());
                        y = double.Parse(converterParts[1].Trim());
                    }
                    else if (converterParts.Length == 1)
                    {
                        x = double.Parse(converterParts[0].Trim());
                        y = 0;
                    }
                    else
                    {
                        x = 0;
                        y = 0;
                    }
                    return new Position2D(x, y);
                }
                catch
                {
                    throw new ArgumentException("Cannot convert [" + value.ToString() + "] to pointF");
                }
            }
            return base.ConvertFrom(context, culture, value);

        }

        /// <summary>
        /// Creates a string from the specified Position2D
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns>A Position2D</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value.GetType() == typeof(Position2D))
                {
                    Position2D pt = (Position2D)value;
                    return string.Format("{0}, {1}", pt.X, pt.Y);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

    }
}
