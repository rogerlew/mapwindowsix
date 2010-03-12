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
// The Initial Developer of this Original Code is Ted Dunsford. Created in January, 2008.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MapWindow.Geometries;
using MapWindow.Main;

namespace MapWindow.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the datatype is point
    /// </summary>
    public class PointShapefile:Shapefile
    {
        
        /// <summary>
        /// Creates a new instance of a PointShapefile for in-ram handling only.
        /// </summary>
        public PointShapefile() : base(FeatureTypes.Point)
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader();
            Header.FileLength = 100;
            Header.ShapeType = ShapeTypes.Point;
         
        }

		/// <summary>
		/// Creates a new instance of a PointShapefile that is loaded from the supplied filename.
		/// </summary>
		/// <param name="filename">The string filename of the polygon shapefile to load</param>
		public PointShapefile(string filename): this()
		{
			Open(filename, null);
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
            Header = new ShapefileHeader(filename);
            CoordinateType = CoordinateTypes.Regular;
            if (Header.ShapeType == ShapeTypes.PointM)
            {
                CoordinateType = CoordinateTypes.M;
            }
            if (Header.ShapeType == ShapeTypes.PointZ)
            {
                CoordinateType = CoordinateTypes.Z;
            }
            base.Envelope = Header.ToEnvelope(); 
            Name = Path.GetFileNameWithoutExtension(filename);
            Attributes.Open(filename);
            FillPoints(filename, progressHandler);
            ReadProjection();
        }

 

        /// <summary>
        /// Obtains a typed list of ShapefilePoint structures with double values associated with the various coordinates.
        /// </summary>
        /// <param name="filename">A string filename</param>
        /// <param name="progressHandler">A progress indicator</param>
        private void FillPoints(string filename, IProgressHandler progressHandler)
        {
            

            // Check to ensure the filename is not null
            if (filename == null)
            {
                throw new NullReferenceException(MessageStrings.ArgumentNull_S.Replace("%S",filename));
            }

            if (File.Exists(filename) == false)
            {
                throw new FileNotFoundException(MessageStrings.FileNotFound_S.Replace("%S", filename));
            }

            // Reading the headers gives us an easier way to track the number of shapes and their overall length etc.
            List<ShapeHeader> shapeHeaders = ReadIndexFile(filename);

            // Get the basic header information.  
            ShapefileHeader header = new ShapefileHeader(filename);
            Extent = new Extent(new[] { header.Xmin, header.Ymin, header.Xmax, header.Ymax });
            Envelope = Extent.ToEnvelope();
            // Check to ensure that the filename is the correct shape type
            if (header.ShapeType != ShapeTypes.Point && header.ShapeType != ShapeTypes.PointM && header.ShapeType != ShapeTypes.PointZ)
            {
                throw new ApplicationException(MessageStrings.FileNotPoints_S.Replace("%S", filename));
            }

            // This will set up a reader so that we can read values in huge chunks, which is much faster than one value at a time.
            IO.BufferedBinaryReader bbReader = new IO.BufferedBinaryReader(filename, progressHandler);

            if (bbReader.FileLength == 100)
            {
                bbReader.Close();
                // the file is empty so we are done reading
                return;
            }

            // Skip the shapefile header by skipping the first 100 bytes in the shapefile
            bbReader.Seek(100, SeekOrigin.Begin);
            int numShapes = shapeHeaders.Count;
            byte[] bigEndian = new byte[numShapes * 8];
            byte[] allCoords = new byte[numShapes * 16];
            bool isM = false;
            bool isZ = false;
            if(header.ShapeType == ShapeTypes.PointM || header.ShapeType == ShapeTypes.PointZ)
            {
                isM = true;
            }
            if (header.ShapeType == ShapeTypes.PointZ)
            {
                isZ = true;
            }
            byte[] allM = new byte[8];
            if(isM) allM = new byte[numShapes * 8];
            byte[] allZ = new byte[8];
            if(isZ) allZ = new byte[numShapes * 8];
            for (int shp = 0; shp < numShapes; shp++)
            {
                // Read from the index file because some deleted records
                // might still exist in the .shp file.
                long offset = (shapeHeaders[shp].ByteOffset);
                bbReader.Seek(offset, SeekOrigin.Begin);
                bbReader.Read(bigEndian, shp * 8, 8);
                ShapeTypes type = (ShapeTypes)bbReader.ReadInt32();
                //bbReader.Seek(4, SeekOrigin.Current);
                bbReader.Read(allCoords, shp * 16, 16);
                if(isZ)
                {
                    bbReader.Read(allZ, shp*8, 8);
                }
                if (isM)
                {
                    bbReader.Read(allM, shp*8, 8);
                }
                ShapeRange shape = new ShapeRange(FeatureTypes.Point);
                shape.StartIndex = shp;
                shape.ContentLength = 8;
                shape.NumPoints = 1;
                shape.NumParts = 1;
                ShapeIndices.Add(shape);
            }
            double[] vert = new double[2 * numShapes];
            Buffer.BlockCopy(allCoords, 0, vert, 0, numShapes*16);
            Vertex = vert;
            if(isM)
            {
                double[] m = new double[numShapes];
                Buffer.BlockCopy(allM, 0, m, 0, numShapes * 8);
                M = m;
            }
            if(isZ)
            {
                double[] z = new double[numShapes];
                Buffer.BlockCopy(allZ, 0, z, 0, numShapes * 8);
                Z = z;
            }
            for (int shp = 0; shp < numShapes; shp++)
            {
                PartRange part = new PartRange(vert, shp, 0, FeatureTypes.Point);
                part.NumVertices = 1;
                ShapeRange shape = ShapeIndices[shp];
                shape.Parts.Add(part);
                shape.Extent = new Extent(new[] {vert[shp*2], vert[shp*2 + 1], vert[shp*2], vert[shp*2 + 1]});
            }

                bbReader.Dispose();
            
            
            
  
        }

        
        public override IFeature GetFeature(int index)
        {
            IFeature f = GetPoint(index);
            f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            return f;
        }

     

        /// <summary>
        /// Saves the shapefile to a different filename, but still as a shapefile.  This method does not support saving to 
        /// any other file format.
        /// </summary>
        /// <param name="filename">The string filename to save as</param>
        /// <param name="overwrite">A boolean that is true if the file should be overwritten</param>
        public override void SaveAs(string filename, bool overwrite)
        {
            Filename = filename;
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
                Header.ShapeType = ShapeTypes.Point;
            }
            if (CoordinateType == CoordinateTypes.M)
            {
                Header.ShapeType = ShapeTypes.PointM;
            }
            if (CoordinateType == CoordinateTypes.Z)
            {
                Header.ShapeType = ShapeTypes.PointZ;
            }
            if (Header.ShapeType == ShapeTypes.Point || Header.ShapeType == ShapeTypes.PointM)
            {
                // test to see if the coordinates have added z or m values in the first coordinate
                int numOrdinates = 2;
                if (!IndexMode)
                {
                    if (Features.Count > 0)
                    {
                        Coordinate c = Features[0].BasicGeometry.Coordinates[0];
                        if (!double.IsNaN(c.Z)) numOrdinates = 3;
                    }
                }
                else
                {
                    if (Z != null && Z.Length == ShapeIndices.Count)
                    {
                        numOrdinates = 3;
                    }
                }
                if (numOrdinates > 2)
                {
                    Header.ShapeType = ShapeTypes.PointZ;
                    Header.FileLength = 18 * Features.Count + 50;
                }
            } 
            if (Header.ShapeType == ShapeTypes.PointZ)
            {
                Header.Zmin = Envelope.Minimum.Z;
                Header.Zmax = Envelope.Maximum.Z;
            }
            
            if (Header.ShapeType == ShapeTypes.PointM || Header.ShapeType == ShapeTypes.PointZ)
            {
                Header.Mmin = Envelope.Minimum.M;
                Header.Mmax = Envelope.Maximum.M;
            }
            
            if(IndexMode)
            {
                Header.ShxLength = ShapeIndices.Count * 4 + 50;
                
            }
            else
            {
                Header.ShxLength = Features.Count * 4 + 50;
            }
            
            Header.SaveAs(filename);
            Stream shpStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None, 1000000);
            Stream shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 1000000);

            int len = 10;
            if (Header.ShapeType == ShapeTypes.PointM) len = 14;
            if (Header.ShapeType == ShapeTypes.PointZ) len = 18;



            // Special slightly faster writing for index mode
            if(IndexMode)
            {
                for(int shp = 0; shp < ShapeIndices.Count; shp++)
                {
                    shpStream.WriteBe(shp + 1);
                    shpStream.WriteBe(len);
                    shxStream.WriteBe(50 + shp * (len + 4));
                    shxStream.WriteBe(len);
                    shpStream.WriteLe((int)Header.ShapeType);
                    shpStream.WriteLe(Vertex[shp*2]);
                    shpStream.WriteLe(Vertex[shp*2+1]);
                    if (Z != null) shpStream.WriteLe(Z[shp]);
                    if (M != null) shpStream.WriteLe(M[shp]);
                }
            }
            else
            {
                int fid = 0;
                foreach (IFeature f in Features)
                {
                    Coordinate c = f.BasicGeometry.Coordinates[0];
                    shpStream.WriteBe(fid + 1);
                    shpStream.WriteBe(len);
                    shxStream.WriteBe(50 + fid * (len + 4));
                    shxStream.WriteBe(len);
                    shpStream.WriteLe((int)Header.ShapeType);
                    if (Header.ShapeType == ShapeTypes.NullShape)
                    {
                        continue;
                    }
                    shpStream.WriteLe(c.X);
                    shpStream.WriteLe(c.Y);
                    if (Header.ShapeType == ShapeTypes.PointZ)
                    {
                        shpStream.WriteLe(c.Z);
                    }
                    if (Header.ShapeType == ShapeTypes.PointM || Header.ShapeType == ShapeTypes.PointZ)
                    {
                        shpStream.WriteLe(c.M);
                    }
                    fid++;
                }
            }
            shpStream.Flush();
            shpStream.Close();
            shxStream.Flush();
            shxStream.Close();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            UpdateAttributes();
            sw.Stop();
            MessageBox.Show("Attribute handling Time: " + sw.ElapsedMilliseconds);
            SaveProjection();
            
        }

      
        



    }
}
