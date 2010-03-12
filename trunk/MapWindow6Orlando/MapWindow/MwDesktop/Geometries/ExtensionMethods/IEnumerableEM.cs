//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/18/2009 1:57:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;

namespace MapWindow.Geometries
{


    /// <summary>
    /// IEnumerableEM
    /// </summary>
    public static class IEnumerableEM
    {
        
        /// <summary>
        /// cycles through any strong typed collection where the type implements ICLoneable
        /// and clones each member, inserting that member into the new list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static List<T> CloneList<T>(this IEnumerable<T> original) where T:ICloneable
        {
            List<T> result = new List<T>();
            foreach (T item in original)
            {
                result.Add(Global.SafeCastTo<T>(item.Clone()));
            }
            return result;
        }



    }
}
