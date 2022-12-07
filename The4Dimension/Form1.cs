using ModelViewer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Windows.Input;
using System.IO;
using LibEveryFileExplorer.Files.SimpleFileSystem;
using ExtensionMethods;
using System.Net;
using The4Dimension.ObjectDB;
using The4Dimension.FormEditors;
using System.Globalization;
using System.Threading;

namespace The4Dimension
{
    public partial class Form1 : Form
    {
        public static string ObjectDbLink = "https://raw.githubusercontent.com/KirbysDarkNebula/T4D/master/newobjdb.xml";
        public UserControl1 render = new UserControl1();
        public Dictionary<string, string> LevelNameNum = new Dictionary<string, string>(); //WX-X, stageName
        int APP_VER = Int32.Parse(Application.ProductVersion.Replace(".", ""));
        string LoadedFile = "";
        bool AutoMoveCam = true;
        bool AddObjectOrigin = false;
        public static int ReleaseId = 11;
        private Dictionary<string, string> strings;
        //public PropertyPanel Panell1 = new PropertyPanel();
        public bool fullrefresh = true;
        public Form1(string FileLoad = "")
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            try
            {
                strings = new Dictionary<string, string>();
                InitializeComponent();
                #region StageList 
                string[] lines = Properties.Resources.AllStageList.Split(Environment.NewLine[0]);
                int nextIndex = -1;
                for (int i = 1; i < 3; i++)
                {
                    for (int y = 1; y < 6; y++)
                    {
                        LevelNameNum.Add("W" + i.ToString() + "-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                    }
                }
                for (int i = 3; i < 8; i++)
                {
                    for (int y = 1; y < 7; y++)
                    {
                        LevelNameNum.Add("W" + i.ToString() + "-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                    }
                }
                for (int y = 1; y < 10; y++)
                {
                    LevelNameNum.Add("W8-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                }
                for (int y = 1; y < 6; y++)
                {
                    LevelNameNum.Add("S1-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                }
                for (int i = 10; i < 17; i++)
                {
                    for (int y = 1; y < 7; y++)
                    {
                        LevelNameNum.Add("S" + (i - 8).ToString() + "-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                    }
                }
                LevelNameNum.Add("S8-Championship", lines[++nextIndex].Trim());
                string[] spstg = Properties.Resources.SpecialStageList.Split(Environment.NewLine[0]);
                nextIndex = 0;
                int spstgindex = 0;
                for (int i = 0; i < spstg.Length; i++)
                {
                    if (spstg[i].Trim().StartsWith("Demo"))
                    {
                        LevelNameNum.Add("Cutscene " +  ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")" , spstg[spstgindex].Trim());
                        if(!spstg[i+1].Trim().StartsWith("Demo")){ nextIndex = 0; }
                    }
                    else if (spstg[i].Trim().StartsWith("KinopioHouse"))
                    {
                        LevelNameNum.Add("Toad house " + ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")", spstg[spstgindex].Trim());
                        if (!spstg[i + 1].Trim().StartsWith("KinopioHouse")){ nextIndex = 0; }
                    }
                    else if (spstg[i].Trim().StartsWith("KinopioGame"))
                    {
                        LevelNameNum.Add("Gameover house " + ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")", spstg[spstgindex].Trim());
                        if (!spstg[i + 1].Trim().StartsWith("KinopioGame")){ nextIndex = 0; }
                    }                    
                    else if (spstg[i].Trim().StartsWith("Mystery"))
                    {
                        LevelNameNum.Add("Mystery box " + ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")", spstg[spstgindex].Trim());
                        if (i != spstg.Length - 1) 
                        { if (!spstg[i + 1].Trim().StartsWith("Mystery")) { nextIndex = 0; } }
                        
                    }
                    ++spstgindex;
                }
                #endregion

                 
                while (Properties.Settings.Default.OpenRecent.Count > 9)
                {
                    Properties.Settings.Default.OpenRecent.RemoveAt(0);
                }
                int d = Properties.Settings.Default.OpenRecent.Count;

                for (int i = Properties.Settings.Default.OpenRecent.Count; i > 0; i--)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = "OpnRec" + (Properties.Settings.Default.OpenRecent.Count-i+1);
                    btn.Text = (Properties.Settings.Default.OpenRecent.Count-i+1).ToString() + ". " + Properties.Settings.Default.OpenRecent[i-1];
                    btn.Click += OpenRecent_click;
                    openRecentToolStripMenuItem.DropDownItems.Add(btn);
                }
                ToolStripMenuItem clr = new ToolStripMenuItem();
                clr.Name = "OpnRecClr";
                clr.Text = "Clear";
                clr.Click += OpenRecentClear;
                openRecentToolStripMenuItem.DropDownItems.Add(clr);


                render.CamDistance =Properties.Settings.Default.CamDistance;
                render.Camspeed =Properties.Settings.Default.CamSpeed;
                KeyPreview = true;
                elementHost1.Child = render;
                /*  */
                    render.MouseLeftButtonDown += render_LeftClick;
                render.MouseMove += render_MouseMove;
                render.MouseLeftButtonDown += render_MouseLeftButtonDown;
                render.MouseLeftButtonUp += render_MouseLeftButtonUp;
                render.KeyDown += render_KeyDown;
                render.KeyUp += render_KeyUP;
                render.CameraInertiaFactor = Properties.Settings.Default.CameraInertia;
                render.ShowFps = Properties.Settings.Default.ShowFps;
                render.ShowTriangleCount = Properties.Settings.Default.ShowTriCount;
                render.ShowDebugInfo = Properties.Settings.Default.ShowDbgInfo;
                render.CamMode = Properties.Settings.Default.CameraMode == 0 ? HelixToolkit.Wpf.CameraMode.Inspect : HelixToolkit.Wpf.CameraMode.WalkAround;
                render.ZoomSensitivity = Properties.Settings.Default.ZoomSen;
                render.RotationSensitivity = Properties.Settings.Default.RotSen;
                render.HasAA = Properties.Settings.Default.HasAA;
                render.TextureFilter = Properties.Settings.Default.TextFilter;
                render.BackfaceCull = Properties.Settings.Default.BackfaceCull;
                AutoMoveCam = Properties.Settings.Default.AutoMoveCam;
                AddObjectOrigin = Properties.Settings.Default.AddObjectOrigin;
                
                Focus();
                if (FileLoad != "")
                {
                    LoadedFile = FileLoad;
                }
            }
            catch (Exception ex)
            {
                string err = "There was an error in the application.\r\n" +
                    "________________________________________\r\n" +
                    ex.Message + "\r\n\r\n" + ex.StackTrace + "\r\n";
                File.WriteAllText("Error.log", err);
                MessageBox.Show("There was an error in the application:\r\n" + ex.Message);
                MessageBox.Show("A log of the error was saved in the same folder of this application.");
            }
        }
        public int Scenario = 1;
        public bool UseDesignSound = Properties.Settings.Default.UseDesignSound;
        public Dictionary<string, byte[]> SzsFiles = null;
        public Dictionary<string, AllInfoSection> AllInfos = new Dictionary<string, AllInfoSection>();
        public AllRailInfoSection AllRailInfos = new AllRailInfoSection();
        public Dictionary<string, int> highestID = new Dictionary<string, int>();
        public Dictionary<string, string> CreatorClassNameTable = new Dictionary<string, string>();
        CameraParams LevelCams = new CameraParams();
        public CustomStack<UndoAction> Undo = new CustomStack<UndoAction>();
        public static List<ClipBoardItem> clipboard = new List<ClipBoardItem>();
        public static Encoding DefEnc = Encoding.GetEncoding("Shift-JIS");
        public ObjectDb ObjectDatabase = null;
        public NewDb NewObjectDatabase = null;
        List<String> CustomModels = new List<string>();
        public Dictionary<string, TabPage> PropertyTabs = new Dictionary<string, TabPage>();
        List<bool> ShowAreas = new List<bool>() { false, false, true }; // Cameras, Areas, Rails
        private void Form1_Load(object sender, EventArgs e)
        {
            menuStrip2.Items.Insert(1, new ToolStripSeparator());
            generate2DSectionToolStripMenuItem.Enabled = Properties.Settings.Default.ExperimentalFeatures;
            generate2DSectionToolStripMenuItem.Visible = Properties.Settings.Default.ExperimentalFeatures;
            generatePreloadFileListToolStripMenuItem.Enabled = Properties.Settings.Default.ExperimentalFeatures;
            generatePreloadFileListToolStripMenuItem.Visible = Properties.Settings.Default.ExperimentalFeatures;
            WorldMapEd.Enabled = Properties.Settings.Default.ExperimentalFeatures;
            WorldMapEd.Visible = Properties.Settings.Default.ExperimentalFeatures;
            toolStripSeparator5.Enabled = Properties.Settings.Default.ExperimentalFeatures;
            toolStripSeparator5.Visible = Properties.Settings.Default.ExperimentalFeatures;
            //Panell1.panel = panel2;
            //Panell1.owner = this;
            foreach (TabPage tab in SelectedProperties.TabPages)
            {
                PropertyTabs.Add(tab.Name, tab);
            }
            if (!Directory.Exists("models"))
            {
                if (MessageBox.Show("You must convert every model from the game before you can use the editor, convert now ? (you need to have the extracted ROMFS of the game on your pc)", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show("Select the ROMFS folder (This folder should contain ObjectData, SystemData, StageData, etc...)\r\nKeep this folder in the same path, from there will be loaded the levels, some files needed for editing the levels and the bgm.\r\nThe content of the folder won't be modified unless you save edited levels there");
                    ModelDumper dlg = new ModelDumper();
                    dlg.ShowDialog();
                    if (!Directory.Exists("models")) Application.Exit();
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Directory.CreateDirectory("models");
                    File.WriteAllBytes(@"models\baseModels.zip", Properties.Resources.BaseModels);
                    System.IO.Compression.ZipFile.ExtractToDirectory(@"models\baseModels.zip", @"models");
                    File.Delete(@"models\baseModels.zip");
                    Directory.CreateDirectory(@"models\Tex");
                    MessageBox.Show("You won't be able to see any model, to do the procedure delete the models folder");
                }
            }
            if (!Properties.Settings.Default.DownloadDb)/* LoadObjectDatabase();*/ LoadNewDatabase();
            List<string> remove = new List<string>(PropertyTabs.Keys);
            RefreshTabs(remove, null);
            ObjectName.Text = "No object selected";
            /*if (Panell1.known == null)
            {
                Panell1.known = new Dictionary<string, NewDb.EntryProperty>();
                if (NewObjectDatabase != null)
                {
                    foreach (NewDb.EntryProperty property in NewObjectDatabase.KnownProperties)
                    {
                        Panell1.known.Add(property.name, property);
                    }
                }
            }*/
            LoadCreatorClassNameTable();
            if (LoadedFile != "") LoadFile(LoadedFile);
            else SetUiLock(false);
            if (Properties.Settings.Default.CheckUpdates || Properties.Settings.Default.DownloadDb)
            {
                StatusLbl.Visible = true;
                downloadLatestObjectDatabaseToolStripMenuItem.Enabled = true;
                StartupChecks.RunWorkerAsync();
            }
            if (Properties.Settings.Default.CheckUpdates)
            {
                StatusLbl.Text = "Checking updates";
                if (Properties.Settings.Default.DownloadDb) StatusLbl.Text += " and downloading database..."; else StatusLbl.Text += "...";
            }
            else if (Properties.Settings.Default.DownloadDb) StatusLbl.Text = "Downloading database...";
            if (Directory.Exists("CustomModels"))
            {
                foreach (string s in Directory.EnumerateFiles("CustomModels"))
                    if (s.EndsWith(".obj")) CustomModels.Add(Path.GetFileNameWithoutExtension(s));
            }
            SetIncrement(10);
        }

        void UnloadLevel()
        {
            render.UnloadLevel();
            SzsFiles = null;
            SaveChangeLabel();
            C0ListEditingStack = new Stack<List<LevelObj>>();
            IsEditingC0List = false;
            SelectionIndex = new Stack<int>();
            InitialAllInfosSection = -1;
            C0EditingPanel.Visible = false;
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("AllRailInfos").Name = "AllRailInfos";
            treeView1.Nodes.Add("AreaObjInfo").Name = "AreaObjInfo";
            treeView1.Nodes.Add("CameraAreaInfo").Name = "CameraAreaInfo";
            treeView1.Nodes.Add("DemoSceneObjInfo").Name = "DemoSceneObjInfo";
            treeView1.Nodes.Add("GoalObjInfo").Name = "GoalObjInfo";
            treeView1.Nodes.Add("ObjInfo").Name = "ObjInfo";
            treeView1.Nodes.Add("StartEventObjInfo").Name = "StartEventObjInfo";
            treeView1.Nodes.Add("StartInfo").Name = "StartInfo";

            AllInfos = new Dictionary<string, AllInfoSection>();
            AllRailInfos = new AllRailInfoSection();
            highestID = new Dictionary<string, int>();
            Undo = new CustomStack<UndoAction>();
            comboBox1.Items.Clear();
            //ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;
            RefreshTabs(PropertyTabs.Keys.ToList(), null);
            SelectedProperties.Enabled = false;
            ShowAreas[0] = false;
            ShowAreas[1] = false;
            ShowAreas[2] = false;
            showAreasToolStripMenuItem.Text = "Show Areas";
            CommonScenario.Checked = true;
            Scenario1.Checked = true;
            Scenario2.Checked = true;
            Scenario3.Checked = true;
            ShownScenarios[0] = true; ShownScenarios[1] = true; ShownScenarios[2] = true; ShownScenarios[3] = true; ShownScenarios[4] = true;
            toolStripMenuItem1.Text = "Show Cameras";
            //if (MessageBox.Show("Keep clipboard ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) clipboard = new List<ClipBoardItem>();
            LoadedFile = "";
            this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
            OtherLevelDataMenu.DropDownItems.Clear();
            SetUiLock(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Title = "Open a level file";
            opn.Filter = "Supported formats (.szs, .byml, .byaml, .xml)|*.szs; *.byml; *.byaml; *.xml|Every file|*.*";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                LoadFile(opn.FileName);
            }
        }

        private void openFromLevelNameWXXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.GamePath.Trim() == "")
            {
                MessageBox.Show("To use this function you must set the game ROMFS path");
                return;
            }
            else if (!Directory.Exists(Properties.Settings.Default.GamePath + "\\StageData"))
            {
                MessageBox.Show("Folder " + Properties.Settings.Default.GamePath + "\\StageData Not found!\r\nProbably your Romfs dump is incomplete or was modified");
                return;
            }
            FrmLevNameOpen f = new FrmLevNameOpen(LevelNameNum);
            f.ShowDialog();
            if (f.res == null) return;
            else
            {
                if (File.Exists(Properties.Settings.Default.GamePath + "\\StageData\\" + LevelNameNum[f.res])) LoadFile(Properties.Settings.Default.GamePath + "\\StageData\\" + LevelNameNum[f.res]);
                else MessageBox.Show(Properties.Settings.Default.GamePath + "\\StageData\\" + LevelNameNum[f.res] + " Not found!\r\nProbably your Romfs dump is incomplete or was modified");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnloadLevel();
            menuStrip2.Enabled = true;
            elementHost1.Show();
            comboBox1.Items.Add("AllRailInfos");
            highestID.Add("AllRailInfos", 0);
            render.AddKey("AllRailInfos");
            comboBox1.SelectedIndex = 0;
            SetupSZS();
            SetUiLock(true);
            saveToolStripMenuItem.Enabled = false;
            Scenario = 1; 
            List<string> remove = new List<string>();
            foreach (string tab in PropertyTabs.Keys.Reverse())
            {
                remove.Add(tab);
            }
            RefreshTabs(remove, null);
            SelectedProperties.Enabled = false;
            SelectedProperties.Visible = false;
            ObjectName.Text = "No object selected";
        }

        void SetUiLock(bool Lock)
        {
            GenIncrement.Enabled = Lock;
            menuStrip2.Enabled = Lock;
            splitContainer1.Enabled = Lock;
            saveAsXmlToolStripMenuItem.Enabled = Lock;
            saveAsBymlToolStripMenuItem1.Enabled = Lock;
            saveToolStripMenuItem.Enabled = Lock;
            generate2DSectionToolStripMenuItem.Enabled = Lock;
            UndoMenu.Enabled = Lock;
            findToolStripMenuItem.Enabled = Lock;
            label3.Text = "";
            if (Lock) ZoomCheckWarning.Start(); else ZoomCheckWarning.Stop();
            OtherLevelDataMenu.Enabled = Lock;
            saveAsSZSToolStripMenuItem.Enabled = Lock;
            saveAsToolStripMenuItem.Enabled = Lock;
            generatePreloadFileListToolStripMenuItem.Enabled = Lock;
        }

        #region FileLoading
        public List<string> opnfiles = new List<string>();
        public void LoadFile(string FilePath) //Checks the file type and then loads the file
        {

            if (Path.GetExtension(FilePath).ToLower() == ".xml")//xml and byml originally tried to load levels, now only xml does this as it makes more sense for byml to be readable, like when dropping a byml file to t4d.exe
            {/*
                LoadedFile = FilePath;
                SetUiLock(true);
                OpenFile(File.ReadAllText(FilePath, DefEnc));
                SetupSZS();*/
            }
            else if (Path.GetExtension(FilePath).ToLower() == ".byml" || Path.GetExtension(FilePath).ToLower() == ".byaml")
            {
                LoadedFile = FilePath;
                FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(LoadedFile), LoadedFile, false);
                frm.ShowDialog();
                if (frm.XmlRes != null) { File.WriteAllBytes(FilePath, BymlConverter.GetByml(frm.XmlRes)); }
                /* Original function was less useful
                LoadedFile = FilePath;
                SetUiLock(true);
                OpenFile(BymlConverter.GetXml(FilePath));
                SetupSZS();*/
            }
            else if (Path.GetExtension(FilePath).ToLower() == ".szs")//load the main level and then try to load the design and sound
            {
                UnloadLevel();
                elementHost1.Show();
                LoadedFile = FilePath;
                Scenario = 1;
                if (Int32.TryParse(FilePath.Substring(FilePath.Count() - 5, 1), out _))
                {
                    Scenario = Int32.Parse(FilePath.Substring(FilePath.Count() - 5, 1));
                }
                string DesignFile = FilePath.Substring(0, FilePath.Count()-8)+"Design"+ Scenario.ToString()+".szs";
                string SoundFile = FilePath.Substring(0, FilePath.Count() - 8) + "Sound" + Scenario.ToString() + ".szs";
                OtherLevelDataMenu.DropDownItems.Clear();
                SzsFiles = new Dictionary<string, byte[]>();
                LevelCams = new CameraParams();
                CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
                NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
                SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(FilePath)));
                int index = 0;
                List<ToolStripMenuItem> OtherFiles = new List<ToolStripMenuItem>();
                byte[] StageData = null;
                opnfiles.Add(FilePath);
                UseDesignSound = Properties.Settings.Default.UseDesignSound;
                foreach (SFSFile f in SzsArch.ToFileSystem().Files)
                {
                    if (f.FileName.ToLower() == "stagedata.byml") StageData = f.Data;
                    else
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = "LoadFile" + index.ToString();
                        btn.Text = f.FileName;
                        btn.Click += LoadFileList_click;
                        OtherFiles.Add(btn);
                        SzsFiles.Add(f.FileName, f.Data);
                    }
                    index++;
                }
                OtherLevelDataMenu.DropDownItems.AddRange(OtherFiles.ToArray());
                if (StageData != null)
                {
                    Debug.Print("Size : " + (StageData.Length / 1024).ToString());

                    OpenFile(BymlConverter.GetXml(StageData),"Map", ((!File.Exists(DesignFile)&&!File.Exists(SoundFile))||!Properties.Settings.Default.UseDesignSound));
                }
                else
                {
                    MessageBox.Show("StageData.byml not found in the file !");
                    SzsFiles = null;
                    SetUiLock(false);
                }
                if ((File.Exists(DesignFile) || File.Exists(SoundFile))&& UseDesignSound == true)
                {
                    if (File.Exists(DesignFile))
                    {
                        //SzsFiles = new Dictionary<string, byte[]>();
                        y = new CommonCompressors.YAZ0();
                        SzsArch = new NDS.NitroSystem.FND.NARC();
                        SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(DesignFile)));
                        OtherFiles = new List<ToolStripMenuItem>();
                        StageData = null;
                        opnfiles.Add(DesignFile);
                        foreach (SFSFile f in SzsArch.ToFileSystem().Files)
                        {
                            if (f.FileName.ToLower() == "stagedata.byml") StageData = f.Data;
                            else
                            {
                                ToolStripMenuItem btn = new ToolStripMenuItem();
                                btn.Name = "LoadFile" + index.ToString();
                                btn.Text = f.FileName;
                                btn.Click += LoadFileList_click;
                                OtherFiles.Add(btn);
                                SzsFiles.Add(f.FileName, f.Data);
                            }
                            index++;
                        }
                        OtherLevelDataMenu.DropDownItems.AddRange(OtherFiles.ToArray());
                        if (StageData != null)
                        {
                            Debug.Print("Size : " + (StageData.Length / 1024).ToString());
                            OpenFile(BymlConverter.GetXml(StageData),"Design",!File.Exists(SoundFile));
                        }
                        else
                        {
                            MessageBox.Show("StageData.byml not found in the file !");
                            SzsFiles = null;
                            SetUiLock(false);
                        }
                    }
                    if (File.Exists(SoundFile))
                    {
                        //SzsFiles = new Dictionary<string, byte[]>();
                        y = new CommonCompressors.YAZ0();
                        SzsArch = new NDS.NitroSystem.FND.NARC();
                        SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(SoundFile)));
                        OtherFiles = new List<ToolStripMenuItem>();
                        StageData = null;
                        opnfiles.Add(SoundFile);
                        foreach (SFSFile f in SzsArch.ToFileSystem().Files)
                        {
                            if (f.FileName.ToLower() == "stagedata.byml") StageData = f.Data;
                            else
                            {
                                ToolStripMenuItem btn = new ToolStripMenuItem();
                                btn.Name = "LoadFile" + index.ToString();
                                btn.Text = f.FileName;
                                btn.Click += LoadFileList_click;
                                OtherFiles.Add(btn);
                                SzsFiles.Add(f.FileName, f.Data);
                            }
                            index++;
                        }
                        OtherLevelDataMenu.DropDownItems.AddRange(OtherFiles.ToArray());
                        if (StageData != null)
                        {
                            Debug.Print("Size : " + (StageData.Length / 1024).ToString());
                            OpenFile(BymlConverter.GetXml(StageData),"Sound",true);
                        }
                        else
                        {
                            MessageBox.Show("StageData.byml not found in the file !");
                            SzsFiles = null;
                            SetUiLock(false);
                        }
                    }
                    this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + FilePath.Substring(0, FilePath.Count() - 8) + " - Scenario " + Scenario.ToString();
                }
                else { this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + FilePath; }

                if (SzsFiles.ContainsKey("CameraParam.byml"))
                {
                    LoadCameras(BymlConverter.GetXml(SzsFiles["CameraParam.byml"]));
                }

                SetUiLock(true);

                AddRecentOpen(LoadedFile);
            }
            else if (Path.GetExtension(FilePath).ToLower() == ".bcmdl"|| Path.GetExtension(FilePath).ToLower() == ".obj")
            {
                new FormEditors.FrmObjImport(FilePath).ShowDialog();
                if (this.Text != "The 4th Dimension") { elementHost1.Show(); }
            }
            else
            {
                LoadedFile = "";
                MessageBox.Show("File type not supported!");
                if (this.Text != "The 4th Dimension") { elementHost1.Show(); }
                //SetUiLock(false);
            }
            
        }

        private void LoadCameras(string XmlText)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XmlText);
            XmlNode n = xml.SelectSingleNode("/Root/C1/C0");//CameraParam

            for (int i = 0; i < n.ChildNodes.Count; i++)
            {
                LevelCams.Add(LoadCAMERA(n.ChildNodes[i].ChildNodes));
            }

            n = xml.SelectSingleNode("/Root/C1/C1");//VisionParam


            return;
        }

        private void OpenRecent_click(object sender, EventArgs e)
        {
            string filepath = (((ToolStripMenuItem)sender).Text);
            LoadFile(filepath.Substring(3));
        }
        private void OpenRecentClear(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenRecent.Clear();
            openRecentToolStripMenuItem.DropDownItems.Clear();
            ToolStripMenuItem clr = new ToolStripMenuItem();
            clr.Name = "OpnRecClr";
            clr.Text = "Clear";
            clr.Click += OpenRecentClear;
            openRecentToolStripMenuItem.DropDownItems.Add(clr);
        }
        private void LoadFileList_click(object sender, EventArgs e)
        {
            string name = ((ToolStripMenuItem)sender).Text;
            FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(SzsFiles[name]), name, false);
            frm.ShowDialog();
            if (frm.XmlRes != null) SzsFiles[name] = BymlConverter.GetByml(frm.XmlRes);
        }
        
        public void LoadObjectDatabase()
        {
            ObjectDatabase = null;
            if (!File.Exists(@"objectdb.xml"))
            {
                MessageBox.Show("The object database wasn't found, some objects may not appear, and you won't be able to get informations about how to use objects, you can download the database from Help -> Download latest object database");
                return;
            } else
            {
                try { ObjectDatabase = ObjectDb.FromXml(File.ReadAllText(@"objectdb.xml")); }
                catch (Exception ex) { MessageBox.Show("Could't load the objects database: \r\n\r\n" + ex.Message); }
            }
        }
        public void LoadNewDatabase()
        {
            NewObjectDatabase = null;
            if (!File.Exists(@"newobjdb.xml"))
            {
                if (MessageBox.Show("The object database wasn't found, do you want to create a generic one?\r\n Some objects may still not appear and you won't be able to get information on how to use objects, so you can try downloading the database from Help -> Download latest object database", "Warning", MessageBoxButtons.YesNo) == DialogResult.No) return;
                try
                {
                    File.WriteAllText(@"newobjdb.xml", Properties.Resources.sampledb);
                    NewObjectDatabase = NewDb.FromXml(File.ReadAllText(@"newobjdb.xml"));
                }
                catch (Exception ex) { MessageBox.Show("Could't load the object database: \r\n\r\n" + ex.Message + ex.StackTrace); }
            }
            else
            {
                try { NewObjectDatabase = NewDb.FromXml(File.ReadAllText(@"newobjdb.xml")); }
                catch (Exception ex) { MessageBox.Show("Could't load the object database: \r\n\r\n" + ex.Message + ex.StackTrace); }
            }
            //Panell1.newDb = NewObjectDatabase;
        }

        public void LoadCreatorClassNameTable()
        {
            CreatorClassNameTable.Clear();
            string a = Properties.Settings.Default.UseLayeredFs ? Properties.Settings.Default.LayeredFSPath + "\\SystemData\\CreatorClassNameTable.szs" : @"CreatorClassNameTable.szs";
            if (!File.Exists(a))
            {
                if (Properties.Settings.Default.UseLayeredFs && !Directory.Exists(Properties.Settings.Default.LayeredFSPath + "\\SystemData\\"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.LayeredFSPath + "\\SystemData\\");
                }
                if (Properties.Settings.Default.GamePath.Trim() != "" && File.Exists(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs"))
                {
                    File.Copy(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs", a);
                }
                else
                {
                    if (Properties.Settings.Default.GamePath.Trim() == "") MessageBox.Show("to add objects to the game, and get list of every objects in the editor you need CreatorClassNameTable.szs in the same folder as this program, this file is placed inside GameRomFS:SystemData\\CreatorClassNameTable.szs");
                    else MessageBox.Show(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs not found.\r\nProbably your Romfs dump is incomplete or was modified.\r\nWithout this file you can only duplicate or delete objects.");
                    creatorClassNameTableEditorToolStripMenuItem.Enabled = false;
                    return;
                }
            }
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(a)));
            string ConvertedCCN = BymlConverter.GetXml(SzsArch.ToFileSystem().Files[0].Data);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(ConvertedCCN);
            XmlNode n = xml.SelectSingleNode("/Root/C0");
            foreach (XmlNode C1Block in n.ChildNodes)
            {
                string ClassName = "";
                string ObjName = "";
                foreach (XmlNode SubNode in C1Block.ChildNodes)
                {
                    if (SubNode.Attributes["Name"].Value == "ClassName")
                        ClassName = SubNode.Attributes["StringValue"].Value;
                    else ObjName = SubNode.Attributes["StringValue"].Value;
                }
                CreatorClassNameTable.Add(ObjName, ClassName);
            }
            comboBox2.Items.Clear();
            foreach(string item in CreatorClassNameTable.Keys.ToArray())
            {
                comboBox2.Items.Add(item);
            }
        }

        void OpenFile(string XmlText, string Type, bool Last)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XmlText);
            XmlNode n = xml.SelectSingleNode("/Root/C1/C1");
            if (n.Attributes["Name"].Value == "AllInfos") ProcessAllInfos(n.ChildNodes); else throw new Exception("Not The AllInfos node !");
            n = xml.SelectNodes("/Root/C1/C1")[1];
            if (n.Attributes["Name"].Value == "AllRailInfos") ProcessRailInfos(n.ChildNodes); else throw new Exception("Not The AllRailInfos node !");
            if (Type == "Map")
            {
                comboBox1.Items.Add("AllRailInfos");
                render.AddKey("AllRailInfos");
                LoadRailsModels(AllRailInfos);
            }

            /*xml.Load(System.IO.Path.GetDirectoryName(file) + "\\PreLoadFileList1.xml");
            n = xml.SelectSingleNode("/Root/C1");
            foreach (XmlNode subnode in n.ChildNodes)
            {
                if (subnode.ChildNodes.Count == 2 && subnode.ChildNodes[1].Attributes["StringValue"].Value == "Archive")
                {
                    string FileName = subnode.ChildNodes[0].Attributes["StringValue"].Value.Split('/')[1];
                    FileName = FileName.Substring(0, FileName.Length - 4);
                    Models.Add(FileName.ToLower(), "models\\" + FileName + ".obj");
                }
            } Reading the file list doesn't seem to be useful for now
            */
            if (Last)
            {
                foreach (string k in AllInfos.Keys.ToArray())
                {
                    render.AddKey(k);
                    if (k == "AreaObjInfo") LoadModels(AllInfos[k], k, "models\\UnkArea.obj");
                    else if (k == "CameraAreaInfo") LoadModels(AllInfos[k], k, "models\\UnkGreen.obj");
                    else LoadModels(AllInfos[k], k);
                }
                if (AllInfos.ContainsKey("AreaObjInfo")) HideLayer("AreaObjInfo");
                if (AllInfos.ContainsKey("CameraAreaInfo")) HideLayer("CameraAreaInfo");
                if (treeView1.Nodes["StartInfo"].Nodes.Count > 0)
                {
                    int mario0 = AllInfos["StartInfo"].GetById(0);
                    SelectInfoIndex("StartInfo", mario0);
                    render.CameraToObj(CurrentAllInfosSectionName, mario0);
                }
            }
        }

        void LoadRailsModels(AllRailInfoSection source)
        {
            foreach (Rail r in source)
            {
                render.addRail(r.GetPointArray(), r.Closed, 5, -1);
            }
        }

        void LoadRailsModels(Rail source, int at = -1)
        {
            List<Point3D> points = new List<Point3D>();
            foreach (Rail.Point p in source.Points) points.Add(new Point3D(p.X, -p.Z, p.Y));
            
            render.addRail(source.GetPointArray(), source.Closed, 5, at);
        }

        void LoadModels(List<LevelObj> Source, string Type, string PlaceHolderMod = "models\\UnkBlue.obj", int at = -1)
        {
            for (int i = 0; i < Source.Count; i++)
            {
                if (!Source[i].Prop.ContainsKey("dbname")) 
                {
                    if (NewObjectDatabase != null && NewObjectDatabase.IdtoDB.ContainsKey(Source[i].ToString()))
                    {
                        Source[i].Prop.Add("dbname", NewObjectDatabase.IdtoDB[Source[i].ToString()]);
                    }

                }
                string Path = GetModelname(Source[i].GetName(true));//db name
                if (((Node)Source[i].Prop["name"]).StringValue == "DemoActor" || ((Node)Source[i].Prop["name"]).StringValue == "DemoSceneGolePole")
                {
                    if (Source[i].Prop.ContainsKey("ModelName"))
                    {
                        Path = GetModelname(((Node)Source[i].Prop["ModelName"]).StringValue);
                    }
                }
                if (!System.IO.File.Exists(Path)) Path = PlaceHolderMod;
                Single X, Y, Z, ScaleX, ScaleY, ScaleZ, RotX, RotY, RotZ, A, B, C;
                Single[] scale_XDD;
                Single[] rotation;
                Single[] position;
                if (Source[i].Prop.ContainsKey("pos") == true)
                { position = (Single[])Source[i].Prop["pos"]; }
                else
                {
                    if (Source[i].Prop["pos_x"] != null)
                    {
                        A = Single.Parse(((Node)Source[i].Prop["pos_x"]).StringValue);
                        B = Single.Parse(((Node)Source[i].Prop["pos_y"]).StringValue);
                        C = Single.Parse(((Node)Source[i].Prop["pos_z"]).StringValue);
                        position = new Single[3] { A, B, C };

                        Source[i].Prop.Add("pos", position);
                        Source[i].Prop.Remove("pos_x");
                        Source[i].Prop.Remove("pos_y");
                        Source[i].Prop.Remove("pos_z");
                    }
                    else
                    {
                        position = new Single[3] { 1, 1, 1 };
                    }
                }
                X = position[0];
                Y = position[1];
                Z = position[2];
                //X = Single.Parse(((Node)Source[i].Prop["pos_x"]).StringValue);
                //Y = Single.Parse(((Node)Source[i].Prop["pos_y"]).StringValue);
                //Z = Single.Parse(((Node)Source[i].Prop["pos_z"]).StringValue);
                if (Source[i].Prop.ContainsKey("scale") == true)
                { scale_XDD = (Single[])Source[i].Prop["scale"]; }
                else
                {
                    if (Source[i].Prop["scale_x"] != null)
                    {
                        A = Single.Parse(((Node)Source[i].Prop["scale_x"]).StringValue);
                        B = Single.Parse(((Node)Source[i].Prop["scale_y"]).StringValue);
                        C = Single.Parse(((Node)Source[i].Prop["scale_z"]).StringValue);
                        scale_XDD = new Single[3] { A,B,C };

                        Source[i].Prop.Add("scale", scale_XDD);
                        Source[i].Prop.Remove("scale_x");
                        Source[i].Prop.Remove("scale_y");
                        Source[i].Prop.Remove("scale_z");

                        //cl.Scale = (Single[])((Single[])GetListByName(type)[index].Prop["scale"]).Clone();
                        /*Single[] test;
                        test = (Single[])((Single[])Source[i].Prop["scale"]).Clone();
                        string text;
                            text = test[2].ToString();
                        MessageBox.Show("test " + text);*/
                    }
                    else
                    {
                        scale_XDD = new Single[3] { 1, 1, 1 };
                    }
                }
                ScaleX = scale_XDD[0];
                ScaleY = scale_XDD[1];
                ScaleZ = scale_XDD[2];
                if (Source[i].Prop.ContainsKey("dir") == true)
                { rotation = (Single[])Source[i].Prop["dir"]; }
                else
                {
                    if (Source[i].Prop["dir_x"] != null)
                    {
                        A = Single.Parse(((Node)Source[i].Prop["dir_x"]).StringValue);
                        B = Single.Parse(((Node)Source[i].Prop["dir_y"]).StringValue);
                        C = Single.Parse(((Node)Source[i].Prop["dir_z"]).StringValue);
                        rotation = new Single[3] { A, B, C };

                        Source[i].Prop.Add("dir", rotation);
                        Source[i].Prop.Remove("dir_x");
                        Source[i].Prop.Remove("dir_y");
                        Source[i].Prop.Remove("dir_z");
                    }
                    else
                    {
                        rotation = new Single[3] { 1, 1, 1 };
                    }
                }
                RotX = rotation[0];
                RotY = rotation[1];
                RotZ = rotation[2];
                render.addModel(Path, Type, new Vector3D(X, -Z, Y), new Vector3D(ScaleX, ScaleZ, ScaleY), RotX, -RotZ, RotY, at);
            }
        }

        public void AddChildrenModels(C0List tmp, bool area)
        {
            if (tmp == null) return;
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            if (tmp.List.Count > 0)
            {
                List<string> modelsPaths = new List<string>();
                List<Vector3D> Pos = new List<Vector3D>();
                List<Vector3D> Rot = new List<Vector3D>();
                List<Vector3D> Scale = new List<Vector3D>();
                foreach (LevelObj o in tmp.List)
                {
                    string Path;
                    if (area) Path = "models\\UnkArea.obj"; else
                    {
                        if (!o.Prop.ContainsKey("dbname"))
                        {
                            if (NewObjectDatabase != null&& NewObjectDatabase.IdtoDB.ContainsKey(o.ToString()))
                            {
                                o.Prop.Add("dbname", NewObjectDatabase.IdtoDB[o.ToString()]);
                            }

                        }
                        Path = GetModelname(o.GetName(true));//db name
                        if (!System.IO.File.Exists(Path)) Path = "models\\UnkRed.obj";
                    }
                    Single X, Y, Z, ScaleX, ScaleY, ScaleZ, RotX, RotY, RotZ, A, B, C;
                    Single[] scale_XDD;
                    Single[] rotation;
                    Single[] position;
                    if (o.Prop.ContainsKey("pos") == true)
                    { position = (Single[])o.Prop["pos"]; }
                    else
                    {
                        if (o.Prop["pos_x"] != null)
                        {
                            A = Single.Parse(((Node)o.Prop["pos_x"]).StringValue);
                            B = Single.Parse(((Node)o.Prop["pos_y"]).StringValue);
                            C = Single.Parse(((Node)o.Prop["pos_z"]).StringValue);
                            position = new Single[3] { A, B, C };

                            o.Prop.Add("pos", position);
                            o.Prop.Remove("pos_x");
                            o.Prop.Remove("pos_y");
                            o.Prop.Remove("pos_z");
                        }
                        else
                        {
                            position = new Single[3] { 1, 1, 1 };
                        }
                    }
                    X = position[0];
                    Y = position[1];
                    Z = position[2];
                    //X = Single.Parse(((Node)Source[i].Prop["pos_x"]).StringValue);
                    //Y = Single.Parse(((Node)Source[i].Prop["pos_y"]).StringValue);
                    //Z = Single.Parse(((Node)Source[i].Prop["pos_z"]).StringValue);
                    if (o.Prop.ContainsKey("scale") == true)
                    { scale_XDD = (Single[])o.Prop["scale"]; }
                    else
                    {
                        if (o.Prop["scale_x"] != null)
                        {
                            A = Single.Parse(((Node)o.Prop["scale_x"]).StringValue);
                            B = Single.Parse(((Node)o.Prop["scale_y"]).StringValue);
                            C = Single.Parse(((Node)o.Prop["scale_z"]).StringValue);
                            scale_XDD = new Single[3] { A, B, C };

                            o.Prop.Add("scale", scale_XDD);
                            o.Prop.Remove("scale_x");
                            o.Prop.Remove("scale_y");
                            o.Prop.Remove("scale_z");
                        }
                        else
                        {
                            scale_XDD = new Single[3] { 1, 1, 1 };
                        }
                    }
                    ScaleX = scale_XDD[0];
                    ScaleY = scale_XDD[1];
                    ScaleZ = scale_XDD[2];
                    if (o.Prop.ContainsKey("dir") == true)
                    { rotation = (Single[])o.Prop["dir"]; }
                    else
                    {
                        if (o.Prop["dir_x"] != null)
                        {
                            A = Single.Parse(((Node)o.Prop["dir_x"]).StringValue);
                            B = Single.Parse(((Node)o.Prop["dir_y"]).StringValue);
                            C = Single.Parse(((Node)o.Prop["dir_z"]).StringValue);
                            rotation = new Single[3] { A, B, C };

                            o.Prop.Add("dir", rotation);
                            o.Prop.Remove("dir_x");
                            o.Prop.Remove("dir_y");
                            o.Prop.Remove("dir_z");
                        }
                        else
                        {
                            rotation = new Single[3] { 1, 1, 1 };
                        }
                    }
                    RotX = rotation[0];
                    RotY = rotation[1];
                    RotZ = rotation[2];
                    Pos.Add(new Vector3D(X, -Z, Y));
                    Rot.Add(new Vector3D(RotX, -RotZ, RotY));
                    Scale.Add(new Vector3D(ScaleX, ScaleZ, ScaleY));
                    modelsPaths.Add(Path);
                }
                render.AddTmpObjects(Pos, Scale, Rot, modelsPaths, area ? "TmpAreaChildrenObjs" : "TmpChildrenObjs");
            }
        }

        public string GetModelname(string ObjName)
        {
            if (ObjName.Contains("MovePoint")) return "models\\MovePoint.obj";
            if (CustomModels.Contains(ObjName)) return "CustomModels\\" + ObjName + ".obj";
            //else if (ObjectDatabase != null && ObjectDatabase.IdToModel.ContainsKey(ObjName)) return "models\\" + ObjectDatabase.IdToModel[ObjName] + ".obj";
            
            else if (NewObjectDatabase != null) 
            {
                string id = "";
                if (NewObjectDatabase.DBtoId.TryGetValue(ObjName, out id))
                {

                    if (CustomModels.Contains(id)) return "CustomModels\\" + id + ".obj";
                    if (NewObjectDatabase.IdToModel.ContainsKey(id))
                    {
                        return @"models\" + NewObjectDatabase.IdToModel[id] + ".obj";
                    }
                    else if (File.Exists("models\\" + id + ".obj")) return "models\\" + id + ".obj";
                }
                else if (NewObjectDatabase.IdToModel.ContainsKey(ObjName))
                {
                    return @"models\" + NewObjectDatabase.IdToModel[ObjName] + ".obj";
                }
                else return "models\\" + ObjName + ".obj";
            }
            else return "models\\" + ObjName + ".obj";
            return "models\\UnkBlue.obj";
        }

        void ProcessAllInfos(XmlNodeList xml)
        {
            for (int i = 0; i < xml.Count; i++)
            {

                if (!comboBox1.Items.Contains(xml[i].Attributes["Name"].Value)) comboBox1.Items.Add(xml[i].Attributes["Name"].Value);
                ProcessAllOBJECTS(xml[i].ChildNodes, xml[i].Attributes["Name"].Value);
            }
        }

        void ProcessRailInfos(XmlNodeList xml)
        {
            if (!highestID.ContainsKey("AllRailInfos")) highestID.Add("AllRailInfos", 0);
            for (int i = 0; i < xml.Count; i++)
            {
                foreach (XmlNode node in xml[i].ChildNodes)
                {
                    Rail tempR = LoadRail(node.ChildNodes, "AllRailInfos");
                    AllRailInfos.Add(tempR);
                    treeView1.Nodes["AllRailInfos"].Nodes.Add(tempR.Name).Tag = "Rail";

                }
            }
        }

        void ProcessAllOBJECTS(XmlNodeList xml, string Type)
        {
            if (!AllInfos.ContainsKey(Type)) AllInfos.Add(Type, new AllInfoSection());
            foreach (XmlNode N in xml)
            {
                LevelObj tempO = LoadOBJECT(N.ChildNodes, Type);
                GetListByName(Type).Add(tempO);

                if (NewObjectDatabase.Entries.ContainsKey(((Node)tempO.Prop["name"]).StringValue)) treeView1.Nodes[Type].Nodes.Add(NewObjectDatabase.Entries[((Node)tempO.Prop["name"]).StringValue].dbname);
                else treeView1.Nodes[Type].Nodes.Add(((Node)tempO.Prop["name"]).StringValue);
            }
        }

        Rail LoadRail(XmlNodeList xml, string Type)
        {
            if (!highestID.ContainsKey(Type)) highestID.Add(Type, 0);
            Rail Ret = new Rail();
            List<int> Args = new List<int>();
            for (int i = 0; i < xml.Count; i++)
            {
                XmlNode xNode = xml[i];
                if (xNode.NodeType == XmlNodeType.Element)
                {
                    if (xNode.Attributes["Name"].Value.StartsWith("Arg")) Args.Add(Int32.Parse(xNode.Attributes["StringValue"].Value));
                    else if (xNode.Attributes["Name"].Value == "LayerName") Ret.LayerName = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "closed") Ret._closed = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "l_id") Ret.l_id = Int32.Parse(xNode.Attributes["StringValue"].Value);
                    else if (xNode.Attributes["Name"].Value == "name") Ret.Name = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "no") Ret.no = Int32.Parse(xNode.Attributes["StringValue"].Value);
                    else if (xNode.Attributes["Name"].Value == "type") Ret.Type = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "Points")
                    {
                        XmlNodeList PointsList = xNode.ChildNodes;
                        foreach (XmlNode Points in PointsList)
                        {
                            Rail.Point P = new Rail.Point();
                            List<int> _Args = new List<int>();
                            List<Single> _X = new List<Single>();
                            List<Single> _Y = new List<Single>();
                            List<Single> _Z = new List<Single>();
                            foreach (XmlNode Point in Points.ChildNodes)
                            {
                                if (Point.Attributes["Name"].Value.StartsWith("Arg")) _Args.Add(Int32.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value.EndsWith("_x")) _X.Add(Single.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value.EndsWith("_y")) _Y.Add(Single.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value.EndsWith("_z")) _Z.Add(Single.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value == "id") P.ID = (Int32.Parse(Point.Attributes["StringValue"].Value));
                            }
                            P.Args = _Args;
                            P._X = _X;
                            P._Y = _Y;
                            P._Z = _Z;
                            Ret.Points.Add(P);
                        }
                    }
                    if (xNode.Attributes["Name"].Value == "l_id") if (Int32.Parse(xNode.Attributes["StringValue"].Value) > highestID[Type]) highestID[Type] = Int32.Parse(xNode.Attributes["StringValue"].Value);
                }
            }
            if (Args.Count != 0) Ret.Arg = Args;
            return Ret;
        }

        LevelObj LoadOBJECT(XmlNodeList xml, string Type)
        {
            if (!highestID.ContainsKey(Type)) highestID.Add(Type, 0);
            LevelObj Ret = new LevelObj();
            List<int> Args = new List<int>();
            AllInfoSection Children = new AllInfoSection();
            AllInfoSection AreaChildren = new AllInfoSection();
            for (int i = 0; i < xml.Count; i++)
            {
                XmlNode xNode = xml[i];
                if (xNode.NodeType == XmlNodeType.Element)
                {
                    if (xNode.Attributes["Name"].Value.StartsWith("Arg")) Args.Add(Int32.Parse(xNode.Attributes["StringValue"].Value));
                    else
                    {
                        if (xNode.Name == "C1") // Rail editing
                        {
                            if (xNode.Attributes["Name"].Value == "Rail") Ret.Prop.Add("Rail", LoadRail(xNode.ChildNodes, "AllRailInfos").l_id);
                            else throw new Exception("C1 type not implemented :(");
                        }
                        else if (xNode.Name == "C0")//C0 List editing
                        {// instead of adding as children to the current object we add every children as their own objects
                            C0List c0Section = new C0List();
                            XmlNodeList objList = xNode.ChildNodes;
                            foreach (XmlNode Object in objList)
                            {
                                //c0Section.List.Add(LoadOBJECT(Object.ChildNodes, Type));
                                if (xNode.Attributes["Name"].Value == "AreaChildren")
                                {
                                    //GetListByName("AreaObjInfo").Add(LoadOBJECT(Object.ChildNodes, Type));
                                    AreaChildren.Add(LoadOBJECT(Object.ChildNodes, Type));
                                }
                                else
                                {

                                    //GetListByName(Type).Add(LoadOBJECT(Object.ChildNodes, Type));
                                    Children.Add(LoadOBJECT(Object.ChildNodes, Type));
                                }
                            }
                            //Ret.Prop.Add(xNode.Attributes["Name"].Value, c0Section);
                        }
                        else if (xNode.Attributes["Name"].Value != "Rail")
                            Ret.Prop.Add(xNode.Attributes["Name"].Value, new Node(xNode.Attributes["StringValue"].Value, xNode.Name));
                        if (xNode.Attributes["Name"].Value == "l_id")
                        {
                            if (Int32.Parse(xNode.Attributes["StringValue"].Value) > highestID[Type]) highestID[Type] = Int32.Parse(xNode.Attributes["StringValue"].Value);

                            if (Ret.Prop.ContainsKey("l_id") && AllInfos[Type].GetById(int.Parse(((Node)Ret.Prop["l_id"]).StringValue)) != -1)// if an object already contains this id then change this id to highest + 1
                            {
                                ((Node)Ret.Prop["l_id"]).StringValue = (highestID[Type] + 1).ToString();
                                highestID[Type] += 1;
                            }
                            if (Children.Count >0)
                            {
                                foreach (LevelObj o in Children)
                                {
                                    if (!o.Prop.ContainsKey("GenerateParent"))
                                    {
                                        o.Prop.Add("GenerateParent", new Node(((Node)Ret.Prop["l_id"]).StringValue, xNode.Name));
                                    }
                                    else if (((Node)o.Prop["GenerateParent"]).StringValue != ((Node)Ret.Prop["l_id"]).StringValue)
                                    {
                                        ((Node)o.Prop["GenerateParent"]).StringValue = ((Node)Ret.Prop["l_id"]).StringValue;
                                    }
                                    if (o.Prop.ContainsKey("l_id") && ((Node)o.Prop["l_id"]).StringValue == ((Node)Ret.Prop["l_id"]).StringValue)// if an object already contains this id then change this id to highest + 1
                                    {
                                        ((Node)o.Prop["l_id"]).StringValue = (highestID[Type] + 1).ToString();
                                        highestID[Type] += 1;
                                    }
                                    if (NewObjectDatabase.Entries.ContainsKey(((Node)o.Prop["name"]).StringValue)) treeView1.Nodes[Type].Nodes.Add(NewObjectDatabase.Entries[((Node)o.Prop["name"]).StringValue].dbname);
                                    else treeView1.Nodes[Type].Nodes.Add(((Node)o.Prop["name"]).StringValue);
                                    GetListByName(Type).Add(o);
                                }
                            }
                            if (AreaChildren.Count > 0)
                            {
                                foreach (LevelObj o in AreaChildren)
                                {
                                    if (!o.Prop.ContainsKey("AreaParent"))
                                    {
                                        o.Prop.Add("AreaParent", new Node(((Node)Ret.Prop["l_id"]).StringValue, xNode.Name));
                                    }
                                    else if (((Node)o.Prop["AreaParent"]).StringValue != ((Node)Ret.Prop["l_id"]).StringValue)
                                    {
                                        ((Node)o.Prop["AreaParent"]).StringValue = ((Node)Ret.Prop["l_id"]).StringValue;
                                    }

                                    if (NewObjectDatabase.Entries.ContainsKey(((Node)o.Prop["name"]).StringValue)) treeView1.Nodes["AreaObjInfo"].Nodes.Add(NewObjectDatabase.Entries[((Node)o.Prop["name"]).StringValue].dbname);
                                    else treeView1.Nodes["AreaObjInfo"].Nodes.Add(((Node)o.Prop["name"]).StringValue);
                                    GetListByName("AreaObjInfo").Add(o);
                                }
                            }
                        }
                    }
                }
            }
            if (Args.Count != 0) Ret.Prop.Add("Arg", Args.ToArray());
            if (!Ret.Prop.ContainsKey("GenerateParent")&& !Ret.Prop.ContainsKey("AreaParent"))
            {
                if (Type == "AreaObjInfo") Ret.Prop.Add("AreaParent", new Node("-1", "D1"));
                else Ret.Prop.Add("GenerateParent", new Node("-1", "D1"));
            }

            return Ret;
        }

        Camera3DL LoadCAMERA(XmlNodeList xml)
        {
            Camera3DL Ret = new Camera3DL();

            for (int i = 0; i < xml.Count; i++)
            {
                XmlNode xNode = xml[i];
                if (xNode.NodeType == XmlNodeType.Element)
                { 
                    if (xNode.Name == "C1") 
                    {
                        Ret.Attributes.Add(xNode.Attributes["Name"].Value, CameraC1(xNode));
                    }
                    else if (xNode.Name == "D0")
                    {
                        Ret.Attributes.Add(xNode.Attributes["Name"].Value, new Node(xNode.Attributes["StringValue"].Value, xNode.Name));
                    }
                    else if (xNode.Name == "D1")
                    {

                        if (xNode.Attributes["Name"].Value == "UserGroupId")
                        {
                            Ret.UserGroupId = Int32.Parse(xNode.Attributes["StringValue"].Value);
                        }
                        else
                        {

                            Ret.Attributes.Add(xNode.Attributes["Name"].Value, new Node(xNode.Attributes["StringValue"].Value, xNode.Name));
                        }
                    }
                    else if (xNode.Name == "D2")
                    {

                        Ret.Attributes.Add(xNode.Attributes["Name"].Value, new Node(xNode.Attributes["StringValue"].Value, xNode.Name));
                    }
                    else if (xNode.Name == "A0")
                    {
                        if (xNode.Attributes["Name"].Value == "UserName")
                        {
                            Ret.UserName = xNode.Attributes["StringValue"].Value;
                        }
                        else if (xNode.Attributes["Name"].Value == "Category")
                        {

                            Ret.Category = xNode.Attributes["StringValue"].Value;
                        }
                        else if (xNode.Attributes["Name"].Value == "Class")
                        {
                            Ret.Class = xNode.Attributes["StringValue"].Value;

                        }
                        else
                        {
                            Ret.Attributes.Add(xNode.Attributes["Name"].Value, new Node(xNode.Attributes["StringValue"].Value, xNode.Name));
                        }
                    }
                    
                }
            }



            return Ret;
        }

        Node CameraC1( XmlNode xNode)
        {
            XmlNodeList xml = xNode.ChildNodes;
            Dictionary<string,Node> temp = new Dictionary<string,Node>();
            for (int j = 0; j < xNode.ChildNodes.Count; j++)
            {
                XmlNode tempxNode = xml[j];
                if (tempxNode.NodeType == XmlNodeType.Element)
                {
                    if (tempxNode.Name == "C1")
                    {
                        Node xmld = CameraC1(tempxNode);
                        temp.Add(tempxNode.Attributes["Name"].Value, xmld);
                    }
                    else
                    {
                        temp.Add(tempxNode.Attributes["Name"].Value, new Node(tempxNode.Attributes["StringValue"].Value, tempxNode.Name));
                    }
                }

            }
            return new Node(temp, "C1");
            //Ret.Attributes.Add(xNode.Attributes["Name"].Value, new Node(null, "C1"));
        }
        #endregion

        #region ObjectsEvents

        #region Find
        private void objectByIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(CurrentAllInfosSectionName, "l_id", Convert.ToInt32(f.Res));
        }

        private void objectByCameraIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(CurrentAllInfosSectionName, "CameraId", Convert.ToInt32(f.Res));
        }

        private void Switch___FindClick(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(CurrentAllInfosSectionName, ((ToolStripMenuItem)sender).Text, Convert.ToInt32(f.Res));
        }

        private void objectByViewIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(CurrentAllInfosSectionName, "ViewId", Convert.ToInt32(f.Res));
        }

        private void objectByRailNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput(true);
            f.ShowDialog();
            if (f.Res == null || (string)f.Res == "") return;
            FindIndex(CurrentAllInfosSectionName, "Rail", (string)f.Res);
        }
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*   if (comboBox1.Text != "AllRailInfos")
               {
                   if (ObjectsListBox.Items.Count < )
                   {
                       panel1.Visible = false;
                       panel1.Enabled = false;
                   }
                   else
                   {
                       panel1.Visible = true;
                       panel1.Enabled = true;
                   }
               }
               else
               { */
            SelectInfoIndex(-1);
            ObjInfoSel = -1;
            AreaObjOldSelection = -1;
            CameraAreaOldSelection = -1;
            /* }*/
            index = -2;
            btn_cameraCode.Visible = false;
            ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;
            List<string> remove = new List<string>();
            List<string> add = new List<string>();
            if (!AllInfos.ContainsKey(comboBox1.Text))
            {
                if (comboBox1.Text == "AllRailInfos")
                {
                    //checkBox1.Visible = false;
                    for (int i = 0; i < AllRailInfos.Count; i++) ObjectsListBox.Items.Add(AllRailInfos[i].ToString());
                    remove = new List<string>
                    {
                        "General",
                        "Extra",
                        "DemoExtra",
                        "StartGeneral",
                        "Args", 
                        "DefArgs"

                    };
                    add = new List<string>();
                    add.Add("RailTab");
                    add.Add("Args");

                    //RefreshTabs(remove, add);
                }
                ObjectsListBox.SelectionMode = SelectionMode.One;
            }
            else
            {
                ObjectsListBox.SelectionMode = SelectionMode.MultiExtended;// ObjectsListBox.SelectionMode = SelectionMode.One; //ObjectsListBox.SelectionMode = SelectionMode.MultiExtended;

                if (comboBox1.Text == "CameraAreaInfo" || comboBox1.Text == "DemoSceneObjInfo" || comboBox1.Text == "StartInfo" || comboBox1.Text == "StartEventObjInfo") ObjectsListBox.SelectionMode = SelectionMode.One;
                //for (int i = 0; i < CurrentAllInfosSection.Count; i++) ObjectsListBox.Items.Add(CurrentAllInfosSection[i].GetName(true));//db name
            }
            if (selecting) return;
            if (CurrentAllInfosSelectedIndex != -1) SelectCorrectProperty();
            if (ObjectsListBox.SelectedIndices.Count == 1)
            {
                if (NewObjectDatabase != null)
                {
                    if (comboBox1.Text != "StartInfo" && comboBox1.Text != "CameraAreaInfo")
                    {
                        if (NewObjectDatabase.DBtoId.ContainsKey(ObjectsListBox.SelectedItem.ToString()))
                        {
                            remove.Insert(0, "GroupArgs");
                            remove.Insert(0, "DefArgs");
                            remove.Insert(0, "Args");
                            remove.Add("GroupGen");
                            add.Add("Args");
                        }
                        else
                        {
                            remove.Insert(0, "DefArgs");
                            remove.Insert(0, "GroupArgs");
                            remove.Insert(0, "Args");
                            remove.Add("GroupGen");
                            add.Add("DefArgs");
                            if (comboBox1.Text == "AllRailInfos")
                            {

                                label14.Text = "";
                            }
                            else
                            {
                                label14.Text = "This object's args are undocumented!";
                            }
                        }
                    }
                    else
                    {
                        remove.Insert(0, "GroupArgs");
                        remove.Insert(0, "DefArgs");
                        remove.Insert(0, "Args");
                        remove.Add("GroupGen");
                    }
                }
                else
                {
                    if (comboBox1.Text != "StartInfo" && comboBox1.Text != "CameraAreaInfo")
                    {
                        remove.Add("DefArgs");
                        remove.Insert(0, "GroupArgs");
                        remove.Insert(0, "Args");
                        remove.Add("GroupGen");
                        add.Add("DefArgs");
                        if (comboBox1.Text == "AllRailInfos")
                        {
                            label14.Text = "";
                        }
                        else
                        {
                            label14.Text = "This object's args are undocumented!";
                        }
                    }
                }
            }
        }

        bool selecting = false;
        bool changedAllInfos = false;
        private void render_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control || RenderIsDragging) return;
            object[] indexes = render.GetOBJ(sender, e); //indexes[0] Allinfos name, [1] int index of the object
            if (indexes[0] == null) return; //this means indexes[0] = -1
            if ((string)indexes[0] == "SelectedRail" || ((string)indexes[0]).Contains("RailHandle") || (string)indexes[0] == "TmpChildrenObjs" || (string)indexes[0] == "TmpAreaChildrenObjs" || (IsEditingC0List && (string)indexes[0] != "C0EditingListObjs")) return;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift && (string)indexes[0] == CurrentAllInfosSectionName && (int)indexes[1] == CurrentAllInfosSelectedIndex)
            {
                SelectInfoIndex(-1);
            }
            else if ((ModifierKeys & Keys.Shift) == Keys.Shift && (string)indexes[0] == CurrentAllInfosSectionName && CurrentAllInfosSelectedIndex != -1)
            {
                treeView1.SelectedNodes.Add(treeView1.Nodes[(string)indexes[0]].Nodes[(int)indexes[1]]);
            }
            else
            {
                if (RenderIsDragging == true){ return; }
                if (DraggingAxis[0] || DraggingAxis[1] || DraggingAxis[2]) { return; }
                selecting = true;
                if ((string)indexes[0] != "C0EditingListObjs") 
                {
                    if (CurrentAllInfosSectionName != (string)indexes[0])
                    {
                        //comboBox1.SelectedIndex = comboBox1.Items.IndexOf((string)indexes[0]);
                        changedAllInfos = true;
                    }
                }
                if (treeView1.SelectedNodes.Count > 0)
                {
                    SelectInfoIndex(-1);
                }
                selecting = false;

                SelectInfoIndex((string)indexes[0], (int)indexes[1]);
                changedAllInfos = false;
            }
        }
        bool RenderIsDragging = false;
        object[] DraggingArgs = null;
        bool[] DraggingAxis = new bool[5];
        Vector3D StartPos;

        private void render_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) //Render hotkeys
        {
            if (e.Key == Key.X)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[0] == false && (DraggingAxis[1] == false || DraggingAxis[2] == false) && (string)DraggingArgs[0] != "SelectedRail" && (string)DraggingArgs[0] != "RailHandle")
                    {
                        DraggingAxis[0] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "X");
                    }
                    else if (DraggingAxis[0] == true && (DraggingAxis[1] == false || DraggingAxis[2] == false))
                    {
                        DraggingAxis[0] = false;
                        render.removeAxis("X");
                    }
                    else if (DraggingAxis[0] == false && (DraggingAxis[1] == true || DraggingAxis[2] == true))
                    {
                        DraggingAxis[0] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "X");
                        render.removeAxis("Y");
                        DraggingAxis[1] = false;
                        render.removeAxis("Z");
                        DraggingAxis[2] = false;
                    }
                }
            }
            if (e.Key == Key.Y)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[1] == false && (DraggingAxis[0] == false || DraggingAxis[2] == false)&& (string)DraggingArgs[0]!="SelectedRail" && (string)DraggingArgs[0] != "RailHandle")
                    {
                        DraggingAxis[1] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Y");
                    }
                    else if (DraggingAxis[1] == true && (DraggingAxis[0] == false || DraggingAxis[2] == false))
                    {
                        DraggingAxis[1] = false;
                        render.removeAxis("Y");
                    }
                    else if (DraggingAxis[1] == false && (DraggingAxis[0] == true || DraggingAxis[2] == true))
                    {
                        DraggingAxis[1] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Y");
                        render.removeAxis("X");
                        DraggingAxis[0] = false;
                        render.removeAxis("Z");
                        DraggingAxis[2] = false;
                    }
                }
            }
            if (e.Key == Key.S)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[3] == false)
                    {
                        DraggingAxis[4] = false;
                        DraggingAxis[3] = true;
                    }
                    else { DraggingAxis[3] = false; }
                }
            }
            if (e.Key == Key.R)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[4] == false)
                    {
                        DraggingAxis[4] = true;
                        DraggingAxis[3] = false;
                    }
                    else { DraggingAxis[4] = false; }
                }
            }
            if (e.Key == Key.Z)
            {
                if (Undo.Count > 0 && RenderIsDragging==false && (ModifierKeys & Keys.Control) == Keys.Control) Undo.Pop().Undo();
                else if (RenderIsDragging == true)
                {
                    if (DraggingAxis[2] == false && (DraggingAxis[0] == false || DraggingAxis[1] == false) && (string)DraggingArgs[0] != "SelectedRail" && (string)DraggingArgs[0] != "RailHandle")
                    {
                        DraggingAxis[2] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Z");
                    }
                    else if (DraggingAxis[2] == true && (DraggingAxis[0] == false || DraggingAxis[1] == false))
                    {
                        DraggingAxis[2] = false;
                        render.removeAxis("Z");
                    }
                    else if (DraggingAxis[2] == false && (DraggingAxis[0] == true || DraggingAxis[1] == true))
                    {
                        DraggingAxis[2] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Z");
                        render.removeAxis("X");
                        DraggingAxis[0] = false;
                        render.removeAxis("Y");
                        DraggingAxis[1] = false;
                    }
                }
                return;
            }
            if (e.Key == Key.Escape)
            {
                if (treeView1.SelectedNode != null)
                {
                    SelectInfoIndex(-1);
                }
            }
            if (treeView1.SelectedNode == null ||treeView1.SelectedNode.Index == -1) return;
            if (CurrentAllInfosSectionName == "AllRailInfos")
            {
                if (e.Key == Key.N)
                {
                    Rail tmp = AllRailInfos[CurrentAllInfosSelectedIndex];
                    tmp.Points.Add(tmp.Points[tmp.Points.Count - 1].Clone_increment());
                    UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
                    render.SelectRail(AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
                }
            }
            
            if (e.Key == Key.Space && CurrentAllInfosSelectedIndex != -1) render.CameraToObj(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex);
            if ((ModifierKeys & Keys.Shift) == Keys.Shift && e.Key == Key.A) { if (Btn_AddObj.Enabled == true) BtnAddObj_Click(null, null); } //Add obj
            else if (e.Key == Key.D && treeView1.SelectedNodes.Count > 0) button2_Click(null, null); //Duplicate
            else if (e.Key == Key.Delete) button3_Click(null, null); //Delete obj
            else if (e.Key == Key.F && (ModifierKeys & Keys.Control) == Keys.Control) findToolStripMenuItem.ShowDropDown();
            else return;
        }

        private void Listbox_keyDown(object sender, System.Windows.Forms.KeyEventArgs e) //Listbox hotkeys
        {

        }


        private void render_KeyUP(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                if (DraggingArgs != null) endDragging();
                RenderIsDragging = false;
                DraggingArgs = null;
                propertyGrid1.Refresh();
            }
        }

        void UpdateRailpos(int id, List<Point3D[]> Points)
        {
            render.UpdateRailpos(id, Points, AllRailInfos[id].Closed);
            if (CurrentAllInfosSectionName == "AllRailInfos" && CurrentAllInfosSelectedIndex != -1) render.SelectRail(AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
        }
        List<Vector3D> DragPos = new List<Vector3D>();
        Vector3D Displacement = new Vector3D();
        private void render_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed || (ModifierKeys & Keys.Control) != Keys.Control || !RenderIsDragging) { RenderIsDragging = false; return; }
            int RoundTo = (ModifierKeys & Keys.Alt) == Keys.Alt ? 100 : ((ModifierKeys & Keys.Shift) == Keys.Shift ? 50 : 0);
            if (((string)DraggingArgs[0] == "SelectedRail" || ((string)DraggingArgs[0]) == "RailHandle") && (ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                RoundTo = 0;
            }
            Vector3D NewPos = render.Drag(DraggingArgs, e, RoundTo);
            DragPos.Add(NewPos);
            if (DragPos.Count == 1) { Displacement = (Vector3D)DraggingArgs[2] - NewPos; /*labelStatus.Text = (-Displacement.Z).ToString();*/ }
            if (NewPos == null) return;
            if ((string)DraggingArgs[0] == "SelectedRail" && (ModifierKeys & Keys.Alt) != Keys.Alt)
            {
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].X_prev -= AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].X - (float)NewPos.X;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].X_next -= AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].X - (float)NewPos.X;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].X = (float)NewPos.X;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Y_prev -=  AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Y - (float)NewPos.Z;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Y_next -=  AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Y - (float)NewPos.Z;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Y = (float)NewPos.Z;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Z_prev -= AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Z + (float)NewPos.Y;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Z_next -= AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Z + (float)NewPos.Y;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[(int)DraggingArgs[1]].Z = -(float)NewPos.Y;
                UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
            }else if (((string)DraggingArgs[0]) == "RailHandle" && (ModifierKeys & Keys.Alt) != Keys.Alt)
            {
                int PntIndx = (int)DraggingArgs[1] / 2;
                int HandleIndx = ((int)DraggingArgs[1]) % 2 + 1;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[PntIndx]._X[HandleIndx] = (float)NewPos.X;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[PntIndx]._Y[HandleIndx] = (float)NewPos.Z;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[PntIndx]._Z[HandleIndx] = -(float)NewPos.Y;
                UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
            }
            else if (((string)DraggingArgs[0] == "SelectedRail" || ((string)DraggingArgs[0]) == "RailHandle"))
            {
                int PntIndx = ((string)DraggingArgs[0]) == "RailHandle"?(int)DraggingArgs[1] / 2 : (int)DraggingArgs[1];

                AllRailInfos[CurrentAllInfosSelectedIndex].Points[PntIndx].setX = (float)NewPos.X;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[PntIndx].setY = (float)NewPos.Z;
                AllRailInfos[CurrentAllInfosSelectedIndex].Points[PntIndx].setZ = -(float)NewPos.Y;
                UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());

            }
            else if ((string)DraggingArgs[0] == "TmpChildrenObjs")
            {
                ((Single[])((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[0] = (Single)NewPos.X;
                ((Single[])((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[1] = (Single)NewPos.Z;
                ((Single[])((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[2] = -(Single)NewPos.Y;
                UpdateOBJPos((int)DraggingArgs[1], null, "TmpChildrenObjs");
            }
            else if ((string)DraggingArgs[0] != "AllRailInfos" && (string)DraggingArgs[0] != "RailHandleRail")
            {
                NewPos = new Vector3D(((NewPos.X) + Displacement.X)*1, NewPos.Y + Displacement.Y, NewPos.Z + Displacement.Z);
                string type = "pos";
                //if (DraggingAxis[3]==true) { type = "scale"; }
                //if (DraggingAxis[4]==true) { type = "dir"; }
                if ((DraggingAxis[0] == false && DraggingAxis[1] == false && DraggingAxis[2] == false)|| (DraggingAxis[0] == true && DraggingAxis[1] == true && DraggingAxis[2] == true))
                {
                    ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[0] = (Single)NewPos.X;
                    ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[1] = (Single)NewPos.Z;
                    ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[2] = -(Single)NewPos.Y;
                    //render.removeAxis();

                }
                else
                {
                    if(DraggingAxis[0] == true)
                    {
                        ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[0] = (Single)NewPos.X;
                    }
                    if (DraggingAxis[1] == true)
                    {
                        ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[1] = (Single)NewPos.Z;
                    }
                    if (DraggingAxis[2] == true)
                    {
                        ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[2] = -(Single)NewPos.Y;
                    }
                }

                UpdateOBJPos((int)DraggingArgs[1], GetListByName((string)DraggingArgs[0]), (string)DraggingArgs[0]);
            }
            DraggingArgs[2] = NewPos;
        }

        void endDragging()
        {

            DragPos.Clear();
            SelectCorrectProperty();
            if (DraggingArgs[0] == null || DraggingArgs[1] == null || DraggingArgs[2] == null) return;
            if (IsEditingC0List && (string)DraggingArgs[0] != "C0EditingListObjs") return;

            if (DraggingAxis[0] == true) { render.removeAxis("X"); DraggingAxis[0] = false; }
            if (DraggingAxis[1] == true) { render.removeAxis("Y"); DraggingAxis[1] = false; }
            if (DraggingAxis[2] == true) { render.removeAxis("Z"); DraggingAxis[2] = false; }
            if ((string)DraggingArgs[0] == "RailHandle") return;
            if ((string)DraggingArgs[0] == "SelectedRail")
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    Vector3D Disp = new Vector3D(((Vector3D)args[2]).X - AllRailInfos[(int)args[0]].Points[(int)args[1]].X, ((Vector3D)args[2]).Z - AllRailInfos[(int)args[0]].Points[(int)args[1]].Y, ((Vector3D)args[2]).Y + AllRailInfos[(int)args[0]].Points[(int)args[1]].Z);
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].X = (float)((Vector3D)args[2]).X;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].X_prev += (float)Disp.X;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].X_next += (float)Disp.X;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Y = (float)((Vector3D)args[2]).Z;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Y_prev +=  (float)Disp.Y;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Y_next +=  (float)Disp.Y;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Z = -(float)((Vector3D)args[2]).Y;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Z_prev -= (float)Disp.Z;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Z_next -= (float)Disp.Z;
                    UpdateRailpos((int)args[0], AllRailInfos[(int)args[0]].GetPointArray());
                    propertyGrid1.Refresh();
                };
                Undo.Push(new UndoAction("Moved " + treeView1.SelectedNode.Text.ToString() + "'s point[" + DraggingArgs[1].ToString() + "] : ", new object[] { CurrentAllInfosSelectedIndex, (int)DraggingArgs[1], StartPos }, act));

                SaveChangeLabel();
            }
            else if ((string)DraggingArgs[0] != "AllRailInfos")
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    render.ClearSelection();
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    int id = (int)args[1];
                    Vector3D pos = (Vector3D)args[2];
                    ((Single[])type[id].Prop["pos"])[0] = (Single)pos.X; //These values were stored directly
                    ((Single[])type[id].Prop["pos"])[1] = (Single)pos.Y;
                    ((Single[])type[id].Prop["pos"])[2] = (Single)pos.Z;
                    string typename = (string)args[3];
                    if (typename != "C0EditingListObjs" || type.GetHashCode() == CurrentAllInfosSection.GetHashCode())
                    {
                        UpdateOBJPos(id, type, typename, true);
                    }
                    if (CurrentAllInfosSelectedIndex == id) RefreshTmpChildrenObjects();
                    propertyGrid1.Refresh();
                };
                Undo.Push(new UndoAction("Moved object : " + GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].GetName(true)/*db name*/, new object[] { GetListByName((string)DraggingArgs[0]), DraggingArgs[1], StartPos, DraggingArgs[0] }, act));

            SaveChangeLabel();
            }
            if (CurrentAllInfosSectionName != "AllRailInfos")
            {
               /* Panell1.Obj = CurrentAllInfosSection[CurrentInfosSelectedIndex];
                Panell1.Rail = null;
            }
            else
            {
                Panell1.Obj = null;
                Panell1.Rail = AllRailInfos[CurrentInfosSelectedIndex];*/
            }
            index = CurrentAllInfosSelectedIndex;
            RefreshProperties();
            //Panell1.Refresh();
        }

        private void render_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingArgs != null) endDragging();
            RenderIsDragging = false;
            DraggingArgs = null;
        }

        private void render_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != Keys.Control || RenderIsDragging) return;
            RenderIsDragging = true;
            DraggingArgs = render.GetOBJ(sender, e);
            if (DraggingArgs[0] == null) { RenderIsDragging = false; return; }
            if ((string)DraggingArgs[0] == "SelectedRail" && !IsEditingC0List)
            {
                StartPos = AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray()[(int)DraggingArgs[1]][0].ToVect();
                return;
            }
            else if ((string)DraggingArgs[0] == "TmpChildrenObjs")
            {
                StartPos = new Vector3D(
                    ((Single[])((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[0],
                    ((Single[])((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[1],
                    ((Single[])((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[2]);
            }
            else if ((string)DraggingArgs[0] == "C0EditingListObjs")
            {
                if (CurrentAllInfosSelectedIndex != (int)DraggingArgs[1])
                {
                    ObjectsListBox.ClearSelected();
                    SelectInfoIndex((string)DraggingArgs[0],(int)DraggingArgs[1]);
                }
                StartPos = new Vector3D((Single)((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[0],
                       (Single)((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[1],
                       (Single)((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[2]);
            }
            else if (((string)DraggingArgs[0]).Contains("RailHandle"))
            {
            }
            else if (!IsEditingC0List)
            {
                //comboBox1.SelectedIndex = comboBox1.Items.IndexOf((string)DraggingArgs[0]);
                if (CurrentAllInfosSelectedIndex != (int)DraggingArgs[1])
                {
                    ObjectsListBox.ClearSelected();
                    SelectInfoIndex((string)DraggingArgs[0], (int)DraggingArgs[1]);
                }
                if ((string)DraggingArgs[0] != "AllRailInfos")
                    StartPos = new Vector3D(
                    float.Parse(((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[0].ToString()),
                    float.Parse(((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[1].ToString()),
                    float.Parse(((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[2].ToString()));
            }
            else { RenderIsDragging = false; return; }
        }

        private void ZoomCheckWarning_Tick(object sender, EventArgs e)
        {
            //Maybe not the best way to accomplish this, but it works... // (Spoiler) It doesn't
            /*double d = Math.Abs(render.TooCloseCheck());
            if (d <= 130)
            {
                render.CameraToObj(CurrentAllInfosSectionName, CurrentInfosSelectedIndex);
                //label3.Text = "You are zooming in too much, the camera may glitch, zoom out to fix.";
            }
            else if (d > 1000000)
            {
                if (CurrentInfosSelectedIndex == -1) label3.Text = "You are too far from the level, select an object and press space.";
                else label3.Text = "You are too far from the selected object, press space.";
            }
            else label3.Text = "";*/
        }

        #region Hiding layers
        void HideLayer(string layerName)
        {
            if (layerName == "" || layerName == null) return;
            if (!AllInfos.ContainsKey(layerName)) return;
            AllInfos[layerName].IsHidden = true;
            render.HideGroup(layerName);
        }

        void ShowLayer(string layerName)
        {
            if (layerName == "" || layerName == null) return;
            if (!AllInfos.ContainsKey(layerName)) return;
            AllInfos[layerName].IsHidden = false;
            for (int i = 0; i < AllInfos[layerName].Count; i++) UpdateOBJPos(i, AllInfos[layerName], layerName);
        }

        void HideShowRails(bool hide)
        {
            if (hide)
            {
                
            }
            else
            {


            }
        }

        void HideScenario(string Scenario, int ScId)
        {
            foreach (string info in AllInfos.Keys)
            {
                if ((info == "AreaObjInfo" && ShowAreas[1] == false) || (info == "CameraAreaInfo" && ShowAreas[0] == false)) continue;
                if ((ScId != 0 && ScId != 4) && ShownScenarios[ScId] == false)
                {
                    string newscen = "シナリオ";
                    if ((ScId == 1 || ScId == 2) && (ShownScenarios[1] == false && ShownScenarios[2] == false))
                    {
                        newscen += "1＆2";
                    }

                    foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(newscen))
                    {
                        render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    }
                    newscen = "シナリオ";
                    if ((ScId == 2 || ScId == 3) && (ShownScenarios[3] == false && ShownScenarios[2] == false))
                    {
                        newscen += "2＆3";
                    }
                    foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(newscen))
                    {
                        render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    }
                    newscen = "シナリオ";
                    if ((ScId == 1 || ScId == 3) && (ShownScenarios[1] == false && ShownScenarios[3] == false))
                    {
                        newscen += "1＆3";
                    }
                    foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(newscen))
                    {
                        render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    }
                }
                foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(Scenario))
                {
                    render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(0, 0, 0), 0, 0, 0, false);
                }
            }
            
        }

        void ShowScenario(string Scenario, int ScId)
        {
            foreach (string info in AllInfos.Keys)
            {
                if ((info == "AreaObjInfo" && ShowAreas[1] == false) || (info == "CameraAreaInfo" && ShowAreas[0] == false)) continue;

                if ((ScId != 0 && ScId != 4) && ShownScenarios[ScId] == true)
                {
                    string newscen = "シナリオ";
                    if ((ScId == 1 || ScId == 2) && (ShownScenarios[1] == true || ShownScenarios[2] == true))
                    {
                        newscen += "1＆2";
                    }

                    foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(newscen))
                    {
                        if (info == "AreaObjInfo" || info == "CameraAreaInfo")
                        {
                            render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[2], ((Single[])obj.Prop["scale"])[1]), ((Single[])obj.Prop["dir"])[0], ((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                        }
                        else
                        {
                            render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[1], ((Single[])obj.Prop["scale"])[2]), ((Single[])obj.Prop["dir"])[0], -((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                        }
                    }
                    newscen = "シナリオ";
                    if ((ScId == 2 || ScId == 3) && (ShownScenarios[3] == true || ShownScenarios[2] == true))
                    {
                        newscen += "2＆3";
                    }
                    foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(newscen))
                    {
                        if (info == "AreaObjInfo" || info == "CameraAreaInfo")
                        {
                            render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[2], ((Single[])obj.Prop["scale"])[1]), ((Single[])obj.Prop["dir"])[0], ((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                        }
                        else
                        {
                            render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[1], ((Single[])obj.Prop["scale"])[2]), ((Single[])obj.Prop["dir"])[0], -((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                        }
                    }
                    newscen = "シナリオ";
                    if ((ScId == 1 || ScId == 3) && (ShownScenarios[1] == true || ShownScenarios[3] == true))
                    {
                        newscen += "1＆3";
                    }
                    foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(newscen))
                    {
                        if (info == "AreaObjInfo" || info == "CameraAreaInfo")
                        {
                            render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[2], ((Single[])obj.Prop["scale"])[1]), ((Single[])obj.Prop["dir"])[0], ((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                        }
                        else
                        {
                            render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[1], ((Single[])obj.Prop["scale"])[2]), ((Single[])obj.Prop["dir"])[0], -((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                        }
                    }
                }

                foreach (LevelObj obj in AllInfos[info].GetbyScenarioX(Scenario))
                {
                    if (info == "AreaObjInfo" || info == "CameraAreaInfo")
                    {
                        render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[2], ((Single[])obj.Prop["scale"])[1]), ((Single[])obj.Prop["dir"])[0], ((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                    }
                    else
                    {
                        render.ChangeTransform(info, AllInfos[info].IndexOf(obj), render.Positions[info][AllInfos[info].IndexOf(obj)], new Vector3D(((Single[])obj.Prop["scale"])[0], ((Single[])obj.Prop["scale"])[1], ((Single[])obj.Prop["scale"])[2]), ((Single[])obj.Prop["dir"])[0], -((Single[])obj.Prop["dir"])[2], ((Single[])obj.Prop["dir"])[1], false);
                    }
                }
            }
        }

        #endregion

        int AreaObjOldSelection = -1;
        int CameraAreaOldSelection = -1;
        int RailOldSel = -1;
        int ObjInfoSel = -1;
        int index = -2;
        private void SelectCorrectProperty()
        {
            List<string> remove;
            List<string> add;

            if (CurrentAllInfosSectionName == "StartInfo")
            {
                remove = new List<string>
                    {
                        "General",
                        "Extra",
                        "DemoExtra",
                        "RailTab",
                        "StartEvent",
                        "Args",
                        "DefArgs"
                    };
                add = new List<string>() { "StartGeneral" };
            }
            else if (CurrentAllInfosSectionName == "AllRailInfos")
            {
                remove = new List<string>
                    {
                        "General",
                        "Extra",
                        "DemoExtra",
                        "StartGeneral",
                        "StartEvent",
                        "Args",
                        "DefArgs"

                    };
                add = new List<string>();
                add.Add("RailTab");
            }
            else if (CurrentAllInfosSectionName == "DemoSceneObjInfo")
            {
                remove = new List<string>
                    {
                        "Extra",
                        "General",
                        "StartGeneral",
                        "StartEvent",
                        "RailTab"
                    };
                add = new List<string>() { "DemoExtra" };
            }
            else if (CurrentAllInfosSectionName == "StartEventObjInfo")
            {
                remove = new List<string>
                    {
                        "Extra",
                        "General",
                        "StartGeneral",
                        "RailTab",
                        "DemoExtra"
                    };
                add = new List<string>() { "StartEvent" };
            }
            else
            {
                remove = new List<string>
                    {
                        "StartGeneral",
                        "DemoExtra",
                        "StartEvent",
                        "RailTab"
                    };
                add = new List<string>() { "General", "Extra" };
            }
            if (NewObjectDatabase != null)
            {
                if (CurrentAllInfosSectionName != "StartInfo" && CurrentAllInfosSectionName != "CameraAreaInfo")
                {
                    if (NewObjectDatabase.DBtoId.ContainsKey(treeView1.SelectedNode.Text))
                    {
                        remove.Insert(0,"GroupArgs");
                        remove.Insert(0, "DefArgs");
                        //remove.Insert(0, "Args");
                        remove.Add("GroupGen");
                        add.Add("Args");
                    }
                    else
                    {
                        remove.Insert(0, "GroupArgs");
                        remove.Insert(0, "Args");
                        remove.Add("GroupGen");
                        add.Add("DefArgs");
                        if (CurrentAllInfosSectionName == "AllRailInfos")
                        {

                            label14.Text = "";
                        }
                        else
                        {
                            label14.Text = "This object's args are undocumented!";
                        }
                    }
                }
                else
                {
                    remove.Insert(0, "GroupArgs");
                    remove.Insert(0, "DefArgs");
                    remove.Insert(0, "Args");
                    remove.Add("GroupGen");
                }
            }
            else
            {
                if (CurrentAllInfosSectionName != "StartInfo" && CurrentAllInfosSectionName != "CameraAreaInfo")
                {
                    remove.Insert(0, "GroupArgs");
                    remove.Insert(0, "Args");
                    remove.Add("GroupGen");
                    add.Add("DefArgs");
                    if (CurrentAllInfosSectionName == "AllRailInfos")
                    {
                        label14.Text = "";
                    }
                    else
                    {
                        label14.Text = "This object's args are undocumented!";
                    }
                }
            }
            if (treeView1.SelectedNodes.Count > 1)
            {
                remove = new List<string>()
                {
                        "Args",
                        "DefArgs",
                        "Extra",
                        "DemoExtra",
                        "General",
                        "StartGeneral",
                        "RailTab"
                };
                add = new List<string>()
                {
                    "GroupGen",
                    "GroupArgs"
                };
            }


            RefreshTabs(remove, add);

        }
        private void ObjectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        private void RefreshTmpChildrenObjects()
        {
            render.ClearTmpObjects();
            if (CurrentAllInfosSelectedIndex == -1) return;
            if (Properties.Settings.Default.ShowChildren && CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("GenerateChildren"))
            {
                AddChildrenModels((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"], false);
            }
            if (Properties.Settings.Default.ShowChildren && CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("AreaChildren"))
            {
                AddChildrenModels((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["AreaChildren"], true);
            }
        }

        private void ObjectsListBox_Doubleclick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (CurrentAllInfosSelectedIndex < 0) return;
            if (ObjectsListBox.SelectedItems.Count > 1) return;
            render.CameraToObj(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex);
        }

        private void bymlXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Byml File|*.byml; *.byaml|*.*|*.*";
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "Xml file|*.xml";
            if (opn.ShowDialog() == DialogResult.OK && sav.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sav.FileName, BymlConverter.GetXml(File.ReadAllBytes(opn.FileName)), DefEnc);
            }
        }

        private void xmlBymlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Xml file|*.xml|*.*|*.*";
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "Byml File|*.byml; *.byaml";
            if (opn.ShowDialog() == DialogResult.OK && sav.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sav.FileName, BymlConverter.GetByml(File.ReadAllText(opn.FileName, DefEnc)));
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            FrmCredits c = new FrmCredits();
            c.ShowDialog();
        }

        private void hotkeysListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(strings["Hotkey"]);
        }

        private void Undo_loading(object sender, EventArgs e)
        {
            UndoMenu.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            int count = 0;
            foreach (UndoAction act in Undo.ToArray().Reverse())
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "Undo" + count.ToString();
                btn.Text = act.ToString();
                btn.Click += UndoListItem_Click;
                btn.MouseEnter += UndoListItem_MouseEnter;
                Items.Add(btn);
                count++;
            }
            UndoMenu.DropDownItems.AddRange(Items.ToArray());
        }

        private void UndoListItem_MouseEnter(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("Undo".Length));
            for (int i = 0; i < UndoMenu.DropDownItems.Count; i++)
            {
                if (i < index) UndoMenu.DropDownItems[i].BackColor = Color.LightBlue;
                else UndoMenu.DropDownItems[i].BackColor = SystemColors.Control;
            }
        }

        private void UndoListItem_Click(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("Undo".Length));
            for (int i = 0; i <= index; i++)
            {
                Undo.Pop().Undo();
            }
            UndoMenu.HideDropDown();
        }

        private void gbatempThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://gbatemp.net/threads/wip-the-fourth-dimension-a-super-mario-3d-land-level-editor.424001/");
        }

        private void Form1_closing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void creatorClassNameTableEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmCCNTEdit f = new FormEditors.FrmCCNTEdit(CreatorClassNameTable, this);
            f.ShowDialog();
            LoadCreatorClassNameTable();
        }

        private void stagesBgmEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string bgm = Properties.Settings.Default.UseLayeredFs ? Properties.Settings.Default.LayeredFSPath + "\\SoundData\\BgmTable.szs" : @"BgmTable.szs";
            if (Properties.Settings.Default.GamePath.Trim() == "" && !File.Exists(bgm))
            {
                MessageBox.Show("You must set the game Romfs path first !");
                return;
            }
            else if (!File.Exists(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs") && !File.Exists(bgm))
            {
                MessageBox.Show(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs not found !\r\nProbably your Romfs dump is incomplete or was modified.");
                return;
            }
            else if (File.Exists(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs") && !File.Exists(bgm))
            {
                if (Properties.Settings.Default.UseLayeredFs && !Directory.Exists(Properties.Settings.Default.LayeredFSPath + "\\SoundData"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.LayeredFSPath + "\\SoundData");
                }
                File.Copy(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs", bgm);
            }
            BgmEditors.FrmBgmMain f = new BgmEditors.FrmBgmMain(LevelNameNum);
            f.Show();
        }

        private void From_Activated(object sender, EventArgs e) //Resume sorting
        {
            render.SetSortFrequency(0.5);
        }

        private void Form_Deactivate(object sender, EventArgs e) //Stop sorting
        {
            render.SetSortFrequency(0);
        }

        private void ListBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ObjectsListBox.ClearSelected();
                SelectInfoIndex (- 1);
                propertyGrid1.SelectedObject = null;
            }
        }

        private void modelImporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormEditors.FrmObjImport().ShowDialog();
        }

        private void objectsDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewObjectDatabase == null)
            {
                LoadNewDatabase();
            }
            Form f = Application.OpenForms["ObjectDbEditor"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            //ObjectDbEditor d = new ObjectDbEditor(ObjectDatabase);
            ObjectDbEditor d = new ObjectDbEditor(NewObjectDatabase.Copy());
            d.CCNT = CreatorClassNameTable;
            d.ShowDialog();
            if (d.NewDatabase != null)
            {
                NewObjectDatabase = d.NewDatabase;
                DBtoXML(NewObjectDatabase);
            }
            if (Properties.Settings.Default.ExperimentalFeatures) MessageBox.Show("CreatorClassNameTable "+NewObjectDatabase.docpercent(CreatorClassNameTable).ToString("G2", new CultureInfo("en-us")) + "% documented.");
            //get db from form
            //LoadObjectDatabase();*/
            LoadNewDatabase();
        }

        private void DBtoXML(NewDb newObjectDatabase)
        {
            // make database for each entry 
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = DefEnc }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("database");
                    xr.WriteAttributeString("version", newObjectDatabase.version.ToString("F1", new CultureInfo("en-us")));
                    xr.WriteStartElement("categories");

                    foreach (int i in newObjectDatabase.Categories.Keys)
                    {
                        xr.WriteStartElement("category");
                        xr.WriteAttributeString("id", i.ToString());
                        xr.WriteAttributeString("name", newObjectDatabase.Categories[i].ToString());
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();

                    xr.WriteStartElement("types");

                    foreach (int i in newObjectDatabase.Types.Keys)
                    {
                        xr.WriteStartElement("type");
                        xr.WriteAttributeString("id", i.ToString());
                        xr.WriteAttributeString("name", newObjectDatabase.Types[i].ToString());
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();
                    xr.WriteStartElement("objects");

                    foreach (NewDb.NewDbEntry entry in newObjectDatabase.Entries.Values)
                    {
                        xr.WriteStartElement("object");
                        xr.WriteAttributeString("id", entry.filename);
                        xr.WriteAttributeString("name", entry.dbname);
                        xr.WriteAttributeString("category", entry.category.ToString());
                        xr.WriteAttributeString("type", entry.type.ToString());
                        xr.WriteAttributeString("extra", entry.extra);
                        if(entry.modelname != "")
                        {
                            xr.WriteStartElement("model");
                            xr.WriteValue(entry.modelname);
                            xr.WriteEndElement();
                        }
                        for(int i = 0; i< entry.args.Count; i++)
                        {
                            xr.WriteStartElement(entry.args[i].type);
                            xr.WriteAttributeString("arg", entry.args[i].arg_id.ToString());
                            xr.WriteAttributeString("name", entry.args[i].name);
                            switch (entry.args[i].type)
                            {
                                case "bool":

                                    xr.WriteAttributeString("default", entry.args[i].default_value);
                                    xr.WriteAttributeString("info", entry.args[i].info);
                                    break;
                                case "int":
                                    if (entry.args[i].max != entry.args[i].min)
                                    {
                                        xr.WriteAttributeString("max", entry.args[i].max.ToString());
                                        xr.WriteAttributeString("min", entry.args[i].min.ToString());
                                    }
                                    xr.WriteAttributeString("default", entry.args[i].default_value);
                                    xr.WriteAttributeString("info", entry.args[i].info);
                                    break;
                                case "option":
                                    xr.WriteAttributeString("default", entry.args[i].default_value);
                                    xr.WriteAttributeString("info", entry.args[i].info);
                                    foreach (string s in entry.args[i].options.Keys)
                                    {

                                        xr.WriteStartElement("op");

                                        xr.WriteAttributeString("name", s);
                                        xr.WriteAttributeString("value", entry.args[i].options[s]);
                                        xr.WriteEndElement();
                                    }
                                    break;
                            }
                            xr.WriteEndElement();
                        }
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();

                    xr.WriteEndElement();
                    xr.Close();
                }
                if (File.Exists(@"newobjdb.xml.bak"))
                {
                    File.Delete(@"newobjdb.xml.bak");
                }
                File.Copy(@"newobjdb.xml", @"newobjdb.xml.bak");
                File.WriteAllText("newobjdb.xml", DefEnc.GetString(stream.ToArray()));
            }
            return;
        }

        private void generate2DSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AllInfos.ContainsKey("ObjInfo"))
            {
                AllInfos.Add("ObjInfo", new AllInfoSection());
                highestID.Add("ObjInfo", 0);
                render.AddKey("ObjInfo");

            }
            Form f = Application.OpenForms["Frm2DSection"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            Frm2DSection d = new Frm2DSection();
            d.Show();
        }

        private void lblDescription_Click(object sender, EventArgs e)
        {
            //if (lblDescription.Tag.ToString() != "-1") new ObjectDB.ObjectDBView(ObjectDatabase.Entries[ObjectsListBox.SelectedItem.ToString()], ObjectsListBox.SelectedItem.ToString()).Show();
        }

        private void oggToBcstmConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BgmEditors.FrmMakeBcstm().ShowDialog();
        }

        private void downloadLatestObjectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("objectdb.xml"))
            {
                if (MessageBox.Show("An object database file already exists, if you download a new one it will be replaced, if you edited it your changes will be lost, do you want to continue ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No) return;
                if (File.Exists("objectdb.xml.bak")) File.Delete("objectdb.xml.bak");
                File.Copy("objectdb.xml", "objectdb.xml.bak");
                File.Delete("objectdb.xml");
            }
            try
            {
                new WebClient().DownloadFile(Properties.Settings.Default.DownloadDbLink == "" ? ObjectDbLink : Properties.Settings.Default.DownloadDbLink, "objectdb.xml");
                //LoadObjectDatabase();
                LoadNewDatabase();
                File.Delete("objectdb.xml.bak");
                MessageBox.Show("Done!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error, the file wasn't downloaded: \r\n" + ex.Message);
                if (File.Exists("objectdb.xml.bak"))
                {
                    if (File.Exists("objectdb.xml")) File.Delete("objectdb.xml");
                    File.Copy("objectdb.xml.bak", "objectdb.xml");
                    File.Delete("objectdb.xml.bak");
                    MessageBox.Show("The backup was restored");
                }
                return;
            }
        }

        private void generatePreloadFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SzsFiles == null)
            {
                MessageBox.Show("Szs not loaded?");
                return;
            }
            if (!SzsFiles.ContainsKey("PreLoadFileList1.byml"))
            {
                SzsFiles.Add("PreLoadFileList1.byml", new byte[] { 0x00 });
            }
            string PreLoadFileList = Properties.Resources.GenericPreloadList;
            string ObjsList = "";
            List<string> ProcessedItems = new List<string>();
            foreach (LevelObj o in AllInfos["ObjInfo"])
            {
                if (ProcessedItems.Contains(o.GetName())) continue;//internal name
                ProcessedItems.Add(o.GetName());//internal name
                if (NewObjectDatabase.Entries.ContainsKey(o.GetName()) && NewObjectDatabase.Entries[o.GetName()].filename != "")//internal name
                {
                    foreach (string s in NewObjectDatabase.Entries[o.GetName()].children)//internal name
                    {
                            ObjsList += "<C1>\r\n<A0 Name=\"Path\" StringValue=\"" + "/ObjectData" + s + ".szs" + "\" />\r\n<A0 Name=\"Type\" StringValue=\"Archive\" />\r\n</C1>";
                    }
                }
            }
            PreLoadFileList = PreLoadFileList.Insert(252, ObjsList);
            if (SzsFiles.ContainsKey("PreLoadFileList2.byml"))
            {
                MessageBox.Show("This SZS contains more PreLoadFileList files, the result of the generation will be put in your clipboard as xml, open the file you want to replace and paste replacing everything, then click save (the layout of the text doesn't matter)");
                Clipboard.SetText(PreLoadFileList);
            }
            else
            {
                SzsFiles["PreLoadFileList1.byml"] = BymlConverter.GetByml(PreLoadFileList);
                MessageBox.Show("Done");
            }
        }
        #endregion

        #region LevelEditing
        bool IsEditingC0List = false;
        Stack<List<LevelObj>> C0ListEditingStack = new Stack<List<LevelObj>>();
        Stack<int> SelectionIndex = new Stack<int>();
        int InitialAllInfosSection = -1;
        List<LevelObj> dummyList = new List<LevelObj>();
        public List<LevelObj> CurrentAllInfosSection
        {
            get
            {
                if (IsEditingC0List) return C0ListEditingStack.Peek();
                else
                {
                    if (treeView1.SelectedNodes.Count > 1)
                    {
                    }
                    if (treeView1.SelectedNode.Parent == null)
                    {
                        return AllInfos[treeView1.SelectedNode.Name];
                    }
                    else
                    {
                        if ((string)treeView1.SelectedNode.Parent.Tag == "Rail") return dummyList;
                        if ((string)treeView1.SelectedNode.Parent.Name == "AllRailInfos") return dummyList;
                        return AllInfos[treeView1.SelectedNode.Parent.Name];
                    }
                }
            }
        }

        List<LevelObj> CurrentAllInfosSelection = new List<LevelObj>();
        List<LevelObj> AllinfosSelection0 = new List<LevelObj>();
        public string CurrentAllInfosSectionName
        {
            get
            {
                if (IsEditingC0List) return "C0EditingListObjs";
                else
                {
                    if (treeView1.SelectedNode == null)
                    {
                        return null;
                    }
                    else if (treeView1.SelectedNode.Parent == null)
                    {
                        return treeView1.SelectedNode.Name;
                    }
                    else
                    {
                        if ((string)treeView1.SelectedNode.Parent.Tag == "Rail") return "AllRailInfos";
                        return treeView1.SelectedNode.Parent.Name;
                    }
                    return "";
                }
            }
        }

        public List<LevelObj> GetListByName(string name)
        {
            if (name == "C0EditingListObjs" || IsEditingC0List) return C0ListEditingStack.Peek();
            else return AllInfos[name];
        }

        void EditC0List(C0List list, bool addToStack = true) { if (list != null) EditC0List(list.List, addToStack); }
        public void EditC0List(List<LevelObj> list, bool addToStack = true)
        {
/*            if (addToStack && ObjectsListBox.SelectedItems.Count > 1)
            {
                MessageBox.Show("You can't edit more C0lists at once, edit only on one object then copy the children objects to the others");
                return;
            }
            fileToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            comboBox1.Text = "C0EditingListObjs";
            C0EditingPanel.Visible = true;
            IsEditingC0List = true;
            if (addToStack) {
                if (C0ListEditingStack.Count == 0) InitialAllInfosSection = comboBox1.SelectedIndex;
                C0ListEditingStack.Push(list);
                SelectionIndex.Push(CurrentAllInfosSelectedIndex);
            }
            ObjectsListBox.SelectedItems.Clear();
            render.ClearC0Objects();
            LoadModels(list, "C0EditingListObjs");
            ObjectsListBox.Items.Clear();
            if (!higestID.ContainsKey("C0EditingListObjs")) higestID.Add("C0EditingListObjs", 0); else higestID["C0EditingListObjs"] = 0;
            foreach (LevelObj o in list) if (o.Prop.ContainsKey("l_id")) if (Int32.Parse(((Node)o.Prop["l_id"]).StringValue) > higestID["C0EditingListObjs"]) higestID["C0EditingListObjs"] = Int32.Parse(((Node)o.Prop["l_id"]).StringValue);
            foreach (LevelObj o in list)
            {
                ObjectsListBox.Items.Add(o.GetName(true));
            }
            //ObjectsListBox.Items.AddRange(list.ToArray());
            propertyGrid1.SelectedObject = null;*/
        }

        private void C0ListEditorGoBack()
        {
            if (C0ListEditingStack.Count == 1)
            {
                render.ClearC0Objects();
                propertyGrid1.SelectedObject = null;
                IsEditingC0List = false;
                comboBox1.SelectedIndex = InitialAllInfosSection;
                comboBox1_SelectedIndexChanged(null,null);
                C0ListEditingStack.Pop();
                C0EditingPanel.Visible = false;
            }
            else
            {
                C0ListEditingStack.Pop();
                EditC0List(C0ListEditingStack.Peek(), false);
            }
            //Panell1.ClearObjs();
            SelectInfoIndex(SelectionIndex.Pop());
            fileToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            C0ListEditorGoBack();
        }

        public void SetComboboxToLevObjList(List<LevelObj> list)
        {
            for (int i = 0; i < AllInfos.Keys.Count; i++) if (AllInfos.Values.ToArray()[i] == list) comboBox1.SelectedIndex = i;
        }

        private void CameraCode_click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName().Contains("Fog"))//internal name
            {
                int fogid = ((int)((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])[0]);
                //int FogScenario = 0;


               /* if (((Node)(CurrentAllInfosSection[CurrentInfosSelectedIndex]).Prop["LayerName"]).StringValue.Contains("シナリオ"))
                {
                   if (((Node)(CurrentAllInfosSection[CurrentInfosSelectedIndex]).Prop["LayerName"]).StringValue.Substring("シナリオ".Length).Length == 1)
                   {
                        FogScenario = int.Parse(((Node)(CurrentAllInfosSection[CurrentInfosSelectedIndex]).Prop["LayerName"]).StringValue.Substring("シナリオ".Length));
                   }
                }*/ // wrong, it doesn't *necessarily* use the 

                if (fogid < 0)
                {
                    MessageBox.Show("The FogArea Arg[0] can't be less than 0!");
                    return;
                }
                if (SzsFiles == null)
                {
                    MessageBox.Show("To use this function you must load a level from an szs file");
                    return;
                }
                string TmpFogParam = Properties.Resources.GenericFogParam;
                string ParamFile = "";
                if (!SzsFiles.ContainsKey("FogParam.byml"))//if it doesnt' contain a fogparam file it tries to make a fogparam for the scenario, if the scenario is common then it creates the default file 
                {
                    ParamFile = "FogParam" + (Scenario > 0 ? Scenario.ToString() : "") + ".byml";
                    //"FogParam" + (FogScenario > 0 ? FogScenario.ToString() : "") + ".byml";
                }
                else
                {
                    ParamFile = "FogParam.byml";
                }
                if (!SzsFiles.ContainsKey(ParamFile))
                {
                    if (MessageBox.Show("This level doesn't contain the FogParam file, do you want to generate a specific one for this scenario? (otherwise a generic one will be generated)", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        ParamFile = "FogParam.byml";
                    }

                    SzsFiles.Add(ParamFile, BymlConverter.GetByml(TmpFogParam));
                    FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(TmpFogParam, fogid, Scenario, true);
                    f.ShowDialog();
                    SzsFiles[ParamFile] = f.fogparamfilenew.ToArray();
                    OtherLevelDataMenu.DropDownItems.Clear();
                    for (int i = 0; i < SzsFiles.Keys.Count; i++)
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = "LoadFile" + i.ToString();
                        btn.Text = SzsFiles.Keys.ToArray()[i];
                        btn.Click += LoadFileList_click;
                        OtherLevelDataMenu.DropDownItems.Add(btn);
                    }
                SaveChangeLabel();
                }
                else 
                {
                    string FogParam = BymlConverter.GetXml(SzsFiles[ParamFile]);
                    if (!FogParam.Contains("<C0 Name=\"FogAreas\">"))
                    {
                        DialogResult r = MessageBox.Show("The FogParam from this szs can't be used, do you want to generate a new FogParam ?", ParamFile, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(TmpFogParam, fogid, Scenario, true);
                            f.ShowDialog();
                            SzsFiles[ParamFile] = f.fogparamfilenew.ToArray();
                            SaveChangeLabel();
                        }
                        else return;
                    }
                    else
                    {
                        if (FogParam.Contains("<D1 Name=\"Area Id\" StringValue=\"" + fogid.ToString() + "\" />"))// Add Fog editor loading the fog properties instead of  creating a new one
                        { //edits already existing area
                            /*
                            FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(SzsFiles[ParamFile]), ParamFile, false, FogParam.IndexOf("<D1 Name=\"Area Id\" StringValue=\"" + fogid.ToString() + "\" />"));
                            frm.ShowDialog();
                            if (frm.XmlRes != null) SzsFiles[ParamFile] = BymlConverter.GetByml(frm.XmlRes);
                            */
                            FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(FogParam, fogid, Scenario, FogParam.IndexOf("<D1 Name=\"Area Id\" StringValue=\"" + fogid.ToString() + "\" />"));
                            f.ShowDialog();
                            if (f.fogparamfilenew != null) SzsFiles[ParamFile] = f.fogparamfilenew.ToArray();
                            SaveChangeLabel();
                        }
                        else
                        { //adds the area to the file
                            FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(FogParam, fogid, Scenario);
                            f.ShowDialog();
                            if (f.fogparamfilenew != null) SzsFiles[ParamFile] = f.fogparamfilenew.ToArray();
                            SaveChangeLabel();
                        }
                    }
                }
            }
            else if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("CameraId"))//internal name
            {

            }
        }

        private void AddType_click(object sender, EventArgs e) //Addtype
        {
            FormEditors.FrmStringInput f = new FormEditors.FrmStringInput();
            f.ShowDialog();
            string[] internalNames = new string[] {"C0EditingListObjs", "TmpChildrenObjs", "SelectedRail", "TmpAreaChildrenObjs" };
            if (f.Result == null) return;
            else if (f.Result.Trim() == "") return;
            else if (AllInfos.ContainsKey(f.Result)) MessageBox.Show("This type is already in use");
            else if (internalNames.Contains(f.Result)) MessageBox.Show("This type name is reserved");
            else
            {
                comboBox1.Items.Add(f.Result);
                render.AddKey(f.Result);
                AllInfos.Add(f.Result, new AllInfoSection());
                highestID.Add(f.Result, 0);
            }
        }

        bool warningShow = false;
        public void UpdateSelectedRailView()
        {
            UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
        }

        public void RailPointsChanged(List<Rail.Point> OldPoints)
        {
            UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
            Action<object[]> act;
            act = (object[] args ) =>
            {
                int id = (int)args[0];
                AllRailInfos[id].Points = (List<Rail.Point>)args[1];
                propertyGrid1.Refresh();
                UpdateRailpos(id, AllRailInfos[id].GetPointArray());
            };
            Undo.Push(new UndoAction("Changed points of rail: " + AllRailInfos[CurrentAllInfosSelectedIndex].ToString(), new object[] { CurrentAllInfosSelectedIndex, OldPoints }, act));

SaveChangeLabel();


        }

        public int GetObjectGenChidCount(string type, int index)
        {
            if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("GenerateChildren")) return 0;
            else return ((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List.Count;
        }

        private void UpdateOBJPos(ListBox.SelectedIndexCollection selectedIndices, List<LevelObj> Source, string Type)
        {
            foreach (int i in selectedIndices) UpdateOBJPos(i, Source, Type);
        }

        public void UpdateOBJPos(int id, List<LevelObj> Source, string Type, bool isUndo = false)
        {
            if (Type == "AllRailInfos")
            {
                return;
            }
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            if (Type == "TmpChildrenObjs")
            {
                Source = ((C0List)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["GenerateChildren"]).List;
            }
           // xyz = ads[1]
            Single X, Y, Z, ScaleX, ScaleY, ScaleZ, RotX, RotY, RotZ, A, B, C;
            Single[] scale_XDD;
            Single[] rotation;
            Single[] position;
            if (Source[id].Prop.ContainsKey("pos") == true)
            { position = (Single[])Source[id].Prop["pos"]; }
            else
            {
                if (Source[id].Prop["pos_x"] != null)
                {
                    A = Single.Parse(((Node)Source[id].Prop["pos_x"]).StringValue);
                    B = Single.Parse(((Node)Source[id].Prop["pos_y"]).StringValue);
                    C = Single.Parse(((Node)Source[id].Prop["pos_z"]).StringValue);
                    position = new Single[3] { A, B, C };

                    Source[id].Prop.Add("pos", position);
                    Source[id].Prop.Remove("pos_x");
                    Source[id].Prop.Remove("pos_y");
                    Source[id].Prop.Remove("pos_z");
                }
                else
                {
                    position = new Single[3] { 1, 1, 1 };
                }
            }
            X = position[0];
            Y = position[1];
            Z = position[2];
            if (Source[id].Prop.ContainsKey("scale") == true)
            { 
                scale_XDD = (Single[])Source[id].Prop["scale"];

                if (Source[id].Prop.ContainsKey("scale_x") == true && Source[id].Prop["scale_x"] != null)
                {
                    Source[id].Prop.Remove("scale_x");
                    Source[id].Prop.Remove("scale_y");
                    Source[id].Prop.Remove("scale_z");
                }
            }
            else 
            {
                if (Source[id].Prop["scale_x"] != null) 
                {
                    A = Single.Parse(((Node)Source[id].Prop["scale_x"]).StringValue);
                    B = Single.Parse(((Node)Source[id].Prop["scale_y"]).StringValue);
                    C = Single.Parse(((Node)Source[id].Prop["scale_z"]).StringValue);
                    scale_XDD = new Single[3] { A,B,C };
                    Source[id].Prop.Add("scale", scale_XDD);
                    Source[id].Prop.Remove("scale_x");
                    Source[id].Prop.Remove("scale_y");
                    Source[id].Prop.Remove("scale_z");
                }
                else
                {
                    scale_XDD = new Single[3] { 1, 1, 1 };
                    Source[id].Prop.Add("scale", scale_XDD);
                }
            }
            ScaleX = scale_XDD[0];
            ScaleY = scale_XDD[1];
            ScaleZ = scale_XDD[2];//Single.Parse(((Node)Source[id].Prop["scale_z"]).StringValue);
            if (Source[id].Prop.ContainsKey("dir") == true)
            {
                rotation = (Single[])Source[id].Prop["dir"];

                if (Source[id].Prop.ContainsKey("dir_x") == true && Source[id].Prop["dir_x"] != null)
                {
                    Source[id].Prop.Remove("dir_x");
                    Source[id].Prop.Remove("dir_y");
                    Source[id].Prop.Remove("dir_z");
                }
            }
            else
            {
                if (Source[id].Prop["dir_x"] != null)
                {
                    A = Single.Parse(((Node)Source[id].Prop["dir_x"]).StringValue);
                    B = Single.Parse(((Node)Source[id].Prop["dir_y"]).StringValue);
                    C = Single.Parse(((Node)Source[id].Prop["dir_z"]).StringValue);
                    rotation = new Single[3] { A, B, C };
                    Source[id].Prop.Add("dir", rotation);
                    Source[id].Prop.Remove("dir_x");
                    Source[id].Prop.Remove("dir_y");
                    Source[id].Prop.Remove("dir_z");
                }
                else
                {
                    rotation = new Single[3] { 1, 1, 1 };
                    Source[id].Prop.Add("dir", rotation);
                }
            }
            RotX = rotation[0];
            RotY = rotation[1];
            RotZ = rotation[2];
            render.ChangeTransform(Type, id, new Vector3D(X, -Z, Y), new Vector3D(ScaleX, ScaleZ, ScaleY), RotX, -RotZ, RotY , (Type != "AreaObjInfo" && Type != "CameraAreaInfo" && !isUndo));
            //ScaleXd = Single.Parse(((Node)Source[id].array(prop)Prop(0).["X"]).StringValue);
        }

        private void button3_Click(object sender, EventArgs e) //Remove objects
        {
            DelSelectedObj();
        }

        void DelSelectedObj(bool NoUndo = false)
        {
            if (CurrentAllInfosSelectedIndex == -1) return;
            if (CurrentAllInfosSectionName == "AllRailInfos")
            {
                if (!NoUndo)
                {
                    Rail tmp = AllRailInfos[CurrentAllInfosSelectedIndex].Clone();
                    Action<object[]> action;
                    action = (object[] args) =>
                    {
                        int at = (int)args[0];
                        AddRail((Rail)args[1], at, true);
                    };
                    Undo.Push(new UndoAction("Removed rail: " + tmp.ToString(), new object[] { CurrentAllInfosSelectedIndex, tmp }, action));

                SaveChangeLabel();

                }
                render.RemoveModel(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex);
                AllRailInfos.RemoveAt(CurrentAllInfosSelectedIndex);
                treeView1.Nodes[CurrentAllInfosSectionName].Nodes.RemoveAt(CurrentAllInfosSelectedIndex);
            }
            else
            {
                int[] indexes = GetSelectedIndexes();
                if (!NoUndo)
                {
                    List<LevelObj> tmp = new List<LevelObj>();
                    for (int i = 0; i < indexes.Length; i++) tmp.Add(CurrentAllInfosSection[indexes[i]]);
                    Action<object[]> action;
                    action = (object[] args) =>
                    {
                        List<LevelObj> type = (List<LevelObj>)args[0];
                        int[] at = (int[])args[1];
                        LevelObj[] t = ((LevelObj[])args[2]).Reverse().ToArray();
                        int[] index = at.Reverse().ToArray();

                        for (int i = 0; i < t.Length; i++) { if (i != t.Length - 1){ selecting = true; } AddObj(t[i], type, (string)args[3], false, index[i], type.GetHashCode()); selecting = false; }
                        RefreshTmpChildrenObjects();
                    };
                    string name = (indexes.Length == 1) ? "Removed object: " + tmp[0].GetName(true)/*db name*/ : "Removed " + indexes.Length.ToString() + " objects";
                    Undo.Push(new UndoAction(name, new object[] {CurrentAllInfosSection, indexes, tmp.ToArray(), CurrentAllInfosSectionName }, action));
                    SaveChangeLabel();
                }
                if (indexes.Length > 1)
                {
                    string oldinfos = CurrentAllInfosSectionName;
                    foreach (int i in indexes)
                    {
                        if (i != indexes[indexes.Length - 1]) selecting = true;
                        render.RemoveModel(oldinfos, i);
                        AllInfos[oldinfos].RemoveAt(i);
                        treeView1.Nodes[oldinfos].Nodes.RemoveAt(i);
                        selecting = false;
                    }
                    if (indexes.Min() > 0) SelectInfoIndex(oldinfos, indexes.Min() - 1);
                }
                else
                {
                    if (indexes.Min() > 0 && indexes.Min()< treeView1.Nodes[CurrentAllInfosSectionName].Nodes.Count)
                    {
                        string tempinfos = CurrentAllInfosSectionName;
                        selecting = true;
                        render.RemoveModel(CurrentAllInfosSectionName, indexes[0]);
                        CurrentAllInfosSection.RemoveAt(indexes[0]);
                        treeView1.Nodes[CurrentAllInfosSectionName].Nodes.RemoveAt(indexes[0]);
                        selecting = false;
                        SelectInfoIndex(tempinfos, indexes.Min() - 1);
                    }
                    else
                    {
                        string tempinfos = CurrentAllInfosSectionName;
                        render.RemoveModel(CurrentAllInfosSectionName, indexes[0]);
                        CurrentAllInfosSection.RemoveAt(indexes[0]);
                        treeView1.Nodes[CurrentAllInfosSectionName].Nodes.RemoveAt(indexes[0]);
                        SelectInfoIndex(tempinfos, -1);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//Duplicating objects
        {
            if (CurrentAllInfosSelectedIndex < 1) return;
            if (CurrentAllInfosSectionName == "AllRailInfos")
            {
                Rail tmp = new Rail();
                tmp = AllRailInfos[CurrentAllInfosSelectedIndex].Clone();
                AddRail(tmp);
            }
            else
            {
                if (/*ObjectsListBox.SelectedIndices.Count*/ 1 == 1)
                {
                    selecting = true;
                    AddObj(CurrentAllInfosSection[CurrentAllInfosSelectedIndex], CurrentAllInfosSection, CurrentAllInfosSectionName);
                    selecting = false;
                    SelectInfoIndex(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex);
                    RefreshProperties();
                    //ObjectsListBox.SetSelected(ObjectsListBox.Items.Count - 1,true);
                }
                else
                {
                    for (int i = 0; i < CurrentAllInfosSelection.Count; i++)
                    {
                        if (i+1 != CurrentAllInfosSelection.Count) selecting = true;
                        AddObj(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])], CurrentAllInfosSection, CurrentAllInfosSectionName);
                        selecting = false;
                    }
                }
            }
        }

        void AddRail(Rail r, int at = -1, bool IsUndo = false, int l_id = -1)
        {
            highestID["AllRailInfos"]++;
            if (l_id == -1)
            {
                r.l_id = highestID["AllRailInfos"];
            }
            else
            {
                r.l_id = l_id;
            }
            LoadRailsModels(r, at);
            if (at == -1) AllRailInfos.Add(r); else AllRailInfos.Insert(at, r);

            //if (at == -1) ObjectsListBox.Items.Add(r.ToString()); else ObjectsListBox.Items.Insert(at, r.ToString());
            if (at == -1) treeView1.Nodes["AllRailInfos"].Nodes.Add(r.ToString()); else treeView1.Nodes["AllRailInfos"].Nodes.Insert(at, r.ToString());
            //treeView1.Nodes["AllRailInfos"].Nodes[treeView1.Nodes["AllRailInfos"].Nodes.Count - 1];
            //ObjectsListBox.SetSelected(at == -1 ? ObjectsListBox.Items.Count - 1 : at, true);
            if (!IsUndo)
            {
                Action<object[]> action;
                action = (object[] args) =>
                {
                    AllRailInfos.RemoveAt((int)args[0]);
                    treeView1.Nodes["AllRailInfos"].Nodes.RemoveAt((int)args[0]);
                };
                Undo.Push(new UndoAction("Added rail: " + r.ToString(), new object[] {treeView1.Nodes["AllRailInfos"].Nodes.Count - 1 }, action));

            SaveChangeLabel();

            }
            SelectInfoIndex("AllRailInfos", treeView1.Nodes["AllRailInfos"].Nodes.Count - 1);
        }

        void AddObj(LevelObj inobj, List<LevelObj> list, string name, bool clone = true, int at = -1, int UndoHash = -1)
        {
            if (!highestID.ContainsKey(name)) highestID.Add(name, 0);
            highestID[name]++;
            LevelObj obj = new LevelObj();
            if (clone) obj = inobj.Clone(); else obj = inobj;
            if (obj.Prop.ContainsKey("l_id")) 
            {
                if (AllInfos[name].GetById(int.Parse(((Node)obj.Prop["l_id"]).StringValue)) != -1)
                {
                    while (AllInfos[name].GetById(highestID[name]) != -1)
                    {
                        highestID[name]++;
                    }
                    obj.Prop["l_id"] = new Node(highestID[name].ToString(), "D1");
                }
            }
            if (inobj.GetName()/*internal name*/ == "CameraArea") obj.Prop["CameraId"] = new Node(highestID[name].ToString(), "D1");
            if (at == -1) list.Add(obj); else list.Insert(at, obj);
            if (UndoHash == -1 || CurrentAllInfosSection.GetHashCode() == UndoHash)
            {
                if (!obj.Prop.ContainsKey("dbname"))
                {
                    if (NewObjectDatabase!= null && NewObjectDatabase.IdtoDB.ContainsKey(obj.ToString()))
                    {
                        obj.Prop.Add("dbname", NewObjectDatabase.IdtoDB[obj.ToString()]);
                    }

                }
                //if (at == -1) ObjectsListBox.Items.Add(obj.GetName(true))/*db name*/; else ObjectsListBox.Items.Insert(at, obj.GetName(true))/*db name*/;
                if (at == -1) treeView1.Nodes[name].Nodes.Add(obj.GetName(true))/*db name*/; else treeView1.Nodes[name].Nodes.Insert(at, obj.GetName(true))/*db name*/;
                List<LevelObj> tmp = new List<LevelObj>();
                tmp.Add(obj);
                if (name == "AreaObjInfo") LoadModels(tmp, name, "models\\UnkArea.obj", at);
                else if (name == "CameraAreaInfo") LoadModels(tmp, name, "models\\UnkGreen.obj", at);
                else LoadModels(tmp, name, "models\\UnkBlue.obj", at);
                SelectInfoIndex(treeView1.Nodes[name].Nodes.Count - 1);
                //ObjectsListBox.ClearSelected();
                //ObjectsListBox.SetSelected(at == -1 ? ObjectsListBox.Items.Count - 1 : at, true);
            }
            if (UndoHash == -1)
            {
                Action<object[]> action;
                action = (object[] args) =>
                {
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    int index = (int)args[1];
                    string typename = (string)args[2];
                    type.RemoveAt(index);
                    if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode())
                    {
                        render.RemoveModel(typename, index);
                        //ObjectsListBox.Items.RemoveAt(index);
                        treeView1.Nodes[typename].Nodes[index].Remove();
                    }
                    render.ClearTmpObjects();
                };
                Undo.Push(new UndoAction("Added object: " + obj.GetName(true)/*db name*/, new object[] { CurrentAllInfosSection, treeView1.Nodes[CurrentAllInfosSectionName].Nodes.Count - 1, CurrentAllInfosSectionName }, action));

            SaveChangeLabel();

            }
        }  

        private void BtnAddObj_Click(object sender, EventArgs e)//Add new object
        {
            Vector3D pos = AddObjectOrigin ? new Vector3D(0,0,0) : render.GetPositionInView();
            if ((string)Btn_AddObj.Tag == "AllRailInfos")
            {
                AddRail(new Rail(true, pos));
                if (AutoMoveCam) render.LookAt(pos);
            }else if ((string)Btn_AddObj.Tag == "Rail")
            {
                Rail tmp = AllRailInfos[treeView1.Nodes["AllRailInfos"].Nodes.IndexOf(treeView1.SelectedNode.Parent)];
                tmp.Points.Add(tmp.Points[tmp.Points.Count - 1].Clone_increment());
                UpdateRailpos(treeView1.Nodes["AllRailInfos"].Nodes.IndexOf(treeView1.SelectedNode.Parent), AllRailInfos[treeView1.Nodes["AllRailInfos"].Nodes.IndexOf(treeView1.SelectedNode.Parent)].GetPointArray());
                render.SelectRail(AllRailInfos[treeView1.Nodes["AllRailInfos"].Nodes.IndexOf(treeView1.SelectedNode.Parent)].GetPointArray());
                treeView1.SelectedNode.Parent.Nodes.Add("Rail Point");
            }
            else
            {

                FrmAddObj frm = new FrmAddObj(CreatorClassNameTable.Keys.ToArray(), NewObjectDatabase, (string)Btn_AddObj.Tag, pos );
                frm.ShowDialog();
                if (frm.Value == null) return;
                if (!AllInfos.ContainsKey(CurrentAllInfosSectionName))
                {
                    comboBox1.Items.Add(CurrentAllInfosSectionName);
                    render.AddKey(CurrentAllInfosSectionName);
                    AllInfos.Add(CurrentAllInfosSectionName, new AllInfoSection());
                    highestID.Add(CurrentAllInfosSectionName, 0);
                }
                AddObj(frm.Value,  CurrentAllInfosSection, CurrentAllInfosSectionName);
                if (AutoMoveCam) render.LookAt(pos);
            }
        }

        private void pasteValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSelectedIndex < 0)
            {
                if (clipboard[clipboard.Count - 1].Type != ClipBoardItem.ClipboardType.FullObject) return;
            }
            if (CurrentAllInfosSectionName == "AllRailInfos" && !(clipboard[clipboard.Count - 1].Type == ClipBoardItem.ClipboardType.Rail || clipboard[clipboard.Count - 1].Type == ClipBoardItem.ClipboardType.IntArray)) return;
            PasteValue(CurrentAllInfosSelectedIndex, CurrentAllInfosSection,CurrentAllInfosSectionName, clipboard[clipboard.Count - 1]);
            ClipBoardMenu.Close();
        }

        private void copyPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSectionName == "AllRailInfos") return;
            if (CurrentAllInfosSelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "pos");
        }

        private void copyRotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSectionName == "AllRailInfos") return;
            if (CurrentAllInfosSelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "dir");
        }

        private void copyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSectionName == "AllRailInfos") return;
            if (CurrentAllInfosSelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "scale");
        }

        private void ClipBoardMenu_CopyArgs_Click(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "AllRailInfos") return;
            if (CurrentAllInfosSelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "Arg");
        }

        private void ClipBoardMenu_CopyFull_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSelectedIndex < 0) return;
            int[] indexes = GetSelectedIndexes();
            if (indexes.Length == 1) CopyValue(indexes, CurrentAllInfosSectionName, "Full");
            else CopyValue(indexes, CurrentAllInfosSectionName, "FullArray");
        }

        private void Btn_CopyObjs_Click(object sender, EventArgs e)
        {
            int[] indexes = GetSelectedIndexes();
            if (indexes.Length > 1) CopyValue(indexes, CurrentAllInfosSectionName, "FullArray");
        }


        private void ClipBoardMenu_CopyRail_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "Rail");
        }

        void CopyValue(int[] indexes, string type, string value)
        {
            ClipBoardItem cl = new ClipBoardItem();
            int index = indexes[0];
            /*if (value == "pos_" || value == "dir_")
            {
                if (value == "pos_") cl.Type = ClipBoardItem.ClipboardType.Position;
                else if (value == "dir_") cl.Type = ClipBoardItem.ClipboardType.Rotation;
                else cl.Type = ClipBoardItem.ClipboardType.Scale;
                if (GetListByName(type)[index].Prop.ContainsKey(value + "x") && GetListByName(type)[index].Prop.ContainsKey(value + "y") && GetListByName(type)[index].Prop.ContainsKey(value + "z"))
                {
                    cl.X = Single.Parse(((Node)GetListByName(type)[index].Prop[value + "x"]).StringValue);
                    cl.Y = Single.Parse(((Node)GetListByName(type)[index].Prop[value + "y"]).StringValue);
                    cl.Z = Single.Parse(((Node)GetListByName(type)[index].Prop[value + "z"]).StringValue);
                }
                else MessageBox.Show("You can't copy this value from this object");
            }*/
            if (value == "Arg")
            {
                if (type == "AllRailInfos")
                {
                    cl.Type = ClipBoardItem.ClipboardType.IntArray;
                    cl.Args = AllRailInfos[index].Arg.ToArray();
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.IntArray;
                    if (GetListByName(type)[index].Prop.ContainsKey("Arg"))
                    {
                        cl.Args = (int[])((int[])GetListByName(type)[index].Prop["Arg"]).Clone(); //This looks strange but (int[])GetListByName(type)[index].Prop["Arg"] doesn't work
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "scale")
            {
                if (type == "AllRailInfos")
                {
                    return;
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.Scale;
                    if (GetListByName(type)[index].Prop.ContainsKey("scale"))
                    {
                        cl.Scale = (Single[])((Single[])GetListByName(type)[index].Prop["scale"]).Clone();
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "dir")
            {
                if (type == "AllRailInfos")
                {
                    return;
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.Rotation;
                    if (GetListByName(type)[index].Prop.ContainsKey("dir"))
                    {
                        cl.Dir = (Single[])((Single[])GetListByName(type)[index].Prop["dir"]).Clone();
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "pos")
            {
                if (type == "AllRailInfos")
                {
                    return;
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.Position;
                    if (GetListByName(type)[index].Prop.ContainsKey("pos"))
                    {
                        cl.Pos = (Single[])((Single[])GetListByName(type)[index].Prop["pos"]).Clone();
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "Full")
            {
                if (type == "AllRailInfos")
                {
                    cl.Type = ClipBoardItem.ClipboardType.Rail;
                    cl.Rail = AllRailInfos[index].Clone();
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.FullObject;
                    cl.Objs = new LevelObj[] { GetListByName(type)[index].Clone() };
                }
            }
            else if (value == "FullArray")
            {
                if (type == "AllRailInfos")
                {
                    MessageBox.Show("Multi-Rail copy not implemented");
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.ObjectArray;
                    List<LevelObj> l = new List<LevelObj>();
                    foreach (int i in indexes) l.Add(GetListByName(type)[i].Clone());
                    cl.Objs = l.ToArray();
                }
            }
            else if (value == "Rail")
            {
                //cl.Type = ClipBoardItem.ClipboardType.Rail;
                //cl.Rail = ((Rail)GetListByName(type)[index].Prop["Rail"]).Clone();
            }
            clipboard.Add(cl);
            if (clipboard.Count > 10) clipboard.RemoveAt(0);
            ClipBoardMenu_Paste.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            for (int i = 0; i < clipboard.Count; i++)
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "ClipboardN" + i.ToString();
                btn.Text = clipboard[i].ToString();
                btn.Click += QuickClipboardItem_Click;
                Items.Add(btn);
            }
            Items.Reverse();
            ClipBoardMenu_Paste.DropDownItems.AddRange(Items.ToArray());
        }


        private void ClipBoardMenu_Opening(object sender, CancelEventArgs e)
        {
            if (GetSelectedIndexes().Length > 1)
            {
                ClipBoardMenu_CopyPos.Enabled = false;
                ClipBoardMenu_CopyRot.Enabled = false;
                ClipBoardMenu_CopyScale.Enabled = false;
                ClipBoardMenu_CopyArgs.Enabled = false;
                ClipBoardMenu_CopyFull.Text = "Copy objects";
                ClipBoardMenu_Paste.Enabled = false;
                ClipBoardMenu_CopyRail.Visible = false;
            }
            else
            {
                ClipBoardMenu_CopyArgs.Enabled = true;
                ClipBoardMenu_Paste.Enabled = true;
                ClipBoardMenu_CopyRail.Visible = false;
                if (CurrentAllInfosSectionName == "AllRailInfos")
                {
                    ClipBoardMenu_CopyPos.Enabled = false;
                    ClipBoardMenu_CopyRot.Enabled = false;
                    ClipBoardMenu_CopyScale.Enabled = false;
                    ClipBoardMenu_CopyFull.Text = "Copy rail";
                }
                else
                {
                    ClipBoardMenu_CopyFull.Text = "Copy full object";
                    ClipBoardMenu_CopyPos.Enabled = true;
                    ClipBoardMenu_CopyRot.Enabled = true;
                    ClipBoardMenu_CopyScale.Enabled = true;
                    if (treeView1.SelectedNodes.Count == 1 && CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Keys.Contains("Rail") && CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Rail"] is Rail)
                        ClipBoardMenu_CopyRail.Visible = true;
                }
                ClipBoardMenu_Paste.DropDownItems.Clear();
                List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
                for (int i = 0; i < clipboard.Count; i++)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = "ClipboardN" + i.ToString();
                    btn.Text = clipboard[i].ToString(CurrentAllInfosSelectedIndex);
                    btn.Click += QuickClipboardItem_Click;
                    Items.Add(btn);
                }
                Items.Reverse();
                ClipBoardMenu_Paste.DropDownItems.AddRange(Items.ToArray());
            }
        }

        private void QuickClipboardItem_Click(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("ClipboardN".Length));
            if (CurrentAllInfosSelectedIndex < 0)
            {
                if (clipboard[index].Type != ClipBoardItem.ClipboardType.FullObject && clipboard[index].Type != ClipBoardItem.ClipboardType.ObjectArray) return;
            }
            if (CurrentAllInfosSectionName == "AllRailInfos" && !(clipboard[index].Type == ClipBoardItem.ClipboardType.Rail || clipboard[index].Type == ClipBoardItem.ClipboardType.IntArray)) return;
            PasteValue(CurrentAllInfosSelectedIndex, CurrentAllInfosSection,CurrentAllInfosSectionName, clipboard[index]);
            /*if (SenderName.Contains("Arg"))
            {
                Panell1.ArgRefresh();
            }
            else
            {
                Panell1.Refresh();
            }*/
            RefreshProperties();
        }

        public void PasteValue(int index, List<LevelObj> type, string TypeName, ClipBoardItem itm)
        {
            if (index >= 0)
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    List<LevelObj> _type = (List<LevelObj>)args[0];
                    string _typeName = (string)args[1];
                    int id = (int)args[2];
                    object Inobj = args[3];
                    if (_typeName == "AllRailInfos")
                    {
                        AllRailInfos[id] = ((Rail)Inobj);
                    }
                    else
                    {
                        _type[id] = ((LevelObj)Inobj);
                        if (_type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, _type, _typeName);
                    }
                };
                object obj = TypeName == "AllRailInfos" ? (object)AllRailInfos[index].Clone() : type[index].Clone();
                Undo.Push(new UndoAction("Pasted value to object " + obj.ToString(), new object[] { type, TypeName, index, obj }, act));

    SaveChangeLabel();

            }
            if (itm.Type == ClipBoardItem.ClipboardType.Position)
            {
                if (type[index].Prop.ContainsKey("pos")) type[index].Prop["pos"] = itm.Pos.Clone();
                else type[index].Prop.Add("pos", itm.Pos.Clone());
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Rotation)
            {
                if (type[index].Prop.ContainsKey("dir")) type[index].Prop["dir"] = itm.Dir.Clone();
                else type[index].Prop.Add("dir", itm.Dir.Clone());
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Scale)
            {
                if (type[index].Prop.ContainsKey("scale")) type[index].Prop["scale"] = itm.Scale.Clone();
                else type[index].Prop.Add("scale", itm.Scale.Clone());
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.IntArray)
            {
                if (TypeName == "AllRailInfos")
                {
                    AllRailInfos[index].Arg = itm.Args.ToList();
                    return;
                }
                else
                {
                    if (type[index].Prop.ContainsKey("Arg")) type[index].Prop["Arg"] = itm.Args.Clone();
                    else type[index].Prop.Add("Arg", itm.Args.Clone());
                }
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Rail)
            {
                if (TypeName == "AllRailInfos")
                {
                    int id = AllRailInfos[index].l_id;
                    int no = AllRailInfos[index].no;
                    string name = AllRailInfos[index].Name;
                    AllRailInfos[index] = itm.Rail.Clone();
                    AllRailInfos[index].l_id = id;
                    AllRailInfos[index].no = no;
                    AllRailInfos[index].Name = name;
                    ObjectsListBox_SelectedIndexChanged(null, null);
                    return;
                }
                else
                {
                    if (type[index].Prop.ContainsKey("Rail")) type[index].Prop["Rail"] = itm.Rail.Clone();
                    else type[index].Prop.Add("Rail", itm.Rail.Clone());
                }
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.FullObject)
            {
                if (index < 0 || propertyGrid1.SelectedObject == null) AddObj(itm.Objs[0],  type, TypeName, true);
                else
                {
                    string name = itm.Objs[0].GetName();/*internal name*/
                    if (name == "ObjectChildArea" || name == "SwitchKeepOnArea" || name == "SwitchOnArea")
                    {
                        if (!type[index].Prop.ContainsKey("ChildrenArea")) type[index].Prop.Add("ChildrenArea", new C0List());
                        ((C0List)type[index].Prop["ChildrenArea"]).List.Add(itm.Objs[0].Clone());
                    }
                    else 
                    {
                        if (!type[index].Prop.ContainsKey("GenerateChildren")) type[index].Prop.Add("GenerateChildren", new C0List());
                        ((C0List)type[index].Prop["GenerateChildren"]).List.Add(itm.Objs[0].Clone());
                    }
                }
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.ObjectArray)
            {
                if (index < 0 || propertyGrid1.SelectedObject == null) foreach (LevelObj o in itm.Objs) AddObj(o,  type, TypeName, true);
                else
                {
                    if (index < 0 || propertyGrid1.SelectedObject == null) foreach (LevelObj o in itm.Objs) AddObj(o, type, TypeName, true);
                    foreach (LevelObj o in itm.Objs)
                    {
                        string name = o.GetName();/*internal name*/
                        if (name == "ObjectChildArea" || name == "SwitchKeepOnArea" || name == "SwitchOnArea")
                        {
                            if (!type[index].Prop.ContainsKey("ChildrenArea")) type[index].Prop.Add("ChildrenArea", new C0List());
                            ((C0List)type[index].Prop["ChildrenArea"]).List.Add(o.Clone());
                        }
                        else
                        {
                            if (!type[index].Prop.ContainsKey("GenerateChildren")) type[index].Prop.Add("GenerateChildren", new C0List());
                            ((C0List)type[index].Prop["GenerateChildren"]).List.Add(o.Clone());
                        }
                    }
                }
            }
            propertyGrid1.Refresh();
            if (index >= 0) UpdateOBJPos(index,  type, CurrentAllInfosSectionName);
        }

        void FindIndex(string type, string PropertyName, string Value)
        {
            Form ToClose = Application.OpenForms["FrmSearchResults"];
            if (ToClose != null) ToClose.Close();
            List<string> HitsNames = new List<string>();
            List<int> HitsIndexes = new List<int>();
            if (type == "AllRailInfos")
            {
                MessageBox.Show("You can't search here");
                
            }
            else
            {
                for (int i = 0; i < GetListByName(type).Count; i++)
                {
                    if (GetListByName(type)[i].Prop.ContainsKey(PropertyName) && GetListByName(type)[i].Prop[PropertyName] is Rail)
                    {
                        if (((Rail)GetListByName(type)[i].Prop[PropertyName]).Name.ToLower() == Value.ToLower())
                        {
                            HitsNames.Add(GetListByName(type)[i].GetName(true));/*db name*/ HitsIndexes.Add(i);
                        }
                    }
                    else if (GetListByName(type)[i].Prop.ContainsKey(PropertyName))
                    {
                        if (((Node)GetListByName(type)[i].Prop[PropertyName]).StringValue.ToLower().Contains(Value.ToLower()))
                        {
                            HitsNames.Add(GetListByName(type)[i].GetName(true));/*db name*/ HitsIndexes.Add(i);
                        }
                    }
                    if (GetListByName(type)[i].Prop.ContainsKey("GenerateChildren"))
                    {
                        C0List children = (C0List)GetListByName(type)[i].Prop["GenerateChildren"];
                        for (int ii = 0; ii < children.List.Count; ii++)
                        {
                            if (children.List[ii].Prop.ContainsKey(PropertyName) && children.List[ii].Prop[PropertyName] is Rail)
                            {
                                if (((Rail)children.List[ii].Prop[PropertyName]).Name.ToLower() == Value.ToLower())
                                {
                                    HitsNames.Add(children.List[ii].GetName(true)/*db name*/ + " In GenerateChildren[" + ii.ToString() + "]"); HitsIndexes.Add(i); 
                                }
                            }
                            else if (children.List[ii].Prop.ContainsKey(PropertyName))
                            {
                                if (((Node)children.List[ii].Prop[PropertyName]).StringValue.ToLower().Contains(Value.ToLower()))
                                {
                                    HitsNames.Add(children.List[ii].GetName(true)/*db name*/ + " In GenerateChildren[" + ii.ToString() + "]"); HitsIndexes.Add(i);
                                }
                            }
                        }
                    }
                }
            }
            if (HitsIndexes.Count == 0) { MessageBox.Show("Not found"); return; }
            else
            {
                FormEditors.FrmSearchResults f = new FormEditors.FrmSearchResults(type, HitsNames, HitsIndexes, this);
                f.Text = "Search Results for: " + PropertyName + " = " + Value.ToString() + "  in " + type;
                f.Show();
            }
        }

        void FindIndex(string type, string PropertyName, int Value)
        {
            Form ToClose = Application.OpenForms["FrmSearchResults"];
            if (ToClose != null) ToClose.Close();
            List<string> HitsNames = new List<string>();
            List<int> HitsIndexes = new List<int>();
            if (type == "AllRailInfos")
            {
                if (PropertyName == "l_id")
                {
                    for (int i = 0; i < AllRailInfos.Count; i++)
                    {
                        if (AllRailInfos[i].l_id == Value) { HitsNames.Add(AllRailInfos[i].ToString()); HitsIndexes.Add(i); }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GetListByName(type).Count; i++)
                {
                    if (GetListByName(type)[i].Prop.ContainsKey(PropertyName) && GetListByName(type)[i].Prop[PropertyName] is Node && ((Node)GetListByName(type)[i].Prop[PropertyName]).NodeType == Node.NodeTypes.Int)
                    {
                        if (((Node)GetListByName(type)[i].Prop[PropertyName]).StringValue == Value.ToString()) { HitsNames.Add(GetListByName(type)[i].GetName(true));/*db name*/ HitsIndexes.Add(i); }
                    }
                    /*if (GetListByName(type)[i].Prop.ContainsKey("GenerateChildren"))
                    {
                        C0List children = (C0List)GetListByName(type)[i].Prop["GenerateChildren"];
                        for (int ii = 0; ii < children.List.Count; ii++)
                        {
                            if (children.List[ii].Prop.ContainsKey(PropertyName) && children.List[ii].Prop[PropertyName] is Node && ((Node)children.List[ii].Prop[PropertyName]).NodeType == Node.NodeTypes.Int)
                            {
                                if (((Node)children.List[ii].Prop[PropertyName]).StringValue == Value.ToString()) { HitsNames.Add(children.List[ii].GetName(true) + " In GenerateChildren[" + ii.ToString() + "]"); HitsIndexes.Add(i); }
                            }
                        }
                    } */
                }
            }
            if (HitsIndexes.Count == 0) { MessageBox.Show("Not found"); return; }
            else
            {
                FormEditors.FrmSearchResults f = new FormEditors.FrmSearchResults(type, HitsNames, HitsIndexes, this);
                f.Text = "Search Results for: " + PropertyName + " = " + Value.ToString() + "  in " +type;
                f.Show();
            }
        }

        public void SetSelectedObj(string Type, int Index)
        {
            SelectInfoIndex(Type, Index);
        }
        #endregion

        #region Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadedFile.EndsWith(".szs", StringComparison.InvariantCultureIgnoreCase) && SzsFiles != null) SzsSave(LoadedFile);
            else XmlSave(LoadedFile, LoadedFile.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase));
            
        }

        void SetupSZS()
        {
            if (SzsFiles == null)
            {
                SzsFiles = new Dictionary<string, byte[]>();
                LevelCams = new CameraParams();
                SzsFiles.Add("CameraParam.byml", BymlConverter.GetByml(Properties.Resources.GenericCameraParam));
                SzsFiles.Add("PreLoadFileList1.byml", BymlConverter.GetByml(Properties.Resources.GenericPreloadList));
                SzsFiles.Add("StageInfo1.byml", BymlConverter.GetByml(Properties.Resources.GenericStageInfo));
                for (int i = 0; i < SzsFiles.Keys.Count; i++)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = "LoadFile" + i.ToString();
                    btn.Text = SzsFiles.Keys.ToArray()[i];
                    btn.Click += LoadFileList_click;
                    OtherLevelDataMenu.DropDownItems.Add(btn);
                }
            }
        }
        public void AddRecentOpen(string Filename)
        {
            if (!Properties.Settings.Default.OpenRecent.Contains(Filename))
            {
                Properties.Settings.Default.OpenRecent.Add(Filename);
                while (Properties.Settings.Default.OpenRecent.Count > 9)
                {
                    Properties.Settings.Default.OpenRecent.RemoveAt(0);
                }
                int d = Properties.Settings.Default.OpenRecent.Count;
            }
            else
            {
                Properties.Settings.Default.OpenRecent.RemoveAt(Properties.Settings.Default.OpenRecent.IndexOf(Filename));
                Properties.Settings.Default.OpenRecent.Add(Filename);
            }
            openRecentToolStripMenuItem.DropDownItems.Clear();
            for (int i = Properties.Settings.Default.OpenRecent.Count; i > 0; i--)
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "OpnRec" + (Properties.Settings.Default.OpenRecent.Count - i + 1);
                btn.Text = (Properties.Settings.Default.OpenRecent.Count - i + 1).ToString() + ". " + Properties.Settings.Default.OpenRecent[i - 1];
                btn.Click += OpenRecent_click;
                openRecentToolStripMenuItem.DropDownItems.Add(btn);

            }
            ToolStripMenuItem clr = new ToolStripMenuItem();
            clr.Name = "OpnRecClr";
            clr.Text = "Clear";
            clr.Click += OpenRecentClear;
            openRecentToolStripMenuItem.DropDownItems.Add(clr);
        }
        public Dictionary<string, AllInfoSection> DesignInfos = new Dictionary<string, AllInfoSection>();
        public Dictionary<string, AllInfoSection> SoundInfos = new Dictionary<string, AllInfoSection>();
        public Dictionary<string, AllInfoSection> MapInfos = new Dictionary<string, AllInfoSection>();
        void SzsSave(string filename)
        {
            DesignInfos.Clear();
            SoundInfos.Clear();
            MapInfos.Clear();
            foreach (string type in AllInfos.Keys)
            {
                MapInfos.Add(type, new AllInfoSection());
            }
            foreach (string section in AllInfos.Keys)
            {

                MapInfos[section].AddRange(AllInfos[section]);
                
            }
            //MapInfos = new Dictionary<string, AllInfoSection>(AllInfos);
            if ((!(filename.Substring(filename.Count() - 15).Contains("Sound") || filename.Substring(filename.Count() - 15).Contains("Design")))&& Properties.Settings.Default.UseDesignSound )
            {
                Scenario = 1;
                if (Int32.TryParse(filename.Substring(filename.Count() - 5, 1), out _))
                {
                    Scenario = Int32.Parse(filename.Substring(filename.Count() - 5, 1));
                }
                string DesignFile = filename.Substring(0, filename.Count() - 8) + "Design" + Scenario.ToString() + ".szs";
                string SoundFile = filename.Substring(0, filename.Count() - 8) + "Sound" + Scenario.ToString() + ".szs";
                DesignInfos.Add("AreaObjInfo", new AllInfoSection());
                SoundInfos.Add("AreaObjInfo", new AllInfoSection());
                SoundInfos.Add("ObjInfo", new AllInfoSection());
                CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
                NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
                if (SzsFiles.ContainsKey("CameraParam.byml"))//We create our own CameraParam.byml from the LevelCams and [VisionParams]
                {
                    SzsFiles.Remove("CameraParam.byml");
                }
                SzsFiles.Add("CameraParam.byml", BymlConverter.GetByml(MakeCamXML()));
                SFSDirectory dir = new SFSDirectory("", true);
                SFSDirectory dsgndir = new SFSDirectory("", true);
                SFSDirectory snddir = new SFSDirectory("", true);
                for (int i = 0; i < SzsFiles.Count; i++)
                {
                    SFSFile file = new SFSFile(i, SzsFiles.Keys.ToArray()[i], dir);
                    file.Data = SzsFiles.Values.ToArray()[i];
                    if (SzsFiles.Keys.ToArray()[i].Contains("FogParam")|| SzsFiles.Keys.ToArray()[i].Contains("LightParam")|| SzsFiles.Keys.ToArray()[i].Contains("ModelTo")||SzsFiles.Keys.ToArray()[i].Contains("AreaIdTo")) 
                    {
                        dsgndir.Files.Add(file);
                    }
                    else
                    {
                        dir.Files.Add(file);
                    }

                }
                SFSFile StgData = new SFSFile(SzsFiles.Count, "StageData.byml", dir);
                StgData.Data = BymlConverter.GetByml(MakeXML(true));
                dir.Files.Add(StgData);
                SzsArch.FromFileSystem(dir);
                File.WriteAllBytes(filename, y.Compress(SzsArch.Write()));
                if (DesignInfos["AreaObjInfo"].Count>0) 
                {
                    SzsArch = new NDS.NitroSystem.FND.NARC();
                    StgData = new SFSFile(SzsFiles.Count, "StageData.byml", dsgndir);
                    StgData.Data = BymlConverter.GetByml(MakeXML(false, 1));
                    dsgndir.Files.Add(StgData);
                    SzsArch.FromFileSystem(dsgndir);
                    File.WriteAllBytes(DesignFile, y.Compress(SzsArch.Write()));
                }
                if (SoundInfos["AreaObjInfo"].Count > 0||SoundInfos["ObjInfo"].Count>0)
                {
                    SzsArch = new NDS.NitroSystem.FND.NARC();
                    StgData = new SFSFile(SzsFiles.Count, "StageData.byml", snddir);
                    StgData.Data = BymlConverter.GetByml(MakeXML(false, 2));
                    snddir.Files.Add(StgData);
                    SzsArch.FromFileSystem(snddir);
                    File.WriteAllBytes(SoundFile, y.Compress(SzsArch.Write()));
                }
                AddRecentOpen(filename);
                labelStatus.Text = "Saved!";
            }
            else
            {
                CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
                NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
                SFSDirectory dir = new SFSDirectory("", true);
                for (int i = 0; i < SzsFiles.Count; i++)
                {
                    SFSFile file = new SFSFile(i, SzsFiles.Keys.ToArray()[i], dir);
                    file.Data = SzsFiles.Values.ToArray()[i];
                    dir.Files.Add(file);
                }
                SFSFile StgData = new SFSFile(SzsFiles.Count, "StageData.byml", dir);
                StgData.Data = BymlConverter.GetByml(MakeXML());
                dir.Files.Add(StgData);
                SzsArch.FromFileSystem(dir);
                File.WriteAllBytes(filename, y.Compress(SzsArch.Write()));

                AddRecentOpen(filename);

                labelStatus.Text = "Saved!";
            }
            if (this.Text.Contains("*")) this.Text = this.Text.Remove(this.Text.LastIndexOf("*")-1);
        }

        private string MakeCamXML()
        {
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = DefEnc }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("Root");
                    xr.WriteStartElement("isBigEndian");
                    xr.WriteAttributeString("Value", "False");
                    xr.WriteEndElement();
                    xr.WriteStartElement("BymlFormatVersion");
                    xr.WriteAttributeString("Value", ((uint)1).ToString());
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1"); //Byml Root
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", "CameraParams");

                    for (int i = 0; i< LevelCams.Count; i++)
                    {
                        LevelCams[i].CamToXml(xr);
                    }

                    xr.WriteEndElement();
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", "VisionParam");
                    //need to make this editable later
                    xr.WriteStartElement("D2");
                    xr.WriteAttributeString("Name", "FarClipDistance");
                    xr.WriteAttributeString("StringValue", "120000");
                    xr.WriteEndElement();
                    xr.WriteStartElement("D2");
                    xr.WriteAttributeString("Name", "FovyDegree");
                    xr.WriteAttributeString("StringValue", "45");
                    xr.WriteEndElement();
                    xr.WriteStartElement("D2");
                    xr.WriteAttributeString("Name", "NearClipDistacne");
                    xr.WriteAttributeString("StringValue", "100");
                    xr.WriteEndElement();
                    xr.WriteStartElement("D2");
                    xr.WriteAttributeString("Name", "StereovisionDepth");
                    xr.WriteAttributeString("StringValue", "0,4");
                    xr.WriteEndElement();
                    xr.WriteStartElement("D2");
                    xr.WriteAttributeString("Name", "StereovisionDistance");
                    xr.WriteAttributeString("StringValue", "350");
                    xr.WriteEndElement();

                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.Close();
                }

                return (DefEnc.GetString(stream.ToArray()));
            }
        }

        void XmlSave(string filename, bool BYML)
        {
            if (BYML) File.WriteAllBytes(filename, BymlConverter.GetByml(MakeXML()));
            else File.WriteAllText(filename, MakeXML(), DefEnc);
            MessageBox.Show("Done!");
        }

        private void saveAsSZSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedFile);
            sav.Filter = "szs file|*.szs";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                SzsSave(sav.FileName);
                LoadedFile = sav.FileName;
                this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void saveAsXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedFile);
            sav.Filter = "Xml file|*.xml";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                XmlSave(sav.FileName, false);
                LoadedFile = sav.FileName;
                this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void saveAsBymlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedFile);
            sav.Filter = "Byml file|*.byml; *.byaml";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                XmlSave(sav.FileName, true);
                LoadedFile = sav.FileName;
                this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        string MakeXML(bool getobjs = false, int snddsgn = 0)
        {
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = DefEnc }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("Root");
                    xr.WriteStartElement("isBigEndian");
                    xr.WriteAttributeString("Value", "False");
                    xr.WriteEndElement();
                    xr.WriteStartElement("BymlFormatVersion");
                    xr.WriteAttributeString("Value", ((uint)1).ToString());
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1"); //Byml Root
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", "AllInfos");
                    if (snddsgn == 0) { 
                        if (getobjs == true)
                        {
                            List<string> keys = AllInfos.Keys.ToList();
                            keys.Sort(StringComparer.Ordinal);
                            foreach (string k in keys)
                            {
                                if (AllInfos[k].Count != 0)
                                {
                                    WriteOBJInfoSection(xr, k, AllInfos[k], "C0", true);
                                }

                            }
                        }
                        else
                        {
                            List<string> keys = MapInfos.Keys.ToList();
                            keys.Sort(StringComparer.Ordinal);
                            foreach (string k in keys)
                            {
                                if (MapInfos[k].Count != 0)
                                {
                                    WriteOBJInfoSection(xr, k, MapInfos[k]);
                                }

                            }
                        }
                    }
                    else if(snddsgn == 1)
                    {
                        List<string> keys = DesignInfos.Keys.ToList();
                        keys.Sort(StringComparer.Ordinal);
                        foreach (string k in keys)
                        {
                            if (DesignInfos[k].Count != 0)
                            {
                                WriteOBJInfoSection(xr, k, DesignInfos[k]);
                            }

                        }
                    }
                    else
                    {
                        List<string> keys = SoundInfos.Keys.ToList();
                        keys.Sort(StringComparer.Ordinal);
                        foreach (string k in keys)
                        {
                            if (SoundInfos[k].Count != 0)
                            {
                                WriteOBJInfoSection(xr, k, SoundInfos[k]);
                            }

                        }
                    }
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", "AllRailInfos");
                    if (AllRailInfos.Count != 0 && snddsgn == 0)
                    {
                        xr.WriteStartElement("C0");
                        xr.WriteAttributeString("Name", "RailInfo");
                        foreach (Rail r in AllRailInfos)
                        {
                            xr.WriteStartElement("C1");
                            WriteRail(xr, r);
                            xr.WriteEndElement();
                        }
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", "LayerInfos");
                    WriteLayerInfos(xr, snddsgn);
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.Close();
                }

                return (DefEnc.GetString(stream.ToArray()));
            }
        }

        void WriteLayerInfos(XmlWriter xr, int snddsgn = 0)
        {
            //string[] LayerNames = new string[5] { "共通", "共通サブ", "シナリオ1", "シナリオ1＆2", "シナリオ1＆3" }; //PlaceHolders
            List<string> LayerNames = new List<string>();
            Dictionary<string, Dictionary<string, List<LevelObj>>> _AllInfos = new Dictionary<string, Dictionary<string, List<LevelObj>>>();
            List<string> keys;
            if (snddsgn == 0) 
            {
                 keys = MapInfos.Keys.ToList();
            }
            else if (snddsgn == 1)
            {
                keys = DesignInfos.Keys.ToList();
            }
            else 
            { 
                keys = SoundInfos.Keys.ToList(); 
            }

            keys.Sort(StringComparer.Ordinal);
            if (snddsgn == 0)
            {
                foreach (string k in keys)
                {
                    if (MapInfos[k].Count != 0)
                    {
                        _AllInfos.Add(k, new Dictionary<string, List<LevelObj>>());
                        ProcessLayerNames(MapInfos[k], _AllInfos[k], LayerNames);
                    }
                }
            }else if (snddsgn == 1)
            {
                foreach (string k in keys)
                {
                    if (DesignInfos[k].Count != 0)
                    {
                        _AllInfos.Add(k, new Dictionary<string, List<LevelObj>>());
                        ProcessLayerNames(DesignInfos[k], _AllInfos[k], LayerNames);
                    }
                }
            }
            else
            {
                foreach (string k in keys)
                {
                    if (SoundInfos[k].Count != 0)
                    {
                        _AllInfos.Add(k, new Dictionary<string, List<LevelObj>>());
                        ProcessLayerNames(SoundInfos[k], _AllInfos[k], LayerNames);
                    }
                }
            }

            for (int i = 0; i < LayerNames.Count; i++)
            {
                xr.WriteStartElement("C1");
                xr.WriteStartElement("C1");
                xr.WriteAttributeString("Name", "Infos");
                foreach (string k in _AllInfos.Keys)
                {
                    if (_AllInfos[k].ContainsKey(LayerNames[i]))
                    {
                        WriteOBJInfoSection(xr, k, _AllInfos[k][LayerNames[i]]);
                    }
                }
                xr.WriteEndElement();
                xr.WriteStartElement("A0");
                xr.WriteAttributeString("Name", "LayerName");
                xr.WriteAttributeString("StringValue", GetEnglishName(LayerNames[i]));
                xr.WriteEndElement();
                xr.WriteEndElement();
            }
        }

        string GetEnglishName(string Name)
        {
            if (Name == "共通") return "Common";
            else if (Name == "共通サブ") return "CommonSub";
            else if (Name.StartsWith("シナリオ"))
            {
                if (Name.Length == "シナリオ1".Length) return "Scenario" + Name.Substring("シナリオ".Length, 1);
                else return "Scenario" + Name.Substring("シナリオ".Length, 1) + "And" + Name.Substring("シナリオ1＆".Length, 1);
            }
            else throw new Exception("Unsupported name !");
        }

        void ProcessLayerNames( List<LevelObj> list, Dictionary<string, List<LevelObj>> Dict,  List<string> AllLayerNames)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string layerName = ((Node)list[i].Prop["LayerName"]).StringValue;
                if (Dict.ContainsKey(layerName)) Dict[layerName].Add(list[i]);
                else { Dict.Add(layerName, new List<LevelObj>()); Dict[layerName].Add(list[i]); }
                if (!AllLayerNames.Contains(layerName)) AllLayerNames.Add(layerName);
            }
            if (AllLayerNames.Count > 5) throw new Exception("Too many layer names !");
        }

        void WriteOBJInfoSection(XmlWriter xr, string name, List<LevelObj> list, string startelement = "C0", bool getobjs = false)
        {

            Dictionary<string, LevelObj> delete = new Dictionary<string, LevelObj>();
            xr.WriteStartElement(startelement);
            xr.WriteAttributeString("Name", name);
            list = new List<LevelObj>(list);
            foreach (LevelObj obj in list)
            {
                if (getobjs == true)
                {
                    string o = obj.ObjectData["name"].ToString().Substring(9);
                    switch (o)
                    {//AreaObjInfo ObjInfo
                        case "FogArea":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "FogAreaCameraPos":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "LightArea":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "SpotLightArea":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "AudioVolumeSettingArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "AudioEffectChangeArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "BgmChangeArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "SoundEmitObj":
                            SoundInfos["ObjInfo"].Add(obj);
                            MapInfos["ObjInfo"].Remove(obj);
                            break;
                        case "SoundEmitArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        default:
                            WriteOBJ(xr, obj, name);
                            break;
                    }
                    
                }
                else
                {
                    WriteOBJ(xr, obj, name);
                }
            }
            xr.WriteEndElement();
        }

        void WriteOBJ(XmlWriter xr, LevelObj obj, string Infos, bool rec = false)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            if (obj.GetName(true).StartsWith("@")) return; //for @CameraPositionHelper /*db name*/
            if (obj.Prop.ContainsKey("GenerateParent") && ((Node)obj.Prop["GenerateParent"]).StringValue != "-1"&& !rec) return;// we ignore every object that has a parent id different than -1 (which means it is a child of another object) unless it's being processed by a parent
            if (obj.Prop.ContainsKey("AreaParent") && ((Node)obj.Prop["AreaParent"]).StringValue != "-1"&& !rec) return;
            xr.WriteStartElement("C1");
            if (Infos == "ObjInfo")
            {
                if (AllInfos.ContainsKey("AreaObjInfo") && AllInfos["AreaObjInfo"].GetByParentId(int.Parse(((Node)obj.Prop["l_id"]).StringValue)).Count > 0 && !obj.Prop.ContainsKey("AreaChildren")) { obj.Prop.Add("AreaChildren", null); }
                if (AllInfos.ContainsKey("ObjInfo") && AllInfos["ObjInfo"].GetByParentId(int.Parse(((Node)obj.Prop["l_id"]).StringValue)).Count > 0 && !obj.Prop.ContainsKey("GenerateChildren")) { obj.Prop.Add("GenerateChildren", null); }

            }
            List<string> Keys = obj.Prop.Keys.ToList();
            Keys.Sort(StringComparer.Ordinal);
            foreach (string Key in Keys)
            {
                Object node = obj.Prop[Key];
                if (Key == "dbname") continue;
                if (node is int[]) //Args
                {
                    int[] tmp = ((int[])node);
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        xr.WriteStartElement("D1");
                        xr.WriteAttributeString("Name", Key + i.ToString());
                        xr.WriteAttributeString("StringValue", tmp[i].ToString());
                        xr.WriteEndElement();
                    }
                }
                else if (node is Single[]) //POS, ROT, SCA XYZ
                    
                    {
                        Single[] tmp = ((Single[])node);
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            string side = "";
                            switch (i) 
                            {
                            case 0:
                                side = "_x";
                                break;
                            case 1:
                                side = "_y";
                                break;
                            case 2:
                                side = "_z";
                                break;
                        }

                            xr.WriteStartElement("D2");
                            xr.WriteAttributeString("Name", Key + side/*.ToString()*/);
                            xr.WriteAttributeString("StringValue", tmp[i].ToString());
                            xr.WriteEndElement();
                        }
                    }
                else if (node is C0List)
                {// for each object in the allinfos of this object that has this object as parent -> if they're areas they go into generateareas, otherwise generatechildren
                    C0List tmp = (C0List)node;
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", Key);
                    foreach (LevelObj o in tmp.List) WriteOBJ(xr, o, Infos);
                    xr.WriteEndElement();
                }
                else if (Key == "AreaChildren")
                {

                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", Key);
                    foreach (LevelObj o in AllInfos["AreaObjInfo"].GetByParentId(int.Parse(((Node)obj.Prop["l_id"]).StringValue))) WriteOBJ(xr, o, "AreaObjInfo", true);
                    xr.WriteEndElement();
                }
                else if (Key == "GenerateChildren")
                {
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", Key);
                    foreach (LevelObj o in AllInfos["ObjInfo"].GetByParentId(int.Parse(((Node)obj.Prop["l_id"]).StringValue))) WriteOBJ(xr, o, "ObjInfo", true);
                    xr.WriteEndElement();
                }
                /*else if (node is Rail)
                {
                    Rail tmp = (Rail)node;
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", Key);
                    WriteRail(xr, tmp);
                    xr.WriteEndElement();
                }*/
                else if (Key == "Rail")
                {
                    if (AllRailInfos.GetById((int)node) == -1)
                    {
                        xr.WriteStartElement("FF");
                        xr.WriteAttributeString("Name", Key);
                        xr.WriteAttributeString("StringValue", "00000000");
                        xr.WriteEndElement();
                    }
                    else
                    {
                        Rail tmp = (Rail)AllRailInfos[AllRailInfos.GetById((int)node)];
                        xr.WriteStartElement("C1");
                        xr.WriteAttributeString("Name", Key);
                        WriteRail(xr, tmp);
                        xr.WriteEndElement();

                    }
                }
                else
                {
                    if (NewObjectDatabase == null || !NewObjectDatabase.DBtoId.ContainsKey(node.ToString()))
                    {
                        Node tmp = (Node)node;
                        string startelement = tmp._StringNodeType;
                        if (tmp.NodeType == Node.NodeTypes.Empty) startelement = "A1";
                        else if (tmp.NodeType == Node.NodeTypes.String) startelement = "A0";
                        else if (tmp.NodeType == Node.NodeTypes.Int) startelement = "D1";
                        else if (tmp.NodeType == Node.NodeTypes.Single) startelement = "D2";
                        xr.WriteStartElement(startelement);
                        xr.WriteAttributeString("Name", Key);
                        xr.WriteAttributeString("StringValue", tmp.StringValue);
                        xr.WriteEndElement();
                    }
                }
            }
            xr.WriteEndElement();
        }

        void WriteRail(XmlWriter xr, Rail r)
        {
            for (int i = 0; i < r.Arg.Count; i++)
            {
                xr.WriteStartElement("D1");
                xr.WriteAttributeString("Name", "Arg" + i.ToString());
                xr.WriteAttributeString("StringValue", r.Arg[i].ToString());
                xr.WriteEndElement();
            }
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "LayerName");
            xr.WriteAttributeString("StringValue", r.LayerName);
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "MultiFileName");
            xr.WriteAttributeString("StringValue", "StageData_tool");
            xr.WriteEndElement();
            xr.WriteStartElement("C0");
            xr.WriteAttributeString("Name", "Points");
            foreach (Rail.Point p in r.Points) writePoint(xr, p);
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "closed");
            xr.WriteAttributeString("StringValue", r._closed);
            xr.WriteEndElement();
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "l_id");
            xr.WriteAttributeString("StringValue", r.l_id.ToString());
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "name");
            xr.WriteAttributeString("StringValue", r.Name);
            xr.WriteEndElement();
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "no");
            xr.WriteAttributeString("StringValue", r.no.ToString());
            xr.WriteEndElement();
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "num_pnt");
            xr.WriteAttributeString("StringValue", r.Points.Count.ToString());
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "type");
            xr.WriteAttributeString("StringValue", r.Type);
            xr.WriteEndElement();
        }

        void writePoint(XmlWriter xr, Rail.Point p)
        {
            xr.WriteStartElement("C1");
            for (int i = 0; i < p.Args.Count; i++)
            {
                xr.WriteStartElement("D1");
                xr.WriteAttributeString("Name", "Arg" + i.ToString());
                xr.WriteAttributeString("StringValue", p.Args[i].ToString());
                xr.WriteEndElement();
            }
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "id");
            xr.WriteAttributeString("StringValue", p.ID.ToString());
            xr.WriteEndElement();
            for (int i = 0; i < p._X.Count; i++)
            {
                xr.WriteStartElement("D2");
                xr.WriteAttributeString("Name", "pnt" + i.ToString() + "_x");
                xr.WriteAttributeString("StringValue", p._X[i].ToString());
                xr.WriteEndElement();
                xr.WriteStartElement("D2");
                xr.WriteAttributeString("Name", "pnt" + i.ToString() + "_y");
                xr.WriteAttributeString("StringValue", p._Y[i].ToString());
                xr.WriteEndElement();
                xr.WriteStartElement("D2");
                xr.WriteAttributeString("Name", "pnt" + i.ToString() + "_z");
                xr.WriteAttributeString("StringValue", p._Z[i].ToString());
                xr.WriteEndElement();
            }
            xr.WriteEndElement();
        }


        #endregion

        #region Settings

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormEditors.Settings(render).ShowDialog();
            /*
            SettingsPanel.Visible = true;
            CamInertiaUpDown.Value = (decimal)render.CameraInertiaFactor;
            ChbFps.Checked = render.ShowFps;
            ChbTriCount.Checked = render.ShowTriangleCount;
            ChbDebugInfo.Checked = render.ShowDebugInfo;
            cbCameraMode.SelectedIndex = render.CamMode == HelixToolkit.Wpf.CameraMode.Inspect ? 0 : 1;
            ZoomSenUpDown.Value = (decimal)render.ZoomSensitivity;
            RotSenUpDown.Value = (decimal)render.RotationSensitivity;
            ChbAddCameraMove.Checked = AutoMoveCam;
            ChbStartupUpdate.Checked = Properties.Settings.Default.CheckUpdates;
            ChbStartupDb.Checked = Properties.Settings.Default.DownloadDb;
            tbUrl.Text = Properties.Settings.Default.DownloadDbLink;
            chbAddObjectOrigin.Checked = AddObjectOrigin;
            SettingsPanel.Focus();*/
        }

        #endregion

        private async void StartupChecks_DoWork(object sender, DoWorkEventArgs e)
        {
            string state = "";
            try
            {               
                if (Properties.Settings.Default.DownloadDb)
                {
                    state = "downloading object database";
                    if (File.Exists("newobjdb.xml.bak")) File.Delete("newobjdb.xml.bak");
                    if (File.Exists("newobjdb.xml")) File.Move("newobjdb.xml", "newobjdb.xml.bak");
                    new WebClient().DownloadFile(Properties.Settings.Default.DownloadDbLink == "" ? ObjectDbLink : Properties.Settings.Default.DownloadDbLink, "newobjdb.xml");
                    if (!File.Exists("newobjdb.xml"))
                    {
                        MessageBox.Show("There was an error downloading the object database");
                        if (File.Exists("newobjdb.xml.bak")) File.Move("newobjdb.xml.bak", "newobjdb.xml");
                    }
                }
                if (Properties.Settings.Default.CheckUpdates)
                {
                    state = " checking updates";
                    
                    var githubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("T4D"));
                    var ver = await githubClient.Repository.Release.GetAll("KirbysDarkNebula", "T4D");
                    if (ver.Count > ReleaseId)
                    {
                        if (MessageBox.Show("There's a new version of T4D available, do you want to open its release page in your web browser?", "", MessageBoxButtons.YesNo) == DialogResult.Yes) System.Diagnostics.Process.Start("https://github.com/KirbysDarkNebula/T4D/releases/latest");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error" + state + ": " + ex.Message);
                if (File.Exists("newobjdb.xml.bak") && !File.Exists("newobjdb.xml")) File.Move("newobjdb.xml.bak", "newobjdb.xml");
            }
        }

        private void StartupChecks_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusLbl.Text = "";
            StatusLbl.Visible = false;
            downloadLatestObjectDatabaseToolStripMenuItem.Enabled = true;
            //if (ObjectDatabase == null) LoadObjectDatabase();
            if (NewObjectDatabase == null) LoadNewDatabase();
        }

        private void guideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/KirbysDarkNebula/T4D/blob/master/guide.md");
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
        private void downloadLatestT4DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/KirbysDarkNebula/t4d-qol/releases/latest");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != ""&&(int.TryParse(textBox1.Text, out _)== false || int.Parse(textBox1.Text) > 9999)) { textBox1.Text = 10.ToString(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out _) == false|| textBox1.Text == "") { textBox1.Text = 10.ToString(); }
            Int32 add;
            int.TryParse(textBox1.Text, out add);
            string type = comboBox1.Text;
            int id = CurrentAllInfosSelectedIndex;
            string kind = "";
            switch (IncType.SelectedIndex)
            {
                case 0:
                    kind = "pos";
                    break;
                case 1:
                    kind = "dir";
                    break;
                case 2:
                    kind = "scale";
                    break;
            }
            int OldValue = (int)((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
            ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex] += (Single)add;
            Single current = ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
            if (IncType.SelectedIndex == 1 && (current > 360 || current < -360))
            {
                while (current > 360)
                {
                    ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex] -= 360;
                    current = ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
                }
                while (current < -360)
                {
                    ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex] += 360;
                    current = ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
                }
            }
            string name = "["+IncAxis.SelectedIndex.ToString()+"]";
            string pname = kind;

            Action<object[]> action;
            action = (object[] args) =>
            {
                
                    List<LevelObj> typ = (List<LevelObj>)args[0];
                    render.ClearSelection();
                    string TypeName = (string)args[1];
                    int ido = (int)args[2];
                    string propName = (string)args[5];
                    object value = args[4];
                    int axis = Int32.Parse(String.Join("", ((string)args[3]).Split('[', ']')));
                    if (propName == "pos" || propName == "scale" || propName == "dir")
                        ((Single[])(((typ)[ido].Prop[propName])))[axis] = Single.Parse((value).ToString());
                    else
                    {
                        ((Int32[])(((typ)[ido].Prop[propName])))[axis] = (Int32)value;
                    }
                    propertyGrid1.Refresh();
                    if (typ.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(ido, typ, TypeName, true);
                
            };
            Undo.Push(new UndoAction("Changed value: " + name + " of array " + pname + " of object: " + CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName(true)/*db name*/, new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex, name, OldValue, pname }, action));
            SaveChangeLabel();
            UpdateOBJPos(id, GetListByName(type), type);
            propertyGrid1.Update();
            propertyGrid1.Refresh();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out _) == false || textBox1.Text == "") { textBox1.Text = 10.ToString(); }
            Int32 add;
            int.TryParse(textBox1.Text, out add);
            string typo = comboBox1.Text;
            int ido = CurrentAllInfosSelectedIndex;
            string kind = "";
            switch (IncType.SelectedIndex)
            {
                case 0:
                    kind = "pos";
                    break;
                case 1:
                    kind = "dir";
                    break;
                case 2:
                    kind = "scale";
                    break;
            }
            int OldValue = (int)((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
            ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex] -= (Single)add;
            Single current = ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
            if (IncType.SelectedIndex == 1 && (current > 360 || current < -360))
            {
                while (current > 360) 
                { 
                    ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex] -= 360;
                    current = ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
                }
                while (current < -360) 
                { 
                    ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex] += 360;
                    current = ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
                }
            }

            


            string name = "["+IncAxis.SelectedIndex.ToString()+"]";
            string pname = kind;

            Action<object[]> action;
            action = (object[] args) =>
            {
                
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    render.ClearSelection();
                    string TypeName = (string)args[1];
                    int id = (int)args[2];
                    string propName = (string)args[5];
                    object value = args[4];
                    int axis = Int32.Parse(String.Join("", ((string)args[3]).Split('[', ']')));
                    if (propName == "pos" || propName == "scale" || propName == "dir")
                        ((Single[])(((type)[id].Prop[propName])))[axis] = Single.Parse((value).ToString());
                    else
                    {
                        ((Int32[])(((type)[id].Prop[propName])))[axis] = (Int32)value;
                    }
                    propertyGrid1.Refresh();
                    if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, type, TypeName, true);
                
            };
            Undo.Push(new UndoAction("Changed value: " + name + " of array " + pname + " of object: " + CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName(true)/*db name*/, new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex, name, OldValue, pname }, action));

SaveChangeLabel();

            UpdateOBJPos(ido, GetListByName(typo), typo);
            propertyGrid1.Update();
            propertyGrid1.Refresh();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                
                e.Effect = DragDropEffects.Copy;
                elementHost1.Hide();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string oldfile = "";

            string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (LoadedFile != "") oldfile = LoadedFile;
            LoadFile(filename[0]);
            if (!LoadedFile.ToLower().EndsWith(".szs"))
            {
                LoadedFile = oldfile;
                if (LoadedFile.ToLower().EndsWith(".szs")) elementHost1.Show();
            }
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {
            if (this.Text.Contains(@"\"))
            {
                elementHost1.Show();
            }
        }

        private void objectByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput(true);
            f.ShowDialog();
            if (f.Res == null || (string)f.Res == "") return;
            FindIndex(CurrentAllInfosSectionName, "name", (string)f.Res);
        }

        private void WorldMapEd_Click(object sender, EventArgs e)
        {
            string LFSPath =  Properties.Settings.Default.LayeredFSPath;
            if (Properties.Settings.Default.GamePath.Trim() == "" && !File.Exists(Properties.Settings.Default.UseLayeredFs ? LFSPath + "\\ObjectData\\GameSystemDataTable.szs" : @"GameSystemDataTable.szs"))
            {
                MessageBox.Show("You must set the game Romfs path first!");
                return;
            }
            else if (!File.Exists(Properties.Settings.Default.UseLayeredFs ? LFSPath + "\\ObjectData\\GameSystemDataTable.szs" : @"GameSystemDataTable.szs") && !File.Exists(Properties.Settings.Default.GamePath + "\\ObjectData\\GameSystemDataTable.szs"))
            {
                MessageBox.Show((Properties.Settings.Default.UseLayeredFs ? LFSPath + "\\ObjectData\\GameSystemDataTable.szs" : @"GameSystemDataTable.szs") + " not found!\r\nYour ROMFS dump may be incomplete or corrupted, please dump your game again.");
                return;
            }
            else if (!File.Exists(Properties.Settings.Default.UseLayeredFs ? LFSPath + "\\ObjectData\\GameSystemDataTable.szs" : @"GameSystemDataTable.szs") && File.Exists(Properties.Settings.Default.GamePath + "\\ObjectData\\GameSystemDataTable.szs"))
            {
                File.Copy(Properties.Settings.Default.GamePath + "\\ObjectData\\GameSystemDataTable.szs", Properties.Settings.Default.UseLayeredFs ? LFSPath + "\\ObjectData\\GameSystemDataTable.szs" : @"GameSystemDataTable.szs");
            }
            if (!Directory.Exists(@"icons"))
            {
                Directory.CreateDirectory("icons");
                File.WriteAllBytes(@"icons\Worldmap.zip", Properties.Resources.Worldmap);
                System.IO.Compression.ZipFile.ExtractToDirectory(@"icons\Worldmap.zip", @"icons");
                File.Delete(@"icons\Worldmap.zip");
            }
            FormEditors.FrmWorldEditor f = new FormEditors.FrmWorldEditor();
            f.ShowDialog();
        }

        private void RefreshTabs(List<string> remove, List<string> add)
        {
            if (remove != null)
            {
                foreach (string s in remove)
                {
                    if (SelectedProperties.TabPages.Contains(PropertyTabs[s]))
                    {
                        SelectedProperties.TabPages.Remove(PropertyTabs[s]);
                    }

                }
            }
            if (add != null)
            {
                foreach (string s in add)
                {
                    if (!SelectedProperties.TabPages.Contains(PropertyTabs[s]))
                    {
                        SelectedProperties.TabPages.Add(PropertyTabs[s]);
                    }
                }
            }
            elementHost1.Focus();
            elementHost1.Select();
        }
        bool refreshdone = false;
        private void RefreshProperties()
        {
            refreshdone = false;
            if (CurrentAllInfosSection.Count > 0)
            {
                ClippingGroupId.Enabled = false;
                Priority.Enabled = false;
                CameraId.Enabled = false;
                ViewId.Enabled = false;
                ShapeModelNo.Enabled = false;
                Rail.Enabled = false;
                ShapeModelNoChck.Checked = false;
                RailChck.Checked = false;
                ClippingGroupIdChck.Checked = false;
                PriorityChck.Checked = false;
                CameraIdChck.Checked = false;
                ViewIdChck.Checked = false;
                foreach (KeyValuePair<string, TabPage> tabs in PropertyTabs)
                {
                    foreach (KeyValuePair<string, object> prop in CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop)
                    {

                        string tabidentifier = "";
                        if (SelectedProperties.TabPages.Contains(tabs.Value))
                        {
                            if (tabs.Key == "General")
                            {
                                tabidentifier = "Gen";
                                //break;
                                if (prop.Key == "GenerateParent" || prop.Key == "AreaParent")
                                {
                                    Parent.Value = int.Parse(((Node)prop.Value).StringValue);
                                    if (AllInfos.ContainsKey("ObjInfo") && AllInfos["ObjInfo"].GetById((int)Parent.Value) != -1 && (int)Parent.Value != int.Parse(((Node)AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["l_id"]).StringValue))
                                    {
                                        button10.Enabled = true;
                                    }
                                    else
                                    {
                                        button10.Enabled = false;
                                    }
                                }
                                else 
                                {
                                    CurrentProperty(tabidentifier, prop, tabs, CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop);
                                    if (prop.Key == "l_id")
                                    {
                                        if (CurrentAllInfosSectionName == "ObjInfo" && AllInfos.ContainsKey("ObjInfo") && AllInfos["ObjInfo"].GetByParentId(int.Parse(((Node)prop.Value).StringValue)).Count > 0)
                                        {
                                            button2.Enabled = true;
                                        }
                                        else if (CurrentAllInfosSectionName == "ObjInfo" && AllInfos.ContainsKey("AreaObjInfo") && AllInfos["AreaObjInfo"].GetByParentId(int.Parse(((Node)prop.Value).StringValue)).Count > 0)
                                        {
                                            button2.Enabled = true;
                                        }
                                        else
                                        {
                                            button2.Enabled = false;
                                        }
                                    }
                                }
                            }
                            else if (tabs.Key == "Extra")
                            {
                                if (prop.Key == "ClippingGroupId" || prop.Key == "Priority" || prop.Key == "CameraId" || prop.Key == "ViewId" || prop.Key == "Rail" || prop.Key == "ShapeModelNo")
                                {
                                    ((CheckBox)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(prop.Key + "Chck")]).Checked = true;
                                    ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(prop.Key)]).Enabled = true;
                                    if (prop.Key == "Rail")
                                    {
                                        EditRailBtn.Enabled = true;
                                        EditRailBtn.Text = "Edit Rail";
                                        ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(prop.Key)]).Value = (int)prop.Value;
                                    }else if (prop.Key == "CameraId")
                                    {
                                        EditCamBtn.Enabled = true;
                                        
                                        if (LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, int.Parse(prop.Value.ToString().Substring(6))) != -1) EditCamBtn.Text = "Edit Camera";
                                        else EditCamBtn.Text = "Add Camera";
                                        ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(prop.Key)]).Value = int.Parse(prop.Value.ToString().Substring(6));
                                    }
                                    else
                                    {

                                        ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(prop.Key)]).Value = int.Parse(prop.Value.ToString().Substring(6));
                                    }
                                }
                                /*else if (prop.Key == "Rail")
                                {
                                    ((Button)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey("EditRailBtn")]).Text = "Edit Rail : " + prop.Value.ToString();
                                    if (prop.Value.ToString().Contains("Unk Type (FF)"))
                                    {
                                        ((Button)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey("EditRailBtn")]).Text = "Fix Rail : " + prop.Value.ToString();
                                        ((Button)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey("EditRailBtn")]).BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        ((Button)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey("EditRailBtn")]).BackColor = default;
                                    }

                                }*/
                                if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("Rail"))
                                {
                                    EditRailBtn.Enabled = false;
                                }
                                if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("CameraId"))
                                {
                                    EditCamBtn.Enabled = false;
                                }
                            }
                            else if (tabs.Key == "StartGeneral")
                            {
                                tabidentifier = "Mario";
                                CurrentProperty(tabidentifier, prop, tabs, CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop);
                                //break;
                            }
                            else if (tabs.Key == "StartEvent")
                            {
                                tabidentifier = "StrtEv";
                                CurrentProperty(tabidentifier, prop, tabs, CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop);
                                //break;
                            }
                            else if (tabs.Key == "DemoExtra")
                            {
                                tabidentifier = "Demo";
                                CurrentProperty(tabidentifier, prop, tabs, CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop);
                                //break;
                            }
                            else if (tabs.Key == "Args" || tabs.Key == "DefArgs" || tabs.Key == "GroupArgs")
                            {
                                if (prop.Key == "Arg")
                                {
                                    RefreshArgs();
                                }

                                //break;
                            }
                            else if (tabs.Key == "GroupGen")
                            {
                                tabidentifier = "Grp";
                                GroupProperty(tabidentifier, prop, tabs, CurrentAllInfosSelection);//list<LevelObj> CurrentAllInfosSelection[0].Prop this 



                                //break;
                            }
                        }

                    }
                }
            }
            else if (AllRailInfos.Count > 0)
            {
                foreach (KeyValuePair<string, TabPage> tabs in PropertyTabs)
                {
                    Rail prop = AllRailInfos[CurrentAllInfosSelectedIndex];
                    //string tabidentifier = "";
                    if (SelectedProperties.TabPages.Contains(tabs.Value))
                    {
                        if (tabs.Key == "RailTab")
                        {
                            //tabidentifier = "Rail";
                            RailName.Text = prop.Name;
                            Raill_id.Value = prop.l_id;
                            RailLayerName.Text = prop.LayerName;
                            Railno.Value = prop.no;
                            RailClosed.Checked = prop.Closed;
                            RailType.SelectedIndex = RailType.Items.IndexOf(prop.Type);
                            EditRailBtn.Tag = prop.Points;
                            ObjectName.Text = "Rail : " + prop.Name;
                        }
                        else if (tabs.Key == "Args" || tabs.Key == "DefArgs")
                        {
                            RefreshArgs();
                        }
                    }
                }
            }
            elementHost1.Focus();
            elementHost1.Select();
            refreshdone = true;
        }

        private void GroupProperty(string tabidentifier, KeyValuePair<string, object> prop, KeyValuePair<string, TabPage> tabs, List<LevelObj> currentAllInfosSelection)
        {
            //add checkbox so we can determine if all objects should use the same coordinates or if the coordinates should be relative (default is relative)
            //bool isrelative = true;

            string property = "";

            if (SelectedProperties.TabPages[tabs.Key].Controls.IndexOfKey(tabidentifier + prop.Key) != -1 || SelectedProperties.TabPages[tabs.Key].Controls.IndexOfKey(tabidentifier + prop.Key + 0) != -1)
            {
                if (prop.Value.GetType() == typeof(Node))
                {
                    if (((Node)prop.Value).NodeType == Node.NodeTypes.Int)
                    {
                        ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key)]).Value = -1;
                    }
                }
                else if ((prop.Value).GetType() == typeof(Single[]))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if(prop.Key == "scale")
                        {
                            ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Value = 1;
                        }
                        else
                        {
                            ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Value = 0;
                        }
                    }
                }
            }
            else if (prop.Key.Equals("AreaParent")) { property = "AreaParent"; }
            else if (prop.Key.Equals("GenerateParent")) { property = "GenerateParent"; }
            if (property != "")
            {
                int a = int.Parse(((Node)CurrentAllInfosSelection[0].Prop[property]).StringValue);
                for (int i = 1; i < CurrentAllInfosSelection.Count; i++)
                {
                    if (int.Parse(((Node)CurrentAllInfosSelection[i].Prop[property]).StringValue) != a) { a = -1; break; }
                }
                GrpParent.Value = a;

                if (AllInfos.ContainsKey("ObjInfo") && AllInfos["ObjInfo"].GetById((int)GrpParent.Value) != -1)
                {
                    GrpParentBtn.Enabled = true;
                }
                else
                {
                    GrpParentBtn.Enabled = false;
                }

            }



        }

        private void CurrentProperty(string tabidentifier, KeyValuePair<string, object> prop, KeyValuePair<string, TabPage> tabs, Dictionary<string,object> objprop)
        {
            if (SelectedProperties.TabPages[tabs.Key].Controls.IndexOfKey(tabidentifier + prop.Key) != -1 || SelectedProperties.TabPages[tabs.Key].Controls.IndexOfKey(tabidentifier + prop.Key + 0) != -1)
            {
                if (prop.Value.GetType() == typeof(Node))
                {
                    if (((Node)prop.Value).NodeType == Node.NodeTypes.String)
                    {
                        if (prop.Key == "name")
                        {
                            if (CurrentAllInfosSectionName != "StartInfo" && CurrentAllInfosSectionName != "AllRailInfos" && CurrentAllInfosSectionName != "DemoSceneObjInfo" && CurrentAllInfosSectionName != "StartEventObjInfo")
                            {
                                if (comboBox2.Items.Contains(((Node)prop.Value).StringValue))
                                {
                                    comboBox2.SelectedIndex = comboBox2.Items.IndexOf(((Node)prop.Value).StringValue);
                                }
                                else
                                {
                                    comboBox2.Text = ((Node)prop.Value).StringValue;
                                }
                            }
                            else if (CurrentAllInfosSectionName == "StartEventObjInfo")
                            {
                                if (comboBox3.Items.Contains(((Node)prop.Value).StringValue))
                                {
                                    comboBox3.SelectedIndex = comboBox3.Items.IndexOf(((Node)prop.Value).StringValue);
                                }
                                else
                                {
                                    comboBox3.Text = ((Node)prop.Value).StringValue;
                                }
                            }
                            else if (CurrentAllInfosSectionName == "DemoSceneObjInfo")
                            {
                                if (comboBox4.Items.Contains(((Node)prop.Value).StringValue))
                                {
                                    comboBox4.SelectedIndex = comboBox4.Items.IndexOf(((Node)prop.Value).StringValue);
                                }
                                else
                                {
                                    comboBox4.Text = ((Node)prop.Value).StringValue;
                                }
                            }

                            if (objprop.ContainsKey("dbname"))
                            {
                                ObjectName.Text = objprop["dbname"].ToString();
                                if (NewObjectDatabase.Entries[objprop["name"].ToString().Substring(9)].extra != null && NewObjectDatabase.Entries[objprop["name"].ToString().Substring(9)].extra != "")
                                {
                                    toolTip1.SetToolTip(ObjectName, NewObjectDatabase.Entries[objprop["name"].ToString().Substring(9)].extra);
                                }
                                else
                                {
                                    toolTip1.SetToolTip(ObjectName, "No info available for this object.");
                                }
                            }
                            else
                            {
                                ObjectName.Text = objprop["name"].ToString().Substring(9) + " - (Undocumented object)";
                                toolTip1.SetToolTip(ObjectName, "No info available for this object.");
                            }
                        }
                        if (prop.Key != "name" || (CurrentAllInfosSectionName == "StartInfo" || CurrentAllInfosSectionName == "AllRailInfos") )
                        SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key)].Text = prop.Value.ToString().Substring(9);

                    }
                    else if (((Node)prop.Value).NodeType == Node.NodeTypes.Int)
                    {
                        ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key)]).Value = int.Parse(prop.Value.ToString().Substring(6));
                    }
                }
                else if ((prop.Value).GetType() == typeof(Single[]))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        
                        decimal singvalue = (Single.IsNaN(((Single[])prop.Value)[i])) ? 0 : (decimal)((Single[])prop.Value)[i];
                        if (prop.Key == "dir")
                        {
                            while (singvalue > 360)
                            {
                                singvalue -= 360;
                            }
                            while (singvalue < -360)
                            {
                                singvalue += 360;
                            }
                        }
                        if (singvalue < ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Minimum) singvalue = ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Minimum;
                        if (singvalue > ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Maximum) singvalue = ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Maximum;
                        ((NumericUpDown)SelectedProperties.TabPages[tabs.Key].Controls[tabs.Value.Controls.IndexOfKey(tabidentifier + prop.Key + i)]).Value = singvalue;
                    }
                }
            }
        }
        private void RefreshArgs()
        {
            if (CurrentAllInfosSectionName == "StartInfo" || CurrentAllInfosSectionName == "CameraAreaInfo")
            {
                return;
            }
            if (SelectedProperties.TabPages.ContainsKey("DefArgs"))
            {
                if (CurrentAllInfosSectionName == "AllRailInfos")
                {
                    for (int i = 0; i < AllRailInfos[CurrentAllInfosSelectedIndex].Arg.Count; i++)
                    {
                        ((NumericUpDown)DefArgs.Controls["Defarg_int" + i.ToString()]).Value = (AllRailInfos[CurrentAllInfosSelectedIndex].Arg)[i];
                    }
                }
                else
                {

                    for (int i = 0; i < ((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"]).Length; i++)
                    {
                        ((NumericUpDown)DefArgs.Controls["Defarg_int" + i.ToString()]).Value = ((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])[i];
                    }
                }
                return;
            }
            if (SelectedProperties.TabPages.ContainsKey("GroupArgs"))
            {
                for (int i = 0; i < ((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"]).Length; i++)
                {
                    ((NumericUpDown)GroupArgs.Controls["Grparg_int" + i.ToString()]).Value = -1;
                }
                return;
            }
            NewDb newDb = new NewDb();
            if (NewObjectDatabase != null)
            {
                newDb = NewObjectDatabase;
            }
            if (CurrentAllInfosSection.Count > 0)
            {
                int y = 0;
                int x = 0;
                List<Control> args = new List<Control>();
                foreach (Control contrl in SelectedProperties.TabPages["Args"].Controls)
                {
                    args.Add(contrl);
                    if (contrl.Name == "ArgSection")
                    {
                        y = contrl.Top + 20;
                    }
                }
                foreach (Control control in args)
                {
                    if (control.Name.Contains("arg"))
                    {
                        SelectedProperties.TabPages["Args"].Controls.Remove(control);
                    }
                }
                foreach (KeyValuePair<string, object> prop in CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop)
                {
                    if (prop.Key == "Arg")
                    {
                        object objname;
                        CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.TryGetValue("name", out objname);
                        y += 10;
                        int indie = 0;
                        if (newDb != null && newDb.Entries.ContainsKey(((Node)objname).StringValue))
                        {
                            if (newDb.Entries[(((Node)objname).StringValue)].args.Count == 0)
                            {

                                SelectedProperties.TabPages["Args"].Controls.Add(new Label() {Name = "noargstext", Text = "This object doesn't have any documented args.", Location = new Point(10,y), AutoSize = true });
                            }else
                            {
                                foreach (NewDb.EntryArg arg in ((NewDb.NewDbEntry)newDb.Entries[((Node)objname).StringValue]).args)
                                {
                                    if (arg.type == "bool")
                                    {
                                        x = 16;
                                        CheckBox check = new CheckBox();
                                        var valuee = prop.Value;
                                        int value;
                                        if (((int[])valuee).Length - 1 >= newDb.Entries[((Node)objname).StringValue].args[indie].arg_id)
                                        {
                                            value = (((int[])valuee)[newDb.Entries[((Node)objname).StringValue].args[indie].arg_id]);
                                        }
                                        else
                                        {
                                            value = -1;
                                        }
                                        if (value == -1)
                                        {
                                            check.Checked = false;
                                        }
                                        else
                                        {
                                            check.Checked = true;
                                        }
                                        check.AutoSize = true;
                                        check.Enabled = true;
                                        check.Visible = true;

                                        Label label = new Label();
                                        label.Height = 15;
                                        label.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                        label.Top = y;
                                        label.Left = x;
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                        label.Width = label.Width - 10;
                                        x += 126;

                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.Entries[((Node)objname).StringValue].args[indie].info);
                                        label.Name = "lbl_arg" + indie;
                                        label.AutoSize = true;
                                        check.Top = y+3;
                                        check.Left = x;
                                        //check.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                        check.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                        check.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        check.Name = "arg_bool" + newDb.Entries[((Node)objname).StringValue].args[indie].arg_id;
                                        check.CheckedChanged += new System.EventHandler(this.checkupdated);
                                        check.Width += 0;
                                        SelectedProperties.TabPages["Args"].Controls.Add(label);
                                        SelectedProperties.TabPages["Args"].Controls.Add(check);
                                        y += 25;
                                    }
                                    else if (arg.type == "option")
                                    {
                                        x = 16;
                                        ComboBox combobox = new ComboBox();
                                        foreach (string option in arg.options.Keys)
                                        {
                                            combobox.Items.Add(option);
                                        }
                                        var valuee = prop.Value;
                                        int value = 0;
                                        if (newDb.Entries[((Node)objname).StringValue].args[indie].arg_id < ((int[])valuee).Length && newDb.Entries[((Node)objname).StringValue].args[indie].revoptions.ContainsKey(((int[])valuee)[newDb.Entries[((Node)objname).StringValue].args[indie].arg_id].ToString()))
                                        {
                                            if (((int[])valuee).Length - 1 >= newDb.Entries[((Node)objname).StringValue].args[indie].arg_id)
                                            {
                                                value = combobox.Items.IndexOf(newDb.Entries[((Node)objname).StringValue].args[indie].revoptions[((int[])valuee)[newDb.Entries[((Node)objname).StringValue].args[indie].arg_id].ToString()]);
                                            }
                                            else
                                            {
                                                value = combobox.Items.IndexOf(newDb.Entries[((Node)objname).StringValue].args[indie].revoptions[newDb.Entries[((Node)objname).StringValue].args[indie].default_value]);
                                            }
                                        }
                                        else
                                        {
                                            value = combobox.Items.IndexOf(newDb.Entries[((Node)objname).StringValue].args[indie].revoptions[newDb.Entries[((Node)objname).StringValue].args[indie].default_value]);
                                        }
                                        combobox.SelectedIndex = value;
                                        combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                                        combobox.Name = "arg_option" + newDb.Entries[((Node)objname).StringValue].args[indie].arg_id;

                                        combobox.Tag = indie;
                                        combobox.SelectedIndexChanged += new System.EventHandler(this.comboboxupdated);

                                        Label label = new Label();
                                        label.Height = 15;
                                        label.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                        label.Top = y;
                                        label.Left = x;
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                        label.Width = label.Width - 10;
                                        x += 126;
                                        combobox.Top = y;
                                        combobox.Left = x;
                                        combobox.Width += 15;
                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.Entries[((Node)objname).StringValue].args[indie].info);
                                        label.Name = "lbl_arg" + indie;
                                        label.AutoSize = true;
                                        SelectedProperties.TabPages["Args"].Controls.Add(combobox);
                                        SelectedProperties.TabPages["Args"].Controls.Add(label);

                                        y += 25;
                                        //newDb.Entries[(string)objname].args[indie].revoptions[((int[])valuee)[newDb.Entries["Kinopio"].args[indie].arg_id]];
                                    }
                                    else if (arg.type == "int")
                                    {
                                        x = 16;
                                        var valuee = prop.Value;
                                        int value;
                                        if (((int[])valuee).Length - 1 >= newDb.Entries[((Node)objname).StringValue].args[indie].arg_id)
                                        {
                                            value = (((int[])valuee)[newDb.Entries[((Node)objname).StringValue].args[indie].arg_id]);
                                        }
                                        else
                                        {
                                            value = -1;
                                        }
                                        NumericUpDown upDown = new NumericUpDown();
                                        upDown.DecimalPlaces = 0;
                                        if (newDb.Entries[((Node)objname).StringValue].args[indie].min != 0)
                                        {
                                            upDown.Minimum = newDb.Entries[((Node)objname).StringValue].args[indie].min;
                                        }
                                        else
                                        {
                                            upDown.Minimum = -10000;
                                        }
                                        if (newDb.Entries[((Node)objname).StringValue].args[indie].max != 0)
                                        {
                                            upDown.Maximum = newDb.Entries[((Node)objname).StringValue].args[indie].max;
                                        }
                                        else
                                        {
                                            upDown.Maximum = 10000;
                                        }

                                        if (value < upDown.Minimum) value = (int)upDown.Minimum;
                                        if (value > upDown.Maximum) value = (int)upDown.Maximum;

                                        upDown.Name = "arg_int" + newDb.Entries[((Node)objname).StringValue].args[indie].arg_id;
                                        upDown.Value = value;
                                        upDown.Enabled = true;
                                        upDown.Visible = true;
                                        upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                        Label label = new Label();
                                        label.Height = 15;
                                        label.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                        label.Top = y;
                                        label.Left = x;
                                        label.Name = "lbl_arg" + indie;
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.Entries[((Node)objname).StringValue].args[indie].info);
                                        label.AutoSize = true;
                                        x += 126;
                                        upDown.Top = y;
                                        upDown.Left = x;
                                        upDown.Width += 15;
                                        SelectedProperties.TabPages["Args"].Controls.Add(label);
                                        SelectedProperties.TabPages["Args"].Controls.Add(upDown);
                                        y += 25;
                                    }
                                    indie++;
                                }
                            }
                            y += 10;
                            break;
                        }
                        else
                        {
                            //int z = 0;
                            foreach (int i in (int[])prop.Value)
                            {


                                /*
                                x = 0;
                                int value = i;
                                NumericUpDown upDown = new NumericUpDown();
                                upDown.DecimalPlaces = 0;
                                upDown.Minimum = -1;
                                upDown.Maximum = 1000;
                                upDown.Name = "arg_int" + z;
                                upDown.Value = value;
                                upDown.Enabled = true;
                                upDown.Visible = true;
                                upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                Label label = new Label();
                                label.Height = 15;
                                label.Text = "Arg[" + z.ToString() + "] :";
                                label.Name = "lbl_arg" + z;
                                label.Top = y;
                                label.Left = x;
                                label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                label.Width = label.Width - 10;
                                x += 100;
                                upDown.Top = y;
                                upDown.Left = x;
                                SelectedProperties.TabPages["Args"].Controls.Add(label);
                                SelectedProperties.TabPages["Args"].Controls.Add(upDown);
                                y += 25;
                                z++;*/
                            }
                            y += 10;
                            break;
                        }


                    }
                }
            }
            else if (AllRailInfos.Count > 0)
            {
                
            }
        }
        private void numupdownupdated(object sender, EventArgs e)
        {
            if (!refreshdone) return;

            //object oldvalue = null;
            //int idx = -1;
            //Action<object[]> action;
            //string name = null;

            if (treeView1.SelectedNodes.Count > 1)
            {
                for (int i = 0; i < CurrentAllInfosSelection.Count; i++)
                {
                    //CurrentAllInfosSelection[i].Prop[]
                    if (((NumericUpDown)sender).Name.ToLower().Contains("arg"))
                    {
                        if (((int[])CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop["Arg"]).Length < 10)
                        {
                            int z = 0;
                            int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                            foreach (int iii in (int[])CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop["Arg"])
                            {
                                array[z] = iii;
                                z++;
                            }
                            CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop["Arg"] = array;
                        }
                    ((int[])CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop["Arg"])[int.Parse(((NumericUpDown)sender).Name.Substring(((NumericUpDown)sender).Name.Length - 1))] = (int)((NumericUpDown)sender).Value;

                    }
                    else
                    {
                        string property = ((NumericUpDown)sender).Name.Substring("Grp".Length);
                        int index = 0;
                        string numbered = "";
                        if (!CurrentAllInfosSelection[i].Prop.ContainsKey(property))
                        {
                            if (int.TryParse(property.Substring(property.Length - 1), out _))
                            {
                                index = int.Parse(property.Substring(property.Length - 1));
                                numbered = property.Substring(0, property.Length - 1);
                                if (!CurrentAllInfosSelection[i].Prop.ContainsKey(numbered))
                                {
                                    CurrentAllInfosSelection[i].Prop.Add(property, new Node(((NumericUpDown)sender).Value.ToString(), "D1"));
                                    SaveChangeLabel();
                                    return;
                                }
                                else
                                {
                                    property = numbered;
                                }
                            }
                            else
                            {
                                CurrentAllInfosSelection[i].Prop.Add(property, new Node(((NumericUpDown)sender).Value.ToString(), "D1"));
                                SaveChangeLabel();
                                return;
                            }

                        }
                        if (CurrentAllInfosSelection[i].Prop[property].GetType() == typeof(Node))
                        {
                            ((Node)(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop[property])).StringValue = ((NumericUpDown)sender).Value.ToString();
                        }
                        else if (CurrentAllInfosSelection[i].Prop[numbered].GetType() == typeof(Single[]))
                        {

                            if (IsRelative.Checked)
                            {
                                if (numbered == "scale")
                                {
                                    ((Single[])(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop[numbered]))[index] = (Single)((NumericUpDown)sender).Value + ((Single[])AllinfosSelection0[i].Prop[property])[index] - 1;
                                    if (((Single[])(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop[numbered]))[index] < 0) ((Single[])(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop[numbered]))[index] = 0;
                                }
                                else
                                {
                                    ((Single[])(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop[numbered]))[index] = (Single)((NumericUpDown)sender).Value + ((Single[])AllinfosSelection0[i].Prop[property])[index];
                                }
                            }
                            else
                            {
                                ((Single[])(CurrentAllInfosSection[CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i])].Prop[numbered]))[index] = (Single)((NumericUpDown)sender).Value;

                            }
                            UpdateOBJPos(CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i]), CurrentAllInfosSection, CurrentAllInfosSectionName);
                            render.UpdateSelected(i, CurrentAllInfosSection.IndexOf(CurrentAllInfosSelection[i]), CurrentAllInfosSectionName);
                        }
                    }
                }
                SaveChangeLabel();
                return;
            }
            if (((NumericUpDown)sender).Name.ToLower().Contains("arg"))
            {

                if (CurrentAllInfosSectionName == "AllRailInfos")
                {
                    if (AllRailInfos[CurrentAllInfosSelectedIndex].Arg.Count < 10)
                    {
                        int z = 0;
                        int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                        foreach (int iii in AllRailInfos[CurrentAllInfosSelectedIndex].Arg)
                        {
                            array[z] = iii;
                            z++;
                        }
                        AllRailInfos[CurrentAllInfosSelectedIndex].Arg = array.ToList();

                    }
                AllRailInfos[CurrentAllInfosSelectedIndex].Arg[int.Parse(((NumericUpDown)sender).Name.Substring(((NumericUpDown)sender).Name.Length - 1))] = (int)((NumericUpDown)sender).Value;
                }
                else
                {
                    if (((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"]).Length < 10)
                    {
                        int z = 0;
                        int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                        foreach (int iii in (int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])
                        {
                            array[z] = iii;
                            z++;
                        }
                        CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"] = array;
                    }
                //idx = int.Parse(((NumericUpDown)sender).Name.Substring(((NumericUpDown)sender).Name.Length - 1));
                //oldvalue = ((int[])CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["Arg"])[idx];
                ((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])[int.Parse(((NumericUpDown)sender).Name.Substring(((NumericUpDown)sender).Name.Length - 1))] = (int)((NumericUpDown)sender).Value;
                    //name = "Arg";

                }
            SaveChangeLabel();
            }
            else
            {
                string tabidentifier = "";
                
                if (CurrentAllInfosSection.Count > 0)
                {
                    if (SelectedProperties.SelectedTab.Name == "General")
                    {
                        tabidentifier = "Gen";
                        
                    }
                    else if (SelectedProperties.SelectedTab.Name == "Extra")
                    {

                    }
                    else if (SelectedProperties.SelectedTab.Name == "StartGeneral")
                    {
                        tabidentifier = "Mario";
                    }
                    else if (SelectedProperties.SelectedTab.Name == "StartEvent")
                    {
                        tabidentifier = "StrtEv";
                    }
                    else if (SelectedProperties.SelectedTab.Name == "DemoExtra")
                    {
                        tabidentifier = "Demo";
                    }
                    string property = ((NumericUpDown)sender).Name.Substring(tabidentifier.Length);
                    int index = 0;
                    string numbered = "";
                    if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey(property))
                    {
                        if (int.TryParse(property.Substring(property.Length - 1), out _))
                        {
                            index = int.Parse(property.Substring(property.Length - 1));
                            numbered = property.Substring(0, property.Length - 1);
                            if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey(numbered))
                            {
                                CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add(property, new Node(((NumericUpDown)sender).Value.ToString(), "D1"));
                                SaveChangeLabel();
                                return;
                            }
                            else
                            {
                                property = numbered;
                            }
                        }
                        else
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add(property, new Node(((NumericUpDown)sender).Value.ToString(), "D1"));
                            SaveChangeLabel();
                            return;
                        }

                    }

                    if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property].GetType() == typeof(Node))
                    {
                        //name = property;
                        //oldvalue = int.Parse(((Node)(CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop[property])).StringValue);
                        ((Node)(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property])).StringValue = ((NumericUpDown)sender).Value.ToString();
                        if (property == "l_id")
                        {
                            if (CurrentAllInfosSectionName == "ObjInfo" && AllInfos["ObjInfo"].GetByParentId((int)((NumericUpDown)sender).Value).Count > 0)
                            {
                                button2.Enabled = true;
                            }
                            else
                            {
                                button2.Enabled = false;
                            }
                        }
                        else if (property == "CameraId")
                        {
                            if (LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue,(int)((NumericUpDown)sender).Value) != -1)
                            {
                                EditCamBtn.Text = "Edit Camera";
                            }
                            else
                            {
                                EditCamBtn.Text = "Add Camera";
                            }
                        }
                    }
                    else if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property].GetType() == typeof(int))
                    {
                        CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property] = (int)((NumericUpDown)sender).Value;
                        if (property == "Rail")
                        {
                            if (AllRailInfos.GetById((int)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property]) != -1)
                            {
                                EditRailBtn.Text = "Edit Rail";
                            }
                            else
                            {
                                EditRailBtn.Text = "Add Rail";
                            }
                        }
                    }
                    else if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[numbered].GetType() == typeof(Single[]))
                    {
                        //name = numbered;
                        //idx = index;
                        //oldvalue = ((Single[])(CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop[numbered]))[idx];
                        ((Single[])(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[numbered]))[index] = (Single)((NumericUpDown)sender).Value;
                        UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);
                    }
                    //selectedobject.prop[property] = ((NumericUpDown)sender).Value;
                }
                else if (AllRailInfos.Count > 0)
                {
                    tabidentifier = "Rail";
                    string property = ((NumericUpDown)sender).Name.ToLower().Substring(tabidentifier.Length);
                    //name = property;
                    //oldvalue = AllRailInfos[CurrentInfosSelectedIndex][property];
                    AllRailInfos[CurrentAllInfosSelectedIndex][property] = (Int32)(((NumericUpDown)sender).Value);
                }
            SaveChangeLabel();
            }
            /*
            action = (object[] undoargs) =>
            {
                List<LevelObj> type = (List<LevelObj>)undoargs[0];
                render.ClearSelection();
                string TypeName = (string)undoargs[1];
                int id = (int)undoargs[2];
                string propName = (string)undoargs[3];
                object value = undoargs[4];
                NumericUpDown numup = (NumericUpDown)undoargs[5];
                int indx = (int)undoargs[6];
                refreshdone = false;
                if (type[id].Prop[propName] is Node) { ((Node)type[id].Prop[propName]).StringValue = value.ToString(); numup.Value = (int)value; }
                else if (type[id].Prop[propName] is Single[]) { ((Single[])type[id].Prop[propName])[indx] = ((Single)value); numup.Value = (decimal)((Single)value); }
                else if (type[id].Prop[propName] is int[]) { ((int[])type[id].Prop[propName])[indx] = ((int)value); numup.Value = ((int)value); }
                else
                {
                    type[id].Prop[propName] = value;

                    numup.Value = (decimal)value;
                }
                refreshdone = true;
                if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, type, TypeName, true);

            };
            object[] args = new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, CurrentInfosSelectedIndex, name, oldvalue, sender, idx };
            UndoAction undo = new UndoAction("Test undo " + name, args, action);
            Undo.Push(undo);*/
        }
        private void checkupdated(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Name.ToLower().Contains("arg"))
            {
                if (CurrentAllInfosSection.Count > 0)
                {
                    if (((int[])(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])).Length < 10)
                    {
                        int z = 0;
                        int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                        foreach (int iii in (int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])
                        {
                            array[z] = iii;
                            z++;
                        }
                        CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"] = array;
                    }
                    int value;
                    if (((CheckBox)sender).Checked)
                    {
                        value = 1;
                    }
                    else
                    {
                        value = -1;
                    }
                ((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])[int.Parse(((CheckBox)sender).Name.Substring(8))] = value;

                }
                else if (AllRailInfos.Count > 0)
                {

                    if ((AllRailInfos[CurrentAllInfosSelectedIndex].Arg).Count < 10)
                    {
                        int z = 0;
                        int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                        foreach (int iii in (AllRailInfos[CurrentAllInfosSelectedIndex].Arg))
                        {
                            array[z] = iii;
                            z++;
                        }
                        AllRailInfos[CurrentAllInfosSelectedIndex].Arg = array.ToList();
                    }
                    int value;
                    if (((CheckBox)sender).Checked)
                    {
                        value = 1;
                    }
                    else
                    {
                        value = -1;
                    }
                (AllRailInfos[CurrentAllInfosSelectedIndex].Arg)[int.Parse(((CheckBox)sender).Name.Substring(8))] = value;

                }
            }
            else
            {
                if (CurrentAllInfosSection.Count > 0)
                {

                }
                else if (AllRailInfos.Count > 0)
                {
                    if (((CheckBox)sender).Checked)
                    {
                        (AllRailInfos[CurrentAllInfosSelectedIndex].Closed) = true;
                    }
                    else
                    {
                        (AllRailInfos[CurrentAllInfosSelectedIndex].Closed) = false;
                    }
                    UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
                }
            }

            SaveChangeLabel();


        }
        private void comboboxupdated(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Name.ToLower().Contains("arg"))
            {
                if (!int.TryParse(((Control)sender).Tag.ToString(), out _))
                {
                    string a;
                    ((NewDb.EntryProperty)((ComboBox)sender).Tag).options.TryGetValue(((ComboBox)sender).SelectedItem.ToString(), out a);
                    //a = newDb.KnownProperties[].options;
                    CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[((Control)sender).Name] = new Node(a, "A0");
                }
                else
                {
                    if (((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"]).Length < 10)
                    {
                        int z = 0;
                        int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                        foreach (int iii in (int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])
                        {
                            array[z] = iii;
                            z++;
                        }
                        CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"] = array;
                    }
                    string aaa = CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"].ToString().Substring(9);
                    string addaa = ((ComboBox)sender).Name.Substring(10);
                    string omg = ((ComboBox)sender).SelectedItem.ToString();
                    int value = int.Parse(NewObjectDatabase.Entries[aaa].args[(int)(((ComboBox)sender).Tag)].options[omg]);
                    ((int[])CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Arg"])[int.Parse(((ComboBox)sender).Name.Substring(10))] = value;
                }
            }
            else
            {
                if (CurrentAllInfosSection.Count > 0)
                {
                    string start = "";
                    if (SelectedProperties.SelectedTab.Name == "DemoExtra")
                    {
                        start = "Demo";
                    }
                    CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[((ComboBox)sender).Name.Substring(start.Length)] = ((ComboBox)sender).SelectedText;
                }
                else if (AllRailInfos.Count > 0)
                {
                    AllRailInfos[CurrentAllInfosSelectedIndex].Type = ((ComboBox)sender).Text;
                }
            }

        SaveChangeLabel();


        }

        private void SaveChangeLabel()
        {
            //if (SzsFiles.GetHashCode() != LoadedLevelHash) //Check for level hash if it's different add *
            this.Text = this.Text + ((!this.Text.Contains("*")) ? " *" : "");
            labelStatus.Text = "";
            return;
        }

        private void EditChildrenBtn_Click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey((string)((Control)sender).Tag))
            {
                EditC0List(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[(string)((Control)sender).Tag] as C0List);
            }
            else
            {
                if (MessageBox.Show("This object doesn't have a "+(string)((Control)sender).Tag + " property, do you want to add it now?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add((string)((Control)sender).Tag, new C0List());
                }
            }

        }

        private void EditRailBtn_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text.Contains("Add"))
            {// get the highest rail id set the object's rail id to that, create a new rail with that id
                
                int id = (int)Rail.Value;
                Vector3D pos = new Vector3D(((Single[])AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["pos"])[0], -((Single[])AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["pos"])[2],((Single[])AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["pos"])[1]);
                if (AllRailInfos.GetById(id) != -1 || id == -1)
                {
                    id = highestID["AllRailInfos"] + 1;
                    AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["Rail"] = id;
                }
                if (AutoMoveCam) render.LookAt(pos);

                AddRail(new Rail(true, pos), -1, default, id);

                SelectInfoIndex("AllRailInfos", AllRailInfos.GetById(id));
            }
            else
            {// change to AllRailInfos and select the rail with the given id
                int id = (int)Rail.Value;
                SelectInfoIndex("AllRailInfos", AllRailInfos.GetById(id));
            }
        }

        private void RailPointsBtn_Click(object sender, EventArgs e)
        {
            FrmRailPointEditor Form = new FrmRailPointEditor(((Rail)AllRailInfos[CurrentAllInfosSelectedIndex]).Points);
            Form.ShowDialog();
        }

        private void PropertyTBox_Validated(object sender, EventArgs e)
        {
            if (!refreshdone) return;
            string tabidentifier = "";
            if (((TextBox)sender).Text == "")
            {
                if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey(((TextBox)sender).Name)) { CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add(((TextBox)sender).Name, new Node("", "A0")); }
                ((TextBox)sender).Text = CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[((TextBox)sender).Name].ToString().Substring(9);
            }
            if (SelectedProperties.SelectedTab.Name == "General")
            {
                tabidentifier = "Gen";

            }
            else if (SelectedProperties.SelectedTab.Name == "Extra")
            {
            }
            else if (SelectedProperties.SelectedTab.Name == "StartGeneral")
            {
                tabidentifier = "Mario";
            }
            else if (SelectedProperties.SelectedTab.Name == "StartEvent")
            {
                tabidentifier = "StrtEv";
            }
            else if (SelectedProperties.SelectedTab.Name == "DemoExtra")
            {
                tabidentifier = "Demo";
            }
            else if (SelectedProperties.SelectedTab.Name == "RailTab")
            {
                tabidentifier = "Rail";
                string railprop = ((TextBox)sender).Name.Substring(tabidentifier.Length);
                (AllRailInfos[CurrentAllInfosSelectedIndex][railprop]) = ((TextBox)sender).Text;
                if (railprop == "Name")
                {
                    treeView1.Nodes["AllRailInfos"].Nodes[CurrentAllInfosSelectedIndex].Text = AllRailInfos[CurrentAllInfosSelectedIndex].Name;
                    ObjectName.Text = "Rail : " + AllRailInfos[CurrentAllInfosSelectedIndex].Name;
                }

                return;
            }

            string property = ((TextBox)sender).Name.Substring(tabidentifier.Length);
            if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey(property))
            {
                CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add(property, new Node(((TextBox)sender).Text, "A0"));
            }
            if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property].GetType() == typeof(Node))
            {
                ((Node)(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property])).StringValue = ((TextBox)sender).Text;
            }
            if (property == "name")
            {/*
                if (((TextBox)sender).Text == (string)((TextBox)sender).Tag)
                {
                    return;
                }
                else
                {
                    ((TextBox)sender).Tag = ((TextBox)sender).Text;
                }
                obj.Prop[((TextBox)sender).Name] = new Node(((TextBox)sender).Text, "A0");*/
                NewDb newDb = new NewDb();
                newDb = NewObjectDatabase;
                if (newDb != null)
                {
                    if (newDb.Entries.ContainsKey(((TextBox)sender).Text))
                    {
                        if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("dbname"))
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["dbname"] = newDb.IdtoDB[((TextBox)sender).Text];
                        }
                        else
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add("dbname", newDb.IdtoDB[((TextBox)sender).Text]);
                        }
                    }
                    else
                    {
                        if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("dbname"))
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Remove("dbname");
                            //CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["dbname"] =CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["name"].ToString().Substring(9);
                        }
                        else
                        {
                            //CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop.Add("dbname", CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["name"].ToString().Substring(9));

                        }
                    }
                }
                string path = GetModelname(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName(true));//db name
                if (!System.IO.File.Exists(path)) path = "models\\UnkBlue.obj";
                if (CurrentAllInfosSectionName == "AreaObjInfo") path = "models\\UnkArea.obj";
                else if (CurrentAllInfosSectionName == "CameraAreaInfo") path = "models\\UnkGreen.obj";
                render.ChangeModel(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex, path);
                UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);

                fullrefresh = false;
                if (newDb != null && newDb.IdtoDB.ContainsKey(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"].ToString().Substring(9)))
                {
                    treeView1.Nodes[CurrentAllInfosSectionName].Nodes[CurrentAllInfosSelectedIndex].Text = CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["dbname"].ToString();
                }
                else
                {
                    treeView1.Nodes[CurrentAllInfosSectionName].Nodes[CurrentAllInfosSelectedIndex].Text = CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"].ToString().Substring(9);
                }

                fullrefresh = true;


                RefreshProperties();
            }
            if (property == "ModelName")
            {
                string path = GetModelname(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName(true));//db name
                if (!System.IO.File.Exists(path)) path = "models\\UnkBlue.obj";
                if (((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue == "DemoActor" || ((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue == "DemoSceneGolePole")
                {
                    if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("ModelName"))
                    {
                        path = GetModelname(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["ModelName"]).StringValue);
                    }
                }
                render.ChangeModel(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex, path);
                UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);
            }

        SaveChangeLabel();


        }

        private void EnableDisableCheck(object sender, EventArgs e)
        {
            if (!refreshdone) return;
            if (((CheckBox)sender).Checked == true)
            {
                if (SelectedProperties.SelectedTab.Name == "GroupGen")
                {
                    SelectedProperties.TabPages[SelectedProperties.SelectedTab.Name].Controls["Grp"+(string)((CheckBox)sender).Tag].Enabled = true;

                    AddRemoveProp(sender, (string)((CheckBox)sender).Tag, new Node("-1", "D1"), true);
                }
                else
                {
                    SelectedProperties.TabPages[SelectedProperties.SelectedTab.Name].Controls[(string)((CheckBox)sender).Tag].Enabled = true;

                    
                    if (((CheckBox)sender).Name.Contains("Rail"))
                    {
                        EditRailBtn.Enabled = true;
                        EditRailBtn.Text = "Add Rail";
                        Rail.Enabled = true;
                        Rail.Value = -1;
                        CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["Rail"] = -1;
                    }
                    else
                    {
                        AddRemoveProp(sender, (string)((CheckBox)sender).Tag, new Node("-1", "D1"));
                        if (((CheckBox)sender).Name.Contains("Camera"))
                        {
                            EditCamBtn.Enabled = true;
                            EditCamBtn.Text = "Add Camera";
                        }
                    }
                }

            }
            else
            {
                if (SelectedProperties.SelectedTab.Name == "GroupGen")
                {
                    SelectedProperties.TabPages[SelectedProperties.SelectedTab.Name].Controls["Grp" + (string)((CheckBox)sender).Tag].Enabled = false;
                    AddRemoveProp(sender, (string)((CheckBox)sender).Tag,default, true);
                }
                else
                {
                    SelectedProperties.TabPages[SelectedProperties.SelectedTab.Name].Controls[(string)((CheckBox)sender).Tag].Enabled = false;
                    AddRemoveProp(sender, (string)((CheckBox)sender).Tag);
                    if (((CheckBox)sender).Name.Contains("Rail"))
                    {
                        EditRailBtn.Enabled = false;
                    }
                    if (((CheckBox)sender).Name.Contains("Camera"))
                    {
                        EditCamBtn.Enabled = false;
                    }
                }
            }

        SaveChangeLabel();


        }
        private void AddRemoveProp(object sender, string propname = null , Node property = null, bool group = false)
        {
            string tabidentifier = "";
            if (SelectedProperties.SelectedTab.Name == "General")
            {
                tabidentifier = "Gen";
            }
            else if (SelectedProperties.SelectedTab.Name == "StartGeneral")
            {
                tabidentifier = "Mario";
            }
            else if (SelectedProperties.SelectedTab.Name == "StartEvent")
            {
                tabidentifier = "StrtEv";
            }
            else if (SelectedProperties.SelectedTab.Name == "DemoExtra")
            {
                tabidentifier = "Demo";
            }
            else if (SelectedProperties.SelectedTab.Name == "GroupGen")
            {
                tabidentifier = "Grp";
            }
            string prop = SelectedProperties.TabPages[SelectedProperties.SelectedTab.Name].Controls[tabidentifier+(string)((CheckBox)sender).Tag].Name.Substring(tabidentifier.Length);
            if ((object)property != null)
            {
                if (group)
                {
                    for (int u = 0; u < CurrentAllInfosSelection.Count; u++)
                    {
                        CurrentAllInfosSelection[u].Prop[prop] = property;
                    }
                }
                else
                {

                    CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[prop] = property;
                }

            }
            else
            {
                if (group)
                {
                    for (int u = 0; u < CurrentAllInfosSelection.Count; u++)
                    {
                        CurrentAllInfosSelection[u].Prop.Remove(prop);
                    }
                }
                else
                {
                    CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Remove(prop);
                }
            }
            return;
        }

        private void PropertyTbox_Enter(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PropertyTBox_Validated(sender, null);
                
            }
            return;
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }

        private void NameChangeTest_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_Validated(object sender, EventArgs e)
        {
            if (!refreshdone) return;
            string tabidentifier = "";
            if (((ComboBox)sender).Text == "")
            {
                ((ComboBox)sender).Text = ((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue;
            }
            if (SelectedProperties.SelectedTab.Name == "General")
            {
                tabidentifier = "Gen";

            }
            else if (SelectedProperties.SelectedTab.Name == "DemoExtra")
            {
                tabidentifier = "Demo";
            }
            else if (SelectedProperties.SelectedTab.Name == "StartEvent")
            {
                tabidentifier = "StrtEv";
            }
            string property = "name";
            if (!CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey(property))
            {
                CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add(property, new Node(((ComboBox)sender).Text, "A0"));
            }
            if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property].GetType() == typeof(Node))
            {
                ((Node)(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop[property])).StringValue = ((ComboBox)sender).Text;
            }
            if (property == "name")
            {/*
                if (((TextBox)sender).Text == (string)((TextBox)sender).Tag)
                {
                    return;
                }
                else
                {
                    ((TextBox)sender).Tag = ((TextBox)sender).Text;
                }
                obj.Prop[((TextBox)sender).Name] = new Node(((TextBox)sender).Text, "A0");*/

                if (((ComboBox)sender).Items.Contains(((ComboBox)sender).Text))
                {
                    ((ComboBox)sender).SelectedIndex = ((ComboBox)sender).Items.IndexOf(((ComboBox)sender).Text);
                }
                else
                {
                 //   comboBox2.Text = ((Node)prop.Value).StringValue;
                }
                NewDb newDb = new NewDb();
                newDb = NewObjectDatabase;
                if (newDb != null)
                {
                    if (newDb.Entries.ContainsKey(((ComboBox)sender).Text))
                    {
                        if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("dbname"))
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["dbname"] = newDb.IdtoDB[((ComboBox)sender).Text];
                        }
                        else
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Add("dbname", newDb.IdtoDB[((ComboBox)sender).Text]);
                        }
                    }
                    else
                    {
                        if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.ContainsKey("dbname"))
                        {
                            CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop.Remove("dbname");
                            //CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["dbname"] =CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["name"].ToString().Substring(9);
                        }
                        else
                        {
                            //CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop.Add("dbname", CurrentAllInfosSection[CurrentInfosSelectedIndex].Prop["name"].ToString().Substring(9));

                        }
                    }
                }
                string path = GetModelname(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName(true));//db name
                if (!System.IO.File.Exists(path)) path = "models\\UnkBlue.obj";
                if (CurrentAllInfosSectionName == "AreaObjInfo") path = "models\\UnkArea.obj";
                else if (CurrentAllInfosSectionName == "CameraAreaInfo") path = "models\\UnkGreen.obj";
                //foreach (int i in treeView1.SelectedNodes)
                render.ChangeModel(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex, path);
                UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);

                fullrefresh = false;
                if (newDb != null && newDb.IdtoDB.ContainsKey(CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"].ToString().Substring(9)))
                {
                    treeView1.Nodes[CurrentAllInfosSectionName].Nodes[CurrentAllInfosSelectedIndex].Text = CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["dbname"].ToString();
                }
                else
                {
                    treeView1.Nodes[CurrentAllInfosSectionName].Nodes[CurrentAllInfosSelectedIndex].Text = CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"].ToString().Substring(9);
                }

                fullrefresh = true;


                RefreshProperties();
            }

        SaveChangeLabel();


        }

        private void comboBox2_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboBox2_Validated(sender, null);
            }
            return;
        }
        bool increased = false;
        void SetIncrement(int val)
        {
            increased = false;
            GenIncrement.Value = val;
            Demopos0.Increment = val;
            Demopos1.Increment = val;
            Demopos2.Increment = val;
            Mariopos0.Increment = val;
            Mariopos1.Increment = val;
            Mariopos2.Increment = val;
            Grppos0.Increment = val;
            Grppos1.Increment = val;
            Grppos2.Increment = val;
            Genpos0.Increment = val;
            Genpos1.Increment = val;
            Genpos2.Increment = val;
            StrtEvpos0.Increment = val;
            StrtEvpos1.Increment = val;
            StrtEvpos2.Increment = val;
            increased = true;
        }

        private void DemoIncrement_ValueChanged(object sender, EventArgs e)
        {
            if (increased)
            {
                SetIncrement((int)DemoIncrement.Value);
            }
            Demopos0.Increment = DemoIncrement.Value;
            Demopos1.Increment = DemoIncrement.Value;
            Demopos2.Increment = DemoIncrement.Value;
        }

        private void MarioIncrement_ValueChanged(object sender, EventArgs e)
        {
            if (increased)
            {
                SetIncrement((int)MarioIncrement.Value);
            }
            Mariopos0.Increment = MarioIncrement.Value;
            Mariopos1.Increment = MarioIncrement.Value;
            Mariopos2.Increment = MarioIncrement.Value;
        }

        /*private void GrpIncrement_ValueChanged(object sender, EventArgs e)
        {
            if (increased)
            {
                SetIncrement((int)GrpIncrement.Value);
            }
            Grppos0.Increment = GrpIncrement.Value;
            Grppos1.Increment = GrpIncrement.Value;
            Grppos2.Increment = GrpIncrement.Value;
        }*/

        private void GenIncrement_ValueChanged(object sender, EventArgs e)
        {
            if (increased)
            {
                SetIncrement((int)GenIncrement.Value);
            }
            Genpos0.Increment = GenIncrement.Value;
            Genpos1.Increment = GenIncrement.Value;
            Genpos2.Increment = GenIncrement.Value;
        }

        private void Parentupdownupdated(object sender, EventArgs e)
        {

            if (refreshdone == false) return;
            if (CurrentAllInfosSectionName == "AreaObjInfo")
            {
                if (!AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop.ContainsKey("AreaParent")) { AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop.Add("AreaParent",new Node("-1", "D1")); }
                ((Node)AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["AreaParent"]).StringValue = Parent.Value.ToString();
                if (AllInfos.ContainsKey("ObjInfo") && AllInfos["ObjInfo"].GetById((int)Parent.Value) != -1 && (int)Parent.Value != int.Parse(((Node)AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["l_id"]).StringValue))
                {
                    button10.Enabled = true;
                }
                else
                {
                    button10.Enabled = false;
                }
            }else if (CurrentAllInfosSectionName == "ObjInfo")
            {
                if (!AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop.ContainsKey("GenerateParent")) { AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop.Add("GenerateParent", new Node("-1", "D1")); }

                ((Node)AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["GenerateParent"]).StringValue = Parent.Value.ToString();
                if (AllInfos["ObjInfo"].GetById((int)Parent.Value) != -1&& (int)Parent.Value != int.Parse(((Node)AllInfos[CurrentAllInfosSectionName][CurrentAllInfosSelectedIndex].Prop["l_id"]).StringValue))
                {
                    button10.Enabled = true;
                }
                else
                {
                    button10.Enabled = false;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            selecting = true;
            ObjectsListBox.ClearSelected();

            selecting = false;
            if (((Button)sender).Name.Contains("Grp") && AllInfos.ContainsKey("ObjInfo"))
            {
                SelectInfoIndex("ObjInfo", AllInfos["ObjInfo"].GetById((int)GrpParent.Value));
                //ObjectsListBox.SetSelected(AllInfos["ObjInfo"].GetById((int)GrpParent.Value), true);
            }
            else
            {
                SelectInfoIndex("ObjInfo", AllInfos["ObjInfo"].GetById((int)Parent.Value));
                //ObjectsListBox.SetSelected(AllInfos["ObjInfo"].GetById((int)Parent.Value), true);
            }
            //CurrentInfosSelectedIndex = AllInfos["ObjInfo"].GetById();
            render.CameraToObj("ObjInfo", CurrentAllInfosSelectedIndex);
        }

        private void Children_Click(object sender, EventArgs e)
        {
            int selid = CurrentAllInfosSelectedIndex;

            string info;
            if (AllInfos["ObjInfo"].GetByParentId(int.Parse(((Node)AllInfos["ObjInfo"][selid].Prop["l_id"]).StringValue)).Count != 0) { info = "ObjInfo"; }
            else if (AllInfos["AreaObjInfo"].GetByParentId(int.Parse(((Node)AllInfos["ObjInfo"][selid].Prop["l_id"]).StringValue)).Count != 0) { info = "AreaObjInfo"; }
            else return;
            selecting = true;
            //if (comboBox1.SelectedIndex != comboBox1.Items.IndexOf(info)) comboBox1.SelectedIndex = comboBox1.Items.IndexOf(info);
            ObjectsListBox.ClearSelected();
            selecting = false; 

            foreach (LevelObj o in AllInfos[info].GetByParentId(int.Parse(((Node)AllInfos["ObjInfo"][selid].Prop["l_id"]).StringValue)))
            {
                SelectInfoIndex(info, AllInfos[info].GetById(int.Parse(((Node)o.Prop["l_id"]).StringValue)));
            }
            render.CameraToObj(info, CurrentAllInfosSelectedIndex);
        }

        private void GrpParentUpdwn(object sender, EventArgs e)
        {
            if (!refreshdone) return;
            for (int i = 0; i < CurrentAllInfosSelection.Count; i++)
            {
                string property = "";
                if (CurrentAllInfosSectionName == "ObjInfo")
                {
                    property = "GenerateParent";
                }else if (CurrentAllInfosSectionName == "AreaObjInfo")
                {
                    property = "AreaParent";
                }
                if (!CurrentAllInfosSelection[i].Prop.ContainsKey(property))
                {
                    CurrentAllInfosSelection[i].Prop.Add(property, new Node(((NumericUpDown)sender).Value.ToString(), "D1"));
                }
                else
                {
                    ((Node)CurrentAllInfosSelection[i].Prop[property]).StringValue = ((NumericUpDown)sender).Value.ToString();
                }

            }
            if (AllInfos.ContainsKey("ObjInfo") && AllInfos["ObjInfo"].GetById((int)GrpParent.Value) != -1)
            {
                GrpParentBtn.Enabled = true;
            }
            else
            {
                GrpParentBtn.Enabled = false;
            }
        }

        private void OtherLevelDataMenu_Click(object sender, EventArgs e)
        {

        }

        private void EditCamBtn_Click(object sender, EventArgs e)
        {
            int cameraId = int.Parse(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["CameraId"]).StringValue);
            if (cameraId < 0)
            {
                MessageBox.Show("CameraId can't be less than 0!");
                return;
            }


            int camindex = LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, cameraId);
            if (camindex != -1)
            {
                FormEditors.FrmAddCameraSettings frm = new FormEditors.FrmAddCameraSettings(LevelCams[LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, cameraId)]);
                frm.parent = this;
                frm.ShowDialog(); 
                foreach (FrmCameraViewer viewer in frm.CamPreviews)
                {
                    viewer.Close();
                }
                if (frm.Ret != null)
                {
                    if (frm.Ret.UserGroupId != cameraId)
                    {
                        ((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["CameraId"]).StringValue = frm.Ret.UserGroupId.ToString();

                        LevelCams.Remove(LevelCams[LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, cameraId)]);
                        LevelCams.Add(frm.Ret);
                    }
                    else
                    {
                        LevelCams.Insert(LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, cameraId), frm.Ret);
                        LevelCams.Remove(LevelCams[LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, cameraId) + 1]);
                    }
                }
                else return;
                //if (frm.XmlRes != null) SzsFiles["CameraParam.byml"] = BymlConverter.GetByml(frm.XmlRes);
                SaveChangeLabel();
            }
            else
            {
                FormEditors.FrmAddCameraSettings frm = new FormEditors.FrmAddCameraSettings(new Camera3DL(true) { UserGroupId = cameraId, UserName = ((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, Category = (CurrentAllInfosSectionName == "CameraAreaInfo") ? "Map" : "Object" }, true);
                frm.parent = this;
                frm.ShowDialog();
                foreach (FrmCameraViewer viewer in frm.CamPreviews)
                {
                    viewer.Close();
                }
                if (frm.Ret != null)
                {
                    if (LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, frm.Ret.UserGroupId) != -1)
                    {
                        LevelCams.Insert(LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, frm.Ret.UserGroupId), frm.Ret);
                        LevelCams.Remove(LevelCams[LevelCams.GetIndex(((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["name"]).StringValue, frm.Ret.UserGroupId) + 1]);
                    }
                    else
                    {

                        LevelCams.Add(frm.Ret);
                    }

                }
                else return;
                if (frm.Ret.UserGroupId != cameraId) ((Node)CurrentAllInfosSection[CurrentAllInfosSelectedIndex].Prop["CameraId"]).StringValue = frm.Ret.UserGroupId.ToString();
                //if (frm.XmlRes != null) SzsFiles["CameraParam.byml"] = BymlConverter.GetByml(frm.XmlRes);
                EditCamBtn.Text = "Edit Camera";
                SaveChangeLabel();
            }
        }
        List<bool> ShownScenarios = new List<bool>() { true, true, true, true, true }; // common 1 2 3 subcommon
        private void CommonScenario_CheckedChanged(object sender, EventArgs e)
        {
            string scenario = "";
            int scid = -1;
            if ((string)((ToolStripMenuItem)sender).Tag == "0")
            {
                scenario = "共通";
                scid = 0;
            }
            else if ((string)((ToolStripMenuItem)sender).Tag == "1" || (string)((ToolStripMenuItem)sender).Tag == "2" || (string)((ToolStripMenuItem)sender).Tag == "3")
            {
                scenario = "シナリオ" + ((ToolStripMenuItem)sender).Tag;
                scid = int.Parse((string)((ToolStripMenuItem)sender).Tag);
            }
            ShownScenarios[scid] = ((ToolStripMenuItem)sender).Checked;
            if (!((ToolStripMenuItem)sender).Checked)HideScenario(scenario, scid);
            if (((ToolStripMenuItem)sender).Checked)ShowScenario(scenario, scid);
        }

        private void ShowCamerasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ShowAreas[0])
            {
                ShowAreas[0] = false;
                toolStripMenuItem1.Text = "Show Cameras";
                HideLayer("CameraAreaInfo");
            }
            else
            {

                ShowAreas[0] = true;
                toolStripMenuItem1.Text = "Hide Cameras";
                ShowLayer("CameraAreaInfo");
            }
        }
        private void showAreasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowAreas[1])
            {
                ShowAreas[1] = false;
                showAreasToolStripMenuItem.Text = "Show Areas";
                HideLayer("AreaObjInfo");
            }
            else
            {

                ShowAreas[1] = true;
                showAreasToolStripMenuItem.Text = "Hide Areas";
                ShowLayer("AreaObjInfo");
            }
        }

        private void showRailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowAreas[2])
            {
                ShowAreas[2] = false;
                showRailsToolStripMenuItem.Text = "Show Rails";
            }
            else
            {
                ShowAreas[2] = true;
                showRailsToolStripMenuItem.Text = "Hide Rails";
            }
            HideShowRails(ShowAreas[2]);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Parent == null)
            {
                Btn_AddObj.Tag = treeView1.SelectedNode.Name;
            }
            else
            {

                if ((string)treeView1.SelectedNode.Parent.Tag == "Rail")
                {
                    //change property tabs to points and args
                    Btn_AddObj.Tag = treeView1.SelectedNode.Parent.Tag;
                }
                else
                {
                    Btn_AddObj.Tag = treeView1.SelectedNode.Parent.Name;
                }
                if (treeView1.SelectedNode.Parent.Name == "AllRailInfos")
                {

                    //change property tabs to Rails and args
                }
                else if (treeView1.SelectedNode.Parent.Name == "DemoSceneObjInfo")
                {

                    //change property tabs to Demo and args
                }
                else if (treeView1.SelectedNode.Parent.Name == "StartInfo")
                {
                    //change property tabs to Mario
                }
                else if (treeView1.SelectedNode.Parent.Name == "StartEventObjInfo")
                {
                    //change property tabs to StartEv and args
                }
                else if (treeView1.SelectedNode.Parent.Name == "CameraAreaInfo")
                {

                    //change property tabs to Gen and Extra
                }
                else
                {

                    //change property tabs to Gen extra and args
                }
            }
            if (selecting) return;
            btn_cameraCode.Visible = false;
            render.ClearTmpObjects();
            render.UnselectRail();
            render.ClearSelection();
            //lblDescription.Text = "";
            //lblDescription.Tag = -1;

            List<string> remove = new List<string>();
            List<string> add = new List<string>();
            if (CurrentAllInfosSelectedIndex < 0)
            {
                if (DraggingArgs == null)
                {
                    index = -1;
                    foreach (string tab in PropertyTabs.Keys.Reverse())
                    {
                        remove.Add(tab);
                    }
                    RefreshTabs(remove, null);
                    SelectedProperties.Enabled = false;
                    SelectedProperties.Visible = false;
                    ObjectName.Text = "No object selected";
                }
                return;
            }
            else
            {
                SelectedProperties.Enabled = true;
                SelectedProperties.Visible = true;
            }
            if (treeView1.SelectedNodes.Count > 1)
            {

                Btn_CopyObjs.Visible = true;
                //Btn_Duplicate.Visible = false;
                btn_delObj.Text = "Delete objects";
                if (CurrentAllInfosSectionName != "AllRailInfos")
                {
                    CurrentAllInfosSelection.Clear();
                    AllinfosSelection0.Clear();
                    for (int a = 0; a < treeView1.SelectedNodes.Count; a++)
                    {
                        CurrentAllInfosSelection.Add(CurrentAllInfosSection[treeView1.Nodes[CurrentAllInfosSectionName].Nodes.IndexOf(treeView1.SelectedNodes[a])]);
                        AllinfosSelection0.Add(CurrentAllInfosSelection[a].Clone());
                    }

                    render.SelectObjs(CurrentAllInfosSectionName, GetSelectedIndexes());
                }
                ObjectName.Text = "Selected multiple objects";
                if (DraggingArgs == null)
                {
                    SelectCorrectProperty();
                }
                RefreshProperties();
                index = -2;
                return;
            }
            else
            {
                Btn_CopyObjs.Visible = false;
                Btn_Duplicate.Visible = true;
                btn_delObj.Text = "Delete object";
                index = CurrentAllInfosSelectedIndex;
                //if (ObjectDatabase != null) UpdateHint();
                if (DraggingArgs == null)
                {
                    SelectCorrectProperty();
                    if (changedAllInfos)
                    {
                        if (treeView1.SelectedNodes.Count == 1)
                        {
                            if (NewObjectDatabase != null)
                            {
                                if (CurrentAllInfosSectionName != "StartInfo" && CurrentAllInfosSectionName != "CameraAreaInfo")
                                {
                                    if (NewObjectDatabase.DBtoId.ContainsKey(treeView1.SelectedNode.Text))
                                    {
                                        remove.Insert(0, "GroupArgs");
                                        remove.Insert(0, "DefArgs");
                                        remove.Insert(0, "Args");
                                        remove.Add("GroupGen");
                                        add.Add("Args");
                                    }
                                    else
                                    {
                                        remove.Insert(0, "DefArgs");
                                        remove.Insert(0, "GroupArgs");
                                        remove.Insert(0, "Args");
                                        remove.Add("GroupGen");
                                        add.Add("DefArgs");
                                        if (CurrentAllInfosSectionName == "AllRailInfos")
                                        {

                                            label14.Text = "";
                                        }
                                        else
                                        {
                                            label14.Text = "This object's args are undocumented!";
                                        }
                                    }
                                }
                                else
                                {
                                    remove.Insert(0, "GroupArgs");
                                    remove.Insert(0, "DefArgs");
                                    remove.Insert(0, "Args");
                                    remove.Add("GroupGen");
                                }
                            }
                            else
                            {
                                if (CurrentAllInfosSectionName != "StartInfo" && CurrentAllInfosSectionName != "CameraAreaInfo")
                                {
                                    remove.Add("DefArgs");
                                    remove.Insert(0, "GroupArgs");
                                    remove.Insert(0, "Args");
                                    remove.Add("GroupGen");
                                    add.Add("DefArgs");
                                    if (CurrentAllInfosSectionName == "AllRailInfos")
                                    {
                                        label14.Text = "";
                                    }
                                    else
                                    {
                                        label14.Text = "This object's args are undocumented!";
                                    }
                                }
                            }
                            RefreshTabs(remove, add);
                        }
                    }
                }

            }
            if (CurrentAllInfosSectionName == "AreaObjInfo")
            {
                if (((AllInfoSection)CurrentAllInfosSection).IsHidden)
                {
                    if (AreaObjOldSelection != -1 && AreaObjOldSelection < AllInfos["AreaObjInfo"].Count) render.ChangeTransform(CurrentAllInfosSectionName, AreaObjOldSelection, render.Positions[CurrentAllInfosSectionName][AreaObjOldSelection], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);
                }
                AreaObjOldSelection = CurrentAllInfosSelectedIndex;
                if (CurrentAllInfosSection[CurrentAllInfosSelectedIndex].GetName()/*internal name*/.Contains("FogArea")) { btn_cameraCode.Visible = true; btn_cameraCode.Text = "Edit fog area"; }
                RefreshProperties();
                return;
            }
            else if (CurrentAllInfosSectionName == "CameraAreaInfo")
            {
                if (((AllInfoSection)CurrentAllInfosSection).IsHidden)
                {
                    if (CameraAreaOldSelection != -1 && CameraAreaOldSelection < AllInfos["CameraAreaInfo"].Count) render.ChangeTransform(CurrentAllInfosSectionName, CameraAreaOldSelection, render.Positions[CurrentAllInfosSectionName][CameraAreaOldSelection], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);
                }
                CameraAreaOldSelection = CurrentAllInfosSelectedIndex;
                //if (CurrentAllInfosSection[CurrentInfosSelectedIndex].GetName().Contains("CameraAre")) { btn_cameraCode.Visible = true; btn_cameraCode.Text = "Edit camera code"; }
                RefreshProperties();
                return;
            }
            else if (CurrentAllInfosSectionName == "AllRailInfos")
            {
                if (AllRailInfos.IsHidden)
                {
                    if (RailOldSel != -1 && RailOldSel < AllRailInfos.Count)
                    {
                        render.HideGroup("AllRailInfos");
                    }
                    List<Point3D> p3d = new List<Point3D>();
                    foreach (The4Dimension.Rail.Point p in AllRailInfos[CurrentAllInfosSelectedIndex].Points)
                    {
                        p3d.Add(p.ToPoint3D());
                    }

                    render.ShowRail(CurrentAllInfosSelectedIndex);
                    UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
                }
                RailOldSel = CurrentAllInfosSelectedIndex;
                UpdateRailpos(CurrentAllInfosSelectedIndex, AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
                render.SelectRail(AllRailInfos[CurrentAllInfosSelectedIndex].GetPointArray());
                RefreshProperties();
            }
            else if (CurrentAllInfosSectionName != null)
            {
                if (((AllInfoSection)CurrentAllInfosSection).IsHidden)
                {
                    if (ObjInfoSel != -1 && ObjInfoSel < AllInfos[CurrentAllInfosSectionName].Count) render.ChangeTransform(CurrentAllInfosSectionName, ObjInfoSel, render.Positions[CurrentAllInfosSectionName][ObjInfoSel], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    UpdateOBJPos(CurrentAllInfosSelectedIndex, CurrentAllInfosSection, CurrentAllInfosSectionName);
                }
                ObjInfoSel = CurrentAllInfosSelectedIndex;
                render.SelectObjs(CurrentAllInfosSectionName, GetSelectedIndexes());
                RefreshProperties();
            }

        }

        int CurrentAllInfosSelectedIndex
        {
            get 
            {
                if (treeView1.SelectedNode == null ||treeView1.SelectedNode.Parent == null)
                {
                    return -1;
                }
                if ((string)treeView1.SelectedNode.Parent.Tag == "Rail")
                {
                    //change property tabs to points and args
                    return treeView1.Nodes["AllRailInfos"].Nodes.IndexOf(treeView1.SelectedNode.Parent);
                }
                else
                {
                    return treeView1.SelectedNode.Parent.Nodes.IndexOf(treeView1.SelectedNode);
                }
            }
        }
        void SelectInfoIndex(int idx)
        {
            if (idx < 0) treeView1.SelectedNode = treeView1.Nodes[CurrentAllInfosSectionName];
            else treeView1.SelectedNode = treeView1.Nodes[CurrentAllInfosSectionName].Nodes[idx];
            return;
        }
        void SelectInfoIndex(string section, int idx)
        {
            if (idx < 0) treeView1.SelectedNode = treeView1.Nodes[section];
            else treeView1.SelectedNode = treeView1.Nodes[section].Nodes[idx];
            return;
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {

            if (treeView1.SelectedNodes.Count > 1) return; // all with count are temporary
            if (CurrentAllInfosSelectedIndex == -1) return;
            render.CameraToObj(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex);
        }

        private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SelectInfoIndex(-1);
            }
        }

        private void treeView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && e.Control)
            {
                if (Undo.Count > 0) Undo.Pop().Undo();
                return;
            }
            if (CurrentAllInfosSelectedIndex == -1) return;
            if (e.KeyCode == Keys.Space) render.CameraToObj(CurrentAllInfosSectionName, CurrentAllInfosSelectedIndex);
            else if (e.KeyCode == Keys.D && e.Control && treeView1.SelectedNodes.Count > 0) button2_Click(null, null); //Duplicate
            else if (e.KeyCode == Keys.Delete) button3_Click(null, null); //Delete obj
            else if (e.KeyCode == Keys.F && e.Control) findToolStripMenuItem.ShowDropDown();
            else if (e.KeyCode == Keys.Oemplus) { if (Btn_AddObj.Enabled == true) BtnAddObj_Click(null, null); } //Add obj            

            else return;
            e.SuppressKeyPress = true;
        }

        int[] GetSelectedIndexes()
        {
            if (CurrentAllInfosSectionName == null) return new int[0];
            if (treeView1.SelectedNodes.Count == 0) return new int[0];
            int[] r = new int[treeView1.SelectedNodes.Count];
            int idx = 0;
            foreach (TreeNode node in treeView1.SelectedNodes)
            {
                r[idx] = treeView1.Nodes[CurrentAllInfosSectionName].Nodes.IndexOf(node);
                idx++;
            }

            int[] res = r.OrderBy(y => y).ToArray();
            
            return res.Reverse().ToArray();
        }

    }
}