using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWindow.Tools;
using MapWindow.Tools.Param;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;

namespace MapWindowTools.Analysis
{
    class AR_1__Model : ITool
    {
        #region Private members

        private Parameter[] _inputParameters;
        private Parameter[] _outputParameters;
        private DataTable tblDataTable;

        decimal forecastingValue;
        private string _workingPath;

        private Decimal SumX;
        private Decimal SumY;
        private Decimal SumSqrX;
        private Decimal SumXY;

        #endregion

        #region Private Methods

        private DataTable ParseCSV(string inputString)
        {

            DataTable dt = new DataTable();

            // declare the Regular Expression that will match versus the input string
            Regex re = new Regex("((?<field>[^\",\\r\\n]+)|\"(?<field>([^\"]|\"\")+)\")(,|(?<rowbreak>\\r\\n|\\n|$))");

            ArrayList colArray = new ArrayList();
            ArrayList rowArray = new ArrayList();

            int colCount = 0;
            int maxColCount = 0;
            string rowbreak = "";
            string field = "";

            MatchCollection mc = re.Matches(inputString);

            foreach (Match m in mc)
            {

                // retrieve the field and replace two double-quotes with a single double-quote
                field = m.Result("${field}").Replace("\"\"", "\"");

                rowbreak = m.Result("${rowbreak}");

                if (field.Length > 0)
                {
                    colArray.Add(field);
                    colCount++;
                }

                if (rowbreak.Length > 0)
                {

                    // add the column array to the row Array List
                    rowArray.Add(colArray.ToArray());

                    // create a new Array List to hold the field values
                    colArray = new ArrayList();

                    if (colCount > maxColCount)
                        maxColCount = colCount;

                    colCount = 0;
                }
            }

            if (rowbreak.Length == 0)
            {
                // this is executed when the last line doesn't
                // end with a line break
                rowArray.Add(colArray.ToArray());
                if (colCount > maxColCount)
                    maxColCount = colCount;
            }

            // convert the row Array List into an Array object for easier access
            Array ra = rowArray.ToArray();
            Array ss;

            for (int t = 0; t < 1; t++)
            {
                // convert the column Array List into an Array object for easier access
                ss = (Array)(ra.GetValue(t));


                // create the columns for the table
                for (int i = 0; i < ss.Length; i++)
                    dt.Columns.Add(ss.GetValue(i).ToString());
            }

            for (int i = 7; i < ra.Length; i++)
            {
                // create a new DataRow
                DataRow dr = dt.NewRow();

                // convert the column Array List into an Array object for easier access
                Array ca = (Array)(ra.GetValue(i));

                // add each field into the new DataRow
                for (int j = 0; j < ca.Length; j++)
                    dr[j] = ca.GetValue(j);

                // add the new DataRow to the DataTable
                dt.Rows.Add(dr);
            }

            // in case no data was parsed, create a single column
            if (dt.Columns.Count == 0)
                dt.Columns.Add("NoData");

            return dt;
        }

        private DataTable ParseCSVFile(string path)
        {

            string inputString = "";

            // check that the file exists before opening it
            if (File.Exists(path))
            {

                StreamReader sr = new StreamReader(path);
                inputString = sr.ReadToEnd();
                sr.Close();

            }

            return ParseCSV(inputString);
        }

        private void DataTable2CSV(DataTable table, string filename, string seperateChar)
        {
            StreamWriter sr = null;
            try
            {

                sr = new StreamWriter(filename);
                string seperator = "";
                StringBuilder builder = new StringBuilder();
                foreach (DataColumn col in table.Columns)
                {

                    builder.Append(seperator).Append(col.ColumnName);

                    seperator = seperateChar;
                }

                sr.WriteLine(builder.ToString());

                foreach (DataRow row in table.Rows)
                {

                    seperator = "";
                    builder = new StringBuilder();
                    foreach (DataColumn col in table.Columns)
                    {

                        builder.Append(seperator).Append(row[col.ColumnName]);
                        seperator = seperateChar;

                    }

                    sr.WriteLine(builder.ToString());

                }

            }

            finally
            {

                if (sr != null)
                {

                    sr.Close();

                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal ForecastingValuefunction(DataTable dt, double value)
        {
            Decimal[] Xd = new decimal[dt.Rows.Count];

            Decimal[] Yd = new decimal[dt.Rows.Count];

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                try
                {
                    //The value of Tday
                    Xd[k] = Convert.ToDecimal(dt.Rows[k][1]);

                    //The Value of T-1 Day
                    Yd[k] = Convert.ToDecimal(dt.Rows[k][2]);

                    SumX += Convert.ToDecimal(dt.Rows[k][1]);

                    SumY += Convert.ToDecimal(dt.Rows[k][2]);

                    SumXY += Convert.ToDecimal(dt.Rows[k][1]) * Convert.ToDecimal(dt.Rows[k][2]);

                    SumSqrX += Convert.ToDecimal(dt.Rows[k][1]) * Convert.ToDecimal(dt.Rows[k][1]);
                }
                catch
                {

                }


            }

            //Calculate m value
            decimal mstep1 = Convert.ToDecimal(dt.Rows.Count) * SumXY;

            decimal mstep2 = SumX * SumY;

            decimal mstep3 = Convert.ToDecimal(dt.Rows.Count) * SumSqrX;

            decimal mstep4 = SumX * SumX;

            decimal mstep5 = mstep1 - mstep2;

            decimal mstep6 = mstep3 - mstep4;

            decimal m = mstep5 / mstep6;

            //Calculate c value

            decimal cstep1 = Convert.ToDecimal(1.0 / dt.Rows.Count);

            decimal cstep2 = cstep1 * SumY;

            decimal cstep3 = cstep1 * SumX;

            decimal cstep4 = m * cstep3;

            decimal cstep5 = cstep2 - cstep4;

            decimal c = cstep5;

            //equation is y=mx+c
            //y is forecasting value
            //x is current value

            //Y is Currect value
            //X is Forecasting Value
            //X= ( Y - C ) / m

            decimal ForecastingResult = (Convert.ToDecimal(value) - c) / m;

            //    decimal forecastingvalue = m * Convert.ToDecimal(value) + c;

            return ForecastingResult;

        }

        #endregion

        #region ITool Members

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        Parameter[] ITool.InputParameters
        {
            get { return (_inputParameters); }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        Parameter[] ITool.OutputParameters
        {
            get { return (_outputParameters); }
        }


        public string Author
        {
            get { return "AR(1) Model"; }
        }

        public string Category
        {
            get { return "Forecasting"; }
        }

        public string Description
        {
            get { return "AR(1) Model Tool"; }
        }

        bool ITool.Execute(ICancelProgressHandler cancelProgressHandler)
        {
            string fileName = _inputParameters[0].Value.ToString();
            string destinationfileName = _outputParameters[0].Value.ToString();
            if (Execute(fileName, destinationfileName, cancelProgressHandler) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Execute(string CSVFilePath, string CSVDestination, ICancelProgressHandler cancelProgressHandler)
        {
            tblDataTable = ParseCSVFile(CSVFilePath);

            Decimal[] ForecastingValue = new decimal[tblDataTable.Rows.Count];
            Decimal[] ActualValues = new decimal[tblDataTable.Rows.Count];
            DateTime[] Date = new DateTime[tblDataTable.Rows.Count];

            for (int j = 0; j < tblDataTable.Rows.Count; j++)
            {
                try
                {
                    double t = Convert.ToDouble(tblDataTable.Rows[j][2]);

                    //Here the forecasting value is gettting based on the actual value 
                    //ex: day t is forecasted based on the t-1 value
                    forecastingValue = ForecastingValuefunction(tblDataTable, t);

                    //Here forecasting is done based on the forecasted value
                    //ex: t day is forecastedf based on the t-1 forecasted value
                    //if (forecastingValue == 0)
                    //{
                    //    forecastingValue = ForecastingValuefunction(tblDataTable, t);
                    //    ForecastingValue[j] = forecastingValue;
                    //}
                    //else
                    //{
                    //    double dt = Convert.ToDouble(ForecastingValue[j-1]);

                    //    forecastingValue = ForecastingValuefunction(tblDataTable, dt);

                    //    ForecastingValue[j] = forecastingValue;
                    //}

                    //
                    ForecastingValue[j] = forecastingValue;
                    ActualValues[j] = Convert.ToDecimal(tblDataTable.Rows[j][1]);
                    Date[j] = Convert.ToDateTime(tblDataTable.Rows[j][0]);

                    cancelProgressHandler.Progress("", Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(ActualValues[j])) * 100), ActualValues[j].ToString());
                    if (cancelProgressHandler.Cancel)
                        return false;
                }
                catch
                {

                }

            }

            DataTable dt1 = new DataTable();

            dt1.Columns.Add("Actual Values");
            dt1.Columns.Add("Forecasting Values");
            dt1.Columns.Add("Date");

            for (int g = 0; g < ForecastingValue.Length; g++)
            {
                DataRow dr = dt1.NewRow();
                dr[0] = Math.Pow(10, Convert.ToDouble(ActualValues[g]));

                dr[1] = Math.Pow(10, Convert.ToDouble(ForecastingValue[g]));

                //dr[0] = ActualValues[g];

                //dr[1] = ForecastingValue[g];

                dr[2] = Date[g];

                dt1.Rows.Add(dr);

            }

            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    DataTable2CSV(dt1, CSVDestination, ",");
                    cancelProgressHandler.Progress("", Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(dt1.Rows.Count)) * 100), dt1.Rows[j][0].ToString());
                    if (cancelProgressHandler.Cancel)
                        return false;
                }
            }
            else
            {
                return false;
            }

            return true;
            //decimal testValue = ForecastingValue(tblDataTable, 2.250420002);


        }

        public System.Drawing.Bitmap HelpImage
        {
            get { return null; }
        }

        public string HelpText
        {
            get { return "AR(1) Tool Help"; }
        }

        public string HelpURL
        {
            get { return "AR(1) Model"; }
        }

        public System.Drawing.Bitmap Icon
        {
            get { return (null); }
        }

        void ITool.Initialize()
        {
            _inputParameters = new Parameter[1];

            _inputParameters[0] = new OpenFileParam("Select the Data table");

            _outputParameters = new Parameter[1];
            _outputParameters[0] = new SaveFilePram("Save Lag Values");
        }


        public string Name
        {
            get { return "AR(1) Model"; }
        }


        void ITool.ParameterChanged(Parameter sender)
        {
            return;
        }

        public string ToolTip
        {
            get { return "AR(1) Model"; }
        }

        public string UniqueName
        {
            get { return "AR(1) Tool"; }
        }

        public Version Version
        {
            get { return (new Version(1, 0, 0, 0)); }
        }

        string ITool.WorkingPath
        {
            set { _workingPath = value; }
        }

        #endregion


    }
}
