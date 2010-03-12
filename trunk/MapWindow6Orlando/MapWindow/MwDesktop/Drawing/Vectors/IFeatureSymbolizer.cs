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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 12:30:41 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
namespace MapWindow.Drawing
{


    /// <summary>
    /// IFeatureSymbolizer
    /// </summary>
    public interface IFeatureSymbolizer : ILegendItem
    {

        #region Methods

       

        /// <summary>
        /// Draws a simple rectangle in the specified location.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="target"></param>
        void Draw(Graphics g, Rectangle target);
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether or not this specific feature should be drawn.
        /// </summary>
        bool IsVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates.  If pixels are
        /// specified, a back transform is used to approximate pixel sizes. 
        /// </summary>
        ScaleModes ScaleMode
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        void SetOutline(Color outlineColor, double width);
       

        /// <summary>
        /// Gets or sets the smoothing mode to use that controls advanced features like
        /// anti-aliasing.  By default this is set to antialias.
        /// </summary>
        bool Smoothing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the graphics unit to work with.
        /// </summary>
        GraphicsUnit Units
        {
            get;
            set;
        }

        #endregion

    }
}
