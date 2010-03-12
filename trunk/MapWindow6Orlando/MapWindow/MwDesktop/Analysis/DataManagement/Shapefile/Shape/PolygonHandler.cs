using System;
using System.Collections;
using System.Collections.Generic;
using MapWindow.Analysis.Topology.Algorithm;
using MapWindow.Geometries;
using System.Linq;
namespace MapWindow.Analysis.DataManagement.Shapefile.Shape
{
	/// <summary>
	/// Converts a Shapefile point to a OGIS Polygon.
	/// </summary>
	public class PolygonHandler : ShapeHandler
	{		
		/// <summary>
		/// The ShapeType this handler handles.
		/// </summary>
        public override ShapeGeometryTypes ShapeType
		{
			get
			{
                return ShapeGeometryTypes.Polygon;
			}
		}

		/// <summary>
		/// Reads a stream and converts the shapefile record to an equilivent geometry object.
		/// </summary>
		/// <param name="file">The stream to read.</param>
		/// <param name="geometryFactory">The geometry factory to use when making the object.</param>
		/// <returns>The Geometry object that represents the shape file record.</returns>
		public override IGeometry Read(BigEndianBinaryReader file, IGeometryFactory geometryFactory)
		{
			int shapeTypeNum = file.ReadInt32();
            ShapeGeometryTypes shapeType = (ShapeGeometryTypes)Enum.Parse(typeof(ShapeGeometryTypes), shapeTypeNum.ToString());
            if ( ! ( shapeType == ShapeGeometryTypes.Polygon  || shapeType == ShapeGeometryTypes.PolygonM ||
                     shapeType == ShapeGeometryTypes.PolygonZ || shapeType == ShapeGeometryTypes.PolygonZM))	
				throw new ShapefileException("Attempting to load a non-polygon as polygon.");

			// Read and for now ignore bounds.
			double[] box = new double[4];
			for (int i = 0; i < 4; i++) 
				box[i] = file.ReadDouble();

		    int numParts = file.ReadInt32();
			int numPoints = file.ReadInt32();
			int[] partOffsets = new int[numParts];
			for (int i = 0; i < numParts; i++)
				partOffsets[i] = file.ReadInt32();

			ArrayList shells = new ArrayList();
			ArrayList holes = new ArrayList();

		    for (int part = 0; part < numParts; part++)
			{
				int start = partOffsets[part];
			    int finish;
			    if (part == numParts - 1)
					 finish = numPoints;
				else finish = partOffsets[part + 1];
				int length = finish - start;
                CoordinateList points = new CoordinateList();
				for (int i = 0; i < length; i++)
				{
					Coordinate external = new Coordinate(file.ReadDouble(), file.ReadDouble() );					
                    new PrecisionModel(geometryFactory.PrecisionModel).MakePrecise(external);
                    Coordinate internalCoord = external;
					points.Add(internalCoord);
				}

				ILinearRing ring = geometryFactory.CreateLinearRing(points.ToArray());
				
                // If shape have only a part, jump orientation check and add to shells
                if (numParts == 1)
                    shells.Add(ring);
                else
                {
                    // Orientation check
                    if (CGAlgorithms.IsCounterClockwise(points.ToArray()))
                        holes.Add(ring);
                    else shells.Add(ring);
                }
			}

			// Now we have a list of all shells and all holes
			ArrayList holesForShells = new ArrayList(shells.Count);
			for (int i = 0; i < shells.Count; i++)
				holesForShells.Add(new ArrayList());
			// Find holes
			for (int i = 0; i < holes.Count; i++)
			{
				LinearRing testRing = (LinearRing) holes[i];
				LinearRing minShell = null;
				IEnvelope minEnv = null;
				IEnvelope testEnv = testRing.EnvelopeInternal;
				Coordinate testPt = testRing.GetCoordinateN(0);
				LinearRing tryRing;
				for (int j = 0; j < shells.Count; j++)
				{
					tryRing = (LinearRing) shells[j];
					IEnvelope tryEnv = tryRing.EnvelopeInternal;
					if (minShell != null) 
						minEnv = minShell.EnvelopeInternal;
					bool isContained = false;
					CoordinateList coordList = new CoordinateList(tryRing.Coordinates);
					if (tryEnv.Contains(testEnv)
                        && (CGAlgorithms.IsPointInRing(testPt, coordList.ToArray()) 
                        || (PointInList(testPt, coordList)))) 				
						isContained = true;

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (isContained)
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                            minShell = tryRing;

                        // Suggested by Brian Macomber and added 3/28/2006:
                        // holes were being found but never added to the holesForShells array
                        // so when converted to geometry by the factory, the inner rings were never created.
                        ArrayList holesForThisShell = (ArrayList)holesForShells[j];
                        holesForThisShell.Add(holes[i]);
                    }
				}
			}

			IPolygon[] polygons = new Polygon[shells.Count];
			for (int i = 0; i < shells.Count; i++)			
				polygons[i] = geometryFactory.CreatePolygon((LinearRing) shells[i], 
                    (LinearRing[])((ArrayList) holesForShells[i]).ToArray(typeof(LinearRing)));
        
			if (polygons.Length == 1)
				return polygons[0];
			// It's a multi part
			return geometryFactory.CreateMultiPolygon(polygons);
		}

		/// <summary>
		/// Writes a Geometry to the given binary wirter.
		/// </summary>
		/// <param name="geometry">The geometry to write.</param>
		/// <param name="file">The file stream to write to.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public override void Write(IBasicGeometry geometry, System.IO.BinaryWriter file, IGeometryFactory geometryFactory)
		{
            // Diego Guidi say's: his check seems to be not useful and slow the operations...
			//  if (!geometry.IsValid)    
			//	Trace.WriteLine("Invalid polygon being written.");

			IGeometryCollection multi;
            if (geometry is IGeometryCollection ||
               geometry is IMultiLineString ||
               geometry is IMultiPoint ||
               geometry is IMultiPolygon)
				multi = (IGeometryCollection)geometry;
			else 
			{
                IGeometryFactory gf;
                if (geometry is IGeometry)
                {
                    gf = new GeometryFactory(new PrecisionModel(((IGeometry)geometry).PrecisionModel));
                }
                else
                {
                    gf = new GeometryFactory();		
                }
						
				multi = gf.CreateMultiPolygon( new[]{(Polygon) geometry} );
			}

			file.Write(int.Parse(Enum.Format(typeof(ShapeGeometryTypes), ShapeType, "d")));
        
			IEnvelope box = multi.EnvelopeInternal;
			IEnvelope bounds = GetEnvelopeExternal(new PrecisionModel(geometryFactory.PrecisionModel),  box);
			file.Write(bounds.Minimum.X);
			file.Write(bounds.Minimum.Y);
			file.Write(bounds.Maximum.X);
			file.Write(bounds.Maximum.Y);
        
			int numParts = GetNumParts(multi);
			int numPoints = multi.NumPoints;
			file.Write(numParts);
			file.Write(numPoints);
        			
			// write the offsets to the points
			int offset=0;
			for (int part = 0; part < multi.NumGeometries; part++)
			{
				// offset to the shell points
				Polygon polygon = (Polygon)multi.Geometries[part];
				file.Write(offset);
				offset = offset + polygon.ExteriorRing.NumPoints;

				// offstes to the holes
				foreach (LinearRing ring in polygon.Holes)
				{
					file.Write(offset);
					offset = offset + ring.NumPoints;
				}	
			}

			// write the points 
			for (int part = 0; part < multi.NumGeometries; part++)
			{
				Polygon poly = (Polygon)multi.Geometries[part];
				WriteCoords(poly.Shell.Coordinates, file);
				foreach(ILinearRing ring in poly.Holes)
				{				
					WriteCoords(ring.Coordinates, file);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="file"></param>
		private static void WriteCoords(IEnumerable<Coordinate> points, System.IO.BinaryWriter file)
		{
			Coordinate external;
			foreach (Coordinate point in points)
			{
				// external = geometryFactory.PrecisionModel.ToExternal(point);
                external = point;
				file.Write(external.X);
				file.Write(external.Y);
			}
		}

		/// <summary>
		/// Gets the length of the shapefile record using the geometry passed in.
		/// </summary>
		/// <param name="geometry">The geometry to get the length for.</param>
		/// <returns>The length in bytes this geometry is going to use when written out as a shapefile record.</returns>
		public override int GetLength(IBasicGeometry geometry)
		{
			int numParts = geometry.NumGeometries;
			return (22 + (2 * numParts) + geometry.NumPoints * 8);
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
		private static int GetNumParts(IGeometry geometry)
		{
			int numParts=0;
			if (geometry is MultiPolygon)
            {
                MultiPolygon mpoly = geometry as MultiPolygon;
                foreach (Polygon poly in mpoly.Geometries)
					numParts = numParts + poly.Holes.Length + 1;
            }
			else if (geometry is Polygon)
				numParts = ((Polygon)geometry).Holes.Length + 1;
			else throw new InvalidOperationException("Should not get here.");
			return numParts;
		}

		/// <summary>
		/// Test if a point is in a list of coordinates.
		/// </summary>
		/// <param name="testPoint">TestPoint the point to test for.</param>
		/// <param name="pointList">PointList the list of points to look through.</param>
		/// <returns>true if testPoint is a point in the pointList list.</returns>
		private static bool PointInList(Coordinate testPoint, IEnumerable<Coordinate> pointList) 
		{
			foreach(Coordinate p in pointList)
				if (p.Equals2D(testPoint))
					return true;
			return false;
		}
	}
}
