//********************************************************************************************************
// Product Name: MapWindow.Tools.mwIDW
// Description:  creates a raster from a point feature set using Inverse Distance Weighting
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
// Name                   |           Date         |          Comments
// -----------------------|------------------------|--------------------------------------------
// Ted Dunsford           |  8/21/2009             |  Cleaned up some formatting issues using re-sharper 
// KP                     |  9/2009                |  Used IDW as model for InverseDistance
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs. 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;

namespace MapWindow.Tools
{
    public enum NeighborhoodType {FixedDistance,FixedCount};

    public class IDW : ITool
    {
        private string _workingPath;
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;
        private List<string> _neighborhoodType;

        /// <summary>
        /// A UniqueName Identifying this Tool, if another tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        string ITool.UniqueName
        { get { return (TextStrings.MapWindowInverseDistance); } }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        string ITool.Name
        {   get { return ("IDW"); }}

        string ITool.Author
        { get { return (TextStrings.MapWindowDevelopmentTeam); } }

        Version ITool.Version
        { get { return (new Version(1,0,0,0));}}

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        void ITool.Initialize()
        {
            _inputParam = new Parameter[7];
            _inputParam[0] = new PointFeatureSetParam(TextStrings.PointFeatureSet);
            _inputParam[1] = new ListParam(TextStrings.Zvalue);
            _inputParam[1].HelpText = TextStrings.layercontainsvalues;
            _inputParam[2] = new DoubleParam(TextStrings.CellSize, 0, 0, double.MaxValue);
            _inputParam[2].HelpText = TextStrings.Thecellsizeingeographicunits;
            _inputParam[3] = new DoubleParam(TextStrings.Power, 2, 1, double.MaxValue);
            _inputParam[3].HelpText = TextStrings.Theinfluenceofdistance;
            _neighborhoodType = new List<string>();
            _neighborhoodType.Add(TextStrings.FixedDistance);
            _neighborhoodType.Add(TextStrings.FixedCount);
            _inputParam[4] = new ListParam(TextStrings.NeighborhoodType, _neighborhoodType, 0);
            _inputParam[4].HelpText = TextStrings.Selectthetypeofneighborhood;
            _inputParam[5] = new IntParam(TextStrings.MinMaxnumberofpoints, 12, 0, int.MaxValue);
            _inputParam[5].HelpText = TextStrings.FixedDistanceHelpText;
            _inputParam[6] = new DoubleParam(TextStrings.MinMaxdistance, 0, 0, double.MaxValue);
            _inputParam[6].HelpText = TextStrings.FixedDistanceHelpText; ;
            
            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.Raster);
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        string ITool.Description
        {
            get { return (TextStrings.IDWDescription); }//IDWDescription
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate other Parameter default values.
        /// </summary>
        void ITool.ParameterChanged(Parameter sender)
        {
            if (sender == _inputParam[0])
            {
                FeatureSet fs = (_inputParam[0].Value as FeatureSet);
                ListParam lp = (_inputParam[1] as ListParam);
                if (fs != null && lp != null)
                {
                    lp.ValueList.Clear();
                    for (int i = 0; i < fs.DataTable.Columns.Count; i++)
                    {   
                        lp.ValueList.Add(fs.DataTable.Columns[i].ColumnName);
                    }
                    lp.Value = -1;
                }
            }
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        string ITool.ToolTip
        {
            get { return (TextStrings.InverseDistanceWeighting); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        string ITool.Category
        {
            get{return(TextStrings.Interpolation);}
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
            if (input == null) return false;
            input.FillAttributes();
            ListParam lp = _inputParam[1] as ListParam;
            if (lp == null) return false;
            string zField = (lp).ValueList[lp.Value];
            double cellSize = (double)_inputParam[2].Value;
            double power = (double)_inputParam[3].Value;
            NeighborhoodType neighborType;
            if (_inputParam[4].Value as string == TextStrings.FixedDistance)
                neighborType = NeighborhoodType.FixedDistance;
            else
                neighborType = NeighborhoodType.FixedCount;
            int pointCount = (int)_inputParam[5].Value;
            double distance = (double)_inputParam[6].Value;
            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input, zField, cellSize, power, neighborType, pointCount, distance, output, CancelProgressHandler);
        }

        /// <summary>
        /// Executes the Area tool with programatic input
        /// Ping delete static for external testing
        /// </summary>
        /// <param name="input">The input raster</param>
        /// <param name="output">The output polygon feature set</param>
        /// <param name="zField">The field name containing the values to interpolate</param>
        /// <param name="cellSize">The double geographic size of the raster cells to create</param>
        /// <param name="power">The double power representing the inverse</param>
        /// <param name="neighborType">Fixed distance of fixed number of neighbors</param>
        /// <param name="pointCount">The number of neighbors to include if the neighborhood type
        /// is Fixed</param>
        /// <param name="distance">Points further from the raster cell than this distance are not included 
        /// in the calculation if the neighborhood type is Fixed Distance.</param>
        /// <param name="output">The output raster where values are stored.  The filename is used, but the number
        /// of rows and columns will be computed from the cellSize and input featureset</param>
        /// <param name="cancelProgressHandler">A progress handler for receiving progress messages</param>
        /// <returns>A boolean, true if the IDW process worked correctly</returns>
        public bool Execute(IFeatureSet input, string zField, double cellSize, double power, NeighborhoodType neighborType, int pointCount, double distance, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
           
            //Validates the input and output data
            if (input == null || output == null)
                return false;

            //If the cellSize is 0 we calculate a cellsize based on the input extents
            if (cellSize == 0)
                cellSize = input.Envelope.Width / 255;

            //Defines the dimesions and position of the raster
            int numColumns = Convert.ToInt32(Math.Round(input.Envelope.Width / cellSize));
            int numRows = Convert.ToInt32(Math.Round(input.Envelope.Height / cellSize));
          
            output = Raster.Create(output.Filename, "",numColumns, numRows, 1, typeof(double), new[] {""} );

            output.CellHeight = cellSize;
            output.CellWidth = cellSize;
            output.Xllcenter = input.Envelope.Minimum.X + (cellSize/2);
            output.Yllcenter = input.Envelope.Minimum.Y + (cellSize/2);

            //Used to calculate progress
            int lastUpdate=0;

            //Populates the KD tree
            MapWindow.Analysis.Topology.KDTree.KDTree kd = new MapWindow.Analysis.Topology.KDTree.KDTree(2);
            List<int> randomList = new List<int>();
            for (int i = 0; i < input.Features.Count; i++)
            {
                randomList.Add(i);
            }

            Random rnd = new Random();
            List<int> completed = new List<int>();
            while (randomList.Count > 0)
            {
                int index = rnd.Next(0,randomList.Count -1);
                Coordinate coord = input.Features[randomList[index]].Coordinates[0];
                while (kd.Search(coord.ToArray()) != null)
                    coord.X = coord.X * 1.000000000000001D;                    
                kd.Insert(coord.ToArray(), input.Features[randomList[index]]);
                completed.Add(randomList[index]);
                randomList.RemoveAt(index);
            }

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //Makes sure we don't try to search for more points then exist
            if (kd.Count < pointCount)
                pointCount = kd.Count;

            if (neighborType == NeighborhoodType.FixedCount)
            {
                //we add all the old features to output
                for (int x = 0; x < numColumns; x++)
                {
                    for (int y = 0; y < numRows; y++)
                    {                     
                        //Gets the pointCount number of cells closest to the current cell
                        Coordinate cellCenter = output.CellToProj(y, x);
                        Double[] pixelCoord = new double[2];
                        pixelCoord[0] = output.CellToProj(y, x).X;
                        pixelCoord[1] = output.CellToProj(y, x).Y;
                        sw.Start();
                        object[] result = kd.Nearest(pixelCoord, pointCount);
                        sw.Stop();

                        //Sets up the IDW numerator and denominator
                        double top = 0;
                        double bottom = 0;
                        foreach (object feat in result)
                        {
                              IFeature featurePt = feat as Feature;
                              if (featurePt == null) continue;
                              double distanceToCell = cellCenter.Distance(featurePt.Coordinates[0]);
                              if (distanceToCell <= distance || distance == 0)
                              {
                                  //If we can't convert the value to a double throw it out
                                  try
                                  {
                                      Convert.ToDouble(featurePt.DataRow[zField]);
                                  }
                                  catch
                                  {
                                      continue;
                                  }

                                  if (power == 2)
                                  {
                                      top += (1 / (distanceToCell * distanceToCell)) * Convert.ToDouble(featurePt.DataRow[zField]);
                                      bottom += (1 / (distanceToCell* distanceToCell));
                                  }
                                  else
                                  {
                                      top += (1 / Math.Pow(distanceToCell, power)) * Convert.ToDouble(featurePt.DataRow[zField]);
                                      bottom += (1 / Math.Pow(distanceToCell, power));
                                  }
                              }
                        }

                        output.Value[y, x] = top / bottom;
                    }

                    //Checks if we need to update the status bar
                    if (Convert.ToInt32(Convert.ToDouble(x*numRows ) / Convert.ToDouble(numColumns * numRows) * 100) > lastUpdate)
                    {
                        lastUpdate = Convert.ToInt32(Convert.ToDouble(x * numRows) / Convert.ToDouble(numColumns * numRows) * 100);
                        cancelProgressHandler.Progress("", lastUpdate, "Cell: " + (x * numRows) + " of " + (numColumns * numRows));
                        if (cancelProgressHandler.Cancel)
                        return false;
                    }
                }
                System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
            }

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
            get { return (TextStrings.IDWDescription); }
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
