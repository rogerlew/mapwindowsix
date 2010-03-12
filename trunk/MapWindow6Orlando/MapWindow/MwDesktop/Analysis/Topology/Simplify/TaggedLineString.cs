
using System.Collections;
using System.Collections.Generic;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Simplify
{
    /// <summary>
    /// 
    /// </summary>
    public class TaggedLineString
    {
        private readonly LineString _parentLine;
        private TaggedLineSegment[] _segs;
        private readonly IList _resultSegs = new ArrayList();
        private readonly int _minimumSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLine"></param>
        public TaggedLineString(LineString parentLine) : this(parentLine, 2) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLine"></param>
        /// <param name="minimumSize"></param>
        public TaggedLineString(LineString parentLine, int minimumSize)
        {
            _parentLine = parentLine;
            _minimumSize = minimumSize;
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int MinimumSize
        {
            get
            {
                return _minimumSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual LineString Parent
        {
            get
            {
                return _parentLine;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IList<Coordinate> ParentCoordinates
        {
            get
            {
                return _parentLine.Coordinates;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate[] ResultCoordinates
        {
            get
            {
                return ExtractCoordinates(_resultSegs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int ResultSize
        {
            get
            {
                int resultSegsSize = _resultSegs.Count;
                return resultSegsSize == 0 ? 0 : resultSegsSize + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual TaggedLineSegment GetSegment(int i)
        {
            return _segs[i]; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            IList<Coordinate> pts = _parentLine.Coordinates;
            _segs = new TaggedLineSegment[pts.Count - 1];
            for (int i = 0; i < pts.Count - 1; i++)
            {
                TaggedLineSegment seg
                         = new TaggedLineSegment(pts[i], pts[i + 1], _parentLine, i);
                _segs[i] = seg;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual TaggedLineSegment[] Segments
        {
            get
            {
                return _segs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seg"></param>
        public virtual void AddToResult(LineSegment seg)
        {
            _resultSegs.Add(seg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ILineString AsLineString()
        {
            return _parentLine.Factory.CreateLineString(ExtractCoordinates(_resultSegs));        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ILinearRing AsLinearRing()
        {
            return _parentLine.Factory.CreateLinearRing(ExtractCoordinates(_resultSegs));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segs"></param>
        /// <returns></returns>
        private static Coordinate[] ExtractCoordinates(IList segs)
        {
            Coordinate[] pts = new Coordinate[segs.Count + 1];
            LineSegment seg = null;
            for (int i = 0; i < segs.Count; i++)
            {
                seg = (LineSegment)segs[i];
                pts[i] = seg.CP0;
            }
            // add last point
            if (seg != null) pts[pts.Length - 1] = seg.cP1;
            return pts;
        }
    }
}
