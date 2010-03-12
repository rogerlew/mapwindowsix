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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Data;
using MapWindow.Forms;
using MapWindow.Geometries;
using MapWindow.Main;
using System.Linq;
using MapWindow.Serialization;
namespace MapWindow.Drawing
{
    /// <summary>
    /// This is should not be instantiated because it cannot in itself perform the necessary functions.
    /// Instead, most of the specified functionality must be implemented in the more specific classes.
    /// This is also why there is no direct constructor for this class.  You can use the static
    /// "FromFile" or "FromFeatureLayer" to create FeatureLayers from a file.
    /// </summary>
    public abstract class FeatureLayer : Layer, IFeatureLayer 
    {
       

        #region Events

        /// <summary>
        /// Occurs before the attribute Table is displayed, also allowing this event to be handled.
        /// </summary>
        public event HandledEventHandler ViewAttributes;

        /// <summary>
        /// Occurs before the label setup dialog is displayed, allowing the event to be handled.
        /// </summary>
        public event HandledEventHandler LabelSetup;

        /// <summary>
        /// Occurs after a snapshot is taken, and contains an event argument with the bitmap
        /// to be displayed.
        /// </summary>
        public event EventHandler<SnapShotEventArgs> SnapShotTaken;

        /// <summary>
        /// Occurs after a new symbolic scheme has been applied to the layer.
        /// </summary>
        public event EventHandler SchemeApplied;
    

        #endregion

        #region Variables


       
        private LabelSetup _labelDialog;
        private AttributeDialog _attributeDialog;

        private ILabelLayer _labelLayer;
        private string _name;
        private IFeatureScheme _scheme;
        private ISelection _selection;
        private IDrawingFilter _drawingFilter;
        private bool _showLabels;
        private Rectangle _drawingBounds;
        private IFeatureSymbolizer _featureSymbolizer;
        private IFeatureSymbolizer _selectionFeatureSymbolizer;
        private IDictionary<IFeatureCategory, Extent> _categoryExtents;
        private FastDrawnState[] _drawnStates;
        private bool _editMode;
        private int _chunkSize;
        private bool _drawnStatesNeeded;
       
        #endregion

        #region Constructors


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="featureSet">The data bearing layer to apply new drawing characteristics to</param>
        protected FeatureLayer(IFeatureSet featureSet)
        {
            Configure(featureSet);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="featureSet">The data bearing layer to apply new drawing characteristics to</param>
        /// <param name="container">The container this layer should be added to</param>
        protected FeatureLayer(IFeatureSet featureSet, ICollection<ILayer> container):base(container)
        {
            Configure(featureSet);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="featureSet">The data bearing layer to apply new drawing characteristics to</param>
        /// <param name="container">The container this layer should be added to</param>
        /// <param name="progressHandler">A progress handler for receiving status messages</param>
        protected FeatureLayer(IFeatureSet featureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(container, progressHandler)
        {
            Configure(featureSet);
        }


        private void Configure(IFeatureSet featureSet)
        {
            _categoryExtents = new Dictionary<IFeatureCategory, Extent>();
            _drawingBounds = new Rectangle(-32000, -32000, 64000, 64000);
            DataSet = featureSet;
            LegendText = featureSet.Name;
            _name = featureSet.Name;
            ContextMenuItems.Insert(2, new MenuItem("View Attributes", ViewAttributesClick));
            MenuItem label = new MenuItem("Labeling");
            label.MenuItems.Add(new MenuItem("Label Setup", LabelSetupClick));
            label.MenuItems.Add(new MenuItem("Set Dynamic Visibility", LabelExtentsClick));
            ContextMenuItems.Insert(4, label);
            MenuItem selection = new MenuItem("Selection");
            ContextMenuItems.Insert(5, selection);
            selection.MenuItems.Add(new MenuItem("Zoom to Selected Features", SelectionZoomClick));
            selection.MenuItems.Add(new MenuItem("Create Layer from Selected Features", SelectionToLayerClick));
            if (!featureSet.IndexMode) _editMode = true;
            // Categories and selections

            // these are like a stored procedure that only care about the selection and no other drawing characteristics.
            if(_editMode)
            {
                _drawingFilter = new DrawingFilter(DataSet.Features, _scheme, 5000);
                _selection = new Selection(featureSet, _drawingFilter);
            }
            else
            {
                _selection = new IndexSelection(this);
            }
            _selection.Changed += SelectedFeaturesChanged;
            _labelDialog = new LabelSetup();
            _labelDialog.ChangesApplied += LabelDialogChangesApplied;
            _drawnStatesNeeded = false;
           
        }
        /// <summary>
        /// Gets or sets the boolean flag that controls whether the DrawnStates are needed.  If nothing is selected,
        /// and there is only one category, and there is no filter expression on that category, then this should be false.
        /// </summary>
        public bool DrawnStatesNeeded
        {
            get { return _drawnStatesNeeded; }
            set
            {
                if (_drawnStatesNeeded == false && value) AssignFastDrawnStates();
                _drawnStatesNeeded = value;
            }
        
        }
            
        void DataSetVerticesInvalidated(object sender, EventArgs e)
        {
            OnApplyScheme(Symbology);
            Invalidate();
        }

      

        

        void LabelExtentsClick(object sender, EventArgs e)
        {
            if (_labelLayer != null)
            {
                _labelLayer.UseDynamicVisibility = true;
                _labelLayer.DynamicVisibilityWidth = MapFrame.Extents.Width;
                DynamicVisibilityModeDialog dvg = new DynamicVisibilityModeDialog();
                dvg.ShowDialog();
                _labelLayer.DynamicVisiblityMode = dvg.DynamicVisibilityMode;

            }
        }

        void SelectionZoomClick(object sender, EventArgs e)
        {
            ZoomToSelectedFeatures();
        }

        /// <summary>
        /// Zooms to the envelope of the selected features.
        /// </summary>
        public void ZoomToSelectedFeatures()
        {
            if (_selection.Count == 0) return;
            IEnvelope env = _selection.Envelope;
            if(env.Width == 0 || env.Height == 0)
            {
                env.ExpandBy(2, 2);
            }
            OnZoomToLayer(env);
        }

      

        void SelectionToLayerClick(object sender, EventArgs e)
        {
            IFeatureLayer newLayer;
            if (CreateLayerFromSelectedFeatures(out newLayer))
            {
                IGroup grp = GetParentItem() as IGroup;
                if (grp != null)
                {
                    int index = grp.IndexOf(this);
                    grp.Insert(index + 1, newLayer);
                }
                newLayer.LegendText = LegendText + " selection";
            }
        }

        void SelectedFeaturesChanged(object sender, EventArgs e)
        {
            if (!_drawnStatesNeeded && !_editMode) AssignFastDrawnStates();
            OnItemChanged();
            OnSelectionChanged();
        }

        void LabelDialogChangesApplied(object sender, EventArgs e)
        {
            _labelLayer.CopyProperties(_labelDialog.Layer);
            
            _labelLayer.CreateLabels();
            if(MapFrame != null)MapFrame.Invalidate();           
        }


        #endregion


        #region Methods

        /// <summary>
        /// Applies the specified scheme to this layer, applying the filter constraints in the scheme.
        /// It is at this time when features are grouped as a one-time operation into their symbol
        /// categories, so that this doesn't have to happen independantly each drawing cycle.
        /// </summary>
        /// <param name="inScheme">The scheme to be applied to this layer.</param>
        public void ApplyScheme(IFeatureScheme inScheme)
        {
            OnApplyScheme(inScheme);
        }

        /// <summary>
        /// Clears the current selection, reverting the geometries back to their
        /// normal colors.
        /// </summary>
        /// <param name="affectedArea">An out value that represents the envelope that was modified by the clear selection instruction</param>
        public override bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = _selection.Envelope;
            if (!_drawnStatesNeeded) return false;
            bool changed = false;
            if(IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                if (_selection.Count > 0) changed = true;
                _selection.Clear();
            }
            else
            {
                SuspendChangeEvent();
                foreach (IFeatureCategory category in _scheme.GetCategories())
                {
                    if (!category.IsWithinLegendSelection()) continue;
                    _selection.RegionCategory = category;
                    _selection.Clear();
                    _selection.RegionCategory = null;
                }
                ResumeChangeEvent();
            }
            return changed;
        }


        /// <summary>
        /// This method actaully draws the image to the snapshot using the graphics object.  This should be overridden in 
        /// sub-classes because the drawing methods are very different.
        /// </summary>
        /// <param name="g">A graphics object to draw to</param>
        /// <param name="p">A projection handling interface designed to translate geographic coordinates to screen coordinates</param>
        public virtual void DrawSnapShot(Graphics g, IProj p)
        {

            throw new NotImplementedException("This should be overridden in sub-classes");
        }


        /// <summary>
        /// Saves a featureset with only the selected features to the specified filename.
        /// </summary>
        /// <param name="filename">The string filename to export features to.</param>
        public void ExportSelection(string filename)
        {
            FeatureSet fs = _selection.ToFeatureSet();
            fs.SaveAs(filename, true);
        }

        /// <summary>
        /// This is testing the idea of using an input parameter type that is marked as out
        /// instead of a return type.
        /// </summary>
        /// <param name="result">The result of the creation</param>
        /// <returns>Boolean, true if a layer can be created</returns>
        public virtual bool CreateLayerFromSelectedFeatures(out IFeatureLayer result)
        {
            // This needs to be overridden at the higher levels where drawing function
            // and point types exist, but is available here so that you don't need
            // to know what kind of feature layer you have to create an output layer.
            result = null;
            return false;
        }

        /// <summary>
        /// This method inverts the selection for the specified region.  Members already a part of the selection
        /// will be removed from the selection, while members that are not a part of the selection will be added
        /// to the selection.
        /// </summary>
        /// <param name="tolerant">The region specifying where featuers should be added or removed from the selection</param>
        /// <param name="strict">With polygon selection it is better not to allow any tolerance since the polygons already contain it</param>
        /// <param name="affectedArea">The geographic region that will be impacted by the changes</param>
        /// <param name="selectionMode">The SelectionModes enumeration that clarifies how the features should interact with the region</param>
        public override bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionModes selectionMode, out IEnvelope affectedArea)
        {
            if (!_drawnStatesNeeded && !_editMode) AssignFastDrawnStates();
            IEnvelope region = tolerant;
            bool changed = false;
            affectedArea = new Envelope();
            if (DataSet.FeatureType == FeatureTypes.Polygon)
            {
                region = strict;
            }
            _selection.SelectionMode = selectionMode;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                changed = _selection.InvertSelection(region, out affectedArea);
            }
            else
            {
                List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
                foreach (IFeatureCategory category in categories)
                {
                    if (!category.IsWithinLegendSelection()) continue;
                    _selection.RegionCategory = category;
                    if (_selection.AddRegion(region, out affectedArea)) changed = true;
                    _selection.RegionCategory = null;
                }
            }
         
            return changed;

        }

        /// <summary>
        /// Cycles through all the features and selects them
        /// </summary>
        public virtual void SelectAll()
        {
            if (!_drawnStatesNeeded && !_editMode) AssignFastDrawnStates();
            IEnvelope ignoreme;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                
                _selection.AddRegion(Envelope, out ignoreme);
            }
            else
            {
                SuspendChangeEvent();
                foreach (IFeatureCategory category in _scheme.GetCategories())
                {
                    if (!category.IsWithinLegendSelection()) continue;
                    _selection.RegionCategory = category;
                    _selection.AddRegion(Envelope, out ignoreme);
                    _selection.RegionCategory = null;
                }
                ResumeChangeEvent();
            }
        }
       
        /// <summary>
        /// Highlights the values in the specified region, and returns the affected area from the selection,
        /// which should allow for slightly faster drawing in cases where only a small area is changed.
        /// This will also specify the method by which members should be selected.
        /// </summary>
        /// <param name="tolerant">The envelope to change</param>
        /// <param name="strict">The envelope to use in cases like polygons where the geometry has no tolerance</param>
        /// <param name="affectedArea">The geographic envelope of the region impacted by the selection.</param>
        /// <param name="selectionMode">The selection mode that clarifies the rules to use for selection.</param>
        /// <returns>Boolean, true if items were selected.</returns>
        public override bool Select(IEnvelope tolerant, IEnvelope strict, SelectionModes selectionMode, out IEnvelope affectedArea)
        {
            if (!_drawnStatesNeeded && !_editMode) AssignFastDrawnStates();
            IEnvelope region = tolerant;
            if(DataSet.FeatureType == FeatureTypes.Polygon) region = strict;
            affectedArea = _selection.Envelope;
            
            bool changed = false;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                _selection.SelectionMode = selectionMode;
                changed = _selection.AddRegion(region, out affectedArea);
            }
            else
            {
                if (!_drawnStatesNeeded) AssignFastDrawnStates();
                SuspendChangeEvent();
                _selection.SuspendChanges();
                List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
                foreach (IFeatureCategory category in categories)
                {
                    if (!category.IsSelected) continue;
                    _selection.RegionCategory = category;
                    _selection.AddRegion(region, out affectedArea);
                    _selection.RegionCategory = null;
                }
                _selection.ResumeChanges();
                ResumeChangeEvent();
            }
            return changed;
            
        }

       

        /// <summary>
        /// Selects the specified list of features.  If the specified feature is already selected,
        /// this method will not alter it.  This will only fire a single SelectionExtended event,
        /// rather than firing it for each member selected.
        /// </summary>
        /// <param name="featureIndices">A List of integers representing the zero-based feature index values</param>
        /// <returns>boolean if any changes occur</returns>
        public void Select(IEnumerable<int> featureIndices)
        {
            if(_editMode)
            {
                List<IFeature> features = new List<IFeature>();
                foreach (int fid in featureIndices)
                {
                    features.Add(DataSet.Features[fid]);
                }
                IFeatureSelection sel = _selection as IFeatureSelection;
                if(sel != null)sel.AddRange(features);
            }
            else
            {
                if (!_drawnStatesNeeded) AssignFastDrawnStates();
                IIndexSelection sel = _selection as IIndexSelection;
                if(sel != null)sel.AddRange(featureIndices);
               
            }
        }


        /// <summary>
        /// Selects a single feature specified by the integer index in the Features list.
        /// </summary>
        /// <param name="featureIndex">The zero-based integer index of the feature.</param>
        /// <returns>Boolean that is true if the specified feature was successfully selected</returns>
        public void Select(int featureIndex)
        {
            if(_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null) sel.Add(DataSet.Features[featureIndex]);
            }
            else
            {
                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null) sel.Add(featureIndex);
            }
        }

        /// <summary>
        /// Selects the specified feature
        /// </summary>
        /// <param name="feature">The feature to select</param>
        public void Select(IFeature feature)
        {
            if (_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null) sel.Add(feature);
            }
            else
            {
                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null) sel.Add(DataSet.Features.IndexOf(feature));
            }
        }

        /// <summary>
        /// Selects all the features in this layer that are associated
        /// with the specified attribute.  This automatically relaces the existing selection.
        /// </summary>
        /// <param name="filterExpression">The string expression to
        /// identify based on attributes the features to select.</param>
        public void SelectByAttribute(string filterExpression)
        {
            SelectByAttribute(filterExpression, ModifySelectionModes.Replace);
        }

        /// <summary>
        /// Modifies the features with a new selection based on the modifyMode.
        /// </summary>
        /// <param name="filterExpression">The string filter expression to use</param>
        /// <param name="modifyMode">Determines how the newly chosen features should interact with the existing selection</param>
        public void SelectByAttribute(string filterExpression, ModifySelectionModes modifyMode)
        {
            if (!_drawnStatesNeeded && !_editMode) AssignFastDrawnStates();
            List<int> newSelection = DataSet.SelectIndexByAttribute(filterExpression);
            _selection.SuspendChanges();
            if (modifyMode == ModifySelectionModes.Replace)
            {
                _selection.Clear();
                Select(newSelection);
            }
            if (modifyMode == ModifySelectionModes.Append)
            {
                Select(newSelection);
            }
            if (modifyMode == ModifySelectionModes.SelectFrom)
            {
                List<int> cond = new List<int>();
                if(_editMode)
                {
                    IFeatureSelection fs = _selection as IFeatureSelection;
                    if(fs != null)
                    {
                        foreach (IFeature feature in fs)
                        {
                            cond.Add(DataSet.Features.IndexOf(feature));
                        }
                    }
                }
                else
                {
                    IIndexSelection sel = _selection as IIndexSelection;
                    if(sel != null)
                    {
                        cond = sel.ToList();
                    }
                }
                IEnumerable<int> result = cond.Intersect(newSelection);
                _selection.Clear();
                Select(result);
            }
            if (modifyMode == ModifySelectionModes.Subtract)
            {
                UnSelect(newSelection);
            }
            _selection.ResumeChanges();
            OnItemChanged();
        }
        

        /// <summary>
        /// Creates a bitmap of the requested size that covers the specified geographic extent using
        /// the current symbolizer for this layer.  This does not have any drawing optimizations,
        /// or techniques to speed up performance and should only be used in special cases like
        /// draping of vector content onto a texture.  It also doesn't worry about selections.
        /// </summary>
        /// <param name="geographicExtent">The extent to use when computing the snapshot.</param>
        /// <param name="width">The integer height of the bitmap.  The height is calculated based on 
        /// the aspect ratio of the specified geographic extent.</param>
        /// <returns>A Bitmap object</returns>
        public Bitmap SnapShot(IEnvelope geographicExtent, int width)
        {
            int height = Convert.ToInt32((geographicExtent.Height / geographicExtent.Width) * width);
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            ImageProjection p = new ImageProjection(geographicExtent, new Rectangle(0, 0, width, height));
            DrawSnapShot(g, p);
            g.Dispose();
            OnSnapShotTaken(bmp);
            return bmp;

        }




        /// <summary>
        /// Gets the visible characteristic for an individual feature
        /// </summary>
        /// <param name="index"></param>
        public bool GetVisible(int index)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[DataSet.Features[index]].IsVisible;
            }
            else
            {
                return DrawnStates[index].Visible;
            }
        }

        /// <summary>
        /// Gets the visible characteristic for a given feature, rather than using the index.
        /// </summary>
        /// <param name="feature"></param>
        public bool GetVisible(IFeature feature)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[feature].IsVisible;
            }
            else
            {
                int index = DataSet.Features.IndexOf(feature);
                return DrawnStates[index].Visible;
            }

        }


        /// <summary>
        /// Gets the visible characteristic for an individual feature, regardless of whether 
        /// this layer is in edit mode.
        /// </summary>
        /// <param name="index"></param>
        public IFeatureCategory GetCategory(int index)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[DataSet.Features[index]].SchemeCategory;
            }
            else
            {
                return DrawnStates[index].Category;
            }
        }

        /// <summary>
        /// Gets the visible characteristic for a given feature, rather than using the index,
        /// regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="feature"></param>
        public IFeatureCategory GetCategory(IFeature feature)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[feature].SchemeCategory;
            }
            else
            {
                int index = DataSet.Features.IndexOf(feature);
                return DrawnStates[index].Category;
            }

        }

       
        public void SetCategory(int index, IFeatureCategory category)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[DataSet.Features[index]].SchemeCategory = category;
            }
            else
            {
                DrawnStates[index].Category = category;
            }
            if(!_scheme.GetCategories().Contains(category))
            {
                _scheme.InsertCategory(0, category);
            }
        }

        public void SetCategory(IFeature feature, IFeatureCategory category)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[feature].SchemeCategory = category;
            }
            else
            {
                int index = DataSet.Features.IndexOf(feature);
                DrawnStates[index].Category = category;
            }
            if(!_scheme.GetCategories().Contains(category))
            {
                _scheme.InsertCategory(0, category);
            }

        }
        


        /// <summary>
        /// Sets the visible characteristic for an individual feature regardless of
        /// whether this layer is in edit mode.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="visible"></param>
        public void SetVisible(int index, bool visible)
        {
            if(_editMode)
            {
                DrawingFilter.DrawnStates[DataSet.Features[index]].IsVisible = visible;
            }
            else
            {
                DrawnStates[index].Visible = false;
            }
        }

       
        /// <summary>
        /// This forces the creation of a category for the specified symbolizer, if it doesn't exist.
        /// This will add the specified feature to the category.  Be sure that the symbolizer type
        /// matches the feature type.
        /// </summary>
        /// <param name="index">The integer index of the shape to control.</param>
        /// <param name="symbolizer">The symbolizer to assign.</param>
        public void SetShapeSymbolizer(int index, IFeatureSymbolizer symbolizer)
        {
            foreach (IFeatureCategory category in Symbology.GetCategories())
            {
                if (category.Symbolizer != symbolizer) continue;
                if(DataSet.IndexMode)
                {
                    _drawnStates[index].Category = category;
                    _drawnStates[index].Visible = true;
                }
                else
                {
                    IFeature f = DataSet.Features[index];
                    DrawingFilter.DrawnStates[f].SchemeCategory = category;
                }
                Invalidate();
                return;
            }
            ICategory cat = Symbology.CreateNewCategory(Color.Blue, 3);
            Symbology.AddCategory(cat);
            if(DataSet.IndexMode)
            {
                _drawnStates[index].Category = (IFeatureCategory)cat;
                _drawnStates[index].Visible = true;
            }
            else
            {
                IFeature f = DataSet.Features[index];
                DrawingFilter.DrawnStates[f].SchemeCategory = (IFeatureCategory)cat;
            }
            Invalidate();
            

        }


        /// <summary>
        /// Sets the visible characteristic for a given feature, rather than using the index
        /// regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="visible"></param>
        public void SetVisible(IFeature feature, bool visible)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[feature].IsVisible = visible;
            }
            else
            {
                int index = DataSet.Features.IndexOf(feature);
                DrawnStates[index].Visible = visible;
            }
            
        }

        

        /// <summary>
        /// Displays a form with the attributes for this shapefile.
        /// </summary>
        public void ShowAttributes()
        {
            // Allow derived classes to prevent this
            HandledEventArgs result = new HandledEventArgs(false);
            OnViewAttributes(result);
            if (result.Handled) return;

            if (_attributeDialog != null)
            {
                if (_attributeDialog.Visible) _attributeDialog.Close();
                _attributeDialog = null;
            }
            _attributeDialog = new AttributeDialog(this);
            //if (DataSet.DataTable == null)
            //{
            //    DataSet.FillAttributes(); // load attributes the first time they are needed, not at the first load
            //}         
            _attributeDialog.Show();
        }

        /// <summary>
        /// Unselects all the features that are currently selected
        /// </summary>
        public virtual void UnSelectAll()
        {
            _selection.Clear();
        }

        /// <summary>
        /// Unselects the specified features.  If any features already unselected, they are ignored.
        /// This will only fire a single Selection
        /// </summary>
        /// <param name="featureIndices"></param>
        /// <returns>Boolean that is true if the selection was changed.</returns>
        public void UnSelect(IEnumerable<int> featureIndices)
        {
            if(_editMode)
            {
                List<IFeature> features = new List<IFeature>();
                foreach (int fid in featureIndices)
                {
                    features.Add(DataSet.Features[fid]);
                }
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null) sel.RemoveRange(features);
            }
            else
            {
                if (!_drawnStatesNeeded) AssignFastDrawnStates();
                IIndexSelection sel = _selection as IIndexSelection;
                if(sel != null) sel.RemoveRange(featureIndices);
            }

        }

        /// <summary>
        /// Unselects the specified feature.
        /// </summary>
        /// <param name="featureIndex">The integer representing the feature to unselect.</param>
        /// <returns>Boolean, true if the feature was unselected.</returns>
        public void UnSelect(int featureIndex)
        {
            if(_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null) sel.Remove(DataSet.Features[featureIndex]);
            }
            else
            {
                if (!_drawnStatesNeeded) AssignFastDrawnStates();
                 IIndexSelection sel = _selection as IIndexSelection;
                if(sel != null) sel.Remove(featureIndex);
            }
        }

        /// <summary>
        /// Removes the specified feature from the selection.
        /// </summary>
        /// <param name="feature">The feature to unselect.</param>
        public void UnSelect(IFeature feature)
        {
            if(_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null) sel.Remove(feature);
            }
            else
            {
                if (!_drawnStatesNeeded) AssignFastDrawnStates();
                IIndexSelection sel = _selection as IIndexSelection;
                if(sel != null) sel.Remove(DataSet.Features.IndexOf(feature));
            }
           
        }

        /// <summary>
        /// Un-highlights or returns the features from the specified region.  The specified selectionMode
        /// will be used to determine how to choose features.
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="affectedArea">The geographic envelope that will be visibly impacted by the change</param>
        /// <param name="selectionMode">The selection mode that controls how to choose members relative to the region </param>
        /// <returns>Boolean, true if members were removed from the selection.</returns>
        public override bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionModes selectionMode, out IEnvelope affectedArea)
        {
            if (!_drawnStatesNeeded && !_editMode) AssignFastDrawnStates();
            IEnvelope region = tolerant;
            if (DataSet.FeatureType == FeatureTypes.Polygon) region = strict;
            affectedArea = new Envelope();

            bool changed = false;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                if (_editMode)
                {

                    _selection.SelectionMode = selectionMode;
                    changed = _selection.RemoveRegion(region, out affectedArea);
                }
                else
                {
                    _selection.SelectionMode = selectionMode;
                    changed = _selection.RemoveRegion(region, out affectedArea);
                }
            }
            else
            {
                SuspendChangeEvent();
                _selection.SuspendChanges();
                List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
                foreach (IFeatureCategory category in categories)
                {
                    if (!category.IsSelected) continue;
                    _selection.RegionCategory = category;
                    _selection.RemoveRegion(region, out affectedArea);
                    _selection.RegionCategory = null;
                }
                _selection.ResumeChanges();
                ResumeChangeEvent();
            }
            return changed;
        }

        /// <summary>
        /// Occurs when the properties should be shown, and launches a layer dialog.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShowProperties(HandledEventArgs e)
        {
            FeatureLayerDialog ldg = new FeatureLayerDialog(this);
            ldg.ShowDialog();
            e.Handled = true;
        }
       
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean.  If edit mode is true, feature index is ignored, and features
        /// are assumed to be entirely loaded into ram.  If edit mode is false, then index
        /// is used instead and features are not assumed to be loaded into ram.
        /// </summary>
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                if (_editMode)
                {
                    _drawingFilter = new DrawingFilter(DataSet.Features, _scheme, 5000);
                    _selection = new Selection(DataSet, _drawingFilter);
                }
                else
                {
                    _selection = new IndexSelection(this);
                }
            }
        }

        /// <summary>
        /// Controls the drawn states according to a feature index.  This is used if the EditMode is 
        /// false.  When EditMode is true, then drawn states are tied to the features instead.
        /// </summary>
        public FastDrawnState[] DrawnStates
        {
            get
            {
                if(_drawnStates == null)
                {
                    DrawnStatesNeeded = true;
                }
                return _drawnStates;
            }
            set { _drawnStates = value; }
        }
        
        /// <summary>
        /// Gets or sets the chunk size on the drawing filter.  This should be controlled
        /// by drawing layers.
        /// </summary>
        protected int ChunkSize
        {
            get { return _chunkSize; }
            set { _chunkSize = value; }
        }

        /// <summary>
        /// Gets or sets the underlying dataset for this layer, specifically as an IFeatureSet
        /// </summary>
		[Serialize("DataSet", ConstructorArgumentIndex = 0)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IFeatureSet DataSet
        {
            get { return base.DataSet as IFeatureSet; }
            set 
            {
                if (DataSet != null) OnIgnoreFeaturesetEvents(DataSet);
                base.DataSet = value;
                OnHandleFeaturesetEvents(DataSet);
            }
        }

        /// <summary>
        /// Gets or sets a rectangle that gives the maximal extent for 2D GDI+ drawing in pixels.
        /// Coordinates outside this range will cause overflow exceptions to occur.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle DrawingBounds
        {
            get { return _drawingBounds; }
            set { _drawingBounds = value; }
        }

        /// <summary>
        /// Gets or sets the drawing filter that can be used to narrow the list of features and then
        /// cycle through those features.  Using a for-each expression on the filter will automatically
        /// apply the constraints specified by the characteristics.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public IDrawingFilter DrawingFilter
        {
            get { return _drawingFilter; }
            set 
            { 
                _drawingFilter = value; 
            }
        }

        /// <summary>
        /// Gets the envelope of the DataSet supporting this FeatureLayer
        /// </summary>
        [Category("General"), Description("Gets the envelope of the DataSet supporting this FeatureLayer")]
        public override IEnvelope Envelope
        {
            get 
            {
                if (DataSet == null) return new Envelope();
                if(!(DataSet.Extent == null || DataSet.Extent.IsEmpty()))return DataSet.Extent.ToEnvelope();
                return DataSet.Envelope; 
            }
        }


        /// <summary>
        /// Gets or sets the label layer
        /// </summary>
        [Category("General"), Description("Gets or sets the label layer associated with this feature layer."), ShallowCopy]
        [Serialize("LabelLayer")]
        public virtual ILabelLayer LabelLayer
        {
            get { return _labelLayer; }
            set
            {
                _labelLayer = value;
                _labelLayer.FeatureLayer = this;
                _labelLayer.CreateLabels();
                OnItemChanged();
            }
           
        }

        /// <summary>
        /// Restructures the LegendItems based on whether or not the symbology makes use
        /// of schemes.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                if (Symbology.AppearsInLegend)
                {
                    List<ILegendItem> list = new List<ILegendItem>();
                    list.Add(Symbology);
                    return list;
                }
                return Symbology.GetCategories().Cast<ILegendItem>();
            }
        }

        

    
        /// <summary>
        ///  Gets or sets a string name for this layer.  This is not necessarilly the same as the legend text.
        /// </summary>
        [Category("General"), Description("Gets or sets a string name for this layer.  This is not necessarilly the same as the legend text.")]
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

       

     

        /// <summary>
        /// Gets the current feature scheme, but to change it ApplyScheme should be called, so that
        /// feature categories are updated as well.
        /// </summary>
        [ShallowCopy]
        public IFeatureScheme Symbology
        {
            get { return _scheme; }
            set 
            {
                if (value == _scheme) return;
                OnExcludeScheme(_scheme);
                _scheme = value;
                OnIncludeScheme(value);
                ApplyScheme(value);
            }
        }

        /// <summary>
        /// Occurs when setting the symbology to a new scheme and allows removing event handlers
        /// </summary>
        /// <param name="scheme"></param>
        protected virtual void OnExcludeScheme(IFeatureScheme scheme)
        {
            if (scheme == null) return;
            scheme.ItemChanged -= SchemeItemChanged;
            scheme.SetParentItem(null);
            scheme.SelectFeatures -= OnSelectFeatures;
        }

        /// <summary>
        /// Occurs when setting symbology to a new scheme and allows adding event handlers
        /// </summary>
        /// <param name="scheme"></param>
        protected virtual void OnIncludeScheme(IFeatureScheme scheme)
        {
            if (scheme == null) return;
            scheme.ItemChanged += SchemeItemChanged;
            scheme.SetParentItem(this);
            scheme.SelectFeatures += OnSelectFeatures;
        }

        /// <summary>
        /// Occurs when selecting features and fires the SelectByAttribute event with
        /// the expression used as the filter expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectFeatures(object sender, ExpressionEventArgs e)
        {
            SelectByAttribute(e.Expression);
        }

     

        /// <summary>
        /// Echoes the ItemChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SchemeItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        /// <summary>
        /// Gets a Selection class that is allows the user to cycle through all the selected features with
        /// a foreach method.  This can be null.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISelection Selection
        {
            get
            {
                return _selection;
            }
        }

       

       
        /// <summary>
        /// Gets or sets whether labels should be drawn.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether labels should be drawn."), Serialize("ShowLabels")]
        public virtual bool ShowLabels
        {
            get { return _showLabels; }
            set 
            { 
                _showLabels = value;
                OnItemChanged();
            }
        }

        /// <summary>
        /// Gets or sets and interface for the shared symbol characteristics between point, line and polygon features
        /// </summary>
        [Category("Appearance"), Description("Gets or sets a collection of feature characteristics for this layer."),
        ShallowCopy]
        public virtual IFeatureSymbolizer Symbolizer
        {
            get 
            {
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {
                            return categories.First().Symbolizer;
                        }
                    }
                }
                return _featureSymbolizer;

            }
            set
            {
                value.SetParentItem(this);
                bool defaultExisted = false;
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {

                            categories.First().Symbolizer = value;
                            defaultExisted = true;
                        }
                    }
                }
                if(defaultExisted == false) _featureSymbolizer = value;
               
               
            }
        }


        

        /// <summary>
        /// Gets or sets the shared characteristics to use with the selected features.
        /// </summary>
        [Category("Symbology"), Description("Gets or sets a collection of feature characteristics for this layer."),
        ShallowCopy]
        public virtual IFeatureSymbolizer SelectionSymbolizer
        {
            get 
            {
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {
                            return categories.First().SelectionSymbolizer;
                        }
                    }
                }
                return _selectionFeatureSymbolizer;
            }
            set 
            {
                value.SetParentItem(this);
                bool defaultExisted = false;
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {

                            categories.First().SelectionSymbolizer = value;
                            defaultExisted = true;
                        }
                    }
                }
                if (defaultExisted == false) _selectionFeatureSymbolizer = value;
                
            }
        }



        /// <summary>
        /// Occurs as a new featureset is being assigned to this layer
        /// </summary>
        protected virtual void OnHandleFeaturesetEvents(IFeatureSet featureSet)
        {
            if (featureSet == null) return;
            DataSet.VerticesInvalidated += DataSetVerticesInvalidated;
            DataSet.FeatureAdded += DataSetFeatureAdded;
            DataSet.FeatureRemoved += DataSetFeatureRemoved;
        }

        void DataSetFeatureRemoved(object sender, FeatureEventArgs e)
        {
            _drawingFilter.DrawnStates.Remove(e.Feature);
        }

        void DataSetFeatureAdded(object sender, FeatureEventArgs e)
        {
            if (_drawingFilter == null) return;
            if (_drawingFilter.DrawnStates == null) return;
            _drawingFilter.DrawnStates.Add(e.Feature, new DrawnState(Symbology.GetCategories().First(), false, 0, true));
        }

      

     

        /// <summary>
        /// Unwires event handlers for the specified featureset.
        /// </summary>
        /// <param name="featureSet"></param>
        protected virtual void OnIgnoreFeaturesetEvents(IFeatureSet featureSet)
        {
            if (featureSet == null) return;
            DataSet.VerticesInvalidated -= DataSetVerticesInvalidated;
            DataSet.FeatureAdded -= DataSetFeatureAdded;
            DataSet.FeatureRemoved -= DataSetFeatureRemoved;
        }

       

        ///// <summary>
        ///// Gets an InverseSelection class that allows the user to cycle through all the unselected features with
        ///// a foreach method, even if the features are in many categories.
        ///// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public IFilterCollection UnSelectedFeatures
        //{
        //    get { return _unselectedFeatures; }
        //}

        /// <summary>
        /// Gets the dictionary of extents that is calculated from the categories.  This is calculated one time,
        /// when the scheme is applied, and then the cached value is used to help drawing symbols that
        /// are modified by the categorical boundaries.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDictionary<IFeatureCategory, Extent> CategoryExtents
        {
            get { return _categoryExtents; }
        }

       

        #endregion

        #region Protected Methods

        

        /// <summary>
        /// This calculates the extent for the category and caches it in the extents collection
        /// </summary>
        /// <param name="category"></param>
        protected virtual Extent CalculateCategoryExtent(IFeatureCategory category)
        {
            Extent ext = new Extent(new[]{double.MaxValue, double.MaxValue, double.MinValue, double.MinValue});
            if(_editMode)
            {
                IDictionary<IFeature, IDrawnState> features = _drawingFilter.DrawnStates;

                foreach (IFeature f in DataSet.Features)
                {
                    if (category == features[f].SchemeCategory)
                    {
                        ext.ExpandToInclude(new Extent(f.Envelope));
                    }
                }
                if (_categoryExtents.Keys.Contains(category))
                {
                    _categoryExtents[category] = ext.Copy();
                }
                else
                {
                    _categoryExtents.Add(category, ext.Copy());
                }
            }
            else
            {
                FastDrawnState[] states = DrawnStates;
                List<ShapeRange> ranges = DataSet.ShapeIndices;
                for(int shp = 0; shp < DrawnStates.Length; shp++)
                {
                    if(!_categoryExtents.ContainsKey(states[shp].Category))
                    {
                        _categoryExtents.Add(states[shp].Category, ranges[shp].Extent.Copy());
                    }
                    else
                    {
                        _categoryExtents[states[shp].Category].ExpandToInclude(ranges[shp].Extent);
                    }
                }
            }
            
            return ext;
        }

        /// <summary>
        /// This method cycles through all the Categories in the scheme and creates a new
        /// category.  
        /// </summary>
        /// <param name="scheme">The scheme to apply</param>
        protected virtual void OnApplyScheme(IFeatureScheme scheme)
        {
            //_drawingFilter.ApplyScheme(scheme); 

            if(_editMode)
            {
                _drawingFilter.ApplyScheme(scheme); 
            }
            else
            {
                List<IFeatureCategory> categories = scheme.GetCategories().ToList();
                if(_drawnStatesNeeded || (_selection != null && _selection.Count > 0) || categories.Count > 1 || categories[0].FilterExpression != null)
                {
                    AssignFastDrawnStates();
                }
            }
            
            _categoryExtents.Clear();
            OnSchemeApplied();
            OnItemChanged(this);
        }

       
        protected void AssignFastDrawnStates()
        {
            _drawnStatesNeeded = true;
            _drawnStates = new FastDrawnState[DataSet.ShapeIndices.Count];
            _selection = new IndexSelection(this); // update the new drawn-states;
            _selection.Changed += SelectedFeaturesChanged;
            // Fastest when no categories are used because we don't need DataTable at all
            List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
            IFeatureCategory deflt = null;
            if(categories.Count > 0 && categories[0].FilterExpression == null) deflt = categories[0];
            for (int i = 0; i < DataSet.ShapeIndices.Count; i++)
            {
                _drawnStates[i] = new FastDrawnState();
                _drawnStates[i].Category = deflt;
            }
            if(categories.Count == 1 && categories[0].FilterExpression == null) return;
            bool containsFID = false;
            DataTable table = DataSet.DataTable;
            foreach (var category in categories)
            {
                if (category.FilterExpression != null && category.FilterExpression.Contains("[FID]"))
                {
                    containsFID = true;
                }
            }
            if(containsFID && table.Columns.Contains("FID") == false)
            {
                table.Columns.Add("FID");
                for(int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i]["FID"] = i;
                }
            }
            foreach (var category in categories)
            {
                DataRow[] result = table.Select(category.FilterExpression);
                foreach (DataRow row in result)
                {
                    _drawnStates[table.Rows.IndexOf(row)].Category = category;
                }
            }
            if(containsFID)table.Columns.Remove("FID");
            
        }

      


        /// <summary>
        /// This fires the scheme applied event after the scheme has been altered
        /// </summary>
        protected virtual void OnSchemeApplied()
        {
            if(SchemeApplied != null)SchemeApplied(this, new EventArgs());
        }

        /// <summary>
        /// Occurs during a copy operation and handles removing surplus event handlers
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            // Remove event handlers from the copy (since Memberwise clone also clones handlers.)
            FeatureLayer flCopy = copy as FeatureLayer;
            if (flCopy == null) return;
            if(flCopy.ViewAttributes != null)
            {
                foreach (var handler in flCopy.ViewAttributes.GetInvocationList())
                {
                    flCopy.ViewAttributes -= (HandledEventHandler) handler;
                }
            }
            if(flCopy.LabelSetup != null)
            {
                foreach (var handler in flCopy.LabelSetup.GetInvocationList())
                {
                    flCopy.LabelSetup -= (HandledEventHandler) handler;
                }
            }
            if(flCopy.SnapShotTaken != null)
            {
                foreach (var handler in flCopy.SnapShotTaken.GetInvocationList())
                {
                    flCopy.SnapShotTaken -= (EventHandler<SnapShotEventArgs>) handler;
                }
            }
            if(flCopy.SchemeApplied != null)
            {
                foreach (var handler in flCopy.SchemeApplied.GetInvocationList())
                {
                    flCopy.SchemeApplied -= (EventHandler) handler;
                }
            }


            base.OnCopy(copy);
        }

        /// <summary>
        /// A default method to generate a label layer.
        /// </summary>
        protected virtual void OnCreateLabels()
        {
            _labelLayer = new LabelLayer();
        }

        /// <summary>
        /// Handles the situation for exporting the layer as a new source.
        /// </summary>
        protected override void OnExportData()
        {
            ExportFeature frmExport = new ExportFeature();
            frmExport.Filename = DataSet.Filename;
            if (frmExport.ShowDialog() != DialogResult.OK) return;
            if (frmExport.FeaturesIndex == 0)
            {
                DataSet.SaveAs(frmExport.Filename, true);
            }
            else if (frmExport.FeaturesIndex == 1)
            {
                FeatureSet fs = _selection.ToFeatureSet();
                fs.SaveAs(frmExport.Filename, true);
            }
            else if (frmExport.FeaturesIndex == 2)
            {
                List<IFeature> features = DataSet.Select(MapFrame.Extents);
                FeatureSet fs = new FeatureSet(features);
                if (fs.Features.Count == 0)
                {
                    fs.CopyTableSchema(DataSet);
                }
                fs.SaveAs(frmExport.Filename, true);               
            }
            AddToMapDialog dlgAddLayer = new AddToMapDialog();
            if (dlgAddLayer.ShowDialog() != DialogResult.OK) return;
            if (dlgAddLayer.AddLayer == false) return;
            IFeatureLayer newLayer = OpenFile(frmExport.Filename) as IFeatureLayer;
            IGroup parent = GetParentItem() as IGroup;
            if (parent != null)
            {
                int index = parent.IndexOf(this);
                if (newLayer != null)
                {
                    parent.Insert(index + 1, newLayer);
                }
            }
        }

        /// <summary>
        /// Occurs before the label setup dialog is shown.  If handled is set to true,
        /// then the dialog will not be shown.
        /// </summary>
        /// <param name="e">A HandledEventArgs parameter</param>
        protected virtual void OnLabelSetup(HandledEventArgs e)
        {
            if (LabelSetup != null) LabelSetup(this, e);
        }

        

       

        /// <summary>
        /// Fires teh SnapShotTaken event.  This can be overridden in order to modify the bitmap returned etc.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSnapShotTaken(Bitmap e)
        {
            if(SnapShotTaken != null)SnapShotTaken(this, new SnapShotEventArgs(e));
        }

        /// <summary>
        /// Occurs before attributes are about to be viewed.  Overriding this
        /// allows that to be handled, and if e.Handled is true, this class
        /// won't show the attributes.
        /// </summary>
        /// <param name="e">A HandledEventArgs</param>
        protected virtual void OnViewAttributes(HandledEventArgs e)
        {
            if (ViewAttributes != null)
            {
                ViewAttributes(this, e);
            }
        }


        #endregion


        #region EventHandlers

        private void LabelSetupClick(object sender, EventArgs e)
        {
            HandledEventArgs result = new HandledEventArgs(false);
            OnLabelSetup(result);
            if (result.Handled) return;

            if (_labelLayer == null)
            {
                OnCreateLabels();
                ShowLabels = true;
            }
            _labelDialog.Layer = _labelLayer.Copy();
            _labelDialog.Show();

        }

        private void ViewAttributesClick(object sender, EventArgs e)
        {
            ShowAttributes();
            
        }


       

       

        #endregion

        #region Private Methods

       


        #endregion

        #region Static Methods

       

        #endregion


        
    }


}
