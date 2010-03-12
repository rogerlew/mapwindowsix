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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MapWindow.Geometries;
using MapWindow.Main;
namespace MapWindow.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the vectors are lines
    /// </summary>
    public class LineShapefile: Shapefile
    {
        private const int BLOCKSIZE = 16000000;
        /// <summary>
        /// Creates a new instance of a LineShapefile for in-ram handling only.
        /// </summary>
        public LineShapefile()
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader();
            Header.FileLength = 100;
            Header.ShapeType = ShapeTypes.PolyLine;
            FeatureType = FeatureTypes.Line;
        }

		/// <summary>
		/// Creates a new instance of a LineShapefile that is loaded from the supplied filename.
		/// </summary>
		/// <param name="filename">The string filename of the polygon shapefile to load</param>
		public LineShapefile(string filename) : this()
		{
			Open(filename, null);
		}

        /// <summary>
        /// Opens a shapefile
        /// </summary>
        /// <param name="filename">The string filename of the line shapefile to load</param>
        /// <param name="progressHandler">Any valid implementation of the MapWindow.Main.IProgressHandler</param>
        public void Open(string filename, IProgressHandler progressHandler)
        {
            Filename = filename;
            FeatureType = FeatureTypes.Line;
            Header = new ShapefileHeader(filename);
            CoordinateType = CoordinateTypes.Regular;
            IndexMode = true;
            if (Header.ShapeType == ShapeTypes.PolyLineM)
            {
                CoordinateType = CoordinateTypes.M;
            }
            if (Header.ShapeType == ShapeTypes.PolyLineZ)
            {
                CoordinateType = CoordinateTypes.Z;
            }
            base.Envelope = Header.ToEnvelope(); 
            Name = Path.GetFileNameWithoutExtension(filename);
            Attributes.Open(filename);
            FillLines(filename, progressHandler);
            ReadProjection();
        }

        // X Y Poly Lines
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 3        Integer     1           Little
        // Byte 12      Xmin                Double      1           Little
        // Byte 20      Ymin                Double      1           Little
        // Byte 28      Xmax                Double      1           Little
        // Byte 36      Ymax                Double      1           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little


        // X Y M Poly Lines
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 23       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y*      Mmin                Double      1           Little
        // Byte Y + 8*  Mmax                Double      1           Little
        // Byte Y + 16* Marray              Double      NumPoints   Little
       
       
        
        // X Y Z M Poly Lines
        // ---------------------------------------------------------
        // Position     Value               Type        Number  Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big       
        // Byte 8       Shape Type 13       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y       Zmin                Double      1           Little
        // Byte Y + 8   Zmax                Double      1           Little
        // Byte Y + 16  Zarray              Double      NumPoints   Little
        // Byte Z*      Mmin                Double      1           Little
        // Byte Z+8*    Mmax                Double      1           Little
        // Byte Z+16*   Marray              Double      NumPoints   Little

      
        /// <summary>
        /// Gets the specified feature by constructing it from the vertices, rather
        /// than requiring that all the features be created. (which takes up a lot of memory).
        /// </summary>
        /// <param name="index">The integer index</param>
        public override IFeature GetFeature(int index)
        {
            IFeature f = GetLine(index);
            f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            return f;
        }

       

        private void FillLines(string filename, IProgressHandler progressHandler)
        {
           // Check to ensure the filename is not null
            if (filename == null)
            {
                throw new NullReferenceException(MessageStrings.ArgumentNull_S.Replace("%S", filename));
            }

            if (File.Exists(filename) == false)
            {
                throw new FileNotFoundException(MessageStrings.FileNotFound_S.Replace("%S", filename));
            }

            // Get the basic header information.  
            ShapefileHeader header = new ShapefileHeader(filename);
            Extent = new Extent(new[] { header.Xmin, header.Ymin, header.Xmax, header.Ymax });
            // Check to ensure that the filename is the correct shape type
            if (header.ShapeType != ShapeTypes.PolyLine &&
                 header.ShapeType != ShapeTypes.PolyLineM &&
                 header.ShapeType != ShapeTypes.PolyLineZ)
            {
                throw new ArgumentException(MessageStrings.FileNotLines_S.Replace("%S", filename));

            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(filename);


            // This will set up a reader so that we can read values in huge chunks, which is much faster than one value at a time.
            IO.BufferedBinaryReader bbReader = new IO.BufferedBinaryReader(filename, progressHandler);

            if (bbReader.FileLength == 100)
            {
                // We have reached the end of the file so we can close the file
                bbReader.Close();
                return;
            }

            // Skip the shapefile header by skipping the first 100 bytes in the shapefile
            bbReader.Seek(100, SeekOrigin.Begin);

            int numShapes = shapeHeaders.Count;

           
            int[] partOffsets = new int[numShapes];
            byte[] bigEndians = new byte[numShapes * 8];
            byte[] allBounds = new byte[numShapes*32];
            ByteBlock allParts = new ByteBlock(BLOCKSIZE); // probably all will be in one block, but use a byteBlock just in case.
            ByteBlock allCoords = new ByteBlock(BLOCKSIZE);
            bool isM = (header.ShapeType == ShapeTypes.PolyLineM || header.ShapeType == ShapeTypes.PolyLineZ);
            bool isZ = (header.ShapeType == ShapeTypes.PolyLineZ);
            ByteBlock allZ = null;
            ByteBlock allM = null;
            if(isZ)
            {
                allZ = new ByteBlock(BLOCKSIZE);
            }
            if(isM)
            {
                allM = new ByteBlock(BLOCKSIZE); 
            }
           
            int pointOffset = 0;
            for (int shp = 0; shp < numShapes; shp++)
            {

                // Read from the index file because some deleted records
                // might still exist in the .shp file.
                long offset = (shapeHeaders[shp].ByteOffset);
                bbReader.Seek(offset, SeekOrigin.Begin);
               
                // time: 200 ms
                ShapeRange shape = new ShapeRange(FeatureTypes.Line);
                
                                                                       // Position     Value               Type        Number      Byte Order
                                                                       //-------------------------------------------------------------------- 


                
                shape.RecordNumber = bbReader.ReadInt32(false);       // Byte 0       Record Number       Integer     1           Big
                
                shape.ContentLength = bbReader.ReadInt32(false);      // Byte 4       Content Length      Integer     1           Big

                //bbReader.Read(bigEndians, shp*8, 8);
                //byte[] cl = new byte[4];
                //Array.Copy(bigEndians, shp * 8 + 4, cl, 0, 4);
                //if (BitConverter.IsLittleEndian) Array.Reverse(cl);
                //shape.ContentLength = BitConverter.ToInt32(cl, 0);
                shape.ShapeType = (ShapeTypes)bbReader.ReadInt32();     // Byte 8       Shape Type          Integer     1           Little
                shape.StartIndex = pointOffset;
                if (shape.ShapeType == ShapeTypes.NullShape)
                {
                    continue;
                }
                bbReader.Read(allBounds, shp*32, 32);
                //double xMin = bbReader.ReadDouble();                  // Byte 12      Xmin                Double      1           Little
               // double yMin = bbReader.ReadDouble();                  // Byte 20      Ymin                Double      1           Little
               // double xMax = bbReader.ReadDouble();                  // Byte 28      Xmax                Double      1           Little
               // double yMax = bbReader.ReadDouble();                  // Byte 36      Ymax                Double      1           Little

                shape.NumParts = bbReader.ReadInt32();                    // Byte 44      NumParts            Integer     1           Little
                shape.NumPoints = bbReader.ReadInt32();                 // Byte 48      NumPoints           Integer     1           Little
                
                partOffsets[shp] = allParts.IntOffset();
                allParts.Read(shape.NumParts * 4, bbReader);
                
                allCoords.Read(shape.NumPoints * 16, bbReader);
                
                pointOffset += shape.NumPoints;

                //double mMin;
                //double mMax;
                if (header.ShapeType == ShapeTypes.PolyLineM)
                {
                    // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (shape.ContentLength * 2 > 44 + 4 * shape.NumParts + 16 * shape.NumPoints)
                    {
                        //mMin = bbReader.ReadDouble();
                        //mMax = bbReader.ReadDouble();
                        bbReader.Seek(16, SeekOrigin.Current);
                        if(allM != null)allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
                if (header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    bool hasM = shape.ContentLength * 2 > 60 + 4 * shape.NumParts + 24 * shape.NumPoints;
                    double zMin = bbReader.ReadDouble();
                    double zMax = bbReader.ReadDouble();
                    //bbReader.Seek(16, SeekOrigin.Current);
                    // For Z shapefiles, the Z part is not optional.
                    if(allZ != null)allZ.Read(shape.NumPoints * 8, bbReader);

                    // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (hasM)
                    {
                        double mMin = bbReader.ReadDouble();
                        double mMax = bbReader.ReadDouble();
                        //bbReader.Seek(16, SeekOrigin.Current);
                        if(allM != null) allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
               
                
                // Now that we have read all the values, create the geometries from the points and parts arrays.
                ShapeIndices.Add(shape); 
            }
            double[] vert = allCoords.ToDoubleArray();
            Vertex = vert;
            if (isM) M = allM.ToDoubleArray();
            if (isZ) Z = allZ.ToDoubleArray();
            //Array.Reverse(bigEndians);
            //int[] bigInts = new int[numShapes*2];
            //Buffer.BlockCopy(bigEndians, 0, bigInts, 0, bigEndians.Length);
            List<ShapeRange> shapes = ShapeIndices;
            double[] bounds = new double[numShapes * 4];
            Buffer.BlockCopy(allBounds, 0, bounds, 0, allBounds.Length);
            int[] parts = allParts.ToIntArray();
            for (int shp = 0; shp < numShapes; shp++)
            {
                ShapeRange shape = shapes[shp];
                //shape.RecordNumber = bigInts[numShapes * 2 - shp * 2 - 2];
                //shape.ContentLength = bigInts[numShapes * 2 - shp * 2 - 1];
                shape.Extent = new Extent(bounds, shp * 4);
                for (int part = 0; part < shape.NumParts; part++)
                {
                    int offset = partOffsets[shp];
                    int endIndex = shape.NumPoints + shape.StartIndex;
                    int startIndex = parts[offset + part] + shape.StartIndex;
                    if (part < shape.NumParts - 1)
                    {
                        int prt = parts[offset + part + 1];
                        endIndex = prt + shape.StartIndex;
                    }
                    int count = endIndex - startIndex;
                    PartRange partR = new PartRange(vert, shape.StartIndex, parts[offset + part], FeatureTypes.Line);
                    partR.NumVertices = count;
                    shape.Parts.Add(partR);
                }
            }
            GC.Collect();
            bbReader.Dispose();
        }

       

        /// <summary>
        /// Saves the file to a new location
        /// </summary>
        /// <param name="filename">The filename to save</param>
        /// <param name="overwrite">Boolean that specifies whether or not to overwrite the existing file</param>
        public override void SaveAs(string filename, bool overwrite)
        {
            if (IndexMode)
            {
                SaveAsIndexed(filename, overwrite);
                return;
            }
            Filename = filename;
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir))
            {
                if (MessageBox.Show("Directory " + dir + " does not exist.  Do you want to create it?", "Create Directory?", MessageBoxButtons.YesNo) != DialogResult.OK)
                    return;
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(filename) && filename != Filename && overwrite == false)
            {
                if (MessageBox.Show("The file already exists.  Do you wish to overwrite it?", "File Exists", MessageBoxButtons.YesNo) == DialogResult.No) return;
                File.Delete(filename);
            }
            InvalidateEnvelope();
            Header.Xmin = Envelope.Minimum.X;
            Header.Xmax = Envelope.Maximum.X;
            Header.Ymin = Envelope.Minimum.Y;
            Header.Ymax = Envelope.Maximum.Y;
            if (CoordinateType == CoordinateTypes.Regular)
            {
                Header.ShapeType = ShapeTypes.PolyLine;
            }
            if (CoordinateType == CoordinateTypes.M)
            {
                Header.ShapeType = ShapeTypes.PolyLineM;
            }
            if (CoordinateType == CoordinateTypes.Z)
            {
                Header.ShapeType = ShapeTypes.PolyLineZ;
            }
            if (Header.ShapeType == ShapeTypes.PolyLineZ)
            {
                Header.Zmin = Envelope.Minimum.Z;
                Header.Zmax = Envelope.Maximum.Z;
            }
            if (Header.ShapeType == ShapeTypes.PolyLineM || Header.ShapeType == ShapeTypes.PolyLineZ)
            {
                Header.Mmin = Envelope.Minimum.M;
                Header.Mmax = Envelope.Maximum.M;
            }
            

            Header.ShxLength = Features.Count * 4 + 50;
            Header.SaveAs(filename);
            

            IO.BufferedBinaryWriter bbWriter = new IO.BufferedBinaryWriter(filename);
            IO.BufferedBinaryWriter indexWriter = new IO.BufferedBinaryWriter(Header.ShxFilename);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;
            
            foreach (IFeature f in Features)
            {
                List<int> parts = new List<int>();
                offset += contentLength; // adding the previous content length from each loop calculates the word offset
                List<Coordinate> points = new List<Coordinate>();
                contentLength = 22;
                for (int iPart = 0; iPart < f.NumGeometries; iPart++)
                {
                    parts.Add(points.Count);
                    IBasicLineString bl = f.GetBasicGeometryN(iPart) as IBasicLineString;
                    if (bl == null) continue;
                    foreach (Coordinate coord in bl.Coordinates)
                    {
                        points.Add(coord);
                    }
                }
                contentLength += 2 * parts.Count;
                if (Header.ShapeType == ShapeTypes.PolyLine)
                {
                    contentLength += points.Count * 8; // x, y
                }
                if (Header.ShapeType == ShapeTypes.PolyLineM)
                {
                    contentLength += 8; // mmin mmax
                    contentLength += points.Count * 12; // x, y, m
                }
                if (Header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += points.Count * 16; // x, y, z, m
                } 

                //                                              Index File
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                indexWriter.Write(offset, false);               // Byte 0     Offset             Integer     1           Big                 
                indexWriter.Write(contentLength, false);        // Byte 4     Content Length     Integer     1           Big

                //                                              X Y Poly Lines
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                bbWriter.Write(fid+1, false);                  // Byte 0       Record Number       Integer     1           Big
                bbWriter.Write(contentLength, false);        // Byte 4       Content Length      Integer     1           Big
                bbWriter.Write((int)Header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
                if (Header.ShapeType == ShapeTypes.NullShape)
                {
                    continue;
                }
                IEnvelope env = f.Envelope ?? new Envelope();
                bbWriter.Write(env.Minimum.X);             // Byte 12      Xmin                Double      1           Little
                bbWriter.Write(env.Minimum.Y);             // Byte 20      Ymin                Double      1           Little
                bbWriter.Write(env.Maximum.X);             // Byte 28      Xmax                Double      1           Little
                bbWriter.Write(env.Maximum.Y);             // Byte 36      Ymax                Double      1           Little
                bbWriter.Write(parts.Count);                 // Byte 44      NumParts            Integer     1           Little
                bbWriter.Write(points.Count);                // Byte 48      NumPoints           Integer     1           Little
                foreach (int iPart in parts)                 // Byte 52      Parts               Integer     NumParts    Little 
                {
                    bbWriter.Write(iPart);
                }

                double[] xyVals = new double[points.Count * 2];
                for(int ipoint = 0; ipoint < points.Count; ipoint++)      // Byte X       Points              Point       NumPoints   Little
                {
                    double[] c = points[ipoint].ToArray();
                    xyVals[ipoint * 2] = c[0];
                    xyVals[ipoint * 2 + 1] = c[1];
                }
                bbWriter.Write(xyVals);
                if (Header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    if (f.Envelope != null)
                    {
                        bbWriter.Write(f.Envelope.Minimum.Z);
                        bbWriter.Write(f.Envelope.Maximum.Z);
                    }
                    double[] zVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; ipoint++)      // Byte X       Points              Point       NumPoints   Little
                    {
                        zVals[ipoint] = points[ipoint].Z;
                    }
                    bbWriter.Write(zVals);
                }

                if (Header.ShapeType == ShapeTypes.PolyLineM || Header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    if(f.Envelope == null)
                    {
                        bbWriter.Write(0.0);
                        bbWriter.Write(0.0);
                    }
                    else
                    {
                        bbWriter.Write(f.Envelope.Minimum.M);
                        bbWriter.Write(f.Envelope.Maximum.M); 
                    }
                    
                    double[] mVals = new double[points.Count];
                    for (int ipoint = 0; ipoint < points.Count; ipoint++)      // Byte X       Points              Point       NumPoints   Little
                    {
                        mVals[ipoint] = points[ipoint].M;
                    }
                    bbWriter.Write(mVals);
                }
              
                fid++;
                offset += 4; // header bytes
            }
           
            bbWriter.Close();
            indexWriter.Close();
            
            offset += contentLength;
            //offset += 4;
            WriteFileLength(Filename, offset);
            WriteFileLength(Header.ShxFilename, 50 + fid * 4);
            UpdateAttributes();
            SaveProjection();
        }

        /// <summary>
        /// Saves the file to a new location
        /// </summary>
        /// <param name="filename">The filename to save</param>
        /// <param name="overwrite">Boolean that specifies whether or not to overwrite the existing file</param>
        private void SaveAsIndexed(string filename, bool overwrite)
        {
            Filename = filename;
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir))
            {
                if (MessageBox.Show("Directory " + dir + " does not exist.  Do you want to create it?", "Create Directory?", MessageBoxButtons.YesNo) != DialogResult.OK)
                    return;
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(filename) && filename != Filename && overwrite == false)
            {
                if (MessageBox.Show("The file already exists.  Do you wish to overwrite it?", "File Exists", MessageBoxButtons.YesNo) == DialogResult.No) return;
                File.Delete(filename);
            }
            InvalidateEnvelope();
            Header.Xmin = Envelope.Minimum.X;
            Header.Xmax = Envelope.Maximum.X;
            Header.Ymin = Envelope.Minimum.Y;
            Header.Ymax = Envelope.Maximum.Y;
            if (CoordinateType == CoordinateTypes.Regular)
            {
                Header.ShapeType = ShapeTypes.PolyLine;
            }
            if (CoordinateType == CoordinateTypes.M)
            {
                Header.ShapeType = ShapeTypes.PolyLineM;
            }
            if (CoordinateType == CoordinateTypes.Z)
            {
                Header.ShapeType = ShapeTypes.PolyLineZ;
            }
            if (Header.ShapeType == ShapeTypes.PolygonZ)
            {
                Header.Zmin = Z.Min();
                Header.Zmax = Z.Max();
            }
            if (Header.ShapeType == ShapeTypes.PolygonM || Header.ShapeType == ShapeTypes.PolygonZ)
            {
                Header.Mmin = M.Min();
                Header.Mmax = M.Max();
            }

            Header.ShxLength = ShapeIndices.Count * 4 + 50;
            Header.SaveAs(filename);

            FileStream shpStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
            FileStream shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;

            foreach (ShapeRange shape in ShapeIndices)
            {

                offset += contentLength; // adding the previous content length from each loop calculates the word offset
             
                contentLength = 22;
                contentLength += 2 * shape.Parts.Count;
                if (Header.ShapeType == ShapeTypes.PolyLine)
                {
                    contentLength += shape.NumPoints * 8; // x, y
                }
                if (Header.ShapeType == ShapeTypes.PolyLineM)
                {
                    contentLength += 8; // mmin mmax
                    contentLength += shape.NumPoints * 12; // x, y, m
                }
                if (Header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += shape.NumPoints * 16; // x, y, z, m
                }

                //                                              Index File
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shxStream.WriteBe(offset);                     // Byte 0     Offset             Integer     1           Big                 
                shxStream.WriteBe(contentLength);              // Byte 4     Content Length     Integer     1           Big

                //                                              X Y Poly Lines
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shpStream.WriteBe(fid + 1);                     // Byte 0       Record Number       Integer     1           Big
                shpStream.WriteBe(contentLength);               // Byte 4       Content Length      Integer     1           Big
                shpStream.WriteLe((int)Header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
                if (Header.ShapeType == ShapeTypes.NullShape)
                {
                    continue;
                }
                shpStream.WriteLe(shape.Extent.XMin);             // Byte 12      Xmin                Double      1           Little
                shpStream.WriteLe(shape.Extent.YMin);             // Byte 20      Ymin                Double      1           Little
                shpStream.WriteLe(shape.Extent.XMax);             // Byte 28      Xmax                Double      1           Little
                shpStream.WriteLe(shape.Extent.YMax);             // Byte 36      Ymax                Double      1           Little
                shpStream.WriteLe(shape.NumParts);                 // Byte 44      NumParts            Integer     1           Little
                shpStream.WriteLe(shape.NumPoints);                // Byte 48      NumPoints           Integer     1           Little
                foreach (PartRange part in shape.Parts)                 // Byte 52      Parts               Integer     NumParts    Little 
                {

                    shpStream.WriteLe(part.PartOffset);
                }
                int start = shape.StartIndex;
                int count = shape.NumPoints;
                shpStream.WriteLe(Vertex, start * 2, count * 2);            // Byte X       Points              Point       NumPoints   Little

                
                if (Header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    double[] shapeZ = new double[count];
                    Array.Copy(Z, start, shapeZ, 0, count);
                    shpStream.WriteLe(shapeZ.Min());
                    shpStream.WriteLe(shapeZ.Max());
                    shpStream.WriteLe(Z, start, count);
                }

                if (Header.ShapeType == ShapeTypes.PolyLineM || Header.ShapeType == ShapeTypes.PolyLineZ)
                {
                    if (M != null && M.Length >= start + count)
                    {
                        double[] shapeM = new double[count];
                        Array.Copy(M, start, shapeM, 0, count);
                        shpStream.WriteLe(shapeM.Min());
                        shpStream.WriteLe(shapeM.Max());
                        shpStream.WriteLe(M, start, count);
                    }
                }

                fid++;
                offset += 4; // header bytes
            }

            shpStream.Flush();
            shxStream.Flush();
            shpStream.Close();
            shxStream.Close();

            offset += contentLength;
            //offset += 4;
            WriteFileLength(Filename, offset);
            WriteFileLength(Header.ShxFilename, 50 + fid * 4);
            UpdateAttributes();
            SaveProjection();
        }
    }
}
