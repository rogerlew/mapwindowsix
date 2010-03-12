//********************************************************************************************************
// Product Name: MapWindow.Tools.mwNoDataValueChange
// Description:  Change the No Data Values
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
// KP                     |  9/2009                |  Used IDW as model for NoDataValueChange
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindow.Tools
{
    
    public class NoDataValueChange:ITool
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
            get { return (TextStrings.RasterOverlay); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.NoDataValueChangeDescription); }//NoDataValueChangeDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            double value1 = Convert.ToDouble(_inputParam[1].Value);
            
            double Value2 = Convert.ToDouble(_inputParam[2].Value);

            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, value1, Value2,output, cancelProgressHandler);

        }
        /// <summary>
        /// Executes the Erase Opaeration tool programaticaly
        /// Ping Yang deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input">The input raster</param>
        /// <param name="oldValue">The original double value representing no-data</param>
        /// <param name="newValue">The new double value representing no-data</param>
        /// <param name="output">The output raster</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IRaster input, double oldValue, double newValue,  IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input == null || newValue==0 || output == null)
            {
                return false;
            }

            IEnvelope envelope = input.Bounds.Envelope;

            int noOfCol=input.NumColumns;
            int noOfRow=input.NumRows;
            int previous = 0;


            //create output raster
            output = Raster.Create(output.Filename, "", noOfCol, noOfRow, 1, typeof(int), new[] { "" });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input.NoDataValue;

            //Loop throug every cell
            int max = (output.Bounds.NumRows + 1);
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                        if (input.Value[i, j] == oldValue)
                            output.Value[i, j] = newValue;
                        else
                            output.Value[i, j] = input.Value[i, j];
                    
                    if (cancelProgressHandler.Cancel)
                        return false;
                }
                int current = Convert.ToInt32(Math.Round(i * 100D / max));
                //only update when increment in persentage
                if (current > previous)
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                previous = current;
            }

           // output = Temp;
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
            get
            {
                return (TextStrings.NoDataValueChangeDescription);
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
            _inputParam = new Parameter[3];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster);
            _inputParam[0].HelpText = TextStrings.InputRasternoValue;
            _inputParam[1] = new StringParam(TextStrings.Optional);
            _inputParam[1].HelpText = TextStrings.Optionaltochange;
            _inputParam[2]=new StringParam(TextStrings.UserNewValues);
            _inputParam[2].HelpText = TextStrings.UserinputNewValue;
                       
        
            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster);
            _outputParam[0].HelpText = TextStrings.newrastername;
            

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
            get { return (TextStrings.NoDataValueChangeDescription); }
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
            //This will Diplay NoDataValue(Already exisit) in the Optional input box;
            if (sender != _inputParam[0]) return;
            IRaster inputTemp = _inputParam[0].Value as IRaster;
            StringParam string1 = (_inputParam[1] as StringParam);
            if (string1 != null && inputTemp != null) string1.Value = inputTemp.NoDataValue.ToString();
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return (TextStrings.NoDataValueChangeDescription); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowChangeNoDataValues); }
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
