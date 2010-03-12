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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2009 11:22:18 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using MapWindow.Forms;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// PointSchemeCategory
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class PolygonCategory : FeatureCategory, IPolygonCategory
    {
        #region Private Variables

       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointSchemeCategory
        /// </summary>
        public PolygonCategory()
        {
            Symbolizer = new PolygonSymbolizer();
            SelectionSymbolizer = new PolygonSymbolizer(true);
        }

        /// <summary>
        /// Specifies a category that is made up from a simple color.
        /// </summary>
        /// <param name="fillColor">The color to fill the polygons with</param>
        /// <param name="outlineColor">The border color for the polygons</param>
        /// <param name="outlineWidth">The width of the line drawn on the border</param>
        public PolygonCategory(Color fillColor, Color outlineColor, double outlineWidth)
        {
            Symbolizer = new PolygonSymbolizer(fillColor, outlineColor, outlineWidth);
            SelectionSymbolizer = new PolygonSymbolizer(Color.Cyan, Color.DarkCyan, outlineWidth);
        }

        /// <summary>
        /// Creates a new PolygonCategory with the specified image being tiled within the category.  
        /// The s
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        public PolygonCategory(Image picture, WrapMode wrap, double angle)
        {
            Symbolizer = new PolygonSymbolizer(picture, wrap, angle);
            SelectionSymbolizer = new PolygonSymbolizer(Color.Cyan);
        }

        /// <summary>
        /// Creates a new PolygonCategory with the specified image being tiled within the category.  
        /// The simple outline characteristics are also defined.
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        /// <param name="outlineColor">The color to use</param>
        /// <param name="outlineWidth">The outline width</param>
        public PolygonCategory(Image picture, WrapMode wrap, double angle, Color outlineColor, double outlineWidth)
        {
            Symbolizer = new PolygonSymbolizer(picture, wrap, angle, outlineColor, outlineWidth);
            SelectionSymbolizer = new PolygonSymbolizer(Color.Cyan, Color.DarkCyan);
        }

        /// <summary>
        /// Creates a new instance of a Gradient Pattern using the specified colors and angle
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient</param>
        /// <param name="style">The type of gradient to use</param>
        /// <param name="outlineColor">The color to use for the border symbolizer</param>
        /// <param name="outlineWidth">The width of the line to use for the border symbolizer</param>
        public PolygonCategory(Color startColor, Color endColor, double angle, GradientTypes style, Color outlineColor, double outlineWidth)
        {
            Symbolizer = new PolygonSymbolizer(startColor, endColor, angle, style, outlineColor, outlineWidth);
            SelectionSymbolizer = new PolygonSymbolizer(Color.LightCyan, Color.DarkCyan, angle, style, Color.DarkCyan, outlineWidth);
        }

        /// <summary>
        /// Creates a new instanec of a default point scheme category where the geographic symbol size has been
        /// scaled to the specified extent.
        /// </summary>
        /// <param name="extent">The geographic extent that is 100 times wider than the geographic size of the points.</param>
        public PolygonCategory(IEnvelope extent)
        {
            Symbolizer = new PolygonSymbolizer(false, extent);
            SelectionSymbolizer = new PolygonSymbolizer(true, extent);
        }

        /// <summary>
        /// Creates a new category based on a symbolizer, and uses the same symbolizer, but with a fill and border color of light cyan
        /// for the selection symbolizer
        /// </summary>
        /// <param name="polygonSymbolizer">The symbolizer to use in order to create a category</param>
        public PolygonCategory(IPolygonSymbolizer polygonSymbolizer)
        {
            Symbolizer = polygonSymbolizer;
            IPolygonSymbolizer select = polygonSymbolizer.Copy();
            select.OutlineSymbolizer.ScaleMode = ScaleModes.Symbolic;
        }

        #endregion

        #region Methods


        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category.  For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern.  If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        public override Color GetColor()
        {
            if(Symbolizer == null || Symbolizer.Patterns == null || Symbolizer.Patterns.Count == 0)return Color.Gray;
            IPattern p = Symbolizer.Patterns[0];
            return p.GetFillColor();
        }

        /// <summary>
        /// Sets teh fill color of the top-most pattern to the specified color, if the pattern can reciee a color.
        /// </summary>
        /// <param name="color">Sets the color of the top most pattern for the principal symbolizer.</param>
        public override void SetColor(Color color)
        {
            if (Symbolizer == null || Symbolizer.Patterns == null || Symbolizer.Patterns.Count == 0) return;
            Symbolizer.Patterns[0].SetFillColor(color);
        }
      
      

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer for this category
        /// </summary>
        [Description("Gets or sets the symbolizer for this category"),
        TypeConverter(typeof(GeneralTypeConverter)),
        Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        public new IPolygonSymbolizer Symbolizer
        {
            get { return base.Symbolizer as IPolygonSymbolizer; }
            set { base.Symbolizer = value; }
        }

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        [Description("Gets or sets the symbolizer to use to draw selected features from this category."),
        TypeConverter(typeof(GeneralTypeConverter)),
        Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        public new IPolygonSymbolizer SelectionSymbolizer
        {
            get { return base.SelectionSymbolizer as IPolygonSymbolizer; }
            set { base.SelectionSymbolizer = value; }
        }

        #endregion



    }
}
