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
using System.Collections.Generic;
using MapWindow.Geometries;

namespace MapWindow.Analysis.Topology.Simplify
{
    /// <summary>
    /// Simplifies a line (sequence of points) using
    /// the standard Douglas-Peucker algorithm.
    /// </summary>
    public class DouglasPeuckerLineSimplifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="distanceTolerance"></param>
        /// <returns></returns>
        public static IList<Coordinate> Simplify(IList<Coordinate> pts, double distanceTolerance)
        {
            DouglasPeuckerLineSimplifier simp = new DouglasPeuckerLineSimplifier(pts);
            simp._distanceTolerance = distanceTolerance;
            return simp.Simplify();
        }

        private readonly IList<Coordinate> _pts;
        private bool[] _usePt;
        private double _distanceTolerance;
        private readonly LineSegment _seg = new LineSegment();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        private DouglasPeuckerLineSimplifier(IList<Coordinate> pts)
        {
            _pts = pts;
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Coordinate[] Simplify()
        {
            _usePt = new bool[_pts.Count];
            for (int i = 0; i < _pts.Count; i++)
                _usePt[i] = true;
            
            SimplifySection(0, _pts.Count - 1);
            CoordinateList coordList = new CoordinateList();
            for (int i = 0; i < _pts.Count; i++)            
                if (_usePt[i])
                    coordList.Add(new Coordinate(_pts[i]));            
            return coordList.ToCoordinateArray();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void SimplifySection(int i, int j)
        {
            if ((i + 1) == j)
                return;            
            _seg.P0 = _pts[i];
            _seg.P1 = _pts[j];
            double maxDistance = -1.0;
            int maxIndex = i;
            for (int k = i + 1; k < j; k++)
            {
                double distance = _seg.Distance(_pts[k]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = k;
                }
            }
            if (maxDistance <= _distanceTolerance)
                for (int k = i + 1; k < j; k++)                
                    _usePt[k] = false;                            
            else
            {
                SimplifySection(i, maxIndex);
                SimplifySection(maxIndex, j);
            }
        }
    }
}
