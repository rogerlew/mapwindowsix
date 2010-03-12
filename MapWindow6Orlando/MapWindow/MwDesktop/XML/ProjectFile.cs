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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/28/2009 8:39:25 AM
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
using System.Xml;
using MapWindow.Components;
namespace MapWindow.XML
{


    /// <summary>
    /// ProjectFile
    /// </summary>
    public class ProjectFile
    {
        #region Private Variables

        private IEnvelope _extents;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectFile
        /// </summary>
        public ProjectFile()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Automatically determines the version from the file and opens the filename.
        /// Plugins will be ignored from 4.0 project settings.
        /// </summary>
        /// <param name="filename">The string path of the file to open as a filename.</param>
        public void Open(string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.DocumentElement;

            doc.Load(filename);
           // LoadExtents(root["Extents"]);
           // LoadGroup(root["Groups"]);
        
        }

        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the extents of the project
        /// </summary>
        public IEnvelope Extents
        {
            get { return _extents; }
            set { _extents = value; }
        }


        #endregion

        #region Private Methods

        ////private void LoadExtents(XmlElement xmlExtents)
        ////{
        ////    _extents = new Envelope();
        ////    try
        ////    {
        ////        double xMax = double.Parse(xmlExtents.Attributes["xMax"].InnerText);
        ////        double xMin = double.Parse(xmlExtents.Attributes["xMin"].InnerText);
        ////        double yMax = double.Parse(xmlExtents.Attributes["yMax"].InnerText);
        ////        double yMin = double.Parse(xmlExtents.Attributes["yMin"].InnerText);
        ////        _extents = new Envelope(xMin, xMax, yMin, yMax);
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }

        ////}

        ////private void LoadGroup(XmlElement xmlGroups)
        ////{
        ////    List<IndexedGroup> groups = new List<IndexedGroup>();

        ////    foreach (XmlElement xmlGroup in xmlGroups)
        ////    {
        ////        if (xmlGroup["Layers"] != null)
        ////        {
        ////            IndexedGroup group = new IndexedGroup();
        ////            group.Name = xmlGroup.Attributes["Name"].InnerText;
        ////            group.Index = int.Parse(xmlGroup.Attributes["Position"].InnerText);
        ////            group.IsExpanded = bool.Parse(xmlGroup.Attributes["Expanded"].InnerText);
        ////            foreach (XmlElement xmlLayer in xmlGroup)
        ////            {
        ////                string path = xmlLayer.Attributes["Path"].InnerText;
        ////                ILayer layer = LayerManager.DefaultLayerManager.OpenLayer(path);
        ////                layer.IsVisible = bool.Parse(xmlLayer.Attributes["Visible"].InnerText);
        ////                layer.IsExpanded = bool.Parse(xmlLayer.Attributes["Expanded"].InnerText);
        ////                IFeatureLayer fl = layer as IFeatureLayer;
        ////                if (fl != null)
        ////                {
        //                    //XmlElement xmlSymbology = xmlLayer.Attributes["ShapeFileProperties"];
        //                    //IPointLayer pl = fl as IPointLayer;
        //                    //if (pl != null)
        //                    //{
        //                    //    string pointType = xmlSymbology.Attributes["PointType"].InnerText;
                                
        //                    //    if (pointType == "ptFontChar")
        //                    //    {
        //                    //        CharacterSymbol charSymbol = new CharacterSymbol();
                                    
        //                    //    }
        //                    //    else if (pointType == "ptImageList")
        //                    //    {
        //                    //        PictureSymbol pictureSymbol = new PictureSymbol();


        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        ISimpleSymbol symbol = new SimpleSymbol();
        //                    //        symbol.Color = mwParseColor(xmlSymbology.Attributes["Color"]);
        //                    //        float size = float.Parse(xmlSymbology.Attributes["LineOrPointSize"]);
        //                    //        symbol.Size = new Size2D(size, size);

        //                    //    }

                                
                                
                                
                                
        //                   // }
        //                }

        //            }
        //            groups.Add(group);
        //        }
        //    }

            
        //}

        //private void mwParseShape(string pointType, ISimpleSymbol symbol)
        //{
        //    switch (pointType)
        //    {
        //        case "ptCircle": symbol.PointShape = PointShapes.Ellipse; break;
        //        case "ptDiamond": symbol.PointShape = PointShapes.Diamond; break;
        //        case "ptSquare": symbol.PointShape = PointShapes.Rectangle; break;
        //        case "ptTriangleDown":
        //            symbol.PointShape = PointShapes.Triangle;
        //            symbol.Angle = -90F;
        //            break;
        //        case "ptTriangleLeft":
        //            symbol.PointShape = PointShapes.Triangle;
        //            symbol.Angle = 180F;
        //            break;
        //        case "ptTriangleRight":
        //            symbol.PointShape = PointShapes.Triangle;
        //            break;
        //        case "ptTriangleUp":
        //            symbol.PointShape = PointShapes.Triangle;
        //            symbol.Angle = 90F;
        //            break;
        //    }
        //}
        

        //private Color mwParseColor(string colorString)
        //{
        //    int rgb = int.Parse(colorString);
        //    int r = rgb % 256;
        //    int g = rgb / 256 % 256;
        //    int b = rgb / (256 * 256);
        //    return Color.FromArgb(r, g, b);
        //}
       

        //private void LoadLayers(XmlElement xmlLayers)
        //{
            
        //}


        


       

        //private class IndexedGroup : IComparable
        //{
        //    public int Index;
        //    public string Name;
        //    public bool IsExpanded;
        //    public List<IndexedGroup> Layers;
        //    public IndexedGroup()
        //    {
        //        Layers = new List<IndexedGroup>();
        //    }

        //    #region IComparable Members

        //    public int CompareTo(object obj)
        //    {
        //        IndexedGroup lyr = obj as IndexedGroup;
        //        if (lyr != null)
        //        {
        //            return Index.CompareTo(lyr.Index);
        //        }
        //    }

        //    #endregion
        //}

        #endregion

    }
}
