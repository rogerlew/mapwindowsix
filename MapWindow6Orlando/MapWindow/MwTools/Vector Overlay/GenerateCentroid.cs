//********************************************************************************************************
// Product Name: MapWindow.Tools.mwGenerateCentroid
// Description:  Generate centroid FeatureSet from input FeatureSet
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
// Ted Dunsford           |  9/17/2009             |  Copy attributes from the original featureset. 
// KP                     |  9/2009                |  Used IDW as model for GenerateCentroid
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindow.Tools
{
    public class GenerateCentroid:ITool
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
            get { return (TextStrings.VectorOverlay); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.GenerateCentroidDescription); }
        }

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            if (input1 != null) input1.FillAttributes();


            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the generate centroid FeatureSet Opaeration tool programaticaly.
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input1, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input1 == null || output == null)
            {
                return false;
            }
            bool multiPoint=false;
            foreach(IFeature f1 in input1.Features)
            {
                if(f1.NumGeometries>1)
                    multiPoint=true;
            }

            if (multiPoint == false)
                output.FeatureType = FeatureTypes.Point;
            else
                output.FeatureType = FeatureTypes.MultiPoint;

            int previous = 0;
            int i = 0;
            int maxFeature = input1.Features.Count;
            output.CopyTableSchema(input1);
            foreach (IFeature f in input1.Features)
            {
                if (cancelProgressHandler.Cancel)
                    return false;

                IFeature fnew = new Feature(f.Centroid());
                
                //Add the centroid to output
                output.Features.Add(fnew);

                fnew.CopyAttributes(f);

                int current = Convert.ToInt32(Math.Round(i * 100D / maxFeature));
                //only update when increment in percentage
                if (current > previous)
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                previous = current;
                i++;
            }
            output.SaveAs(output.Filename, true);
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
                return (TextStrings.generatecentroidFeatureSet);
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
        /// The parameters array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet);
            _inputParam[0].HelpText = TextStrings.InputFeatureSettogenerate;

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet);
            _outputParam[0].HelpText = TextStrings.SelectResultFeatureSetDirectory;
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
            get { return (TextStrings.GenerateCentroid); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public Parameter[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate input2 parameters default values.
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
            get { return (TextStrings.GenerateCentroidfrominputFeatureSet); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowGenerateCentroid); }
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
