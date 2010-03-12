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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 11:56:59 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using MapWindow.Data;

namespace MapWindow.Drawing
{


    /// <summary>
    /// IColorScheme
    /// </summary>
    public interface IColorScheme : IScheme
    {
       
        #region Methods

        /// <summary>
        /// Applies the specified color scheme from a list of predefined scheme options.
        /// </summary>
        /// <param name="scheme">The predefined color scheme</param>
        /// <param name="raster">The raster that provides values to govern symbolizing</param>
        void ApplyScheme(ColorSchemes scheme, IRaster raster);

        /// <summary>
        /// Creates the categories for this scheme based on statistics and values 
        /// sampled from the specified raster.
        /// </summary>
        /// <param name="raster">The raster to use when creating categories</param>
        void CreateCategories(IRaster raster);

        /// <summary>
        /// Gets the values from the raster.  If MaxSampleCount is less than the
        /// number of cells, then it randomly samples the raster with MaxSampleCount
        /// values.  Otherwise it gets all the values in the raster.
        /// </summary>
        /// <param name="raster">The raster to sample</param>
        void GetValues(IRaster raster);
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the floating point value for the opacity 
        /// </summary>
        float Opacity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of color scheme categories to use.
        /// </summary>
        ColorCategoryCollection Categories
        {
            get; set;
        }

        /// <summary>
        /// gets or sets the editor settings for controls that affect the color scheme.
        /// </summary>
        new RasterEditorSettings EditorSettings
        {
            get; set;
        }

        #endregion



    }
}
