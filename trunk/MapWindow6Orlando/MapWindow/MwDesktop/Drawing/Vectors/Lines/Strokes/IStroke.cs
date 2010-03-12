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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:26:19 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
namespace MapWindow.Drawing
{


    /// <summary>
    /// IStroke
    /// </summary>
    public interface IStroke : IDescriptor
    {
      

        #region Methods

     

     
        /// <summary>
        /// This is an optional expression that allows drawing to the specified GraphicsPath.
        /// Overriding this allows for unconventional behavior to be included, such as
        /// specifying marker decorations, rather than simply returning a pen.  A pen
        /// is also returned publicly for convenience.
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics device to draw to</param>
        /// <param name="path">the System.Drawing.Drawing2D.GraphicsPath to draw</param>
        /// <param name="scaleWidth">This is 1 for symbolic drawing, but could be 
        /// any number for geographic drawing.</param>
        void DrawPath(Graphics g, GraphicsPath path, double scaleWidth);
       

        /// <summary>
        /// Casts this stroke to the appropriate pen
        /// </summary>
        /// <returns></returns>
        Pen ToPen(double width);

        /// <summary>
        /// Gets a color to represent this line.  If the stroke doesn't work as a color,
        /// then this color will be gray.
        /// </summary>
        /// <returns></returns>
        Color GetColor();

        /// <summary>
        /// Sets the color of this stroke to the specified color if possible.
        /// </summary>
        /// <param name="color">The color to assign to this color.</param>
        void SetColor(Color color);
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets the stroke style for this stroke
        /// </summary>
        StrokeStyles StrokeStyle
        {
            get;
        }

        #endregion



    }
}
