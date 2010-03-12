//********************************************************************************************************
// Product Name: MapWindow.Tools.IntParam
// Description:  Int Parameters returned by an ITool allows the tool to specify a range and default value
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Tools.Param
{
    /// <summary>
    /// Int Parameters returned by an ITool allows the tool to specify a range and default value
    /// </summary>
    public class IntParam : Parameter
    {

        #region variables
        private int _min = int.MinValue;
        private int _max = int.MaxValue;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new integer parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public IntParam( string name)
        {
            this.Name = name;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow Int Param";
            this.DefaultSpecified = false;
        }

        /// <summary>
        /// Creates a new integer parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        public IntParam(string name,int value)
        {
            this.Name = name;
            this.Value = value;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow Int Param";
            this.DefaultSpecified = true;
        }

        /// <summary>
        /// Creates a new integer parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public IntParam(string name, int value, int min, int max)
        {
            this.Name = name;
            this.Max = max;
            this.Min = min;
            this.Value = value;
            this.ParamVisible = ShowParamInModel.No;
            this.ParamType = "MapWindow Int Param";
            this.DefaultSpecified = true;
        }
        #endregion

        #region properties

        /// <summary>
        /// The minimum range for the parameter Default: -2,147,483,648
        /// </summary>
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// The maximum range for the paramater Default: 2,147,483,648
        /// </summary>
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Specifies the value to use by default must be between the min and max
        /// </summary>
        public new int Value
        {
            get 
            {
                if (this.DefaultSpecified) return (int)base.Value;
                else return 0;
            }
            set 
            { 
                base.Value = value;
                this.DefaultSpecified = true;
            }
        }

        #endregion

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> DataSets)
        {
            return (new IntElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> DataSets)
        {
            return (new IntElement(this));

        }
    }
}
