
namespace MapWindow.Geometries
{
    /// <summary>
    /// A type specific MultiGeometry that specifically uses points
    /// </summary>
    public interface IMultiPoint: IGeometryCollection
    {
        /// <summary>
        /// Gets or sets the point that resides at the specified index
        /// </summary>
        /// <param name="index">A zero-based integer index specifying the point to get or set</param>
        /// <returns></returns>
        new IPoint this[int index]
        {
            get;
            set;
        }
    }
}
