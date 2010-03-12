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

namespace MapWindow.Geometries.Utilities
{
    /// <summary>
    /// A visitor to Geometry elements which can
    /// be short-circuited by a given condition.
    /// </summary>
    public abstract class ShortCircuitedGeometryVisitor
    {
        private bool _isDone;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        public void ApplyTo(IGeometry geom) 
        {
            for (int i = 0; i < geom.NumGeometries && ! _isDone; i++) 
            {
                IGeometry element = geom.GetGeometryN(i);
                if (!(element is GeometryCollection)) 
                {
                    Visit(element);
                    if (IsDone()) 
                    {
                        _isDone = true;
                        return;
                    }
                }
                else ApplyTo(element);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        protected abstract void Visit(IGeometry element);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsDone();
    }
}
