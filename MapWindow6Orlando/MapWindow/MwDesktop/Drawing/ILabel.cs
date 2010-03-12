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
using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Drawing
{
    /// <summary>
    /// Supports the properties specific for a single label
    /// </summary>
    public interface ILabelOld: IRenderable
    {

        /// <summary>
        /// This may be slower, but in the case of rotated text, this polygon
        /// should return the boundaries of the label itself.
        /// </summary>
        IPolygon Boundary
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the label text is bold.
        /// </summary>
        bool Bold
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the brush to draw the text using
        /// </summary>
        Brush Brush
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the font to draw the label
        /// </summary>
        Font Font
        {
            get;
            set;
        }

      

        /// <summary>
        /// A degree measure for rotation, with 0 being horizontal, and
        /// each added degree rotating the label by that much.
        /// </summary>
        double Orientation
        {
            get;
            set;
        }


        /// <summary>
        /// Scaling labels adjust with the zoom level so that they get smaller
        /// as you zoom out, and get larger as you zoom in.
        /// </summary>
        bool Scaling
        {
            get;
            set;
        }

        /// <summary>
        /// The actual text to be drawn
        /// </summary>
        string Text
        {
            get;
            set;
        }
        


    }
}
