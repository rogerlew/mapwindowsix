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
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main
{
    /// <summary>
    /// The easiest way to implement this interface is to inherit System.ContainerControl
    /// </summary>
    public interface IContainerControlOld : IScrollableControl
    {
        #region Methods

        /// <summary>
        /// Performs scaling of the container control and its children.
        /// </summary>
        void PerformAutoScale();

        /// <summary>
        /// Verifies the value of the control that is losing focus; conditionally dependent on whether automatic validation is turned on.
        /// </summary>
        /// <param name="checkAutoValidate">If true, the value of the</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        bool Validate(bool checkAutoValidate);

        /// <summary>
        /// Verifies the value of the control losing focus by causing the System.Windows.Forms.Control.Validating and System.Windows.Forms.Control.Validated events to occur, in that order.
        /// </summary>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        bool Validate();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the active control on the container control.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.Control that is currently active on the System.Windows.Forms.ContainerControl.
        /// </returns>
        System.Windows.Forms.Control ActiveControl { set; get; }


        /// <summary>
        /// Gets or sets the dimensions that the control was designed to.
        /// </summary>
        /// <returns>
        /// A System.Drawing.SizeF containing the dots per inch (DPI) or System.Drawing.Font size that the control was designed to.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The width or height of the System.Drawing.SizeF value is less than 0 when setting this value.</exception>
        System.Drawing.SizeF AutoScaleDimensions { set; get; }

        /// <summary>
        /// Gets or sets the automatic scaling mode of the control.
        /// </summary>
        /// <returns>
        /// An System.Windows.Forms.AutoScaleMode that represents the current scaling mode. The default is System.Windows.Forms.AutoScaleMode.None.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">An System.Windows.Forms.AutoScaleMode value that is not valid was used to set this property.</exception>
        System.Windows.Forms.AutoScaleMode AutoScaleMode { set; get; }

        /// <summary>
        /// Gets the current run-time dimensions of the screen.
        /// </summary>
        /// <returns>
        /// A System.Drawing.SizeF containing the current dots per inch (DPI) or System.Drawing.Font size of the screen.
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">A Win32 device context could not be created for the current screen.</exception>
        System.Drawing.SizeF CurrentAutoScaleDimensions { get; }

        /// <summary>
        /// Gets the form that the container control is assigned to.
        /// </summary>
        /// <returns>
        /// The System.Windows.Forms.Form that the container control is assigned to.
        /// </returns>
        System.Windows.Forms.Form ParentForm { get; }

        #endregion

    }
}
