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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2008 1:25:22 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using MapWindow.Drawing;
using MapWindow.Geometries;
namespace MapWindow.GeoMap
{


    /// <summary>
    /// GeoRenderBase is a base for anything that can be drawn to the map, not necessarilly just a layer, 
    /// though layers represent one instance of a render base.
    /// </summary>
    public class GeoRenderBase : Component
    {
        #region Events

        /// <summary>
        /// Occurs immediately before the drawing event.  Setting the cancel parameter to false will
        /// force the drawing to exit without actually rendering the contents.
        /// </summary>
        public event EventHandler<GeoDrawVerifyArgs> BeforeDrawing;

        /// <summary>
        /// Occurs after the drawing is complete
        /// </summary>
        public event EventHandler<MapDrawArgs> DrawingCompleted;

        /// <summary>
        /// Occurs whenever the geographic bounds for this renderable object have changed
        /// </summary>
        public event EventHandler<EnvelopeArgs> EnvelopeChanged;

        /// <summary>
        /// Occurs when the data has been loaded into the model after the first drawing method
        /// </summary>
        public event EventHandler<MapDrawArgs> Inititialized;

        /// <summary>
        /// Occurs when an outside request is sent to invalidate this object
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Occurs immediately after the visible parameter has been adjusted.
        /// </summary>
        public event EventHandler VisibleChanged;

       

        #endregion

        #region Private Variables

        private IEnvelope _envelope;
        private bool _isInitialized;
        private bool _isVisible;
        private int _numStages = 1;
        private int _currentStage = 0;
        private int _numChuncks = 1;
        private List<Image> _stencils; // stencils are like a cache of the images.
        private TimeSpan _geometryTime;
        private TimeSpan _conversionTime;
        private TimeSpan _drawingTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoRenderBase
        /// </summary>
        public GeoRenderBase()
        {
            _isVisible = true;
            _envelope = new Envelope();
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method does not purge the existing stencils, but rather flushes content by overwriting
        /// existing content with the transparent color, but only for the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to clear</param>
        public virtual void Clear(Rectangle rect)
        {
            foreach (Image stencil in _stencils)
            {
                Graphics g = Graphics.FromImage(stencil);
                g.FillRectangle(Brushes.Transparent, rect);
                g.Dispose();
            }
        }

        /// <summary>
        /// This can be overridden, but should not be.  Instead override OnDraw so that
        /// the standard checks on visibility can be performed.
        /// </summary>
        /// <param name="args">A DrawingArgs class.  This is used so that this method
        /// can also be called asynchronously, or be easilly updated later.</param>
        public virtual void Draw2D(MapDrawArgs args)
        {
            try
            {
               
                if (_isVisible == false)
                {
                    OnDrawingCompleted(new GeoDrawCompletedArgs(args, false));
                    return;
                }
                if (OnBeforeDrawing(new GeoDrawVerifyArgs(args)) == true)
                {
                    OnDrawingCompleted(new GeoDrawCompletedArgs(args, true));
                    return;
                }

                for (int stage = 0; stage < _numStages; stage++)
                {
                    args.GeoGraphics.Stage = stage;
                    if (IsInitialized == false)
                    {
                        Initialize(args); // keep building if we haven't finished drawing to the stencil
                    }
                    OnDraw(args); // Draw whatever we have on the stencil
                }
                args.GeoGraphics.Stage = 0;
                OnDrawingCompleted(new GeoDrawCompletedArgs(args, false));

            }
            catch (Exception ex)
            {
                OnDrawingCompleted(new GeoDrawCompletedArgs(args, ex));
            }


        }

        /// <summary>
        /// Instructs the stencil to copy the current information based on the dX and dY values
        /// that are passed in.
        /// </summary>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        public virtual void Pan(int dX, int dY)
        {

            for(int I = 0; I < _stencils.Count; I++)
            {
                Image stencil = _stencils[I];
                Bitmap bmp = new Bitmap(stencil.Width, stencil.Height);

                Rectangle dest = new Rectangle(0, 0, bmp.Width, bmp.Height);
                dest.X += dX;
                if (dest.X < 0) dest.X = 0;
                dest.Y += dY;
                if (dest.Y < 0) dest.Y = 0;
                dest.Width = dest.Width - Math.Abs(dX);
                dest.Height = dest.Height - Math.Abs(dY);

                Rectangle source = new Rectangle(0, 0, bmp.Width, bmp.Height);
                source.X -= dX;
                if (source.X < 0) source.X = 0;
                source.Y -= dY;
                if (source.Y < 0) source.Y = 0;
                source.Width = source.Width - Math.Abs(dX);
                source.Height = source.Height - Math.Abs(dY);

               
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(stencil, dest, source, GraphicsUnit.Pixel);
                g.Dispose();
                stencil.Dispose();
                _stencils[I] = bmp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan GeometryTime
        {
            get { return _geometryTime; }
            protected set { _geometryTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan DrawingTime
        {
            get { return _drawingTime; }
            protected set { _drawingTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ConversionTime
        {
            get { return _conversionTime; }
            protected set { _conversionTime = value; }
        }

        /// <summary>
        /// This handles the initialization where structures are created
        /// </summary>
        public virtual void Initialize(MapDrawArgs args)
        {
            
            Graphics stencilDevice;
            if (_stencils == null) _stencils = new List<Image>();
            if (_stencils.Count > 0)
            {
                if (_stencils[0].Width != args.GeoGraphics.ClientRectangle.Width ||
                    _stencils[0].Height != args.GeoGraphics.ClientRectangle.Height)
                {
                    _stencils = new List<Image>();
                }
            }
            if (_stencils.Count == args.GeoGraphics.Stage)
            {
                Image bmp = new Bitmap(args.GeoGraphics.ClientRectangle.Width, args.GeoGraphics.ClientRectangle.Height);
                _stencils.Add(bmp);
            }
            
            stencilDevice = Graphics.FromImage(_stencils[args.GeoGraphics.Stage]);
            if (args.GeoGraphics.Chunk == 0)
            {
                stencilDevice.FillRectangle(Brushes.White, args.ClipRectangle);
            }

            // Any customize initialization that is layer specific will be drawn on the stencil.

            MapDrawArgs newArgs = new MapDrawArgs(stencilDevice, args.ClipRectangle, args.GeoGraphics);
            OnInitialize(newArgs);
            //_stencils[args.GeoGraphics.Stage].Save("C:\\stencil" + args.GeoGraphics.Stage.ToString() + ".bmp");
            stencilDevice.Dispose();

        }

        /// <summary>
        /// Invalidates the drawing methods
        /// </summary>
        public virtual void Invalidate()
        {
            _isInitialized = false;
            OnInvalidate(new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Designer variable
        /// </summary>
        protected System.ComponentModel.IContainer components = null; // the members to be contained

        /// <summary>
        /// Gets the current stage
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int CurrentStage
        {
            get { return _currentStage; }
            protected set { _currentStage = value; }
        }

       

        /// <summary>
        /// Obtains an IEnvelope in world coordinates that contains this object
        /// </summary>
        /// <returns></returns>
        [Category("General"), Description("Obtains an IEnvelope that contains this object")]
        public virtual IEnvelope Envelope
        {
            get { return _envelope; }
            protected set
            {
                if (_envelope == null && value == null) return;
                if (_envelope.Equals(value) == false)
                {
                    OnEnvelopeChanged(new EnvelopeArgs(_envelope));
                }
                _envelope = value;

            }
        }

        /// <summary>
        /// Gets whether or not the unmanaged drawing structures have been created for this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsInitialized
        {
            get { return _isInitialized; }
            set
            {
                _isInitialized = value;
            }
        }

        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        [Category("General"), Description("Gets or sets whether or not this object will be drawn or painted.")]
        public virtual bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnVisibleChanged(new EventArgs());
                }
            }
        }

       

        /// <summary>
        /// Gets the number of stage for this object.  If each layer were considered to be drawn on a transparent stensil,
        /// then a layer with multiple stages can be thought of as having several separate stensils.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int NumStages
        {
            get { return _numStages; }
            protected set { _numStages = value; }
        }

        /// <summary>
        /// Gets the number of chunks.
        /// </summary>
        public virtual int NumChunks
        {
            get { return _numChuncks; }
            protected set { _numChuncks = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary> 
        /// Clean up any resources being used.
        /// Part of the Component Model for a Component.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // The Subtypes of layers will simply override this one since they have to control the special instance of unmanaged variables
            if (disposing && (components != null))
            {
                components.Dispose();
            }
        }

        /// <summary>
        /// Fires the event that occurs before drawing to a directX map
        /// </summary>
        /// <param name="e">The BeforeDrawingArgs parameter with information about the drawing</param>
        /// <returns>Boolean, that if true should cancel the draw event.</returns>
        protected virtual bool OnBeforeDrawing(GeoDrawVerifyArgs e)
        {
            // Regardless of what it is we are drawing, we can support the identical short-circuit code here.
            if (e.GeoGraphics.Envelope.Intersects(this.Envelope) == false)
            {
                e.Handled = true;
                return true;
            }

            if (BeforeDrawing == null) return false;
            BeforeDrawing(this, e);
            return e.Handled;
        }

        
        /// <summary>
        /// Fires the EnvelopeChanged event.
        /// </summary>
        /// <param name="e">The EnvelopeArgs specifying the envelope</param>
        protected virtual void OnEnvelopeChanged(EnvelopeArgs e)
        {
            if (EnvelopeChanged != null)
            {
                EnvelopeChanged(this, e);
            }
        }
  
  
      
        /// <summary>
        /// This will be called once for each part.  If there is only one part, then this will still be called for 
        /// the first part.  The OnDraw method is called once, regardless of the number of parts.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDraw(MapDrawArgs e)
        {
            e.Graphics.DrawImage(_stencils[e.GeoGraphics.Stage], e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);   
        }

    

        /// <summary>
        /// Fires an event when we are done with the drawing code, whether the drawing was successful, cancelled, or threw an exception.
        /// </summary>
        /// <param name="e">A DrawingCompletedArgs parameter containing information about the drawing</param>
        protected virtual void OnDrawingCompleted(GeoDrawCompletedArgs e)
        {
            if (DrawingCompleted == null) return;
            DrawingCompleted(this, e);
        }

        /// <summary>
        /// Fires the Initialized event
        /// </summary>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnInitialize(MapDrawArgs e)
        {
            if (Inititialized != null)
            {
                Inititialized(this, e);
            }
           
        }

        /// <summary>
        /// Fires the Invalidated event
        /// </summary>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnInvalidate(EventArgs e)
        {
            if (Invalidated != null)
            {
                Invalidated(this, e);
            }
        }

        /// <summary>
        /// Fires the Visible Changed event
        /// </summary>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            if (VisibleChanged != null) VisibleChanged(this, e);
        }

        #endregion

    }
}
