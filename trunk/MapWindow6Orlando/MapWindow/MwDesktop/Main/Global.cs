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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 8:49:44 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using MapWindow.Drawing;

namespace MapWindow.Main
{


    /// <summary>
    /// Global has some basic methods that may be useful in lots of places.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Gets a cool Highlight brush for highlighting things
        /// </summary>
        /// <param name="box">The rectangle in the box</param>
        /// <param name="selectionHighlight">The color to use for the higlight</param>
        /// <returns></returns>
        public static Brush HighlightBrush(Rectangle box, Color selectionHighlight)
        {
            float med = selectionHighlight.GetBrightness();
            float bright = med + 0.05f;
            if (bright > 1f) bright = 1f;
            float dark = med - 0.05f;
            if (dark < 0f) dark = 0f;
            Color brtCol = ColorFromHSL(selectionHighlight.GetHue(), selectionHighlight.GetSaturation(), bright);
            Color drkCol = ColorFromHSL(selectionHighlight.GetHue(), selectionHighlight.GetSaturation(), dark);
            return new System.Drawing.Drawing2D.LinearGradientBrush(box, brtCol, drkCol, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
        }

        /// <summary>
        /// Draws a rectangle with ever so slightly rounded edges.  Good for selection borders.
        /// </summary>
        /// <param name="g">The Graphics object</param>
        /// <param name="pen">The pen to draw with</param>
        /// <param name="rect">The rectangle to draw to.</param>
        public static void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle rect)
        {
            int l = rect.Left;
            int r = rect.Right;
            int t = rect.Top;
            int b = rect.Bottom;
            g.DrawLine(pen, l + 1, t, r - 1, t);
            g.DrawLine(pen, l + 1, b, r - 1, b);
            g.DrawLine(pen, l, t + 1, l, b - 1);
            g.DrawLine(pen, r, t + 1, r, b - 1);

            g.DrawLine(pen, l, t + 2, l + 2, t);
            g.DrawLine(pen, r - 2, t, r, t + 2);
            g.DrawLine(pen, l, b - 2, l + 2, b);
            g.DrawLine(pen, r, b - 2, r - 2, b);
        }

        /// <summary>
        /// Converts a double numeric value into the appropriate T data type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertT<T>(double value)
        {
            T test = default(T);
            if (test is byte)
            {
                return SafeCastTo<T>(Convert.ToByte(value));
            }
            if (test is short)
            {
                return SafeCastTo<T>(Convert.ToInt16(value));
            }
            if (test is int)
            {
                return SafeCastTo<T>(Convert.ToInt32(value));
            }
            if (test is long)
            {
                return SafeCastTo<T>(Convert.ToInt64(value));
            }

            if (test is float)
            {
                return SafeCastTo<T>(Convert.ToSingle(value));
            }
            if (test is double)
            {
                return SafeCastTo<T>(Convert.ToDouble(value));
            }

            if (test is UInt16)
            {
                return SafeCastTo<T>(Convert.ToUInt16(value));
            }
            if (test is UInt32)
            {
                return SafeCastTo<T>(Convert.ToUInt32(value));
            }
            if (test is UInt64)
            {
                return SafeCastTo<T>(Convert.ToInt64(value));
            }

            return default(T);
            
        }
        
        /// <summary>
        /// Gets the type of a numeric veriable
        /// </summary>
        /// <typeparam name="T">The type to test against.</typeparam>
        /// <param name="variable">A variable to test (this can be default(T) for instance</param>
        /// <returns>The datatype</returns>
        public static Type GetNumericType<T>(T variable)
        {
            object test = variable;
            if (test is byte)
            {
                return typeof (byte);
            }
            if (test is short)
            {
                return typeof(short);
            }
            if (test is int)
            {
                return typeof(int);
            }
            if (test is long)
            {
                return typeof(long);
            }

            if (test is float)
            {
                return typeof(float);
            }
            if (test is double)
            {
                return typeof(double);
            }

            if (test is UInt16)
            {
                return typeof(UInt16);
            }
            if (test is UInt32)
            {
                return typeof(UInt32);
            }
            if (test is UInt64)
            {
                return typeof(UInt64);
            }
            throw new ArgumentException("The specified value was not a standard numeric format");
        }

        /// <summary>
        /// For Numeric types, this will 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MaximumValue<T>()
        {
            T value = default(T);
            if (value is byte)
            {
                return SafeCastTo<T>(byte.MaxValue);
            }
            if (value is short)
            {
                return SafeCastTo<T>(short.MaxValue);
            }
            if (value is int)
            {
                return SafeCastTo<T>(int.MaxValue);
            }
            if (value is long)
            {
                return SafeCastTo<T>(long.MaxValue);
            }

            if (value is float)
            {
                return SafeCastTo<T>(float.MaxValue);
            }
            if (value is double)
            {
                return SafeCastTo<T>(double.MaxValue);
            }
            
            if (value is UInt16)
            {
                return SafeCastTo<T>(UInt16.MaxValue);
            }
            if (value is UInt32)
            {
                return SafeCastTo<T>(UInt32.MaxValue);
            }
            if (value is UInt64)
            {
                return SafeCastTo<T>(UInt64.MaxValue);
            }
            
            return default(T);
        }


        /// <summary>
        /// For Numeric types, this will 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MinimumValue<T>()
        {
            T value = default(T);
            if (value is byte)
            {
                return SafeCastTo<T>(byte.MinValue);
            }
            if (value is short)
            {
                return SafeCastTo<T>(short.MinValue);
            }
            if (value is int)
            {
                return SafeCastTo<T>(int.MinValue);
            }
            if (value is long)
            {
                return SafeCastTo<T>(long.MinValue);
            }

            if (value is float)
            {
                return SafeCastTo<T>(float.MinValue);
            }
            if (value is double)
            {
                return SafeCastTo<T>(double.MinValue);
            }

            if (value is UInt16)
            {
                return SafeCastTo<T>(UInt16.MinValue);
            }
            if (value is UInt32)
            {
                return SafeCastTo<T>(UInt32.MinValue);
            }
            if (value is UInt64)
            {
                return SafeCastTo<T>(UInt64.MinValue);
            }

            return default(T);
        }


        /// <summary>
        /// A static variable to access any and all frames in the project
        /// </summary>
        public static List<IFrame> ProjectMapFrames = new List<IFrame>();

        /// <summary>
        /// This attempts to convert a value into a byte.  If it fails, the byte will be 0.
        /// </summary>
        /// <param name="Expression">The expression (like a string) to convert.</param>
        /// <returns>A byte that is 0 if the test fails.</returns>
        public static byte GetByte(object Expression)
        {
            byte retNum;
            if (Byte.TryParse(GetString(Expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum))
            {
                return retNum;
            }
            return 0;
        }

        /// <summary>
        /// This attempts to convert a value into a double.  If it fails, the double will be double.NaN.
        /// </summary>
        /// <param name="expression">The expression (like a string) to convert.</param>
        /// <returns>A double that is double.NAN if the test fails.</returns>
        public static double GetDouble(object expression)
        {
            double retNum;
            return Double.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : double.NaN;
        }

        /// <summary>
        /// This attempts to convert a value into a float.  If it fails, the float will be 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to convert.</param>
        /// <returns>A float that is 0 if the test fails.</returns>
        public static float GetFloat(object expression)
        {
            float retNum;
            return Single.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : 0F;
        }

        
        /// <summary>
        /// This attempts to convert a value into an integer.  If it fails, it returns 0.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static int GetInteger(object expression)
        {
            int retNum;
            return Int32.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : 0;
        }

        /// <summary>
        /// Obtains a system.Drawing.Rectangle based on the two points, using them as 
        /// opposite extremes for the rectangle.
        /// </summary>
        /// <param name="a">one corner point of the rectangle.</param>
        /// <param name="b">The opposing corner of the rectangle.</param>
        /// <returns>A System.Draing.Rectangle</returns>
        public static Rectangle GetRectangle(Point a, Point b)
        {
            int x = Math.Min(a.X, b.X);
            int y = Math.Min(a.Y, b.Y);
            int w = Math.Abs(a.X - b.X);
            int h = Math.Abs(a.Y - b.Y);
            return new Rectangle(x, y, w, h);
        }


        /// <summary>
        /// This attempts to convert a value into a short.  If it fails, it returns 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to convert.</param>
        /// <returns>A short that is 0 if the test fails.</returns>
        public static double GetShort(object expression)
        {
            short retNum;
            if (Int16.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum))
            {
                return retNum;
            }
            return 0;
        }

        /// <summary>
        /// Gets the string form of the number using culture settings
        /// </summary>
        /// <param name="Expression">The expression to obtain the string for</param>
        /// <returns>A string</returns>
        public static string GetString(object Expression)
        {
            return Convert.ToString(Expression, Globalization.CulturePreferences.CultureInformation.NumberFormat);
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a byte.
        /// </summary>
        /// <param name="Expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsByte(object Expression)
        {
            byte retNum;
            bool isNum = Byte.TryParse(GetString(Expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a double.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsDouble(object expression)
        {
            double retNum;
   
            bool isNum = Double.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a float.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsFloat(object expression)
        {
            float retNum;
            bool isNum = Single.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into an integer.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as an integer, false otherwise</returns>
        public static bool IsInteger(object expression)
        {
            int retNum;
            bool isNum = Int32.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a short.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsShort(object expression)
        {
            short retNum;
            bool isNum = Int16.TryParse(GetString(expression), System.Globalization.NumberStyles.Any, Globalization.CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// An instance of Random that is created when needed and sits around so we don't keep creating new ones.
        /// </summary>
        private static readonly Random DefaultRandom = new Random();

        /// <summary>
        /// Returns a completely random opaque color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color RandomColor()
        {
            return Color.FromArgb(DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Color</returns>
        public static Color RandomTranslucent(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Color</returns>
        public static Color RandomLightColor(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, ColorFromHSL(DefaultRandom.Next(0, 360), ((double)DefaultRandom.Next(0, 255) / 256), ((double)DefaultRandom.Next(123, 255) / 256)));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Color</returns>
        public static Color RandomDarkColor(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, ColorFromHSL(DefaultRandom.Next(0, 360), ((double)DefaultRandom.Next(0, 255) / 256), ((double)DefaultRandom.Next(0, 123) / 256)));
        }

      


        
        

        ///  <summary>
        /// Converts a colour from HSL to RGB
        /// </summary>
        /// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
        /// <param name="hue">A double representing degrees ranging from 0 to 360 and is equal to the GetHue() on a Color structure.</param>
        /// <param name="brightness">A double value ranging from 0 to 1, where 0 is black and 1 is white.</param>
        /// <param name="saturation">A double value ranging from 0 to 1, where 0 is gray and 1 is fully saturated with color.</param>
        /// <returns>A Color structure with the equivalent hue saturation and brightness</returns>
        public static Color ColorFromHSL(double hue, double saturation, double brightness)
        {
            double normalizedHue = hue / 360;

            double red, green, blue;

            if (brightness == 0)
            {
                red = green = blue = 0;
            }
            else
            {
                if (saturation == 0)
                {
                    red = green = blue = brightness;
                }
                else
                {
                    double temp2;
                    if (brightness <= 0.5)
                    {
                        temp2 = brightness * (1.0 + saturation);
                    }
                    else
                    {
                        temp2 = brightness + saturation - (brightness * saturation);
                    }
                    
                    double temp1 = 2.0 * brightness - temp2;

                    double[] temp3 = new[] { normalizedHue + 1.0 / 3.0, normalizedHue, normalizedHue - 1.0 / 3.0 };
                    double[] color = new double[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (temp3[i] < 0) temp3[i] += 1.0;

                        if (temp3[i] > 1) temp3[i] -= 1.0;

                        if (6.0 * temp3[i] < 1.0)
                        {
                            color[i] = temp1 + (temp2 - temp1) * temp3[i] * 6.0;
                        }
                        else if (2.0 * temp3[i] < 1.0)
                        {
                            color[i] = temp2;
                        }
                        else if (3.0 * temp3[i] < 2.0)
                        {
                            color[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - temp3[i]) * 6.0);
                        }
                        else
                        {
                            color[i] = temp1;
                        }
                    }

                    red = color[0];
                    green = color[1];
                    blue = color[2];

                }

            }
            if (red > 1) red = 1;
            if (red < 0) red = 0;
            if (green > 1) green = 1;
            if (green < 0) green = 0;
            if (blue > 1) blue = 1;
            if (blue < 0) blue = 0;
            return Color.FromArgb((int)(255 * red), (int)(255 * green), (int)(255 * blue));

        }

        /// <summary>
        /// A Generic Safe Casting method that should simply exist as part of the core framework
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T SafeCastTo<T>(object obj)
        {
            if (obj == null)
            {
                return default(T);
            }
            if (!(obj is T))
            {
                return default(T);
            }
            return (T)obj;
        }

        /// <summary>
        /// Uses the standard enum parsing, but returns it cast as the specified T parameter
        /// </summary>
        /// <typeparam name="T">The type of the enum to use</typeparam>
        /// <param name="text">The string to parse into a copy of the enumeration</param>
        /// <returns>an enumeration of the specified type</returns>
        public static T ParseEnum<T>(string text)
        {
            return SafeCastTo<T>(Enum.Parse(typeof(T), text));
        }

    }
}
