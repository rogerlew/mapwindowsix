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
using System.Windows.Forms;

namespace MapWindow.Main
{
    /// <summary>
    /// Provides an empty control that can be used to create other controls.
    /// </summary>
    public interface IUserControl : IContainerControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets how the control will resize itself.
        /// </summary>
        /// <returns>
        /// A value from the System.Windows.Forms.AutoSizeMode enumeration. The default is System.Windows.Forms.AutoSizeMode.GrowOnly.
        /// </returns>
        System.Windows.Forms.AutoSizeMode AutoSizeMode { set; get; }

        /// <summary>
        /// Gets or sets the border style of the tree view control.
        /// </summary>
        /// <returns>
        /// One of the System.Windows.Forms.BorderStyle values. The default is System.Windows.Forms.BorderStyle.Fixed3D.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// System.ComponentModel.InvalidEnumArgumentException: The assigned value is not one of the System.Windows.Forms.BorderStyle values.
        /// </exception>
        System.Windows.Forms.BorderStyle BorderStyle { set; get; }

        #endregion

        /// <summary>
        /// Occurs when the AutoSize changes
        /// </summary>
        event System.EventHandler AutoSizeChanged;

        /// <summary>
        /// Occurs when the System.Windows.Forms.UserControl.AutoValidate property changes.
        /// </summary>
        event System.EventHandler AutoValidateChanged;

        /// <summary>
        /// Occurs before the control becomes visible for the first time.
        /// </summary>
        event System.EventHandler Load;

    }
}
