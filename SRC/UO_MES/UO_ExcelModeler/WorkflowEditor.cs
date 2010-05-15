using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Lassalle.Flow;
using UO_Service.Inquery;

namespace UO_ExcelModeler
{
    public partial class WorkflowEditor : Form
    {
        Lassalle.PrnFlow.PrnFlow prnflow = null;
        string strProgName = null;
        string strFileName = null;

        public WorkflowEditor()
        {
            InitializeComponent();
        }

        private void WorkflowEditor_Load(object sender, EventArgs e)
        {
            // Get the printer page size
            prnflow = new Lassalle.PrnFlow.PrnFlow();

            addFlow.SuspendLayout();
            addFlow.AutoScroll = false;
            addFlow.Images.Add(imagesWorkflow.Images["SpecStep"]);
            addFlow.Images.Add(imagesWorkflow.Images["StartSpecStep"]);
            addFlow.Images.Add(imagesWorkflow.Images["SubWorkflowStep"]);
            addFlow.Images.Add(imagesWorkflow.Images["StartSubWorkflowStep"]);
            // Snap & Display grid
            addFlow.Grid.Draw = true;
            addFlow.Grid.Snap = true;
            addFlow.Grid.Style = GridStyle.Pixels;
            addFlow.Grid.Color = Color.Silver;
            addFlow.Grid.Size = new Size(16, 16);

            // Display a printer page grid
            addFlow.PageGrid.Size = new Size((int)prnflow.GetPageSize(addFlow).Width, (int)prnflow.GetPageSize(addFlow).Height);
            addFlow.PageGrid.Color = Color.Chocolate;
            addFlow.PageGrid.Style = GridStyle.DottedLines;
            addFlow.PageGrid.Draw = true;
            addFlow.ResumeLayout();

            strProgName = this.Text;
            strFileName = "";
            MakeCaption();
            SelectionChangeHandle();
            cmbSelectNodeType.SelectedIndex = 0;
        }

        private void WorkflowEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!OkToTrash())
                e.Cancel = true;
        }

        private void addFlow_SelectionChange(object sender, SelectionChangeArgs e)
        {
            SelectionChangeHandle();
        }

        #region File Menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!OkToTrash())
                return;

            bool interactiveEventsOnly = addFlow.InteractiveEventsOnly;
            bool canUndoRedo = addFlow.CanUndoRedo;
            bool sendSelectionChangeEvent = addFlow.SendSelectionChangeEvent;

            addFlow.InteractiveEventsOnly = true;
            addFlow.SendSelectionChangeEvent = false;
            addFlow.CanUndoRedo = false;

            addFlow.Nodes.Clear();
            addFlow.ResetUndoRedo();
            addFlow.SetChangedFlag(false);

            addFlow.CanUndoRedo = canUndoRedo;
            addFlow.SendSelectionChangeEvent = sendSelectionChangeEvent;
            addFlow.InteractiveEventsOnly = interactiveEventsOnly;

            strFileName = "";
            MakeCaption();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Filter = "AddFlow Diagrams(*.xml)|*.xml|All Files(*.*)|*.*";
            ofd.FileName = "*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
                LoadFile(ofd.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (strFileName.Length == 0)
                SaveFileDlg();
            else
                SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDlg();
        }

        private void saveAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export as Image";
            sfd.DefaultExt = ".wmf";
            sfd.AddExtension = true;
            sfd.Filter = "Bitmap format(*.bmp)|*.bmp|Enhanced Windows metafile format (*.emf)|*.emf|Exchangeable Image File (*.exif)|*.exif|Graphics Interchange Format (*.gif)|*.gif|Windows icon format (*.ico)|*.ico|Joint Photographic Experts Group format(*.jpeg)|*.jpeg|W3C Portable Network Graphics format (*.png)|*.png|SVG file(*.svg)|*.svg|Tag Image File Format (*.tiff)|*.tiff|Windows metafile format (*.wmf)|*.wmf";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Metafile mf = addFlow.ExportMetafile(false, true, true);
                ImageFormat imf = ImageFormat.Bmp;

                switch (sfd.FilterIndex)
                {
                    case 1:
                        imf = ImageFormat.Bmp;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 2:
                        imf = ImageFormat.Emf;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 3:
                        imf = ImageFormat.Exif;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 4:
                        imf = ImageFormat.Gif;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 5:
                        imf = ImageFormat.Icon;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 6:
                        imf = ImageFormat.Jpeg;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 7:
                        imf = ImageFormat.Png;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 8:
                        System.Xml.XmlDocument oDocument = Lassalle.Flow.SVG.SVGFlow.FlowToSVG(addFlow);
                        oDocument.Save(sfd.FileName);
                        break;
                    case 9:
                        imf = ImageFormat.Tiff;
                        mf.Save(sfd.FileName, imf);
                        break;
                    case 10:
                        imf = ImageFormat.Wmf;
                        mf.Save(sfd.FileName, imf);
                        break;
                }
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prnflow.PageSetup();
            addFlow.PageGrid.Size = new Size((int)prnflow.GetPageSize(addFlow).Width, (int)prnflow.GetPageSize(addFlow).Height);
            addFlow.Invalidate();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prnflow.Print(addFlow);
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prnflow.Preview(addFlow);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Edit Menu
        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bool isItems = (addFlow.Items.Count > 0);
            bool isSelectedItems = (addFlow.SelectedItems.Count > 0);

            Item item = addFlow.SelectedItem;
            bool isCurrentNode = (item != null && item is Node);
            bool isCurrentLink = (item != null && item is Link);
            undoToolStripMenuItem.Enabled = addFlow.CanUndo;
            redoToolStripMenuItem.Enabled = addFlow.CanRedo;
            if (redoToolStripMenuItem.Enabled)
                redoToolStripMenuItem.Text = ResX.Redo + ActionName(addFlow.RedoCode);
            if (undoToolStripMenuItem.Enabled)
                undoToolStripMenuItem.Text = ResX.Undo + ActionName(addFlow.UndoCode);
            cutToolStripMenuItem.Enabled = isSelectedItems;
            copyToolStripMenuItem.Enabled = isSelectedItems;
            exportAsMetafileToolStripMenuItem.Enabled = isSelectedItems;
            pasteToolStripMenuItem.Enabled = Clipboard.GetDataObject().GetDataPresent("AddFlow.XMLFormat");
            deleteToolStripMenuItem.Enabled = isSelectedItems;

            selectAllToolStripMenuItem.Enabled = isItems;
            ungroupToolStripMenuItem.Enabled = isCurrentNode;
            zOrderBackToolStripMenuItem.Enabled = isCurrentNode || isCurrentLink;
            zOrderFrontToolStripMenuItem.Enabled = isCurrentNode || isCurrentLink;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
            deleteToolStripMenuItem_Click(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            addFlow.WriteXml(writer, true, false);
            DataObject data = new DataObject();
            data.SetData("AddFlow.XMLFormat", stream);
            Clipboard.SetDataObject(data, true);
        }

        private void exportAsMetafileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Metafile mf = addFlow.ExportMetafile(true, true, false);
            // The following line does not work. 
            //Clipboard.SetDataObject(mf, true);
            // Therefore we have replaced it by the next which copy the metafile to the
            // clipboard, using the old GDI Metafile format instead of the new GDI + format.
            // As a result, the antialiasing effect is lost.
            ClipboardMetafileHelper.PutEnhMetafileOnClipboard(this.Handle, mf);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();
            if (data.GetDataPresent("AddFlow.XMLFormat"))
            {
                // We first unselect the selected items
                addFlow.SelectedItems.Clear();

                // Get the data from the clipboard
                MemoryStream stream = (MemoryStream)data.GetData("AddFlow.XMLFormat");

                // We paste the portion of the diagram in our AddFlow control.
                // We group all the load actions in only one action so that we could undo
                // the paste action in one time.
                addFlow.BeginAction(1003);

                XmlTextReader reader = new XmlTextReader(stream);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                addFlow.ReadXml(reader, true, false, false);
                reader.Close();

                // We move a little each pasted node and link so that they do not recover
                // the original items.
                float dx = addFlow.Grid.Size.Width;
                float dy = addFlow.Grid.Size.Height;
                foreach (Item item in addFlow.SelectedItems)
                {
                    if (item is Node)
                    {
                        Node node = (Node)item;
                        node.Location = new PointF(node.Location.X + dx, node.Location.Y + dy);
                    }
                    else
                    {
                        PointF pt;
                        Link link = (Link)item;

                        if (link.AdjustOrg)
                        {
                            pt = link.Points[0];
                            link.Points[0] = new PointF(pt.X + dx, pt.Y + dy);
                        }
                        for (int k = 1; k < link.Points.Count - 1; k++)
                        {
                            pt = link.Points[k];
                            link.Points[k] = new PointF(pt.X + dx, pt.Y + dy);
                        }
                        if (link.AdjustDst)
                        {
                            pt = link.Points[link.Points.Count - 1];
                            link.Points[link.Points.Count - 1] = new PointF(pt.X + dx, pt.Y + dy);
                        }
                    }
                }

                addFlow.EndAction();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.SendSelectionChangeEvent = false;
            addFlow.DeleteSel();
            addFlow.SendSelectionChangeEvent = true;
            SelectionChangeHandle();
        }

        private void selectModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addFlow.MouseAction == MouseAction.Selection)
                addFlow.MouseAction = MouseAction.None;
            else
                addFlow.MouseAction = MouseAction.Selection;
            selectModeToolStripMenuItem.Checked = (addFlow.MouseAction == MouseAction.Selection);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.SendSelectionChangeEvent = false;
            foreach (Item item in addFlow.Items)
                item.Selected = true;
            addFlow.SendSelectionChangeEvent = true;
            SelectionChangeHandle();
        }

        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float leftMin = 0;
            float topMin = 0;
            float rightMax = 0;
            float bottomMax = 0;
            bool first = false;

            first = true;
            foreach (Item item in addFlow.SelectedItems)
            {
                if (item is Node)
                {
                    Node node = (Node)item;
                    RectangleF rc = node.Rect;
                    if (first)
                    {
                        first = false;
                        leftMin = rc.Left;
                        topMin = rc.Top;
                        rightMax = rc.Left + rc.Width;
                        bottomMax = rc.Top + rc.Height;
                    }
                    else
                    {
                        if (rc.Left < leftMin)
                            leftMin = rc.Left;
                        if (rc.Top < topMin)
                            topMin = rc.Top;
                        if (rc.Right > rightMax)
                            rightMax = rc.Right;
                        if (rc.Bottom > bottomMax)
                            bottomMax = rc.Bottom;
                    }
                }
            }

            // We group the nodes only if there are several nodes.
            if (!first)
            {
                // Create the "owner" node. Make it not visible, not resizeable
                Node owner = new Node(leftMin, topMin, rightMax - leftMin, bottomMax - topMin);
                owner.Hidden = true;
                owner.XSizeable = false;
                owner.YSizeable = false;
                owner.RemoveChildren = false;
                owner.Shape.Style = ShapeStyle.Rectangle;
                addFlow.Nodes.Add(owner);

                // Create a Rigid, Hidden and unselectable link between the owner node and each 
                // selected node.
                foreach (Item item in addFlow.SelectedItems)
                {
                    if (item is Node)
                    {
                        Node node = (Node)item;
                        node.Parent = owner;
                    }
                }

                // Select the owner node
                addFlow.SelectedItem = owner;
            }
        }

        private void ungroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addFlow.SelectedItem is Node)
                addFlow.Nodes.Remove((Node)addFlow.SelectedItem);
        }

        private void zOrderBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addFlow.SelectedItem != null)
                addFlow.SelectedItem.ZOrder = 0;
        }

        private void zOrderFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addFlow.SelectedItem != null)
                addFlow.SelectedItem.ZOrder = addFlow.Items.Count - 1;
        }
        #endregion

        #region View Menu
        private void zoom75ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.Zoom = new Zoom(0.75f, 0.75f);
        }

        private void zoom100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.Zoom = new Zoom(1, 1);
        }

        private void zoom150ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlow.Zoom = new Zoom(1.5f, 1.5f);
        }

        private void zoomRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addFlow.MouseAction == MouseAction.ZoomIsotropic)
                addFlow.MouseAction = MouseAction.None;
            else
                addFlow.MouseAction = MouseAction.ZoomIsotropic;
            zoomRectangleToolStripMenuItem.Checked = (addFlow.MouseAction == MouseAction.ZoomIsotropic);
        }

        private void workflowStepWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
            workflowStepWindowToolStripMenuItem.Checked = !splitContainer.Panel2Collapsed;
            workflowStepWindowShowMenuItem.Checked = !workflowStepWindowShowMenuItem.Checked;
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusBar.Visible = !statusBar.Visible;
            statusBarToolStripMenuItem.Checked = !statusBarToolStripMenuItem.Checked;
            statusBarShowMenuItem.Checked = !statusBarShowMenuItem.Checked;
        }
        #endregion

        #region Help Menu
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ResX.Menu_About);
        }
        #endregion

        #region mapping Menus to Toolbars
        private void standardToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Tag.ToString())
            {
                case "New":
                    newToolStripMenuItem_Click(sender, e);
                    break;
                case "Open":
                    openToolStripMenuItem_Click(sender, e);
                    break;
                case "Save":
                    saveToolStripMenuItem_Click(sender, e);
                    break;
                case "PrintPreview":
                    printPreviewToolStripMenuItem_Click(sender, e);
                    break;
                case "Print":
                    printToolStripMenuItem_Click(sender, e);
                    break;
                case "Cut":
                    cutToolStripMenuItem_Click(sender, e);
                    break;
                case "Copy":
                    copyToolStripMenuItem_Click(sender, e);
                    break;
                case "Paste":
                    pasteToolStripMenuItem_Click(sender, e);
                    break;
                case "Help":
                    aboutToolStripMenuItem_Click(sender, e);
                    break;
                case "Undo":
                    undoToolStripMenuItem_Click(sender, e);
                    break;
                case "Redo":
                    redoToolStripMenuItem_Click(sender, e);
                    break;
            }
        }
        #endregion

        #region UI show context menu
        private void standardToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            standardToolbar.Visible = !standardToolbar.Visible;
            standardToolbarShowMenuItem.Checked = !standardToolbarShowMenuItem.Checked;
        }

        private void workflowStepWindowShowMenuItem_Click(object sender, EventArgs e)
        {
            workflowStepWindowToolStripMenuItem_Click(sender, e);
        }

        private void statusBarShowMenuItem_Click(object sender, EventArgs e)
        {
            statusBarToolStripMenuItem_Click(sender, e);
        }

        private void showPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Workfow step drag and drop support
        private void cmbSelectNodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView.Items.Clear();
            switch (cmbSelectNodeType.SelectedItem.ToString())
            {
                case "SpecStep":
                case "工艺步骤":
                    QuerySpec qs = new QuerySpec();
                    foreach (string spec in qs.GetAllSpecRevisions(""))
                    {
                        listView.Items.Add(new ListViewItem(spec, "SpecStep"));
                    }
                    break;
                case "SubWorkflowStep":
                case "子流程步骤":
                    break;
            }
        }

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            addFlow.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void addFlow_DragEnter(object sender, DragEventArgs e)
        {
            ListViewItem listViewItem = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem", true);
            if (listViewItem != null)
                e.Effect = DragDropEffects.Move;
        }

        private void addFlow_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem listViewItem = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem", true);
            if (listViewItem != null)
            {
                Point ptClient = addFlow.PointToClient(new Point(e.X, e.Y));
                Point ptAddFlow = addFlow.PointToAddFlow(ptClient);
                Node node = new Node(ptAddFlow.X, ptAddFlow.Y, 64, 64, addFlow.DefNodeProp);
                UpdateNodeImage(node);
                node.Text = listViewItem.Text;
                addFlow.Nodes.Add(node);
            }
        }

        private void addFlow_AfterAddLink(object sender, AfterAddLinkEventArgs e)
        {
            UpdateNodeImage(e.Link.Org);
            UpdateNodeImage(e.Link.Dst);
        }

        private void addFlow_AfterRemoveLink(object sender, AfterRemoveLinkEventArgs e)
        {
            UpdateNodeImage(e.Link.Org);
            UpdateNodeImage(e.Link.Dst);
        }
        #endregion

        #region helper functions
        bool SaveFileDlg()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (strFileName.Length > 1)
                sfd.FileName = strFileName;
            else
                sfd.FileName = "*.xml";
            sfd.Filter = "AddFlow Diagrams(*.xml)|*.xml|All Files(*.*)|*.*";
            sfd.InitialDirectory = Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                strFileName = sfd.FileName;
                SaveFile();
                MakeCaption();
                return true;
            }
            else
                return false;
        }

        void LoadFile(string strFileName)
        {
            addFlow.ResetUndoRedo();
            addFlow.Nodes.Clear();
            XmlTextReader reader = new XmlTextReader(strFileName);
            reader.WhitespaceHandling = System.Xml.WhitespaceHandling.None;
            addFlow.ReadXml(reader);
            reader.Close();
            addFlow.SetChangedFlag(false);
            this.strFileName = strFileName;
            MakeCaption();
        }

        void SaveFile()
        {
            XmlTextWriter writer = new XmlTextWriter(strFileName, null);
            writer.Formatting = Formatting.Indented;
            addFlow.WriteXml(writer);
            writer.Close();
            addFlow.SetChangedFlag(false);
        }

        void MakeCaption()
        {
            this.Text = strProgName + " - " + FileTitle();
        }

        string FileTitle()
        {
            if (strFileName.Length > 1)
                return Path.GetFileName(strFileName);
            else
                return ResX.Untitled;
        }

        bool OkToTrash()
        {
            if (!addFlow.IsChanged)
                return true;
            DialogResult dr = MessageBox.Show(string.Format(ResX.OkToTrash_Prompt, FileTitle()), strProgName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            switch (dr)
            {
                case DialogResult.Yes:
                    return SaveFileDlg();
                case DialogResult.No:
                    return true;
                case DialogResult.Cancel:
                default:
                    return false;
            }
        }

        string ActionName(Action code)
        {
            switch (code)
            {
                case Action.NodeAdd:
                    return ResX.Action_NodeAdd;
                case Action.NodeResize:
                    return ResX.Action_NodeResize;
                case Action.DeleteSel:
                    return ResX.Action_DeleteSel;
                case Action.MoveItems:
                    return ResX.Action_MoveItems;
                case Action.LinkAdd:
                    return ResX.Action_LinkAdd;
                case Action.LinkStretch:
                    return ResX.Action_LinkStretch;
                case Action.ZOrder:
                    return ResX.Action_ZOrder;
                case Action.Org:
                    return ResX.Action_Org;
                case Action.Dst:
                    return ResX.Action_Dst;
                case Action.Text:
                    return ResX.Action_Text;
                case (Action)1001:
                    return "node properties change";
                case (Action)1002:
                    return "link properties change";
                case (Action)1003:
                    return "paste";
                default:
                    // here you can add your own action code
                    // (see the AddFlow method: BeginAction)
                    return "";
            }
        }

        private void SelectionChangeHandle()
        {
            Item item = addFlow.SelectedItem;
            if (item != null)
            {
                if (item is Node)
                {
                    Node node = (Node)item;
                }
                else if (item is Link)
                {
                    Link link = (Link)item;
                }
            }
        }

        private void UpdateNodeImage(Node node)
        {
            addFlow.SkipUndo = true;
            if (node.InLinks != null)
            {
                if (node.InLinks.Count == 0)
                    node.ImageIndex = 1;
                else
                    node.ImageIndex = 0;
            }
            else
                node.ImageIndex = 1;
            addFlow.SkipUndo = false;
        }
        #endregion

    }

    #region ClipboardMetafileHelper
    public class ClipboardMetafileHelper
    {
        [DllImport("user32.dll", EntryPoint = "OpenClipboard",
        SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public extern static bool OpenClipboard(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "EmptyClipboard",
        SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public extern static bool EmptyClipboard();

        [DllImport("user32.dll", EntryPoint = "SetClipboardData",
        SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr SetClipboardData(int uFormat, IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "CloseClipboard",
        SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public extern static bool CloseClipboard();

        [DllImport("gdi32.dll", EntryPoint = "CopyEnhMetaFileA",
        SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr CopyEnhMetaFile(IntPtr hemfSrc, IntPtr hNULL);

        [DllImport("gdi32.dll", EntryPoint = "DeleteEnhMetaFile",
        SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public extern static bool DeleteEnhMetaFile(IntPtr hemfSrc);

        // Metafile mf is set to an invalid state inside this function
        public static bool PutEnhMetafileOnClipboard(IntPtr hWnd, Metafile mf)
        {
            bool bResult = false;
            IntPtr hEMF = mf.GetHenhmetafile(); // invalidates mf
            if (!hEMF.Equals(new IntPtr(0)))
            {
                IntPtr hEMF2 = CopyEnhMetaFile(hEMF, new IntPtr(0));
                if (!hEMF2.Equals(new IntPtr(0)))
                {
                    if (OpenClipboard(hWnd))
                    {
                        if (EmptyClipboard())
                        {
                            IntPtr hRes = SetClipboardData(14, hEMF2); // 14 == CF_ENHMETAFILE()
                            bResult = hRes.Equals(hEMF2);
                            CloseClipboard();
                        }
                    }
                }
                DeleteEnhMetaFile(hEMF);
            }
            return bResult;
        }
    }
    #endregion
}