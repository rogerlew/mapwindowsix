//********************************************************************************************************
// Product Name: MapWindow.Tools.mwDPSimplification
// Description:  This tool reduces the number of points on polylines using the Douglas-Peucker line
//               simplification algorithm
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
// The Initializeializeial Developer of this Original Code is Jiri Kadlec. Created in Feb, 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name               |   Date             |         Comments
//
//--------------------|--------------------|--------------------------------------------------------
// Brian Marchionni   |  04/30/2009        |  Cleaned it up
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for DPSimplification
// Ping Yang          |  12/2009           |  Cleaning code and fixing bugs. 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using MapWindow.Geometries;
using MapWindow.Data;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    /// <summary>
    /// Line-simplification using Douglas-Peucker algorithm
    /// </summary>
    public class DpSimplification : ITool
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
        {   get { return (TextStrings.MapWindowLineSimplification); }}

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        string ITool.Name
        {   get { return (TextStrings.SimplifyLines); }}

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        void ITool.Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new LineFeatureSetParam(TextStrings.LineFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.Tolerance);
            _inputParam[1].Value = 10.0;

            _outputParam = new Parameter[1];
            _outputParam[0] = new LineFeatureSetParam(TextStrings.LineFeatureSet);
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        string ITool.Description
        {
            get { return (TextStrings.DouglasPeuckerlinesimplification); }
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        string ITool.ToolTip
        {
            get { return (TextStrings.DPlinesimplification); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        string ITool.Category
        {
            get{return(TextStrings.Generalization);}
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
            double tolerance = (double) _inputParam[1].Value;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, tolerance, output, CancelProgressHandler);
        }
        /// <summary>
        /// Executes the DP line simplefy tool programaticaly
        /// Ping Yang Added it for external Testing
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="tolerance">The tolerance to use when simplefiying</param>
        /// <param name="output">The output polygon feature set</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input, double tolerance, IFeatureSet output)
        {
            //Validates the input and output data
            if (input == null || output == null)
                return false;

            //We copy all the fields 
            foreach (DataColumn inputColumn in input.DataTable.Columns)
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));

            int numTotalOldPoints = 0;
            int numTotalNewPoints = 0;

            for (int j = 0; j < input.Features.Count; j++)
            {
                int numOldPoints = 0;
                int numNewPoints = 0;


                Geometry geom = input.Features[j].BasicGeometry as Geometry;
                if (geom != null) numOldPoints = geom.NumPoints;
                numTotalOldPoints += numOldPoints;
                if (geom != null)
                {
                    for (int part = 0; part < geom.NumGeometries; part++)
                    {
                        Geometry geomPart = (Geometry)geom.GetGeometryN(part);
                        //do the simplification
                        IList<Coordinate> oldCoords = geomPart.Coordinates;
                        IList<Coordinate> newCoords =
                            MapWindow.Analysis.Topology.Simplify.DouglasPeuckerLineSimplifier.Simplify(oldCoords, tolerance);

                        //convert the coordinates back to a geometry
                        Geometry newGeom = new LineString(newCoords);
                        numNewPoints += newGeom.NumPoints;
                        numTotalNewPoints += numNewPoints;
                        Feature newFeature = new Feature(newGeom, output);
                        foreach (DataColumn colSource in input.DataTable.Columns)
                            newFeature.DataRow[colSource.ColumnName] = input.Features[j].DataRow[colSource.ColumnName];
                    }
                }
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// Executes the DP line simplefy tool programaticaly
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="tolerance">The tolerance to use when simplefiying</param>
        /// <param name="output">The output polygon feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input, double tolerance, IFeatureSet output, 
            ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input == null || output == null)
                return false;

            //We copy all the fields 
            foreach (DataColumn inputColumn in input.DataTable.Columns)
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));

            int numTotalOldPoints = 0;
                int numTotalNewPoints = 0;

            for (int j = 0; j < input.Features.Count; j++)
            {
                int numOldPoints = 0;
                int numNewPoints = 0;
                
                
                Geometry geom = input.Features[j].BasicGeometry as Geometry;
                if (geom != null) numOldPoints = geom.NumPoints;
                numTotalOldPoints += numOldPoints;
                if (geom != null)
                {
                    for (int part = 0; part < geom.NumGeometries; part++)
                    {
                        Geometry geomPart = (Geometry)geom.GetGeometryN(part);
                        //do the simplification
                        IList<Coordinate> oldCoords = geomPart.Coordinates;
                        IList<Coordinate> newCoords = 
                            MapWindow.Analysis.Topology.Simplify.DouglasPeuckerLineSimplifier.Simplify(oldCoords, tolerance);
                   
                        //convert the coordinates back to a geometry
                        Geometry newGeom = new LineString(newCoords);
                        numNewPoints += newGeom.NumPoints;
                        numTotalNewPoints += numNewPoints;
                        Feature newFeature = new Feature(newGeom, output);
                        foreach (DataColumn colSource in input.DataTable.Columns)
                            newFeature.DataRow[colSource.ColumnName] = input.Features[j].DataRow[colSource.ColumnName];
                    }
                }

                //Status updates is done here, shows number of old / new points
                cancelProgressHandler.Progress("", Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(input.Features.Count)) * 100), 
                    numOldPoints + "-->" + numNewPoints);
                if (cancelProgressHandler.Cancel)
                    return false;
            }
            cancelProgressHandler.Progress("", 100, TextStrings.Originalnumberofpoints + numTotalOldPoints + " " + TextStrings.Newnumberofpoints + numTotalNewPoints);

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
            get { return (TextStrings.DouglasPeuckerlinesimplification); }
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
