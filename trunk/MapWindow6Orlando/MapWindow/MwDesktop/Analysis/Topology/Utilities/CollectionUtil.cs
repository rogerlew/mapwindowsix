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
using System.Collections.Generic;
using System.Text;

namespace MapWindow.Analysis.Topology.Utilities
{
    /// <summary>
    /// Executes a transformation function on each element of a collection
    /// and returns the results in a new List.
    /// </summary>
    public class CollectionUtil
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public delegate T FunctionDelegate<T>(T obj);

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" />
        /// and returns the results in a new <see cref="IList" />.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IList Transform(ICollection coll, FunctionDelegate<object> func)
        {
            IList result = new ArrayList();
            IEnumerator i = coll.GetEnumerator(); 
            foreach(object obj in coll)           
                result.Add(func(obj));
            return result;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" /> 
        /// but does not accumulate the result.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        public static void Apply(ICollection coll, FunctionDelegate<object> func)
        {
            foreach(object obj in coll)
                func(obj);
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" />
        /// and collects all the entries for which the result
        /// of the function is equal to <c>true</c>.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IList Select(ICollection coll, FunctionDelegate<object> func)
        {
            IList result = new ArrayList();            
            foreach (object obj in coll)
                if (true.Equals(func(obj)))
                    result.Add(obj);                            
            return result;
        }
    }
}
