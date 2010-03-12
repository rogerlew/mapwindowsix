using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using MapWindow.Geometries;
using MapWindow.Tools;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindowTools
{
    /// <summary>
    /// Union the features
    /// </summary>

    class mwAggregate:MapWindow.Tools.ITool
    {
        private string _workingPath;
        private Parameters[] _inputParam;
        private Parameters[] _outputParam;

        #region ITool Members

        // <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return ("Geometry"); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return ("Given a featureset, this will combine all of the features into a single shape.  Polygons that touch will lose the separation boundaries."); }
        }

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(MapWindow.Tools.ICancelProgressHandler cancelProgressHandler)
        {
            MapWindow.Data.IFeatureSet self=_inputParam[0].Value as MapWindow.Data.IFeatureSet;
            //self.FillAttributes();
            MapWindow.Data.IFeatureSet output = _outputParam[0].Value as MapWindow.Data.IFeatureSet;

            return Execute(self, output, cancelProgressHandler);
            
        }

        /// <summary>
        /// Executes the Union Opaeration tool programaticaly
        /// </summary>
        /// <param name="input">The input are feature sets</param>
        /// <param name="output">The output feature set</param>
        /// <param name="CancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet self, IFeatureSet output, MapWindow.Tools.ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (self == null || self.Features == null || self.Features.Count == 0 || output == null)
            {
                return false;
            }

            IFeature result = self.Features[0];
            MapWindow.Main.ProgressMeter pm = new MapWindow.Main.ProgressMeter(cancelProgressHandler, "Unioning Shapes", self.Features.Count);
            for(int i = 1; i < self.Features.Count; i++)
            {
                if (self.Features[i] == null) continue;
                result = result.Union(self.Features[i]);
                pm.CurrentValue = i;
            }
            pm.Reset();
            output.Features.Add(result);
            output.SaveAs(output.Filename, true);
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
                return ("This tool combines many different polygons into a single, all encompassing shape, though the shape may have mutliple parts.");
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
        public System.Drawing.Bitmap IconBig
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
        public void Init()
        {
            _inputParam = new Parameters[2];
            _inputParam[0] = new FeatureSetParam("Base Feature Set");

            _outputParam = new Parameters[1];
            _outputParam[0]=new FeatureSetParam("Union Feature Set");
            
        }

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public MapWindow.Tools.Param.Parameters[] InputParameters
        {
            get { return _inputParam; }
        }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        public string Name
        {
            get { return ("Aggregate"); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public MapWindow.Tools.Param.Parameters[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate other parameters default values.
        /// </summary>
        void ITool.ParameterChanged(Parameters sender)
        {
            return;
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return ("Aggregate features combines all the features in the featureset to form a single feature."); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return ("MapWindow Aggregate"); }
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
