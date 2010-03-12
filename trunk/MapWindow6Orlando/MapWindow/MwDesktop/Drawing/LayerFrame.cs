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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Projections;
using MapWindow.Serialization;

namespace MapWindow.Drawing
{
    /// <summary>
    /// This is a class that organizes a list of renderable layers into a single "view" which might be
    /// shared by multiple displays.  For instance, if you have a map control and a print preview control,
    /// you could draw the same data frame property on both, just by passing the graphics object for each.
    /// Be sure to handle any scaling or translation that you require through the Transform property
    /// of the graphics object as it will ultimately be that scale which is used to back-calculate the
    /// appropriate pixel sizes for point-size, line-width and other non-georeferenced characteristics.
    /// </summary>
    public abstract class LayerFrame : Group, IFrame
    {
       
       

        #region Events


       
        /// <summary>
        /// Occurs when the visible region being displayed on the map should update
        /// </summary>
        public virtual event EventHandler UpdateMap;


        /// <summary>
        /// Occurs after zooming to a specific location on the map and causes a camera recent.
        /// </summary>
        public virtual event EventHandler<EnvelopeArgs> ExtentsChanged;


       
        

        #endregion

        #region Private Variables

        private System.Drawing.Drawing2D.SmoothingMode _smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        [Serialize("Extents")]
        private IEnvelope _extents;
        readonly IContainer _parent;
        private List<ILayer> _drawingLayers;
        private bool _preventExtentsChanged;
        private ProjectionInfo _projection;
        private bool _extentsInitialized;

        #endregion

        #region Constructors

        /// <summary>
        /// The Constructor for the MapFrame object
        /// </summary>
        protected LayerFrame()
        {
            Configure();
            
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        protected LayerFrame(IContainer container)
        {
            _parent = container;
            Configure();
        }

        private void Configure()
        {
            Layers = new LayerCollection(this);
            base.LegendText = MessageStrings.MapLayers;
            ContextMenuItems = new List<MenuItem>();
            ContextMenuItems.Add(new MenuItem("Remove Map Frame", Remove_Click));
            ContextMenuItems.Add(new MenuItem("Zoom to Map Frame", ZoomToMapFrame_Click));
            ContextMenuItems.Add(new MenuItem("Create Group", CreateGroup_Click));
            base.LegendSymbolMode = SymbolModes.GroupSymbol;
            LegendType = LegendTypes.Group;
            MapFrame = this;
            ParentGroup = this;
            _drawingLayers = new List<ILayer>();
        }

        private void CreateGroup_Click(object sender, EventArgs e)
        {
            OnCreateGroup();
        }


        /// <summary>
        /// Drawing layers are tracked separately, and do not appear in the legend.
        /// </summary>
        public List<ILayer> DrawingLayers
        {
            get { return _drawingLayers; }
            set { _drawingLayers = value; }
           
        }

        /// <summary>
        /// Draws the layers icon to the legend
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            g.DrawIcon(Images.Layers, box);
        }

        /// <summary>
        /// Overrides the default behavior for groups, which should return null in the 
        /// event that they have no layers, with a more tollerant, getting started
        /// behavior where geographic coordinates are assumed.
        /// </summary>
        public override IEnvelope Envelope
        {
            get
            {
                if (base.Envelope == null)
                {
                    return new Envelope(-180, 180, -90, 90);
                }
                return base.Envelope;
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            OnRemoveItem();   
            
        }


        private void ZoomToMapFrame_Click(object sender, EventArgs e)
        {
            Extents = Envelope;
        } 
        
      
        #endregion

        #region Methods

        /// <summary>
        /// Gets the container control that this MapFrame belongs to.
        /// </summary>
        public IContainer Parent
        {
            get { return _parent; }
        }


        #endregion

        #region Properties

        

        public bool ExtentsInitialized
        {
            get { return _extentsInitialized; }
            set { _extentsInitialized = value; }
        }
      
        /// <summary>
        /// Gets or sets the view extents in world coordinates, lat long for example.
        /// </summary>
        public virtual IEnvelope Extents
        {
            get { return _extents; }
            set
            {
                _extents = value; 
                OnExtentsChanged(this, new EnvelopeArgs(_extents));
            }
        }



        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        public override bool IsVisible
        {
            get { return base.IsVisible; }
            set
            {
                if (base.IsVisible != value)
                {
                    // switching values
                    base.IsVisible = value;
                    OnItemChanged();
                }
               
            }
        }

        /// <summary>
        /// This allows methods to specify precisely 
        /// </summary>
        protected bool PreventExtentsChanged
        {
            get { return _preventExtentsChanged; }
            set { _preventExtentsChanged = value; }
        }

        /// <summary>
        /// Gets or sets the projection that is being used by this map-frame.
        /// This is set by the first layer loaded.
        /// </summary>
        public ProjectionInfo Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// Gets the currently selected layer.  This will be an active layer that is used for operations.
        /// </summary>
        public ILayer SelectedLayer
        {
            get 
            {
                if (Layers != null)
                {
                    return Layers.SelectedLayer;
                }
                return null;
            }
        }


        /// <summary>
        /// Controls the smoothing mode.  Default or None will have faster performance
        /// at the cost of quality.
        /// </summary>
        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }



        /// <summary>
        /// The view extents describes the central, viewable portion of the frame, which is 1/3 the width and height of the whole frame
        /// </summary>
        public virtual IEnvelope ViewExtents
        {
            get
            {
                if (Extents != null)
                {
                    IEnvelope view = Extents.Copy();
                    view.ExpandBy(-Extents.Width / 3, -Extents.Height / 3);
                    return view;
                }
                return new Envelope();
            }
        }




        #endregion

        #region Event Handlers

       
        /// <summary>
        /// This adjusts the extents when ZoomToLayer is called in one of the internal layers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Layers_ZoomToLayer(object sender, EnvelopeArgs e)
        {
            Extents = e.Envelope;
        }

        /// <summary>
        /// Zooms to the envelope if no envelope has been established for this frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Layers_LayerAdded(object sender, LayerEventArgs e)
        {
            if (ExtentsInitialized) return;
            ExtentsInitialized = true;
            if(e.Layer != null)Extents = e.Layer.Envelope;
        }

        #endregion

       

        #region Protected Methods

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnExtentsChanged(object sender, EnvelopeArgs e)
        {
            if (_preventExtentsChanged) return;
            if (ExtentsChanged != null) ExtentsChanged(sender, e);
        }



        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        protected virtual void OnUpdateMap()
        {
            if (UpdateMap != null) UpdateMap(this, new EventArgs());
        }

        /// <summary>
        /// This is responsible for wiring the ZoomToLayer event from any layers
        /// in the map frame whenever the layer collection is chagned.
        /// </summary>
        /// <param name="collection"></param>
        protected override void Handle_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.ZoomToLayer += Layers_ZoomToLayer;
            collection.LayerAdded += Layers_LayerAdded;
            collection.LayerRemoved += Layers_LayerRemoved;
            base.Handle_Layer_Events(collection);
        }

        /// <summary>
        /// This is responsible for unwiring the ZoomToLayer event.
        /// </summary>
        /// <param name="collection"></param>
        protected override void Ignore_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.ZoomToLayer -= Layers_ZoomToLayer;
            collection.LayerAdded -= Layers_LayerAdded;
            collection.LayerRemoved -= Layers_LayerRemoved;
            base.Ignore_Layer_Events(collection);
        }

        private void Layers_LayerRemoved(object sender, LayerEventArgs e)
        {
            if(GetLayerCount(true) == 0)
            {
                ExtentsInitialized = false;
            }
        }

        #endregion

       


    }
}
