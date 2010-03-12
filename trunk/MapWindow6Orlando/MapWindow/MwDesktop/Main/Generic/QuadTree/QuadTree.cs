using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Geometries;
namespace MapWindow.Main.Generic
{
    /// <summary>
    /// The major difference between this and the QuadTree is that
    /// it assumes that the items being tested have 0 width and height.
    /// Instead, a MaxLevel is specified in the constructor, as well
    /// as an Envelope.  The idea is that point layers already know
    /// the envelope that contains them.
    /// </summary>
    public class QuadTree<T>
    {
        #region Private Fields

       
        QuadNode<T> _baseNode;
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the PointQuadTree given an area and the number of
        /// times to bisect that envelope to eventually obtain the entire tree.
        /// </summary>
        /// <param name="InitialEnvelope">The envelope of the entire tree</param>
        /// <param name="NumLevels">The initial number of levels that the tree should have.</param>
        /// <exception cref="ArgumentNullException">Envelope cannot be null or empty when using it to define the tree.</exception>
        /// <exception cref="ArgumentException">Envelope width cannot be 0 or empty.</exception>
        /// <exception cref="ArgumentException">Envelope height cannot be 0 or empty.</exception>
        /// <exception cref="ArgumentException">Number of levels should be a positive integer.</exception>
        public QuadTree(IEnvelope InitialEnvelope, int NumLevels)
        {
            if (InitialEnvelope == null) throw new ArgumentNullException("Envelope cannot be null or empty when using it to define the tree.");
            if (InitialEnvelope.Width <= 0) throw new ArgumentException("Envelope width cannot be 0 or empty.");
            if (InitialEnvelope.Height <= 0) throw new ArgumentException("Envelope height cannot be 0 or empty.");
            if (NumLevels < 1) throw new ArgumentException("Number of levels should be a positive integer 1 or greater");
            _baseNode = new QuadNode<T>(InitialEnvelope, NumLevels - 1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the maximum level for this quad tree
        /// </summary>
        public int MaxLevel
        {
            get { return _baseNode.Level; }
        }

        /// <summary>
        /// Checks the specified envelope and returns all the objects that "might" be contained
        /// within the Envelope based on the specified quad-tree depth.
        /// </summary>
        /// <param name="testEnvelope">The rectangular region to investigate</param>
        /// <returns>A list of objects.</returns>
        public List<T> Query(IEnvelope testEnvelope)
        {
            return _baseNode.Query(testEnvelope);
        }

        /// <summary>
        /// Attempts to add an item given the specified coordinates.  If the current envelope is not 
        /// large enough to include the point, then it will expand to include it, not by changing
        /// the existing structure, but simply by defining larger and larger nodes until the point can 
        /// be contained.
        /// </summary>
        /// <param name="X">The X coordinate</param>
        /// <param name="Y">The Y coordinate</param>
        /// <param name="item">The item to add</param>
        public void Add(double X, double Y, T item)
        {
            _baseNode.Add(X, Y, item);
        }

        #endregion



    }
}
