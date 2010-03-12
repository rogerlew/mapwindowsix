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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 11:26:05 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MapWindow.Main;
namespace MapWindow.Drawing
{


    /// <summary>
    /// IPattern
    /// </summary>
    public interface IPattern: ICloneable, IChangeItem
    {
       
        #region Methods

      
        /// <summary>
        /// Copies the properties defining the outline from the specified source onto this pattern.
        /// </summary>
        /// <param name="source">The source pattern to copy outline properties from.</param>
        void CopyOutline(IPattern source);
        
        /// <summary>
        /// Fills the specified graphics path with the pattern specified by this object
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics device to draw to</param>
        /// <param name="gp">The System.Drawing.GraphicsPath that describes the closed shape to fill</param>
        void FillPath(Graphics g, GraphicsPath gp);
       
        /// <summary>
        /// Draws the borders for this graphics path by sequentially drawing all
        /// the strokes in the border symbolizer
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics device to draw to </param>
        /// <param name="gp">The System.Drawing.GraphicsPath that describes the outline to draw</param>
        /// <param name="scaleWidth">The scaleWidth to use for scaling the line width </param>
        void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth);

        /// <summary>
        /// Gets a color that can be used to represent this pattern.  In some cases, a color is not
        /// possible, in which case, this returns Gray.
        /// </summary>
        /// <returns>A single System.Color that can be used to represent this pattern.</returns>
        Color GetFillColor();

        /// <summary>
        /// Sets the color that will attempt to be applied to the top pattern.  If the pattern is
        /// not colorable, this does nothing.
        /// </summary>
        /// <returns></returns>
        void SetFillColor(Color color);
       
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rectangular bounds.  This controls how the gradient is drawn, and
        /// should be set to the envelope of the entire layer being drawn
        /// </summary>
        RectangleF Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the line symbolizer that is the outline for this pattern.
        /// </summary>
        ILineSymbolizer Outline
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the pattern type of this pattern.
        /// </summary>
        PatternTypes PatternType
        {
            get;
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not the pattern should use the outline symbolizer.
        /// </summary>
        bool UseOutline
        {
            get;
            set;
        }

        

        #endregion



    }
}
