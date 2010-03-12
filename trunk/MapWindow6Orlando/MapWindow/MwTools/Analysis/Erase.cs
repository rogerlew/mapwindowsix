//********************************************************************************************************
// Product Name: MapWindow.Tools.Erase
// Description:  Eliminate Second featureset from first featureset
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
// KP                 |  9/2009            |  Used IDW as model for Erase
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    public class Erase : ITool
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
            get { return (TextStrings.RraseDescription); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet self = _inputParam[0].Value as IFeatureSet;
            if (self != null) self.FillAttributes();
            IFeatureSet other = _inputParam[1].Value as IFeatureSet;
            if (other != null) other.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(self, other, output, cancelProgressHandler);
         
        }
        /// <summary>
        /// Executes the Erase Opaeration tool programaticaly
        /// </summary>
        /// <param name="self">The input feature that is to be erased</param>
        /// <param name="other">The other feature defining the area to remove</param>
        /// <param name="output">The resulting erased content</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns>Boolean, true if the operation was a success</returns>
        public static bool Execute(IFeatureSet self, IFeatureSet other, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (self == null || other == null || output == null)
            {
                return false;
            }

            int previous;
            int max = self.Features.Count * other.Features.Count;

            output.CopyTableSchema(self);//Fill the 1st Featureset fields
            IFeatureSet tempSet = self.CombinedFields(other);
            //go through every feature in 1st featureSet
            for (int i = 0; i < self.Features.Count; i++)
            {
                //go through every feature in 2nd featureSet
                for (int j = 0; j < other.Features.Count; j++)
                {
                    self.Features[i].Difference(other.Features[j], tempSet, FieldJoinType.All);
                    previous = Convert.ToInt32(Math.Round((i * j * 50D / max)));
                    if (cancelProgressHandler.Cancel)
                        return false;
                    cancelProgressHandler.Progress("", previous, previous + TextStrings.progresscompleted);
                }
            }
            //Add to the Output Feature Set
            for (int a = 0; a < tempSet.Features.Count; a++)
            {
                output.Features.Add(tempSet.Features[a]);
                previous = Convert.ToInt32(Math.Round((a * 50D / tempSet.Features.Count) + 50D));
                if (cancelProgressHandler.Cancel)
                    return false;
                cancelProgressHandler.Progress("", previous, previous + TextStrings.progresscompleted);

            }

            output.SaveAs(output.Filename, true);
            //add to map?
            return true;
        }
        /// <summary>
        /// Ping Yang Overwrite the function for Executes the Erase Opaeration tool for external testing
        /// </summary>
        public bool Execute(IFeatureSet self, IFeatureSet other, IFeatureSet output)
        {
            if (self == null || other == null || output == null)
            {
                return false;
            }

            int previous;
            int max = self.Features.Count * other.Features.Count;

            output.CopyTableSchema(self);//Fill the 1st Featureset fields
            IFeatureSet tempSet = self.CombinedFields(other);
            //go through every feature in 1st featureSet
            for (int i = 0; i < self.Features.Count; i++)
            {
                //go through every feature in 2nd featureSet
                for (int j = 0; j < other.Features.Count; j++)
                {
                    self.Features[i].Difference(other.Features[j], tempSet, FieldJoinType.All);
                    previous = Convert.ToInt32(Math.Round((i * j * 50D / max)));
                  }
            }
            //Add to the Output Feature Set
            for (int a = 0; a < tempSet.Features.Count; a++)
            {
                output.Features.Add(tempSet.Features[a]);
                previous = Convert.ToInt32(Math.Round((a * 50D / tempSet.Features.Count) + 50D));
            }

            output.SaveAs(output.Filename, true);
            //add to map?
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
                return (TextStrings.EliminateSecondFeatureset);
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
            _inputParam[1] = new FeatureSetParam(TextStrings.RemoveFeatureSet);
            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.ErasedResultFeatureSet);
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
            get { return (TextStrings.Erase); }
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
            get { return (TextStrings.Erase2ndinput); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowErase); }
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
