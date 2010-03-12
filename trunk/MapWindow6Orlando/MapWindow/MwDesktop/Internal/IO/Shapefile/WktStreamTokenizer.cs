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
using System.IO;

using MapWindow.Analysis.Topology.Utilities;

namespace MapWindow.Data
{
	/// <summary>
	/// Reads a stream of Well Known Text (wkt) string and returns a stream of tokens.
	/// </summary>
    [Obsolete("This class is only for GeoTools.NET code compatibility: use WKT Reader for read WKT Streams.")]
	public class WktStreamTokenizer : GeoToolsStreamTokenizer
	{
		/// <summary>
		/// Initializes a new instance of the WktStreamTokenizer class.
		/// </summary>
		/// <remarks>The WktStreamTokenizer class ais in reading WKT streams.</remarks>
		/// <param name="reader">A TextReader that contains WKT.</param>
		public WktStreamTokenizer(TextReader reader) : base(reader, true)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");
		}

		/// <summary>
		/// Reads a token and checks it is what is expected.
		/// </summary>
		/// <param name="expectedToken">The expected token.</param>
		/// <exception cref="Exception">If the token is not what is expected.</exception>
		public void ReadToken(string expectedToken)
		{
			this.NextToken();
			if (this.GetStringValue() != expectedToken)
				throw new Exception(String.Format("Expecting comma ('{3}') but got a '{0}' at line {1} column {2}.",
                    this.GetStringValue(), this.LineNumber, this.Column, expectedToken));
		}
		
		/// <summary>
		/// Reads a string inside double quotes.
		/// </summary>
		/// <remarks>
		/// White space inside quotes is preserved.
		/// </remarks>
		/// <returns>The string inside the double quotes.</returns>
		public string ReadDoubleQuotedWord()
		{
			string word = String.Empty;
			ReadToken("\"");	
			NextToken(false);
			// while (GetStringValue() != String.Empty)
            while (GetStringValue() != String.Empty && GetStringValue() != "\"") // monoGIS-paul42 fix
			{
				word = word + GetStringValue();
				NextToken(false);
			} 
			return word;
		}

		/// <summary>
		/// Reads the authority and authority code.
		/// </summary>
		/// <param name="authority">String to place the authority in.</param>
		/// <param name="authorityCode">String to place the authority code in.</param>
		public void ReadAuthority(ref string authority,ref string authorityCode)
		{
			//AUTHORITY["EPSG","9102"]]
			ReadToken("AUTHORITY");
			ReadToken("[");
			authority = this.ReadDoubleQuotedWord();
			ReadToken(",");
			authorityCode = this.ReadDoubleQuotedWord();
			ReadToken("]");
		}
	}
}
