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
    /// Defines a base class for controls that support auto-scrolling behavior.
    /// </summary>
    public interface IScrollableControl : IControl
    {
        #region Methods

        /// <summary>
        /// Scrolls the specified child control into view on an auto-scroll enabled control.
        /// </summary>
        /// <param name="activeControl">The child control to scroll into view.</param>
        void ScrollControlIntoView(System.Windows.Forms.Control activeControl);


        /// <summary>
        /// Sets the size of the auto-scroll margins.
        /// </summary>
        /// <param name="x">The System.Drawing.Size.Width value.</param>
        /// <param name="y">The System.Drawing.Size.Height value.</param>
        void SetAutoScrollMargin(int x, int y);

        #endregion

        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether the container enables the user to scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        /// <returns>true if the container enables auto-scrolling; otherwise, false. The default value is false.</returns>
        bool AutoScroll { set; get; }

        /// <summary>
        /// Gets or sets the size of the auto-scroll margin.
        /// </summary>
        /// <returns>
        /// A System.Drawing.Size that represents the height and width of the auto-scroll margin in pixels.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"> The system.Drawing.Size.Height or 
        /// System.Drawing.Size.Width value assigned is less than 0.</exception>
        System.Drawing.Size AutoScrollMargin { set; get; }


        /// <summary>
        /// Gets or sets the minimum size of the auto-scroll.
        /// </summary>
        /// <returns>
        /// A System.Drawing.Size that represents the minimum height and width of the scrollbars in pixels.
        /// </returns>
        System.Drawing.Size AutoScrollMinSize { set; get; }


        /// <summary>
        /// Gets or sets the location of the auto-scroll position.
        /// </summary>
        /// <returns>A System.Drawing.Point that represents the auto-scroll position in pixels.
        /// </returns>
        System.Drawing.Point AutoScrollPosition { set; get; }


        /// <summary>
        /// Gets the characteristics associated with the horizontal scroll bar.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.HScrollProperties that contains information about the horizontal scroll bar.
        /// </returns>
        System.Windows.Forms.HScrollProperties HorizontalScroll { get; }

        /// <summary>
        /// Gets the characteristics associated with the vertical scroll bar.
        /// </summary>
        /// <returns>
        /// A System.Windows.Forms.HScrollProperties that contains information about the vertical scroll bar.
        /// </returns>
        System.Windows.Forms.VScrollProperties VerticalScroll { get; }

        /// <summary>
        /// Occurs when the user or code scrolls through the client area.
        /// </summary>
        event System.Windows.Forms.ScrollEventHandler Scroll;


        #endregion


    }
}
