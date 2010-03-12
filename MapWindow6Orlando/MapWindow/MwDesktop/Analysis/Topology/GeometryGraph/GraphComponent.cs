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

using MapWindow.Geometries;
using MapWindow.Analysis.Topology.Utilities;

namespace MapWindow.GeometriesGraph
{
    /// <summary>
    /// A GraphComponent is the parent class for the objects'
    /// that form a graph.  Each GraphComponent can carry a
    /// Label.
    /// </summary>
    abstract public class GraphComponent
    {

        #region Private Variables

        private Label _label;
        private bool _isInResult = false;
        private bool _isCovered = false;
        private bool _isCoveredSet = false;
        private bool _isVisited = false;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public GraphComponent() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inLabel"></param>
        public GraphComponent(Label inLabel)
        {
            _label = inLabel;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Label Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }     
                
        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsInResult
        { 
            get
            {
                return _isInResult;
            }
            set
            {
                _isInResult = value;
            }           
        }

       

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsCovered
        {
            get
            {
                return _isCovered;
            }
            set
            {
                _isCovered = value;
                _isCoveredSet = true;                
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsCoveredSet 
        {
            get
            {
                return _isCoveredSet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsVisited
        {
            get
            {
                return _isVisited;
            }
            set
            {
                _isVisited = value;
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// A coordinate in this component (or null, if there are none).
        /// </returns>
        abstract public Coordinate Coordinate { get; set; }

        /// <summary>
        /// Compute the contribution to an IM for this component.
        /// </summary>
        abstract public void ComputeIM(IntersectionMatrix im);

        /// <summary>
        /// An isolated component is one that does not intersect or touch any other
        /// component.  This is the case if the label has valid locations for
        /// only a single Geometry.
        /// </summary>
        /// <returns><c>true</c> if this component is isolated.</returns>
        abstract public bool IsIsolated { get; }

        /// <summary>
        /// Update the IM with the contribution for this component.
        /// A component only contributes if it has a labelling for both parent geometries.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateIM(IntersectionMatrix im)
        {
            Assert.IsTrue(Label.GeometryCount >= 2, "found partial label");
            ComputeIM(im);
        }
    }
}
