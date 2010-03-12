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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 10:04:07 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.Xml;
using System.Xml.Serialization;
namespace MapWindow.Data
{


    /// <summary>
    /// PyramidHeader
    /// </summary>
    [XmlRoot("PyramidHeader")]
    public class PyramidHeader
    {
        #region Private Variables

        private PyramidImageHeader[] _imageHeaders;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PyramidHeader
        /// </summary>
        public PyramidHeader()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Calculates the header size for this 
        /// </summary>
        /// <returns></returns>
        public long HeaderSize()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer s = new XmlSerializer(typeof(PyramidHeader));
            s.Serialize(ms, this);
            return ms.Length;
        }

       
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the array that encompasses all of the basic header content
        /// necessary for working with this image.
        /// </summary>
        [XmlElement("ImageHeaders")]
        public PyramidImageHeader[] ImageHeaders
        {
            get { return _imageHeaders; }
            set { _imageHeaders = value; }
        }

        

        #endregion



    }
}
