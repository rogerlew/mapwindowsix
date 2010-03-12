//********************************************************************************************************
// Product Name: MapWindow.Tools.mwRasterSlope
// Description:  Generate slope raster from given altitude raster.
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
//------------------------|------------------------|--------------------------------------------------
// KP                     |  9/2009                |  Used IDW as model for RasterSlope
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;



namespace MapWindow.Tools
{
    public class RasterSlope:ITool
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
            get { return (TextStrings.RasterSlopeDescription); }//RasterSlopeDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            MapWindow.Data.IRaster input1 = _inputParam[0].Value as MapWindow.Data.IRaster;
            double inZFactor = (double)_inputParam[1].Value;

            bool slopeInPercent =(bool) _inputParam[2].Value;

            MapWindow.Data.IRaster output = _outputParam[0].Value as MapWindow.Data.IRaster;

            return Execute(input1, inZFactor,slopeInPercent, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="ras">The input altitude raster.</param>
        /// <param name="output">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(IRaster ras,double inZFactor, bool slopeInPercent,IRaster output, MapWindow.Tools.ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (ras == null || output == null)
            {
                return false;
            }
            double z1, z2, z3, z4, z5, z6, z7, z8, dZ_dx, dZ_dy, slope;
            int current, previous;
            try
            {
                int noOfCol = ras.NumColumns;
                int noOfRow = ras.NumRows;
                IEnvelope envelope1 = new Envelope() as IEnvelope;
                envelope1 = ras.Bounds.Envelope;


                output = Raster.Create(output.Filename, "", noOfCol, noOfRow, 1, typeof(double), new string[] { "" });
                output.NoDataValue = ras.NoDataValue;

                //output.Bounds = ras.Bounds.Copy();
                output.Bounds = ras.Bounds;

                previous = 0;
                for (int i = 0; i < output.NumRows; i++)
                {
                    current = Convert.ToInt32(Math.Round(i * 100D / output.NumRows));
                    //only update when increment in percentage
                    if (current > previous)
                        cancelProgressHandler.Progress("", current, current.ToString() + TextStrings.progresscompleted);
                    previous = current;

                    for (int j = 0; j < output.NumColumns; j++)
                    {
                        if (i > 0 && i < output.NumRows - 1 && j > 0 && j < output.NumColumns - 1)
                        {
                            z1 = ras.Value[i - 1, j - 1];
                            z2 = ras.Value[i - 1, j];
                            z3 = ras.Value[i - 1, j + 1];
                            z4 = ras.Value[i, j - 1];
                            z5 = ras.Value[i, j + 1];
                            z6 = ras.Value[i + 1, j - 1];
                            z7 = ras.Value[i + 1, j];
                            z8 = ras.Value[i + 1, j + 1];

                            //3rd Order Finite Difference slope algorithm
                            dZ_dx = inZFactor * ((z3 - z1) + 2 * (z5 - z4) + (z8 - z6)) / (8 * ras.CellWidth);
                            dZ_dy = inZFactor * ((z1 - z6) + 2 * (z2 - z7) + (z3 - z8)) / (8 * ras.CellHeight);

                            slope = Math.Atan(Math.Sqrt((dZ_dx * dZ_dx) + (dZ_dy * dZ_dy))) * (180 / Math.PI);
                            //change to radious and in persentage
                            if (slopeInPercent)
                                slope = (Math.Tan(slope * Math.PI / 180)) * 100;
                            output.Value[i, j] = slope;

                            if (cancelProgressHandler.Cancel == true)
                                return false;
                        }
                        else
                            output.Value[i, j] = output.NoDataValue;

                        if (cancelProgressHandler.Cancel == true)
                            return false;
                    }

                }

                //output = Temp;
                if (output.IsFullyWindowed())
                {
                    output.Save();
                    return true;
                }
                else
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
                return (TextStrings.RasterSlopeDescription);
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
            _inputParam[2] = new BooleanParam(TextStrings.slopeinpercentage, TextStrings.boxSlopeInPercentage, false);
            _inputParam[2].HelpText = TextStrings.slopeinpercentageindegree;

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputslopeRaster);
            _outputParam[0].HelpText = TextStrings.Resultofaverageslope;
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
            get { return (TextStrings.SlopeRasterLayer); }
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
            get { return (TextStrings.GenerateslopeRasterLayer); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowRasterSlope); }
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
