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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2009 5:59:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MapWindow.Components;
using MapWindow.Geometries;
using MapWindow.Main;

namespace MapWindow.Drawing
{


    /// <summary>
    /// Group
    /// </summary>
    public class Group : Layer, IGroup
    {
        #region Events

        /// <summary>
        /// This occurs when a new layer is added either to this group, or one of the child groups within this group.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// This occurs when a layer is removed from this group.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerRemoved;

        #endregion


        #region Private Variables

        private int _handle;
        private Image _image;
        private ILegend _legend;
        private IGroup _parentGroup;
        private bool _stateLocked;
        private bool _selectionEnabled;
        private ILayerCollection _layers;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Group
        /// </summary>
        public Group()
        {
            Configure();
        }
        /// <summary>
        /// Creates a group that sits in a layer list and uses the specified progress handler
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="progressHandler">the progress handler</param>
        public Group(IFrame frame, IProgressHandler progressHandler)
            : base(progressHandler)
        {
            MapFrame = frame;
            Configure();
        }
       
       
        /// <summary>
        /// Creates a group that sits in a layer list and uses the specified progress handler
        /// </summary>
        /// <param name="container">the layer list</param>
        /// <param name="frame"></param>
        /// <param name="progressHandler">the progress handler</param>
        public Group(ICollection<ILayer> container, IFrame frame, IProgressHandler progressHandler)
            : base(container, progressHandler)
        {
            MapFrame = frame;
            Configure();
        }

        private void Configure()
        {
            Layers = new LayerCollection(MapFrame, this);
            _stateLocked = false;
            IsDragable = true;
            base.IsExpanded = true;
            ContextMenuItems = new List<MenuItem>();
            ContextMenuItems.Add(new MenuItem("Remove Group", Remove_Click));
            ContextMenuItems.Add(new MenuItem("Zoom to Group", ZoomToGroup_Click));
            ContextMenuItems.Add(new MenuItem("Create new Group", CreateGroup_Click));
            _selectionEnabled = true;
        }



        #endregion

        #region Methods

        public virtual void Add(ILayer layer)
        {
            _layers.Add(layer);
        }

        public virtual bool Remove(ILayer layer)
        {
            return _layers.Remove(layer);
        }

        public virtual void Insert(int index, ILayer layer)
        {
            _layers.Insert(index, layer);
        }


        public override Size GetLegendSymbolSize()
        {
            return new Size(16, 16);
        }

        /// <summary>
        /// As a group, MapFrames can either receive layers or groups, but not
        /// sub-members like symbolizers or icons.
        /// </summary>
        /// <param name="item">The ILegendItem to receive</param>
        /// <returns>Boolean that is true if the item can be received</returns>
        public override bool CanReceiveItem(ILegendItem item)
        {
            ILayer lyr = item as ILayer;
            if (lyr != null)
            {
                if(lyr != this) return true; // don't allow groups to add to themselves
            }
            return false;
        }

        
        /// <summary>
        /// Returns a snapshot image of this group
        /// </summary>
        /// <param name="imgWidth">Width in pixels of the returned image (height is determined by the number of layers in the group)</param>
        /// <returns>Bitmap of the group and sublayers (expanded)</returns>
        public Bitmap Legend_SnapShot(int imgWidth)
        {
            bool TO_DO_GROUP_LEGEND_SNAPSHOT;
            return new Bitmap(100, 100);
        }

        /// <summary>
        /// Invalidates the specified region in each of the layers in this group.
        /// </summary>
        /// <param name="region">The envelope region</param>
        public override void Invalidate(IEnvelope region)
        {
            foreach (ILayer layer in GetLayers())
            {
                layer.Invalidate(region);
            }
        }

        /// <summary>
        /// Overrides the basic behavior for Invalidation to ensure that each of the
        /// sub layers also are invalidated.
        /// </summary>
        public override void Invalidate()
        {
            foreach (ILayer layer in GetLayers())
            {
                layer.Invalidate();
            }
        }

        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        /// <param name="affectedAreas">The region of the selection that was cleared</param>
        /// <returns>Boolean, true if any sub-members of the group were changed by the clear method</returns>
        public override bool ClearSelection(out IEnvelope affectedAreas)
        {
            affectedAreas = new Envelope();
            bool changed = false;
            if (!_selectionEnabled) return false;
            MapFrame.SuspendEvents();
            foreach (ILayer layer in GetLayers())
            {
                IEnvelope layerArea;
                if(layer.ClearSelection(out layerArea)) changed = true;
                affectedAreas.ExpandToInclude(layerArea);
            }
            MapFrame.ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return changed;
        }

    

        public override bool Select(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            MapFrame.SuspendEvents();
            
            foreach (ILayer s in GetLayers())
            {
                if (s.SelectionEnabled == false) continue;
                IEnvelope layerArea;
                
                if(s.Select(tolerant, strict, mode, out layerArea)) somethingChanged = true;
                
                
                affectedArea.ExpandToInclude(layerArea);
               
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            MapFrame.ResumeEvents();
            sw.Stop();
            Debug.WriteLine("ResumeEvents: " + sw.ElapsedMilliseconds);
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
           
            return somethingChanged;
        }

        /// <summary>
        /// Gets the layers cast as ILayer without any information about the actual drawing methods.
        /// This is useful for handling methods that my come from various types of maps.
        /// </summary>
        /// <returns>An enumerable collection of ILayer</returns>
        public virtual IList<ILayer> GetLayers()
        {
            return _layers.Cast<ILayer>().ToList();
        }



        /// <summary>
        /// Returns the number of data layers, not counting groups.  If recursive is true, then layers that are within
        /// groups will be counted, even though the groups themselves are not.
        /// </summary>
        /// <param name="recursive">Boolean, if true forces checking even the number of child members.</param>
        /// <returns>An integer representing the total number of layers in this collection and its children.</returns>
        public int GetLayerCount(bool recursive)
        {
            // if this is not overridden, this just looks at the Layers collection
            int count = 0;
            IList<ILayer> layers = GetLayers();
            foreach (ILayer item in layers)
            {
                IGroup grp = item as IGroup;
                if(grp != null)
                {
                    if (recursive) count += grp.GetLayerCount(true);
                }
                else
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region to invert the selected state of members</param>
        /// <param name="mode">The selection mode determining how to test for intersection</param>
        /// <param name="strict">The envelope when polygons are being selected that don't need to buffer the envelope.</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members</param>
        /// <returns>boolean, true if members were changed by the selection process.</returns>
        public override bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            MapFrame.SuspendEvents();
            foreach (ILayer s in GetLayers())
            {
                IEnvelope layerArea;
                if (s.InvertSelection(tolerant, strict, mode, out layerArea)) somethingChanged = true;
                affectedArea.ExpandToInclude(layerArea);
            }
            MapFrame.ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs in cases like clicking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region in cases like selecting polygons where no tolerance is allowed</param>
        /// <param name="mode">The selection mode</param>
        /// <param name="affectedArea">The envelope affected area</param>
        /// <returns>Boolean, true if any members were added to the selection</returns>
        public override bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionModes mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            SuspendEvents();
            foreach (ILayer s in GetLayers())
            {
                IEnvelope layerArea;
                if (s.UnSelect(tolerant, strict, mode, out layerArea)) somethingChanged = true;
                affectedArea.ExpandToInclude(layerArea);
            }
            ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        public virtual void SuspendEvents()
        {
            _layers.SuspendEvents();
        }

        public virtual void ResumeEvents()
        {
            _layers.ResumeEvents();
        }

        public virtual bool EventsSuspended
        {
            get { return _layers.EventsSuspended; }
        }


        #endregion

        #region Properties

        /// <summary>
        /// Boolean, true if any sub-layers in the group are visible.  Setting this
        /// will force all the layers in this group to become visible.
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                foreach (ILayer lyr in GetLayers())
                {
                    if(lyr.IsVisible) return true;
                }
                return false;
            }
            set
            {
                foreach (ILayer lyr in GetLayers())
                {
                    lyr.IsVisible = value;
                }
                base.IsVisible = value;
                
            }
        }

        /// <summary>
        /// The envelope that contains all of the layers for this data frame.  Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public override IEnvelope Envelope
        {
            get
            {
                if (GetLayers() == null || GetLayers().Count() == 0) return null;
               
                IEnvelope env = GetLayers().First().Envelope;
                foreach (ILayer layer in GetLayers())
                {
                    env.ExpandToInclude(layer.Envelope);
                }

                return env;
            }

        }

        /// <summary>
        /// Gets the integer handle for this group
        /// </summary>
        public int Handle
        {
            get { return _handle; }
            protected set { _handle = value; }
        }

        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        public Image Icon
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Gets the currently invalidated region as a union of all the 
        /// invalidated regions of individual layers in this group.
        /// </summary>
        public override IEnvelope InvalidRegion
        {
            get 
            {
                IEnvelope result = new Envelope();
                foreach (ILayer lyr in GetLayers())
                {
                    if (lyr.InvalidRegion != null) result.ExpandToInclude(lyr.InvalidRegion);
                }
                return result;
            }
           
        }


        /// <summary>
        /// Gets the layer handle of the specified layer
        /// </summary>
        /// <param name="PositionInGroup">0 based index into list of layers</param>
        /// <returns>Layer's handle on success, -1 on failure</returns>
        public int LayerHandle(int PositionInGroup)
        {
            throw new NotSupportedException();
        }



        /// <summary>
        /// gets or sets the list of layers.
        /// </summary>
        [ShallowCopy]
        protected ILayerCollection Layers
        {
            get
            {
                return _layers;
            }
            set
            {
                if (Layers != null)
                {
                    Ignore_Layer_Events(_layers);
                }
                Handle_Layer_Events(value);
                _layers = value;
            }
        }

        /// <summary>
        /// Returns the count of the layers that are currently stored in this map frame.
        /// </summary>
        public int LayerCount
        {
            get { return _layers.Count; }
        }

        /// <summary>
        /// Gets a boolean that is true if any of the immediate layers or groups contained within this
        /// control are visible.  Setting this will set the visibility for each of the members of this
        /// map frame.
        /// </summary>
        public bool LayersVisible
        {
            get
            {
                foreach (ILayer layer in GetLayers())
                {
                    if (layer.IsVisible) return true;
                }
                return false;
            }
            set
            {
                foreach (ILayer layer in GetLayers())
                {
                    layer.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets the legend that this group belongs to... regardless of how deep the item is.
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            protected set { _legend = value; }
        }

        /// <summary>
        /// This is a different view of the layers cast as legend items.  This allows
        /// easier cycling in recursive legend code.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                return _layers.Cast<ILegendItem>();
            }
        }


        /// <summary>
        /// Gets the parent group of this group. 
        /// </summary>
        public IGroup ParentGroup
        {
            get { return _parentGroup; }
            protected set { _parentGroup = value; }
        }

        /// <summary>
        /// Gets or sets the progress handler to use.  Setting this will set the progress handler for
        /// each of the layers in this map frame.
        /// </summary>
        public override IProgressHandler ProgressHandler
        {
            get { return base.ProgressHandler; }
            set
            {
                base.ProgressHandler = value;
                foreach (ILayer layer in GetLayers())
                {
                    layer.ProgressHandler = value;
                }
            }
        }

     

        /// <summary>
        /// gets or sets the locked property, which prevents the user from changing the visual state 
        /// except layer by layer
        /// </summary>
        public bool StateLocked
        {
            get { return _stateLocked; }
            set { _stateLocked = value; }
        }

      

        #endregion


      

       
        #region Private Methods

        private void Remove_Click(object sender, EventArgs e)
        {
            OnRemoveItem();

        }

        private void ZoomToGroup_Click(object sender, EventArgs e)
        {
            OnZoomToLayer(Envelope);
        }

        private void CreateGroup_Click(object sender, EventArgs e)
        {
            OnCreateGroup();
        }

        /// <summary>
        /// Creates an overrideable method when sub-groups are being created
        /// </summary>
        protected virtual void OnCreateGroup()
        {
            Group grp = new Group(Layers, MapFrame, ProgressHandler);
            grp.LegendText = "New Group";
        }

        private void Layers_ItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        private void Layers_LayerVisibleChanged(object sender, EventArgs e)
        {
            OnVisibleChanged(sender, e);
        }

        private void Layers_ZoomToLayer(object sender, EnvelopeArgs e)
        {
            OnZoomToLayer(e.Envelope);

        }

        /// <summary>
        /// Given a new LayerCollection, we need to be sensitive to certain events
        /// </summary>
        protected virtual void Handle_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.LayerVisibleChanged += Layers_LayerVisibleChanged;
            collection.ItemChanged += Layers_ItemChanged;
            collection.ZoomToLayer += Layers_ZoomToLayer;
            collection.SelectionChanging += collection_SelectionChanging;
            collection.LayerSelected += collection_LayerSelected;
            collection.LayerAdded += Layers_LayerAdded;
            collection.LayerRemoved += Layers_LayerRemoved;
            
        }

        void collection_LayerSelected(object sender, LayerSelectedEventArgs e)
        {
            OnLayerSelected(e.Layer, e.IsSelected);
        }

    

        void collection_SelectionChanging(object sender, FeatureLayerSelectionEventArgs e)
        {
            OnSelectionChanged();
        }

        private void Layers_LayerRemoved(object sender, LayerEventArgs e)
        {
            OnLayerRemoved(sender, e);
        }

        protected virtual void OnLayerRemoved(object sender, LayerEventArgs e)
        {
            if (LayerRemoved != null) LayerRemoved(sender, e);
        }

        private void Layers_LayerAdded(object sender, LayerEventArgs e)
        {
            OnLayerAdded(sender, e);    
        }

        /// <summary>
        /// Simply echo this event out to members above this group that might be listening to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            if (LayerAdded != null) LayerAdded(sender, e);
        }

       
        /// <summary>
        /// When setting an old layer collection it is advisable to not only add
        /// new handlers to the new collection, but remove the handlers related
        /// to the old collection.
        /// </summary>
        /// <param name="collection"></param>
        protected virtual void Ignore_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.LayerVisibleChanged -= Layers_LayerVisibleChanged;
            collection.ItemChanged -= Layers_ItemChanged;
            collection.ZoomToLayer -= Layers_ZoomToLayer;
            collection.SelectionChanging -= collection_SelectionChanging;
            collection.LayerSelected -= collection_LayerSelected;
            collection.LayerAdded -= Layers_LayerAdded;
            collection.LayerRemoved -= Layers_LayerRemoved;
        }

        #endregion



        #region IList<ILayer> Members

        public virtual int IndexOf(ILayer item)
        {
            return _layers.IndexOf(item);
        }

        public virtual void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

        public virtual ILayer this[int index]
        {
            get
            {
                return _layers[index];
            }
            set
            {
                _layers[index] = value;
            }
        }

        #endregion

        #region ICollection<ILayer> Members


        public virtual void Clear()
        {
            _layers.Clear();
        }

        public virtual bool Contains(ILayer item)
        {
            return _layers.Contains(item);
        }

        public virtual void CopyTo(ILayer[] array, int arrayIndex)
        {
            _layers.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return _layers.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return _layers.IsReadOnly; }
        }

       

        #endregion

        #region IEnumerable<ILayer> Members

        public virtual IEnumerator<ILayer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        #endregion
    }
}
