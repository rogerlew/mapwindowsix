//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the the original author of the file, which is an adaptation of the Java KDTree library implemented by Levy 
// and Heckel. This simplified version is written by Marco A. Alvarez
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from the KDTreeDll
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford, shortly after 
// Allen Anselmo first added it to MapWinGeoProc in August 2008.  
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;

namespace MapWindow.Analysis.Topology.KDTree
{
    /// <summary>
    /// KeyDuplicateException is thrown when the <TT>KDTree.insert</TT> method
    /// is invoked on a key already in the KDTree.
    /// 
    /// @author Simon Levy
    /// Translation by Marco A. Alvarez
    /// </summary> 
    public class KeyDuplicateException : Exception
    {
        /// <summary>
        /// KeyDuplicateException
        /// </summary>
        public KeyDuplicateException():base(MessageStrings.KeyDuplicateException)
        {
           
        }
    }
}
