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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/29/2008 8:44:01 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using MapWindow.Main;
namespace MapWindow.Drawing
{


    /// <summary>
    /// ColorRampBuilder
    /// </summary>
    public class ColorRampBuilder : IColorRampBuilder
    {
        #region Events

        /// <summary>
        /// Occurs when the symbology has been changed
        /// </summary>
        public event EventHandler SymbologyChanged;

        /// <summary>
        /// Occurs if any of the properties that would contribute to bitmap construction have changed
        /// </summary>
        public event EventHandler ColorSchemeChanged;

        #endregion


        #region Private Variables

        private IColorBreakList _colorBreaks;
        private ColorSchemes _colorScheme;
        private bool _symbologyHasChanged;
        private readonly double _maximum;
        private readonly double _minimum;
        private float _opacity;
        private GradientModels _gradientModel;
        private NumberFormats _numberFormat;
        private int _decimalCount;
        private bool _colorSchemeHasChanged;
        private ILegendItem _parent;
        
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates this item as a base class to be fleshed out later.
        /// </summary>
        protected ColorRampBuilder()
        {
            _colorBreaks = new ColorBreakList();
            Configure();
        }

        /// <summary>
        /// This can be used by derivative classes to create a new, empty instance of a ColorRampBuilder
        /// where Minimum and Maximum are overridden.
        /// </summary>
        protected ColorRampBuilder(ILegendItem parent)
        {
            _parent = parent;
            _colorBreaks = new ColorBreakList(parent);
            Configure();
        }

       

        /// <summary>
        /// Creates a new instance of ColorRampBuilder
        /// </summary>
        /// <param name="maximum">THe maximum value</param>
        /// <param name="minimum">The minimum value</param>
        public ColorRampBuilder(double minimum, double maximum)
        {
            _colorBreaks = new ColorBreakList();
            _minimum = minimum;
            _maximum = maximum;
            Configure();
        }

        /// <summary>
        /// Creates a new instance of ColorRampBuilder
        /// </summary>
        /// <param name="minimum">The minimum value</param>
        /// <param name="maximum">The maximum value</param>
        /// <param name="parent">The legend item to use as a parent</param>
        public ColorRampBuilder(double minimum, double maximum, ILegendItem parent)
        {
            _parent = parent;
            _colorBreaks = new ColorBreakList(parent);
            _minimum = minimum;
            _maximum = maximum;
            Configure();
        }



        private void Configure()
        {
            _opacity = 1;
            _gradientModel = GradientModels.Linear;
            _colorBreaks.ItemChanged += _colorBreaks_ItemChanged;
            ColorScheme = ColorSchemes.FallLeaves;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This creates a colorbreak with the specified range, but all the other
        /// characteristics of this ColorRampBuilder, like NumberFormat and Decimal Count and Gradient style.
        /// </summary>
        /// <param name="value">The single value to use when creating the new colorbreak.</param>
        public virtual IRasterCategory AddColorBreak(IComparable value)
        {
            IRasterCategory cb = new RasterCategory();
            cb.LowValue = value;
            cb.HighValue = value;
            cb.LowColor = Color.DarkGray;
            cb.HighColor = Color.WhiteSmoke;
            cb.GradientModel = _gradientModel;
            cb.NumberFormat = _numberFormat;
            cb.DecimalCount = _decimalCount;
            _colorBreaks.Add(cb);
            OnColorSchemeChanged();
            OnSymbologyChange();
            return cb;
        }

        /// <summary>
        /// This creates a colorbreak with the specified range, but all the other
        /// characteristics of this ColorRampBuilder, like NumberFormat and Decimal Count and Gradient style.
        /// </summary>
        /// <param name="startValue">The start value.  If this is lower than endValue, it will become the low value, otherwise, it will become the high value.</param>
        /// <param name="endValue">The end Value.  If this is lower than startValue, it will become the low value, otherwise, it will become the high value.</param>
        public virtual IRasterCategory AddColorBreak(IComparable startValue, IComparable endValue)
        {
            IRasterCategory cb = new RasterCategory(startValue, endValue);
            cb.LowColor = Color.DarkGray;
            cb.HighColor = Color.WhiteSmoke;
            cb.GradientModel = _gradientModel;
            cb.NumberFormat = _numberFormat;
            cb.DecimalCount = _decimalCount;
            cb.LegendText = cb.LowValue + " - " + cb.HighValue;
            _colorBreaks.Add(cb);
            OnColorSchemeChanged();
            OnSymbologyChange();
            return cb;

        }

        /// <summary>
        /// This creates a deep clone of the member because the typical case wants a "backup" of the
        /// colorscheme.  
        /// </summary>
        public object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// This creates a deep clone of the member because the typical case wants a "backup" of the
        /// colorscheme.  
        /// </summary>
        /// <returns>An IColorRampBuilder with a duplicate set of colorbreaks, maxima, minima etc.</returns>
        public IColorRampBuilder Copy()
        {
            ColorRampBuilder clone = (ColorRampBuilder)MemberwiseClone();
            clone.ColorBreaks = new ColorBreakList();
            foreach (IRasterCategory cb in _colorBreaks)
            {
                clone.ColorBreaks.Add(cb.Copy());
            }
            return clone;
        }

        /// <summary>
        /// Creates a hue ramp.  The specified fromColor provides the saturation and lightness
        /// and starting/ending hue to use.
        /// </summary>
        /// <param name="numBreaks">The integer number of breaks to included in the hue ramp.</param>
        /// <param name="fromColor">The color to provide the lightness and saturation for the ramp.</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        /// <param name="isAscending">If this is true, increasing values get increasing hue</param>
        public virtual void CreateHueRamp(int numBreaks, Color fromColor, bool isContinuous, bool isAscending)
        {
            double step = (Maximum - Minimum) / numBreaks;
            double hueStep = (double)360 / numBreaks;
            double startHue = fromColor.GetHue();
            double saturation = fromColor.GetSaturation();
            double brightness = fromColor.GetBrightness();
            int alpha = ByteRange(_opacity * 255);
            _colorBreaks.Clear();
            for (int brk = 0; brk < numBreaks; brk++)
            {
                IRasterCategory cb = AddColorBreak(Minimum + step * brk, Minimum + step * (brk + 1));
                double leftHue;
                double midHue;
                double rightHue;
                if (isAscending)
                {
                    leftHue = (startHue + (hueStep * brk)) % 360;
                    midHue = (startHue + hueStep * (brk + 0.5)) % 360;
                    rightHue = (startHue + hueStep * (brk + 1)) % 360;
                }
                else
                {
                    leftHue = (startHue - (hueStep * brk));
                    if (leftHue < 0) leftHue += 360;
                    midHue = (startHue - hueStep * (brk - 0.5));
                    if (midHue < 0) midHue += 360;
                    rightHue = (startHue - hueStep * (brk - 1));
                    if (rightHue < 0) rightHue += 360;
                }
                if (isContinuous)
                {
                    cb.LowColor = Color.FromArgb(alpha, Global.ColorFromHSL(leftHue, saturation, brightness));
                    cb.HighColor = Color.FromArgb(alpha, Global.ColorFromHSL(rightHue, saturation, brightness));
                }
                else
                {
                    cb.LowColor = Color.FromArgb(alpha, Global.ColorFromHSL(midHue, saturation, brightness));
                    cb.HighColor = cb.LowColor;
                }
               
            }
            OnColorSchemeChanged();
            OnSymbologyChange();

        }



        /// <summary>
        /// Opens color scheme settings from a mwleg file
        /// </summary>
        /// <param name="filename">The mwleg file to open.</param>
        public virtual void Open(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement root = doc.DocumentElement;
            ColorBreaks.Clear();
            if (root != null)
            {
                if (root.Attributes["SchemeType"].InnerText == "Grid")
                {
                    foreach (XmlNode brk in root.ChildNodes)
                    {
                        IRasterCategory cb = AddColorBreak(Convert.ToDouble(brk.Attributes["LowValue"].InnerText), Convert.ToDouble(brk.Attributes["HighValue"].InnerText));
                        cb.GradientModel = (GradientModels)int.Parse(brk.Attributes["GradientModel"].InnerText);
                        cb.LegendText = brk.Attributes["Caption"].InnerText;
                        cb.LowColor = Color.FromArgb(int.Parse(brk.Attributes["LowColor"].InnerText));
                        cb.HighColor = Color.FromArgb(int.Parse(brk.Attributes["HighColor"].InnerText));
                    }
                }
            }
            OnColorSchemeChanged();
            OnSymbologyChange();
        }

        /// <summary>
        /// Saves color scheme settings 
        /// </summary>
        /// <param name="filename">the mwleg file to save to</param>
        public virtual void Save(string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("ColoringScheme");
            XmlElement brk;
            XmlAttribute caption;
            XmlAttribute lValue;
            XmlAttribute hValue;
            XmlAttribute lColor;
            XmlAttribute hColor;
            XmlAttribute gradientModel;

            XmlAttribute schemeType = doc.CreateAttribute("SchemeType");
            schemeType.InnerText = "Grid";
            root.Attributes.Append(schemeType);
            foreach (IRasterCategory cb in ColorBreaks)
            {
                brk = doc.CreateElement("Break");
                caption = doc.CreateAttribute("Caption");
                lValue = doc.CreateAttribute("LowValue");
                hValue = doc.CreateAttribute("HighValue");
                lColor = doc.CreateAttribute("LowColor");
                hColor = doc.CreateAttribute("HighColor");
                gradientModel = doc.CreateAttribute("GradientModel");
                caption.InnerText = cb.LegendText;
                lValue.InnerText = cb.LowValue.ToString();
                hValue.InnerText = cb.HighValue.ToString();
                lColor.InnerText = cb.LowColor.ToArgb().ToString();
                hColor.InnerText = cb.HighColor.ToArgb().ToString();
                gradientModel.InnerText = ((int)cb.GradientModel).ToString();
                brk.Attributes.Append(caption);
                brk.Attributes.Append(lValue);
                brk.Attributes.Append(hValue);
                brk.Attributes.Append(lColor);
                brk.Attributes.Append(hColor);
                brk.Attributes.Append(gradientModel);
                root.AppendChild(brk);
            }
            doc.AppendChild(root);
            doc.Save(filename);
        }

        /// <summary>
        /// Creates the specified number of breaks that subdivide the range of raster values into
        /// the specified number of colorbreaks.  The breaks will use LowColor for the lowest
        /// colors and range to HighColor for the highest colors.
        /// </summary>
        /// <param name="numBreaks">The integer number of colorbreaks to create</param>
        /// <param name="LowColor">The color matching the low values of the color ramp</param>
        /// <param name="HighColor">The color matching the high values of the color ramp</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        public virtual void CreateRamp(int numBreaks, Color LowColor, Color HighColor, bool isContinuous)
        {
            
            double step = (Maximum - Minimum) / numBreaks;
            double rStep = (HighColor.R - (double)LowColor.R) / numBreaks;
            double bStep = (HighColor.B - (double) LowColor.B)/numBreaks;
            double gStep = (HighColor.G - (double) LowColor.G)/numBreaks;
            int opacity = ByteRange(_opacity * 255);
            _colorBreaks.Clear();
            for (int brk = 0; brk < numBreaks; brk++)
            {
                IRasterCategory cb = AddColorBreak(Minimum + step * brk, Minimum + step * (brk + 1));
                if (isContinuous)
                {
                    cb.LowColor = Color.FromArgb(opacity, ByteRange(rStep * brk), ByteRange(rStep * brk), ByteRange(rStep * brk));
                    cb.HighColor = Color.FromArgb(opacity, ByteRange(rStep * (brk + 1)), ByteRange(gStep * (brk + 1)), ByteRange(bStep * (brk + 1)));
                }
                else
                {
                    cb.LowColor = Color.FromArgb(opacity, ByteRange(rStep * (brk + .5)), ByteRange(gStep * (brk + .5)), ByteRange(bStep * (brk + .5)));
                    cb.HighColor = cb.LowColor;
                }
                
            }
            OnColorSchemeChanged();
            OnSymbologyChange();
        }

        /// <summary>
        /// Creates numBreaks that are randomply colored.
        /// </summary>
        /// <param name="numBreaks">The integer count of the number of breaks to create</param>
        public virtual void CreateRandomBreaks(int numBreaks)
        {
          
            Random rnd = new Random();
            double step = (Maximum - Minimum) / numBreaks;

            int opacity = ByteRange(_opacity * 255);
            _colorBreaks.Clear();
            for (int brk = 0; brk < numBreaks; brk++)
            {
                IRasterCategory cb = AddColorBreak(Minimum + step * brk, Minimum + step * (brk + 1));
              
                cb.LowColor = Color.FromArgb(opacity, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                cb.HighColor = cb.LowColor;
                cb.GradientModel = _gradientModel; // sets it to match this color scheme
            }
            OnColorSchemeChanged();
            OnSymbologyChange();
        }

        /// <summary>
        /// Creates a saturation ramp.  The specified fromColor provides the saturation and lightness
        /// to use.  If isStepped is true, each break only has one color.  If it is false,
        /// then each break transitions smoothly from one color to the next.
        /// </summary>
        /// <param name="numBreaks">The integer number of breaks to included in the hue ramp.</param>
        /// <param name="fromColor">The color to provide the lightness and saturation for the ramp.</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        /// <param name="isAscending">If this is true, increasing values get increasing saturation</param>
        public virtual void CreateSaturationRamp(int numBreaks, Color fromColor, bool isContinuous, bool isAscending)
        {
        
            _colorBreaks.Clear();

            double step = (Maximum - Minimum) / numBreaks;
            double saturationStep = 1.0 / numBreaks;
            double hue = fromColor.GetHue();
            double brightness = fromColor.GetBrightness();
            int alpha = ByteRange(_opacity * 255);
            for (int brk = 0; brk < numBreaks; brk++)
            {
                IRasterCategory cb = AddColorBreak(Minimum + step * brk, Minimum + step * (brk + 1));
                double leftSaturation;
                double midSaturation;
                double rightSaturation;
                if (isAscending)
                {
                    leftSaturation = (saturationStep * brk);
                    if (leftSaturation > 1) leftSaturation = 1;
                    if (leftSaturation < 0) leftSaturation = 0;
                    midSaturation = (saturationStep * (brk + 0.5));
                    if (midSaturation > 1) midSaturation = 1;
                    if (midSaturation < 0) midSaturation = 0;
                    rightSaturation = (saturationStep * (brk + 1));
                    if (rightSaturation > 1) rightSaturation = 1;
                    if (rightSaturation < 0) rightSaturation = 0;
                }
                else
                {
                    leftSaturation = (1 - saturationStep * brk);
                    if (leftSaturation > 1) leftSaturation = 1;
                    if (leftSaturation < 0) leftSaturation = 0;
                    midSaturation = (1 - saturationStep * (brk + 0.5));
                    if (midSaturation > 1) midSaturation = 1;
                    if (midSaturation < 0) midSaturation = 0;
                    rightSaturation = (1 - saturationStep * (brk + 1));
                    if (rightSaturation > 1) rightSaturation = 1;
                    if (rightSaturation < 0) rightSaturation = 0;
                }
                if (isContinuous)
                {
                    cb.LowColor = Color.FromArgb(alpha, Global.ColorFromHSL(hue, leftSaturation, brightness));
                    cb.HighColor = Color.FromArgb(alpha, Global.ColorFromHSL(hue, rightSaturation, brightness));
                }
                else
                {
                    cb.LowColor = Color.FromArgb(alpha, Global.ColorFromHSL(hue, midSaturation, brightness));
                    cb.HighColor = cb.LowColor;
                }
             
            }
            OnColorSchemeChanged();
            OnSymbologyChange();

        }


        /// <summary>
        /// Clears any existing color breaks in the ColorBreaks list and divides the range from Minimum to Maximum
        /// int numBreaks equal breaks.  It then uses fromColor for the hue and saturation values, but then
        /// changes the lightness value in equal steps.
        /// </summary>
        /// <param name="numBreaks">The integer number of color breaks to create.</param>
        /// <param name="fromColor">The color to use for the hue and saturation.</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        /// <param name="isAscending">If this is true, increasing values get increasing brightness.</param>
        public virtual void CreateShadingRamp(int numBreaks, Color fromColor, bool isContinuous, bool isAscending)
        {
            Color startColor;
            Color endColor;
            if (isAscending)
            {

                startColor = Global.ColorFromHSL(fromColor.GetHue(), fromColor.GetSaturation(), 0);
                endColor = Global.ColorFromHSL(fromColor.GetHue(), fromColor.GetSaturation(), 1);
            }
            else
            {
                startColor = Global.ColorFromHSL(fromColor.GetHue(), fromColor.GetSaturation(), 1);
                endColor = Global.ColorFromHSL(fromColor.GetHue(), fromColor.GetSaturation(), 0);
            }
            CreateRamp(numBreaks, startColor, endColor, isContinuous);
            
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ILegendItem to use as the parent for all of the color breaks.
        /// </summary>
        public ILegendItem Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                if(_colorBreaks != null) _colorBreaks.Parent = value;
            }
        }

        /// <summary>
        /// Gets or sets a list of colorbreaks
        /// </summary>
        [Category("Symbology"),
         TypeConverter(typeof(Forms.GeneralTypeConverter)),
         Description("Gets or sets the list of colors or color ranges that should be applied to this raster.")]
        public virtual IColorBreakList ColorBreaks
        {
            // This is overridden in subclasses, so _defaultColorbreaks is only used for the generic class.
            get { return _colorBreaks; }
            set
            {
                _colorBreaks = value;
                _colorBreaks.ItemChanged += _colorBreaks_ItemChanged;
                OnSymbologyChange();
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// Setting this will clear the existing colorbreaks and re-create the values to match the setting.
        /// If a built in ColorScheme has not yet been set, this simply returns null.
        /// </summary>
        [Category("Symbology"),
         Description("Gets or sets the colorbreaks to a pre-configured color scheme.")]
        public ColorSchemes ColorScheme
        {
            get { return _colorScheme; }
            set
            {
                _colorScheme = value;
                IRasterCategory cb;
                if (ColorBreaks == null)
                {
                    ColorBreaks = new ColorBreakList();
                }
                int alpha = ByteRange(Convert.ToInt32(_opacity * 255F));
                // this part should be overridden in the type specific version
                switch (value)
                {
                    case ColorSchemes.Summer_Mountains:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 10, 100, 10);
                        cb.HighColor = Color.FromArgb(alpha, 153, 125, 25);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 153, 125, 25);
                        cb.HighColor = Color.FromArgb(alpha, 255, 255, 255);
                        break;
                    case ColorSchemes.FallLeaves:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 10, 100, 10);
                        cb.HighColor = Color.FromArgb(alpha, 199, 130, 61);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 199, 130, 61);
                        cb.HighColor = Color.FromArgb(alpha, 241, 220, 133);
                        break;
                    case ColorSchemes.Desert:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 211, 206, 97);
                        cb.HighColor = Color.FromArgb(alpha, 139, 120, 112);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 139, 120, 112);
                        cb.HighColor = Color.FromArgb(alpha, 255, 255, 255);
                        break;
                    case ColorSchemes.Glaciers:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 105, 171, 224);
                        cb.HighColor = Color.FromArgb(alpha, 162, 234, 240);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 162, 234, 240);
                        cb.HighColor = Color.FromArgb(alpha, 255, 255, 255);
                        break;
                    case ColorSchemes.Meadow:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 68, 128, 71);
                        cb.HighColor = Color.FromArgb(alpha, 43, 91, 30);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 43, 91, 30);
                        cb.HighColor = Color.FromArgb(alpha, 167, 220, 168);
                        break;
                    case ColorSchemes.Valley_Fires:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 164, 0, 0);
                        cb.HighColor = Color.FromArgb(alpha, 255, 128, 64);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 255, 128, 64);
                        cb.HighColor = Color.FromArgb(alpha, 255, 255, 191);
                        break;
                    case ColorSchemes.DeadSea:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 51, 137, 208);
                        cb.HighColor = Color.FromArgb(alpha, 226, 227, 166);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 226, 227, 166);
                        cb.HighColor = Color.FromArgb(alpha, 151, 146, 117);
                        break;
                    case ColorSchemes.Highway:
                        _colorBreaks.Clear();
                        cb = AddColorBreak(Minimum, (Minimum + Maximum) / 2);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 51, 137, 208);
                        cb.HighColor = Color.FromArgb(alpha, 214, 207, 124);
                        cb = AddColorBreak((Minimum + Maximum) / 2, Maximum);
                        cb.IsNumeric = true;
                        cb.LowColor = Color.FromArgb(alpha, 214, 207, 124);
                        cb.HighColor = Color.FromArgb(alpha, 54, 152, 69);
                        break;
                    default:
                        break;

                }
                OnSymbologyChange();
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// If this is true, then the raster bitmap should be re-calculated.
        /// </summary>
        [Browsable(false)]
        public bool ColorSchemeHasChanged
        {
            get { return _colorSchemeHasChanged; }
            set { _colorSchemeHasChanged = value; }
        }

        /// <summary>
        /// Gets or sets the number of zeros after the decimal for any new colorbreaks
        /// </summary>
        public virtual int DecimalCount
        {
            get { return _decimalCount; }
            set 
            { 
                _decimalCount = value;
                foreach (IRasterCategory brk in _colorBreaks)
                {
                    brk.DecimalCount = value;
                }
                OnSymbologyChange();
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the technique to use when controlling how bi-valued ranges distribute
        /// the color spread.  
        /// </summary>
        [Category("Symbology"), Description("Gets or sets the technique to use when controlling how bi-valued colorbreaks distribute the color spread.")]
        public virtual GradientModels GradientModel
        {
            get { return _gradientModel; }
            set
            {
                _gradientModel = value;
                _colorSchemeHasChanged = true;
                OnSymbologyChange();
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// Gets whether or not this symbolizer has been altered since the last time a bitmap was created using this symbolizer.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool SymbologyHasChanged
        {
            get { return _symbologyHasChanged; }
            set 
            {
                _symbologyHasChanged = value; 
            }
        }

        /// <summary>
        /// Gets the maximum double value for this color ramp builder.  This is either set in the constructor or else
        /// is overridden to be calculated from the underlying data format.
        /// </summary>
        [Category("Behavior"), Description("Gets the maximum double value for this color ramp builder.")]
        public virtual double Maximum
        {
            get { return _maximum; }
        }

        /// <summary>
        /// Gets the minimum double value for this color ramp builder.  This is either set in the constructor or else
        /// is overridden to be calculated from the underlying data format.
        /// </summary>
        [Category("Behavior"), Description("Gets the minimum double value for this color ramp builder.")]
        public virtual double Minimum
        {
            get { return _minimum; }
        }

        /// <summary>
        /// Gets or sets a float value from 0 to 1, where 1 is fully opaque while 0 is fully transparent
        /// </summary>
        [Category("Symbology"), Description("Gets or sets the factor required to change elevation into the same horizontal and vertical units.")]
        public virtual float Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                _colorSchemeHasChanged = true;
                OnSymbologyChange();
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number format for new colorbreaks created by this object.
        /// Setting this will also update all the colorbreaks contained by this object.
        /// </summary>
        public virtual NumberFormats NumberFormat
        {
            get { return _numberFormat; }
            set 
            {
                _numberFormat = value;
                foreach (IRasterCategory brk in _colorBreaks)
                {
                    brk.NumberFormat = value;
                }
                OnSymbologyChange(); // this doesn't affect the bitmap construction
            }
        }

        

        
       


        #endregion

        #region Event Handlers

        void _colorBreaks_ItemChanged(object sender, EventArgs e)
        {
            // if someone changes any of the individual colorbreaks, we should update the view.
            OnSymbologyChange();
            OnColorSchemeChanged();
        }

        #endregion


        #region Protected Methods

        /// <summary>
        /// Fires the on color scheme changed event
        /// </summary>
        protected virtual void OnColorSchemeChanged()
        {
            _colorSchemeHasChanged = true;
            if (ColorSchemeChanged != null)
            {
                ColorSchemeChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Fires the SymbologyChanged event
        /// </summary>
        protected virtual void OnSymbologyChange()
        {
            _symbologyHasChanged = true;
            if (SymbologyChanged != null)
            { 
                SymbologyChanged(this, new EventArgs());
            }
        }


        #endregion

        #region Private Functions

        /// <summary>
        /// Returns an integer that ranges from 0 to 255.  If value is larger than 255, the value will be equal to 255.
        /// If the value is smaller than 255, it will be equal to 255.
        /// </summary>
        /// <param name="value">A Double value to convert.</param>
        /// <returns>An integer ranging from 0 to 255</returns>
        static int ByteRange(double value)
        {
            int rounded = (int)Math.Round(value);
            if (rounded > 255) return 255;
            if (rounded < 0) return 0;
            return rounded;
        }


        #endregion

    }
}
