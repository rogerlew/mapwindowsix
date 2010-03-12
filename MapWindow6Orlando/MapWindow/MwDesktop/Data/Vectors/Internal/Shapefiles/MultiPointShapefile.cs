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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MapWindow.Geometries;
using MapWindow.Main;
namespace MapWindow.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where each features has multiple points 
    /// </summary>
    public class MultiPointShapefile:Shapefile
    {
        private const int BLOCKSIZE = 16000000;
        private readonly GeometryFactory _geometryFactory;
      
        /// <summary>
        /// Creates a new instance of a MultiPointShapefile for in-ram handling only.
        /// </summary>
        public MultiPointShapefile():base(FeatureTypes.MultiPoint)
        {
            _geometryFactory = new GeometryFactory();
            Attributes = new AttributeTable();
            Header = new ShapefileHeader();
            Header.FileLength = 100;
            Header.ShapeType = ShapeTypes.MultiPoint;

        }

		/// <summary>
		/// Creates a new instance of a MultiPointShapefile that is loaded from the supplied filename.
		/// </summary>
		/// <param name="filename">The string filename of the polygon shapefile to load</param>
		public MultiPointShapefile(string filename) : this()
		{
			Open(filename, null);
		}

        public override IFeature GetFeature(int index)
        {
            IFeature f = GetMultiPoint(index);
            f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            return f;
        }

        /// <summary>
        /// Opens a shapefile
        /// </summary>
        /// <param name="filename">The string filename of the point shapefile to load</param>
        /// <param name="progressHandler">Any valid implementation of the MapWindow.Main.IProgressHandler</param>
        public void Open(string filename, IProgressHandler progressHandler)
        {
            IndexMode = true;
            Filename = filename;
            FeatureType = FeatureTypes.MultiPoint;
            Header = new ShapefileHeader(filename);
            CoordinateType = CoordinateTypes.Regular;
            if (Header.ShapeType == ShapeTypes.MultiPointM)
            {
                CoordinateType = CoordinateTypes.M;
            }
            if (Header.ShapeType == ShapeTypes.MultiPointZ)
            {
                CoordinateType = CoordinateTypes.Z;
            }
            Name = Path.GetFileNameWithoutExtension(filename);
            Attributes.Open(filename);
            FillPoints(filename, progressHandler);
            ReadProjection();
        }

        // X Y MultiPoints: Total Length = 28 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 8        Integer     1           Little
        // Byte 12      Xmin                Double      1           Little
        // Byte 20      Ymin                Double      1           Little
        // Byte 28      Xmax                Double      1           Little
        // Byte 36      Ymax                Double      1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte X       Points              Point       NumPoints   Little


        // X Y M MultiPoints: Total Length = 34 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 28       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumPoints           Integer     1           Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y*      Mmin                Double      1           Little
        // Byte Y + 8*  Mmax                Double      1           Little
        // Byte Y + 16* Marray              Double      NumPoints   Little
       
       
        
        // X Y Z M MultiPoints: Total Length = 44 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number  Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big       
        // Byte 8       Shape Type 18       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumPoints           Integer     1           Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y       Zmin                Double      1           Little
        // Byte Y + 8   Zmax                Double      1           Little
        // Byte Y + 16  Zarray              Double      NumPoints   Little
        // Byte Z*      Mmin                Double      1           Little
        // Byte Z+8*    Mmax                Double      1           Little
        // Byte Z+16*   Marray              Double      NumPoints   Little


        private void FillPoints(string filename, IProgressHandler progressHandler)
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
            if (header.ShapeType != ShapeTypes.MultiPoint &&
                 header.ShapeType != ShapeTypes.MultiPointM &&
                 header.ShapeType != ShapeTypes.MultiPointZ)
            {
                throw new ArgumentException(MessageStrings.FileNotLines_S.Replace("%S", filename));

            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(filename);


            // This will set up a reader so that we can read values in huge chunks, which is much faster than one value at a time.
            IO.BufferedBinaryReader bbReader = new IO.BufferedBinaryReader(filename, progressHandler);

            if (bbReader.FileLength == 100)
            {
                // The shapefile is empty so we can simply return here
                bbReader.Close();
                return;
            }


            // Skip the shapefile header by skipping the first 100 bytes in the shapefile
            bbReader.Seek(100, SeekOrigin.Begin);

            int numShapes = shapeHeaders.Count;

            byte[] bigEndians = new byte[numShapes * 8];
            byte[] allBounds = new byte[numShapes * 32];
        
            ByteBlock allCoords = new ByteBlock(BLOCKSIZE);
            bool isM = (header.ShapeType == ShapeTypes.MultiPointZ || header.ShapeType == ShapeTypes.MultiPointM);
            bool isZ = (header.ShapeType == ShapeTypes.PolyLineZ);
            ByteBlock allZ = null;
            ByteBlock allM = null;
            if (isZ)
            {
                allZ = new ByteBlock(BLOCKSIZE);
            }
            if (isM)
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
                ShapeRange shape = new ShapeRange(FeatureTypes.MultiPoint);


                shape.RecordNumber = bbReader.ReadInt32(false);       // Byte 0       Record Number       Integer     1           Big
                shape.ContentLength = bbReader.ReadInt32(false);      // Byte 4       Content Length      Integer     1           Big
                //bbReader.Read(bigEndians, shp * 8, 8);
                shape.ShapeType = (ShapeTypes)bbReader.ReadInt32();     
                shape.StartIndex = pointOffset;
                if (shape.ShapeType == ShapeTypes.NullShape)
                {
                    continue;
                }
                bbReader.Read(allBounds, shp * 32, 32);
                shape.NumParts = 1;                                    
                shape.NumPoints = bbReader.ReadInt32();                 
                allCoords.Read(shape.NumPoints * 16, bbReader);
                pointOffset += shape.NumPoints;

                if (header.ShapeType == ShapeTypes.MultiPointM)
                {
                    // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (shape.ContentLength * 2 > 44 + 4 * shape.NumParts + 16 * shape.NumPoints)
                    {
                        double mMin = bbReader.ReadDouble();
                        double mMax = bbReader.ReadDouble();
                        //bbReader.Seek(16, SeekOrigin.Current);
                        if (allM != null) allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
                if (header.ShapeType == ShapeTypes.MultiPointZ)
                {
                    bool hasM = shape.ContentLength * 2 > 60 + 4 * shape.NumParts + 24 * shape.NumPoints;
                    double zMin = bbReader.ReadDouble();
                    double zMax = bbReader.ReadDouble();
                    // For Z shapefiles, the Z part is not optional.
                    if (allZ != null) allZ.Read(shape.NumPoints * 8, bbReader);

                    // These are listed as "optional" but there isn't a good indicator of how to determine if they were added.
                    // To handle the "optional" M values, check the contentLength for the feature.
                    // The content length does not include the 8-byte record header and is listed in 16-bit words.
                    if (hasM)
                    {
                        double mMin = bbReader.ReadDouble();
                        double mMax = bbReader.ReadDouble();
                        
                        if (allM != null) allM.Read(shape.NumPoints * 8, bbReader);
                    }
                }
                // Now that we have read all the values, create the geometries from the points and parts arrays.
                ShapeIndices.Add(shape);
            }
            double[] vert = allCoords.ToDoubleArray();
            Vertex = vert;
            if (isM) M = allM.ToDoubleArray();
            if (isZ) Z = allZ.ToDoubleArray();
            Array.Reverse(bigEndians);
            List<ShapeRange> shapes = ShapeIndices;
            double[] bounds = new double[numShapes * 4];
            Buffer.BlockCopy(allBounds, 0, bounds, 0, allBounds.Length);
            for (int shp = 0; shp < numShapes; shp++)
            {
                ShapeRange shape = shapes[shp];
                shape.Extent = new Extent(bounds, shp * 4);
                int endIndex = shape.NumPoints + shape.StartIndex;
                int startIndex = shape.StartIndex;
                int count = endIndex - startIndex;
                PartRange partR = new PartRange(vert, shape.StartIndex, 0, FeatureTypes.MultiPoint);
                partR.NumVertices = count;
                shape.Parts.Add(partR);
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
            if(IndexMode)
            {
                SaveAsIndexed(filename, overwrite);
                return;
            }
            if (File.Exists(filename) && filename != Filename && overwrite == false)
            {
                if (MessageBox.Show("The file already exists.  Do you wish to overwrite it?", "File Exists", MessageBoxButtons.YesNo) == DialogResult.No) return;
                File.Delete(filename);
            }
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir))
            {
                if (MessageBox.Show("Directory " + dir + " does not exist.  Do you want to create it?", "Create Directory?", MessageBoxButtons.YesNo) != DialogResult.OK)
                    return;
                Directory.CreateDirectory(dir);
            }
            InvalidateEnvelope();
            Header.Xmin = Envelope.Minimum.X;
            Header.Xmax = Envelope.Maximum.X;
            Header.Ymin = Envelope.Minimum.Y;
            Header.Ymax = Envelope.Maximum.Y;
            if (CoordinateType == CoordinateTypes.Regular)
            {
                Header.ShapeType = ShapeTypes.MultiPoint;
            }
            if (CoordinateType == CoordinateTypes.M)
            {
                Header.ShapeType = ShapeTypes.MultiPointM;
            }
            if (CoordinateType == CoordinateTypes.Z)
            {
                Header.ShapeType = ShapeTypes.MultiPointZ;
            }
            
            if (Header.ShapeType == ShapeTypes.MultiPoint || Header.ShapeType == ShapeTypes.MultiPointM)
            {
                // test to see if the coordinates have added z or m values in the first coordinate
                Coordinate c = Features[0].BasicGeometry.Coordinates[0];
                if (!double.IsNaN(c.Z))
                {
                    Header.ShapeType = ShapeTypes.MultiPointZ;
                }
            }
            if (Header.ShapeType == ShapeTypes.MultiPointZ)
            {
                Header.Zmin = Envelope.Minimum.Z;
                Header.Zmax = Envelope.Maximum.Z;
            }
            if (Header.ShapeType == ShapeTypes.MultiPointM || Header.ShapeType == ShapeTypes.MultiPointZ)
            {
                Header.Mmin = Envelope.Minimum.M;
                Header.Mmax = Envelope.Maximum.M;
            }
            

            
            Header.ShxLength = 50 + 4 * Features.Count;
            Header.SaveAs(filename);

            IO.BufferedBinaryWriter bbWriter = new IO.BufferedBinaryWriter(filename);
            IO.BufferedBinaryWriter indexWriter = new IO.BufferedBinaryWriter(Header.ShxFilename);
            int fid = 0;
            int offset = 50; // the shapefile header starts at 100 bytes, so the initial offset is 50 words
            int contentLength = 0;
            ProgressMeter = new ProgressMeter(ProgressHandler, "Saving (Not Indexed)...", Features.Count);
            foreach (IFeature f in Features)
            {
                offset += contentLength; // adding the previous content length from each loop calculates the word offset
                List<Coordinate> points = new List<Coordinate>();
                contentLength = 20;
                for (int iPart = 0; iPart < f.NumGeometries; iPart++)
                {
                    IList<Coordinate> coords = f.BasicGeometry.GetBasicGeometryN(iPart).Coordinates;
                    foreach (Coordinate coord in coords)
                    {
                        points.Add(coord);
                    }
                }

                if (Header.ShapeType == ShapeTypes.MultiPoint)
                {

                    contentLength += points.Count * 8;
                }
                if (Header.ShapeType == ShapeTypes.MultiPointM)
                {
                    contentLength += 8; // mmin, mmax
                    contentLength += points.Count * 12;
                }
                if (Header.ShapeType == ShapeTypes.MultiPointZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += points.Count * 16;
                }

                //                                              Index File
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                indexWriter.Write(offset, false);            // Byte 0       Offset       Integer     1           Big                 
                indexWriter.Write(contentLength, false);     // Byte 4       Content Length       Integer     1           Big

                //                                              X Y Poly Lines
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                bbWriter.Write(fid+1, false);                // Byte 0       Record Number       Integer     1           Big
                bbWriter.Write(contentLength, false);        // Byte 4       Content Length      Integer     1           Big
                bbWriter.Write((int)Header.ShapeType);       // Byte 8       Shape Type 3        Integer     1           Little
                if (Header.ShapeType == ShapeTypes.NullShape)
                {
                    continue;
                }
                bbWriter.Write(f.Envelope.Minimum.X);             // Byte 12      Xmin                Double      1           Little
                bbWriter.Write(f.Envelope.Minimum.Y);             // Byte 20      Ymin                Double      1           Little
                bbWriter.Write(f.Envelope.Maximum.X);             // Byte 28      Xmax                Double      1           Little
                bbWriter.Write(f.Envelope.Maximum.Y);             // Byte 36      Ymax                Double      1           Little

                bbWriter.Write(points.Count);                // Byte 44      NumPoints           Integer     1           Little
             

                foreach (Coordinate coord in points)        // Byte X       Points              Point       NumPoints   Little
                {
                    bbWriter.Write(coord.X);
                    bbWriter.Write(coord.Y);
                    //if (Header.ShapeType == ShapeTypes.MultiPointZ)
                    //{
                    //    bbWriter.Write(coord.Z);
                    //}
                }

                if (Header.ShapeType == ShapeTypes.MultiPointZ)
                {
                    bbWriter.Write(f.Envelope.Minimum.Z);
                    bbWriter.Write(f.Envelope.Maximum.Z);
                    foreach (Coordinate coord in points)        // Byte X       Points              Point       NumPoints   Little
                    {
                        bbWriter.Write(coord.Z);
                    }
                }
                if (Header.ShapeType == ShapeTypes.MultiPointM || Header.ShapeType == ShapeTypes.MultiPointZ)
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
                    foreach (Coordinate coord in points)        // Byte X       Points              Point       NumPoints   Little
                    {
                        bbWriter.Write(coord.M);
                    }
                }

                ProgressMeter.CurrentValue = fid;
                fid++;
                offset += 4;
            }
            ProgressMeter.Reset();
            bbWriter.Close();
            indexWriter.Close();

            offset += contentLength;
            WriteFileLength(Filename, offset);
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

            if (File.Exists(filename) && filename != Filename && overwrite == false)
            {
                if (MessageBox.Show("The file already exists.  Do you wish to overwrite it?", "File Exists", MessageBoxButtons.YesNo) == DialogResult.No) return;
                File.Delete(filename);
            }
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir))
            {
                if (MessageBox.Show("Directory " + dir + " does not exist.  Do you want to create it?", "Create Directory?", MessageBoxButtons.YesNo) != DialogResult.OK)
                    return;
                Directory.CreateDirectory(dir);
            }
            Header.Xmin = Envelope.Minimum.X;
            Header.Xmax = Envelope.Maximum.X;
            Header.Ymin = Envelope.Minimum.Y;
            Header.Ymax = Envelope.Maximum.Y;
            if (CoordinateType == CoordinateTypes.Regular)
            {
                Header.ShapeType = ShapeTypes.MultiPoint;
            }
            if (CoordinateType == CoordinateTypes.M)
            {
                Header.ShapeType = ShapeTypes.MultiPointM;
            }
            if (CoordinateType == CoordinateTypes.Z)
            {
                Header.ShapeType = ShapeTypes.MultiPointZ;
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
               
                contentLength = 20;
                

                if (Header.ShapeType == ShapeTypes.MultiPoint)
                {

                    contentLength += shape.NumPoints * 8;
                }
                if (Header.ShapeType == ShapeTypes.MultiPointM)
                {
                    contentLength += 8; // mmin, mmax
                    contentLength += shape.NumPoints * 12;
                }
                if (Header.ShapeType == ShapeTypes.MultiPointZ)
                {
                    contentLength += 16; // mmin, mmax, zmin, zmax
                    contentLength += shape.NumPoints * 16;
                }

                //                                              Index File
                //                                              ---------------------------------------------------------
                //                                              Position     Value               Type        Number      Byte Order
                //                                              ---------------------------------------------------------
                shxStream.WriteBe(offset);                      // Byte 0       Offset       Integer     1           Big                 
                shxStream.WriteBe(contentLength);               // Byte 4       Content Length       Integer     1           Big

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
                shpStream.WriteLe(shape.NumPoints);                // Byte 44      NumPoints           Integer     1           Little



                int start = shape.StartIndex;
                int count = shape.NumPoints;
                shpStream.WriteLe(Vertex, start * 2, count * 2);            // Byte X       Points              Point       NumPoints   Little

                if (Header.ShapeType == ShapeTypes.MultiPointZ)
                {
                    double[] shapeZ = new double[count];
                    Array.Copy(Z, start, shapeZ, 0, count);
                    shpStream.WriteLe(shapeZ.Min());
                    shpStream.WriteLe(shapeZ.Max());
                    shpStream.WriteLe(Z, start, count);
                }
                if (Header.ShapeType == ShapeTypes.MultiPointM || Header.ShapeType == ShapeTypes.MultiPointZ)
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
                offset += 4;
            }

            shpStream.Flush();
            shxStream.Flush();
            shpStream.Close();
            shxStream.Close();

            offset += contentLength;
            WriteFileLength(Filename, offset);
            UpdateAttributes();
            SaveProjection();
        }


    }
}
