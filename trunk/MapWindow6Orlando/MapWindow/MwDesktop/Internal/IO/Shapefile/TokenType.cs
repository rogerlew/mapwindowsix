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

namespace MapWindow.Analysis.Topology.Utilities
{
	/// <summary>
	/// Represents the type of token created by the StreamTokenizer class.
	/// </summary>
    [Obsolete("This enum is only for GeoTools.NET code compatibility.")]
	public enum TokenType
	{
		/// <summary>
		/// Indicates that the token is a word.
		/// </summary>
		Word,

		/// <summary>
		/// Indicates that the token is a number. 
		/// </summary>
		Number,

		/// <summary>
		/// Indicates that the end of line has been read. The field can only have this value if the eolIsSignificant method has been called with the argument true. 
		/// </summary>
		Eol,

		/// <summary>
		/// Indicates that the end of the input stream has been reached.
		/// </summary>
		Eof,

		/// <summary>
		/// Indictaes that the token is white space (space, tab, newline).
		/// </summary>
		Whitespace,

		/// <summary>
		/// Characters that are not whitespace, numbers, etc...
		/// </summary>
		Symbol
	}
}
