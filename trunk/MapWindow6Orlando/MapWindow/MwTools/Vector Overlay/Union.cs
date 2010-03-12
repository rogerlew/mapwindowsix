//********************************************************************************************************
// Product Name: MapWindow.Tools.Union
// Description:  Add all features of input with attributes
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
// KP                     |  9/2009                |  Used IDW as model for Union
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    /// <summary>
    /// Union the features
    /// </summary>

    public class Union:ITool
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

        // <summary>
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
            get { return (TextStrings.UnionDescription); }//UnionDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet self=_inputParam[0].Value as IFeatureSet;
            if (self != null) self.FillAttributes();
            IFeatureSet other = _inputParam[1].Value as IFeatureSet;
            if (other != null) other.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            if (self == null) return false;
            
            if (other == null) return false;
           
            return self.Features.Count < other.Features.Count ? Execute(self, other, output, cancelProgressHandler) : Execute(other, self, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Union Opaeration tool programaticaly
        /// </summary>
        /// <param name="self">The input are feature set</param>
        /// <param name="other">The second input feature set</param>
        /// <param name="output">The output feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet self, IFeatureSet other, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (self == null || other == null || output == null)
            {
                return false;
            }

            IFeatureSet tempOuput = self.Intersection(other, FieldJoinType.All, null);
            IFeatureSet TempFeatureSet = self.CombinedFields(other);
            int previous = 0;
            int max = self.Features.Count;
            //Take (Self-Intersect Featureset)
            List<IFeature> intersectList;
            for (int i =0 ; i< self.Features.Count ; i++)
            {
                intersectList = other.Select(self.Features[i].Envelope);
                foreach (IFeature feat in intersectList)
                {
                    if (cancelProgressHandler.Cancel)
                        return false;
                    self.Features[i].Difference(feat, TempFeatureSet, FieldJoinType.LocalOnly);
                }
                if (Math.Round(i * 40D / max) > previous)
                {
                    previous = Convert.ToInt32(Math.Round(i * 40D / max));
                    cancelProgressHandler.Progress("", previous, previous + TextStrings.progresscompleted);
                }
            }

            max = other.Features.Count;
            //Take (Other-Intersect Featureset)
            for (int i = 0; i < other.Features.Count; i++)
            {
                intersectList = self.Select(other.Features[i].Envelope);
                foreach (IFeature feat in intersectList)
                {
                    if (cancelProgressHandler.Cancel)
                        return false;
                    other.Features[i].Difference(feat, TempFeatureSet, FieldJoinType.LocalOnly);
                }
                if (Math.Round((i * 40D / max) + 40D ) > previous)
                {
                    previous = Convert.ToInt32(Math.Round((i * 40D / max) + 40D));
                    cancelProgressHandler.Progress("", previous, previous + TextStrings.progresscompleted);
                }
            }

            max = TempFeatureSet.Features.Count;
            output.CopyTableSchema(TempFeatureSet);
            //Add the individual feature to output
            for (int i = 0; i < TempFeatureSet.Features.Count; i++)
            {
                output.Features.Add(TempFeatureSet.Features[i]);
                if (Math.Round((i * 10D / max) + 80D) > previous)
                {
                    previous = Convert.ToInt32(Math.Round((i * 10D / max) + 80D));
                    if (cancelProgressHandler.Cancel)
                        return false;
                    cancelProgressHandler.Progress("", previous, previous + TextStrings.progresscompleted);
                }
            }

            max = tempOuput.Features.Count;
            //Add the Intersect feature to output
            for (int i = 0; i < tempOuput.Features.Count; i++)
            {
                output.Features.Add(tempOuput.Features[i]);
                if (cancelProgressHandler.Cancel)
                    return false;
                if (Math.Round((i * 10D / max) + 90D) > previous)
                {
                    previous = Convert.ToInt32(Math.Round((i * 10D / max) + 90D));
                    cancelProgressHandler.Progress("", previous, previous + TextStrings.progresscompleted);
                }
            }

            output.SaveAs(output.Filename, true);
            return true;
                
        }


        /// <summary>
        /// Image displayed in the help area when no input field is selected
        /// </summary>
        public System.Drawing.Bitmap HelpImage
        {
            get { return BitmapResources.Union; }
        }

        /// <summary>
        /// Help text to be displayed when no input field is selected
        /// </summary>
        public string HelpText
        {
            get
            {
                return (TextStrings.addallfeatures);
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
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.BaseFeatureSet);
            _inputParam[0].HelpText = TextStrings.MainFeatureset;
            _inputParam[1] = new FeatureSetParam(TextStrings.ChildFeatureSet);
            _inputParam[1].HelpText = TextStrings.SecondFeatureset;
            _outputParam = new Parameter[1];
            _outputParam[0]=new FeatureSetParam(TextStrings.UnionFeatureSet);    
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
            get { return (TextStrings.Union); }
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
            get { return (TextStrings.Unionofinputs); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowUnion); }
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
