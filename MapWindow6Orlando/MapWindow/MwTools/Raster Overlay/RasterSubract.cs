//********************************************************************************************************
// Product Name: MapWindow.Tools.mwRasterSubract
// Description:  Subtract second raster from first raster cell by cell
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
// KP                     |  9/2009                |  Used IDW as model for RasterSubract
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************
using System;
using MapWindow.Tools;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;


namespace MapWindow.Tools
{
    public class RasterSubract:ITool
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
            get { return (TextStrings.MapWindowDevelopmentTeam);}
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return (TextStrings.RasterOverlay); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.RasterSubractDescription); }//RasterSubractDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            IRaster input2 = _inputParam[1].Value as IRaster;
            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, input2, output, cancelProgressHandler);

        }
        /// <summary>
        /// Executes the Erase Opaeration tool programaticaly
        /// Ping Yang deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input1">The input raster</param>
        /// <param name="input2">The input raster</param>
        /// <param name="output">The output raster</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IRaster input1, IRaster input2, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }

            IEnvelope envelope = UnionEnvelope(input1, input2);

            //Figures out which raster has smaller cells
            IRaster smallestCellRaster;
            if (input1.CellWidth < input2.CellWidth) smallestCellRaster = input1;
            else smallestCellRaster = input2;

            //Given the envelope of the two rasters we calculate the number of columns / rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / smallestCellRaster.CellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / smallestCellRaster.CellHeight));

            
            //create output raster
            output = Raster.Create(output.Filename, "", noOfCol, noOfRow, 1, typeof(int), new[] { "" });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            RcIndex v1;
            RcIndex v2;

            int previous=0;
            int max = (output.Bounds.NumRows + 1);
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    Coordinate I1 = output.CellToProj(i, j);
                    v1 = input1.ProjToCell(I1);
                    double val1;
                    if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                        val1 = input1.Value[v1.Row, v1.Column];
                    else
                        val1 = input1.NoDataValue;

                    v2 = input2.ProjToCell(I1);
                    double val2;
                    if (v2.Row <= input2.EndRow && v2.Column <= input2.EndColumn && v2.Row > -1 && v2.Column > -1)
                        val2 = input2.Value[v2.Row, v2.Column];
                    else
                        val2 = input2.NoDataValue;

                    if (val1 == input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        output.Value[i, j] = output.NoDataValue;
                    }
                    else if (val1 != input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        output.Value[i, j] = output.NoDataValue;
                    }
                    else if (val1 == input1.NoDataValue && val2 != input2.NoDataValue)
                    {
                        output.Value[i, j] = output.NoDataValue;
                    }
                    else
                    {
                        output.Value[i, j] = val1 - val2;
                    }

                    if (cancelProgressHandler.Cancel)
                        return false;
                }
                int current = Convert.ToInt32(Math.Round(i * 100D / max));
                //only update when increment in persentage
                if (current > previous)
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                previous = current;
            }

            //output = Temp;
            output.Save();
            return true;
        }
        /// <summary>
        /// Execute the union region for output envelope 
        /// </summary>
        /// <param name="input1">input raster</param>
        /// <param name="input2">input raster</param>
        /// <returns>Envelope</returns>
        private static IEnvelope UnionEnvelope(IRaster input1, IRaster input2)
        {
            IEnvelope e1 = input1.Bounds.Envelope;
            IEnvelope e2 = input2.Bounds.Envelope;
            e1.ExpandToInclude(e2);
            return (e1);

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
                return (TextStrings.RasterSubractDescription);
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
            _inputParam = new Parameter[2];
            _inputParam[0] = new RasterParam(TextStrings.input1Raster);
            _inputParam[0].HelpText = TextStrings.InputFirstRaster;
            _inputParam[1] = new RasterParam(TextStrings.input2Raster);
            _inputParam[1].HelpText = TextStrings.InputSecondRasterforSubtract;

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster);
            _outputParam[0].HelpText = TextStrings.ResultRasterDirectory;

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
            get { return (TextStrings.SubtractRasterLayer); }
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
            get { return (TextStrings.SubtractRasterLayer); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowRasterSubtraction); }
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
