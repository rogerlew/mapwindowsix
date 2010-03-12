using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using MapWindow.Tools;
using MapWindow.Data;
using MapWindow.Tools.Param;
using MapWindow.Geometries;


namespace MapWindowTools
{
    class mwMergeGrids:ITool
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
            get { return ("MapWindow Development Team"); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return ("Raster Overlay"); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return ("This will Merge Two Raster  Layer cell by cell"); }
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            MapWindow.Data.IRaster input1 = _inputParam[0].Value as MapWindow.Data.IRaster;
            MapWindow.Data.IRaster input2 = _inputParam[1].Value as MapWindow.Data.IRaster;

            MapWindow.Data.IRaster output = _outputParam[0].Value as MapWindow.Data.IRaster;

            return Execute(input1, input2, output, cancelProgressHandler);

        }

        /// <summary>
        /// Executes the Erase Opaeration tool programaticaly
        /// </summary>
        /// <param name="input1">The input raster</param>
        /// <param name="input2">The input raster</param>
        /// <param name="output">The output raster</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IRaster input1, IRaster input2, IRaster output, MapWindow.Tools.ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }

            IEnvelope envelope = new Envelope() as IEnvelope;

            envelope = UnionEnvelope(input1, input2);

            int noOfCol;
            int noOfRow;

            //Figures out which raster has smaller cells
            IRaster smallestCellRaster;
            if (input1.CellWidth < input2.CellWidth) smallestCellRaster = input1;
            else smallestCellRaster = input2;

            //Given the envelope of the two rasters we calculate the number of columns / rows
            noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / smallestCellRaster.CellWidth));
            noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / smallestCellRaster.CellHeight));

            //Create the new raster with the appropriate dimensions
            Raster Temp = new Raster();
            //Temp.CreateNew(output.Filename, noOfRow, noOfCol, input1.DataType);
            Temp.CreateNew(output.Filename, "", noOfCol, noOfRow, 1, input1.DataType, new string[] { "" });

            Temp.CellWidth = smallestCellRaster.CellWidth;
            Temp.CellHeight = smallestCellRaster.CellHeight;
            Temp.Xllcenter = envelope.Minimum.X + (Temp.CellWidth / 2);
            Temp.Yllcenter = envelope.Minimum.Y + (Temp.CellHeight / 2);

            MapWindow.Geometries.ICoordinate I1 = new MapWindow.Geometries.Coordinate() as MapWindow.Geometries.ICoordinate;
            RcIndex v1 = new RcIndex();
            RcIndex v2 = new RcIndex();

            double val1;
            double val2;
            int previous=0;
            int current=0;
            int max = (Temp.Bounds.NumRows + 1);
            for (int i = 0; i < Temp.Bounds.NumRows; i++)
            {
                for (int j = 0; j < Temp.Bounds.NumColumns; j++)
                {
                    I1 = Temp.CellToProj(i, j);
                    v1 = input1.ProjToCell(I1);
                    if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                        val1 = input1.Value[v1.Row, v1.Column];
                    else
                        val1 = input1.NoDataValue;

                    v2 = input2.ProjToCell(I1);
                    if (v2.Row <= input2.EndRow && v2.Column <= input2.EndColumn && v2.Row > -1 && v2.Column > -1)
                        val2 = input2.Value[v2.Row, v2.Column];
                    else
                        val2 = input2.NoDataValue;

                    if (val1 == input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        Temp.Value[i, j] = Temp.NoDataValue;
                    }
                    else if (val1 != input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        Temp.Value[i, j] = val1;
                    }
                    else if (val1 == input1.NoDataValue && val2 != input2.NoDataValue)
                    {
                        Temp.Value[i, j] = val2;
                    }
                    else
                    {
                        Temp.Value[i, j] = val1;
                    }

                    if (cancelProgressHandler.Cancel == true)
                        return false;
                }
                current = Convert.ToInt32(Math.Round(i * 100D / max));
                //only update when increment in persentage
                if(current>previous)
                    cancelProgressHandler.Progress("", current, current.ToString() + "% progress completed");
                previous = current;
            }

            output = Temp;
            output.Save();
            return true;
        }
        /// <summary>
        /// Execute the union region for output envelope 
        /// </summary>
        /// <param name="input1">input raster</param>
        /// <param name="input2">input raster</param>
        /// <returns>Envelope</returns>
        private IEnvelope UnionEnvelope(IRaster input1, IRaster input2)
        {
            IEnvelope e1 = new Envelope() as IEnvelope;
            IEnvelope e2 = new Envelope() as IEnvelope;

            e1 = input1.Bounds.Envelope;
            e2 = input2.Bounds.Envelope;
            e1.ExpandToInclude(e2);
            return (e1);

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
                return ("This will Merge Two Raster  Layer cell by cell");
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
            _inputParam[0] = new RasterParam("input1 Raster");
            _inputParam[0].HelpText = "Input First Raster";
            _inputParam[1] = new RasterParam("input2 Raster");
            _inputParam[1].HelpText = "Input Second Raster for Merging";

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam("Output Raster");
            _outputParam[0].HelpText = "Select the Result Raster Directory and Name";

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
            get { return ("Merge Raster Layers"); }
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
            return;
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return ("Merge two Raster Layers"); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return ("MapWindow Raster Merge"); }
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
