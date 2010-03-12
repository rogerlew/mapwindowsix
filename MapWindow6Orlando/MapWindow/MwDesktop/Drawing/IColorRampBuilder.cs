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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/29/2008 8:27:23 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using MapWindow.Main;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IColorRampMaker
    /// </summary>
    public interface IColorRampBuilder: ICloneable
    {
      
        #region Methods

        /// <summary>
        /// This creates a colorbreak with the specified range, but all the other
        /// characteristics of this ColorRampBuilder, like NumberFormat and Decimal Count and Gradient style.
        /// </summary>
        /// <param name="value">The single value to use when creating the new colorbreak.</param>
        IColorCategory AddColorBreak(IComparable value);

        /// <summary>
        /// This creates a colorbreak with the specified range, but all the other
        /// characteristics of this ColorRampBuilder, like NumberFormat and Decimal Count and Gradient style.
        /// </summary>
        /// <param name="startValue">The start value.  If this is lower than endValue, it will become the low value, otherwise, it will become the high value.</param>
        /// <param name="endValue">The end Value.  If this is lower than startValue, it will become the low value, otherwise, it will become the high value.</param>
        IColorCategory AddColorBreak(IComparable startValue, IComparable endValue);

         /// <summary>
        /// This creates a deep clone of the member because the typical case wants a "backup" of the
        /// colorscheme.  
        /// </summary>
        /// <returns>An IColorRampBuilder with a duplicate set of colorbreaks, maxima, minima etc.</returns>
        IColorRampBuilder Copy();


        /// <summary>
        /// Creates a hue ramp.  The specified fromColor provides the saturation and lightness
        /// and starting/ending hue to use.
        /// </summary>
        /// <param name="numBreaks">The integer number of breaks to included in the hue ramp.</param>
        /// <param name="fromColor">The color to provide the lightness and saturation for the ramp.</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        ///  <param name="isAscending">If this is true, increasing values get increasing saturation</param>
        void CreateHueRamp(int numBreaks, Color fromColor, bool isContinuous, bool isAscending);

        /// <summary>
        /// Creates the specified number of breaks that subdivide the range of raster values into
        /// the specified number of colorbreaks.  The breaks will use StartColor for the lowest
        /// colors and range to EndColor for the highest colors.
        /// </summary>
        /// <param name="numBreaks">The integer number of colorbreaks to create</param>
        /// <param name="StartColor">The color matching the low values of the color ramp</param>
        /// <param name="EndColor">The color matching the high values of the color ramp</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        void CreateRamp(int numBreaks, Color StartColor, Color EndColor, bool isContinuous);


        /// <summary>
        /// Creates numBreaks that are randomply colored.
        /// </summary>
        /// <param name="numBreaks">The integer count of the number of breaks to create</param>
        void CreateRandomBreaks(int numBreaks);


        /// <summary>
        /// Creates a saturation ramp.  The specified fromColor provides the saturation and lightness
        /// to use.  If isStepped is true, each break only has one color.  If it is false,
        /// then each break transitions smoothly from one color to the next.
        /// </summary>
        /// <param name="numBreaks">The integer number of breaks to included in the hue ramp.</param>
        /// <param name="fromColor">The color to provide the lightness and saturation for the ramp.</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        ///  <param name="isAscending">If this is true, increasing values get increasing saturation</param>
        void CreateSaturationRamp(int numBreaks, Color fromColor, bool isContinuous, bool isAscending);



        /// <summary>
        /// Clears any existing color breaks in the ColorBreaks list and divides the range from Minimum to Maximum
        /// int numBreaks equal breaks.  It then uses fromColor for the hue and saturation values, but then
        /// changes the lightness value in equal steps.
        /// </summary>
        /// <param name="numBreaks">The integer number of color breaks to create.</param>
        /// <param name="fromColor">The color to use for the hue and saturation.</param>
        /// <param name="isContinuous">If this is false, each break has one value.  If it is true, then each break is a range of colors.</param>
        ///  <param name="isAscending">If this is true, increasing values get increasing saturation</param>
        void CreateShadingRamp(int numBreaks, Color fromColor, bool isContinuous, bool isAscending);

        /// <summary>
        /// Opens color scheme settings from a mwleg file
        /// </summary>
        /// <param name="filename">The mwleg file to open.</param>
        void Open(string filename);
        
             /// <summary>
        /// Saves color scheme settings 
        /// </summary>
        /// <param name="filename">the mwleg file to save to</param>
        void Save(string filename);
        


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a list of colorbreaks
        /// </summary>
        IColorBreakList ColorBreaks
        {
            // This is overridden in subclasses, so _defaultColorbreaks is only used for the generic class.
            get;
            set;
        }

        /// <summary>
        /// Setting this will clear the existing colorbreaks and re-create the values to match the setting.
        /// If a built in ColorScheme has not yet been set, this simply returns null.
        /// </summary>
        ColorSchemes ColorScheme
        {
            get;
            set;
        }

        /// <summary>
        /// Boolean, true if any of the characteristics that affect a bitmap texture have been updated
        /// </summary>
        bool ColorSchemeHasChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of zeros after the decimal for any new colorbreaks
        /// </summary>
        int DecimalCount
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the technique to use when controling how bi-valued ranges distribute
        /// the color spread.  
        /// </summary>
        GradientModels GradientModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether or not this symbolizer has been altered since the last time a bitmap was created using this symbolizer.
        /// </summary>
        bool SymbologyHasChanged
        {
            get;
            set;
        }




        /// <summary>
        /// Gets the maximum double value.
        /// </summary>
        double Maximum
        {
            get;
        }

        /// <summary>
        /// Gets the minimum double value. 
        /// </summary>
        double Minimum
        {
            get;
        }
        /// <summary>
        /// Gets or sets the number format for new colorbreaks created by this object.
        /// Setting this will also update all the colorbreaks contained by this object.
        /// </summary>
        NumberFormats NumberFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ILegendItem to use as the parent for all of the color breaks.
        /// </summary>
        ILegendItem Parent
        {
            get;
            set;
        }

        #endregion



    }
}
