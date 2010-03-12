using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using MapWindow.Geometries;
namespace MapWindow.Main.Generic
{
    /// <summary>
    /// One rectangle in the entire collection
    /// </summary>
    public class QuadNode<T>
    {
        #region Private Fields

        int _Level;
        IEnvelope _envelope;
        QuadNodeCollection<T> _Nodes;
        List<T> _Items;
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of a single rectangular region in a quad-tree
        /// </summary>
        /// <param name="envelope">The envelope for this quadrant</param>
        /// <param name="level">The depth in the tree for this node</param>
        public QuadNode(IEnvelope envelope, int level)
        {
            _Level = level;
            _envelope = envelope;
            _Nodes = new QuadNodeCollection<T>(envelope, level); 
                                         
        }

        /// <summary>
        /// At the final depth
        /// </summary>
        /// <param name="envelope">The envelope for this quadrant</param>
        public QuadNode(IEnvelope envelope)
        {
            _Level = 0;
            _envelope = envelope;
            _Items = new List<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the envelope for this node
        /// </summary>
        public IEnvelope Envelope
        {
            get { return _envelope; }
        }

        /// <summary>
        /// Gets the maximum level for this quad tree
        /// </summary>
        public int Level
        {
            get { return _Level; }
        }

        /// <summary>
        /// Checks the specified envelope and returns all the objects that "might" be contained
        /// within the Envelope based on the specified quad-tree depth.
        /// </summary>
        /// <param name="testEnvelope">The rectangular region to investigate</param>
        /// <returns>A list of objects.</returns>
        public List<T> Query(IEnvelope testEnvelope)
        {
            List<T> temp = new List<T>();
            if (testEnvelope.Intersects(_envelope))
            {
                if (_Level == 0) return _Items; 
                foreach (QuadNode<T> node in _Nodes)
                {
                    List<T> nodequery = node.Query(testEnvelope);
                    if(nodequery != null)temp.AddRange(nodequery);
                }
                return temp;
            }
            return null;
        }

        /// <summary>
        /// Obtains the list of child nodes belonging to this node
        /// </summary>
        public QuadNodeCollection<T> Nodes
        {
            get { return _Nodes; }
        }

        /// <summary>
        /// Gets or sets the arraylist of items contained in this quad.
        /// If this is the 0th level, it simply returns its own list.
        /// If this level is higher than 0, it will sequentially 
        /// </summary>
        public List<T> Items
        {
            get 
            {
                if (_Level == 0)
                {
                    return _Items;
                }
                else
                {
                    List<T> list = new List<T>();
                    foreach (QuadNode<T> node in _Nodes)
                    {
                        list.AddRange(node.Items);
                    }
                    return list;
                }
                
            }
        }

        /// <summary>
        /// Adds the item to the tree based on the coordinates.  This will return false
        /// if the specified coordinates fell outside the envelope.
        /// </summary>
        /// <param name="X">The X coordinate</param>
        /// <param name="Y">The Y coordinate</param>
        /// <param name="Item">The item to add</param>
        public bool Add(double X, double Y, T Item)
        {
            if (_Level == 0)
            {
                _Items.Add(Item);
                return true;
            }
            else
            {
                QuadNode<T> node = _Nodes.GetFromPoint(X, Y);
                if (node == null) return false;
                return _Nodes.GetFromPoint(X, Y).Add(X, Y, Item);
            }
        }
        #endregion
    }
}
