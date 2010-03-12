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


using System.Drawing;
using System.Windows.Forms;
namespace MapWindow.Map
{


    /// <summary>
    /// PanTool
    /// </summary>
    public class PanFunction: MapFunction
    {
        #region Private Variables

        private bool _isDragging;
        private bool _preventDrag;
        private Point _dragStart;
        private Rectangle _source;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PanTool
        /// </summary>
        public PanFunction(IMap inMap)
            : base(inMap)
        {
           
        }

     
     

        #endregion

        #region Methods



        #endregion

        #region Properties

        /// <summary>
        /// This indicates that this tool is currently being used.
        /// </summary>
        public bool IsDragging
        {
            get { return _isDragging; }
        }
        
        #endregion

        #region Protected Methods


        /// <summary>
        /// Handles the actions that the tool controls during the OnMouseDown event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
           
            if (e.Button == MouseButtons.Left && _preventDrag == false)
            {
                //PreventBackBuffer = true;
                _isDragging = true;
                _dragStart = e.Location;
                _source = e.Map.MapFrame.View;
                Map.CollectAfterDraw = false;

            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event, changing the viewing extents to match the movements
        /// of the mouse if the lef tmouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {

            if (_isDragging)
            {
                Point diff = new Point();
                diff.X =  _dragStart.X - e.X;
                diff.Y =  _dragStart.Y - e.Y;
                e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width, _source.Height); 
                Map.Invalidate();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Mouse Up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left && _isDragging)
            {
                _isDragging = false;
                //PreventBackBuffer = false;
                Point diff = new Point();
                diff.X = _dragStart.X - e.X;
                diff.Y = _dragStart.Y - e.Y;
                e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width, _source.Height);
                Map.Invalidate();
                Application.DoEvents();
                _preventDrag = true;
                e.Map.CollectAfterDraw = true;
                //e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width, _source.Height); 
                e.Map.MapFrame.Pan(new Point(diff.X, diff.Y));
                _preventDrag = false;
                //e.Map.MapFrame.ResetExtents();
            }

            Map.Invalidate();
            base.OnMouseUp(e);
        }


        
      


        #endregion

    }
}
