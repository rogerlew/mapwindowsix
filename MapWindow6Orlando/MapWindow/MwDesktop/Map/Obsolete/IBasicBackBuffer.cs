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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2008 1:58:12 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;

namespace MapWindow.GeoMap
{


    /// <summary>
    /// IBackBuffer
    /// </summary>
    public interface IBasicBackBuffer
    {
      

        #region Methods

        /// <summary>
        /// This clears the specified clip rectangle.  If no rectangle is specified, it clears the current
        /// ClientRectangle region.
        /// </summary>
        void Clear(Rectangle clipRectangle, Color BackgroundColor);

        /// <summary>
        /// Draws the cliprectangel to the graphics device.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        void Draw(Graphics g, Rectangle clipRectangle);
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the source rectangle in back buffer coordinates where the image
        /// should be copied from.
        /// </summary>
        Rectangle SourceRectangle
        {
            get;
            set;
        }


        #endregion



    }
}
