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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 4:44:51 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Forms;
using MapWindow.Main;
using MapWindow.Geometries;
using MapWindow.Data;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{


    /// <summary>
    /// Layer
    /// </summary>
    [ToolboxItem(false)]
    public class Layer : RenderableLegendItem, ILayer
    {



        #region Events

        
        /// <summary>
        /// Occurs if this layer was selected
        /// </summary>
        public event EventHandler<LayerSelectedEventArgs> LayerSelected;

        /// <summary>
        /// Occurs if the maps should zoom to this layer.
        /// </summary>
        public event EventHandler<EnvelopeArgs> ZoomToLayer;


        /// <summary>
        /// Occurs before the properties are actually shown, also allowing the event to be handled.
        /// </summary>
        public event HandledEventHandler ShowProperties;

        /// <summary>
        /// Occurs when all aspects of the layer finish loading.
        /// </summary>
        public event EventHandler FinishedLoading;

        /// <summary>
        /// Occurs after the selection is changed
        /// </summary>
        public event EventHandler SelectionChanged;
       

        #endregion


        #region Private Variables

      
      
        private Layer _editCopy;
        private bool _useDynamicVisibility;
        private double _dynamicVisibilityWidth;
        private DynamicVisibilityModes _dynamicVisibilityMode;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private IEnvelope _invalidatedRegion; // When a region is invalidated instead of the whole layer.
        private IFrame _mapFrame;
        private bool _isFinishedLoading;
        private IPropertyDialogProvider _propertyDialogProvider;
        private IDataSet _dataSet;
        private bool _selectionEnabled;
        #endregion

        #region Constructors


        /// <summary>
        /// Creates a new Layer, but this should be done from the derived classes
        /// </summary>
        protected Layer()
        {
           // InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new Layer, but this should be done from the derived classes
        /// </summary>
        /// <param name="container">The container this layer should be a member of</param>
        protected Layer(ICollection<ILayer> container)
        {
            
            if(container != null) container.Add(this);
            Configure();
        }

        protected Layer(IProgressHandler progressHandler)
        {
            _progressHandler = progressHandler;
            Configure();
        }

        /// <summary>
        /// Creates a new Layer, but this should be done from the derived classes
        /// </summary>
        /// <param name="container">The container this layer should be a member of</param>
        /// <param name="progressHandler">A progress handler for handling progress messages</param>
        protected Layer(ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            _progressHandler = progressHandler;
            if (container != null) container.Add(this);
            Configure();
        }

        private void Configure()
        {
            _selectionEnabled = true;
            base.ContextMenuItems = new List<MenuItem>();
            base.ContextMenuItems.Add(new MenuItem("Remove Layer", RemoveLayerClick));
            base.ContextMenuItems.Add(new MenuItem("Zoom to Layer", ZoomToLayerClick));
            base.ContextMenuItems.Add(new MenuItem("Set Dynamic Visiblity Scale", SetDynamicVisibility));
            MenuItem mnuData = new MenuItem("Data");
            mnuData.MenuItems.Add(new MenuItem("Export Data", ExportDataClick));
            base.ContextMenuItems.Add(mnuData);
            base.ContextMenuItems.Add(new MenuItem("Properties", ShowPropertiesClick));
            
            base.LegendSymbolMode = SymbolModes.Checkbox;
            LegendType = LegendTypes.Layer;
            base.IsExpanded = true;
            _propertyDialogProvider = new PropertyDialogProvider();
            _propertyDialogProvider.ChangesApplied += _propertyDialogProvider_ChangesApplied;
        }

        void _propertyDialogProvider_ChangesApplied(object sender, ChangedObjectEventArgs e)
        {
            CopyProperties(e.Item as ILegendItem);
        }

        



        #endregion

        #region Methods

        void SetDynamicVisibility(object sender, EventArgs e)
        {
            UseDynamicVisibility = true;
            DynamicVisibilityWidth = MapFrame.Extents.Width;
            DynamicVisibilityModeDialog dvg = new DynamicVisibilityModeDialog();
            dvg.ShowDialog();
            DynamicVisiblityMode = dvg.DynamicVisibilityMode;
        }

        /// <summary>
        /// Given a geographic extent, this tests the "IsVisible", "UseDynamicVisibility", "DynamicVisibilityMode" and "DynamicVisibilityWidth"
        /// In order to determine if this layer is visible for the specified scale.
        /// </summary>
        /// <param name="geographicExtent">The geographic extent, where the width will be tested.</param>
        /// <returns>Boolean, true if this layer should be visible for this extent.</returns>
        public bool VisibleAtExtent(IEnvelope geographicExtent)
        {
            if (!IsVisible) return false;
            if (UseDynamicVisibility)
            {
                if (DynamicVisiblityMode == DynamicVisibilityModes.ZoomedIn)
                {
                    if (geographicExtent.Width > DynamicVisibilityWidth)
                    {
                        return false;  // skip the geoLayer if we are zoomed out too far.
                    }
                }
                else
                {
                    if (geographicExtent.Width < DynamicVisibilityWidth)
                    {
                        return false;  // skip the geoLayer if we are zoomed out too far.
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// Tests the specified legend item.  If the item is another layer or a group or a map-frame, then this
        /// will return false.  Furthermore, if the parent of the item is not also this object, then it will
        /// also return false.  The idea is that layers can have sub-nodes move around, but not transport from
        /// place to palce.
        /// </summary>
        /// <param name="item">the legend item to test</param>
        /// <returns>Boolean that if true means that it is ok to insert the specified item into this layer.</returns>
        public override bool CanReceiveItem(ILegendItem item)
        {
            if (item.GetParentItem() != this) return false;
            ILayer lyr = item as ILayer;
            if (lyr != null) return false;
            IFrame mf = item as IFrame;
            if (mf != null) return false;
            IGroup gr = item as IGroup;
            if (gr != null) return false;
            return true;
        }
   
        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        public bool IsWithinLegendSelection()
        {
            if (IsSelected) return true;
            ILayer lyr = GetParentItem() as ILayer;
            while(lyr != null)
            {
                if(lyr.IsSelected) return true;
                lyr = lyr.GetParentItem() as ILayer;
            }
            return false;
           
        }

        /// <summary>
        /// Notifies the layer that the next time an area that intersects with this region
        /// is specified, it must first re-draw content to the image buffer.
        /// </summary>
        /// <param name="region">The envelope where content has become invalidated.</param>
        public virtual void Invalidate(IEnvelope region)
        {
            if (_invalidatedRegion != null)
            {
                // This is set to null when we do the redrawing, so we would rather expand
                // the redraw region than forget to update a modified area.
                _invalidatedRegion.ExpandToInclude(region);
            }
            else
            {
                _invalidatedRegion = region;
            }            
        }

        /// <summary>
        /// Notifies parent layer that this item is invalid and should be redrawn.
        /// </summary>
        public override void Invalidate()
        {
            OnItemChanged(this);
        }
      

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the internal data set.  This can be null, as in the cases of groups or map-frames.
        /// Copying a layer should not create a duplicate of the dataset, but rather it should point to the 
        /// original dataset.  The ShallowCopy attribute is used so even though the DataSet itself may be cloneable,
        /// cloning a layer will treat the dataset like a shallow copy.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public IDataSet DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        /// <summary>
        /// Dynamic visibility represents layers that only appear when you zoom in close enough.
        /// This value represents the geographic width where that happens.
        /// </summary>
        [Serialize("DynamicVisibilityWidth")]
        [Category("Behavior"), Description("Dynamic visibility represents layers that only appear when the zoom scale is closer (or further) from a set scale.  This value represents the geographic width where the change takes place.")]
        public double DynamicVisibilityWidth
        {
            get { return _dynamicVisibilityWidth; }
            set { _dynamicVisibilityWidth = value; }
        }

        /// <summary>
        /// This controls whether the layer is visible when zoomed in closer than the dynamic 
        /// visiblity width or only when further away from the dynamic visibility width
        /// </summary>
        [Serialize("DynamicVisibilityMode")]
        [Category("Behavior"), Description("This controls whether the layer is visible when zoomed in closer than the dynamic visiblity width or only when further away from the dynamic visibility width")]
        public DynamicVisibilityModes DynamicVisiblityMode
        {
            get { return _dynamicVisibilityMode; }
            set { _dynamicVisibilityMode = value; }
        }

        /// <summary>
        /// Gets the currently invalidated region.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnvelope InvalidRegion
        {
            get { return _invalidatedRegion; }
            protected set { _invalidatedRegion = value; }
        }

       

        /// <summary>
        /// Gets the map frame of the parent LayerCollection.  
        /// </summary>
        [Browsable(false), ShallowCopy]
        public IFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }
            set
            {
                _mapFrame = value;
            }
        }


      
        /// <summary>
        /// Gets or sets the ProgressHandler for this layer
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set 
            { 
                _progressHandler = value;
                if (_progressMeter == null) _progressMeter = new ProgressMeter(_progressHandler);
                _progressMeter.ProgressHandler = value;
            }
        }



        /// <summary>
        /// Gets or sets a boolean indicating whether to allow the dynamic visibility
        /// envelope to control visibility.
        /// </summary>
        [Serialize("UseDynamicVisibility")]
        [Category("Behavior"), Description("Gets or sets a boolean indicating whether to allow the dynamic visibility envelope to control visibility.")]
        public bool UseDynamicVisibility
        {
            get { return _useDynamicVisibility; }
            set { _useDynamicVisibility = value; }
        }

        public override bool IsSelected
        {
            get
            {
                return base.IsSelected;
            }
            set
            {
                OnLayerSelected(this, value);
                base.IsSelected = value;
                
            }
        }
 
        #endregion

        #region Protected Methods

    


       
       
        /// <summary>
        /// Fires the zoom to layer event
        /// </summary>
        protected virtual void OnZoomToLayer()
        {
            if (ZoomToLayer == null) return;
            IEnvelope env = Envelope.Copy();
            ZoomToLayer(this, new EnvelopeArgs(env));
        }
        /// <summary>
        /// Fires the zoom to layer event, but specifies the extent.
        /// </summary>
        /// <param name="env">IEnvelope env</param>
        protected virtual void OnZoomToLayer(IEnvelope env)
        {
            if (ZoomToLayer != null)
            {
                ZoomToLayer(this, new EnvelopeArgs(env));
            }
        }

        #endregion




        #region EventHandlers

       
        private void ExportDataClick(object sender, EventArgs e)
        {
            OnExportData();
        }

      

        private void ShowPropertiesClick(object sender, EventArgs e)
        {
            // Allow subclasses to prevent this class from showing the default dialog
            HandledEventArgs result = new HandledEventArgs(false);
            OnShowProperties(result);
            if (result.Handled) return;
         
            _editCopy = this.Copy();
            CopyProperties(_editCopy); // for some reason we are getting blank layers during edits, this tries to fix that
            _propertyDialogProvider.ShowDialog(_editCopy);

           
            LayerManager.DefaultLayerManager.ActiveProjectLayers = new List<ILayer>();

          
        }

        #endregion

        #region Protected Methods

      
        /// <summary>
        /// Layers launch a "Property Grid" by default.  However, this can be overridden with a different UIEditor by this 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IPropertyDialogProvider PropertyDialogProvider
        {
            get
            {
                return _propertyDialogProvider;
            }
            protected set
            {
                _propertyDialogProvider = value;
            }
        }

       
        /// <summary>
        /// Occurs before showing the properties dialog.  If the handled member
        /// was set to true, then this class will not show the event args.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowProperties(HandledEventArgs e)
        {
            if (ShowProperties != null) ShowProperties(this, e);
        }


      

        /// <summary>
        /// This should be overridden to copy the symbolizer properties from editCopy
        /// </summary>
        /// <param name="editCopy">The version that went into the property dialog</param>
        protected override void OnCopyProperties(object editCopy)
        {

            ILayer layer = editCopy as ILayer;
            if (layer != null)
            {
                SuspendChangeEvent();
                base.OnCopyProperties(editCopy);
                ResumeChangeEvent();
            }
            
        }

        /// <summary>
        /// Zooms to the specific layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomToLayerClick(object sender, EventArgs e)
        {
            OnZoomToLayer();
        }

        /// <summary>
        /// Removes this layer from its parent list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLayerClick(object sender, EventArgs e)
        {
            OnRemoveItem();
        }

        
        /// <summary>
        /// Fires the LayerSelected event
        /// </summary>
        protected virtual void OnLayerSelected(ILayer sender, bool selected)
        {
            if (LayerSelected == null) return;
            LayerSelected(this, new LayerSelectedEventArgs(sender, selected));
        }

        /// <summary>
        /// Fires the OnFinishedLoading event.
        /// </summary>
        protected void OnFinishedLoading()
        {
            _isFinishedLoading = true;
            if (FinishedLoading != null) FinishedLoading(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when instructions are being sent for this layer to export data.
        /// </summary>
        protected virtual void OnExportData()
        {

        }

        /// <summary>
        /// special treatment for event handlers during a copy event
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
           
            // Remove event handlers from the copy. (They will be set again when adding to a new map.)
            Layer copyLayer = copy as Layer;
            if (copyLayer == null) return;
            if(copyLayer.LayerSelected != null)
            {
                foreach (var handler in copyLayer.LayerSelected.GetInvocationList())
                {
                    copyLayer.LayerSelected -= (EventHandler<LayerSelectedEventArgs>) handler;
                }
            }
            if(copyLayer.ZoomToLayer != null)
            {
                foreach (var handler in copyLayer.ZoomToLayer.GetInvocationList())
                {
                    copyLayer.ZoomToLayer -= (EventHandler<EnvelopeArgs>) handler;
                }
            }
            if(copyLayer.ShowProperties != null)
            {
                foreach (var handler in copyLayer.ShowProperties.GetInvocationList())
                {
                    copyLayer.ShowProperties -= (HandledEventHandler) handler;
                }
            }
            if (copyLayer.FinishedLoading != null)
            {
                foreach (var handler in copyLayer.FinishedLoading.GetInvocationList())
                {
                    copyLayer.FinishedLoading -= (EventHandler) handler;
                }
            }
            if (copyLayer.SelectionChanged != null)
            {
                foreach (var handler in copyLayer.SelectionChanged.GetInvocationList())
                {
                    copyLayer.SelectionChanged -= (EventHandler)handler;
                }
            }
            base.OnCopy(copy);
        }

        #endregion



        #region Protected Properties

        /// <summary>
        /// Gets or sets the progress meter being used internally by layer classes.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected ProgressMeter ProgressMeter
        {
            get { return _progressMeter; }
            set { _progressMeter = value; }
        }

        #endregion










        #region Static Methods

        /// <summary>
        /// Opens a filename using the default layer provider and returns a new layer.  The layer will not automatically have a container or be added to a map.
        /// </summary>
        /// <param name="filename">The string filename of the layer to open</param>
        /// <returns>An ILayer interface</returns>
        public static ILayer OpenFile(string filename)
        {
            if (System.IO.File.Exists(filename) == false) return null;
            return LayerManager.DefaultLayerManager.OpenLayer(filename);
        }

        /// <summary>
        /// Opens a filename using the default layer provider and returns a new layer.  The layer will not automatically have a container or be added to a map.
        /// </summary>
        /// <param name="filename">The string filename of the layer to open</param>
        /// <param name="progressHandler">An IProgresshandler that overrides the Default Layer Manager's progress handler</param>
        /// <returns>An ILayer interface with the new layer.</returns>
        public static ILayer OpenFile(string filename, IProgressHandler progressHandler)
        {
            return System.IO.File.Exists(filename) == false ? null : LayerManager.DefaultLayerManager.OpenLayer(filename, progressHandler);
        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        public static ILayer OpenFile(string filename, ICollection<ILayer> container)
        {
            if (System.IO.File.Exists(filename) == false) return null;
            ILayerManager dm = LayerManager.DefaultLayerManager;
            return dm.OpenLayer(filename, container);
        }

        /// <summary>
        /// Attempts to call the open filename method for any ILayerProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="filename">A String filename to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this LayerManager.</param>
        /// <param name="container">A container to open this layer in</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this LayerManager.</param>
        /// <returns>An ILayer</returns>
        public virtual ILayer OpenLayer(string filename, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            if (System.IO.File.Exists(filename) == false) return null;
            ILayerManager dm = LayerManager.DefaultLayerManager;
            return dm.OpenLayer(filename, inRam, container, progressHandler);
        }

      

        #endregion

        #region ISelectable Members

        
        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool Select(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// Gets or sets the boolean that controls whether or not items from the layer can be selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SelectionEnabled
        {
            get
            {
                return _selectionEnabled;
            }
            set
            {
                _selectionEnabled = value;
            }
        }

        /// <summary>
        /// Fires the SelectionChanged event
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
        }

        #endregion
    }




}
