using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MapWindow.Components;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using MapWindow.Map;
using MapWindow.XML;
using MapWindow.Main;
using DataSet=MapWindow.Data.DataSet;
using Point=MapWindow.Geometries.Point;

namespace MapWindow
{
    /// <summary>
    /// The main form for the project
    /// </summary>
    public partial class frmMapWindow6 : Form
    {
       
        /// <summary>
        /// Creates a new project form
        /// </summary>
        public frmMapWindow6()
        {
            Settings mySettings = new Settings(Environment.SpecialFolder.ApplicationData + @"\MapWindow6\Settings.bin");
            SettingInfo info = Settings.Info;
            Thread.CurrentThread.CurrentUICulture = info.PreferredCulture;
            Thread.CurrentThread.CurrentCulture = info.PreferredCulture;

            InitializeComponent();

            DataManager.DefaultDataManager.ProgressHandler = mwStatusStrip1;
            geoMap1.GeoMouseMove += geoMap1_GeoMouseMove;
            mwToolStrip1.PrintClicked += mwToolStrip1_PrintClicked;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;

        }

        void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            geoMap1.ResetBuffer();
        }

       
        private void AddLabels()
        {
            IFeatureLayer fl = geoMap1.AddFeatureLayer();
            string expression = "[" + fl.DataSet.DataTable.Columns[0].ColumnName + "]";
            LabelSymbolizer ls = new LabelSymbolizer();
            ls.Orientation = ContentAlignment.MiddleCenter;
            geoMap1.AddLabels(fl, expression, "", ls);
        }

        void geoMap1_GeoMouseMove(object sender, Map.GeoMouseArgs e)
        {
            statusLocation.Text = "X: " + e.GeographicLocation.X.ToString("0.0000000") + ", Y: " + e.GeographicLocation.Y.ToString("0.0000000");
        }

        #region Menu

      
        #endregion
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLayer();
        }
        private void OpenLayer()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Shapefiles|*.shp";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FeatureSet fs = new FeatureSet();
            fs.Open(ofd.FileName);
            geoMap1.Layers.Add(fs);
            FeatureSet newFeature = new FeatureSet();
            newFeature.CopyTableSchema(fs);
            foreach(Feature f in fs.Features)
            {
                newFeature.Features.Add(f);   
            }
            bool stop = true;
        }
 

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings frm = new frmSettings();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                Settings.Info = frm.SettingValues;
            }
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapWindow6));
            //resources.ApplyResources(fileToolStripMenuItem, "fileToolStripMenuItem", Thread.CurrentThread.CurrentCulture);
            
            resources.ApplyResources(fileToolStripMenuItem, "fileToolStripMenuItem");
            
        }

    

        private void mnuSelectByAttributes_Click(object sender, EventArgs e)
        {
            Forms.SelectByAttributes frmAtt = new Forms.SelectByAttributes(geoMap1.MapFrame);
            frmAtt.Show();
        }

        #region Printing

        void mwToolStrip1_PrintClicked(object sender, EventArgs e)
        {
            layoutToolStripMenuItem_Click(sender, e);
        }

        private void layoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.LayoutForm layoutFrm = new Forms.LayoutForm();
            layoutFrm.MapControl = geoMap1;
            layoutFrm.Show(this);
        }

        #endregion

        private void mwToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != mwToolStrip1.Items[11]) return;
            geoMap1.Layers.LayerSelected += Layers_LayerSelected;
        }

        void Layers_LayerSelected(object sender, LayerEventArgs e)
        {
            MessageBox.Show("Test!");
        }

        private void RemoveAndInsert()
        {
            FeatureSet cities = new FeatureSet();
            FeatureSet states = new FeatureSet();
            cities.Open(@"C:\dev\SampleData\HIS_Desktop\cities.shp");
            states.Open(@"C:\dev\SampleData\HIS_Desktop\states.shp");
            IMapFeatureLayer cityLayer = geoMap1.Layers.Add(cities);
            IMapFeatureLayer stateLayer = geoMap1.Layers.Add(states);
            geoMap1.Layers.Remove(stateLayer);
            geoMap1.Layers.Insert(0, stateLayer);
        }

        private void CreateLayers()
        {
            FeatureSet cities = new FeatureSet();
            FeatureSet states = new FeatureSet();
            cities.Open(@"C:\dev\SampleData\HIS_Desktop\cities.shp");
            states.Open(@"C:\dev\SampleData\HIS_Desktop\states.shp");
            IMapFeatureLayer cityLayer = new MapPointLayer(cities);
            IMapFeatureLayer stateLayer = new MapPolygonLayer(states);
            geoMap1.Layers.Insert(0, cityLayer);
            geoMap1.Layers.Insert(0, stateLayer);
        }

        private static IFeatureSet CreatePolygonFeatureSet()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            FeatureSet fs = new FeatureSet(FeatureTypes.Point);
            fs.DataTable.Columns.Add("Number", typeof(int));
            fs.DataTable.Columns.Add("Letter", typeof(string));
            fs.Name = "Test";
            for (int i = 0; i < 10; i++)
            {
                Coordinate center = NextCoordinate(rnd);
                List<Coordinate> shell = new List<Coordinate>();
                for (int j = 0; j < 10; j++)
                {
                    Coordinate c = new Coordinate();
                    double dx = 5*Math.Sin(Math.PI*(double) j/(double) 5);
                    double dy = 5*Math.Cos(Math.PI*(double) j/(double) 5);
                    c.X = center.X + dx;
                    c.Y = center.Y + dy;
                    shell.Add(c);
                }
                Polygon p = new Polygon(shell);
                Feature f = new Feature(p);
                fs.Features.Add(f);
                f.DataRow["Number"] = i;
                f.DataRow["Letter"] = "Shape " + i;
            }
            return fs;
        }

        private static IFeatureSet CreateLineFeatureSet()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            FeatureSet fs = new FeatureSet(FeatureTypes.Point);
            fs.DataTable.Columns.Add("Number", typeof(int));
            fs.DataTable.Columns.Add("Letter", typeof(string));
            fs.Name = "Test";
            for (int i = 0; i < 10; i++)
            {
                List<Coordinate> coords = new List<Coordinate>();
                for(int j = 0; j < 10; j++)
                {
                    coords.Add(NextCoordinate(rnd));
                }
                Feature f = new Feature(new LineString(coords));
                fs.Features.Add(f);
                f.DataRow["Number"] = i;
                f.DataRow["Letter"] = "Shape " + i;
            }
            return fs;
            
        }
        private IFeatureSet CreatePointFeatureset()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            FeatureSet fs = new FeatureSet(FeatureTypes.Point);
            fs.DataTable.Columns.Add("Number", typeof(int));
            fs.DataTable.Columns.Add("Letter", typeof(string));
            fs.Name = "Test";
            for(int i = 0; i < 10; i++)
            {
                Feature f = new Feature(new Point(NextCoordinate(rnd)));
                fs.Features.Add(f);
                f.DataRow["Number"] = i;
                f.DataRow["Letter"] = "Shape " + i;
            }
            return fs;
        }
        private static Coordinate NextCoordinate(Random rnd)
        {
            double x = rnd.NextDouble()*360 - 180;
            double y = rnd.NextDouble()*180 - 90;
            return new Coordinate(x, y);
        }

		private void openMapMenuItem_Click(object sender, EventArgs e)
		{
            if (MessageBox.Show(this, "Are you sure you want to clear all data and open a different map?", "Confirm new map", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
			var dlg = new OpenFileDialog();
			dlg.Filter = "Map Files|*.map.xml";
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
	        try
	        {
	            XmlDeserializer d = new XmlDeserializer();
	            d.Deserialize(geoMap1, File.ReadAllText(dlg.FileName));
                geoMap1.Invalidate();
	        }
	        catch (IOException)
	        {
	            MessageBox.Show(this, "Could not open the specified map file " + dlg.FileName, "Error",
	                            MessageBoxButtons.OK, MessageBoxIcon.Error);
	        }
	        catch (XmlException)
	        {
	            MessageBox.Show(this, "Failed to read the specified map file " + dlg.FileName, "Error",
	                            MessageBoxButtons.OK, MessageBoxIcon.Error);
	        }
		    
		}

		private void saveMapMenuItem_Click(object sender, EventArgs e)
		{
			var dlg = new SaveFileDialog();
			dlg.Filter = "Map Files|*.map.xml";
			dlg.SupportMultiDottedExtensions = true;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					XmlSerializer s = new XmlSerializer();
					string xml = s.Serialize(geoMap1);
					File.WriteAllText(dlg.FileName, xml);
				}
				catch (XmlException)
				{
					MessageBox.Show(this, "Failed to write the specified map file " + dlg.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch (IOException)
				{
					MessageBox.Show(this, "Could not write to the specified map file " + dlg.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void newMapMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, "Are you sure you want to clear all data and start a new map?", "Confirm new map", MessageBoxButtons.YesNo) == DialogResult.Yes)
				((ICollection<ILayer>)geoMap1.Layers).Clear();
		}

        
    }
}