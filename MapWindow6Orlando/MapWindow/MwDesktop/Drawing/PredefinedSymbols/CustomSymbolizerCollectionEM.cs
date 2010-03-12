//********************************************************************************************************
// Product Name: MapWindow.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for MapWindow.Drawing version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/27/2009 12:33:20 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace MapWindow.Drawing
{


    /// <summary>
    /// Extension methods for a collection of custom symbolizers
    /// </summary>
    public static class CustomSymbolizerCollectionEM
    {
        
        #region Methods

        /// <summary>
        /// Saves a list of custom line symbolizers to a file.
        /// </summary>
        /// <param name="self">The list of custom line symbolizers</param>
        /// <param name="fileName">the file name</param>
        public static void Save(this IEnumerable<CustomLineSymbolizer> self, string fileName)
        {
            System.IO.Stream myStream = System.IO.File.Open(fileName, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            Console.WriteLine("Writing Symbolizers to file");
            bformatter.Serialize(myStream, self);
            myStream.Close();

        }

        /// <summary>
        /// Saves a list of custom point symbolizers to a file
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fileName"></param>
        public static void Save(this IEnumerable<CustomPointSymbolizer> self, string fileName)
        {

        }

        /// <summary>
        /// Saves a list of custom polygon symbolizers to a file
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fileName"></param>
        public static void Save(this IEnumerable<CustomPolygonSymbolizer> self, string fileName)
        {

        }

        /// <summary>
        /// Loads the list of custom line symbolizers from a file
        /// </summary>
        /// <param name="self">The list of custom line symbolizers</param>
        /// <param name="fileName">the file name</param>
        public static void Load(this IEnumerable<CustomLineSymbolizer> self, string fileName)
        {
            self = null;
            
            if (File.Exists(fileName))
            {
                System.IO.Stream myStream = System.IO.File.Open(fileName, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                Console.WriteLine("Reading Information");
                self = (IEnumerable<CustomLineSymbolizer>)bformatter.Deserialize(myStream);
                myStream.Close();
            }
        }

        #endregion

    }
}
