using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MapWindow.Geometries;
namespace MapWindow.Main.Generic
{
    /// <summary>
    /// An enumerable collection of PointQuadNodes 
    /// </summary>
    public class QuadNodeCollection<T> : IEnumerable<QuadNode<T>>
    {
        QuadNode<T>[] _Nodes;
        IEnvelope _envelope;
        int _Level;
        /// <summary>
        /// Creates a new collection of PointQuadNodes
        /// </summary>
        public QuadNodeCollection(IEnvelope envelope, int level)
        {
            _Nodes = new QuadNode<T>[4];
            _envelope = envelope;
            _Level = level;
        }

        #region IEnumerable<PointQuadNode> Members

        /// <summary>
        /// Gets an enumerator for this collection
        /// </summary>
        /// <returns>A type specific PointQuadNode IEnumerator</returns>
        public IEnumerator<QuadNode<T>> GetEnumerator()
        {
            return new QuadNodeEnumerator<T>(_Nodes);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Creates an enumerator for this collection
        /// </summary>
        /// <returns>A generic IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new QuadNodeEnumerator<T>(_Nodes) as IEnumerator;
        }

        /// <summary>
        /// Obtains the quad node from the specified index location.
        /// 2 3
        /// 0 1
        /// </summary>
        /// <param name="Index">the numeric index specifying the location</param>
        /// <returns>A PointQuadNode</returns>
        /// <exception cref="System.IndexOutOfRangeException">The specified index was out of bounds</exception>
        public QuadNode<T> this[int Index]
        {
            get
            {
                if (Index < 0 || Index > 3)
                {
                    throw new IndexOutOfRangeException("The specified index was out of bounds");
                }
                if (_Nodes[Index] == null)
                {
                    _Nodes[Index] = CreateSubnode(Index);
                }
                return _Nodes[Index];
            }
            set
            {
                if (Index < 0 || Index > 3)
                {
                    throw new IndexOutOfRangeException("The specified index was out of bounds");
                }
                _Nodes[Index] = value;
            }
        }

        /// <summary>
        /// Retrieves either the existing quad node that corresponds to a point, or creates
        /// a new one.  This only goes one level deep, however.
        /// </summary>
        /// <param name="X">The X coordinate</param>
        /// <param name="Y">The Y coordinate</param>
        /// <returns>The points.</returns>
        public QuadNode<T> GetFromPoint(double X, double Y)
        {
            if (X < _envelope.Minimum.X) return null;
            if (X > _envelope.Maximum.X) return null;
            if (Y < _envelope.Minimum.Y) return null;
            if (Y > _envelope.Maximum.Y) return null;
            if (Y < _envelope.Center().Y)
            {
                if (X < _envelope.Center().X)
                {
                    if (_Nodes[0] == null)
                    {
                        _Nodes[0] = CreateSubnode(0);
                    }
                    return _Nodes[0];
                }
                else
                {
                    if (_Nodes[1] == null)
                    {
                        _Nodes[1] = CreateSubnode(1);
                    }
                    return _Nodes[1];
                }
            }
            else
            {
                if (X < _envelope.Center().X)
                {
                    if (_Nodes[2] == null)
                    {
                        _Nodes[2] = CreateSubnode(2);
                    }
                    return _Nodes[2];
                }
                else
                {
                    if (_Nodes[3] == null)
                    {
                        _Nodes[3] = CreateSubnode(3);
                    }
                    return _Nodes[3];
                }
            }
        }

        /// <summary>
        /// Creates a new child node in the specified quadrant
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private QuadNode<T> CreateSubnode(int index)
        {
            // create a new subquad in the appropriate quadrant
            double minx = 0.0;
            double maxx = 0.0;
            double miny = 0.0;
            double maxy = 0.0;

            switch (index)
            {
                case 0:
                    minx = _envelope.Minimum.X;
                    maxx = _envelope.Center().X;
                    miny = _envelope.Minimum.Y;
                    maxy = _envelope.Center().Y;
                    break;
                case 1:
                    minx = _envelope.Center().X;
                    maxx = _envelope.Maximum.X;
                    miny = _envelope.Minimum.Y;
                    maxy = _envelope.Center().Y;
                    break;
                case 2:
                    minx = _envelope.Minimum.X;
                    maxx = _envelope.Center().X;
                    miny = _envelope.Center().Y;
                    maxy = _envelope.Maximum.Y;
                    break;
                case 3:
                    minx = _envelope.Center().X;
                    maxx = _envelope.Maximum.X;
                    miny = _envelope.Center().Y;
                    maxy = _envelope.Maximum.Y;
                    break;
                default:
                    break;
            }
            Envelope sqEnv = new Envelope(minx, maxx, miny, maxy);
            QuadNode<T> node = null;
            if (_Level == 1)
            {
                // 0th level has items, but no further subquads
                node = new QuadNode<T>(sqEnv);
            }
            else
            {
                node = new QuadNode<T>(sqEnv, _Level - 1);
            }
            return node;
        }

        #endregion
    }
}
