//********************************************************************************************************
// Product Name: MapWindow.Tools.Buffer
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
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna. Created in 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for Buffer
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using MapWindow.Geometries;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    public  class Buffer : ITool
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
            get { return (TextStrings.Analysis); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.BufferDescription); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            DoubleParam dp = _inputParam[1] as DoubleParam;
            double bufferDistance = 1;
            if(dp != null)bufferDistance = dp.Value;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, bufferDistance, output, cancelProgressHandler);
        }
        /// <summary>
        /// Executes the Buffer tool programaticaly
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="bufferDistance">The included radius from current features to use when Buffering</param>
        /// <param name="output">The output polygon feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        /// 
        //Ping Yang Added it for external testing 01/10
        public bool Execute(IFeatureSet input, double bufferDistance, IFeatureSet output)
        {
            //Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            if (input.FeatureType == FeatureTypes.Point)
            {
                return GetBuffer(input, bufferDistance, output);
            }
            if (input.FeatureType == FeatureTypes.MultiPoint)
            {
                return GetBuffer(input, bufferDistance, output);
            }
            if (input.FeatureType == FeatureTypes.Line)
            {
                return GetBuffer(input, bufferDistance, output);
            }
            if (input.FeatureType == FeatureTypes.Polygon)
            {
                return GetBuffer(input, bufferDistance, output);
            }
            return false;
        }

        //Ping Yang Added it for external testing 01/10
        public bool GetBuffer(IFeatureSet input, double bufferDistance, IFeatureSet output)
        {
            int previous = 0;
            int maxNo = input.Features.Count;
            for (int i = 0; i < maxNo; i++)
            {
                input.Features[i].Buffer(bufferDistance, output);

                //Here we update the progress
                int current = Convert.ToInt32(i * 100 / maxNo);
                if (current > previous)
                {
                    previous = current;
                }
            }

            output.Save();
            return true;
        }

        public bool Execute(IFeatureSet input, double bufferDistance, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            if (input.FeatureType == FeatureTypes.Point)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }
            if (input.FeatureType == FeatureTypes.MultiPoint)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }
            if (input.FeatureType == FeatureTypes.Line)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }
            if (input.FeatureType == FeatureTypes.Polygon)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }
            return false;
        }


        public static bool GetBuffer(IFeatureSet input, double bufferDistance, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            int previous = 0;
            int maxNo = input.Features.Count;
            for (int i = 0; i < maxNo; i++)
            {
                input.Features[i].Buffer(bufferDistance, output);

                //Here we update the progress
                int current = Convert.ToInt32(i * 100 / maxNo);
                if (current > previous)
                {
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                    previous = current;
                }

                if (cancelProgressHandler.Cancel)
                    return false;
            }

            output.Save();
            return true;
        }


        /// <summary>
        /// Image displayed in the help area when no input field is selected
        /// </summary>
        public System.Drawing.Bitmap HelpImage
        {
            get { return (null); }
        }

        /// <summary>
        /// Help text to be displayed when no input field is selected
        /// </summary>
        public string HelpText
        {
            get
            {
                return (TextStrings.BufferDescription);
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
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.BufferDistance, 10.0);
            //_inputParam[1].Value = 10.0;

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.OutputPolygonFeatureSet);
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
            get { return (TextStrings.Buffer); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public Parameter[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate other Parameter default values.
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
            get { return (TextStrings.Bufferwithdistance); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowBuffer); }
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
