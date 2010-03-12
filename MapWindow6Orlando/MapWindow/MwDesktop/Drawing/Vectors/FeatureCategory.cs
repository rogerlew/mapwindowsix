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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 1:54:18 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System;
using System.ComponentModel;
using MapWindow.Forms;
using System.Drawing.Design;
using MapWindow.Serialization;
using System.Windows.Forms;
namespace MapWindow.Drawing
{


    /// <summary>
    /// A Scheme category does not reference individual members or indices, but simply describes a symbolic representation that
    /// can be used by an actual category.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class FeatureCategory : Category, IFeatureCategory
    {
        /// <summary>
        /// Occurs when the select features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> SelectFeatures;

      

        #region Private Variables

        private IFeatureSymbolizer _featureSymbolizer;
        private IFeatureSymbolizer _selectionSymbolizer;
        private string _filterExpression;
        private MenuItem _mnuSelectFeatures;
        private MenuItem _mnuRemoveMe;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureSchemeCategory
        /// </summary>
        public FeatureCategory()
        {
            base.LegendSymbolMode = SymbolModes.Symbol;
            LegendType = LegendTypes.Symbol;
            CreateContextMenuItems();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Makes it so that if there are any pre-existing listeners to the SelectFeatuers
        /// event when creating a clone of this object, those listeners are removed.
        /// They should be added correctly when the cloned item is added to the collection,
        /// after being cloned.
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            FeatureCategory cat = copy as FeatureCategory;
            if(cat != null && cat.SelectFeatures != null)
            {
                foreach (var handler in cat.SelectFeatures.GetInvocationList())
                {
                    cat.SelectFeatures -= (EventHandler<ExpressionEventArgs>)handler;
                }
            }
            if(cat != null)
            {
                cat.CreateContextMenuItems();
            }
            base.OnCopy(copy);
        }

        /// <summary>
        /// Forces the creation of an entirely new context menu list.  That way, we are not
        /// pointing to an event handler in the previous parent.
        /// </summary>
        public void CreateContextMenuItems()
        {
            base.ContextMenuItems = new List<MenuItem>();
            _mnuRemoveMe = new MenuItem("Remove Catgory", RemoveCategoryClicked);
            _mnuSelectFeatures = new MenuItem("Select Features", SelectFeaturesClicked);
            base.ContextMenuItems.Add(_mnuRemoveMe);
            base.ContextMenuItems.Add(_mnuSelectFeatures);
        }

        private void RemoveCategoryClicked(object sender, EventArgs e)
        {
            OnRemoveItem();
        }
        
        private void SelectFeaturesClicked(object sender, EventArgs e)
        {
            OnSelectFeatures();
        }

        /// <summary>
        /// Fires the SelectFeatures event
        /// </summary>
        protected virtual void OnSelectFeatures()
        {
            if(SelectFeatures != null)
            {
                SelectFeatures(this, new ExpressionEventArgs(_filterExpression));
            }
        }

       
        /// <summary>
        /// Applies the minimum and maximum in order to create the filter expression.  This will also
        /// count the members that match the specified criteria.
        /// </summary>
        /// <returns>The integer count of the members selected by this expression</returns>
        public override void ApplyMinMax(EditorSettings settings)
        {
            
            base.ApplyMinMax(settings);
            FeatureEditorSettings fs = settings as FeatureEditorSettings;
            if (fs == null) return;
            string field = "[" + fs.FieldName.ToUpper() + "]";
            if (!string.IsNullOrEmpty(fs.NormField)) field += "/[" + fs.NormField.ToUpper() + "]";
            IntervalSnapMethods method = settings.IntervalSnapMethod;
            int digits = settings.IntervalRoundingDigits;
            LegendText = Range.ToString(method, digits);
            _filterExpression = Range.ToExpression(field);
        }

        


        /// <summary>
        /// Controls what happens in the legend when this item is instructed to draw a symbol.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            _featureSymbolizer.Draw(g, box);
        }

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        public bool IsWithinLegendSelection()
        {
            if (IsSelected) return true;
            ILayer lyr = GetParentItem() as ILayer;
            while (lyr != null)
            {
                if (lyr.IsSelected) return true;
                lyr = lyr.GetParentItem() as ILayer;
            }
            return false;

        }
        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category.  For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern.  If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        public virtual Color GetColor()
        {
            return Color.Gray;
        }

        /// <summary>
        /// This applies the color to the top symbol stroke or pattern.
        /// </summary>
        /// <param name="color">The System.Drawing.Color to apply</param>
        public virtual void SetColor(Color color)
        {
            return;
        }

        /// <summary>
        /// In some cases, it is useful to simply be able to show an approximation of the actual expression.
        /// This also removes brackets from the field names to make it slightly cleaner.
        /// </summary>
        public void DisplayExpression()
        {
            string exp = _filterExpression;
            if (exp != null)
            {
                exp = exp.Replace("[", "");
                exp = exp.Replace("]", "");
            }
            LegendText = exp;
        }
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer used for this category.
        /// </summary>
        [Serialize("Symbolizer")]
        public IFeatureSymbolizer Symbolizer
        {
            get { return _featureSymbolizer; }
            set 
            {
                if (_featureSymbolizer != null) _featureSymbolizer.ItemChanged -= Symbolizer_ItemChanged;
                if (_featureSymbolizer != value && value != null)
                {
                    value.ItemChanged += Symbolizer_ItemChanged;
                    value.SetParentItem(this);
                }
                _featureSymbolizer = value;
                OnItemChanged();
            }
        }

        void Symbolizer_ItemChanged(object sender, EventArgs e)
        {
            OnItemChanged();
        }



        /// <summary>
        /// Gets or sets the symbolizer used for this category
        /// </summary>
        [Serialize("SelectionSymbolizer")]
        public IFeatureSymbolizer SelectionSymbolizer
        {
            get { return _selectionSymbolizer; }
            set
            {
                if (_selectionSymbolizer != null) _selectionSymbolizer.ItemChanged -= Symbolizer_ItemChanged;
                if(_selectionSymbolizer != value && value != null)
                {
                    value.ItemChanged += Symbolizer_ItemChanged;
                    value.SetParentItem(this);
                }
                _selectionSymbolizer = value;
                OnItemChanged();
            }
        }

        /// <summary>
        /// Gets or set the filter expression that is used to add members to generate a category based on this scheme.
        /// </summary>
        [Description("Gets or set the filter expression that is used to add members to generate a category based on this scheme."),
         Editor(typeof(ExpressionEditor), typeof(UITypeEditor)), Serialize("FilterExpression")]
        public string FilterExpression
        {
            get
            {
                return _filterExpression;
            }
            set { _filterExpression = value; }
        }



        #endregion



    }
}
