//********************************************************************************************************
// Product Name: MapWindow.Tools.mwFeatureToRaster
// Description:  Generate a new raster from given Polygon.
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
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna with guidence of MapWinGeoProc. 
//Created in 2009.(not fully function exception: open polygon in the boundary.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Ted Dunsford built the genuine functionality to work with the newly created Analysis methods 
// -----------------------|------------------------|---------------------------------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  built the genuine functionality to work with the newly created Analysis methods
// KP                     |  9/2009                |  Used IDW as model for FeatureToRaster
// Ping  Yang             |  12/2009               |  Cleaning code and fixing bugs.
//***********************************************************************************************************************************
using System.Collections.Generic;
using System;
using System.Data;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    public class FeatureToRaster:ITool
    {
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;
        private string _workingPath;

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
            get { return (TextStrings.Conversion); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.FeatureToRasterDescription); }//FeatureToRasterDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet poly = _inputParam[0].Value as IFeatureSet;
            double cellSize = (double)_inputParam[2].Value;
            if (poly == null) return false;
            int indx = (int)_inputParam[1].Value - 1;
            string field = indx < 0 ? "FID" : poly.DataTable.Columns[indx].ColumnName;
            
            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(poly, cellSize, field, output, cancelProgressHandler);
            
        }
        /// <summary>
        /// Generates a new raster given the specified featureset.  Values will be given to
        /// each cell that coincide with the values in the specified field of the attribute
        /// table.  If the cellSize is 0, then it will be automatically calculated so that
        /// the smaller dimension (between width and height) is 256 cells.
        /// Ping Yang delete static for external testing 01/2010
        /// </summary>
        /// <param name="source">The featureset to convert into a vector format</param>
        /// <param name="cellSize">A double giving the geographic cell size.</param>
        /// <param name="fieldName">The string fieldName to use</param>
        /// <param name="output">The raster that will be created</param>
        /// <param name="cancelProgressHandler">A progress handler for handling progress messages</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet source, double cellSize, string fieldName, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (source == null || output == null)
            {
                return false;
            }

            output = MapWindow.Analysis.VectorToRaster.ToRaster(source, ref cellSize, fieldName, output.Filename, "", new string[]{}, cancelProgressHandler);
            output.Save();
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
                return (TextStrings.GenerateRasterfromPolygon);
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
            _inputParam = new Parameter[4];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1polygontoRaster);
            _inputParam[0].HelpText = TextStrings.InputPolygontochange;
            _inputParam[2] = new DoubleParam(TextStrings.DesiredCellSize);
            _inputParam[2].HelpText = TextStrings.Themaximumnumber;
            _inputParam[1] = new ListParam(TextStrings.stringnameoffield);
            _inputParam[1].HelpText = TextStrings.Thevalueofeachcell;
            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster);
            _outputParam[0].HelpText = TextStrings.Resultofaverageslope;
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
            get { return (TextStrings.FeatureToRaster); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public Parameter[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        void ITool.ParameterChanged(Parameter sender)
        {
            if (sender != _inputParam[0]) return;
            List<string> fields = new List<string>();
            IFeatureSet fs = _inputParam[0].Value as IFeatureSet;
            if (fs == null) return;
            DataTable dt = fs.DataTable;
            if (dt == null) return;
            fields.Add("FID [Integer]");
            foreach (DataColumn column in dt.Columns)
            {
                fields.Add(column.ColumnName + " [" + column.DataType.Name + "]");
            }
            ListParam lp = _inputParam[1] as ListParam;
            if(lp == null) return;
            lp.ValueList = fields;
            lp.Value = 0;
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return (TextStrings.newrasteronspecifiedfeatureset); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowFeatureToRaster); }
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
