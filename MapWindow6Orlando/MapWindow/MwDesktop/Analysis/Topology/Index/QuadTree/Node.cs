//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://www.gnu.org/licenses/lgpl.html and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;

using MapWindow.Analysis.Topology.Utilities;
using MapWindow.Geometries;
namespace MapWindow.Analysis.Topology.Index.Quadtree
{
    /// <summary>
    /// Represents a node of a <c>Quadtree</c>.  Nodes contain
    /// items which have a spatial extent corresponding to the node's position
    /// in the quadtree.
    /// </summary>
    public class Node : NodeBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static Node CreateNode(IEnvelope env)
        {
            Key key = new Key(env);
            Node node = new Node(key.Envelope, key.Level);
            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="addEnv"></param>
        /// <returns></returns>
        public static Node CreateExpanded(Node node, IEnvelope addEnv)
        {
            Envelope expandEnv = new Envelope(addEnv);
            if (node != null) 
                expandEnv.ExpandToInclude(node.env);

            Node largerNode = CreateNode(expandEnv);
            if (node != null) 
                largerNode.InsertNode(node);
            return largerNode;
        }

        private IEnvelope env;
        private Coordinate centre;
        private int level;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="level"></param>
        public Node(IEnvelope env, int level)
        {
            this.env = env;
            this.level = level;
            centre = new Coordinate();
            centre.X = (env.Minimum.X + env.Maximum.X) / 2;
            centre.Y = (env.Minimum.Y + env.Maximum.Y) / 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnvelope Envelope
        {
            get
            {
                return env;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(IEnvelope searchEnv)
        {
            return env.Intersects(searchEnv);
        }

        /// <summary> 
        /// Returns the subquad containing the envelope.
        /// Creates the subquad if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchEnv"></param>
        public virtual Node GetNode(IEnvelope searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, centre);            
            // if subquadIndex is -1 searchEnv is not contained in a subquad
            if (subnodeIndex != -1) 
            {
                // create the quad if it does not exist
                Node node = GetSubnode(subnodeIndex);
                // recursively search the found/created quad
                return node.GetNode(searchEnv);
            }
            else return this;            
        }

        /// <summary>
        /// Returns the smallest <i>existing</i>
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchEnv"></param>
        public virtual NodeBase Find(IEnvelope searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, centre);
            if (subnodeIndex == -1)
                return this;
            if (Nodes[subnodeIndex] != null) 
            {
                // query lies in subquad, so search it
                Node node = Nodes[subnodeIndex];
                return node.Find(searchEnv);
            }
            // no existing subquad, so return this one anyway
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public virtual void InsertNode(Node node)
        {
            Assert.IsTrue(env == null || env.Contains(node.Envelope));        
            int index = GetSubnodeIndex(node.env, centre);        
            if (node.level == level - 1)
                Nodes[index] = node;                    
            else 
            {
                // the quad is not a direct child, so make a new child quad to contain it
                // and recursively insert the quad
                Node childNode = CreateSubnode(index);
                childNode.InsertNode(node);
                Nodes[index] = childNode;
            }
        }

        /// <summary>
        /// Get the subquad for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        /// <param name="index"></param>
        private Node GetSubnode(int index)
        {
            if (Nodes[index] == null)
                Nodes[index] = CreateSubnode(index);
            return Nodes[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Node CreateSubnode(int index)
        {
            // create a new subquad in the appropriate quadrant
            double minx = 0.0;
            double maxx = 0.0;
            double miny = 0.0;
            double maxy = 0.0;

            switch (index) 
            {
                case 0:
                    minx = env.Minimum.X;
                    maxx = centre.X;
                    miny = env.Minimum.Y;
                    maxy = centre.Y;
                    break;
                case 1:
                    minx = centre.X;
                    maxx = env.Maximum.X;
                    miny = env.Minimum.Y;
                    maxy = centre.Y;
                    break;
                case 2:
                    minx = env.Minimum.X;
                    maxx = centre.X;
                    miny = centre.Y;
                    maxy = env.Maximum.Y;
                    break;
                case 3:
                    minx = centre.X;
                    maxx = env.Maximum.X;
                    miny = centre.Y;
                    maxy = env.Maximum.Y;
                    break;
	            default:
		            break;
            }
            Envelope sqEnv = new Envelope(minx, maxx, miny, maxy);
            Node node = new Node(sqEnv, level - 1);
            return node;
        }
    }
}
