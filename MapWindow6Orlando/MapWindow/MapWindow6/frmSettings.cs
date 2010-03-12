using System;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

namespace MapWindow
{
    /// <summary>
    /// A dialog for allowing the users to control settings
    /// </summary>
    public partial class frmSettings : Form
    {
        SettingInfo _settingValues;
        readonly CultureInfo[] _specificCultures;
        int[] _supportedIndices = new int[]{8, 66};

        /// <summary>
        /// Creates a new instance of the settings dialog
        /// </summary>
        public frmSettings()
        {
            InitializeComponent();

            if (DesignMode) return;
            _specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            string[] specificCultureNames = new string[_specificCultures.Length];
            for (int i = 0; i < _specificCultures.Length; i++)
            {
                specificCultureNames[i] = _specificCultures[i].DisplayName;
                    
            }
            foreach (int i in _supportedIndices)
            {
                cmbCultureInfo.Items.Add(specificCultureNames[i]);
            }
            //cmbCultureInfo.Items.Add(CultureInfo.CurrentCulture);
            //cmbCultureInfo.Items.AddRange(specificCultureNames);

            _settingValues = Settings.Info ?? new SettingInfo();
            cmbCultureInfo.Text = _settingValues.PreferredCulture.DisplayName;
        }

        private void cmbCultureInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settingValues.PreferredCulture = _specificCultures[_supportedIndices[cmbCultureInfo.SelectedIndex]];
            
            //if (cmbCultureInfo.SelectedIndex == 0)
            //{
            //    _settingValues.PreferredCulture = CultureInfo.CurrentCulture;
            //}
            //else
            //{
            //    _settingValues.PreferredCulture = _specificCultures[cmbCultureInfo.SelectedIndex-1];
            //}
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_settingValues == null) return;

            // This shares the value with mapwindow
            Globalization.CulturePreferences.CultureInformation = _settingValues.PreferredCulture;
            Thread.CurrentThread.CurrentCulture = _settingValues.PreferredCulture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            // This actually saves the preferences
            Settings.Info = _settingValues;
            MessageBox.Show(TextStrings.RestartNeeded);
            
        }

        /// <summary>
        /// Gets or sets the SettingInfo specifying the actual values from this dialog
        /// </summary>
        public SettingInfo SettingValues
        {
            get { return _settingValues; }
            set { _settingValues = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

       
    }
}