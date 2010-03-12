//********************************************************************************************************
// Product Name: MapWindowTools.mwIDW
// Description:  Inverse Distance Weighting point to raster interpolation
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
//********************************************************************************************************
using System.Collections.Generic;
using System.Collections;
using System;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MapWindow.Tools;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow;
using MapWindow.Geometries;
using MapWindow.Main;
using MapWindow.Components;

namespace MapWindowTools.Vector_Overlay_Class
{
    class mwPolygonToRaster:ITool
    {
        private string _workingPath;
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;
        private List<string> _selectionType;

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
            get { return ("MapWindow Development Team"); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return ("Vector Overlay"); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return ("This will Generate Raster from given Polygon."); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet poly = _inputParam[0].Value as IFeatureSet;
            int numRow = (int)_inputParam[1].Value;
            int numCol = (int)_inputParam[2].Value;
            int selectionIndex = (int)_inputParam[3].Value;
            
            MapWindow.Data.IRaster output = _outputParam[0].Value as MapWindow.Data.IRaster;


            return Execute(poly, numRow, numCol,selectionIndex, output, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons.
        /// </summary>
        /// <param name="poly">The Polygon.</param>
        /// <param name="output">The Raster.</param>
        /// <param name="progress">The progress handler.</param>
        public bool Execute(IFeatureSet poly, int noOfRow, int noOfCol, int selectionIndex, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (poly == null || output == null)
            {
                return false;
            }
            if (poly.FeatureType != FeatureTypes.Polygon)
                return false;

            //double CellSize = poly.Envelope.Width / 255;
            //int noOfRow = Convert.ToInt32(poly.Envelope.Height / CellSize);
            //int noOfCol = Convert.ToInt32(poly.Envelope.Width / CellSize);
            output=Raster.Create(output.Filename,"",noOfCol,noOfRow,1,typeof(int),new string[] { "" });

            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, poly.Envelope);

            output.Bounds=bound;

            output.NoDataValue = -1;
            int current, previous = 0;
            double area = 0.0, previousArea = 0.0;
            double hCellWidth = output.CellWidth / 2;
            double hCellHeight = output.CellHeight / 2;
            ICoordinate[] coordinateCell=new Coordinate[4];
            int polyIndex = -1;
            bool cover = false;
            //IEnvelope env=null;
            for (int i = 0; i < output.NumRows; i++)
            {
                current = Convert.ToInt32(i*100 / output.NumRows);
                //only update when increment in percentage
                if (current > previous+5)
                {
                    cancelProgressHandler.Progress("", current, current.ToString() + "% progress completed");
                    previous = current;
                }

                for (int j = 0; j < output.NumColumns; j++)
                {
                    polyIndex = -1;
                    area = 0.0;
                    previousArea = 0.0;
                    cover=false;
                    ICoordinate cordinate=null;
                    cordinate = output.CellToProj(i, j);
                    //make the center of cell as point geometry
                    IPoint pt=new Point(cordinate);

                    for(int f=0;f<poly.Features.Count;f++)
                    {
                        if (selectionIndex == 0)
                        {
                            if (poly.Features[f].Covers(pt))
                            {
                                output.Value[i, j] = f;
                                cover = true;
                                break;
                            }
                            if (cancelProgressHandler.Cancel == true)
                                return false;
                        }
                        else //process area based selection
                        {
                            ICoordinate tempCo = new Coordinate(cordinate.X - hCellWidth, cordinate.Y - hCellHeight);
                            coordinateCell[0] = tempCo;
                            tempCo = new Coordinate(cordinate.X + hCellWidth, cordinate.Y - hCellHeight);
                            coordinateCell[1] = tempCo;
                            tempCo = new Coordinate(cordinate.X + hCellWidth, cordinate.Y + hCellHeight);
                            coordinateCell[2] = tempCo;
                            tempCo = new Coordinate(cordinate.X - hCellWidth, cordinate.Y + hCellHeight);
                            coordinateCell[3] = tempCo;

                            List<ICoordinate> ListCellCordinate = new List<ICoordinate>();
                            ListCellCordinate = coordinateCell.ToList<ICoordinate>();
                            IFeature cell = new Feature(FeatureTypes.Polygon, ListCellCordinate);
                            IFeature commonFeature=poly.Features[f].Intersection(cell);
                            if (commonFeature == null)
                                continue;
                            area=commonFeature.Area();
                            if (area > previousArea)
                            {
                                polyIndex = f;
                                cover = true;
                                previousArea = area;
                            }

                            if (cancelProgressHandler.Cancel == true)
                                return false;
                        }

                        
                    }
                    if (cover == true)
                    {
                        if (selectionIndex == 1)
                            output.Value[i, j] = polyIndex;
                    }
                    else
                        output.Value[i, j] = output.NoDataValue;
                }

            }
            //output.SaveAs(output.Filename);
            output.Save();
            //output.SaveAs(output.Filename);
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
                return ("This will Generate Raster from given Polygon.");
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
            _inputParam[0] = new FeatureSetParam("input1 polygon to Raster.");
            _inputParam[0].HelpText = "Input Polygon to change to Raster featureset";
            _inputParam[1] = new IntParam("Input No of Cell rows in the output raster.");
            _inputParam[1].HelpText = "Standard is 255 but you can input according to your requirement.";
            _inputParam[2] = new IntParam("Input No of Cell colums in the output raster.");
            _inputParam[2].HelpText = "Standard is 255 but you can input according to your requirement.";
            _selectionType = new List<string>();
            _selectionType.Add("Cover Center");
            _selectionType.Add("Cover Area");
            _inputParam[3] = new ListParam("SelectionType", _selectionType, 0);
            _inputParam[3].HelpText = "Select the type of selection when consider cell allocaton.\n\n Cover Center: " +
                                      "what ever the polygon cover the center of the cell will alocate to that polygon.\n\n Cover Area: " +
                                      "This will decide which polygon cover more area with that cell and allocate the more covering polygon"+
                                      " to that cell.";

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam("Output Raster.");
            _outputParam[0].HelpText = "Select the Result of Output feature set with average slope in the attribute.";
        }

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public MapWindow.Tools.Param.Parameter[] InputParameters
        {
            get { return _inputParam; }
        }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        public string Name
        {
            get { return ("Polygon To Raster"); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public MapWindow.Tools.Param.Parameter[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        void ITool.ParameterChanged(Parameter sender)
        {
            if (sender == _inputParam[0])
            {
                _inputParam[1].Value = 255;
                _inputParam[2].Value = 255;
            }
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return ("Polygon To Raster generation"); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return ("MapWindow PolygonToRaster"); }
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
