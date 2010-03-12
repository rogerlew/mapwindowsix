//********************************************************************************************************
// Product Name: MapWindowTools.mwThreshold
// Description:  A tool for creating a boolean mask for a raster based on a thresholg mask
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Jan, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using MapWindow.Tools;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindowTools
{
    
    class mwThreshold:ITool
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
            get { return ("Brian Marchionni"); }
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
            get { return ("This tool creates a raster mask based on a threshold value."); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input = _inputParam[0].Value as IRaster;
            Double threshold = Convert.ToDouble(_inputParam[1].Value);
            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input, threshold, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the threshold operation
        /// </summary>
        /// <param name="input1">The input raster</param>
        /// <param name="threshold">The threshold value</param>
        /// <param name="output">The output raster</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IRaster input, double threshold, IRaster output, MapWindow.Tools.ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            int noOfCol=input.NumColumns;
            int noOfRow=input.NumRows;
            int previous = 0;
            int current = 0;

            //Create the new raster with the appropriate dimensions
            output = Raster.Create(output.Filename, "", noOfCol, noOfRow,1 , typeof(int), new string[] { "" });
            output.CellWidth = input.CellWidth;
            output.CellHeight = input.CellHeight;
            output.Xllcenter = input.Xllcenter;
            output.Yllcenter = input.Yllcenter;
            output.NoDataValue = -1;

            //Loop throug every cell
            int max = (output.Bounds.NumRows + 1);
            for (int i = 0; i < output.NumRows; i++)
            {
                for (int j = 0; j < output.NumColumns; j++)
                {
                    if (input.Value[i, j] == input.NoDataValue)
                        output.Value[i,j] = -1;
                    else if (input.Value[i, j] >= threshold)
                        output.Value[i,j] = 1;
                    else
                        output.Value[i,j] = 0;
                    
                    if (cancelProgressHandler.Cancel == true)
                        return false;
                }

                //only update when increment in persentage
                current = Convert.ToInt32(Math.Round(i * 100D / max));
                if (current > previous)
                    cancelProgressHandler.Progress("", current, current.ToString() + "% progress completed");
                previous = current;
            }

            output.Save();
            return true;
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
            get { return ("This tool creates a raster mask based on a threshold value. If the cell value is greater than or equal to the threshold it is assigned 1 if it is above the threshold it is assigned 0, if it is a NoDataValue it is assinged a new NoDataValue of -1"); }
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
        /// 16x16 Bitmap - The small icon that appears in the Tool Manager Tree
        /// </summary>
        public System.Drawing.Bitmap IconSmall
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
            _inputParam[0] = new RasterParam("input Raster");
            _inputParam[0].HelpText = "Input to create the boolean mask from.";
            _inputParam[1] = new DoubleParam("Threshold value");
            _inputParam[1].HelpText = "The threshold value to use to create the raster mask.";
                       
            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam("Raster Mask");
            _outputParam[0].HelpText = "The mask raster created.";
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
            get { return ("Raster Threshold"); }
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
            //This will Diplay NoDataValue(Already exisit) in the Optional input box;
            if (sender == _inputParam[0])
            {
                try
                {
                    if ((_inputParam[0] as RasterParam).Value != null)
                    {
                        (_inputParam[1] as DoubleParam).Min = (_inputParam[0] as RasterParam).Value.Minimum;
                        (_inputParam[1] as DoubleParam).Max = (_inputParam[0] as RasterParam).Value.Maximum;
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return ("Creates raster mask"); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return ("MapWindow Raster Threshold"); }
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
