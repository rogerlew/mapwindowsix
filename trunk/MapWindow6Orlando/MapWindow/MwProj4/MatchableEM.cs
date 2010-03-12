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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 2:03:38 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;

namespace MapWindow.Projections
{


    /// <summary>
    /// MatchableEM
    /// </summary>
    public static class ProjMatchableEM
    {
        /// <summary>
        /// This tests the public properties from the two objects.  If any properties implement
        /// the IMatchable interface, and do not match, this returns false.  If any public
        /// properties are value types, and they are not equal, then this returns false.
        /// </summary>
        /// <param name="self">This matchable item </param>
        /// <param name="other">The other item to compare to this item</param>
        /// <returns>Boolean, true if there is a match</returns>
        public static bool Matches(this IProjMatchable self, IProjMatchable other)
        {
            List<string> ignoreMe;
            return self.Matches(other, out ignoreMe);
        }



    }
}
