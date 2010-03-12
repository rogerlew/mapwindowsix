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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:33:47 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Legacy
{


    /// <summary>
    /// Reports
    /// </summary>
    public interface IReports
    {
      

        #region Methods
        

        /// <summary>
        /// Similar to <c>GetLegendSnapshot</c> except that only one layer is considered.
        /// </summary>
        /// <param name="LayerHandle">Handle of the layer to take a snapshot of.</param>
        /// <param name="imgWidth">Maximum width of the image.  The height of the image depends on the coloring scheme of the layer.</param>
        Image GetLegendLayerSnapshot(int LayerHandle, int imgWidth);

        /// <summary>
        /// Returns an image of the legend.
        /// </summary>
        /// <param name="VisibleLayersOnly">Specifies that only the visible layers are part of the snapshot.</param>
        /// <param name="imgWidth">Maximum width of the image.  The height of the image depends on the number of layers loaded.</param>
        Image GetLegendSnapshot(bool VisibleLayersOnly, int imgWidth);

        

        /// <summary>
        /// Returns an image of a north arrow.
        /// </summary>
        Image GetNorthArrow();

        /// <summary>
        /// Returns an image that represents an accurate scale bar.
        /// </summary>
        /// <param name="MapUnits">You must specify what the map units are.</param>
        /// <param name="ScalebarUnits">The unit of measurement to display on the scale bar.  This function can convert the map units to any other unit.</param>
        /// <param name="MaxWidth">Maximum width of the scale bar image.</param>
        Image GetScaleBar(UnitsOfMeasure MapUnits, UnitsOfMeasure ScalebarUnits, int MaxWidth);

        /// <summary>
        /// Returns an image that represents an accurate scale bar.
        /// </summary>
        /// <param name="MapUnits">You must specify what the map units are.</param>
        /// <param name="ScalebarUnits">The unit of measurement to display on the scale bar.  This function can convert the map units to any other unit.</param>
        /// <param name="MaxWidth">Maximum width of the scale bar image.</param>
        Image GetScaleBar(string MapUnits, string ScalebarUnits, int MaxWidth);


        /// <summary>
        /// Returns a <c>MapWinGIS.Image</c> of the view at the specified extents.
        /// </summary>
        /// <param name="BoundBox">The area that you wish to take the picture of.  Uses projected map units.</param>
        Image GetScreenPicture(Envelope BoundBox);

        #endregion

    

       

        

        

       

       

        
    }
}
