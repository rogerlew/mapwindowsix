//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow.dll for the MapWindow 6.0 project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System;
using System.Collections;
using System.Text;

namespace MapWindow.Scrap
{
    /// <summary>  
    /// Thrown by a <c>WKTReader</c> when a parsing problem occurs.
    /// </summary>
    public class ParseException : ApplicationException 
    {
        /// <summary>
        /// Creates a <c>ParseException</c> with the given detail message.
        /// </summary>
        /// <param name="message">A description of this <c>ParseException</c>.</param>
        public ParseException(String message) : base(message) { }

        /// <summary>  
        /// Creates a <c>ParseException</c> with <c>e</c>s detail message.
        /// </summary>
        /// <param name="e">An exception that occurred while a <c>WKTReader</c> was
        /// parsing a Well-known Text string.</param>
        public ParseException(Exception e) : this(e.ToString()) { }
    }
}
