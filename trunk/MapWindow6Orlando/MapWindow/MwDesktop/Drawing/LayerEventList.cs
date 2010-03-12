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
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using MapWindow.Geometries;
using MapWindow.Main;


namespace MapWindow.Drawing
{
    /// <summary>
    /// This extends the ChangeEventList by providing methods that allow access by an object Key, and will pass 
    /// </summary>
    public class LayerEventList<T> : ChangeEventList<T>, ILayerEventList<T> where T : class, ILayer
    {
        
        #region Events

        /// <summary>
        /// Occurs if the maps should zoom to this layer.
        /// </summary>
        public event EventHandler<EnvelopeArgs> ZoomToLayer;

        /// <summary>
        /// Occurs when one of the layers in this collection changes visibility.
        /// </summary>
        public event EventHandler LayerVisibleChanged;

        /// <summary>
        /// Occurs when a layer is added to this item.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Occurs when a layer is removed from this item.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerRemoved;

        /// <summary>
        /// Occurs immedately after a layer is selected.
        /// </summary>
        public event EventHandler<LayerSelectedEventArgs> LayerSelected;


        /// <summary>
        /// Occurs when the selection on a feature layer changes.
        /// </summary>
        public event EventHandler<FeatureLayerSelectionEventArgs> SelectionChanging;

        /// <summary>
        /// Occurs after the selection has changed, and all the layers have had their selection information updated.
        /// </summary>
        public event EventHandler SelectionChanged;

        #endregion
       

        #region Private Variables

        private bool _isInitialized; // True if layers already existed before a member change
        private ILayer _selectedLayer;
        private bool _layerAddedFired;

        #endregion

        #region Constructors


        #endregion

        #region Methods

        /// <summary>
        /// This selects the layer with the specified integer index
        /// </summary>
        /// <param name="index">THe zero based integer index</param>
        public void SelectLayer(int index)
        {
            _selectedLayer = this[index];
        }
       


        #endregion

        #region Properties

        /// <summary>
        /// Gets the currently selected layer in this collection.
        /// </summary>
        public ILayer SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                _selectedLayer = value;
                OnLayerSelected(_selectedLayer, _selectedLayer.IsSelected);
                //OnListChanged();
            }
        }


        /// <summary>
        /// The envelope that contains all of the layers for this data frame.  Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public virtual IEnvelope Envelope
        {
            get
            {
                IEnvelope env = new Envelope();
                if (Count == 0) return env;
                foreach(T lyr in this)
                {
                    env.ExpandToInclude(lyr.Envelope);
                }
                return env;
            }
        }


    


     
     


        #endregion


        /// <summary>
        /// Extends the event listeners to include events like ZoomToLayer and VisibleChanged
        /// </summary>
        /// <param name="item"></param>
        protected override void OnInclude(T item)
        {
            item.ZoomToLayer += Layer_ZoomToLayer;
            item.VisibleChanged += Layer_VisibleChanged;
            item.FinishedLoading += item_FinishedLoading;
            item.SelectionChanged += selectable_SelectionChanged;
            item.LayerSelected += item_LayerSelected;
            base.OnInclude(item);
            OnListChanged();
        }

        void item_LayerSelected(object sender, LayerEventArgs e)
        {
            // This will also automatically fire the event in the setter.
            SelectedLayer = e.Layer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void selectable_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        /// <summary>
        /// Occurs when the selection is changed
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if(SelectionChanged != null)SelectionChanged(this, new EventArgs());
        }
        

        /// <summary>
        /// Ensures that we re-draw the content when inserting new layers
        /// </summary>
        /// <param name="item"></param>
        protected override void OnIncludeComplete(T item)
        {
            OnLayerAdded(this, new LayerEventArgs(item));
        }

        /// <summary>
        /// Removes the extended event listeners once a layer is removed from this list.
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(T item)
        {
            item.ZoomToLayer -= Layer_ZoomToLayer;
            item.VisibleChanged -= Layer_VisibleChanged;
            item.FinishedLoading -= item_FinishedLoading;
            item.SelectionChanged -= selectable_SelectionChanged;
            item.LayerSelected -= item_LayerSelected;
            base.OnExclude(item);
            OnLayerRemoved(item);
            OnListChanged();
        }

        protected virtual void OnLayerRemoved(T item)
        {
            if(LayerRemoved != null)LayerRemoved(this, new LayerEventArgs(item));   
        }


        /// <summary>
        /// Handles the default selection behavior and fires the LayerSelected event.
        /// </summary>
        /// <param name="index">The integer index of the layer being selected</param>
        protected virtual void OnLayerSelected(int index)
        {
            _selectedLayer = base[index];
            if (LayerSelected != null)
            {
                LayerSelected(this, new LayerSelectedEventArgs(_selectedLayer, _selectedLayer.IsSelected));
            }
        }
        /// <summary>
        /// Fires the LayerSelected event and adjusts the selected state of the layer.
        /// </summary>
        /// <param name="layer">The layer to select</param>
        /// <param name="selected">Boolean, true if the specified layer is selected</param>
        protected virtual void OnLayerSelected(ILayer layer, bool selected)
        {
            if (LayerSelected != null)
            {
                LayerSelected(this, new LayerSelectedEventArgs(layer, selected));
            }
        }


        /// <summary>
        /// Fires the ItemChanged event and the MembersChanged event and resets any cached lists
        /// </summary>
        protected override void OnListChanged()
        {
            if (EventsSuspended)
            {
                base.OnListChanged();
                return;
            }
            if (_isInitialized == false)
            {
                if (Count > 0)
                {
                    _isInitialized = true;
                }
            }
            else
            {
                if (Count == 0)
                {
                    _isInitialized = false;
                }
            }
           
            // reset cached extra lists if any

            base.OnListChanged();
        }

        

        /// <summary>
        /// Fires the ZoomToLayer method when one of the layers fires its ZoomTo event
        /// </summary>
        /// <param name="sender">The layer to zoom to</param>
        /// <param name="e">The extent of the layer</param>
        protected virtual void OnZoomToLayer(object sender, EnvelopeArgs e)
        {
            // Just forward the event
            if(ZoomToLayer != null)ZoomToLayer(sender, e);
        }

        /// <summary>
        /// Fires the LayerAdded event
        /// </summary>
        /// <param name="sender">The layer that was added</param>
        /// <param name="e">LayerEventArgs</param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            if (EventsSuspended)
            {
                _layerAddedFired = true;
                return;
            }
            if (LayerAdded != null) LayerAdded(sender, e);
        }

        

        /// <summary>
        /// This occurs when the Resume events method is fired.
        /// </summary>
        protected override void OnResumeEvents()
        {
            if (_layerAddedFired) OnLayerAdded(this, new LayerEventArgs(null));
            base.OnResumeEvents();
        }

        /// <summary>
        /// Fires the selection changed event
        /// </summary>
        /// <param name="sender">the object sender of the event</param>
        /// <param name="args">The FeatureLayerSelectionEventArgs of the layer</param>
        protected virtual void OnSelectionChanging(object sender, FeatureLayerSelectionEventArgs args)
        {
            if (SelectionChanging != null) SelectionChanging(sender, args);
        }

    

    
        void item_FinishedLoading(object sender, EventArgs e)
        {
            OnLayerAdded(this, new LayerEventArgs(sender as ILayer));
        }

        private void Layer_VisibleChanged(object sender, EventArgs e)
        {
            if (LayerVisibleChanged != null) LayerVisibleChanged(sender, e);
        }

        private void Layer_ZoomToLayer(object sender, EnvelopeArgs e)
        {
            OnZoomToLayer(sender, e);
        }

        private void selectable_SelectionChanging(object sender, FeatureLayerSelectionEventArgs e)
        {
            OnSelectionChanging(sender, e);
        }
      
       

        

    }
}
