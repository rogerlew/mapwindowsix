﻿//********************************************************************************************************
// Product Name: SketchPad.Components.exe 
// Description:  An open source drawing pad that is super simple, but extendable
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from SketchPad.Components.exe
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/8/2008 1:01:26 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace MapWindow.Components
{


    /// <summary>
    /// This parses the input values and changes the background color to salmon
    /// if the value won't work as a degree.
    /// </summary>
    internal class AngleTextBox: ValidTextBox
    {
        #region Events

        /// <summary>
        /// Occurs continuously as someone drags the angle control around
        /// </summary>
        public event EventHandler AngleChanged;

        #endregion

        #region Private Variables

       
        private int _angle;
        private int _minAngle;
        private int _maxAngle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AngleTextBox
        /// </summary>
        public AngleTextBox()
        {
            _minAngle = -360;
            _maxAngle = 360;
        }

    

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer angle for this textbox.
        /// </summary>
        public int Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the minimum valid angle.  An angle below the minimum will
        /// register an error and show the error color.
        /// </summary>
        public int MinAngle
        {
            get
            {
                return _minAngle;
            }
            set
            {
                _minAngle = value;
                NormalToolTipText = "Enter an integer degree between " + _minAngle.ToString() + " and " + _maxAngle.ToString() + ".";
                
            }
        }

        /// <summary>
        /// Gets or sets the maximum valid angle.  An angle above the maximum will
        /// register an error and show the error color.
        /// </summary>
        public int MaxAngle
        {
            get
            {
                return _maxAngle;
            }
            set
            {
                _maxAngle = value;
                NormalToolTipText = "Enter an integer degree between " + _minAngle.ToString() + " and " + _maxAngle.ToString() + ".";
            }
        }

        #endregion

        /// <summary>
        /// Fires the TextChanged method and also determines whether or not the text is a valid integer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (int.TryParse(Text, out _angle) == false)
            {
                ErrorMessage = "The text entered into the " + MessageName + " could not be parsed into an integer.";
               
            }
            else
            {
                if (_angle > 360 || _angle < -360) _angle = _angle % 360;
                if (_angle < _minAngle || _angle > _maxAngle)
                {
                    ErrorMessage = "The value entered into the " + MessageName + " was outside the valid range from " + _minAngle.ToString() + " to " + _maxAngle.ToString();
                  
                }
                else
                {
                    ClearError();
                }
                
                OnAngleChanged(new EventArgs());
            }
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Fires the AngleChanged event
        /// </summary>
        /// <param name="e"></param>
        protected void OnAngleChanged(EventArgs e)
        {
            if (AngleChanged != null) AngleChanged(this, e);
        }


    }
}
