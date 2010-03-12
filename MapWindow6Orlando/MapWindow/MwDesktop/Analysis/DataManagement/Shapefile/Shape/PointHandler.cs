using System;
using MapWindow.Geometries;

namespace MapWindow.Analysis.DataManagement.Shapefile.Shape
{
	/// <summary>
	/// Converts a Shapefile point to a OGIS Point.
	/// </summary>
	public class PointHandler : ShapeHandler
	{
		/// <summary>
		/// Initializes a new instance of the PointHandler class.
		/// </summary>
		public PointHandler() : base() { }
	
		/// <summary>
		/// The shape type this handler handles (point).
		/// </summary>
        public override ShapeGeometryTypes ShapeType
		{
			get
			{
                return ShapeGeometryTypes.Point;
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
            if (  ( shapeType == ShapeGeometryTypes.LineString  || shapeType == ShapeGeometryTypes.LineStringM   ||
                     shapeType == ShapeGeometryTypes.LineStringZ || shapeType == ShapeGeometryTypes.LineStringZM  ||
                     shapeType == ShapeGeometryTypes.MultiPatch || shapeType == ShapeGeometryTypes.NullShape ||
                     shapeType == ShapeGeometryTypes.Polygon || shapeType == ShapeGeometryTypes.PolygonM ||
                     shapeType == ShapeGeometryTypes.PolygonZ || shapeType == ShapeGeometryTypes.PolygonZM                                  
                     ))	
				throw new ShapefileException("Attempting to load a non-point shapefile as point.");
			double x= file.ReadDouble();
			double y= file.ReadDouble();
			Coordinate external = new Coordinate(x,y);			
			// return geometryFactory.CreatePoint(geometryFactory.PrecisionModel.ToInternal(external));
            new PrecisionModel(geometryFactory.PrecisionModel).MakePrecise(external);
            return geometryFactory.CreatePoint(external);
		}
		
		/// <summary>
		/// Writes to the given stream the equilivent shape file record given a Geometry object.
		/// </summary>
		/// <param name="geometry">The geometry object to write.</param>
		/// <param name="file">The stream to write to.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public override void Write(IBasicGeometry geometry, System.IO.BinaryWriter file, IGeometryFactory geometryFactory)
		{
            file.Write(int.Parse(Enum.Format(typeof(ShapeGeometryTypes), this.ShapeType, "d")));
			// Coordinate external = geometryFactory.PrecisionModel.ToExternal( geometry.Coordinates[0] );
            Coordinate external = geometry.Coordinates[0];
			file.Write(external.X);
			file.Write(external.Y);
		}

		/// <summary>
		/// Gets the length in bytes the Geometry will need when written as a shape file record.
		/// </summary>
		/// <param name="geometry">The Geometry object to use.</param>
		/// <returns>The length in bytes the Geometry will use when represented as a shape file record.</returns>
		public override int GetLength(IBasicGeometry geometry)
		{
			return 10; // The length of two doubles in 16bit words + the shapeType
		}		
	}
}
