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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2008 9:37:01 AM
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
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Serialization;
using Point=MapWindow.Geometries.Point;

namespace MapWindow.Map
{


    /// <summary>
    /// A MapFrame accomplishes two things.  Firstly, it organizes the layers to be drawn, and establishes the geographic
    /// extents.  Secondly, it hosts the back-buffer image that can be larger than the component that
    /// this map frame would normally be drawn to.  When it receives instructions to paint itself, the client rectangle
    /// will automatically end up behaving like a clip rectangle.
    /// 
    /// </summary>
    public class MapFrame : LayerFrame, IMapFrame
    {
        /// <summary>
        /// Transforms an IMapLayer enumerator into an ILayer Enumerator
        /// </summary>
        public class MapLayerEnumerator : IEnumerator<ILayer>
        {
            private readonly IEnumerator<IMapLayer> _enumerator;

            /// <summary>
            /// Creates a new instance of the MapLayerEnumerator
            /// </summary>
            /// <param name="subEnumerator"></param>
            public MapLayerEnumerator(IEnumerator<IMapLayer> subEnumerator)
            {
                _enumerator = subEnumerator;
            }

            #region IEnumerator<ILayer> Members

            public ILayer Current
            {
                get { return _enumerator.Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return _enumerator.Current; }
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }

            #endregion
        }

        #region Events

        /// <summary>
        /// Occurs after changes have been made to the back buffer that affect the viewing area of the screen,
        /// thereby requiring an invalidation.
        /// </summary>
        public event EventHandler ScreenUpdated;

        /// <summary>
        /// Occurs after every one of the zones, chunks and stages has finished rendering to a stencil.
        /// </summary>
        public event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs when the buffer content has been altered and any containing maps should quick-draw
        /// from the buffer, followed by the tool drawing.
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;


        #endregion

        #region Private Variables


        private Image _buffer;
        private Image _backBuffer;
        private int _currentChunk;
        private Rectangle _view;
        private List<Rectangle> _clipRegions;
        private int _width;
        private int _height;
        private Control _parent;
        private bool _resizing;
        private Rectangle _originalView;
        private bool _isPanning;
        private Timer _chunkTimer;
        private bool _extendBuffer;
        private IMapLayerCollection _layers;
       
        #endregion

        #region Constructors

        /// <summary>
        /// Creates teh default map frame, allowing the control that it belongs to to be set later.
        /// </summary>
        public MapFrame()
        {
            base.Extents = new Envelope(-180, 180, -90, 90);
            Configure();
        }

        /// <summary>
        /// Creates a new instance of a MapFrame without specifying the extents.  The
        /// geographic extents of the world will be used.
        /// </summary>
        /// <param name="inParent">The parent control that should own this map frame.</param>
        public MapFrame(Control inParent)
        {
            _parent = inParent;
            base.Extents = new Envelope(-180, 180, -90, 90);
            Configure();
        }

        /// <summary>
        /// Creates a new instance of MapFrame
        /// </summary>
        public MapFrame(Control inParent, IEnvelope inEnvelope)
        {
            _parent = inParent;
            base.Extents = inEnvelope;
            Configure();
           
        }
        private void Configure()
        {
            CreateBuffer();
            Layers = new MapLayerCollection(this);
            _chunkTimer = new Timer();
            _chunkTimer.Interval = 1;
            base.IsSelected = true;  // by default allow the map frame to be selected
        }

    
        private void CreateBuffer()
        {
            if(_parent != null)
            {
                _width = _parent.ClientSize.Width;
                _height = _parent.ClientSize.Height;
            }
            else
            {
                _width = 1000;
                _height = 800;
            }
           
            if (_extendBuffer)
            {
                _width = _width * 3;
                _height = _height * 3;
            }
           
            _backBuffer = new Bitmap(_width, _height);
            
        }

        public override int IndexOf(ILayer item)
        {
            IMapLayer ml = item as IMapLayer;
            if (ml != null)
            {
                return _layers.IndexOf(ml);
            }
            return -1;
        }

        public override void Add(ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            if (ml != null)
            {
                _layers.Add(ml);
            }
        }

        public override ILayer this[int index]
        {
            get
            {
                return _layers[index];
            }
            set
            {
                IMapLayer ml = value as IMapLayer;
                _layers[index] = ml;
            }
        }

        public override void Clear()
        {
            _layers.Clear();
        }


        public override IEnumerator<ILayer> GetEnumerator()
        {
            return new MapLayerEnumerator(_layers.GetEnumerator());
        }



        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }


        public override bool Contains(ILayer item)
        {
            IMapLayer ml = item as IMapLayer;
            return ml != null && _layers.Contains(ml);
        }

        public override bool Remove(ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            return ml != null && _layers.Remove(ml);
        }

        public override void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

        public override void Insert(int index, ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            if (ml != null)
            {
                _layers.Insert(index, ml);
            }

        }

        public override void SuspendEvents()
        {
            _layers.SuspendEvents();
        }

        public override void ResumeEvents()
        {
            _layers.ResumeEvents();
        }

        public override bool EventsSuspended
        {
            get { return _layers.EventsSuspended; }
        }

        /// <summary>
        /// Overrides the group creation to make sure that the new group will cast its layers
        /// to the appopriate geoLayer type.
        /// </summary>
        protected override void OnCreateGroup()
        {
            MapGroup grp = new MapGroup(Layers, this, ProgressHandler);
            grp.LegendText = "New Group";
        }

        /// <summary>
        /// This is not called during a resize, but rather after panning or zooming where the 
        /// view is used as a guide to update the extents.  This will also call ResetBuffer.
        /// </summary>
        public virtual void ResetExtents()
        {
            // Find the geographic extents that would be centered on the current view
            IEnvelope env = BufferToProj(View);
            if(_extendBuffer) env.ExpandBy(env.Width, env.Height);
            Extents = env;
        }

        


        /// <summary>
        /// Re-creates the buffer based on the size of the control without changing
        /// the geographic extents.  This is used after a resize operation.
        /// </summary>
        public virtual void ResetBuffer()
        {
           
            CreateBuffer();
            // reset the view rectangle to represent the same region
            if (_extendBuffer)
            {
                _view = new Rectangle(_width / 3, _height / 3, _width / 3, _height / 3);
            }
            else
            {
                _view = new Rectangle(0, 0, _width, _height);
            }
           

            Initialize();
 
            
        }

        /// <summary>
        /// Instructs the map frame to draw content from the specified regions to the buffer..
        /// </summary>
        /// <param name="regions">The regions to initialize.</param>
        public void Initialize(List<IEnvelope> regions)
        {

            if (_backBuffer == null) _backBuffer = new Bitmap(_width, _height);
            Graphics bufferDevice = Graphics.FromImage(_backBuffer);
            MapArgs args = new MapArgs(ClientRectangle, Extents, bufferDevice);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            foreach (IEnvelope region in regions)
            {
                if(region == null) continue;
                Rectangle rect = this.ProjToPixel(region);
                gp.StartFigure();
                gp.AddRectangle(rect);
            }
            bufferDevice.Clip = new Region(gp);
            bufferDevice.Clear(Color.White);
            
            
            // First draw all the vector content
            foreach (IMapLayer layer in Layers)
            {
                if(layer.VisibleAtExtent(Extents))layer.DrawRegions(args, regions);
            }
            // Then
            MapLabelLayer.ExistingLabels = new List<RectangleF>();
            foreach (IMapLayer layer in Layers)
            {
                InitializeLabels(regions, args, layer);
            }

            // First draw all the vector content
            foreach (IMapLayer layer in DrawingLayers)
            {
                if(layer.VisibleAtExtent(Extents))layer.DrawRegions(args, regions);
            }

            if (_buffer != null && _buffer != _backBuffer) _buffer.Dispose();
            _buffer = _backBuffer;
            bufferDevice.Clip = new Region(ImageRectangle);
            gp.Dispose();
            List<Rectangle> rects = args.ProjToPixel(regions);
            OnBufferChanged(this, new ClipArgs(rects));

        }

        private static void InitializeLabels(List<IEnvelope> regions, MapArgs args, IRenderable layer)
        {
           
            IMapGroup grp = layer as IMapGroup;
            if (layer.IsVisible)
            {
                if (grp != null)
                {
                    foreach (IMapLayer lyr in grp.Layers)
                    {
                        InitializeLabels(regions, args, lyr);
                    }
                    return;
                }

                

                IMapFeatureLayer mfl = layer as IMapFeatureLayer;
                if (mfl != null)
                {
                    if (mfl.ShowLabels && mfl.LabelLayer != null)
                    {
                        if (mfl.LabelLayer.VisibleAtExtent(args.GeographicExtents)) mfl.LabelLayer.DrawRegions(args, regions);
                    }
                }
            }
        }

        /// <summary>
        /// Uses the current buffer and envelope to force each of the contained layers
        /// to re-draw their content.  This is useful after a zoom or size change.
        /// </summary>
        public virtual void Initialize()
        {
            Initialize(new List<IEnvelope> { Extents });
            OnBufferChanged(this, new ClipArgs(ClientRectangle));
        }

        /// <summary>
        /// Pans the image for this map frame.  Instead of drawing entirely new content, from all 5 zones,
        /// just the slivers of newly revealed area need to be re-drawn.
        /// </summary>
        /// <param name="shift">A System.Drawing.Point showing the amount to shift in pixels</param>
        public virtual void Pan(System.Drawing.Point shift)
        {
           
            _isPanning = true;
            _backBuffer = new Bitmap(_width, _height);

            Graphics g = Graphics.FromImage(_backBuffer);

            g.DrawImageUnscaled(_buffer, -shift.X, -shift.Y);
            g.Dispose();

            IEnvelope env = BufferToProj(View);
            if (_extendBuffer)
            {
                _view = new Rectangle(_width / 3, _height / 3, _width / 3, _height / 3);
                env.ExpandBy(env.Width, env.Height);
            }
            else
            {
                _view = new Rectangle(0, 0, _width, _height);
            }
            PreventExtentsChanged = true;
            base.Extents = env;
            PreventExtentsChanged = false;

            List<Rectangle> clipRects = new List<Rectangle>();
            if (shift.X > 0) 
            {
                clipRects.Add(new Rectangle(Width - shift.X, 0, shift.X, Height));
                if (shift.Y > 0)
                {
                    clipRects.Add(new Rectangle(0, Height - shift.Y, Width - shift.X, shift.Y));
                }
                else
                {
                    clipRects.Add(new Rectangle(0, 0, Width - shift.X, Math.Abs(shift.Y)));
                }
            }
            else 
            {
                clipRects.Add(new Rectangle(0, 0, Math.Abs(shift.X), Height));
                int dx = Math.Abs(shift.X);
                if (shift.Y > 0)
                {
                    clipRects.Add(new Rectangle(dx, Height - shift.Y, Width - dx, shift.Y));
                }
                else
                {
                    clipRects.Add(new Rectangle(dx, 0, Width - dx, Math.Abs(shift.Y)));
                }
            }

            List<IEnvelope> regions = this.PixelToProj(clipRects);
            Initialize(regions);
            _isPanning = false;
            OnExtentsChanged(this, new EnvelopeArgs(base.Extents));
            
        }

        /// <summary>
        /// When content in a geographic region needs to be invalidated
        /// </summary>
        /// <param name="region"></param>
        public override void Invalidate(IEnvelope region)
        {
            foreach (IMapLayer layer in Layers)
            {
                layer.Invalidate(region);
            }
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
        
        public override IList<ILayer> GetLayers()
        {
            return _layers.Cast<ILayer>().ToList();
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="targetRectangle"></param>
        public void Print(Graphics device, Rectangle targetRectangle)
        {
            Print(device, targetRectangle, Extents);
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="targetRectangle"></param>
        /// <param name="targetEnvelope">the extents to draw to the target rectangle</param>
        public void Print(Graphics device, Rectangle targetRectangle, IEnvelope targetEnvelope)
        {
            MapArgs args = new MapArgs(targetRectangle, targetEnvelope, device);
            System.Drawing.Drawing2D.Matrix oldMatrix = device.Transform;
            List<IEnvelope> regions = new List<IEnvelope>();
            regions.Add(targetEnvelope);
            try
            {
                device.TranslateTransform(targetRectangle.X, targetRectangle.Y);

                foreach (IMapLayer ml in Layers)
                {
                    PrintLayer(ml, args);
                }
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                device.Transform = oldMatrix;
            }
        }

        private void PrintLayer(IMapLayer layer, MapArgs args)
        {
            MapLabelLayer.ExistingLabels.Clear();
            IMapGroup group = layer as IMapGroup;
            if (group != null)
            {
                foreach (IMapLayer subLayer in group.Layers)
                {
                    PrintLayer(subLayer, args);
                }
            }

            IMapLayer geoLayer = layer;
            if (geoLayer != null)
            {
                if (geoLayer.UseDynamicVisibility)
                {
                    if (Extents.Width > geoLayer.DynamicVisibilityWidth)
                    {
                        return;  // skip the geoLayer if we are zoomed out too far.
                    }
                }

                if (geoLayer.IsVisible == false) return;

                geoLayer.DrawRegions(args, new List<IEnvelope> { args.GeographicExtents });
                

                IMapFeatureLayer mfl = geoLayer as IMapFeatureLayer;
                if (mfl != null)
                {
                    if(mfl.UseDynamicVisibility)
                    {
                        if (Extents.Width > mfl.DynamicVisibilityWidth) return;
                    }
                    if (mfl.ShowLabels && mfl.LabelLayer != null)
                    {
                        if(mfl.LabelLayer.UseDynamicVisibility)
                        {
                            if (Extents.Width > mfl.LabelLayer.DynamicVisibilityWidth) return;
                        }
                        mfl.LabelLayer.DrawRegions(args, new List<IEnvelope> { args.GeographicExtents });
                    }
                }
            }
        }

        /// <summary>
        /// Forces this MapFrame to copy the buffers for its layers to the back-buffer.
        /// </summary>
        public override void Invalidate()
        {
            Initialize();
           // Draw_Layer_Stencils_To_Buffer(new List<Rectangle>() { new Rectangle(0, 0, _buffer.Width, _buffer.Height) });
        }

      


        ///// <summary>
        ///// This assumes that the content on the buffers of the previous layers
        ///// </summary>
        //private void UninitializeLayers()
        //{
        //    foreach (IGeoLayer layer in Layers)
        //    {
        //        layer.IsInitialized = false;
        //    }
        //}

        // Obsolete
        //private void DrawZones(Graphics bufferDevice, Rectangle clipRectangle)
        //{
        //    Pen p = null;
        //    switch (_zone)
        //    {
        //        case Zones.Center: p = new Pen(Color.Green); break;
        //        case Zones.East: p = new Pen(Color.Red); break;
        //        case Zones.West: p = new Pen(Color.Purple); break;
        //        case Zones.North: p = new Pen(Color.Blue); break;
        //        case Zones.South: p = new Pen(Color.Yellow); break;
        //        default: p = new Pen(Color.Black); break;
        //    }
        //    bufferDevice.DrawRectangle(p, new Rectangle(clipRectangle.X, clipRectangle.Y, clipRectangle.Width - 1, clipRectangle.Height - 1));
        //    bufferDevice.DrawLine(p, new System.Drawing.Point(clipRectangle.X, clipRectangle.Y), new System.Drawing.Point(clipRectangle.Right, clipRectangle.Bottom));
        //    bufferDevice.DrawLine(p, new System.Drawing.Point(clipRectangle.X, clipRectangle.Bottom), new System.Drawing.Point(clipRectangle.Right, clipRectangle.Y));
        //    p.Dispose();

        //}

        #endregion

        #region Component Designer generated code

        

        #endregion

        #region Methods

        /// <summary>
        /// Obtains a rectangle relative to the background image by comparing
        /// the current View rectangle with the parent control's size.
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public Rectangle ParentToView(Rectangle clip)
        {
            Rectangle result = new Rectangle();
            result.X = View.X + (clip.X * View.Width) / _parent.ClientRectangle.Width;
            result.Y = View.Y + (clip.Y * View.Height) / _parent.ClientRectangle.Height;
            result.Width = clip.Width * View.Width / _parent.ClientRectangle.Width;
            result.Height = clip.Height * View.Height / _parent.ClientRectangle.Height;
            return result;
        }

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public void ZoomIn()
        {
            Rectangle r = View;
            r.Inflate(-r.Width / 4, -r.Height / 4);
            View = r;
            ResetExtents();
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public void ZoomOut()
        {
            Rectangle r = View;
            r.Inflate(r.Width / 2, r.Height / 2);
            View = r;
            ResetExtents();
        }
       
        #endregion

        #region Properties

        
        /// <summary>
        /// Gets or sets the integer that specifies the chunk that is actively being drawn
        /// </summary>
        public int CurrentChunk
        {
            get { return _currentChunk; }
            set 
            { 
                _currentChunk = value;
            }
        }

        /// <summary>
        /// This gets or sets the first clipRegion.  If more than one region needs to be set,
        /// the ClipRegions list should be used instead.
        /// </summary>
        public Rectangle ClipRectangle
        {
            get 
            {
                if (_clipRegions == null)
                {
                    _clipRegions = new List<Rectangle>();
                    _clipRegions.Add(ClientRectangle);
                }
                return _clipRegions[0]; 
            }
            set
            {
                if (_clipRegions == null)
                {
                    _clipRegions = new List<Rectangle>();
                }
                _clipRegions.Clear();
                _clipRegions.Add(value);
            }
        }

        /// <summary>
        /// Gets or sets a set of rectangles that should be drawn during the next update
        /// </summary>
        public List<Rectangle> ClipRegions
        {
            get { return _clipRegions; }
            set { _clipRegions = value; }
        }

        /// <summary>
        /// Gets or sets the geogrpahic extents to be drawn to a buffer.
        /// If "ExtendBuffer" is true, then these extents are larger
        /// than the geographic extents of the parent client,
        /// and care should be taken when using PixelToProj,
        /// as it will work differently.
        /// </summary>
        public override IEnvelope Extents
        {
            get { return base.Extents; }
            set 
            {
                if (value == null) return;
                IEnvelope env = value.Copy();
                ResetAspectRatio(env);
                base.Extents = env;
                ResetBuffer();
            }
        }

        /// <summary>
        /// Gets the geographic extents
        /// </summary>
        public IEnvelope GeographicExtents
        {
            get
            {
                return BufferToProj(View);
            }
        }
        

        /// <summary>
        /// Gets or sets whether this map frame should define its buffer
        /// region to be the same size as the client, or three times larger.
        /// </summary>
        [Serialize("ExtendBuffer")]
        public bool ExtendBuffer
        {
            get { return _extendBuffer; }
            set { _extendBuffer = value; }
        }

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

       

        /// <summary>
        /// Gets or sets whether this map frame is currently in the process of redrawing the
        /// stencils after a pan opperation.  Drawing should not take place if this is true.
        /// </summary>
        public bool IsPanning
        {
            get { return _isPanning; }
            set { _isPanning = value; }
        }

      
        /// <summary>
        /// If a BackBuffer.Extents exists, this will enlarge those extents to match the aspect ratio
        /// of the pixel view.  If one doesn't exist, the _mapFrame.Extents will be used instead.
        /// </summary>
        /// <param name="newEnv">The envelope to adjust</param>
        /// <returns>Boolean, true if the aspect was able to be set.  False otherwise.</returns>
        protected void ResetAspectRatio(IEnvelope newEnv)
        {

            // ---------- Aspect Ratio Handling
            if (newEnv == null) return;
            double h = Parent.Height;
            double w = Parent.Width;
            //double h = _height;
            //double w = _width;
            if (h == 0 || w == 0) return; // It isn't exactly an exception, but rather just an indication not to do anything here.

            double controlAspect = w / h;
            double envelopeAspect = newEnv.Width / newEnv.Height;
            Coordinate center = newEnv.Center();


            if (controlAspect > envelopeAspect) // The Control is wider than it is tall
            {
                // If the envelope is proportionately wider than the control, "reveal" more width without changing height   
                // If the envelope is proportionately taller than the control, "hide" width without changing height
                newEnv.SetCenter(center, newEnv.Height * controlAspect, newEnv.Height);
            }
            else // The control is taller than it is wide
            {
                // If the envelope is proportionately wider than the control, "hide" the extra height without changing width   
                // If the envelope is proportionately taller than the control, "reveal" more height without changing width
                newEnv.SetCenter(center, newEnv.Width, newEnv.Width / controlAspect);

            }
            _resizing = false;

            return;
        }

        /// <summary>
        /// When the control is being resized, the view needs to change in order to preserve the aspect ratio,
        /// even though we want to use the exact same extents.
        /// </summary>
        public void ParentResize()
        {
            if (_resizing == false)
            {
                _originalView = _view;
                _resizing = true;
            }
            double w = Parent.ClientRectangle.Width;
            double h = Parent.ClientRectangle.Height;
            double vW = _originalView.Width;
            double vH = _originalView.Height;
            if (h == 0 || w == 0 || vW == 0 || vH == 0) return;
            double controlAspect = w / h;
            double viewAspect = vW / vH;
            if (controlAspect > viewAspect)
            {
                int dW = Convert.ToInt32(vH * controlAspect - vW);
                _view.X = _originalView.X - dW / 2;
                _view.Width = _originalView.Width + dW;
            }
            else
            {
                int dH = Convert.ToInt32(vW / controlAspect - vH);
                _view.Y = _originalView.Y - dH / 2;
                _view.Height = _originalView.Height + dH;
            }

        }


        /// <summary>
        /// Gets or sets the layers
        /// </summary>
        [Serialize("Layers")]
        public new IMapLayerCollection Layers
        {
            get { return _layers; }
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
        /// gets or sets the rectangle in pixel coordinates that will be drawn to the entire screen.
        /// </summary>
        public Rectangle View
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// The view extents describes the central, viewable portion of the frame, which is 1/3 the width and height of the whole frame
        /// </summary>
        public override IEnvelope ViewExtents
        {
            get
            {
                if (Extents != null)
                {
                    IEnvelope view = Extents.Copy();
                    if (_extendBuffer)
                    {
                        view.ExpandBy(-Extents.Width / 3, -Extents.Height / 3);
                    }
                    return view;
                }
                return new Envelope();
            }
        }

        /// <summary>
        /// Gets or sets the width in pixels for this map frame.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

       
        #endregion

        #region Protected Methods

       

        /// <summary>
        /// Draws from the buffer.
        /// </summary>
        /// <param name="pe"></param>
        public void Draw(PaintEventArgs pe)
        {

            
            if (_buffer == null) return;
             
            Rectangle clip = pe.ClipRectangle;
            if (clip.IsEmpty) clip = _parent.ClientRectangle;
            Rectangle clipView = ParentToView(clip);
            if (clip.Width == 0 || clip.Height == 0) return;
            if (clipView.Width == 0 || clipView.Height == 0) return;
            try
            {
                pe.Graphics.DrawImage(_buffer, clip, clipView, GraphicsUnit.Pixel);
            }
            catch
            {
                // There was an exception (probably because of sizing issues) so don't bother with the chunk timer.
                return;
            }
           
            //base.OnPaint(pe);
        }


       

        ///// <summary>
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

       

        /// <summary>
        /// Fires the ScreenUpdated event
        /// </summary>
        protected virtual void OnScreenUpdated()
        {
            if (ScreenUpdated == null) return;
            ScreenUpdated(this, new EventArgs());
        }

        /// <summary>
        /// Firest the FinsihedRefresh event
        /// </summary>
        protected virtual void OnFinishedRefresh()
        {
            if (FinishedRefresh != null)
            {
                FinishedRefresh(this, new EventArgs());
            }
        }

    
        /// <summary>
        /// Unlike PixelToProj, which works relative to the client control,
        /// BufferToProj takes a pixel coordinate on the buffer and 
        /// converts it to geographic coordinates.
        /// </summary>
        /// <param name="position">A System.Drawing.Point describing the pixel position on the back buffer</param>
        /// <returns>An ICoordinate describing the geographic position</returns>
        public Coordinate BufferToProj(System.Drawing.Point position)
        {
            IEnvelope view = Extents;
            if (base.Extents == null) return new Coordinate(0, 0);
            double x, y;
            x = Convert.ToDouble(position.X);
            y = Convert.ToDouble(position.Y);
            x = x * view.Width / _width + view.Minimum.X;
            y = view.Maximum.Y - y * view.Height / _height;

            return new Coordinate(x, y, 0.0);
        }

        /// <summary>
        /// This projects a rectangle relative to the buffer into and IEnvelope in geographic coordinates.
        /// </summary>
        /// <param name="rect">A System.Drawing.Rectangle</param>
        /// <returns>An IEnvelope interface</returns>
        public IEnvelope BufferToProj(Rectangle rect)
        {
            System.Drawing.Point tl = new System.Drawing.Point(rect.X, rect.Y);
            System.Drawing.Point br = new System.Drawing.Point(rect.Right, rect.Bottom);
            Coordinate topLeft = BufferToProj(tl);
            Coordinate bottomRight = BufferToProj(br);
            return new Envelope(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y);
        }

       



        ///// <summary>
        ///// Converts a single point location into an equivalent geographic coordinate
        ///// </summary>
        ///// <param name="position">The location relative to the mapframe cached bitmap</param>
        ///// <returns>The geogrpahic ICoordinate interface</returns>
        //public ICoordinate PixelToProj(System.Drawing.Point position)
        //{
        //    IEnvelope view = ViewExtents;
        //    if (base.Extents == null) return new Coordinate(0, 0);
        //    double x = Convert.ToDouble(position.X);
        //    double y = Convert.ToDouble(position.Y);
        //    x = x * view.Width / ((double)_view.Width) + view.Minimum.X;
        //    y = view.Maximum.Y - y * view.Height / ((double)_view.Height);
        //    return new Coordinate(x, y, 0.0);
        //}

        ///// <summary>
        ///// Converts a rectangle in pixel coordinates relative to the map control into
        ///// a geographic envelope.
        ///// </summary>
        ///// <param name="rect">The rectangle to convert</param>
        ///// <returns>An IEnvelope interface</returns>
        //public IEnvelope PixelToProj(Rectangle rect)
        //{
        //    System.Drawing.Point tl = new System.Drawing.Point(rect.X, rect.Y);
        //    System.Drawing.Point br = new System.Drawing.Point(rect.Right, rect.Bottom);
        //    ICoordinate topLeft = PixelToProj(tl);
        //    ICoordinate bottomRight = PixelToProj(br);
        //    return new Envelope(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y);
        //}


        /// <summary>
        /// Converts a single geographic location into the equivalent point on the 
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A System.Drawing.Point with the new location.</returns>
        public System.Drawing.Point ProjToBuffer(Coordinate location)
        {
            IEnvelope view = Extents;
            if (_width == 0 || _height == 0) return new System.Drawing.Point(0, 0);
            int x = Convert.ToInt32((location.X - view.Minimum.X) * (_width / view.Width));
            int y = Convert.ToInt32((view.Maximum.Y - location.Y) * (_height/ view.Height));
            return new System.Drawing.Point(x, y);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A System.Drawing.Rectangle</returns>
        public Rectangle ProjToBuffer(IEnvelope env)
        {
            Coordinate tl = new Coordinate(env.Minimum.X, env.Maximum.Y);
            Coordinate br = new Coordinate(env.Maximum.X, env.Minimum.Y);
            System.Drawing.Point topLeft = ProjToBuffer(tl);
            System.Drawing.Point bottomRight = ProjToBuffer(br);
            return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }



        ///// <summary>
        ///// Converts a single geographic location into the equivalent point on the 
        ///// screen relative to the top left corner of the map.
        ///// </summary>
        ///// <param name="location">The geographic position to transform</param>
        ///// <returns>A System.Drawing.Point with the new location.</returns>
        //public System.Drawing.Point ProjToPixel(ICoordinate location)
        //{
        //    IEnvelope view = ViewExtents;
        //    if (view.Width == 0 || view.Height == 0) return new System.Drawing.Point(0, 0);
        //    int X = Convert.ToInt32((location.X - view.Minimum.X) * ((double)_view.Width / view.Width));
        //    int Y = Convert.ToInt32((view.Maximum.Y - location.Y) * ((double)_view.Height / view.Height));
        //    return new System.Drawing.Point(X, Y);
        //}

        ///// <summary>
        ///// Converts a single geographic envelope into an equivalent Rectangle
        ///// as it would be drawn on the screen.
        ///// </summary>
        ///// <param name="env">The geographic IEnvelope</param>
        ///// <returns>A System.Drawing.Rectangle</returns>
        //public Rectangle ProjToPixel(IEnvelope env)
        //{
        //    ICoordinate tl = new Coordinate(env.Minimum.X, env.Maximum.Y);
        //    ICoordinate br = new Coordinate(env.Maximum.X, env.Minimum.Y);
        //    System.Drawing.Point topLeft = ProjToPixel(tl);
        //    System.Drawing.Point bottomRight = ProjToPixel(br);
        //    return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        //}

        /// <summary>
        /// This will cause an invalidation for each layer.  The actual rectangle to re-draw is not specified
        /// here, but rather this simply indicates that some re-calculation is necessary.
        /// </summary>
        public void InvalidateLayers()
        {
            if (Layers == null) return;
            foreach (ILayer layer in base.Layers)
            {
                layer.Invalidate();
            }
        }
       
        

        /// <summary>
        /// Gets or sets the buffered image.  Mess with this at your own risk.
        /// </summary>
        public Image BufferImage
        {
            get { return _buffer; }
            set { _buffer = value; }

        }

        /// <summary>
        /// Gets or sets the client rectangle
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return new Rectangle(0, 0, _width, _height); }
        }

        /// <summary>
        /// Gets or sets the ImageRectangle for drawing the buffer
        /// </summary>
        public Rectangle ImageRectangle
        {
            get
            {
                if(_extendBuffer)return new Rectangle(_width/3, _height/3, _width/3, _height/3);
                return new Rectangle(0, 0, _width, _height);
            }
        }
        
       

        ///// <summary> 
        /////  ______________________
        ///// |      |  North |      |
        ///// |      |--------|      |
        ///// | West | Center | East |
        ///// |      |--------|      |
        ///// |      |  South |      |
        /////  ----------------------
        ///// </summary>
        ///// <param name="zone"></param>
        ///// <returns></returns>
        //public Rectangle GetZoneBounds(Zones zone)
        //{
        //    switch (zone)
        //    {
        //        case Zones.Center: _centerZone = _view; return _view;
        //        case Zones.North: return new Rectangle(_centerZone.X, 0, _centerZone.Width, _centerZone.Y);
        //        case Zones.South: return new Rectangle(_centerZone.X, _centerZone.Bottom, _centerZone.Width, ClientRectangle.Height - _centerZone.Bottom);
        //        case Zones.East: return new Rectangle(_centerZone.Right, 0, Right - _centerZone.Right, ClientRectangle.Height);
        //        case Zones.West: return new Rectangle(0, 0, _centerZone.X, ClientRectangle.Height);
        //        default: return _view;
               
        //    }
        //}

        ///// <summary>
        ///// Advances the zone to the next zone.
        ///// </summary>
        ///// <returns></returns>
        //Zones NextZone()
        //{
        //    return (Zones)((((int)_zone) + 1) % 5);
        //}


        
        /// <summary>
        /// Gets or sets the parent control for this map frame.
        /// </summary>
        public new Control Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                CreateBuffer();
            }
        }

        /// <summary>
        /// The right in client rectangle coordinates
        /// </summary>
        public int Right
        {
            get { return ClientRectangle.Right; }
        }

        /// <summary>
        /// The bottom (or height) of this client rectangle
        /// </summary>
        public int Bottom
        {
            get { return ClientRectangle.Bottom; }
        }

       
        /// <summary>
        /// Wires each of the layer events that the MapFrame should be listening to.
        /// </summary>
        /// <param name="collection"></param>
        protected override void Handle_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            IMapLayerCollection glc = collection as IMapLayerCollection;
            if (glc == null) return;
            glc.BufferChanged += GeoLayerBufferChanged;
            glc.LayerAdded += GlcLayerAdded;
            glc.LayerVisibleChanged += GlcLayerVisibleChanged;
            glc.ItemChanged += GlcLayerMembersChanged;
            base.Handle_Layer_Events(collection);
        }

        /// <summary>
        /// Modifies the ZoomToLayer behavior to account for the possiblity of an expanded
        /// MapFrame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Layers_ZoomToLayer(object sender, EnvelopeArgs e)
        {

            if (_extendBuffer)
            {
                e.Envelope.ExpandBy(e.Envelope.Width, e.Envelope.Height);
            }
            Extents = e.Envelope;

        }

        void GlcLayerVisibleChanged(object sender, EventArgs e)
        {
            Initialize();
            OnUpdateMap();
            //OnBufferChanged(this, new ClipArgs(ClientRectangle));
        }

        void GlcLayerMembersChanged(object sender, EventArgs e)
        {
            Initialize();
            //OnBufferChanged(this, new ClipArgs(ClientRectangle));
            OnUpdateMap();
        }

        void GlcLayerAdded(object sender, Main.LayerEventArgs e)
        {
            IMapLayer gl = e.Layer as IMapLayer;
            if (gl == null) return;
            if (!ExtentsInitialized || Extents == null || Extents.IsNull || Extents.Width == 0 || Extents.Height == 0)
            {
                ExtentsInitialized = true;
                if (_extendBuffer)
                {
                    if(e.Layer.Envelope != null)
                    {
                        IEnvelope env = e.Layer.Envelope.Copy();
                        env.ExpandBy(env.Width, env.Height);
                        Extents = env;
                    }
                }
                else
                {
                    if(e.Layer.Envelope != null && e.Layer.Envelope.IsNull == false)
                    {
                        if(e.Layer.Envelope.Width > 0 && e.Layer.Envelope.Width < 1E300)
                        {
                            if(e.Layer.Envelope.Height > 0 && e.Layer.Envelope.Height < 1E300)
                            {
                                Extents = e.Layer.Envelope; 
                            }
                        }
                    } 
                }
                if(gl.DataSet != null)
                {
                    Projection = gl.DataSet.Projection; 
                }
                
            }
            else
            {
                if(Projection == null && gl.DataSet != null && gl.DataSet.Projection != null)
                {
                    Projection = gl.DataSet.Projection;
                }
                if(Projection != null)
                {
                    if(gl.DataSet != null && gl.DataSet.Projection == null)
                    {
                        gl.DataSet.Projection = Projection;
                    }
                    else
                    {
                        IFeatureSet fs = gl.DataSet as IFeatureSet;
                        if (fs != null)
                        {
                            if(gl.DataSet != null)
                            {
                                if (!Projection.Equals(gl.DataSet.Projection))
                                {
                                    if (MessageBox.Show("The new layer has a projection that does not match the current map projection.  Do you wish to re-project the in memory coordinates to match the current projection?  This will not affect the source file", "Projection Mismatch", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        fs.Reproject(Projection);
                                    }
                                }
                                
                            }
                           
                        } 
                    }
                    
                    
                }
                Initialize();
            }
            
        }

        /// <summary>
        /// Unwires events from the layer collection
        /// </summary>
        /// <param name="collection"></param>
        protected override void Ignore_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            IMapLayerCollection glc = collection as IMapLayerCollection;
            if (glc == null) return;
            glc.BufferChanged -= GeoLayerBufferChanged;
            glc.LayerAdded -= GlcLayerAdded;
            glc.LayerVisibleChanged -= GlcLayerVisibleChanged;
            base.Ignore_Layer_Events(collection);
        }

        /// <summary>
        /// When any region for the stencil of any layers is changed, we should update the
        /// image that we have in that region.  This activity will be suspended in the 
        /// case of a large scale update for all the layers until they have all updated.
        /// </summary>
        void GeoLayerBufferChanged(object sender, ClipArgs e)
        {
            OnBufferChanged(this, e);
        }

        /// <summary>
        /// Fires the BufferChanged event.  This is fired even if the new content is not currently
        /// in the view rectangle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBufferChanged(object sender, ClipArgs e)
        {
          
            if (BufferChanged != null) BufferChanged(this, e);
        }

      

        #endregion



        #region IMapGroup Members


        public IMapFrame ParentMapFrame
        {
            get { return this; }
        }

        #endregion

        #region IMapLayer Members

        public void DrawRegions(MapArgs args, List<IEnvelope> regions)
        {
          
        }

        #endregion
    }
}
