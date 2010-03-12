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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 4:47:12 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
namespace MapWindow.Projections
{


    /// <summary>
    /// Parameters
    /// </summary>
    public class ProjectionInfo : ProjDescriptor, IEsriString
    {
        #region Private Variables


        private string _name;
        private GeographicInfo _geographicInfo;
        private double? _falseEasting;
        private double? _falseNorthing;
        private double? _centralMeridian;
        private double? _scaleFactor;
        private double? _latitudeOfOrigin;
        private double? _standardParallel1;
        private double? _standardParallel2;
        private double? _longitudeOf1st;
        private double? _longitudeOf2nd;
        private double? _longitudeOfCenter;
        private double? _azimuth;
        private LinearUnit _unit;
        private bool _south;
        private int? _zone;
        private bool _isGeocentric;
        private bool _isLatLon;
        private bool _geoc;
        private bool _over;
        private bool _nodefs;
        private ITransform _transform;
        private Dictionary<string, string> _parameters; // any extra parameters.
       
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Parameters
        /// </summary>
        public ProjectionInfo()
        {
            _geographicInfo = new GeographicInfo();
            _unit = new LinearUnit();
            _parameters = new Dictionary<string, string>();
            _scaleFactor = 1; // if not specified, default to 1
        }

        /// <summary>
        /// Creates a new projection and automaticalyl reads in the proj4 string
        /// </summary>
        /// <param name="proj4String">THe proj4String to read in while defining the projection</param>
        public ProjectionInfo(string proj4String)
        {
            _geographicInfo = new GeographicInfo();
            _unit = new LinearUnit();
            _parameters = new Dictionary<string, string>();
            _scaleFactor = 1; // if not specified, default to 1
            ReadProj4String(proj4String);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets a boolean that is true if the proj4 string created by the projections matches.
        /// There are multiple ways to write the same projection, but the output proj4 string
        /// should be a good indicator of whether or not they are the same. 
        /// </summary>
        /// <param name="other">The other projection to compare with.</param>
        /// <returns>Boolean, true if the projections are the same.</returns>
        public bool Equals(ProjectionInfo other)
        {
            if (other == null) return false;
            return ToProj4String().Equals(other.ToProj4String());
        }

        /// <summary>
        /// Open a given prj filename
        /// </summary>
        /// <param name="prjFilename"></param>
        public void Open(string prjFilename)
        {
            StreamReader sr = File.OpenText(prjFilename);
            string prj = sr.ReadLine();
            sr.Close();
            ReadEsriString(prj);
        }

        /// <summary>
        /// Exports this projection info by saving it to a *.prj file.
        /// </summary>
        /// <param name="prjFilename">The prj file to save to</param>
        public void SaveAs(string prjFilename)
        {
            if(File.Exists(prjFilename))File.Delete(prjFilename);
            StreamWriter sw = File.CreateText(prjFilename);
            sw.WriteLine(ToEsriString());
            sw.Close();
        }

        /// <summary>
        /// Gets the lambda 0, or central meridian, in radial coordinates
        /// </summary>
        /// <returns></returns>
        public double GetLam0()
        {
            if(_centralMeridian != null)
            {
                return _centralMeridian.Value*GeographicInfo.Unit.Radians;
            }
            return 0;
        }

        /// <summary>
        /// Gets the phi 0 or latitude of origin in radial coordinates
        /// </summary>
        /// <returns></returns>
        public double GetPhi0()
        {
            if (_latitudeOfOrigin != null)
            {
                return _latitudeOfOrigin.Value * GeographicInfo.Unit.Radians;
            }
            return 0;
        }

        /// <summary>
        /// Gets the lat_1 parameter multiplied by radians
        /// </summary>
        /// <returns></returns>
        public double GetPhi1()
        {
            if(_standardParallel1 != null)
            {
                return _standardParallel1.Value*GeographicInfo.Unit.Radians;
            }
            return 0;
        }

        /// <summary>
        /// Gets the lat_2 parameter multiplied by radians
        /// </summary>
        /// <returns></returns>
        public double GetPhi2()
        {
            if(_standardParallel2 != null)
            {
                return _standardParallel2.Value*GeographicInfo.Unit.Radians;
            }
            return 0;
        }

        /// <summary>
        /// Gets the lon_1 parameter in radians
        /// </summary>
        /// <returns></returns>
        public double GetLam1()
        {
            if(_longitudeOf1st != null)
            {
                return _standardParallel1.Value*GeographicInfo.Unit.Radians;
            }
            return 0;
        }

        /// <summary>
        /// Gets the lon_2 parameter in radians
        /// </summary>
        /// <returns></returns>
        public double GetLam2()
        {
            if(_longitudeOf2nd != null)
            {
                return _standardParallel2.Value*GeographicInfo.Unit.Radians;
            }
            return 0;
        }

       

        /// <summary>
        /// Obtains the double valued parameter if it is found and can be parsed to a double.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to return as a double value.</param>
        /// <returns>The double valued parameter of the specified name, or zero.</returns>
        public double ParamD(string parameterName)
        {
            if (_parameters.ContainsKey(parameterName) == false) return 0;
            return double.Parse(_parameters[parameterName]);
        }

        /// <summary>
        /// Obtains the double valued parameter after converting from degrees to radians.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to read</param>
        /// <returns>The double valued parameter in radians.</returns>
        public double ParamR(string parameterName)
        {
            if (_parameters.ContainsKey(parameterName) == false) return 0;
            return double.Parse(Parameters[parameterName])*Math.PI/180;
        }

        /// <summary>
        /// Obtains the integer valued parameter if it is found and can be parsed to an integer
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to find</param>
        /// <returns>An integer value representing the parameter if it was found.</returns>
        public int ParamI(string parameterName)
        {
            if (_parameters.ContainsKey(parameterName) == false) return 0;
            return int.Parse(_parameters[parameterName]);
        }

        /// <summary>
        /// Sets the lambda 0, or central meridian in radial coordinates
        /// </summary>
        /// <param name="value"></param>
        public void SetLam0(double value)
        {
            CentralMeridian = value / GeographicInfo.Unit.Radians;
        }

       
        /// <summary>
        /// Sets the phi 0 or latitude of origin in radial coordinates
        /// </summary>
        /// <param name="value"></param>
        public void SetPhi0(double value)
        {
            LatitudeOfOrigin = value/GeographicInfo.Unit.Radians;
        }
        

       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of this projection information
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///  The geographic information
        /// </summary>
        public GeographicInfo GeographicInfo
        {
            get { return _geographicInfo;  }
            set { _geographicInfo = value; }
        }

        /// <summary>
        /// The false easting for this coordinate system
        /// </summary>
        public double? FalseEasting
        {
            get { return _falseEasting; }
            set { _falseEasting = value; }
        }

        /// <summary>
        /// The false northing for this coordinate system
        /// </summary>
        public double? FalseNorthing
        {
            get { return _falseNorthing; }
            set { _falseNorthing = value; }
        }

        /// <summary>
        /// The horizontal 0 point in geographic terms
        /// </summary>
        public double? CentralMeridian
        {
            get { return _centralMeridian; }
            set { _centralMeridian = value; }
        }

        /// <summary>
        /// The scale factor for this coordinate system
        /// </summary>
        public double ScaleFactor
        {
            get { return (_scaleFactor != null) ? _scaleFactor.Value : 1; }
            set { _scaleFactor = value; }
        }

        /// <summary>
        /// The zero point in geographic terms
        /// </summary>
        public double? LatitudeOfOrigin
        {
            get { return _latitudeOfOrigin; }
            set { _latitudeOfOrigin = value; }
        }

        /// <summary>
        /// The line of latitude where the scale information is preserved.
        /// </summary>
        public double? StandardParallel1
        {
            get { return _standardParallel1;  }
            set { _standardParallel1 = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? StandardParallel2
        {
            get { return _standardParallel2; }
            set { _standardParallel2 = value; }
        }

        /// <summary>
        /// The unit being used for measurements.
        /// </summary>
        public LinearUnit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        
       
        /// <summary>
        /// Gets or sets a boolean indicating whether this projection applies to the
        /// southern coordinate system or not.
        /// </summary>
        public bool IsSouth
        {
            get { return _south; }
            set { _south = value; }
        }

        /// <summary>
        /// Gets or sets the integer zone parameter if it is specified.
        /// </summary>
        public int? Zone
        {
            get { return _zone; }
            set { _zone = value; }
        }
        
        /// <summary>
        /// Gets or sets a boolean that indicates whether or not this
        /// projection is geocentric.
        /// </summary>
        public bool IsGeocentric
        {
            get { return _isGeocentric; }
            set { _isGeocentric = value; }
        }

        /// <summary>
        /// True if this coordinate system is expressed only using geographic coordinates
        /// </summary>
        public bool IsLatLon
        {
            get { return _isLatLon; }
            set { _isLatLon = value; }
        }

        /// <summary>
        /// Gets or sets the transform that converts between geodetic coordinates and projected coordinates.
        /// </summary>
        public ITransform Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating a geocentric latitude parameter
        /// </summary>
        public bool Geoc
        {
            get { return _geoc; }
            set { _geoc = value; }
        }

        /// <summary>
        /// Gets or sets a boolean for the over-ranging flag
        /// </summary>
        public bool Over
        {
            get { return _over; }
            set { _over = value; }
        }

        /// <summary>
        /// Gets or sets the parameters, including special parameters, stored by 
        /// the string names.
        /// </summary>
        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #endregion




        #region IEsriString Members

        /// <summary>
        /// Expresses the entire projection as the ESRI well known text format that can be found in .prj files
        /// </summary>
        /// <returns>The generated string</returns>
        public string ToEsriString()
        {
            string result = @"PROJCS[""" + _name + @"""," + _geographicInfo.ToEsriString() + ", ";
            if (_transform != null) result += @"PROJECTION[""" + _transform.Name + @"""],";
            if (_falseEasting != null) result += @"PARAMETER[""False_Easting""," + _falseEasting + "],";
            if (_falseNorthing != null) result += @"PARAMETER[""False_Northing""," + _falseNorthing + "],";
            if (_centralMeridian != null) result += @"PARAMETER[""Central_Meridian""," + _centralMeridian + "],";
            if (_standardParallel1 != null) result += @"PARAMETER[""Standard_Parallel_1""," + _standardParallel1 + "],";
            if (_standardParallel2 != null) result += @"PARAMETER[""Standard_Parallel_2""," + _standardParallel2 + "],";
            if (_scaleFactor != null) result += @"PARAMETER[""Scale_Factor""," + _scaleFactor + "],";
            if (_azimuth != null) result += @"PARAMETER[""Azimuth""," + _azimuth + "],";
            if (_longitudeOfCenter != null)result += @"PARAMETER[""Longitude_Of_Center"","+ _longitudeOfCenter + "],";
            if (_longitudeOf1st != null)result += @"PARAMETER[""Longitude_Of_1st""," + _longitudeOf1st + "],";
            if (_longitudeOf2nd != null) result += @"PARAMETER[""Longitude_Of_2nd""," + _longitudeOf2nd + "],";
            if (_latitudeOfOrigin != null) result += @"PARAMETER[""Latitude_Of_Origin""," + _latitudeOfOrigin + "],";
            result += _unit.ToEsriString();
            return result;
        }

        /// <summary>
        /// Attempts to generate a new proj4 string based on the current projection parameters,
        /// regardless of whether they were read from a proj4 file or from an esri projection file.
        /// </summary>
        /// <returns></returns>
        public string ToProj4String()
        {
            UpdateParam("x_0", _falseEasting);
            UpdateParam("y_0", _falseNorthing);
            UpdateParam("k", _scaleFactor);
            UpdateParam("lat_0", _latitudeOfOrigin);
            UpdateParam("lon_0", _centralMeridian);
            UpdateParam("lat_1", _standardParallel1);
            UpdateParam("lat_2", _standardParallel2);
            UpdateParam("south", _south);
            UpdateParam("over", _over);
            UpdateParam("geoc", _geoc);
            UpdateParam("alpha", _azimuth);
            UpdateParam("lonc", _longitudeOfCenter);
            if(_transform != null) UpdateParam("proj", _transform.Proj4Name);
           

            string result = "";
            foreach (KeyValuePair<string, string> pair in Parameters)
            {
                if(pair.Key == "south")
                {
                    result += " +south";
                    continue;
                }
                result += " +" + pair.Key + "=" + pair.Value;
            }


            if (_nodefs) result += "+nodefs";
            
            return result;
        }
        private void UpdateParam(string name, string value)
        {
            if (_parameters.ContainsKey(name) && value == null)
            {
                _parameters.Remove(name);
                return;
            }
            if (value == null) return;
            if (_parameters.ContainsKey(name) == false)
            {
                _parameters.Add(name, value);
                return;
            }
            _parameters[name] = value;
        }


        private void UpdateParam(string name, bool value)
        {
            if(_parameters.ContainsKey(name) && value == false)
            {
                _parameters.Remove(name);
                return;
            }
            if (value == false) return;
            if(_parameters.ContainsKey(name) == false)
            {
                _parameters.Add(name,"");
            }
        }

        private void UpdateParam(string name, double? value)
        {
            if(_parameters.ContainsKey(name) && value == null)
            {
                _parameters.Remove(name);
                return;
            }
            if (value == null) return;
            if(_parameters.ContainsKey(name) == false)
            {
                _parameters.Add(name, value.ToString());
                return;
            }
            _parameters[name] = value.ToString();
        }


        /// <summary>
        /// Attempts to parse known parameters from the set of proj4 parameters
        /// </summary>
        /// <param name="proj4string"></param>
        public void ReadProj4String(string proj4string)
        {
            string[] paramList = proj4string.Split('+');
            foreach (string s in paramList)
            {
                if (s == "") continue;
                if (s == "no_defs") continue;
                string[] temp = s.Split('=');
                string name = temp[0].Trim();
                string value = "";
                if (temp.Length > 1)
                {
                    value = temp[1];
                    if (value != null) value = value.Trim();
                }
                
                if(_parameters.ContainsKey(name))
                {
                    // some "+to" parameters exist... but I'm not sure what to do with them
                    // asside from the fact that they seem to specify a second projection?
                }
                else
                {
                    _parameters.Add(name, value);
                }
                
            }
            GeographicInfo.ReadProj4Parameters(_parameters);
            if(_parameters.ContainsKey("lonc"))
            {
                _longitudeOfCenter = double.Parse(_parameters["lonc"]);   
            }
            if(_parameters.ContainsKey("alhpa"))
            {
                _azimuth = double.Parse(_parameters["alpha"]);
            }
            if (_parameters.ContainsKey("x_0"))
            {
                _falseEasting = double.Parse(_parameters["x_0"]);
            }
            if (_parameters.ContainsKey("y_0"))
            {
                _falseNorthing = double.Parse(_parameters["y_0"]);
            }
            if (_parameters.ContainsKey("k"))
            {
                _scaleFactor = double.Parse(_parameters["k"]);
            }
            if (_parameters.ContainsKey("lat_0"))
            {
                _latitudeOfOrigin = double.Parse(_parameters["lat_0"]);
            }
            if(_parameters.ContainsKey("lat_1"))
            {
                _standardParallel1 = double.Parse(_parameters["lat_1"]);
            }
            if (_parameters.ContainsKey("lat_ts"))
            {
                _standardParallel1 = double.Parse(_parameters["lat_ts"]);
            }
            if (_parameters.ContainsKey("lat_2"))
            {
                _standardParallel2 = double.Parse(_parameters["lat_2"]);
            }
            if (_parameters.ContainsKey("lon_0"))
            {
                _centralMeridian = double.Parse(_parameters["lon_0"]);
            }
            if (_parameters.ContainsKey("south"))
            {
                _south = true;
            }
            if (_parameters.ContainsKey("zone"))
            {
                _zone = int.Parse(_parameters["zone"]);
            }
            
            if (_parameters.ContainsKey("geoc"))
            {
                _geoc = bool.Parse(_parameters["geoc"]) && (GeographicInfo.Datum.Spheroid.EccentricitySquared() != 0);
            }
            if (_parameters.ContainsKey("over"))
            {
                _over = bool.Parse(_parameters["over"]);
            }
            if (_parameters.ContainsKey("proj"))
            {
                _transform = TransformManager.DefaultTransformManager.GetProj4(_parameters["proj"]);
                _transform.Init(this);
            }
    
        }
        /// <summary>
        /// A boolean that indicates whether the proj4 parameter "nodefs" appears.
        /// </summary>
        public bool NoDefs
        {
            get { return _nodefs; }
            set { _nodefs = value; }
        }
        /// <summary>
        /// Parses the entire projection from an esri string.  In some cases, this will have
        /// default projection information since only geographic information is obtained.
        /// </summary>
        /// <param name="esriString">The ESRI string to parse</param>
        public void ReadEsriString(string esriString)
        {
            if(esriString.Contains("PROJCS") == false)
            {
                _geographicInfo.ReadEsriString(esriString);
                _isLatLon = true;
                _transform = new LongLat();
                _transform.Init(this);
                return;
            }
            int iStart = esriString.IndexOf(@""",");
            _name = esriString.Substring(8, iStart - 8);
            int iEnd = esriString.IndexOf("PARAMETER");
            string gcs = esriString.Substring(iStart+1,iEnd-(iStart+2));
            _geographicInfo.ReadEsriString(gcs);
            
            _falseEasting = GetParameter("False_Easting", esriString);
            _falseNorthing = GetParameter("False_Northing", esriString);
            _centralMeridian = GetParameter("Central_Meridian", esriString);
            _longitudeOfCenter = GetParameter("Longitude_Of_Center", esriString);
            _standardParallel1 = GetParameter("Standard_Parallel_1", esriString);
            _standardParallel2 = GetParameter("Standard_Parallel_2", esriString);
            _scaleFactor = GetParameter("Scale_Factor", esriString);
            _azimuth = GetParameter("Azimuth", esriString);
            _longitudeOf1st = GetParameter("Longitude_Of_1st", esriString);
            _longitudeOf2nd = GetParameter("Longitude_Of_2nd", esriString);
            _latitudeOfOrigin = GetParameter("Latitude_Of_Origin", esriString) ??
                                GetParameter("Latitude_Of_Center", esriString);
            iStart = esriString.LastIndexOf("UNIT");
            string unit = esriString.Substring(iStart, esriString.Length - iStart);
            _unit.ReadEsriString(unit);

            if (esriString.Contains("PROJECTION"))
            {
                iStart = esriString.IndexOf("PROJECTION") + 12;
                iEnd = esriString.IndexOf("]", iStart) - 1;
                string projection = esriString.Substring(iStart, iEnd - iStart);
                _transform = TransformManager.DefaultTransformManager.GetProjection(projection);
                _transform.Init(this);
            }
            
            
        }

        private static double? GetParameter(string name, string esriString)
        {
            double? result = null;
            int iStart = esriString.IndexOf(@"PARAMETER[""" + name, StringComparison.InvariantCultureIgnoreCase);
            if (iStart >= 0)
            {
                iStart += 13 + name.Length;
                int iEnd = esriString.IndexOf(",", iStart) - 1;
                string tst = esriString.Substring(iStart, iEnd - iStart);
                result = double.Parse(tst);
            }
            return result;
        }

        #endregion
    }
}
