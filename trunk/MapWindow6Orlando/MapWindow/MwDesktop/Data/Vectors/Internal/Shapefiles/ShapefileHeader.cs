using System.IO;
using System.Windows.Forms;
using MapWindow.Data.IO;
//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
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
// The Original Code is MapWindow
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using MapWindow.Geometries;
namespace MapWindow.Data
{
    /// <summary>
    /// The header for the .shp file and the .shx file that support shapefiles
    /// </summary>
    public class ShapefileHeader
    {
        #region Private Variables
        private int _shxLength;

        // Always 9994 if it is a shapefile
        private int _fileCode;

        // The length of the shp file in bytes
        private int _fileLength;

        // The version, which should be 1000
        private int _version;

        // Specifies line, polygon, point etc.
        private ShapeTypes _shapeType;

        // Extent Values for the entire shapefile
        private double _xMin;
        private double _xMax;
        private double _yMin;
        private double _yMax;
        private double _zMin;
        private double _zMax;
        private double _mMin;
        private double _mMax;

        private string _filename;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new, blank ShapeHeader
        /// </summary>
        public ShapefileHeader()
        {
            Clear();
        }

        /// <summary>
        /// Opens the specified filename directly
        /// </summary>
        /// <param name="inFilename">The string filename to open as a header.</param>
        public ShapefileHeader(string inFilename)
        {
            Open(inFilename);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets all the extent values to 0
        /// </summary>
        public void Clear()
        {
            _version = 1000;
            _fileCode = 9994;
            _xMin = 0.0;
            _xMax = 0.0;
            _yMin = 0.0;
            _yMax = 0.0;
            _zMin = 0.0;
            _zMax = 0.0;
            _mMin = 0.0;
            _mMax = 0.0;
        }

        /// <summary>
        /// Parses the first 100 bytes of a shapefile into the important values
        /// </summary>
        /// <param name="inFilename">The filename to read</param>
        public void Open(string inFilename)
        {
            _filename = inFilename;

            //  Position        Field           Value       Type        ByteOrder
            //  --------------------------------------------------------------
            //  Byte 0          File Code       9994        Integer     Big
            //  Byte 4          Unused          0           Integer     Big
            //  Byte 8          Unused          0           Integer     Big
            //  Byte 12         Unused          0           Integer     Big
            //  Byte 16         Unused          0           Integer     Big
            //  Byte 20         Unused          0           Integer     Big
            //  Byte 24         File Length     File Length Integer     Big
            //  Byte 28         Version         1000        Integer     Little
            //  Byte 32         Shape Type      Shape Type  Integer     Little
            //  Byte 36         Bounding Box    Xmin        Double      Little
            //  Byte 44         Bounding Box    Ymin        Double      Little
            //  Byte 52         Bounding Box    Xmax        Double      Little
            //  Byte 60         Bounding Box    Ymax        Double      Little
            //  Byte 68         Bounding Box    Zmin        Double      Little
            //  Byte 76         Bounding Box    Zmax        Double      Little
            //  Byte 84         Bounding Box    Mmin        Double      Little
            //  Byte 92         Bounding Box    Mmax        Double      Little

            BufferedBinaryReader bbReader = new BufferedBinaryReader(inFilename);

            bbReader.FillBuffer(100); // we only need to read 100 bytes from the header.

            bbReader.Close(); // Close the internal readers connected to the file, but don't close the file itself.

            

            // Reading BigEndian simply requires us to reverse the byte order.
            _fileCode = bbReader.ReadInt32(false);

            // Skip the next 20 bytes because they are unused
            bbReader.Seek(20, System.IO.SeekOrigin.Current);

            // Read the file length in reverse sequence
            _fileLength = bbReader.ReadInt32(false);

            // From this point on, all the header values are in little endian

            // Read the version 
            _version = bbReader.ReadInt32();

            // Read in the shapetype that should be the shapetype for the whole shapefile
            _shapeType = (ShapeTypes)bbReader.ReadInt32();

            // Get the extents, each of which are double values.
            _xMin = bbReader.ReadDouble();
            _yMin = bbReader.ReadDouble();
            _xMax = bbReader.ReadDouble();
            _yMax = bbReader.ReadDouble();
            _zMin = bbReader.ReadDouble();
            _zMax = bbReader.ReadDouble();
            _mMin = bbReader.ReadDouble();
            _mMax = bbReader.ReadDouble();

            bbReader.Dispose();
        }

        /// <summary>
        /// Saves changes to the .shp file will also automatically update the .shx file.
        /// </summary>
        public void Save()
        {
            SaveAs(_filename);
        }

        
        /// <summary>
        /// Saves changes to the .shp file and will also automatically create the header for the .shx file.
        /// </summary>
        /// <param name="outFilename">The string filename to create.</param>
        public void SaveAs(string outFilename)
        {
            _filename = outFilename;
            Write(_filename, _fileLength);
            Write(ShxFilename, _shxLength);
        }

        /// <summary>
        /// Writes the current content to the specified file.
        /// </summary>
        /// <param name="destFilename">The string filename to write to</param>
        /// <param name="destFileLength">The only difference between the shp header and the shx header is the file length parameter.</param>
        private void Write(string destFilename, int destFileLength)
        {
            string dir = Path.GetDirectoryName(destFilename);
            if (!Directory.Exists(dir))
            {
                if (MessageBox.Show("Directory " + dir + " does not exist.  Do you want to create it?", "Create Directory?", MessageBoxButtons.YesNo) != DialogResult.OK)
                    return;
                Directory.CreateDirectory(dir);
            }
            if (System.IO.File.Exists(destFilename)) System.IO.File.Delete(destFilename);

            BufferedBinaryWriter bbWriter = new BufferedBinaryWriter(destFilename, null, 100);

            bbWriter.Write(_fileCode, false);                     //  Byte 0          File Code       9994        Integer     Big 

            byte[] bt = new byte[20];
            bbWriter.Write(bt);                                   //  Bytes 4 - 20 are unused

            bbWriter.Write(destFileLength, false);                //  Byte 24         File Length     File Length Integer     Big

            bbWriter.Write(_version);                             //  Byte 28         Version         1000        Integer     Little

            bbWriter.Write((int)_shapeType);                      //  Byte 32         Shape Type      Shape Type  Integer     Little

            bbWriter.Write(_xMin);                                //  Byte 36         Bounding Box    Xmin        Double      Little

            bbWriter.Write(_yMin);                                //  Byte 44         Bounding Box    Ymin        Double      Little

            bbWriter.Write(_xMax);                                //  Byte 52         Bounding Box    Xmax        Double      Little

            bbWriter.Write(_yMax);                                //  Byte 60         Bounding Box    Ymax        Double      Little

            bbWriter.Write(_zMin);                                //  Byte 68         Bounding Box    Zmin        Double      Little

            bbWriter.Write(_zMax);                                //  Byte 76         Bounding Box    Zmax        Double      Little

            bbWriter.Write(_mMin);                                //  Byte 84         Bounding Box    Mmin        Double      Little

            bbWriter.Write(_mMax);                                //  Byte 92         Bounding Box    Mmax        Double      Little

            // ------------ WRITE TO SHP FILE -------------------------


            bbWriter.Close();

        }
       
        #endregion

       

        /// <summary>
        /// Generates a new envelope based on the extents of this shapefile.
        /// </summary>
        /// <returns>An Envelope</returns>
        public IEnvelope ToEnvelope()
        {
            IEnvelope env = new Envelope(_xMin, _xMax, _yMin, _yMax, Zmin, Zmax);
            env.Minimum.M = _mMin;
            env.Maximum.M = _mMax;
            return env; 
        }

        /// <summary>
        /// Gets or sets the integer file code that should always be 9994.
        /// </summary>
        public int FileCode
        {
            get { return _fileCode; }
            set 
            {
                _fileCode = value;
                
            }
        }

        /// <summary>
        /// Gets or sets the integer file length in bytes
        /// </summary>
        public int FileLength
        {
            get { return _fileLength; }
            set 
            {
                _fileLength = value;
                
            }
        }

        /// <summary>
        /// Gets or sets the string filename to use for this header
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Gets or sets the MapWindow.Data.Shapefiles.ShapeType enumeration specifying 
        /// whether the shapes are points, lines, polygons etc.
        /// </summary>
        public ShapeTypes ShapeType
        {
            get { return _shapeType; }
            set
            {
                _shapeType = value;

            }
        }

        /// <summary>
        /// Changes the extension of the filename to .shx instead of .shp
        /// </summary>
        public string ShxFilename
        {
            get { return System.IO.Path.ChangeExtension(_filename, ".shx"); }
        }
        

        /// <summary>
        /// Gets or sets the version, which should be 1000
        /// </summary>
        public int Version
        {
            get { return _version; }
            set 
            {
                _version = value;
               
            }
        }

        /// <summary>
        /// The minimum X coordinate for the values in the shapefile
        /// </summary>
        public double Xmin
        {
            get { return _xMin; }
            set 
            {
                _xMin = value;
                
            }
        }

        /// <summary>
        /// The maximum X coordinate for the shapes in the shapefile
        /// </summary>
        public double Xmax
        {
            get { return _xMax; }
            set 
            {
                _xMax = value;
               
            }
        }


        /// <summary>
        /// The minimum Y coordinate for the values in the shapefile
        /// </summary>
        public double Ymin
        {
            get { return _yMin; }
            set 
            {
                _yMin = value;
               
            }
        }

        /// <summary>
        /// The maximum Y coordinate for the shapes in the shapefile
        /// </summary>
        public double Ymax
        {
            get { return _yMax; }
            set 
            {
                _yMax = value;
               
            }
        }

        /// <summary>
        /// The minimum Z coordinate for the values in the shapefile
        /// </summary>
        public double Zmin
        {
            get { return _zMin; }
            set 
            {
                _zMin = value;
               
            }
        }

        /// <summary>
        /// The maximum Z coordinate for the shapes in the shapefile
        /// </summary>
        public double Zmax
        {
            get { return _zMax; }
            set 
            {
                _zMax = value;
               
            }
        }

        /// <summary>
        /// The minimum M coordinate for the values in the shapefile
        /// </summary>
        public double Mmin
        {
            get { return _mMin; }
            set 
            {
                _mMin = value;
               
            }
        }

        /// <summary>
        /// The maximum M coordinate for the shapes in the shapefile
        /// </summary>
        public double Mmax
        {
            get { return _mMax; }
            set 
            {
                _mMax = value;
               
            }
        }

        /// <summary>
        /// Gets or sets the length of the shx file in 16 bit words.
        /// </summary>
        public int ShxLength
        {
            get { return _shxLength; }
            set { _shxLength = value; }
        }


       
    }
}
