//********************************************************************************************************
// Product Name: MapWindowTools.mwIDW
// Description:  Inverse Distance Weighting point to raster interpolation
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
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna with guidence of MapWinGeoProc. Created in 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//********************************************************************************************************
using System.Collections.Generic;
using System.Collections;
using System;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MapWindow.Tools;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Components;

namespace MapWindowTools.Raster_Overlay_Class
{
    class mwSimpleRasterToPolygon:ITool
    {
        private string _workingPath;
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #region ITool Members
        /// <summary>
        /// Returns the Version of the tool
        /// </summary>
        public Version Version
        {
            get { return (new Version(1, 0, 0, 0)); }
        }

        /// <summary>
        /// Returns the Author of the tool's name
        /// </summary>
        public string Author
        {
            get { return ("MapWindow Development Team"); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return ("Raster Overlay"); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return ("This will Generate Polygon from given Raster."); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            MapWindow.Data.IRaster gridIn = _inputParam[0].Value as MapWindow.Data.IRaster;

            IFeatureSet polyOut = _outputParam[0].Value as IFeatureSet;

            return Execute(gridIn, polyOut, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons.
        /// </summary>
        /// <param name="gridIn">The Polygon Raster(Grid file).</param>
        /// <param name="polyOut">The Polygon shapefile path.</param>
        /// <param name="progress">The progress handler.</param>
        public bool Execute(IRaster gridIn, IFeatureSet polyOut, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (gridIn == null || polyOut == null)
            {
                return false;
            }
            int maxX, maxY;
            int current = 0;
            int previous = 0;
            double noData, currVal, currTrack;
            string strTrackPath;
            IRaster gridTrack = new Raster();


            maxX = gridIn.NumRows - 1;
            maxY = gridIn.NumColumns - 1;
            noData = gridIn.NoDataValue;

            //strTrackPath = System.IO.Path.GetDirectoryName(strInRast) + "\\" + System.IO.Path.GetFileNameWithoutExtension(strInRast) + "_track.bgd";
            gridTrack = Raster.Create("gridTrack.bgd", "", gridIn.NumColumns, gridIn.NumRows, 1, gridIn.DataType, new string[] { "" });
            //gridTrack.CreateNew("gridTrack", "", gridIn.NumColumns, gridIn.NumRows, 1, gridIn.DataType, new string[] { "" });
            gridTrack.Bounds = gridIn.Bounds;
            gridTrack.NoDataValue = gridIn.NoDataValue;

            polyOut.DataTable.Columns.Add("Value", typeof(int));
            polyOut.DataTable.Columns.Add("Zone", typeof(string));
            polyOut.DataTable.Columns.Add("Area", typeof(double));
            polyOut.DataTable.Columns.Add("COMID", typeof(string));
            polyOut.DataTable.Columns.Add("AveSlope", typeof(double));

            for (int i = 0; i <= maxX; i++)
            {

                current = Convert.ToInt32(i * 100 / maxX);
                //only update when increment in percentage
                if (current > previous+5)
                {
                    cancelProgressHandler.Progress("", current, current.ToString() + "% progress completed");
                    previous = current;
                }
                    

                for (int j = 0; j <= maxY; j++)
                {
                    if (i > 0 && j > 0)
                    {
                        currVal = Convert.ToInt16(gridIn.Value[i, j]);
                        currTrack = Convert.ToInt16(gridTrack.Value[i, j]);
                        if (currVal == gridIn.NoDataValue)
                        {
                            gridTrack.Value[i, j] = 1;

                            if (cancelProgressHandler.Cancel == true)
                                return false;
                        }
                        else
                        {
                            if (currTrack == 1)
                            {
                            }
                            else
                            {
                                formPolyFromCell(gridIn, gridTrack, i, j, polyOut, cancelProgressHandler);

                                if (cancelProgressHandler.Cancel == true)
                                    return false;
                            }
                        }
                    }
                    else
                    {
                        gridTrack.Value[i, j] = gridIn.NoDataValue;
                    }

                    

                }
            }
            gridIn.Close();
            gridTrack.Close();
            polyOut.SaveAs(polyOut.Filename, true);
            polyOut.Close();
            return true;

        }


        private void formPolyFromCell(IRaster gridMain, IRaster gridTrack, int startX, int startY, IFeatureSet polyOut, ICancelProgressHandler cancelProgressHandler)
        {
            int nextDir = -1;
            int nextX = -1;
            int nextY = -1;
            int startDir = -1;
            int count =0;

            //ArrayList lstPoints = new ArrayList();
            List<Point> lstPoints = new List<Point>();
            Point startPoint = new Point();
            Point endPoint = new Point();
            lstPoints.Clear();
            startDir = 1;
            cycleCellDirForPoly(gridMain, gridTrack, startX, startY, startDir, gridMain.Value[startX, startY], lstPoints, ref nextDir, ref nextX, ref nextY);
            if (nextDir != -1)
            {
                startPoint = lstPoints[0];
                if (lstPoints.Count > 1)
                {
                    endPoint = (Point)lstPoints[lstPoints.Count - 1];
                }
                else
                {
                    endPoint.X = -1;
                    endPoint.Y = -1;
                }
                if (startPoint == null)
                    return;
                try
                {
//check for speed
                    while (startPoint.X != endPoint.X || startPoint.Y != endPoint.Y)
                    {
                        //count++;
                        cycleCellDirForPoly(gridMain, gridTrack, nextX, nextY, nextDir, gridMain.Value[nextX, nextY], lstPoints, ref nextDir, ref nextX, ref nextY);
                        endPoint = (Point)lstPoints[lstPoints.Count - 1];

                    }
                }
                catch (Exception ex)
                {
                    //throw new SystemException("Exception in formPolyFromCell",ex);
                }

            }
            
            addPolyFromPointsList(polyOut, lstPoints, gridMain.Value[startX, startY], cancelProgressHandler);
            int tmpNum = polyOut.Features.Count - 1;
            if (polyOut.Features.Count == 0)
                return;
            fillTrackGrid(gridTrack, polyOut, tmpNum);
        }
       
        private void fillTrackGrid(IRaster gridTrack, IFeatureSet polyOut, int idxToFill)
        {
            int rowStart;
            int rowStop;
            int colStart;
            int colStop;
            RcIndex start;
            RcIndex stop;
            ICoordinate I;
            start = gridTrack.ProjToCell(polyOut.Features[idxToFill].Envelope.Minimum.X, polyOut.Features[idxToFill].Envelope.Maximum.Y);
            stop = gridTrack.ProjToCell(polyOut.Features[idxToFill].Envelope.Maximum.X, polyOut.Features[idxToFill].Envelope.Minimum.Y);
            rowStart = start.Row;
            colStart = start.Column;
            rowStop = stop.Row;
            colStop = stop.Column;
            for (int rowcell = rowStart; rowcell <= rowStop; rowcell++)
            {
                for (int colcell = colStart; colcell <= colStop; colcell++)
                {
                    I = gridTrack.CellToProj(rowcell, colcell);
                    Point pt = new Point(I);
                    if (polyOut.Features[idxToFill].Covers(pt))
                    {
                        gridTrack.Value[rowcell, colcell] = 1;
                    }
                }
            }
        }

        private void addPolyFromPointsList(IFeatureSet polyOut, List<Point> lstPoints, double currVal, ICancelProgressHandler cancelProgressHandler)
        {

            List<ICoordinate> coordinates = new List<ICoordinate>();
            double area;

 //check           if (lstPoints.Count > 1)
            if (lstPoints.Count > 2)
            {
                foreach (Point tmpPoint in lstPoints)
                {
                    coordinates.Add(tmpPoint.Coordinate);
                }
                try
                {
                    IFeature tmpPoly = new Feature(FeatureTypes.Polygon, coordinates);
                    polyOut.Features.Add(tmpPoly);
                    area = tmpPoly.Area();
                }
                catch (Exception ex)
                {
                    throw new SystemException("Could not add new polygon to featureset in addPolyFromPointsList", ex);
                }



                polyOut.DataTable.Rows[polyOut.Features.Count - 1][2] = area;
                if (currVal == 1)
                {
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][0] = 1;
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][1] = "Initiation";
                }
                if (currVal == 3)
                {
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][0] = 3;
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][1] = "In-flow";
                }
                if (currVal > 3)
                {
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][0] = 2;
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][1] = "Out-flow";
                    polyOut.DataTable.Rows[polyOut.Features.Count - 1][3] = Convert.ToString(currVal);
                }


            }
        }

        private void cycleCellDirForPoly(IRaster gridMain, IRaster gridTrack, int startX, int startY, int startDir, double checkVal, List<Point> lstPoints, ref int nextDir, ref int nextX, ref int nextY)
        {
            int maxRow, maxCol = 0;
            Point pntCurr = new Point();
            int currDir = -1;
            int cellX = -1;
            int cellY = -1;
            double cellVal = -1;
            Point startPoint = new Point();
            bool breakFlag;
            nextDir = -1;
            nextX = -1;
            nextY = -1;
            gridTrack.Value[startX, startY] = 1;
            if (startDir == 2 || startDir == 4 || startDir == 6 || startDir == 8)
            {
                currDir = ((startDir + 5) % 8) + 1;
            }
            else
            {
                currDir = ((startDir + 6) % 8) + 1;
            }
            for (int i = 1; i <= 9; i++)
            {
                breakFlag = false;
                if (currDir == 2 || currDir == 4 || currDir == 6 || currDir == 8)
                {
                    pntCurr = getCellCornerPoint(gridMain, startX, startY, currDir);
                    lstPoints.Add(pntCurr);
                    if (lstPoints.Count > 1)
                    {
                        startPoint = (Point)lstPoints[0];
                        if (pntCurr.X == startPoint.X && pntCurr.Y == startPoint.Y)
                            breakFlag = true;
                    }

                }
                maxRow = gridMain.NumRows;
                maxCol = gridMain.NumColumns;

                ////Chech the Bottom Border then redirect along the boder
                //if (startX == 0 && startY == 0)
                //    currDir = 2;
                //else if (startX == 0)
                //    currDir = 7;
                //else if (startY == 0)
                //    currDir = 1;

                ////Chech the Top Border then redirect along the boder
                //if (startX == maxCol - 1 && startY == maxRow - 1)
                //    currDir = 6;
                //else if (startX == maxCol - 1)
                //    currDir = 3;
                //else if (startY == maxRow - 1)
                //    currDir = 5;
            
                getValAndCellInDir(gridMain, startX, startY, currDir, ref cellVal, ref cellX, ref cellY);

                if (cellVal == checkVal)
                {
                    nextDir = currDir;
                    nextX = cellX;
                    nextY = cellY;
                    break;
                }
                currDir = currDir % 8 + 1;
                if (breakFlag)
                {
                    break;
                }

            }
        }

        private void getValAndCellInDir(IRaster gridMain, int startX, int startY, int currDir, ref double dirVal, ref int dirX, ref int dirY)
        {
            switch (currDir)
            {
                case 1:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX + 1, startY]);
                        dirX = startX + 1;
                        dirY = startY;
                        break;
                    }
                case 2:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX + 1, startY + 1]);
                        dirX = startX + 1;
                        dirY = startY + 1;
                        break;
                    }
                case 3:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX, startY + 1]);
                        dirX = startX;
                        dirY = startY + 1;
                        break;
                    }
                case 4:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX - 1, startY + 1]);
                        dirX = startX - 1;
                        dirY = startY + 1;
                        break;
                    }
                case 5:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX - 1, startY]);
                        dirX = startX - 1;
                        dirY = startY;
                        break;
                    }
                case 6:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX - 1, startY - 1]);
                        dirX = startX - 1;
                        dirY = startY - 1;
                        break;
                    }
                case 7:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX, startY - 1]);
                        dirX = startX;
                        dirY = startY - 1;
                        break;
                    }
                case 8:
                    {
                        dirVal = Convert.ToInt32(gridMain.Value[startX + 1, startY - 1]);
                        dirX = startX + 1;
                        dirY = startY - 1;
                        break;
                    }
            }

        }


        private Point getCellCornerPoint(IRaster g, int x, int y, int corner)
        {
            ICoordinate center;
            double centerX;
            double centerY;
            double hcellwidth;
            double hcellheight;
            double cornerX;
            double cornerY;
            
            hcellwidth = g.CellWidth/2;
            hcellheight = g.CellHeight/2;
            center = g.CellToProj(x, y);
            centerX = center.X;
            centerY = center.Y;
            switch (corner)
            {
                case 2:
                    {
                        cornerX = centerX + hcellwidth;
                        cornerY = centerY - hcellheight;
                        break;
                    }
                case 4:
                    {
                        cornerX = centerX - hcellwidth;
                        cornerY = centerY - hcellheight;
                        break;
                    }
                case 6:
                    {
                        cornerX = centerX - hcellwidth;
                        cornerY = centerY + hcellheight;
                        break;
                    }
                case 8:
                    {
                        cornerX = centerX + hcellwidth;
                        cornerY = centerY + hcellheight;
                        break;
                    }
                default:
                    {
                        cornerX = -1;
                        cornerY = -1;
                        break;
                    }
            }

            Point tmpPoint = new Point(cornerX, cornerY);
            return tmpPoint;
        }


        /// <summary>
        /// Image displayed in the help area when no input field is selected
        /// </summary>
        public System.Drawing.Bitmap HelpImage
        {
            get { return null; }
        }

        /// <summary>
        /// Help text to be displayed when no input field is selected
        /// </summary>
        public string HelpText
        {
            get
            {
                return ("This will Generate Polygon from given Raster.");
            }
        }

        /// <summary>
        /// Returns the address of the tools help web page in HTTP://... format. Return a empty string to hide the help hyperlink.
        /// </summary>
        public string HelpURL
        {
            get { return ("HTTP://www.mapwindow.org"); }
        }


        /// <summary>
        /// 32x32 Bitmap - The Large icon that will appears in the Tool Dialog Next to the tools name
        /// </summary>
        public System.Drawing.Bitmap Icon
        {
            get { return (null); }
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new RasterParam("input1 polygon Raster");
            _inputParam[0].HelpText = "Input Raster to change to Polygon featureset";

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam("Output converted polygon.");
            _outputParam[0].HelpText = "Select the Result of Output feature set with average slope in the attribute.";
        }

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public MapWindow.Tools.Param.Parameter[] InputParameters
        {
            get { return _inputParam; }
        }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        public string Name
        {
            get { return ("Raster To Polygon"); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public MapWindow.Tools.Param.Parameter[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        void ITool.ParameterChanged(Parameter sender)
        {
            return;
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return ("Raster To Polygon generation"); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return ("MapWindow RasterToPolygon"); }
        }

        /// <summary>
        /// This is set before the tool is executed to provide a folder where the tool can save temporary data
        /// </summary>
        public string WorkingPath
        {
            set { _workingPath = value; }
        }
        #endregion
    }
}
