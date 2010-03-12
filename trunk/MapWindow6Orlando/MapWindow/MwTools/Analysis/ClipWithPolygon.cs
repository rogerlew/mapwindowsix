//********************************************************************************************************
// Product Name: MapWindow.Tools.ClipWithPolygon
// Description:  Clip Grids with Polygon Shape File
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
// KP                 |  9/2009            |  Used IDW as model for ClipwithPolygon
// Ping               |  12/2009           |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{

    public class ClipWithPolygon:ITool
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
            get { return (TextStrings.ClipGridswithPolygon); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            IFeatureSet input2 = _inputParam[1].Value as IFeatureSet;

            IRaster output = _outputParam[0].Value as IRaster;

            return Execute(input1, input2, output, cancelProgressHandler);

        }

        /// <summary>
        /// Executes the Erase Opaeration tool programaticaly.
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input1">The input raster.</param>
        /// <param name="input2">The input raster.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(IRaster input1, IFeatureSet input2, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }
            double cellWidth = input1.CellWidth;
            double cellHeight = input1.CellHeight;
            

            //Calculate the Intersect Envelope
            IEnvelope envelope1 = input1.Bounds.Envelope;
            IEnvelope envelope2 = input2.Envelope;
            envelope1 = envelope1.Intersection(envelope2);

            int noOfCol = Convert.ToInt32(envelope1.Height / cellHeight);

            int noOfRow = Convert.ToInt32(envelope1.Width / cellWidth);

            //create output raster
            output = Raster.Create(output.Filename, "", noOfCol, noOfRow, 1, typeof(int), new[] { "" });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope1);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            RcIndex v1;
         
           // MapWindow.Analysis.Topology.Algorithm.PointLocator pointLocator1 = new MapWindow.Analysis.Topology.Algorithm.PointLocator();

            int previous = 0;

            int max = (output.Bounds.NumRows + 1);
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    Coordinate cellCenter = output.CellToProj(i, j);
                   
                   

                    //test if the coordinate is inside of the polygon
                    bool isInside = false;
                    IFeature pointFeat = new Feature(new Point(cellCenter));
                        foreach (IFeature f in input2.Features)
                        {
                            if (f.Contains(pointFeat))
                            {
                                //output.Value[i, j] = val1;
                                isInside = true;
                                break;
                            }
                        }

                        if (isInside)
                        {
                            v1 = input1.ProjToCell(cellCenter);

                            double val1;
                            if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                                val1 = input1.Value[v1.Row, v1.Column];
                            else
                                val1 = output.NoDataValue;

                            output.Value[i, j] = val1;
                        }
                        else
                        {
                            output.Value[i, j] = output.NoDataValue;
                        }
                 
                                        
                    if (cancelProgressHandler.Cancel)
                        return false;
                }
                int current = Convert.ToInt32(Math.Round(i * 100D / max));
                //only update when increment in percentage
                if (current > previous)
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                previous = current;
            }

            //output = Temp;
            output.Save();
            return true;
        }
        /// <summary>
        /// Ping Yang Overwritted Execute Function for External Testing
        /// </summary>
        public bool Execute(IRaster input1, IFeatureSet input2, IRaster output)
        {
            //Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }
            double cellWidth = input1.CellWidth;
            double cellHeight = input1.CellHeight;


            //Calculate the Intersect Envelope
            IEnvelope envelope1 = input1.Bounds.Envelope;
            IEnvelope envelope2 = input2.Envelope;
            envelope1 = envelope1.Intersection(envelope2);

            int noOfCol = Convert.ToInt32(envelope1.Height / cellHeight);

            int noOfRow = Convert.ToInt32(envelope1.Width / cellWidth);

            //create output raster
            output = Raster.Create(output.Filename, "", noOfCol, noOfRow, 1, typeof(int), new[] { "" });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope1);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            RcIndex v1;

            // MapWindow.Analysis.Topology.Algorithm.PointLocator pointLocator1 = new MapWindow.Analysis.Topology.Algorithm.PointLocator();

            int previous = 0;

            int max = (output.Bounds.NumRows + 1);
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    Coordinate cellCenter = output.CellToProj(i, j);



                    //test if the coordinate is inside of the polygon
                    bool isInside = false;
                    IFeature pointFeat = new Feature(new Point(cellCenter));
                    foreach (IFeature f in input2.Features)
                    {
                        if (f.Contains(pointFeat))
                        {
                            //output.Value[i, j] = val1;
                            isInside = true;
                            break;
                        }
                    }

                    if (isInside)
                    {
                        v1 = input1.ProjToCell(cellCenter);

                        double val1;
                        if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                            val1 = input1.Value[v1.Row, v1.Column];
                        else
                            val1 = output.NoDataValue;

                        output.Value[i, j] = val1;
                    }
                    else
                    {
                        output.Value[i, j] = output.NoDataValue;
                    }

                }
            }

            //output = Temp;
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
                return (TextStrings.ClipGridswithPolygon);
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
            _inputParam[0] = new RasterParam(TextStrings.input1Raster);
            _inputParam[0].HelpText = TextStrings.InputRasterforCliping;
            _inputParam[1] = new FeatureSetParam(TextStrings.input2PolygonforCliping);
            _inputParam[1].HelpText = TextStrings.InputPolygonforclipingtoRaster;

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster);
            _outputParam[0].HelpText = TextStrings.ResultRasterDirectory;

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
            get { return (TextStrings.ClipRasterLayer); }
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
            get { return (TextStrings.ClipRasterLayerwithPolygon); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowRasterClip); }
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
