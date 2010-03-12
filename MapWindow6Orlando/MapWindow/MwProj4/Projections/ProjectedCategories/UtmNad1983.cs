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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:08:05 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MapWindow.Projections.ProjectedCategories
{


    /// <summary>
    /// UtmNad1983
    /// </summary>
    public class UtmNad1983 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983UTMZone1N;
        public readonly ProjectionInfo NAD1983UTMZone2N;
        public readonly ProjectionInfo NAD1983UTMZone3N;
        public readonly ProjectionInfo NAD1983UTMZone4N;
        public readonly ProjectionInfo NAD1983UTMZone5N;
        public readonly ProjectionInfo NAD1983UTMZone6N;
        public readonly ProjectionInfo NAD1983UTMZone7N;
        public readonly ProjectionInfo NAD1983UTMZone8N;
        public readonly ProjectionInfo NAD1983UTMZone9N;
        public readonly ProjectionInfo NAD1983UTMZone10N;
        public readonly ProjectionInfo NAD1983UTMZone11N;
        public readonly ProjectionInfo NAD1983UTMZone12N;
        public readonly ProjectionInfo NAD1983UTMZone13N;
        public readonly ProjectionInfo NAD1983UTMZone14N;
        public readonly ProjectionInfo NAD1983UTMZone15N;
        public readonly ProjectionInfo NAD1983UTMZone16N;
        public readonly ProjectionInfo NAD1983UTMZone17N;
        public readonly ProjectionInfo NAD1983UTMZone18N;
        public readonly ProjectionInfo NAD1983UTMZone19N;
        public readonly ProjectionInfo NAD1983UTMZone20N;
        public readonly ProjectionInfo NAD1983UTMZone21N;
        public readonly ProjectionInfo NAD1983UTMZone22N;
        public readonly ProjectionInfo NAD1983UTMZone23N;
        public readonly ProjectionInfo NAD1983UTMZone24N;
        public readonly ProjectionInfo NAD1983UTMZone25N;
        public readonly ProjectionInfo NAD1983UTMZone26N;
        public readonly ProjectionInfo NAD1983UTMZone27N;
        public readonly ProjectionInfo NAD1983UTMZone28N;
        public readonly ProjectionInfo NAD1983UTMZone29N;
        public readonly ProjectionInfo NAD1983UTMZone30N;
        public readonly ProjectionInfo NAD1983UTMZone31N;
        public readonly ProjectionInfo NAD1983UTMZone32N;
        public readonly ProjectionInfo NAD1983UTMZone33N;
        public readonly ProjectionInfo NAD1983UTMZone34N;
        public readonly ProjectionInfo NAD1983UTMZone35N;
        public readonly ProjectionInfo NAD1983UTMZone36N;
        public readonly ProjectionInfo NAD1983UTMZone37N;
        public readonly ProjectionInfo NAD1983UTMZone38N;
        public readonly ProjectionInfo NAD1983UTMZone39N;
        public readonly ProjectionInfo NAD1983UTMZone40N;
        public readonly ProjectionInfo NAD1983UTMZone41N;
        public readonly ProjectionInfo NAD1983UTMZone42N;
        public readonly ProjectionInfo NAD1983UTMZone43N;
        public readonly ProjectionInfo NAD1983UTMZone44N;
        public readonly ProjectionInfo NAD1983UTMZone45N;
        public readonly ProjectionInfo NAD1983UTMZone46N;
        public readonly ProjectionInfo NAD1983UTMZone47N;
        public readonly ProjectionInfo NAD1983UTMZone48N;
        public readonly ProjectionInfo NAD1983UTMZone49N;
        public readonly ProjectionInfo NAD1983UTMZone50N;
        public readonly ProjectionInfo NAD1983UTMZone51N;
        public readonly ProjectionInfo NAD1983UTMZone52N;
        public readonly ProjectionInfo NAD1983UTMZone53N;
        public readonly ProjectionInfo NAD1983UTMZone54N;
        public readonly ProjectionInfo NAD1983UTMZone55N;
        public readonly ProjectionInfo NAD1983UTMZone56N;
        public readonly ProjectionInfo NAD1983UTMZone57N;
        public readonly ProjectionInfo NAD1983UTMZone58N;
        public readonly ProjectionInfo NAD1983UTMZone59N;
        public readonly ProjectionInfo NAD1983UTMZone60N;

        public readonly ProjectionInfo NAD1983UTMZone1S;
        public readonly ProjectionInfo NAD1983UTMZone2S;
        public readonly ProjectionInfo NAD1983UTMZone3S;
        public readonly ProjectionInfo NAD1983UTMZone4S;
        public readonly ProjectionInfo NAD1983UTMZone5S;
        public readonly ProjectionInfo NAD1983UTMZone6S;
        public readonly ProjectionInfo NAD1983UTMZone7S;
        public readonly ProjectionInfo NAD1983UTMZone8S;
        public readonly ProjectionInfo NAD1983UTMZone9S;
        public readonly ProjectionInfo NAD1983UTMZone10S;
        public readonly ProjectionInfo NAD1983UTMZone11S;
        public readonly ProjectionInfo NAD1983UTMZone12S;
        public readonly ProjectionInfo NAD1983UTMZone13S;
        public readonly ProjectionInfo NAD1983UTMZone14S;
        public readonly ProjectionInfo NAD1983UTMZone15S;
        public readonly ProjectionInfo NAD1983UTMZone16S;
        public readonly ProjectionInfo NAD1983UTMZone17S;
        public readonly ProjectionInfo NAD1983UTMZone18S;
        public readonly ProjectionInfo NAD1983UTMZone19S;
        public readonly ProjectionInfo NAD1983UTMZone20S;
        public readonly ProjectionInfo NAD1983UTMZone21S;
        public readonly ProjectionInfo NAD1983UTMZone22S;
        public readonly ProjectionInfo NAD1983UTMZone23S;
        public readonly ProjectionInfo NAD1983UTMZone24S;
        public readonly ProjectionInfo NAD1983UTMZone25S;
        public readonly ProjectionInfo NAD1983UTMZone26S;
        public readonly ProjectionInfo NAD1983UTMZone27S;
        public readonly ProjectionInfo NAD1983UTMZone28S;
        public readonly ProjectionInfo NAD1983UTMZone29S;
        public readonly ProjectionInfo NAD1983UTMZone30S;
        public readonly ProjectionInfo NAD1983UTMZone31S;
        public readonly ProjectionInfo NAD1983UTMZone32S;
        public readonly ProjectionInfo NAD1983UTMZone33S;
        public readonly ProjectionInfo NAD1983UTMZone34S;
        public readonly ProjectionInfo NAD1983UTMZone35S;
        public readonly ProjectionInfo NAD1983UTMZone36S;
        public readonly ProjectionInfo NAD1983UTMZone37S;
        public readonly ProjectionInfo NAD1983UTMZone38S;
        public readonly ProjectionInfo NAD1983UTMZone39S;
        public readonly ProjectionInfo NAD1983UTMZone40S;
        public readonly ProjectionInfo NAD1983UTMZone41S;
        public readonly ProjectionInfo NAD1983UTMZone42S;
        public readonly ProjectionInfo NAD1983UTMZone43S;
        public readonly ProjectionInfo NAD1983UTMZone44S;
        public readonly ProjectionInfo NAD1983UTMZone45S;
        public readonly ProjectionInfo NAD1983UTMZone46S;
        public readonly ProjectionInfo NAD1983UTMZone47S;
        public readonly ProjectionInfo NAD1983UTMZone48S;
        public readonly ProjectionInfo NAD1983UTMZone49S;
        public readonly ProjectionInfo NAD1983UTMZone50S;
        public readonly ProjectionInfo NAD1983UTMZone51S;
        public readonly ProjectionInfo NAD1983UTMZone52S;
        public readonly ProjectionInfo NAD1983UTMZone53S;
        public readonly ProjectionInfo NAD1983UTMZone54S;
        public readonly ProjectionInfo NAD1983UTMZone55S;
        public readonly ProjectionInfo NAD1983UTMZone56S;
        public readonly ProjectionInfo NAD1983UTMZone57S;
        public readonly ProjectionInfo NAD1983UTMZone58S;
        public readonly ProjectionInfo NAD1983UTMZone59S;
        public readonly ProjectionInfo NAD1983UTMZone60S;
        


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmNad1983
        /// </summary>
        public UtmNad1983()
        {
            NAD1983UTMZone1N = new ProjectionInfo("+proj=utm +zone=1 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone2N = new ProjectionInfo("+proj=utm +zone=2 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone3N = new ProjectionInfo("+proj=utm +zone=3 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone6N = new ProjectionInfo("+proj=utm +zone=6 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone7N = new ProjectionInfo("+proj=utm +zone=7 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone8N = new ProjectionInfo("+proj=utm +zone=8 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone9N = new ProjectionInfo("+proj=utm +zone=9 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone10N = new ProjectionInfo("+proj=utm +zone=10 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone14N = new ProjectionInfo("+proj=utm +zone=14 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone15N = new ProjectionInfo("+proj=utm +zone=15 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone16N = new ProjectionInfo("+proj=utm +zone=16 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone23N = new ProjectionInfo("+proj=utm +zone=23 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone24N = new ProjectionInfo("+proj=utm +zone=24 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone25N = new ProjectionInfo("+proj=utm +zone=25 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone27N = new ProjectionInfo("+proj=utm +zone=27 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone41N = new ProjectionInfo("+proj=utm +zone=41 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone42N = new ProjectionInfo("+proj=utm +zone=42 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone43N = new ProjectionInfo("+proj=utm +zone=43 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone44N = new ProjectionInfo("+proj=utm +zone=44 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone45N = new ProjectionInfo("+proj=utm +zone=45 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone50N = new ProjectionInfo("+proj=utm +zone=50 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone51N = new ProjectionInfo("+proj=utm +zone=51 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone52N = new ProjectionInfo("+proj=utm +zone=52 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone53N = new ProjectionInfo("+proj=utm +zone=53 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone54N = new ProjectionInfo("+proj=utm +zone=54 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone55N = new ProjectionInfo("+proj=utm +zone=55 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone56N = new ProjectionInfo("+proj=utm +zone=56 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone57N = new ProjectionInfo("+proj=utm +zone=57 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone58N = new ProjectionInfo("+proj=utm +zone=58 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone59N = new ProjectionInfo("+proj=utm +zone=59 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone60N = new ProjectionInfo("+proj=utm +zone=60 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

            NAD1983UTMZone1S = new ProjectionInfo("+proj=utm +zone=1 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone2S = new ProjectionInfo("+proj=utm +zone=2 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone3S = new ProjectionInfo("+proj=utm +zone=3 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone4S = new ProjectionInfo("+proj=utm +zone=4 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone5S = new ProjectionInfo("+proj=utm +zone=5 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone6S = new ProjectionInfo("+proj=utm +zone=6 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone7S = new ProjectionInfo("+proj=utm +zone=7 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone8S = new ProjectionInfo("+proj=utm +zone=8 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone9S = new ProjectionInfo("+proj=utm +zone=9 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone10S = new ProjectionInfo("+proj=utm +zone=10 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone11S = new ProjectionInfo("+proj=utm +zone=11 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone12S = new ProjectionInfo("+proj=utm +zone=12 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone13S = new ProjectionInfo("+proj=utm +zone=13 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone14S = new ProjectionInfo("+proj=utm +zone=14 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone15S = new ProjectionInfo("+proj=utm +zone=15 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone16S = new ProjectionInfo("+proj=utm +zone=16 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone17S = new ProjectionInfo("+proj=utm +zone=17 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone18S = new ProjectionInfo("+proj=utm +zone=18 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone21S = new ProjectionInfo("+proj=utm +zone=21 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone25S = new ProjectionInfo("+proj=utm +zone=25 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone26S = new ProjectionInfo("+proj=utm +zone=26 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone27S = new ProjectionInfo("+proj=utm +zone=27 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone28S = new ProjectionInfo("+proj=utm +zone=28 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone29S = new ProjectionInfo("+proj=utm +zone=29 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone30S = new ProjectionInfo("+proj=utm +zone=30 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone31S = new ProjectionInfo("+proj=utm +zone=31 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone33S = new ProjectionInfo("+proj=utm +zone=33 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone34S = new ProjectionInfo("+proj=utm +zone=34 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone35S = new ProjectionInfo("+proj=utm +zone=35 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone37S = new ProjectionInfo("+proj=utm +zone=37 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone38S = new ProjectionInfo("+proj=utm +zone=38 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone39S = new ProjectionInfo("+proj=utm +zone=39 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone40S = new ProjectionInfo("+proj=utm +zone=40 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone41S = new ProjectionInfo("+proj=utm +zone=41 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone42S = new ProjectionInfo("+proj=utm +zone=42 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone43S = new ProjectionInfo("+proj=utm +zone=43 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone44S = new ProjectionInfo("+proj=utm +zone=44 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone45S = new ProjectionInfo("+proj=utm +zone=45 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone46S = new ProjectionInfo("+proj=utm +zone=46 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone47S = new ProjectionInfo("+proj=utm +zone=47 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone48S = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone49S = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone50S = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone51S = new ProjectionInfo("+proj=utm +zone=51 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone52S = new ProjectionInfo("+proj=utm +zone=52 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone53S = new ProjectionInfo("+proj=utm +zone=53 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone54S = new ProjectionInfo("+proj=utm +zone=54 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone55S = new ProjectionInfo("+proj=utm +zone=55 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone56S = new ProjectionInfo("+proj=utm +zone=56 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone57S = new ProjectionInfo("+proj=utm +zone=57 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591