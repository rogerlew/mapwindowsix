//********************************************************************************************************
// Product Name: MapWindow.Tools.mwVoronoi
// Description:  computes voronoi polygons around each of the points, 
//               defining the regions that are closer to that point than any other points
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
// The Initializeializeial Developer of this Original Code is Ted Dunsford. Created in 8/26/2009
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name                   |   Date                 |   Comments
//------------------------|------------------------|---------------------------------
// KP                     |  9/2009                |  Used IDW as model for Voronoi
// Ping  Yang             |  12/2009               |  Cleaning code and fixing bugs.
//********************************************************************************************************

using System;
using MapWindow.Data;
using MapWindow.Tools.Param;
namespace MapWindow.Tools
{
    /// <summary>
    /// Line-simplification using Douglas-Peucker algorithm
    /// </summary>
    public class Voronoi : ITool
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
        { get { return (TextStrings.MapWindowVoronoi); } }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        string ITool.Name
        {   get { return (TextStrings.ThiessenPolygons); }}

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        void ITool.Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new PointFeatureSetParam(TextStrings.PointFeatureSet);

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.PolygonFeatureSet);
          
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        string ITool.Description
        {
            get { return (TextStrings.VoronoiDescription); }//VoronoiDescription
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        string ITool.ToolTip
        {
            get{return(TextStrings.CreateVoronoiPolygons);}
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        string ITool.Category
        {
            get{return(TextStrings.Analysis);}
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
        bool ITool.Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            if (input != null) input.FillAttributes();
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, output, cancelProgressHandler);
        }

        /// <summary>
        /// computes voronoi polygons around each of the points,
        /// defining the regions that are closer to that point than any other points
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="output">The output polygon feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input,  IFeatureSet output, 
            ICancelProgressHandler cancelProgressHandler)
        {
            Analysis.Voronoi.VoronoiPolygons(input, output, true, cancelProgressHandler);
            output.Save();
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
            get { return (TextStrings.computethevoronoidiagram); }
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
