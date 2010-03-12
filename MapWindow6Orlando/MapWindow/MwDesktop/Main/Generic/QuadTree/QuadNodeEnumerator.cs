using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Main.Generic
{
    /// <summary>
    /// An enumerator for the PointQuadNode
    /// </summary>
    public class QuadNodeEnumerator<T> : IEnumerator<QuadNode<T>>
    {
        int _Index;
        QuadNode<T>[] _Nodes;

        /// <summary>
        /// Creates a new instance of an enumerator
        /// </summary>
        /// <param name="nodes">Nodes</param>
        public QuadNodeEnumerator(QuadNode<T>[] nodes)
        {
            _Nodes = nodes;
            _Index = -1;
        }


        #region IEnumerator<PointQuadNode> Members

        /// <summary>
        /// Gets the current QuadNode
        /// </summary>
        public QuadNode<T> Current
        {
            get 
            {
                if (_Index < 0 || _Index > 4)
                {
                    throw new IndexOutOfRangeException("use MoveNext() to get to the first member");
                }
                return _Nodes[_Index];
            }
        }

        #endregion

        #region IDisposable Members
        
        /// <summary>
        /// Does nothing
        /// </summary>
        public void Dispose()
        {
            // there are no unmanaged elements here
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Gets the current PointQuadNode as an object
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get 
            {
                if (_Index < 0 || _Index > 4)
                {
                    throw new IndexOutOfRangeException("use MoveNext() to get to the first member");
                }
                return _Nodes[_Index];
            }
        }

        /// <summary>
        /// Advances to the next member.  This should be called once to advance to the first member.
        /// </summary>
        /// <returns>Returns true if there is a node to advance to.</returns>
        public bool MoveNext()
        {
            _Index++;
            while (_Index < 4 && _Nodes[_Index] == null)
            {
                _Index++;
            }
            if (_Index > 3) return false;
            return true;           
        }

        /// <summary>
        /// Resets the enumerator 
        /// </summary>
        public void Reset()
        {
            _Index = -1;
        }

        

        #endregion

       

    }
}
