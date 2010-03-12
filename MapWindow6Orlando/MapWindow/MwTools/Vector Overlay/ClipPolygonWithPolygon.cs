//********************************************************************************************************
// Product Name: MapWindow.Tools.mwClipPolygonWithPolygon
// Description:  Clip Polygon with another Polygon
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
//------------------------------------------------------------------------------------------------------
// KP                     |  9/2009                |  Used IDW as model for ClipPolygonWithPolygon
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************

using System;
using System.Data;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    public class ClipPolygonWithPolygon : ITool
    {
        private string _workingPath;
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

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
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        string ITool.UniqueName
        {   get { return (TextStrings.MapWindowClip); }}

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        string ITool.Name
        {   get { return (TextStrings.ClipFeatureSetWithPolygon); }}

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        void ITool.Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.Featuresettoclip);
            _inputParam[1] = new PolygonFeatureSetParam(TextStrings.Clipbounds);

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.Clippedfeatureset);
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        string ITool.Description
        {
            get { return (TextStrings.tooltakesafeatureset); }
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        string ITool.ToolTip
        {
            get{return(TextStrings.Clipslayerwithlayer);}
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        string ITool.Category
        {
            get{return(TextStrings.VectorOverlay);}
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate other Parameter default values.
        /// </summary>
        void ITool.ParameterChanged(Parameter sender)
        {
            return;
        }

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        Parameter[] ITool.InputParameters
        {
            get{return(_inputParam);}
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        Parameter[] ITool.OutputParameters
        {
            get{return(_outputParam);}
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        bool ITool.Execute(ICancelProgressHandler CancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            if (input != null) input.FillAttributes();
            IFeatureSet input2 = _inputParam[1].Value as IFeatureSet;
            if (input2 != null) input2.FillAttributes();
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, input2, output, CancelProgressHandler);
        }

        /// <summary>
        /// Executes the ClipPolygonWithPolygon tool with programatic input
        /// </summary>
        /// <param name="input">The input feature set to clip</param>
        /// <param name="input2">The input polygon feature set to clip with</param>
        /// <param name="output">The output feature set</param>
        /// <param name="cancelProgressHandler">The progress handler for progress message updates</param>
        /// <returns></returns>
        /// Ping delete "static" for external testing
        public bool Execute(IFeatureSet input, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input == null || input2 == null || output == null)
            {
                cancelProgressHandler.Progress("", 100, TextStrings.Oneparameterinnull);
                return false;
            }
            if (input2.FeatureType != MapWindow.Geometries.FeatureTypes.Polygon)
            {
                cancelProgressHandler.Progress("", 100, TextStrings.secondinputlayer);
                return false;
            }
            output.FeatureType = input.FeatureType;

            //we add all the old features to output
            IFeatureSet tempoutput = input.Intersection(input2, FieldJoinType.LocalOnly, cancelProgressHandler);
            
            //We add all the fields 
            foreach (DataColumn inputColumn in tempoutput.DataTable.Columns)
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));

            foreach (Feature fe in tempoutput.Features)
            {
                output.Features.Add(fe);
            }

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// 32x32 Bitmap - The Large icon that will appears in the Tool Dialog Next to the tools name
        /// </summary>
        System.Drawing.Bitmap ITool.Icon
        {
            get { return (null); }
        }

        /// <summary>
        /// Image displayed in the help area when no input field is selected
        /// </summary>
        System.Drawing.Bitmap ITool.HelpImage
        {
            get { return (null); }
        }

        /// <summary>
        /// Help text to be displayed when no input field is selected
        /// </summary>
        String ITool.HelpText
        {
            get { return (TextStrings.tooltakesafeatureset); }
        }

        /// <summary>
        /// This is set before the tool is executed to provide a folder where the tool can save temporary data
        /// </summary>
        string ITool.WorkingPath
        {
            set {_workingPath = value;}
        }

        /// <summary>
        /// Returns the address of the tools help web page in HTTP://... format. Return a empty string to hide the help hyperlink.
        /// </summary>
        string ITool.HelpURL
        {
            get { return ("HTTP://www.mapwindow.org"); }
        }
    }
}
