//********************************************************************************************************
// Product Name: MapWindow.Tools.mwFindAveregeSlopeExtented
// Description:  Calculate Avrage Slope for given polygons with more user preference
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
// KP                     |  9/2009                |  Used IDW as model for FindAveregeSlopeExtented
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindow.Tools.Terrain_Analysis
{
    public class FindAveregeSlopeExtented:ITool
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
            get { return (TextStrings.FindAveregeSlopeExtentedDescription); }//FindAveregeSlopeExtentedDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster grid = _inputParam[0].Value as IRaster;
            double inZFactor = (double)_inputParam[1].Value;
            bool slopeInPercent = (bool)_inputParam[2].Value;
            IFeatureSet poly = _inputParam[3].Value as IFeatureSet;
            IFeatureSet outerShpFile = _inputParam[4].Value as IFeatureSet;
            int outerShpIndex = (int)_inputParam[5].Value;
            string fldInPolyToStoreSlope = (string)_inputParam[6].Value;

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(grid, inZFactor, slopeInPercent, poly, fldInPolyToStoreSlope, outerShpFile, outerShpIndex, output, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons with more user preferences.
        /// </summary>
        /// <param name="ras">The dem Raster(Grid file).</param>
        /// <param name="inZFactor">The scaler factor</param>
        /// <param name="slopeInPercent">The slope in percentage.</param>
        /// <param name="poly">The flow poly shapefile path.</param>
        /// <param name="fldInPolyToStoreSlope">The field name to store average slope in the attribute.</param>
        /// <param name="outerShpFile">The Featureset where we have the area of interest</param>
        /// <param name="outerShpIndex">The index of featureset which give paticular area of interest.</param>
        /// <param name="output">The path to save created slope Feature set.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(IRaster ras, double inZFactor, bool slopeInPercent, IFeatureSet poly, string fldInPolyToStoreSlope, IFeatureSet outerShpFile, int outerShpIndex, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (ras == null || poly == null || outerShpFile == null || output == null)
            {
                return false;
            }
            if (poly.FeatureType != FeatureTypes.Polygon || outerShpFile.FeatureType != FeatureTypes.Polygon)
                return false;

            int previous = 0;
            IRaster slopegrid = new Raster();

            int[] areaCount = new int[poly.Features.Count];
            double[] areaTotal = new double[poly.Features.Count];
            double[] areaAve = new double[poly.Features.Count];

            Slope(ras, inZFactor, slopeInPercent, slopegrid, cancelProgressHandler);
            if (slopegrid == null)
                throw new SystemException(TextStrings.Slopegridfileisnull);
            foreach (IFeature f in poly.Features)
                output.Features.Add(f);

            for (int i = 0; i < slopegrid.NumRows; i++)
            {
                int current = Convert.ToInt32(Math.Round(i * 100D  / slopegrid.NumRows));
                //only update when increment in percentage
                if (current > previous+5)
                {
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                    previous = current;
                }
                    


                for (int j = 0; j < slopegrid.NumColumns; j++)
                {
                    Coordinate coordin = slopegrid.CellToProj(i, j);
                    IPoint pt = new Point(coordin);
                    IFeature point = new Feature(pt);
                    if (!outerShpFile.Features[outerShpIndex].Covers(point))
                        continue;//not found the point inside.
                    for (int c = 0; c < poly.Features.Count; c++)
                    {
                        if (output.Features[c].Covers(point))
                        {
                            areaCount[c]++;
                            areaTotal[c] += slopegrid.Value[i, j] / 100;
                        }
                        if (cancelProgressHandler.Cancel)
                            return false;
                    }

                }
            }
            //Add the column
            output.DataTable.Columns.Add("FID", typeof(int));
            output.DataTable.Columns.Add(fldInPolyToStoreSlope, typeof(Double));
            for (int c = 0; c < output.Features.Count; c++)
            {

                if (areaCount[c] == 0)
                    areaAve[c] = 0.0;
                else
                    areaAve[c] = areaTotal[c] / areaCount[c];
                //Add the field values
                output.Features[c].DataRow["FID"] = c;
                output.Features[c].DataRow[fldInPolyToStoreSlope] = areaAve[c];
            }
            output.SaveAs(output.Filename,true);
            slopegrid.Close();
            ras.Close();
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
        private static void Slope(IRaster ras, double inZFactor, bool slopeInPercent, IRaster result, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (ras == null || result == null)
            {
                return;
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
                                return;
                        }
                        else
                            temp.Value[i, j] = temp.NoDataValue;

                        if (cancelProgressHandler.Cancel)
                            return;
                    }

                }

                result = temp;
                if (result.IsFullyWindowed())
                {
                    result.Save();
                    return;
                }
                return;
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
                return (TextStrings.Generateaverageslopewithpreferences);
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
            _inputParam = new Parameter[7];
            _inputParam[0] = new RasterParam(TextStrings.input1altitudeRaster);
            _inputParam[0].HelpText = TextStrings.InputRasterforaverageslopecalculation;
            _inputParam[1] = new DoubleParam(TextStrings.inputZfactor, 1.0);
            _inputParam[1].HelpText = TextStrings.InputZfactorforslopedisplay;
            _inputParam[2] = new BooleanParam(TextStrings.slopeinpercentage, TextStrings.boxSlopeInPercentage, false);
            _inputParam[2].HelpText = TextStrings.slopeinpercentage;
            _inputParam[3] = new FeatureSetParam(TextStrings.input1polygonfeatureset);
            _inputParam[3].HelpText = TextStrings.averageslopeinarribute;
            _inputParam[4] = new FeatureSetParam(TextStrings.inputtheareaofinterest);
            _inputParam[4].HelpText = TextStrings.featuresetcontainareainterest;
            _inputParam[5] = new IntParam(TextStrings.Indexofareaofinterestfeature, 0);
            _inputParam[5].HelpText = TextStrings.indexspecificarea;
            _inputParam[6] = new StringParam(TextStrings.Fieldnameforavrageslope, TextStrings.AveSlope);
            _inputParam[6].HelpText = TextStrings.Fieldnamecolomavrageslope;

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.Outputwithaverageslope);
            _outputParam[0].HelpText = TextStrings.SelecttheResultofOutput;
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
            get { return (TextStrings.FindAvrageSlopeExtented); }
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
            get { return (TextStrings.FindAveregeSlopeExtentedDescription); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowFindAverageSlopeExtented); }
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
