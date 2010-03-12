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
using System.ComponentModel;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{


    /// <summary>
    /// RenderBase is a base for anything that can be drawn to the map, not necessarilly just a layer, 
    /// though layers represent one instance of a render base.
    /// </summary>
    [ToolboxItem(false)]
    public class RenderBase : Component, IRenderable
    {
        #region Events

       
      

        /// <summary>
        /// Occurs whenever the geographic bounds for this renderable object have changed
        /// </summary>
        public event EventHandler<EnvelopeArgs> EnvelopeChanged;

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
        

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RenderBase
        /// </summary>
        public RenderBase()
        {
            _isVisible = true;
            _envelope = new Envelope();
        }

        #endregion

        #region Methods

      
        /// <summary>
        /// Invalidates the drawing methods
        /// </summary>
        public virtual void Invalidate()
        {
            _isInitialized = false;
            OnInvalidate(this, new EventArgs());
        }

        #endregion



        #region Properties

        /// <summary>
        /// Designer variable
        /// </summary>
        protected readonly IContainer components; // the members to be contained

       

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
                if (_envelope != null)
                {
                    if (_envelope.Equals(value) == false)
                    {
                        OnEnvelopeChanged(this, new EnvelopeArgs(_envelope));
                    }
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
            protected set
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
                    OnVisibleChanged(this, new EventArgs());
                }
            }
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
        /// Fires the EnvelopeChanged event.
        /// </summary>
        /// <param name="sender">The object sender for this event (this)</param>
        /// <param name="e">The EnvelopeArgs specifying the envelope</param>
        protected virtual void OnEnvelopeChanged(object sender, EnvelopeArgs e)
        {
            if (EnvelopeChanged != null)
            {
                EnvelopeChanged(sender, e);
            }
        }

        /// <summary>
        /// Fires the Invalidated event
        /// </summary>
        /// <param name="sender">The object sender (usually this)</param>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnInvalidate(object sender, EventArgs e)
        {
            if (Invalidated != null)
            {
                Invalidated(sender, e);
            }
        }

        /// <summary>
        /// Fires the Visible Changed event
        /// </summary>
        /// <param name="sender">The object sender (usually this)</param>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnVisibleChanged(object sender, EventArgs e)
        {
            if (VisibleChanged != null) VisibleChanged(sender, e);
        }

        #endregion

    }
}
