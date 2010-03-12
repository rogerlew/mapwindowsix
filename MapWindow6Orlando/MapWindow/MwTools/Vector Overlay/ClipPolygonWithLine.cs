//********************************************************************************************************
// Product Name: MapWindow.Tools.mwClipPolygonWithLine
// Description:  Clip Polygon with Line
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
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna with guidence of MapWinGeoProc. Created in 2009.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// -----------------------|------------------------|--------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for ClipPolygonWithLine
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
//********************************************************************************************************
using System;
using System.Collections.Generic;
using MapWindow.Data;
using MapWindow.Geometries;
using MapWindow.Tools.Param;

namespace MapWindow.Tools
{
    public class ClipPolygonWithLine:ITool
    {

        #region ITool Members

        private string _workingPath;
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        /// <summary>
        /// Returns the Version of the tool
        /// </summary>
        public Version Version
        {
            get { return (new Version(1, 0, 0, 0)); }
        }

        /// <summary>
        /// Returns the Author of the tool's name
        /// </summary>
        public string Author
        {
            get { return (TextStrings.MapWindowDevelopmentTeam); }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public string Category
        {
            get { return (TextStrings.VectorOverlay); }
        }

        /// <summary>
        /// Returns a description of what the tool does for inclusion in the help section of the toolbox list
        /// </summary>
        public string Description
        {
            get { return (TextStrings.ClipPolygonwithLine); }
        }

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public bool  Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            if (input1 != null) input1.FillAttributes();
            var input2 = _inputParam[1].Value as IFeatureSet;
            if (input2 != null) input2.FillAttributes();
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ClipPolygonWithLine Operation tool programaticaly.
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input1">The input Polygon FeatureSet.</param>
        /// <param name="input2">The input Polyline FeatureSet.</param>
        /// <param name="output">The output Polygon FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input1, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }
            if (cancelProgressHandler.Cancel)
                return false;
            IFeature polygon = input1.Features[0];
            IFeature line = input2.Features[0];
            IFeatureSet resultFS = new FeatureSet(FeatureTypes.Polygon);
            int previous = 0;
            if (DoClipPolygonWithLine(ref polygon, ref line, ref output) == false)
            {
                throw new SystemException(TextStrings.Exceptioninclipin);
            }
            int intFeature = output.Features.Count;
            for (int i = 0; i < intFeature; i++)
            {
                Polygon poly = new Polygon(output.Features[i].Coordinates);
                resultFS.AddFeature(poly);

                int current = Convert.ToInt32(Math.Round(i * 100D / intFeature));
                //only update when increment in percentage
                if (current > previous)
                    cancelProgressHandler.Progress("", current, current + TextStrings.progresscompleted);
                previous = current;
            }
            cancelProgressHandler.Progress("", 100, 100 + TextStrings.progresscompleted);
            resultFS.SaveAs(output.Filename, true);
            return true;
        }

        #region ClipPolygonWithLine

        /// <summary>
        /// Divides a polygon into multiple sections depending on where a line crosses it. Saves the resulting
        /// polygon sections to a new polygon shapefile.
        /// </summary>
        /// <param name="polygon">The polygon to be divided.</param>
        /// <param name="line">The line that will be used to divide the polgyon.</param>
        /// <param name="resultSF">The in-memory shapefile where resulting polygons should be saved.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool DoClipPolygonWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultSF)
        {
            if (polygon.NumGeometries > 1)
            {
                return ClipMultiPartPolyWithLine(ref polygon, ref line, ref resultSF, false);
            }
            return Accurate_ClipPolygonWithLine(ref polygon, ref line, ref resultSF);
        }

        #region ClipMultiPartPolyWithLine

        /// <summary>
        /// This will clip MultiPart Polygon with line.
        /// </summary>
        /// <param name="polygon">Input Polygon.</param>
        /// <param name="line">Input Line.</param>
        /// <param name="resultFile">Output Featureset.</param>
        /// <param name="speedOptimized">The speed optimizer.</param>
        /// <returns></returns>
        public static bool ClipMultiPartPolyWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultFile, bool speedOptimized)
        {
            int numParts = polygon.NumGeometries;
            if (numParts == 0)
                numParts = 1;
            if (numParts > 1)
            {
                //multiple parts
                FixMultiPartPoly(ref polygon);
                IFeature[] polyParts = new IFeature[numParts];
                SeparateParts(ref polygon, ref polyParts);
                IFeatureSet holeSF = new FeatureSet(FeatureTypes.Polygon);
                IFeatureSet tempResult = new FeatureSet(FeatureTypes.Polygon);
                IFeatureSet modPolySF = new FeatureSet(FeatureTypes.Polygon);
                IFeatureSet resultSF = new FeatureSet(FeatureTypes.Polygon);

                for (int i = 0; i <= numParts - 1; i++)
                {
                    IFeature currPart = polyParts[i];
                    if (MapWindow.Analysis.Topology.Algorithm.CGAlgorithms.IsCounterClockwise(currPart.Coordinates) == false)
                    {
                        if (speedOptimized)
                        {
                            Fast_ClipPolygonWithLine(ref currPart, ref line, ref tempResult);
                        }
                        else
                        {
                            Accurate_ClipPolygonWithLine(ref currPart, ref line, ref tempResult);
                        }
                        int numResults = tempResult.Features.Count;
                        if (numResults > 0)
                        {
                            for (int j = 0; j <= numResults - 1; j++)
                            {
                                modPolySF.Features.Add(tempResult.Features[j]);
                            }
                        }
                    }
                    else
                    {
                        holeSF.Features.Add(currPart);
                    }
                }
                if (holeSF.Features.Count > 0)
                {
                    ErasePolySFWithPolySF(ref modPolySF, ref holeSF, ref resultSF);
                }
                if (resultSF.Features.Count > 0)
                {
                    resultFile = resultSF;
                    return true;
                }
                resultFile = resultSF;
                return false;
            }
            if (speedOptimized)
                return Fast_ClipPolygonWithLine(ref polygon, ref line, ref resultFile);
            return Accurate_ClipPolygonWithLine(ref polygon, ref line, ref resultFile);
        }

        /// <summary>
        /// Removes portions of the input polygon shapefile that are within the erase polygons.
        /// </summary>
        /// <param name="inputSF">The input polygon shapefile.</param>
        /// <param name="eraseSF">The erase polygon shapefile.</param>
        /// <param name="resultSF">The resulting shapefile, with portions removed.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static void ErasePolySFWithPolySF(ref IFeatureSet inputSF, ref IFeatureSet eraseSF, ref IFeatureSet resultSF)
        {
            //Validates the input and resultSF data
            if (inputSF == null || eraseSF == null || resultSF == null)
            {
                return;
            }

            resultSF.CopyTableSchema(inputSF);//Fill the 1st Featureset fields
            IFeatureSet tempSet = inputSF.CombinedFields(eraseSF);
            //go through every feature in 1st featureSet
            for (int i = 0; i < inputSF.Features.Count; i++)
            {

                //go through every feature in 2nd featureSet
                for (int j = 0; j < eraseSF.Features.Count; j++)
                {
                    inputSF.Features[i].Difference(eraseSF.Features[j], tempSet, FieldJoinType.All);

                }
            }
            //Add to the resultSF Feature Set
            for (int a = 0; a < tempSet.Features.Count; a++)
            {
                resultSF.Features.Add(tempSet.Features[a]);

            }

            resultSF.Save();
            return;
        }

        /// <summary>
        /// For faster clipping of polygons with lines. Limits the finding of intersections to
        /// outside->inside or inside->outside 2pt segments. Assumes only one intersections exists
        /// per segment, that a segment of two inside points or two outside points will not intersect
        /// the polygon.
        /// </summary>
        /// <param name="polygon">The polygon that will be sectioned by the line.</param>
        /// <param name="line">The line that will clip the polygon into multiple parts.</param>
        /// <param name="resultSF">The in-memory shapefile where the polygon sections will be saved.</param>
        /// <returns>False if errors are encountered, true otherwise.</returns>
        public static bool Fast_ClipPolygonWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultSF)
        {
            IFeatureSet resultFile = new FeatureSet(FeatureTypes.Polygon);
            if (polygon != null && line != null)
            {
                //make sure we are dealing with a valid shapefile type
                if (polygon.FeatureType == FeatureTypes.Polygon)
                {
                    //create the result shapefile if it does not already exist

                    if (!polygon.Overlaps(line))
                    {
                        resultSF = resultFile;
                        return false;
                    }
                    //find if all of the line is inside, outside, or part in and out of polygon
                    //line might intersect polygon mutliple times
                    int numPoints = line.NumPoints;
                    bool[] ptsInside = new bool[numPoints];

                    int numInside = 0;
                    int numOutside = 0;

                    int numParts = polygon.NumGeometries;
                    if (numParts == 0)
                    {
                        numParts = 1;
                    }

                    Coordinate[][] polyVertArray = new Coordinate[numParts][];
                    ConvertPolyToVertexArray(ref polygon, ref polyVertArray);

                    //check each point in the line to see if the entire line is either
                    //inside of the polygon or outside of it (we know it's inside polygon bounding box).
                    for (int i = 0; i <= numPoints - 1; i++)
                    {
                        Point currPt = new Point(line.Coordinates[i]);

                        if (polygon.Covers(currPt))
                        {
                            ptsInside[i] = true;
                            numInside += 1;
                        }
                        else
                        {
                            ptsInside[i] = false;
                            numOutside += 1;
                        }
                    }


                    //case: all points are inside polygon - check for possible intersections
                    if (numInside == numPoints)
                    {
                        //assume no intersections exist in fast version
                    }
                        //case: all points are outside of the polygon - check for possible intersections
                    else if (numOutside == numPoints)
                    {
                        //assume no intersections exist in fast version
                    }
                    else //case: part of line is inside and part is outside - find inside segments.
                    {
                        if (Fast_ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultFile) == false)
                        {
                            resultSF = resultFile;
                            return false;
                        }
                    }
                    //resultSF result file, do not save to disk.
                    resultSF = resultFile;

                }
                else
                {
                    resultSF = resultFile;
                    return false;
                }
            }
            else //polygon or line is invalid
            {
                resultSF = resultFile;
                return false;
            }
            return true;
        }

        #region private Fast_ProcessPartInAndOut() -- used by Fast_ClipPolygonWithLine()

        /// <summary>
        /// Given a line that contains portion both inside and outside of the polygon, this
        /// function will split the polygon based only on the segments that completely bisect
        /// the polygon. It assumes: out->out, and in->in 2pt segments do not intersect the
        /// polygon, and out->in, in->out 2pt segments have only one point of intersection.
        /// </summary>
        /// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
        /// <param name="line">The line that intersects the polygon.</param>
        /// <param name="polygon">The polygon that will be split by the intersecting line.</param>
        /// <param name="resultSF">The shapefile that the polygon sections will be saved to.</param>
        /// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
        private static bool Fast_ProcessPartInAndOut(ref bool[] insidePts, ref IFeature line, ref IFeature polygon, ref IFeatureSet resultSF)
        {
            int numLinePts = line.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.NumPoints;
            int[] intersectsPerSeg = new int[numLineSegs];
            Point[][] intersectPts = new Point[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            //cut line into 2pt segments and put in new shapefile.
            IFeatureSet lineSegSF = new FeatureSet();

            IFeature lineSegment;
            IList<Coordinate> coordi = line.Coordinates;
            Coordinate[] secCoordinate = new Coordinate[2];
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                lineSegment = new Feature(FeatureTypes.Line, secCoordinate);
                lineSegSF.Features.Add(lineSegment);
                lineSegment.Coordinates.Clear();

                intersectPts[i] = new Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];

            }
            //find number of intersections, intersection pts, and locations for each 2pt segment
            int numIntersects = CalcSiDeterm(ref lineSegSF, ref polygon, ref intersectsPerSeg, ref intersectPts, ref polyIntLocs);

            if (numIntersects == 0)
            {
                return false;
            }

            IFeature insideLine = new Feature();
            List<Coordinate> insideLineList = new List<Coordinate>();

            List<Coordinate> intersectSegList;
            Point startIntersect = new Point();
            bool startIntExists = false;
            bool validInsideLine = false;
            int insideStart = 0;
            int startIntPolyLoc = 0;

            //loop through each 2pt segment
            for (int i = 0; i <= numLinePts - 2; i++)
            {
                lineSegment = lineSegSF.Features[i];
                int numSegIntersects = intersectsPerSeg[i];
                //****************** case: inside->inside **************************************//
                int ptIndex;
                if (insidePts[i] && insidePts[i + 1])
                {
                    if (numSegIntersects == 0 && i != numLinePts - 2 && i != 0)
                    {
                        //add points to an inside line segment
                        if (startIntExists)
                        {
                            ptIndex = 0;
                            insideLineList.Insert(ptIndex, startIntersect.Coordinate);
                            startIntExists = false;
                            validInsideLine = true;
                            insideStart = startIntPolyLoc;
                        }
                        if (validInsideLine)
                        {
                            ptIndex = insideLine.NumPoints;
                            insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
                        }
                    }
                    else
                    {
                        //we do not handle multiple intersections in the fast version
                        return false;
                    }

                }//end of inside->inside
                //********************** case: inside->outside ****************************************
                else if (insidePts[i] && insidePts[i + 1] == false)
                {
                    if (numSegIntersects == 0)
                    {
                        return false;
                    }
                    if (numSegIntersects == 1)
                    {
                        if (startIntExists)
                        {
                            intersectSegList = new List<Coordinate>();
                            ptIndex = 0;
                            intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
                            ptIndex = 1;
                            intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
                            ptIndex = 2;
                            intersectSegList.Insert(ptIndex, intersectPts[i][0].Coordinate);
                            IFeature intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                            int firstPolyLoc = startIntPolyLoc;
                            int lastPolyLoc = polyIntLocs[i][0] - 1;
                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                            {
                                return false;
                            }

                            startIntExists = false; //we just used it up!
                        }
                        else if (insideLine.NumPoints != 0 && validInsideLine)
                        {
                            ptIndex = insideLineList.Count;
                            insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
                            ptIndex++;
                            insideLineList.Insert(ptIndex, intersectPts[i][0].Coordinate);
                            insideLine = new Feature(FeatureTypes.Line, insideLineList);

                            int firstPolyLoc = insideStart;
                            int lastPolyLoc = polyIntLocs[i][0] - 1;
                            if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                            {
                                return false;
                            }

                            validInsideLine = false;
                            insideLine.Coordinates.Clear();
                        }
                    }
                    else
                    {
                        //we do not handle multiple intersections in the fast version
                        return false;
                    }
                } //end of inside->outside
                //********************** case: outside->inside ***************************************
                else if (insidePts[i] == false && insidePts[i + 1])
                {
                    validInsideLine = false;


                    if (numSegIntersects == 0)
                    {
                        return false;
                    }
                    if (numSegIntersects == 1)
                    {
                        startIntExists = true;
                        startIntersect = intersectPts[i][0];
                        startIntPolyLoc = polyIntLocs[i][0] - 1;
                    }
                    else
                    {
                        //we do not handle multiple intersections in the fast version
                        return false;
                    }
                }
                //************************ case: outside->outside ***********************************
                else if (insidePts[i] == false && insidePts[i + 1] == false)
                {
                    startIntExists = false;
                    validInsideLine = false;

                    if (numSegIntersects == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        //we do not handle multiple intersections in the fast version
                        return false;
                    }
                }//end of outside->outside	
            }//end of looping through 2pt segments
            return true;
        }

        #endregion

        /// <summary>
        /// Determines which shapes are holes and which shapes are islands in
        /// a multi-part polygon and fixes their orientations accordingly.
        /// </summary>
        /// <param name="polygon">The multi-part polygon whose parts need to be checked.</param>
        public static void FixMultiPartPoly(ref IFeature polygon)
        {
            int numParts = polygon.NumGeometries;
            if (numParts == 0)
            {
                numParts = 1;
            }

            if (numParts > 1)//Multiple parts exist
            {

                IFeature[] parts = new IFeature[numParts];
                SeparateParts(ref polygon, ref parts);
                for (int i = 0; i <= numParts - 1; i++)
                {
                    bool currIsClockwise = !MapWindow.Analysis.Topology.Algorithm.CGAlgorithms.IsCounterClockwise(parts[i].Coordinates);
                    bool partIsHole = false;

                    //Decide if the current part is an island or a hole.
                    //Properties of Holes:
                    //1) Extents are inside the extents of another part.
                    //2) All points are inside the above part.		
                    for (int j = 0; j <= numParts - 1; j++)
                    {
                        if (j != i)
                        {
                            if (parts[j].Envelope.Contains(parts[i].Envelope))
                            {
                                //found a potential hole, do further checking
                                Point pt = new Point(parts[i].Coordinates[0]);
                                bool ptInside = parts[j].Covers(pt);
                                
                                if (ptInside)
                                {
                                    partIsHole = true;
                                    break;
                                }
                            }
                        }
                    }//done checking current part against all other parts
                    if (partIsHole && currIsClockwise)
                    {
                        //Hole, make sure it's in counter-clockwise order
                    }
                    else if (!partIsHole && !currIsClockwise)
                    {
                        //Island, make sure it's in clockwise order
                    }

                    /* if (reverse == true)
                     {
                         ReverseSimplePoly(ref parts[i]);
                     }
                     */
                }//done looping through parts and correcting orientation (if necessary)
                IFeature resultShp = new Feature();
                //resultShp.Create(polygon.ShapeType);
                CombineParts(ref parts, ref resultShp);
                polygon = resultShp;
            }//done with multiple parts
        }

        /// <summary>
        /// Takes an array of simple polygons and combines them into one multi-part shape.
        /// </summary>
        /// <param name="parts">The array of polygons.</param>
        /// <param name="resultShp">The resulting multi-part shape.</param>
        public static void CombineParts(ref IFeature[] parts, ref IFeature resultShp)
        {
            int numParts = parts.Length;
            Polygon po = new Polygon(parts[0].Coordinates);
            Polygon poly;
            for (int i = 0; i <= numParts - 1; i++)
            {
                poly = new Polygon(parts[i].Coordinates);
                po = poly.Union(po) as Polygon;
            }
            resultShp = new Feature(po);
        }

        public static void SeparateParts(ref IFeature poly, ref IFeature[] polyParts)
        {
            int numParts = poly.NumGeometries;
            if (numParts == 0)
            {
                numParts = 1;
            }
            IFeature[] parts = new IFeature[numParts];

            if (numParts > 1)
            {
                for (int i = 0; i <= numParts - 1; i++)
                {
                    int countPoints = poly.GetBasicGeometryN(i).Coordinates.Count;
                    List<Coordinate> partsList = new List<Coordinate>();
                    for (int j = 0; j <= countPoints - 1; j++)
                    {
                        partsList.Insert(j, poly.Coordinates[j]);
                    }
                    parts[i] = new Feature(FeatureTypes.Polygon, partsList);
                }
                polyParts = parts;
            }
            else
            {
                parts[0] = new Feature();
                parts[0] = poly;
                polyParts = parts;
            }
        }

        #endregion

        #region Accurate_ClipPolygonWithLine

        public static bool Accurate_ClipPolygonWithLine(ref IFeature polygon, ref IFeature line, ref IFeatureSet resultSF)
        {

            if (polygon != null && line != null)
            {
               
                bool boundsIntersect = line.Crosses(polygon);
                if (boundsIntersect == false)
                {
                    return false;
                }
                //find if all of the line is inside, outside, or part in and out of polygon
                //line might intersect polygon mutliple times
                int numPoints = line.NumPoints;
                bool[] ptsInside = new bool[numPoints];
                int numInside = 0;
                int numOutside = 0;
                int numParts = polygon.NumPoints;
                if (numParts == 0)
                {
                    numParts = 1;
                }
                Coordinate[][] polyVertArray = new Coordinate[numParts][];
                ConvertPolyToVertexArray(ref polygon, ref polyVertArray);
                //check each point in the line to see if the entire line is either
                //inside of the polygon or outside of it (we know it's inside polygon bounding box).
                for (int i = 0; i <= numPoints - 1; i++)
                {
                    Point currPt = new Point(line.Coordinates[i]);
                    if (polygon.Covers(currPt))
                    {
                        ptsInside[i] = true;
                        numInside += 1;
                    }
                    else
                    {
                        ptsInside[i] = false;
                        numOutside += 1;
                    }
                }
                //case: all points are inside polygon - check for possible intersections
                if (numInside == numPoints)
                {
                    if (ProcessAllInside(ref line, ref polygon, ref resultSF) == false)
                    {
                        return false;
                    }
                }
                    //case: all points are outside of the polygon - check for possible intersections
                else if (numOutside == numPoints)
                {
                    if (ProcessAllOutside(ref line, ref polygon, ref resultSF) == false)
                    {
                        return false;
                    }
                }
                    //case: part of line is inside and part is outside - find inside segments.
                else
                {
                    if (ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultSF) == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Takes a MapWinGIS polygon shape and stores all x/y coordinates into a vertex array.
        /// </summary>
        /// <param name="poly">The polygon to be converted.</param>
        /// <param name="polyVertArray">The array[numParts][] that will contain the polygon vertices.</param>
        public static void ConvertPolyToVertexArray(ref IFeature poly, ref Coordinate[][] polyVertArray)
        {
            int numParts = poly.NumGeometries;
            if (numParts == 0)
            {
                numParts = 1;
            }
            int numPoints = poly.NumPoints;
            Coordinate[][] vertArray = new Coordinate[numParts][];
            if (numParts > 1)
            {
                //separate parts of polygon
                for (int i = 0; i <= numParts - 1; i++)
                {
                    int numPtsInPart = poly.GetBasicGeometryN(i).Coordinates.Count;
                    vertArray[i] = new Coordinate[numPtsInPart];
                    for (int j = 0; j <= numPtsInPart - 2; j++)
                    {
                        vertArray[i][j] = poly.GetBasicGeometryN(i).Coordinates[j];
                    }
                    //be sure to 'close' the polygon in the vertex array!
                    vertArray[i][numPtsInPart - 1] = vertArray[i][0];
                }
            }
            else
            {
                //all points in polygon go into same vertex array
                vertArray[0] = new Coordinate[numPoints];
                for (int i = 0; i <= numPoints - 1; i++)
                {
                    vertArray[0][i] = poly.Coordinates[i];
                }
            }

            polyVertArray = vertArray;
        }

        #endregion

        #region ProcessPartInAndOut

        /// <summary>
        /// Given a line that contains portions both inside and outside of the polygon, this
        /// function will split the polygon based only on the segments that completely bisect
        /// the polygon. The possibility of mutliple intersections for any 2pt segment is taken
        /// into account.
        /// </summary>
        /// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
        /// <param name="line">The line that intersects the polygon.</param>
        /// <param name="polygon">The polygon that will be split by the intersecting line.</param>
        /// <param name="resultSF">The shapefile that the polygon sections will be saved to.</param>
        /// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
        private static bool ProcessPartInAndOut(ref bool[] insidePts, ref IFeature line, ref IFeature polygon, ref IFeatureSet resultSF)
        {
            int numLinePts = line.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.NumPoints;
            int[] intersectsPerSeg = new int[numLineSegs];
            Point[][] intersectPts = new Point[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            //cut line into 2pt segments and put in new shapefile.
            IFeatureSet lineSegSF = new FeatureSet(FeatureTypes.Line);
            IFeature lineSegment;
            IList<Coordinate> coordi = line.Coordinates;
            Coordinate[] secCoordinate = new Coordinate[2];
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                lineSegment = new Feature(FeatureTypes.Line, secCoordinate);
                lineSegSF.Features.Add(lineSegment);
                lineSegment.Coordinates.Clear();

                intersectPts[i] = new Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];

            }
            //find number of intersections, intersection pts, and locations for each 2pt segment
            int numIntersects = CalcSiDeterm(ref lineSegSF, ref polygon, ref intersectsPerSeg, ref intersectPts, ref polyIntLocs);

            if (numIntersects == 0)
            {
                return false;
            }

            IFeature insideLine = new Feature();
            List<Coordinate> insideLineList;
            IFeature intersectSeg;
            List<Coordinate> intersectSegList;
            Point startIntersect = new Point();
            bool startIntExists = false;
            bool validInsideLine = false;
            int insideStart = 0;
            int startIntPolyLoc = 0;

            //loop through each 2pt segment
            for (int i = 0; i <= numLinePts - 2; i++)
            {
                insideLineList = new List<Coordinate>();
                lineSegment = lineSegSF.Features[i];
                int numSegIntersects = intersectsPerSeg[i];
                //****************** case: inside->inside **************************************//
                int ptIndex;
                if (insidePts[i] && insidePts[i + 1])
                {
                    if (numSegIntersects == 0 && i != numLinePts - 2 && i != 0)
                    {
                        //add points to an inside line segment
                        if (startIntExists)
                        {
                            ptIndex = 0;
                            insideLineList.Insert(ptIndex, startIntersect.Coordinate);
                            startIntExists = false;
                            validInsideLine = true;
                            insideStart = startIntPolyLoc;
                        }
                        if (validInsideLine)
                        {
                            ptIndex = insideLineList.Count;
                            insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
                        }
                    }
                    else
                    {
                        //sort the intersects and their locations
                        Point[] intPts = new Point[numSegIntersects];
                        Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
                        FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            if (j == 0)
                            {
                                if (startIntExists)
                                {
                                    //first intersect pt is an ending pt, it must be
                                    //combined with a starting intersect pt in order to section the polygon.

                                    intersectSegList = new List<Coordinate>();
                                    ptIndex = 0;
                                    intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
                                    ptIndex = 1;
                                    intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
                                    ptIndex = 2;
                                    intersectSegList.Insert(ptIndex, intPts[0].Coordinate);
                                    intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                                    int firstPolyLoc = startIntPolyLoc;
                                    int lastPolyLoc = polyIntLocs[i][0] - 1;
                                    if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                    {
                                        return false;
                                    }
                                    startIntExists = false; //we used it up!
                                }
                                else if (insideLine.NumPoints != 0 && validInsideLine)
                                {
                                    ptIndex = insideLineList.Count;
                                    insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
                                    ptIndex++;
                                    insideLineList.Insert(ptIndex, intPts[0].Coordinate);
                                    insideLine = new Feature(FeatureTypes.Line, insideLineList);

                                    int firstPolyLoc = insideStart;
                                    int lastPolyLoc = polyIntLocs[i][0] - 1;
                                    if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                    {
                                        return false;
                                    }

                                    validInsideLine = false;
                                    insideLine.Coordinates.Clear();
                                }
                            }
                            else if (j == numSegIntersects - 1 && i != numLinePts - 2)
                            {
                                //last intersect pt is a starting pt, it must be
                                //saved for later combination
                                startIntersect = intPts[j];
                                startIntPolyLoc = polyIntLocs[i][j] - 1;
                                startIntExists = true;
                            }
                            else if (j != 0 || j != numSegIntersects - 1)
                            {
                                //a full poly section is created by two intersect points

                                intersectSegList = new List<Coordinate>();
                                ptIndex = intersectSegList.Count;
                                intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
                                ptIndex++;
                                intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
                                intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                                int firstPolyLoc = polyIntLocs[i][j] - 1;
                                int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
                                if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                {
                                    return false;
                                }
                                j++;
                            }
                        }
                    }

                }
                //********************** case: inside->outside ****************************************
                else if (insidePts[i] && insidePts[i + 1] == false)
                {
                    if (numSegIntersects == 0)
                    {
                        return false;
                    }
                    if (numSegIntersects == 1)
                    {
                        if (startIntExists)
                        {
                            intersectSegList = new List<Coordinate>();
                            ptIndex = 0;
                            intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
                            ptIndex = 1;
                            intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
                            ptIndex = 2;
                            intersectSegList.Insert(ptIndex, intersectPts[i][0].Coordinate);
                            intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                            int firstPolyLoc = startIntPolyLoc;
                            int lastPolyLoc = polyIntLocs[i][0] - 1;
                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                            {
                                return false;
                            }

                            startIntExists = false; //we just used it up!
                        }
                        else if (insideLine.NumPoints != 0 && validInsideLine)
                        {
                            ptIndex = insideLineList.Count;
                            insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
                            ptIndex++;
                            insideLineList.Insert(ptIndex, intersectPts[i][0].Coordinate);
                            insideLine = new Feature(FeatureTypes.Line, insideLineList);

                            int firstPolyLoc = insideStart;
                            int lastPolyLoc = polyIntLocs[i][0] - 1;
                            if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                            {
                                return false;
                            }

                            validInsideLine = false;
                            insideLine.Coordinates.Clear();
                        }
                    }
                    else
                    {
                        //sort the intersects and their locations
                        Point[] intPts = new Point[numSegIntersects];
                        Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
                        FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);
                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            if (j == 0)
                            {
                                if (startIntExists)
                                {
                                    intersectSegList = new List<Coordinate>();
                                    ptIndex = 0;
                                    intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
                                    ptIndex = 1;
                                    intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
                                    ptIndex = 2;
                                    intersectSegList.Insert(ptIndex, intPts[0].Coordinate);
                                    intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                                    int firstPolyLoc = startIntPolyLoc;
                                    int lastPolyLoc = polyIntLocs[i][0] - 1;
                                    if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                    {
                                        return false;
                                    }

                                    startIntExists = false; //we just used it up!
                                }
                                else if (insideLine.NumPoints != 0 && validInsideLine)
                                {
                                    ptIndex = insideLineList.Count;
                                    insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
                                    ptIndex++;
                                    insideLineList.Insert(ptIndex, intPts[0].Coordinate);
                                    insideLine = new Feature(FeatureTypes.Line, insideLineList);

                                    int firstPolyLoc = insideStart;
                                    int lastPolyLoc = polyIntLocs[i][0] - 1;
                                    if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                    {
                                        return false;
                                    }

                                    validInsideLine = false;
                                    insideLine.Coordinates.Clear();
                                }
                            }
                            else
                            {
                                //section the polygon with the intersecting segment
                                //intersectSeg = new Feature();
                                intersectSegList = new List<Coordinate>();
                                ptIndex = 0;
                                intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
                                ptIndex = 1;
                                intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
                                intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                                intersectSeg.Coordinates.Insert(ptIndex, intPts[j + 1].Coordinate);
                                int firstPolyLoc = polyIntLocs[i][j] - 1;
                                int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
                                if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                {
                                    return false;
                                }
                                j++;
                            }
                        }
                    }
                }
                    //********************** case: outside->inside ***************************************
                else if (insidePts[i] == false && insidePts[i + 1])
                {
                    validInsideLine = false;
                    startIntExists = false;

                    if (numSegIntersects == 0)
                    {
                        return false;
                    }
                    if (numSegIntersects == 1)
                    {
                        startIntExists = true;
                        startIntersect = intersectPts[i][0];
                        startIntPolyLoc = polyIntLocs[i][0] - 1;
                    }
                    else
                    {
                        //sort the intersects and their locations
                        Point[] intPts = new Point[numSegIntersects];
                        Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
                        FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                        //an odd number of intersects exist, at least one full poly section
                        //will be created along with one hanging intersect pt.
                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            if (j == numSegIntersects - 1)
                            {
                                startIntExists = true;
                                startIntersect = intPts[j];
                                startIntPolyLoc = polyIntLocs[i][j] - 1;
                            }
                            else
                            {
                                intersectSegList = new List<Coordinate>();
                                ptIndex = 0;
                                intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
                                ptIndex = 1;
                                intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
                                intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                                int firstPolyLoc = polyIntLocs[i][j] - 1;
                                int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
                                if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                                {
                                    return false;
                                }
                                j++;
                            }
                        }
                    }
                }
                //************************ case: outside->outside ***********************************
                else if (insidePts[i] == false && insidePts[i + 1] == false)
                {
                    startIntExists = false;
                    validInsideLine = false;

                    if (numSegIntersects == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        //sort the intersects and their locations
                        Point[] intPts = new Point[numSegIntersects];
                        Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
                        FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                        //should always be an even amount of intersections, full poly section created
                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            intersectSegList = new List<Coordinate>();
                            ptIndex = 0;
                            intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
                            ptIndex = 1;
                            intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
                            intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

                            int firstPolyLoc = polyIntLocs[i][j] - 1;
                            int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
                            {
                                return false;
                            }
                            j++;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region SectionPolygonWithLine
        /// <summary>
        /// Sections a polygon into multiple parts depending on where line crosses it and if previous sectioning has occured.
        /// </summary>
        /// <param name="line">The line that splits the polygon. First and last points are intersect points.</param>
        /// <param name="polygon">The polygon that is to be split by the line.</param>
        /// <param name="polyStart">Index to polygon segment where the first intersect point is found.</param>
        /// <param name="polyEnd">Index to polygon segment where last intersect point is found.</param>
        /// <param name="resultSF">Reference to result shapefile where new polygon sections will be saved.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        private static bool SectionPolygonWithLine(ref IFeature line, ref IFeature polygon, int polyStart, int polyEnd, ref IFeatureSet resultSF)
        {
            int numResults = resultSF.Features.Count;
            bool previousSplits = false;
            if (numResults != 0)
            {
                previousSplits = true;
            }
            //we can now make two new polygons by splitting the original one with the line segment
            IFeature poly1 = new Feature();
            IFeature poly2 = new Feature();

            SplitPolyInTwo(ref line, ref polygon, polyStart, polyEnd, ref poly1, ref poly2);
            if (previousSplits == false)
            {
                int shpIndex = 0;
                resultSF.Features.Insert(shpIndex, poly1);
                shpIndex = 1;
                resultSF.Features.Insert(shpIndex, poly2);
            }
            else
            {
                //this polygon underwent previous splittings, check
                //if the new results overlay the old ones before adding to resultSF

                IFeatureSet test1SF = new FeatureSet();
                IFeatureSet test2SF = new FeatureSet();

                if (ClipPolygonSFWithPolygon(ref resultSF, ref poly1, ref test1SF, false) == false)
                {
                    return false;
                }
                if (ClipPolygonSFWithPolygon(ref resultSF, ref poly2, ref test2SF, false) == false)
                {
                    return false;
                }
                if (test1SF.Features.Count > 0 || test2SF.Features.Count > 0)
                {
                    int numTestShapes = test1SF.Features.Count;
                    const int insertIndex = 0;
                    IFeature insertShape;
                    for (int j = 0; j <= numTestShapes - 1; j++)
                    {
                        insertShape = test1SF.Features[j];
                        resultSF.Features.Insert(insertIndex, insertShape);
                    }
                    numTestShapes = test2SF.Features.Count;
                    for (int j = 0; j <= numTestShapes - 1; j++)
                    {
                        insertShape = test2SF.Features[j];
                        resultSF.Features.Insert(insertIndex, insertShape);
                    }
                }
            }//end of checking against previous splits
            return true;
        }

        #endregion

        #region ProcessAllOutside

        /// <summary>
        /// For lines where every point lies outside the polygon, this function will
        /// find if any 2pt segment crosses through the polygon. If so, it will split
        /// the polygon into mutliple parts using the intersecting line segments.
        /// </summary>
        /// <param name="line">The line whose points are all inside the polygon.</param>
        /// <param name="polygon">The polygon being checked for intersection.</param>
        /// <param name="resultSF">The file where new polygon sections should be saved to.</param>
        /// <returns>False if errors were encountered, true otherwise.</returns>
        private static bool ProcessAllOutside(ref IFeature line, ref IFeature polygon, ref IFeatureSet resultSF)
        {
            int numLinePts = line.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.NumPoints;
            int[] intersectsPerSeg = new int[numLineSegs];
            Point[][] intersectPts = new Point[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            //cut line into 2pt segments and put in new shapefile.
            //string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
            IFeatureSet lineSegSF = new FeatureSet(FeatureTypes.Line);
            //  Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
            IList<Coordinate> coordi = line.Coordinates;
            Coordinate[] secCoordinate = new Coordinate[2];

            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                IFeature lineSegment = new Feature(FeatureTypes.Line, secCoordinate);
                lineSegSF.Features.Add(lineSegment);

                intersectPts[i] = new Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }
            int numIntersects = CalcSiDeterm(ref lineSegSF, ref polygon, ref intersectsPerSeg, ref intersectPts, ref polyIntLocs);
            if (numIntersects == 0)
            {
                //entire line is outside the polygon, no splitting occurs
            }
            else
            {
                //intersections exist! Find out where.
                List<Coordinate> intersectSegList = new List<Coordinate>();
                for (int i = 0; i <= numLineSegs - 1; i++)
                {
                    int numSegIntersects = intersectsPerSeg[i];
                    //if there are less than 4 intersects, the line will not cross the 
                    //polygon in such a way that a new polygon section can be created.
                    if (numSegIntersects == 0)
                    {
                        //do not add the segment to the result file,
                        //this portion is not inside the polygon
                        int c = i + 1;
                        while (intersectsPerSeg[c] == 0 && c <= numLineSegs - 1)
                        {
                            c++;
                            if (c == numLineSegs)
                            {
                                break;
                            }
                        }
                        i = c - 1;
                    }
                    else
                    {
                        //there should always be an even # of intersects for a line of all outside pts
                        //find the valid intersect points from our array

                        Point[] intPts = new Point[numSegIntersects];
                        Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);

                        FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);
                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            int ptIndex = 0;
                            intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
                            ptIndex = 1;
                            intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
                            IFeature intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);
                            int polyStartIndex = polyIntLocs[i][j] - 1;
                            int polyEndIndex = polyIntLocs[i][j + 1] - 1;
                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultSF) == false)
                            {
                                return false;
                            }
                            intersectSeg.Coordinates.Clear();
                            j++;
                        }//end of looping through segment intersect points

                    }//end of else intersects exist for 2pt segment	
                }//end of looping through 2pt line segments
            }//end of else intersects exist
            return true;
        }
        #endregion
 
        #region ProcessAllInside

        /// <summary>
        /// For lines where every point lies within the polygon, this function will
        /// find if any 2pt segment crosses through the polygon. If so, it will split
        /// the polygon into mutliple parts using the intersecting line segments.
        /// </summary>
        /// <param name="line">The line whose points are all inside the polygon.</param>
        /// <param name="polygon">The polygon being checked for intersection.</param>
        /// <param name="resultSF">The file where new polygon sections should be saved to.</param>
        /// <returns>False if errors were encountered, true otherwise.</returns>
        private static bool ProcessAllInside(ref IFeature line, ref IFeature polygon, ref IFeatureSet resultSF)
        {

            int numLinePts = line.NumPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.NumPoints;
            int[] intersectsPerSeg = new int[numLineSegs];
            Point[][] intersectPts = new Point[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            //cut line into 2pt segments and put in new shapefile.
            IFeatureSet lineSegSF = new FeatureSet(FeatureTypes.Line);
            IList<Coordinate> coordi = line.Coordinates;
            Coordinate[] secCoordinate = new Coordinate[2];
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                secCoordinate[0] = coordi[i];
                secCoordinate[1] = coordi[i + 1];
                IFeature lineSegment = new Feature(FeatureTypes.Line, secCoordinate);
                lineSegSF.Features.Add(lineSegment);

                intersectPts[i] = new Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            int numIntersects = CalcSiDeterm(ref lineSegSF, ref polygon, ref intersectsPerSeg, ref intersectPts, ref polyIntLocs);
            if (numIntersects == 0)//entire line is inside the polygon
            {
                //entire line is inside the polygon, no splitting occurs
            }
            else
            {
                //intersections exist! Find out where.
                List<Coordinate> intersectSegList = new List<Coordinate>();
                for (int i = 0; i <= numLineSegs - 1; i++)
                {
                    int numSegIntersects = intersectsPerSeg[i];
                    //if there are less than 4 intersects, the line will not cross the 
                    //polygon in such a way that a new polygon section can be created.
                    if (numSegIntersects <= 2)
                    {
                        //inside lines should be ignored, we only want a portion that crosses
                        //the polygon.
                        int c = i + 1;
                        while (intersectsPerSeg[c] <= 2 && c <= numLineSegs - 1)
                        {
                            c++;
                            if (c == numLineSegs)
                            {
                                break;
                            }
                        }
                        i = c - 1;
                    }
                    else
                    {
                        //there should always be an even # of intersects for a line of all inside pts
                        //find intersecting segments that will split the polygon
                        Point[] intPts = new Point[numSegIntersects];
                        Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
                        FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            if (j == 0 || j == numSegIntersects - 1)
                            {
                                //Any segment formed from inside pt -> intersect pt
                                //or intersect pt -> inside pt will NOT cross the polygon.
                            }
                            else
                            {
                                int ptIndex = 0;
                                intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
                                ptIndex = 1;
                                intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
                                IFeature intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);
                                int polyStartIndex = polyIntLocs[i][j] - 1;
                                int polyEndIndex = polyIntLocs[i][j + 1] - 1;
                                if (SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultSF) == false)
                                {
                                    return false;
                                }
                                intersectSeg.Coordinates.Clear();
                                j++;
                            }
                        }//end of looping through intersect pts
                    }//end of more than 2 intersects exist
                }//end of looping through 2pt line segments	
            }//end of else intersects exist	
            return true;
        }
        #endregion
         
        #region  private SplitPolyInTwo
        
        /// <summary>
        /// Splits original polygon into two portions depending on where line crosses it.
        /// </summary>
        /// <param name="line">The line the crosses the polygon. First and last points are intersects.</param>
        /// <param name="polygon">The polygon that is split by the line.</param>
        /// <param name="beginPolySeg">The section of the polygon where the first intersect point is found.</param>
        /// <param name="endPolySeg">The section of the polygon where the last intersect point is found.</param>
        /// <param name="poly1">First portion of polygon returned after splitting.</param>
        /// <param name="poly2">Second portion of polygon returned after splitting.</param>
        private static void SplitPolyInTwo(ref IFeature line, ref IFeature polygon, int beginPolySeg, int endPolySeg, ref IFeature poly1, ref IFeature poly2)
        {
            //function assumes first and last pts in line are the two intersection pts
            List<Coordinate> firstPartList = new List<Coordinate>();
            List<Coordinate> secondPartList = new List<Coordinate>();
            int numPolyPts = polygon.NumPoints;
            int numLinePts = line.NumPoints;
            bool crossZeroPt;
            int count;

            //now, see if we'll be crossing the zero pt while building the first result poly
            if (beginPolySeg < endPolySeg + 1)
            {
                crossZeroPt = true;
            }
            else
            {
                crossZeroPt = false;
            }

            //split the poly into two portions
            //begin by creating the side where the line will be inserted in the forward direction
            //add all line pts in forward direction
            for (int i = 0; i <= numLinePts - 1; i++)
            {
                firstPartList.Add(line.Coordinates[i]);
            }

            //add polygon pts that are clockwise of the ending line point
            if (crossZeroPt)
            {
                //we'll be crossing the zero point when creating a clockwise poly
                count = (numPolyPts - 1) - (endPolySeg + 1);
                //add all points before the zero point and clockwise of last point in line
                for (int i = 0; i <= count - 1; i++)
                {
                    int position = (endPolySeg + 1) + i;
                    firstPartList.Add(polygon.Coordinates[position]);
                    //firstPart.Coordinates.Add(polygon.Coordinates[position]);
                }
                //add all points after the zero point and up to first line point
                for (int i = 0; i <= beginPolySeg; i++)
                {
                    firstPartList.Add(polygon.Coordinates[i]);
                }
            }
            else
            {
                //we don't need to worry about crossing the zero point
                for (int i = endPolySeg + 1; i <= beginPolySeg; i++)
                {
                    firstPartList.Add(polygon.Coordinates[i]);
                }
            }
            //add beginning line point to close the new polygon
            firstPartList.Add(line.Coordinates[0]);
            IFeature firstPart = new Feature(FeatureTypes.Line, firstPartList);

            //create second portion by removing first from original polygon
            //secondPart = utils.ClipPolygon(MapWinGIS.PolygonOperation.DIFFERENCE_OPERATION, polygon, firstPart);
            //above method (difference) adds unnecessary points to the resulting shape, use below instead.
            //begin by creating the side where the line will be inserted in the forward direction
            //add line pts in reverse order
            for (int i = numLinePts - 1; i >= 0; i--)
            {
                secondPartList.Add(line.Coordinates[i]);
            }

            //add polygon pts that are clockwise of the first line point
            //This may be confusing, but if crossZeroPt was true above, then it would
            //mean that the secondPart does not require crossing over the zero pt.
            //However, if crossZeroPt was false before, then secondPart will require
            //crossing the zeroPt while adding the polygon pts to the new shape.
            if (crossZeroPt == false)
            {
                //we'll be crossing the zero point when creating the second poly
                count = (numPolyPts - 1) - (beginPolySeg + 1);
                //add all points before the zero point and clockwise of first point in line
                for (int i = 0; i <= count - 1; i++)
                {
                    int position = (beginPolySeg + 1) + i;
                    secondPartList.Add(polygon.Coordinates[position]);
                }
                //add all points after the zero point and up to last line point
                for (int i = 0; i <= endPolySeg; i++)
                {
                    secondPartList.Add(polygon.Coordinates[i]);
                }
            }
            else
            {
                //we don't need to worry about crossing the zero point
                for (int i = beginPolySeg + 1; i <= endPolySeg; i++)
                {
                    secondPartList.Add(polygon.Coordinates[i]);
                }
            }
            //add ending line point to close the new polygon
            secondPartList.Add(line.Coordinates[numLinePts - 1]);
            IFeature secondPart = new Feature(FeatureTypes.Line, secondPartList);
            poly1 = firstPart;
            poly2 = secondPart;
        }

        /// <summary>
        /// Sorts all valid intersects in the array intersectPts.
        /// </summary>
        /// <param name="numIntersects">Expected number of valid intersects.</param>
        /// <param name="intersectPts">Array of all possible intersect points.</param>
        /// <param name="validIntersects">Array that will contain only the valid intersect points in sorted order.</param>
        /// <param name="startPt">The reference point to sort the valid intersect points by.</param>
        public static void FindAndSortValidIntersects(int numIntersects, ref Point[] intersectPts, ref Point[] validIntersects, ref Point startPt)
        {
            for (int i = 0; i <= numIntersects - 1; i++)
            {
                validIntersects[i] = intersectPts[i];
            }
            SortPointsArray(ref startPt, ref validIntersects);
        }

        /// <summary>
        /// Given a reference point to the line, and an array of points that
        /// lie along the line, this method sorts the array of points from the point
        /// closest to the reference pt to the pt farthest away.
        /// </summary>
        /// <param name="startPt">Point in line segment used as reference.</param>
        /// <param name="intersectPts">Array of points that lie on the same line as startPt.</param>
        private static void SortPointsArray(ref Point startPt, ref Point[] intersectPts)
        {
            double dist1;
            int numIntersectPts = intersectPts.Length;
            if (numIntersectPts == 2) //if 0 or 1 the points don't need to be sorted
            {
                //do a brute sort
                //just compare distances of each pt to the start pt.
                dist1 = PtDistance(ref startPt, ref intersectPts[0]);
                double dist2 = PtDistance(ref startPt, ref intersectPts[1]);
                if (dist1 > dist2) //need to swap locations
                {
                    Point tempPt = intersectPts[0];
                    intersectPts[0] = intersectPts[1];
                    intersectPts[1] = tempPt;
                }

            }
            else if (numIntersectPts > 2 /*&& numintersectPts <= 10*/)
            {
                //use insertion sort for small arrays
                for (int i = 0; i <= numIntersectPts - 1; i++)
                {
                    Point compPt1 = intersectPts[i];
                    dist1 = PtDistance(ref startPt, ref compPt1);
                    int c = i;
                    Point compPt2;
                    if (c != 0)
                    {
                        compPt2 = intersectPts[c - 1];
                    }
                    else
                    {
                        compPt2 = intersectPts[0];
                    }
                    while (c > 0 && (PtDistance(ref startPt, ref compPt2)) > dist1)
                    {
                        intersectPts[c] = intersectPts[c - 1];
                        c--;
                        if (c != 0)
                        {
                            compPt2.Coordinates.Clear();
                            compPt2 = intersectPts[c - 1];
                        }
                    }
                    if (c != i)
                    {
                        intersectPts[c] = compPt1;
                    }
                }
            }
            //			else if(numIntersectPts > 10)
            //			{
            //TO DO: write a quick-sort function to aid in time
            //haven't done this because it is rare to have
            //a large number of intersect pts for a small line segment
            //quick-sort performs poorly on small lists, that's why insertion
            //sort is used above.
            //			}
        }

        /// <summary>
        /// Sorts all valid intersects in the array intersectPts, along with corresponding polygon locations in array polyLoc.
        /// </summary>
        /// <param name="numIntersects">Expected number of valid intersects.</param>
        /// <param name="intersectPts">Array of all possible intersect points.</param>
        /// <param name="validIntersects">Array that will contain only the valid intersect points in sorted order.</param>
        /// <param name="startPt">The reference point to sort the valid intersect points by.</param>
        /// <param name="polyLoc">Array with corresponding indicies to where an intersect pt occurs in polygon.</param>
        private static void FindAndSortValidIntersects(int numIntersects, ref Point[] intersectPts, ref Point[] validIntersects, ref Point startPt, ref int[] polyLoc)
        {
            for (int i = 0; i <= numIntersects - 1; i++)
            {
                validIntersects[i] = intersectPts[i];
            }
            SortIntersectAndLocationArrays(ref startPt, ref validIntersects, ref polyLoc);
        }

        /// <summary>
        /// Given a reference point to the line, and an array of points that
        /// lie along the line, this method sorts the array of points from the point
        /// closest to the reference pt to the pt farthest away. It also sorts the corresponding
        /// polygon location array so that the indicies refer to the correct intersection point.
        /// </summary>
        /// <param name="startPt">Point in line segment used as reference.</param>
        /// <param name="intersectPts">Array of points that lie on the same line as startPt.</param>
        /// <param name="polyLoc">Array indexing where in polygon an intersect occurs.</param>
        private static void SortIntersectAndLocationArrays(ref Point startPt, ref Point[] intersectPts, ref int[] polyLoc)
        {
            double dist1;
            int numIntersectPts = intersectPts.Length;
            if (numIntersectPts == 2) //if 0 or 1 the points don't need to be sorted
            {
                //do a brute sort
                //just compare distances of each pt to the start pt.
                dist1 = startPt.Distance(intersectPts[0]);
                double dist2 = startPt.Distance(intersectPts[1]);
                if (dist1 > dist2) //need to swap locations
                {
                    Point tempPt = intersectPts[0];
                    intersectPts[0] = intersectPts[1];
                    intersectPts[1] = tempPt;
                    //move poly location so it corresponds to correct intersect point
                    int tempLoc = polyLoc[0];
                    polyLoc[0] = polyLoc[1];
                    polyLoc[1] = tempLoc;
                }
            }
            else
            {
                if (numIntersectPts > 2 /*&& numintersectPts <= 10*/)
                {
                    //use insertion sort for small arrays
                    for (int i = 0; i <= numIntersectPts - 1; i++)
                    {
                        Point compPt1 = intersectPts[i];
                        int tempLoc1 = polyLoc[i];
                        dist1 = startPt.Distance(compPt1);
                        int c = i;
                        Point compPt2;
                        if (c != 0)
                        {
                            compPt2 = intersectPts[c - 1];
                        }
                        else
                        {
                            compPt2 = intersectPts[0];
                        }
                        while (c > 0 && (PtDistance(ref startPt, ref compPt2)) > dist1)
                        {
                            intersectPts[c] = intersectPts[c - 1];
                            polyLoc[c] = polyLoc[c - 1];
                            c--;
                            if (c != 0)
                            {
                                compPt2 = intersectPts[c - 1];
                            }
                        }
                        if (c != i)
                        {
                            intersectPts[c] = compPt1;
                            polyLoc[c] = tempLoc1;
                        }
                    }
                }
            }
            //			else if(numIntersectPts > 10)
            //			{
            //TO DO: write a quick-sort function to aid in time
            //haven't done this because it is rare to have
            //a large number of intersect pts for a small line segment
            //quick-sort performs poorly on small lists, that's why insertion
            //sort is used above.
            //			}
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="pt0">The first point.</param>
        /// <param name="pt1">The second point.</param>
        /// <returns>The distance between pt0 and pt1.</returns>
        public static double PtDistance(ref Point pt0, ref Point pt1)
        {
            double xDiff = pt1.X - pt0.X;
            double yDiff = pt1.Y - pt0.Y;
            double distance = Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
            return distance;
        }
        private static int CalcSiDeterm(ref IFeatureSet lineSF, ref IFeature polygon, ref int[] intersectsPerLineSeg, ref Point[][] intersectionPts, ref int[][] polyIntersectLocs)
        {
            int numSignChanges = 0; //tracks number of determinant sign changes
            int numLines = lineSF.Features.Count;
            int numVerticies = polygon.NumPoints;
            int[][] detSigns = new int[numLines][];
            bool[][] signChanges = new bool[numLines][]; //keeps track of where sign changes occur
            int[][] changeLocations = new int[numLines][];
            int[] intersectsPerLine = new int[numLines];
            Point[][] intersectPts = new Point[numLines][];


            IList<Coordinate> coorPoly = polygon.Coordinates;
            //ICoordinate[] secCoor = new ICoordinate[2];
            for (int lineNo = 0; lineNo <= numLines - 1; lineNo++)
            {
                IFeature line = lineSF.Features[lineNo];
                IList<Coordinate> secCoor;
                IList<Coordinate> coorLine = line.Coordinates;
                int numChangesPerLine = 0;
                detSigns[lineNo] = new int[numVerticies];
                signChanges[lineNo] = new bool[numVerticies];
                intersectPts[lineNo] = new Point[numVerticies];
                changeLocations[lineNo] = new int[numVerticies];

                for (int vertNo = 0; vertNo <= numVerticies - 1; vertNo++)
                {
                    intersectPts[lineNo][vertNo] = new Point();
                    Point intersectPt = new Point();
                    // Calculate the determinant (3x3 square matrix)
                    double si = TurboDeterm(coorPoly[vertNo].X, coorLine[0].X, coorLine[1].X,
                        coorPoly[vertNo].Y, coorLine[0].Y, coorLine[1].Y);

                    // Check the determinant result
                    switch (vertNo)
                    {
                        case 0:
                            if (si == 0)
                                detSigns[lineNo][vertNo] = 0; // we have hit a vertex
                            else if (si > 0)
                                detSigns[lineNo][vertNo] = 1; // +'ve
                            else if (si < 0)
                                detSigns[lineNo][vertNo] = -1; // -'ve
                            signChanges[lineNo][0] = false;		// First element will NEVER be a sign change
                            break;
                        default:
                            if (si == 0)
                                detSigns[lineNo][vertNo] = 0;
                            else if (si > 0)
                                detSigns[lineNo][vertNo] = 1;
                            else if (si < 0)
                                detSigns[lineNo][vertNo] = -1;

                            // Check for sign change
                            if (detSigns[lineNo][vertNo - 1] != detSigns[lineNo][vertNo])
                            {
                                secCoor = new List<Coordinate>();
                                secCoor.Add(coorPoly[vertNo - 1]);
                                secCoor.Add(coorPoly[vertNo]);
                                //calculate the actual intercept point	
                                LineString polyTestLine1 = new LineString(secCoor);
                                secCoor = new List<Coordinate>();
                                secCoor.Add(coorLine[0]);
                                secCoor.Add(coorLine[1]);
                                LineString polyTestLine2 = new LineString(secCoor);
                                bool validIntersect = polyTestLine1.Intersects(polyTestLine2);
                                IGeometry inPt = polyTestLine1.Intersection(polyTestLine2);
                                if (inPt.Coordinates.Count == 1)
                                    intersectPt = new Point(inPt.Coordinate);

                                if (validIntersect)
                                {
                                    signChanges[lineNo][vertNo] = true;
                                    numSignChanges += 1;
                                    numChangesPerLine += 1;
                                    intersectsPerLine[lineNo] = numChangesPerLine;
                                    //we want to store the valid intersect pts at the
                                    //beginning of the array so we don't have to search for them
                                    intersectPts[lineNo][numChangesPerLine - 1] = intersectPt;
                                    //keep track of where the intersect occurs in reference to polygon
                                    changeLocations[lineNo][numChangesPerLine - 1] = vertNo; //intersect pt occurs between vertNo-1 and vertNo
                                }
                                else
                                {
                                    signChanges[lineNo][vertNo] = false;
                                }

                            }
                            else
                            {
                                signChanges[lineNo][vertNo] = false;
                            }
                            break;
                    }//end of switch

                }
            }
            polyIntersectLocs = changeLocations;
            intersectionPts = intersectPts;
            intersectsPerLineSeg = intersectsPerLine;
            return numSignChanges;
        }

        /// <summary>
        /// Calculates the determinant of a 3X3 matrix, where the first two rows
        /// represent the x,y values of two lines, and the third row is (1 1 1).
        /// </summary>
        /// <param name="Elem11">The first element of the first row in the matrix.</param>
        /// <param name="Elem12">The second element of the first row in the matrix.</param>
        /// <param name="Elem13">The third element of the first row in the matrix.</param>
        /// <param name="Elem21">The first element of the second row in the matrix.</param>
        /// <param name="Elem22">The second element of the second row in the matrix.</param>
        /// <param name="Elem23">The third element of the second row in the matrix.</param>
        /// <returns>The determinant of the matrix.</returns>
        private static double TurboDeterm(double Elem11, double Elem12, double Elem13,
            double Elem21, double Elem22, double Elem23)
        {
            // The third row of the 3x3 matrix is (1,1,1)
            return Elem11 * (Elem22 - Elem23)
                - Elem12 * (Elem21 - Elem23)
                + Elem13 * (Elem21 - Elem22);
        }
        #endregion

        #region Clip PolygonSF With Polygon
        /// <summary>
        /// Returns the portions of the polygons in polySF that lie within polygon as a 
        /// new shapefile of polygons: resultPolySF.
        /// </summary>
        /// <param name="polySF">The shapefile of polygons that are to be clipped.</param>
        /// <param name="polygon">The polygon used for clipping.</param>
        /// <param name="resultPolySF">The result shapefile for the resulting polygons to be saved (in-memory).</param>
        /// <param name="copyAttributes">True if copying attrs</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonSFWithPolygon(ref IFeatureSet polySF, ref IFeature polygon, ref IFeatureSet resultPolySF, bool copyAttributes)
        {
            if (copyAttributes)
            {
                polySF.FillAttributes();
                resultPolySF.CopyTableSchema(polySF);
            }

            if (polySF.Features.Count != 0 && polygon.NumPoints != 0 && polySF.FeatureType == FeatureTypes.Polygon)
            {
                int numShapes = polySF.Features.Count;
                for (int i = 0; i <= numShapes - 1; i++)
                {
                    polySF.Features[i].Intersection(polygon, resultPolySF, FieldJoinType.LocalOnly);
                }
            }
            return true;
        }

        #endregion

        #endregion

        /// <summary>
        /// Image displayed in the help area when no input field is selected
        /// </summary>
        public System.Drawing.Bitmap HelpImage
        {
            get { return null; }
        }

        /// <summary>
        /// Help text to be displayed when no input field is selected
        /// </summary>
        public string HelpText
        {
            get
            {
                return (TextStrings.ClipPolygonwithLine);
            }
        }

        /// <summary>
        /// Returns the address of the tools help web page in HTTP://... format. Return a empty string to hide the help hyperlink.
        /// </summary>
        public string HelpURL
        {
            get { return ("HTTP://www.mapwindow.org"); }
        }


        /// <summary>
        /// 32x32 Bitmap - The Large icon that will appears in the Tool Dialog Next to the tools name
        /// </summary>
        public System.Drawing.Bitmap Icon
        {
            get { return (null); }
        }

        /// <summary>
        /// 16x16 Bitmap - The small icon that appears in the Tool Manager Tree
        /// </summary>
        public System.Drawing.Bitmap IconSmall
        {
            get { return (null); }
        }

        /// <summary>
        /// The parameters array should be populated with default values here
        /// </summary>
        /// <returns></returns>
        public void  Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new  PolygonFeatureSetParam(TextStrings.input1PolygonShapefile);
            _inputParam[0].HelpText = TextStrings.InputPolygonforCliping;
            _inputParam[1] = new LineFeatureSetParam(TextStrings.input2LineforCliping);
            _inputParam[1].HelpText = TextStrings.Inputlineforcliping;

            _outputParam = new Parameter[1];
            //_outputParam[0] = new PolygonFeatureSetParam(TextStrings.OutputShapefile);
            _outputParam[0] = new FeatureSetParam(TextStrings.ResultShapefile);
            _outputParam[0].HelpText = TextStrings.SelectResultShapefileDirectory;
        }
        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public Parameter[] InputParameters
        {
            get { return _inputParam; }
        }

        /// <summary>
        /// Returns the name of the tool
        /// </summary>
        public string Name
        {
            get { return (TextStrings.ClipPolygonwithLine); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public Parameter[] OutputParameters
        {
            get { return _outputParam; }
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate input2 parameters default values.
        /// </summary>
        void ITool.ParameterChanged(Parameter sender)
        {
            return;
        }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public string ToolTip
        {
            get { return (TextStrings.ClipPolygonwithLine); }
        }

        /// <summary>
        /// A UniqueName Identifying this Tool, if aninput2 tool with the same UniqueName exists this tool will not be loaded
        /// </summary>
        public string UniqueName
        {
            get { return (TextStrings.MapWindowPolygonClipWithLine); }
        }

        /// <summary>
        /// This is set before the tool is executed to provide a folder where the tool can save temporary data
        /// </summary>
        public string WorkingPath
        {
            set { _workingPath = value; }
        }
        #endregion
    }
 }
