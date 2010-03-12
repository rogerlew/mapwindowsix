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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/21/2009 2:17:16 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Drawing;
using System.Collections.Generic;
using MapWindow.Geometries;
using MapWindow.Drawing;
namespace MapWindow.Map
{


    /// <summary>
    /// GeoArgs
    /// </summary>
    public class MapArgs : EventArgs, IProj
    {
        #region Private Variables

        private Rectangle _bufferRectangle;
        private IEnvelope _bufferEnvelope;
        private Graphics _graphics;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoArgs
        /// </summary>
        public MapArgs(Rectangle bufferRectangle, IEnvelope bufferEnvelope)
        {
            _bufferRectangle = bufferRectangle;
            _bufferEnvelope = bufferEnvelope;
        }

        /// <summary>
        /// Creates a new MapArgs, where the device is also specified, overriding the default buffering behavior.
        /// </summary>
        /// <param name="bufferRectangle"></param>
        /// <param name="bufferEnvelope"></param>
        /// <param name="g"></param>
        public MapArgs(Rectangle bufferRectangle, IEnvelope bufferEnvelope, Graphics g)
        {
            _bufferRectangle = bufferRectangle;
            _bufferEnvelope = bufferEnvelope;
            _graphics = g;
        }

        #endregion

       


        #region Properties

        /// <summary>
        /// Gets the rectangle dimensions of what the buffer should be in pixels
        /// </summary>
        public Rectangle ImageRectangle
        {
            get { return _bufferRectangle; }
        }

        /// <summary>
        /// Gets the geographic bounds of the content of the buffer.
        /// </summary>
        public IEnvelope GeographicExtents
        {
            get { return _bufferEnvelope; }
        }

        /// <summary>
        /// An optional parameter that specifies a device to use instead of the normal buffers.
        /// </summary>
        public Graphics Device
        {
            get { return _graphics; }
        }

        /// <summary>
        /// Gets the dX
        /// </summary>
        public double Dx
        {
            get {return (double)_bufferRectangle.Width / _bufferEnvelope.Width; }
        }

        /// <summary>
        /// Gets the double valued
        /// </summary>
        public double Dy
        {
            get { return (double)_bufferRectangle.Height / _bufferEnvelope.Height; }
        }

        /// <summary>
        /// Gets the minimum X value
        /// </summary>
        public double MinX
        {
            get { return _bufferEnvelope.Minimum.X; }
        }

        /// <summary>
        /// Gets the maximum Y value
        /// </summary>
        public double MaxY
        {
            get { return _bufferEnvelope.Maximum.Y; }
        }
 
     

        #endregion



    }
}
