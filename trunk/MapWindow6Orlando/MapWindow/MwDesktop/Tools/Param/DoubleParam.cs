//********************************************************************************************************
// Product Name: MapWindow.Tools.DoubleParam
// Description:  Double Parameter returned by an ITool allows the tool to specify a range and default value
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
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
    /// Double Parameter returned by an ITool allows the tool to specify a range and default value
    /// </summary>
    public class DoubleParam : Parameter
    {

        #region variables
        private double _min = double.MinValue;
        private double _max = double.MaxValue;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new double parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public DoubleParam(string name)
        {
            this.Name = name;
            this.ParamType = "MapWindow Double Param";
            this.ParamVisible = ShowParamInModel.No;
            this.DefaultSpecified = false;
        }

        /// <summary>
        /// Creates a new double parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        public DoubleParam(string name, double value)
        {
            this.Name = name;
            this.Value = value;
            this.ParamType = "MapWindow Double Param";
            this.ParamVisible = ShowParamInModel.No;
            this.DefaultSpecified = true;
        }

        /// <summary>
        /// Creates a new double parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public DoubleParam(string name, double value, double min, double max)
        {
            this.Name = name;
            this.Max = max;
            this.Min = min;
            this.Value = value;
            this.ParamType = "MapWindow Double Param";
            this.ParamVisible = ShowParamInModel.No;
            this.DefaultSpecified = true;
        }
        #endregion

        #region properties

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new double Value
        {
            get
            {
                if (this.DefaultSpecified) return (double)base.Value;
                else return 0;
            }
            set
            {
                base.Value = (object)value;
                this.DefaultSpecified = true;
            }
        }

        /// <summary>
        /// The minimum range for the parameter Default: -1.79769313486232e308
        /// </summary>
        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// The maximum range for the paramater Default: 1.79769313486232e308
        /// </summary>
        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        #endregion

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> DataSets)
        {
            return (new DoubleElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="DataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> DataSets)
        {
            return (new DoubleElement(this));

        }
    }
}
