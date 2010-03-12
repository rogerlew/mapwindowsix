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
    class LagTool : ITool
    {
        private Parameter[] _inputParameters;
        private Parameter[] _outputParameters;

        private DataTable tblDataTable;

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

            for (int i = 1; i < ra.Length; i++)
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

        #endregion

        #region ITool Members

        public string Author
        {
            get { return "Lag Tool"; }
        }

        public string Category
        {
            get { return "Forecasting"; }
        }

        public string Description
        {
            get { return "Forecasting Lag Tool"; }
        }

        public bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            string fileName = _inputParameters[0].Value.ToString();
            string destinationfileName = _outputParameters[0].Value.ToString();
            Execute(fileName, destinationfileName, cancelProgressHandler);
            return true;
        }

        public bool Execute(string CSVFilePath, string CSVDestination, ICancelProgressHandler cancelProgressHandler)
        {
            tblDataTable = ParseCSVFile(CSVFilePath);
            DataTable LagValueTable = new DataTable();
            LagValueTable = LagTable(tblDataTable);
            for (int j = 0; j < LagValueTable.Rows.Count; j++)
            {
                DataTable2CSV(LagValueTable, CSVDestination, ",");
                cancelProgressHandler.Progress("", Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(LagValueTable.Rows.Count)) * 100), LagValueTable.Rows[j][0].ToString() + ":" + LagValueTable.Rows[j][1].ToString());
                if (cancelProgressHandler.Cancel)
                    return false;
            }

            return true;
        }

        public DataTable LagTable(DataTable dt)
        {
            DataTable dtLagTable = new DataTable();

            dtLagTable.Columns.Add("Date");
            dtLagTable.Columns.Add("t");
            dtLagTable.Columns.Add("t-1");
            dtLagTable.Columns.Add("t-2");
            dtLagTable.Columns.Add("t-3");
            dtLagTable.Columns.Add("t-4");
            dtLagTable.Columns.Add("t-5");
            dtLagTable.Columns.Add("t-6");

            for (int l = 1; l < dt.Rows.Count; l++)
            {
                DataRow dr = dtLagTable.NewRow();

                dr[0] = dt.Rows[l][0];
                dr[1] = Math.Log10(Convert.ToDouble(dt.Rows[l][1]));
                dr[2] = Math.Log10(Convert.ToDouble(dt.Rows[l - 1][1]));

                if (l > 1)
                {
                    dr[3] = Math.Log10(Convert.ToDouble(dt.Rows[l - 2][1]));
                }

                if (l > 2)
                {
                    dr[4] = Math.Log10(Convert.ToDouble(dt.Rows[l - 3][1]));
                }

                if (l > 4)
                {
                    dr[5] = Math.Log10(Convert.ToDouble(dt.Rows[l - 4][1]));
                }

                if (l > 5)
                {
                    dr[6] = Math.Log10(Convert.ToDouble(dt.Rows[l - 4][1]));
                }

                if (l > 6)
                {
                    dr[7] = Math.Log10(Convert.ToDouble(dt.Rows[l - 4][1]));
                }

                dtLagTable.Rows.Add(dr);
            }

            return dtLagTable;
        }




        public System.Drawing.Bitmap HelpImage
        {
            get { return null; }
        }

        public string HelpText
        {
            get { return "Lag Tool Help"; }
        }

        public string HelpURL
        {
            get { return "LagTool"; }
        }

        public System.Drawing.Bitmap Icon
        {
            get { return null; }
        }

        public void Initialize()
        {
            _inputParameters = new Parameter[1];

            _inputParameters[0] = new OpenFileParam("Select the Data table");


            _outputParameters = new Parameter[1];
            _outputParameters[0] = new SaveFilePram("Save Lag Values");
        }

        public Parameter[] InputParameters
        {
            get { return _inputParameters; }
        }

        public string Name
        {
            get { return "Lag Tool"; }
        }

        public Parameter[] OutputParameters
        {
            get { return _outputParameters; }
        }

        public void ParameterChanged(Parameter sender)
        {

        }

        public string ToolTip
        {
            get { return "Lag Tool"; }
        }

        public string UniqueName
        {
            get { return "Lag Tool"; }
        }

        public Version Version
        {
            get { return (new Version(1, 0, 0, 0)); }
        }

        public string WorkingPath
        {
            set { }
        }

        #endregion
    }
}
