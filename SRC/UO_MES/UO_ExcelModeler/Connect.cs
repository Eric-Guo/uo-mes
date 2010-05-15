using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using Extensibility;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace UO_ExcelModeler
{
	#region Read me for Add-in installation and setup information.
	// When run, the Add-in wizard prepared the registry for the Add-in.
	// At a later time, if the Add-in becomes unavailable for reasons such as:
	//   1) You moved this project to a computer other than which is was originally created on.
	//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
	//   3) Registry corruption.
	// you will need to re-register the Add-in by building the MyAddin1Setup project, 
	// right click the project in the Solution Explorer, then choose install.
	#endregion
	
	/// <summary>
	///   The object for implementing an Add-in for Excel.
	/// </summary>
	/// <seealso class='IDTExtensibility2' />
	[GuidAttribute("20E39E45-75CA-4123-8C45-031FB0632F44"), ProgId("UO_ExcelModeler.Connect")]
    public class Connect : Object, Extensibility.IDTExtensibility2, IWin32Window 
	{
        private CommandBarButton btnUpload;
        private void btnUpload_Click(CommandBarButton cmdBarbutton, ref bool cancel)
        {
            Excel._Application app = excelApp as Excel._Application;
            Excel.Workbook workBook = app.ActiveWorkbook;
            if (true == CheckWorkbookIsReady(workBook, ref cancel))
            {
                object missing = System.Reflection.Missing.Value;
                string workBookFullName = workBook.FullName;
                workBook.Close(false, missing, missing);

                excelModeler.Import(workBookFullName);

                app.Workbooks.Open(workBookFullName,
                    missing, missing, missing, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing, missing);
            }
            workBook = null;
            app = null;
        }

        private CommandBarButton btnCleanToInsert;
        private void btnCleanToInsert_Click(CommandBarButton cmdBarbutton, ref bool cancel)
        {
            Excel._Application app = excelApp as Excel._Application;
            Excel.Workbook workBook = app.ActiveWorkbook;
            if (true == CheckWorkbookIsReady(workBook, ref cancel))
            {
                object missing = System.Reflection.Missing.Value;
                string workBookFullName = workBook.FullName;
                workBook.Close(false, missing, missing);

                excelModeler.CleanToInsertStatus(workBookFullName);

                app.Workbooks.Open(workBookFullName,
                    missing, missing, missing, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing, missing);
            }
            workBook = null;
            app = null;
        }

        private CommandBarButton btnWorkflowEditor;
        private void btnWorkflowEditor_Click(CommandBarButton cmdBarbutton, ref bool cancel)
        {
            using (WorkflowEditor workflowEditor = new WorkflowEditor())
            {
                workflowEditor.ShowDialog(this);
            }
        }

        private bool CheckWorkbookIsReady(Excel.Workbook workBook, ref bool cancel)
        {
            if (null == workBook)
            {
                MessageBox.Show(ResX.Workbook_needed, ResX.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancel = true;
                return false;
            }
            if (workBook.Saved == false || workBook.Path == "")
            {
                MessageBox.Show(ResX.Workbook_need_save, ResX.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancel = true;
                return false;
            }
            return true;
        }

        private CommandBarButton btnSetting;
        private void btnSetting_Click(CommandBarButton cmdBarbutton, ref bool cancel)
        {
            using (SettingDlg settingDlg = new SettingDlg())
            {
                if(settingDlg.ShowDialog(this) == DialogResult.OK)
                {
                    RemoveExcelMenu();
                    System.Threading.Thread.CurrentThread.CurrentUICulture = settingDlg.SettedCultureInfo;
                    NeedShowCleanButton = settingDlg.ShowCleanBtn;
                    BuldExcelMenu();
                }
            }
        }

        private CommandBarButton btnAbout;
        private void btnAbout_Click(CommandBarButton cmdBarbutton, ref bool cancel)
        {
            using (AboutBox about = new AboutBox())
            {
                about.ShowDialog(this);
            }
        }

        private ExcelModeler excelModeler;
		/// <summary>
		///		Implements the constructor for the Add-in object.
		///		Place your initialization code within this method.
		/// </summary>
		public Connect()
		{
            string dllPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            excelModeler = new ExcelModeler(dllPath + @"\\UO_Model.dll", dllPath + @"\\UO_Service.dll");

            using (RegistryKey rkSetting = Registry.CurrentUser.CreateSubKey(ExcelModeler.regSettingPath))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo((string)rkSetting.GetValue("Language", "en-US"));
                NeedShowCleanButton = bool.Parse(rkSetting.GetValue("ShowClean", "False").ToString());
            }
		}

		/// <summary>
		///      Implements the OnConnection method of the IDTExtensibility2 interface.
		///      Receives notification that the Add-in is being loaded.
		/// </summary>
		/// <param term='application'>
		///      Root object of the host application.
		/// </param>
		/// <param term='connectMode'>
		///      Describes how the Add-in is being loaded.
		/// </param>
		/// <param term='addInInst'>
		///      Object representing this Add-in.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref System.Array custom)
		{
            excelApp = application;
			addInInstance = addInInst;

            if (connectMode != Extensibility.ext_ConnectMode.ext_cm_Startup)
            {
                OnStartupComplete(ref custom);
            }
		}

		/// <summary>
		///     Implements the OnDisconnection method of the IDTExtensibility2 interface.
		///     Receives notification that the Add-in is being unloaded.
		/// </summary>
		/// <param term='disconnectMode'>
		///      Describes how the Add-in is being unloaded.
		/// </param>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref System.Array custom)
		{
            if (disconnectMode != Extensibility.ext_DisconnectMode.ext_dm_HostShutdown)
            {
                OnBeginShutdown(ref custom);
            }
            excelApp = null;
		}

		/// <summary>
		///      Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
		///      Receives notification that the collection of Add-ins has changed.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnAddInsUpdate(ref System.Array custom)
		{
		}

		/// <summary>
		///      Implements the OnStartupComplete method of the IDTExtensibility2 interface.
		///      Receives notification that the host application has completed loading.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref System.Array custom)
		{
            BuldExcelMenu();
        }

        #region Menu handler
        private void RemoveExcelMenu()
        {
            object omissing = System.Reflection.Missing.Value;
            CommandBars oCommandBars;
            CommandBar oMenuBar;

            oCommandBars = (CommandBars)excelApp.GetType().InvokeMember("CommandBars", BindingFlags.GetProperty, null, excelApp, null);

            oMenuBar = oCommandBars.ActiveMenuBar;

            CommandBarPopup oModelerMenu;
            oModelerMenu = (CommandBarPopup)oMenuBar.Controls[ResX.Modeler_menu];
            oModelerMenu.Delete(omissing);

            oModelerMenu = null;
            oMenuBar = null;
            oCommandBars = null;
        }

        private void BuldExcelMenu()
        {
            object omissing = System.Reflection.Missing.Value;
            CommandBars oCommandBars;
            CommandBar oMenuBar;
            CommandBarPopup oModelerMenu;

            oCommandBars = (CommandBars)excelApp.GetType().InvokeMember("CommandBars", BindingFlags.GetProperty, null, excelApp, null);

            oMenuBar = oCommandBars.ActiveMenuBar;

            try
            {
                oModelerMenu = (CommandBarPopup)oMenuBar.Controls[ResX.Modeler_menu];
            }
            catch (Exception)
            {
                oModelerMenu = (CommandBarPopup)oMenuBar.Controls.Add(MsoControlType.msoControlPopup, omissing, omissing, omissing, true);
                oModelerMenu.Caption = ResX.Modeler_menu;
            }

            #region Add btnUpload
            try
            {
                btnUpload = (CommandBarButton)oModelerMenu.Controls[ResX.btnUploadCaption];
            }
            catch (Exception)
            {
                btnUpload = (CommandBarButton)oModelerMenu.Controls.Add(MsoControlType.msoControlButton, omissing, omissing, omissing, true);
                btnUpload.Style = MsoButtonStyle.msoButtonIconAndCaption;
                btnUpload.Picture = ImageHost.GettIPictureDispFromPicture(ResX.UploadSite);
                btnUpload.Caption = ResX.btnUploadCaption;
            }

            // The following items are optional, but recommended. 
            //The Tag property lets you quickly find the control 
            //and helps MSO keep track of it when more than
            //one application window is visible. The property is required
            //by some Office applications and should be provided.
            btnUpload.Tag = ResX.btnUploadCaption;

            // The OnAction property is optional but recommended. 
            //It should be set to the ProgID of the add-in, so that if
            //the add-in is not loaded when a user presses the button,
            //MSO loads the add-in automatically and then raises
            //the Click event for the add-in to handle. 
            btnUpload.OnAction = "!<UO_ExcelModeler.Connect>";

            btnUpload.Visible = true;
            btnUpload.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(this.btnUpload_Click);
            #endregion

            #region Add btnCleanToInsert
            if (NeedShowCleanButton == true)
            {
                try
                {
                    btnCleanToInsert = (CommandBarButton)oModelerMenu.Controls[ResX.btnCleanToInsertCaption];
                }
                catch (Exception)
                {
                    btnCleanToInsert = (CommandBarButton)oModelerMenu.Controls.Add(MsoControlType.msoControlButton, omissing, omissing, omissing, true);
                    btnCleanToInsert.Style = MsoButtonStyle.msoButtonIconAndCaption;
                    btnCleanToInsert.FaceId = 47;
                    btnCleanToInsert.Caption = ResX.btnCleanToInsertCaption;
                }
                btnCleanToInsert.Tag = ResX.btnCleanToInsertCaption;
                btnCleanToInsert.OnAction = "!<UO_ExcelModeler.Connect>";
                btnCleanToInsert.Visible = true;
                btnCleanToInsert.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(this.btnCleanToInsert_Click);
            }
            #endregion

            #region Add btnWorkflowEditor
            try
            {
                btnWorkflowEditor = (CommandBarButton)oModelerMenu.Controls[ResX.btnWorkflowEditorCaption];
            }
            catch (Exception)
            {
                btnWorkflowEditor = (CommandBarButton)oModelerMenu.Controls.Add(MsoControlType.msoControlButton, omissing, omissing, omissing, true);
                btnWorkflowEditor.Style = MsoButtonStyle.msoButtonIconAndCaption;
                //btnWorkflowEditor.FaceId = 1044;
                btnWorkflowEditor.Picture = ImageHost.GettIPictureDispFromPicture(ResX.WorkflowEditor);
                btnWorkflowEditor.Caption = ResX.btnWorkflowEditorCaption;
            }
            btnWorkflowEditor.Tag = ResX.btnWorkflowEditorCaption;
            btnWorkflowEditor.OnAction = "!<UO_ExcelModeler.Connect>";
            btnWorkflowEditor.Visible = true;
            btnWorkflowEditor.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(this.btnWorkflowEditor_Click);
            #endregion

            #region Add btnSetting
            try
            {
                btnSetting = (CommandBarButton)oModelerMenu.Controls[ResX.btnSettingCaption];
            }
            catch (Exception)
            {
                btnSetting = (CommandBarButton)oModelerMenu.Controls.Add(MsoControlType.msoControlButton, omissing, omissing, omissing, true);
                btnSetting.Style = MsoButtonStyle.msoButtonIconAndCaption;
                btnSetting.FaceId = 2932;
                btnSetting.Caption = ResX.btnSettingCaption;
            }
            btnSetting.Tag = ResX.btnSettingCaption;
            btnSetting.OnAction = "!<UO_ExcelModeler.Connect>";
            btnSetting.Visible = true;
            btnSetting.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(this.btnSetting_Click);
            #endregion

            #region Add btnAbout
            try
            {
                btnAbout = (CommandBarButton)oModelerMenu.Controls[ResX.btnAboutCaption];
            }
            catch (Exception)
            {
                btnAbout = (CommandBarButton)oModelerMenu.Controls.Add(MsoControlType.msoControlButton, omissing, omissing, omissing, true);
                btnAbout.Style = MsoButtonStyle.msoButtonIconAndCaption;
                btnAbout.FaceId = 487;
                btnAbout.BeginGroup = true;
                btnAbout.Caption = ResX.btnAboutCaption;
            }
            btnAbout.Tag = ResX.btnAboutCaption;
            btnAbout.OnAction = "!<UO_ExcelModeler.Connect>";
            btnAbout.Visible = true;
            btnAbout.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(this.btnAbout_Click);
            #endregion

            oModelerMenu = null;
            oMenuBar = null;
            oCommandBars = null;
        }
        #endregion

        /// <summary>
		///      Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
		///      Receives notification that the host application is being unloaded.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref System.Array custom)
		{
            object omissing = System.Reflection.Missing.Value;
            btnUpload.Delete(omissing);
            btnUpload = null;
		}

        private bool needShowCleanButton;
        public bool NeedShowCleanButton
        {
            get { return needShowCleanButton; }
            set { needShowCleanButton = value; }
        }

        private object excelApp;
		private object addInInstance;

        #region COM Register Helper
        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type t)
        {
            using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(excelAddinsRegPath))
            {
                rk.SetValue("FriendlyName", ResX.AppName, RegistryValueKind.String);
                rk.SetValue("Description", ResX.AppDescription, RegistryValueKind.String);
                rk.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
            }

        }

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type t)
        {
            Registry.CurrentUser.DeleteSubKey(excelAddinsRegPath, false);
        }

        private static string excelAddinsRegPath = @"SOFTWARE\Microsoft\Office\Excel\Addins\UO_ExcelModeler.Connect";
        #endregion

        #region IWin32Window implement
        IntPtr IWin32Window.Handle
        {
            get { return new IntPtr((excelApp as Excel._Application).Hwnd); }
        }
        #endregion
    }
}