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
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MapWindow.Geometries;
using MapWindow.Drawing;
using MapWindow.Main;
namespace MapWindow.Scrap
{
    /// <summary>
    /// This is a class that organizes a list of renderable layers into a single "view" which might be
    /// shared by multiple displays.  For instance, if you have a map control and a print preview control,
    /// you could draw the same data frame property on both, just by passing the graphics object for each.
    /// Be sure to handle any scaling or translation that you require through the Transform property
    /// of the graphics object as it will ultimately be that scale which is used to back-calculate the
    /// appropriate pixel sizes for point-size, line-width and other non-georeferenced characteristics.
    /// </summary>
    public class DataFrame: object, IDataFrame
    {
        #region Variables

        /// <summary>
        /// The real world envelope bounding the data
        /// </summary>
        private IEnvelope _extents;
        
     

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor for the DataFrame
        /// </summary>
        public DataFrame()
        {
            _extents = new Envelope();
           
        }
        #endregion

        #region Methods

        

       

   



        #endregion

        #region Properties

        /// <summary>
        /// The envelope that contains all of the layers for this data frame.  Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public MapWindow.Geometries.IEnvelope Envelope
        {
            get
            {
               throw new NotImplementedException("Not implemented yet");
            }
        }


        /// <summary>
        /// Gets or sets the view extents in world coordinates, lat long for example.
        /// </summary>
        public MapWindow.Geometries.IEnvelope Extents
        {
            get { return _extents; }
            set { _extents = value; }
        }

        

        #endregion

        

    }
}
