using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Globalization;

namespace UO_ExcelModeler
{
    public partial class SettingDlg : Form
    {
        public SettingDlg()
        {
            InitializeComponent();
        }

        private const string lngEnglish = "English";
        private const string lngChinese = "ÖÐÎÄ";

        private void SettingDlg_Load(object sender, EventArgs e)
        {
            this.Text = ResX.Modeler_setting;
            lblLanguage.Text = ResX.lblLanguage;
            chkShowCleanToInsertBtn.Text = ResX.chkShowCleanToInsertBtnCaption;
            btnOK.Text = ResX.btnOK;
            btnCancel.Text = ResX.btnCancel;

            cmbLanguage.Items.Add(lngEnglish);
            cmbLanguage.Items.Add(lngChinese);
            using (RegistryKey rkSetting = Registry.CurrentUser.CreateSubKey(ExcelModeler.regSettingPath))
            {
                string strCI = (string)rkSetting.GetValue("Language", "en-US");
                switch (strCI)
                {
                    case "zh-CN":
                        cmbLanguage.SelectedItem = lngChinese;
                        break;
                    case "en-US":
                    default:
                        cmbLanguage.SelectedItem = lngEnglish;
                        break;
                }
                chkShowCleanToInsertBtn.Checked = bool.Parse(rkSetting.GetValue("ShowClean", "False").ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string selLanguage = cmbLanguage.SelectedItem.ToString();
            string strCI = "en-US";
            switch (selLanguage)
            { 
                case lngChinese:
                    strCI = "zh-CN";
                    break;
                case lngEnglish:
                default:
                    strCI = "en-US";
                    break;
            }

            // Apply setting
            SettedCultureInfo = new CultureInfo(strCI);

            // Save setting
            using (RegistryKey rkSetting = Registry.CurrentUser.CreateSubKey(ExcelModeler.regSettingPath))
            {
                rkSetting.SetValue("Language", strCI, RegistryValueKind.String);
                rkSetting.SetValue("ShowClean", chkShowCleanToInsertBtn.Checked.ToString(), RegistryValueKind.String);
            }
        }

        public bool ShowCleanBtn
        {
            get { return chkShowCleanToInsertBtn.Checked; }
        }

        private CultureInfo settedCultureInfo = null;
        public CultureInfo SettedCultureInfo
        {
            get { return settedCultureInfo; }
            set { settedCultureInfo = value; }
        }
    }
}