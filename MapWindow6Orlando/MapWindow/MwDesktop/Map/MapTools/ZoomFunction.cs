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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/11/2008 3:54:51 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
namespace MapWindow.Map
{


    /// <summary>
    /// PanTool
    /// </summary>
    public class ZoomFunction : MapFunction
    {
        #region Private Variables

        private Timer _zoomTimer;
        private IMapFrame _mapFrame;
        private Rectangle _client;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PanTool
        /// </summary>
        public ZoomFunction(IMap inMap)
            : base(inMap)
        {
            Configure();
        }

        private void Configure()
        {
            _zoomTimer = new Timer();
            _zoomTimer.Interval = 100;
            _zoomTimer.Tick += ZoomTimerTick;
            _client = Rectangle.Empty;
        }

        void ZoomTimerTick(object sender, EventArgs e)
        {
            _zoomTimer.Stop();
            if (_mapFrame == null) return;
            _client = Rectangle.Empty;
            _mapFrame.ResetExtents();
            
        }

        #endregion

      
    
        #region Protected Methods

        
        /// <summary>
        /// Mouse Wheel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(GeoMouseArgs e)
        {
 
            _zoomTimer.Stop(); // if the timer was already started, stop it.
            Rectangle r = e.Map.MapFrame.View;

            // For multiple zoom steps before redrawing, we actually
            // want the x coordinate relative to the screen, not
            // the x coordinate relative to the previously modified view.
            if (_client == Rectangle.Empty) _client = r;
            int cw = _client.Width;
            int ch = _client.Height;

            int w = r.Width;
            int h = r.Height;
            if (e.Delta > 0)
            {
                
                r.Inflate(-w / 4, -h / 4);
                // try to keep the mouse cursor in the same geographic position
                r.X += (e.X * w/(2*cw)) - w/4; 
                r.Y += (e.Y * w/(2*cw)) - h/4;
            }
            else
            {
                r.Inflate(w / 2, h / 2);
                r.X += w/2 - (e.X * w / cw);
                r.Y += h/2 - (e.Y * w / cw);

            }

            e.Map.MapFrame.View = r;
            e.Map.Invalidate();
            _zoomTimer.Start();
            _mapFrame = e.Map.MapFrame;
            base.OnMouseWheel(e);
        }


        #endregion

    }
}
