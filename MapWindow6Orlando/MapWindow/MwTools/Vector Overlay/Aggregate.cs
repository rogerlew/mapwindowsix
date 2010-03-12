//********************************************************************************************************
// Product Name: MapWindow.Tools.mwAggregate
// Description:  Tool for aggregating all polygons into a single polygon.
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
// -----------------------|------------------------|--------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for Aggregate
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    /// <summary>
    /// Union all of the features from one polygon feature set into a single polygon.
    /// </summary>
    //class mwAggregate:ITool
    public class Aggregate:ITool//Ping Yang changed it for Testing project
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
            get { return (new Version(0, 0, 0, 2)); }
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
            get { return (TextStrings.Givenafeatureset); }
        }

        /// <summary>
        /// Once the Parameters have been configured, the Execute command can be called, it returns true if succesful.
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet self=_inputParam[0].Value as IFeatureSet;
            //self.FillAttributes();
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;
                       
            return Execute(self, output, cancelProgressHandler);
            
        }

        /// <summary>
        /// Executes the Union Opaeration tool programaticaly
        /// </summary>
        /// <param name="self">The input are feature sets</param>
        /// <param name="output">The output feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
       public bool Execute(IFeatureSet SourceData, IFeatureSet ResultData, ICancelProgressHandler cancelProgressHandler)
           //removed "static" dpa 12/2009 so that this can be run from an external call directly.
         {
            //Validates the input and output data

             if (SourceData == null || SourceData.Features == null || SourceData.Features.Count == 0 || ResultData == null)
            {
                return false;
            }

           IFeature OneFeature = SourceData.Features[0];
           // MapWindow.Main.ProgressMeter pm = new MapWindow.Main.ProgressMeter(cancelProgressHandler, TextStrings.UnioningShapes, self.Features.Count);
             for (int i = 1; i < SourceData.Features.Count; i++)
            {
                if (SourceData.Features[i] == null) continue;
                OneFeature = OneFeature.Union(SourceData.Features[i]);
           //     pm.CurrentValue = i;
            }
           // pm.Reset();
            ResultData.Features.Add(OneFeature);
            ResultData.SaveAs(ResultData.Filename, true);
            return true;          
        }

       //public bool Execute(IFeatureSet SourceData, IFeatureSet ResultData)
       ////Ping Yang removed the progress handler for testing
       //{
       //    //Validates the input and output data

       //    if (SourceData == null || SourceData.Features == null || SourceData.Features.Count == 0 || ResultData == null)
       //    {
       //        return false;
       //    }

       //    IFeature OneFeature = SourceData.Features[0];
       //    for (int i = 1; i < SourceData.Features.Count; i++)
       //    {
       //        if (SourceData.Features[i] == null) continue;
       //        OneFeature = OneFeature.Union(SourceData.Features[i]);
       //        //     pm.CurrentValue = i;
       //    }
       //    // pm.Reset();
       //    ResultData.Features.Add(OneFeature);
       //    ResultData.SaveAs(ResultData.Filename, true);
       //    return true;
       //}

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
                return (TextStrings.toolcombinespolygons);
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
            _inputParam = new Parameter[1];
            _inputParam[0] = new PolygonFeatureSetParam(TextStrings.BaseFeatureSet);

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.UnionFeatureSet);
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
            get { return (TextStrings.Aggregate); }
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
            get { return (TextStrings.AggregateToollip); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowAggregate); }
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
