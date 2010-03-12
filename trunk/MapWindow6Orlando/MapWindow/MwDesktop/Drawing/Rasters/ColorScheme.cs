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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 11:43:53 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using MapWindow.Data;
using MapWindow.Main;

namespace MapWindow.Drawing
{


    /// <summary>
    /// ColorScheme
    /// </summary>
    public class ColorScheme : Scheme, IColorScheme
    {
        #region Private Variables

        private ColorCategoryCollection _categories;
        private float _opacity;



        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorScheme
        /// </summary>
        public ColorScheme()
        {
            _categories = new ColorCategoryCollection(this);
            _opacity = 1;
            EditorSettings = new RasterEditorSettings();
        }

        #endregion

        #region Methods

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


        public void ApplyScheme(ColorSchemes scheme, IRaster raster)
        {
            GetValues(raster);
            double min = Statistics.Minimum;
            double max = Statistics.Maximum;
            
            if (Categories == null)
            {
                Categories = new ColorCategoryCollection(this);
            }
            int alpha = ByteRange(Convert.ToInt32(_opacity * 255F));
            // this part should be overridden in the type specific version
           
            Categories.Clear();
            IColorCategory low = new ColorCategory(min, (min + max)/2);
            low.Range.MaxIsInclusive = true;
            IColorCategory high = new ColorCategory((min + max)/2, max);
            high.Range.MaxIsInclusive = true;
            low.ApplyMinMax(EditorSettings);
            high.ApplyMinMax(EditorSettings);
            Categories.Add(low);
            Categories.Add(high);
            switch (scheme)
            {
                case ColorSchemes.Summer_Mountains:
                    low.LowColor = Color.FromArgb(alpha, 10, 100, 10);
                    low.HighColor = Color.FromArgb(alpha, 153, 125, 25);
                    high.LowColor = Color.FromArgb(alpha, 153, 125, 25);
                    high.HighColor = Color.FromArgb(alpha, 255, 255, 255);
                    break;
                case ColorSchemes.FallLeaves:
                    low.LowColor = Color.FromArgb(alpha, 10, 100, 10);
                    low.HighColor = Color.FromArgb(alpha, 199, 130, 61);
                    high.LowColor = Color.FromArgb(alpha, 199, 130, 61);
                    high.HighColor = Color.FromArgb(alpha, 241, 220, 133);
                    break;
                case ColorSchemes.Desert:
                    low.LowColor = Color.FromArgb(alpha, 211, 206, 97);
                    low.HighColor = Color.FromArgb(alpha, 139, 120, 112);
                    high.LowColor = Color.FromArgb(alpha, 139, 120, 112);
                    high.HighColor = Color.FromArgb(alpha, 255, 255, 255);
                    break;
                case ColorSchemes.Glaciers:
                    low.LowColor = Color.FromArgb(alpha, 105, 171, 224);
                    low.HighColor = Color.FromArgb(alpha, 162, 234, 240);
                    high.LowColor = Color.FromArgb(alpha, 162, 234, 240);
                    high.HighColor = Color.FromArgb(alpha, 255, 255, 255);
                    break;
                case ColorSchemes.Meadow:
                    low.LowColor = Color.FromArgb(alpha, 68, 128, 71);
                    low.HighColor = Color.FromArgb(alpha, 43, 91, 30);
                    high.LowColor = Color.FromArgb(alpha, 43, 91, 30);
                    high.HighColor = Color.FromArgb(alpha, 167, 220, 168);
                    break;
                case ColorSchemes.Valley_Fires:
                    low.LowColor = Color.FromArgb(alpha, 164, 0, 0);
                    low.HighColor = Color.FromArgb(alpha, 255, 128, 64);
                    high.LowColor = Color.FromArgb(alpha, 255, 128, 64);
                    high.HighColor = Color.FromArgb(alpha, 255, 255, 191);
                    break;
                case ColorSchemes.DeadSea:
                    low.LowColor = Color.FromArgb(alpha, 51, 137, 208);
                    low.HighColor = Color.FromArgb(alpha, 226, 227, 166);
                    high.LowColor = Color.FromArgb(alpha, 226, 227, 166);
                    high.HighColor = Color.FromArgb(alpha, 151, 146, 117);
                    break;
                case ColorSchemes.Highway:
                    low.LowColor = Color.FromArgb(alpha, 51, 137, 208);
                    low.HighColor = Color.FromArgb(alpha, 214, 207, 124);
                    high.LowColor = Color.FromArgb(alpha, 214, 207, 124);
                    high.HighColor = Color.FromArgb(alpha, 54, 152, 69);
                    break;
                default:
                    break;

            }
           
            OnItemChanged(this);
        }

        /// <summary>
        /// Creates the category using a random fill color
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category</param>
        /// <param name="size">For points this is the larger dimension, for lines this is the largest width</param>
        /// <returns>A new IFeatureCategory that matches the type of this scheme</returns>
        public override ICategory CreateNewCategory(Color fillColor, double size)
        {
            return new ColorCategory(null, null, fillColor, fillColor);
        }

        /// <summary>
        /// Creates the categories for this scheme based on statistics and values 
        /// sampled from the specified raster.
        /// </summary>
        /// <param name="raster">The raster to use when creating categories</param>
        public void CreateCategories(IRaster raster)
        {
            GetValues(raster);
            CreateBreakCategories();
            OnItemChanged(this);
        }

        /// <summary>
        /// Gets the values from the raster.  If MaxSampleCount is less than the
        /// number of cells, then it randomly samples the raster with MaxSampleCount
        /// values.  Otherwise it gets all the values in the raster.
        /// </summary>
        /// <param name="raster">The raster to sample</param>
        public void GetValues(IRaster raster)
        {
            Values = raster.GetRandomValues(EditorSettings.MaxSampleCount);
            Statistics.Calculate(Values, EditorSettings.Min, EditorSettings.Max);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the floating point value for the opacity 
        /// </summary>
        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        /// <summary>
        /// Gets or sets the raster categories
        /// </summary>
        public ColorCategoryCollection Categories
        {
            get { return _categories; }
            set
            {
                if (_categories != null) _categories.Scheme = null;
                _categories = value;
                if(_categories != null) _categories.Scheme = this;
            }
        }

        /// <summary>
        /// Gets or sets the raster editor settings assiciated with this scheme.
        /// </summary>
        public new RasterEditorSettings EditorSettings
        {
            get { return base.EditorSettings as RasterEditorSettings; }
            set { base.EditorSettings = value; }
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new IFeatureCategory</returns>
        public override ICategory CreateRandomCategory()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return CreateNewCategory(CreateRandomColor(rnd), 20);
        }

        /// <summary>
        /// Occurs when setting the parent item and updates the parent item pointers
        /// </summary>
        /// <param name="value"></param>
        protected override void OnSetParentItem(ILegendItem value)
        {
            base.OnSetParentItem(value);
            _categories.UpdateItemParentPointers();
        }

   
        #endregion



        /// <summary>
        /// Draws the category in the specified location.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        public override void DrawCategory(int index, Graphics g, Rectangle bounds)
        {
            _categories[index].LegendSymbol_Painted(g, bounds);
        }

        /// <summary>
        /// Adds the specified category
        /// </summary>
        /// <param name="category"></param>
        public override void AddCategory(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            if(cc != null) _categories.Add(cc);
        }

        /// <summary>
        /// Attempts to decrease the index value of the specified category, and returns
        /// true if the move was successful.
        /// </summary>
        /// <param name="category">The category to decrease the index of</param>
        /// <returns></returns>
        public override bool DecreaseCategoryIndex(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            return cc != null && _categories.DecreaseIndex(cc);
        }

        /// <summary>
        /// Removes the specified category
        /// </summary>
        /// <param name="category"></param>
        public override void RemoveCategory(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            if (cc != null) _categories.Remove(cc);
        }

        /// <summary>
        /// Inserts the item at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="category"></param>
        public override void InsertCategory(int index, ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            if (cc != null) _categories.Insert(index, cc);
        }

        /// <summary>
        /// Attempts to increase the position of the specified category, and returns true
        /// if the index increase was successful.
        /// </summary>
        /// <param name="category">The category to increase the position of</param>
        /// <returns>Boolean, true if the item's position was increased</returns>
        public override bool IncreaseCategoryIndex(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            return cc != null && _categories.IncreaseIndex(cc);
        }

        /// <summary>
        /// Suspends the change item event from firing as the list is being changed
        /// </summary>
        public override void SuspendEvents()
        {
            _categories.SuspendEvents();
        }

        /// <summary>
        /// Allows the ChangeItem event to get passed on when changes are made
        /// </summary>
        public override void ResumeEvents()
        {
            _categories.ResumeEvents();
        }

        /// <summary>
        /// Clears the categories
        /// </summary>
        public override void ClearCategories()
        {
            _categories.Clear();
        }
    }
}
