//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/6/2009 10:14:34 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using MapWindow.Main;
using MapWindow.Projections;


namespace MapWindow.Data
{


    /// <summary>
    /// BgdRaster
    /// </summary>
    public class BgdRaster<T> : Raster<T> where T:IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new, unspecified version of a bgdRaster than can be opened
        /// </summary>
        public BgdRaster(string filename)
        {
            base.Filename = filename;
            ReadHeader(filename);
            Initialize();
            if(base.NumColumnsInFile * base.NumRowsInFile < 64000000)
            {
                Data = ReadRaster();
                base.GetStatistics();
            }
        }

        /// <summary>
        /// Creates a new instance of a BGD raster, attempting to store the entire structure in memory if possible.
        /// </summary>
        /// <param name="numRows"></param>
        /// <param name="numColumns"></param>
        public BgdRaster(int numRows, int numColumns):base(numRows, numColumns)
        {
            
        }
       
        #endregion

        #region Methods

        /// <summary>
        /// This Method should be overrridden by classes, and provides the primary ability
        /// </summary>
        /// <param name="xOff">The horizontal offset of the area to read values from</param>
        /// <param name="sizeX">The number of values to read into the buffer</param>
        /// <param name="yOff">The vertical offset of the window to read values from</param>
        /// <param name="sizeY">The vertical size of the window to read into the buffer</param>
        /// <returns></returns>
        public override T[][] ReadRaster(int xOff, int yOff, int sizeX, int sizeY)
        {
            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read, NumColumns * ByteSize);
            ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                 MessageStrings.ReadingValuesFrom_S.Replace("%S", Filename), NumRows);
            fs.Seek(HeaderSize, SeekOrigin.Begin);

            // Position the binary reader at the top of the "window"
            fs.Seek(yOff * NumColumnsInFile * ByteSize, SeekOrigin.Current);
            BinaryReader br = new BinaryReader(fs);

            T[][] result = new T[NumRows][];
            int endX = xOff + sizeX;

            for (int row = 0; row < sizeY; row++)
            {
                result[row] = new T[sizeX];
                // Position the binary reader at the beginning of the window
                fs.Seek(ByteSize * xOff, SeekOrigin.Current);
                byte[] values = br.ReadBytes(sizeX*ByteSize);
                Buffer.BlockCopy(values, 0, result[row], 0, ByteSize * sizeX);
                pm.CurrentValue = row;
                fs.Seek(ByteSize * (NumColumnsInFile - endX), SeekOrigin.Current);
            }
            br.Close();
            return result;
        }

        /// <summary>
        /// Writes the bgd content from the specified jagged array of values to the file.
        /// </summary>
        /// <param name="buffer">The data</param>
        /// <param name="xOff">The horizontal offset</param>
        /// <param name="yOff">The vertical offset</param>
        /// <param name="xSize">The number of values to write horizontally</param>
        /// <param name="ySize">The number of values to write vertically</param>
        public override void WriteRaster(T[][] buffer, int xOff, int yOff, int xSize, int ySize)
        {
            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Write, FileShare.Write, NumColumns * ByteSize);
            ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                 MessageStrings.ReadingValuesFrom_S.Replace("%S", Filename), NumRows);
            fs.Seek(HeaderSize, SeekOrigin.Begin);

            // Position the binary reader at the top of the "window"
            fs.Seek(yOff * NumColumnsInFile * ByteSize, SeekOrigin.Current);
            BinaryWriter br = new BinaryWriter(fs);
            int endX = xOff + xSize;
            for (int row = 0; row < ySize; row++)
            {
                // Position the binary reader at the beginning of the window
                fs.Seek(ByteSize * xOff, SeekOrigin.Current);
                byte[] values = new byte[xSize * ByteSize];
                Buffer.BlockCopy(buffer[row], 0, values, 0, xSize * ByteSize);
                br.Write(values);
                pm.CurrentValue = row;
                fs.Seek(ByteSize * (NumColumnsInFile - endX), SeekOrigin.Current);
            }
            br.Close();
        }

        /// <summary>
        /// Copies the raster header, and if copyValues is true, the values to the specified file </summary>
        /// <param name="filename">The full path of the file to copy content to to</param>
        /// <param name="copyValues">Boolean, true if this should copy values as well as just header information</param>
        public override void Copy(string filename, bool copyValues)
        {
            if(copyValues)
            {
                Write(filename);
            }
            else
            {
                WriteHeader(filename);
                T[] blank = new T[NumColumnsInFile];
                T val = Global.ConvertT<T>(NoDataValue);
                for (int col = 0; col < NumColumnsInFile; col ++)
                {
                    blank[col] = val;
                }
                for (int row = 0; row < NumRowsInFile; row++)
                {
                    WriteRow(blank, row);
                }
            }
        }

        /// <summary>
        /// Opens the specified file
        /// </summary>
        /// <param name="filename">The full path of the specified filename</param>
        public override void Open(string filename)
        {
            ReadHeader(filename);
            Data = ReadRaster();
            GetStatistics();
        }

        /// <summary>
        /// Saves the content from this file using the current filename and header information
        /// </summary>
        public override void Save()
        {
            Write(Filename);
        }

        /// <summary>
        /// If no file exists, this writes the header and no-data values.  If a file exists, it will assume
        /// that data already has been filled in the file and will attempt to insert the data values
        /// as a window into the file.  If you want to create a copy of the file and values, just use
        /// System.IO.File.Copy, it almost certainly would be much more optimized.
        /// </summary>
        private void Write(string filename)
        {
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Writing values to " + Filename, NumRows);
            long expectedByteCount = NumRows * NumColumns * ByteSize;
            if (expectedByteCount < 1000000) pm.StepPercent = 5;
            if (expectedByteCount < 5000000) pm.StepPercent = 10;
            if (expectedByteCount < 100000) pm.StepPercent = 50;

            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(Filename);
                // if the following test fails, then the target raster doesn't fit the bill for pasting into, so clear it and write a new one.
                if (fi.Length == HeaderSize + ByteSize * NumColumnsInFile * NumRowsInFile)
                {
                    WriteHeader(filename);
                    WriteRaster(Data);
                    return;
                }

                // If we got here, either the file didn't exist or didn't match the specifications correctly, so write a new one.

                Debug.WriteLine("The size of the file was " + fi.Length + " which didn't match the expected " +
                                HeaderSize + ByteSize * NumColumnsInFile * NumRowsInFile);
            }

            if (File.Exists(Filename)) File.Delete(Filename);
            WriteHeader(filename);

            // Open as append and it will automatically skip the header for us.
            FileStream fs = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.None, ByteSize * NumColumnsInFile);
            BinaryWriter bw = new BinaryWriter(fs);

            // the row and column counters here are relative to the whole file, not just the window that is currently in memory.
            pm.EndValue = NumRowsInFile;

            for (int row = 0; row < NumRowsInFile; row++)
            {
                byte[] rawBytes = new byte[NumColumnsInFile * ByteSize];
                T[] nd = new T[1];
                nd[0] = Global.ConvertT<T>(NoDataValue);
                Buffer.BlockCopy(Data[row - StartRow], 0, rawBytes, StartColumn * ByteSize, NumColumns * ByteSize);
                for (int col = 0; col < StartColumn; col++)
                {
                    Buffer.BlockCopy(nd, 0, rawBytes, col*ByteSize, ByteSize);
                }
                for (int col = EndColumn+1; col < NumColumnsInFile; col++)
                {
                    Buffer.BlockCopy(nd, 0, rawBytes, col * ByteSize, ByteSize);
                }
                bw.Write(rawBytes);
                pm.CurrentValue = row;
            }


            fs.Flush(); // flush anything that hasn't gotten written yet.
            pm.Reset();
            bw.Close();
        }

        /// <summary>
        /// The string filename where this will begin to write data by clearing the existing file
        /// </summary>
        public void WriteHeader(string filename)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.OpenOrCreate));
            bw.Write(NumColumnsInFile);
            bw.Write(NumRowsInFile);
            bw.Write(CellWidth);
            bw.Write(CellHeight);
            bw.Write(Xllcenter);
            bw.Write(Yllcenter);
            if (DataType == typeof (int))
            {
                bw.Write((int)RasterDataTypes.INTEGER);
            }
            if (DataType == typeof(float))
            {
                bw.Write((int) RasterDataTypes.SINGLE);
            }
            if (DataType == typeof(double))
            {
                bw.Write((int)RasterDataTypes.DOUBLE);
            }
            if (DataType == typeof(byte))
            {
                bw.Write((int)RasterDataTypes.BYTE);
            }
            if (DataType == typeof(short))
            {
                bw.Write((int)RasterDataTypes.SHORT);
            }
        
            byte[] nd = new byte[ByteSize];
            T[] nds = new[] {Global.ConvertT<T>(NoDataValue)};
            Buffer.BlockCopy(nds, 0, nd, 0, ByteSize);
            bw.Write(nd);

            // These are each 256 bytes because they are ASCII encoded, not the standard DotNet Unicode
            byte[] proj = new byte[255];
            if (Projection != null)
            {
                byte[] temp = Encoding.Default.GetBytes(Projection.ToProj4String());
                int len = Math.Min(temp.Length, 255);
                for (int i = 0; i < len; i++)
                {
                    proj[i] = temp[i];
                }
            }
            bw.Write(proj);
            byte[] note = new byte[255];
            if (Notes != null)
            {
                byte[] temp = Encoding.Default.GetBytes(Notes);
                int len = Math.Min(temp.Length, 255);
                for (int i = 0; i < len; i++)
                {
                    note[i] = temp[i];
                }
            }


            bw.Write(note);
            bw.Close();
        }


       

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of the header.  There is one no-data value in the header.
        /// </summary>
        public virtual int HeaderSize
        {
            get { return 554 + ByteSize; }
        }

        /// <summary>
        /// Writes the header, regardless of which subtype of binary raster this is written for
        /// </summary>
        /// <param name="filename">The string filename specifying what file to load</param>
        public void ReadHeader(string filename)
        {
            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
            StartColumn = 0;
            NumColumns = br.ReadInt32();
            NumColumnsInFile = NumColumns;
            EndColumn = NumColumns - 1;
            StartRow = 0;
            NumRows = br.ReadInt32();
            NumRowsInFile = NumRows;
            EndRow = NumRows - 1;
            Bounds = new RasterBounds(NumRows, NumColumns, new[] { 0.0, 1.0, 0.0, NumRows, 0.0, -1.0 });

            CellWidth = br.ReadDouble();
            Bounds.AffineCoefficients[5] = -br.ReadDouble(); // dy
            Xllcenter = br.ReadDouble();
            Yllcenter = br.ReadDouble();
            RasterDataTypes dataType = (RasterDataTypes) br.ReadInt32();
            byte[] noDataBytes = br.ReadBytes(ByteSize);
            T[] nd = new T[1];
            Buffer.BlockCopy(noDataBytes, 0, nd, 0, ByteSize);
            NoDataValue = nd[0].ToDouble(null);
            string proj = Encoding.Default.GetString(br.ReadBytes(255)).Replace('\0', ' ').Trim();
            Projection = new ProjectionInfo();
            Projection.ReadProj4String(proj);
            Notes = Encoding.Default.GetString(br.ReadBytes(255)).Replace('\0', ' ').Trim();
            if (Notes.Length == 0) Notes = null;
            br.Close();
        }

        #endregion



    }
}
