//********************************************************************************************************
// Product Name: MapWindow.Tools.mwFindAveregeSlope
// Description:  Generate the average slope in the given polygons.
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
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna. Created in 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// -----------------------|------------------------|--------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper  
// KP                     |  9/2009                |  Used IDW as model for FindAveregeSlope
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindow.Tools
{
    public class FindAveregeSlope:ITool
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
            get { return (TextStrings.MapWindowDevelopmentTeam); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return (TextStrings.TerrainAnalysis); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.FindAveregeSlopeDescription); }//FindAveregeSlopeDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster grid = _inputParam[0].Value as IRaster;
            double inZFactor = (double)_inputParam[1].Value;
            IFeatureSet poly = _inputParam[2].Value as IFeatureSet;

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(grid, inZFactor, poly,output, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons.
        /// </summary>
        /// <param name="ras">The dem Raster(Grid file).</param>
        /// <param name="zFactor">The scaler factor</param>
        /// <param name="poly">The flow poly shapefile path.</param>
        /// <param name="output">The resulting DEM of slopes</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        public bool Execute(IRaster ras, double zFactor, IFeatureSet poly, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (ras == null || poly == null || output == null)
            {
                return false;
            }
            output.FeatureType = poly.FeatureType;
            foreach (IFeature f in poly.Features)
                output.Features.Add(f);
            output.DataTable.Columns.Add("FID", typeof(int));
            output.DataTable.Columns.Add(TextStrings.AveSlope, typeof(Double));

            IRaster slopeGrid = new Raster();
            slopeGrid.DataType = ras.DataType;
            slopeGrid.Bounds = ras.Bounds;

            //FeatureSet polyShape = new FeatureSet();
            int previous = 0;

            if (Slope(ref ras, zFactor, false, ref slopeGrid, cancelProgressHandler) == false)
                return false;

            int shapeCount = output.Features.Count;
            int[] areaCount = new int[shapeCount];
            double[] areaTotal = new double[shapeCount];
            double[] areaAve = new double[shapeCount];
            double dxHalf = slopeGrid.CellWidth / 2;
            double dyHalf = slopeGrid.CellHeight / 2;

            //check whether those two envelope are intersect
            if (ras.Bounds.Envelope.Intersects(output.Envelope) == false)
                return false;

            RcIndex start = slopeGrid.ProjToCell(output.Envelope.Minimum.X, output.Envelope.Maximum.Y);
            RcIndex stop = slopeGrid.ProjToCell(output.Envelope.Maximum.X, output.Envelope.Minimum.Y);

            int rowStart = start.Row;
            int colStart = start.Column;
            int rowStop = stop.Row;
            int colStop = stop.Column;
            for (int row = rowStart - 1; row < rowStop + 1; row++)
            {
                int current = Convert.ToInt32((row - rowStart + 1) * 100.0 / (rowStop + 1 - rowStart + 1));
                //only update when increment in percentage
                if (current > previous + 5)
                {
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                    previous = current;
                }
                    


                for (int col = colStart - 1; col < colStop + 1; col++)
                {
                    Coordinate cent = slopeGrid.CellToProj(row, col);
                    double xCent = cent.X;
                    double yCent = cent.Y;
                    for (int shpindx = 0; shpindx < output.Features.Count; shpindx++)
                    {
                        IFeature tempFeat = output.Features[shpindx];
                        Point pt1 = new Point(xCent, yCent);
                        Point pt2 = new Point(xCent - dxHalf, yCent - dyHalf);
                        Point pt3 = new Point(xCent + dxHalf, yCent - dyHalf);
                        Point pt4 = new Point(xCent + dxHalf, yCent + dyHalf);
                        Point pt5 = new Point(xCent - dxHalf, yCent + dyHalf);
                        if ((((!tempFeat.Covers(pt1) && !tempFeat.Covers(pt2)) && !tempFeat.Covers(pt3)) &&
                             !tempFeat.Covers(pt4)) && !tempFeat.Covers(pt5)) continue;
                        areaCount[shpindx]++;
                        areaTotal[shpindx] += slopeGrid.Value[row, col] / 100;

                        if (cancelProgressHandler.Cancel)
                            return false;
                    }
                }
            }
            for (int shpindx = 0; shpindx < output.Features.Count; shpindx++)
            {
                if (areaCount[shpindx] == 0)
                {
                    areaAve[shpindx] = 0;
                }
                else
                {
                    areaAve[shpindx] = areaTotal[shpindx] / areaCount[shpindx];
                }
                output.Features[shpindx].DataRow["FID"] = shpindx;
                output.Features[shpindx].DataRow[TextStrings.AveSlope] = areaAve[shpindx];

            }
            poly.Close();
            slopeGrid.Close();
            output.SaveAs(output.Filename,true);
            return true;

        }

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="ras">The input altitude raster.</param>
        /// <param name="slopeInPercent"></param>
        /// <param name="result">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <param name="inZFactor"></param>
        /// <returns></returns>
        private static bool Slope(ref IRaster ras, double inZFactor, bool slopeInPercent,ref IRaster result, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (ras == null || result == null)
            {
                return false;
            }
            try
            {
                int noOfCol = ras.NumColumns;
                int noOfRow = ras.NumRows;

                //Create the new raster with the appropriate dimensions

                
                IRaster temp = Raster.Create("SlopeRaster.bgd", "", noOfCol, noOfRow, 1, typeof(double), new[] { "" });
                temp.NoDataValue = ras.NoDataValue;
                temp.Bounds = ras.Bounds;

                for (int i = 0; i < temp.NumRows; i++)
                {

                    for (int j = 0; j < temp.NumColumns; j++)
                    {
                        if (i > 0 && i < temp.NumRows - 1 && j > 0 && j < temp.NumColumns - 1)
                        {
                            double z1 = ras.Value[i - 1, j - 1];
                            double z2 = ras.Value[i - 1, j];
                            double z3 = ras.Value[i - 1, j + 1];
                            double z4 = ras.Value[i, j - 1];
                            double z5 = ras.Value[i, j + 1];
                            double z6 = ras.Value[i + 1, j - 1];
                            double z7 = ras.Value[i + 1, j];
                            double z8 = ras.Value[i + 1, j + 1];

                            //3rd Order Finite Difference slope algorithm
                            double dZ_dx = inZFactor * ((z3 - z1) + 2 * (z5 - z4) + (z8 - z6)) / (8 * ras.CellWidth);
                            double dZ_dy = inZFactor * ((z1 - z6) + 2 * (z2 - z7) + (z3 - z8)) / (8 * ras.CellHeight);

                            double slope = Math.Atan(Math.Sqrt((dZ_dx * dZ_dx) + (dZ_dy * dZ_dy))) * (180 / Math.PI);
                            //change to radious and in persentage
                            if (slopeInPercent)
                                slope = (Math.Tan(slope * Math.PI / 180)) * 100;
                            temp.Value[i, j] = slope;

                            if (cancelProgressHandler.Cancel)
                                return false;
                        }
                        else
                            temp.Value[i, j] = temp.NoDataValue;

                        if (cancelProgressHandler.Cancel)
                            return false;
                    }

                }

                result = temp;
                if (result.IsFullyWindowed())
                {
                    result.Save();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //throw new SystemException("Error in Slope: ", ex);
                throw new SystemException(ex.ToString());
            }
            

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
                return (TextStrings.FindAveregeSlopeDescription);
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
            _inputParam = new Parameter[3];
            _inputParam[0] = new RasterParam(TextStrings.input1altitudeRaster);
            _inputParam[0].HelpText = TextStrings.InputRasterforaverageslopecalculation;
            _inputParam[1] = new DoubleParam(TextStrings.inputZfactor, 1.0);
            _inputParam[1].HelpText = TextStrings.InputZfactorforslopedisplay;
            _inputParam[2] = new PolygonFeatureSetParam(TextStrings.input1polygonfeatureset);
            //_inputParam[2] = new FeatureSetParam(TextStrings."input1 polygon feature set");
            _inputParam[2].HelpText = TextStrings.FindAveregeSlopeDescription;

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.Outputfeaturesetwithaverageslope);
            _outputParam[0].HelpText = TextStrings.Resultofaverageslope;
        }

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public Parameter[] InputParameters
        {
            get { return _inputParam; }
        }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        public string Name
        {
            get { return (TextStrings.FindAverageSlope); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public Parameter[] OutputParameters
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
            get { return (TextStrings.CalculateSlopegivenpolygons); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowFindAverageSlope); }
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
