//********************************************************************************************************
// Product Name: MapWindow.Tools.mwRasterToPolygon
// Description:  Converts a raster data set to a polygon feature set
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
// The Initial developer of this Original Code is Brian Marchionni. Created in Aug. 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Name               |   Date             |         Comments
//------------------------|------------------------|-------------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some unnecessary references using re-sharper
// KP                     |  9/2009                |  Used IDW as model for RasterToPolygon
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************

using System.Collections.Generic;
using System.Collections;
using System;
using System.Diagnostics;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindow.Tools
{
   public class RasterToPolygon:ITool
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
            get { return (TextStrings.Conversion); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.RasterToPolygonDescription); }//RasterToPolygonDescription
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input = _inputParam[0].Value as IRaster;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;
            return Execute(input, output, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons.
        /// Ping delete static for external testing
        /// </summary>
        /// <param name="input">The Polygon Raster(Grid file).</param>
        /// <param name="output">The Polygon shapefile path.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        public bool Execute(IRaster input, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            if ((input == null) || (output == null))
            {
                return false;
            }

            output.DataTable.Columns.Add("Value", typeof(double));

            Hashtable featureHash = new Hashtable();
            double previous = 0.0;
            int current;
            double height = input.CellHeight;
            double width = input.CellWidth;
            int numRows = input.NumRows;
            int numColumns = input.NumColumns;
            double xMin = input.Xllcenter - (input.CellWidth / 2.0);
            double yMin = input.Yllcenter - (input.CellHeight / 2.0);
            double xMax = xMin + (height * input.NumColumns);
            double yMax = yMin + (width * input.NumRows);
            for (int y = 0; y < numRows; y++)
            {
                current = Convert.ToInt32((y * 100.0) / input.NumRows);
                if (current > previous)
                {
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                    previous = current;
                }
                Debug.WriteLine("Row : " + y + " done.");
                for (int x = 0; x < numColumns; x++)
                {
                    double value = input.Value[y, x];
                    List<LineSegment> lineList = new List<LineSegment>();
                    if (!featureHash.Contains(value))
                    {
                        featureHash.Add(value, lineList);
                    }
                    else
                    {
                        lineList = featureHash[value] as List<LineSegment>;
                    }
                    if (y == 0)
                    {
                        lineList.Add(new LineSegment(new Coordinate((x * width) + xMin, yMax), new Coordinate(((x + 1) * width) + xMin, yMax)));
                        if (input.Value[y + 1, x] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate((x * width) + xMin, yMax - height), new Coordinate(((x + 1) * width) + xMin, yMax - height)));
                        }
                    }
                    else if (y == (numRows - 1))
                    {
                        lineList.Add(new LineSegment(new Coordinate((x * width) + xMin, yMin), new Coordinate(((x + 1) * width) + xMin, yMin)));
                        if (input.Value[y - 1, x] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate((x * width) + xMin, yMin + height), new Coordinate(((x + 1) * width) + xMin, yMin + height)));
                        }
                    }
                    else
                    {
                        if (input.Value[y + 1, x] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate((x * width) + xMin, yMax - ((y + 1) * height)), new Coordinate(((x + 1) * width) + xMin, yMax - ((y + 1) * height))));
                        }
                        if (input.Value[y - 1, x] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate((x * width) + xMin, yMax - (y * height)), new Coordinate(((x + 1) * width) + xMin, yMax - (y * height))));
                        }
                    }
                    if (x == 0)
                    {
                        lineList.Add(new LineSegment(new Coordinate(xMin, yMax - (y * height)), new Coordinate(xMin, yMax - ((y + 1) * height))));
                        if (input.Value[y, x + 1] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate(xMin + width, yMax - (y * height)), new Coordinate(xMin + width, yMax - ((y + 1) * height))));
                        }
                    }
                    else if (x == (numColumns - 1))
                    {
                        lineList.Add(new LineSegment(new Coordinate(xMax, yMax - (y * height)), new Coordinate(xMax, yMax - ((y + 1) * height))));
                        if (input.Value[y, x - 1] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate(xMax - width, yMax - (y * height)), new Coordinate(xMax - width, yMax - ((y + 1) * height))));
                        }
                    }
                    else
                    {
                        if (input.Value[y, x + 1] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate(xMin + ((x + 1) * width), yMax - (y * height)), new Coordinate(xMin + ((x + 1) * width), yMax - ((y + 1) * height))));
                        }
                        if (input.Value[y, x - 1] != value)
                        {
                            lineList.Add(new LineSegment(new Coordinate(xMin + (x * width), yMax - (y * height)), new Coordinate(xMin + (x * width), yMax - ((y + 1) * height))));
                        }
                    }
                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }
            }
            Stopwatch sw = new Stopwatch();
            foreach (double key in featureHash.Keys)
            {
                sw.Reset();
                sw.Start();
                List<LineSegment> lineSegList = featureHash[key] as List<LineSegment>;
                if (lineSegList == null) break;
                List<Polygon> polyList = new List<Polygon>();
                while (lineSegList.Count != 0)
                {
                    List<Coordinate> polyShell = new List<Coordinate>();
                    LineSegment start = lineSegList[0];
                    polyShell.Add(start.P0);
                    polyShell.Add(start.P1);
                    lineSegList.Remove(start);
                    while (!polyShell[0].Equals2D(polyShell[polyShell.Count - 1]))
                    {
                        LineSegment segment = lineSegList.Find(delegate (LineSegment o) {return o.P0.Equals2D(polyShell[polyShell.Count - 1]) || o.P1.Equals2D(polyShell[polyShell.Count - 1]);});
                        if (segment.P0.Equals2D(polyShell[polyShell.Count - 1]))
                        {
                            polyShell.Add(segment.P1);
                        }
                        else
                        {
                            polyShell.Add(segment.P0);
                        }
                        lineSegList.Remove(segment);
                    }
                    polyList.Add(new Polygon(polyShell));
                }
                if (polyList.Count == 1)
                {
                    Feature feat = new Feature(polyList[0], output);
                    feat.DataRow["Value"] = key;
                }
                sw.Stop();
                Debug.WriteLine(sw.ElapsedMilliseconds);
            }
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
                return (TextStrings.RasterToPolygonDescription);
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
            _inputParam = new Parameter[1];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster);
            _inputParam[0].HelpText = TextStrings.inputrastetoconvert;

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.Convertedfeatureset);
            _outputParam[0].HelpText = TextStrings.featuresetcreated;
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
            get { return (TextStrings.RasterToPolygon); }
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
            return;
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return (TextStrings.RasterToPolygon); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowSimpeRasterToPolygon); }
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
