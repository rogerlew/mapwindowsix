//********************************************************************************************************
// Product Name: MapWindow.Interfaces Alpha
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
// The Initial Developer of this Original Code is Ted Dunsford. Created in January 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Geometries;
namespace MapWindow.Main
{

    /// <summary>
    /// Carries event with float arguments for coordinate values
    /// </summary>
    public class MouseCoordinateF : System.EventArgs
    {
        private float _X;
        private float _Y;
        private float _Z;
        private System.Windows.Forms.MouseButtons _buttons;
        private int _clicks;
        private int _Delta;

       
        /// <summary>
        /// Creates a new 2D coordinate event using a float coordinates, but 0 clicks, 0 delta and MouseButtons.None
        /// </summary>
        /// <param name="x">The float x coordinate at z = 0</param>
        /// <param name="y">The float y coordinate at z = 0</param>
        public MouseCoordinateF(float x, float y)
            : this(x, y, 0f, 0, System.Windows.Forms.MouseButtons.None, 0)
        {
            // Forwards the constructor with a default value of 0 for z
        }

        /// <summary>
        /// Creates a new 3D coordinate event using a float coordinates, but 0 clicks, 0 delta and MouseButtons.None
        /// </summary>
        /// <param name="x">The float x coordinate</param>
        /// <param name="y">The float y coordinate</param>
        /// <param name="z">The float z coordinate</param>
        public MouseCoordinateF(float x, float y, float z)
            : this(x, y, z, 0, System.Windows.Forms.MouseButtons.None, 0)
        {
            _X = x;
            _Y = y;
            _Z = z;

        }

        /// <summary>
        /// A comprehensive mouse event, specifying the usual mouse event argument parameters, except using float coordinates
        /// </summary>
        /// <param name="x">The float x coordinate</param>
        /// <param name="y">The float y coordinate</param>
        /// <param name="z">The float z coordinate</param>
        /// <param name="buttons">The button conditions for the mouse event</param>
        /// <param name="clicks">The integer number of clicks</param>
        /// <param name="delta">The delta for mouse scrolling</param>
        public MouseCoordinateF(float x, float y, float z, int clicks, System.Windows.Forms.MouseButtons buttons, int delta)
        {
            _X = x;
            _Y = y;
            _Z = z;
            _buttons = buttons;
            _Delta = delta;
            _clicks = clicks;
        }

        /// <summary>
        /// Gets the floating point X coordinate from this event
        /// </summary>
        public float X
        {
            get { return _X; }
        }

        /// <summary>
        /// Gets the floating point Y coordinate from this event
        /// </summary>
        public float Y
        {
            get { return _Y; }
        }

        /// <summary>
        /// Gets the floating point Z coordinate from this event
        /// </summary>
        public float Z
        {
            get { return _Z; }
        }

        /// <summary>
        /// Gets a System.Windows.Forms.MouseButtons enumeration specifying which button was pressed
        /// </summary>
        public System.Windows.Forms.MouseButtons Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Gets an integer number of clicks
        /// </summary>
        public int Clicks
        {
            get { return _clicks; }
        }

        /// <summary>
        /// Gets the integer delta for the mouse scrolling.
        /// </summary>
        public int Delta
        {
            get { return _Delta; }
        }


    }
}
