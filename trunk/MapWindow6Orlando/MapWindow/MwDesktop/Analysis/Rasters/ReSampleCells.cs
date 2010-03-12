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
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created 9/10/2009 2:59:32 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -----------------------|------------------------|--------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper  
//********************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Map;


namespace MapWindow.Analysis.Rasters
{
    /// <summary>
    /// This class Resmple the given raster cells.
    /// </summary>
    class ReSampleCells
    {
        /// <summary>
        /// This will resample the cells.
        /// If the cell size is zero, this will default to the shorter of the width or height
        /// divided by 256
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="cellHeight">Cell Height</param>
        /// <param name="cellWidth">Cell Width</param>
        /// <param name="destFilename">The destintation file name</param>
        /// <returns>Resampled raster name</returns>
        public static IRaster ReSample(IRaster input1, double cellHeight, double cellWidth, string destFilename)
        {
            return ReSample(input1, cellHeight, cellWidth,destFilename,null);
        }

        /// <summary>
        /// This will resample the cells.
        /// If the cell size is zero, this will default to the shorter of the width or height
        /// divided by 256.
        /// </summary>
        /// <param name="input1">Input Raster.</param>
        /// <param name="cellHeight">New Cell Height or Null.</param>
        /// <param name="cellWidth">New Cell Width or Null.</param>
        /// <param name="destFilename">Output Raster Name.</param>
        /// <param name="progressHandler">An interface for handling the progress messages.</param>
        /// <returns>Resampled raster.</returns>
        public static IRaster ReSample(IRaster input1,double cellHeight, double cellWidth, string destFilename, IProgressHandler progressHandler)
        {
            if (input1 == null)
                return null;

            IEnvelope envelope = input1.Bounds.Envelope;

            if (cellHeight == 0)
            {
                cellHeight = envelope.Height / 256;
            }
            if(cellWidth==0)
            {
                cellWidth = envelope.Width / 256;
            }
                
            //Calculate new number of columns and rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / cellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / cellHeight));


            IRaster output = Raster.Create(destFilename, "", noOfCol, noOfRow, 1, input1.DataType, new[] { "" });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            RcIndex index1;
            int max = (output.Bounds.NumRows);
            ProgressMeter pm = new ProgressMeter(progressHandler, "ReSize Cells", max);
            //Loop throug every cell for new value

            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    //Projet the cell position to Map
                    Coordinate cellCenter = output.CellToProj(i, j);
                    index1 = input1.ProjToCell(cellCenter);

                    double val;
                    if (index1.Row <= input1.EndRow && index1.Column <= input1.EndColumn && index1.Row > -1 && index1.Column > -1)
                    {
                        if (input1.Value[index1.Row, index1.Column] == input1.NoDataValue)
                            val = output.NoDataValue;
                        else
                            val = input1.Value[index1.Row, index1.Column];
                    }
                    else
                        val = output.NoDataValue;

                    output.Value[i, j] = val;
                }
                pm.CurrentPercent = i;
            }

            output.Save();
            pm.Reset();
            return output;
        }

    }
}
