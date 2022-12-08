using LibEveryFileExplorer.Files.SimpleFileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace The4Dimension.BgmEditors
{
    public partial class FrmBgmMain : Form
    {
        Dictionary<string, byte[]> SzsFiles = new Dictionary<string, byte[]>();
        Dictionary<string, string> LevelsNum = new Dictionary<string, string>(); //WX-X, LevelName
        Dictionary<string, string> Levels = new Dictionary<string, string>(); //LevelName, Music
        private Dictionary<int, List<string>> WorldLevelDict = new Dictionary<int, List<string>>();//tab id, levels of the world as a list
        List<string> Music = new List<string>();

        public FrmBgmMain(Dictionary<string, string> NametoNum)
        {
            InitializeComponent();
            LevelsNum = NametoNum;
            string[] FullLevelList = NametoNum.Keys.ToArray();
            List<string> vs = new List<string>();
            for (int i = 0; i < FullLevelList.Length; i++)
            {
                if (FullLevelList[i].StartsWith("W"))
                {
                    string newstring = FullLevelList[i].Substring(1, 1);
                    string nextstring = FullLevelList[i + 1].Substring(1, 1);
                    switch (newstring)
                    {
                        case "1":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W1 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W1);
                                vs.Clear();
                            }
                            break;
                        case "2":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W2 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W2);
                                vs.Clear();
                            }
                            break;
                        case "3":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W3 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W3);
                                vs.Clear();
                            }
                            break;
                        case "4":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W4 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W4);
                                vs.Clear();
                            }
                            break;
                        case "5":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W5 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W5);
                                vs.Clear();
                            }
                            break;
                        case "6":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W6 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W6);
                                vs.Clear();
                            }
                            break;
                        case "7":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W7 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W7);
                                vs.Clear();
                            }
                            break;
                        case "8":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W8 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W8);
                                vs.Clear();
                            }
                            break;
                    }

                }
                else if (FullLevelList[i].StartsWith("S"))
                {
                    string nextstring = "";
                    string newstring = FullLevelList[i].Substring(1, 1);
                    if (i != FullLevelList.Length - 1)
                    {
                        nextstring = FullLevelList[i + 1].Substring(1, 1);
                    }
                    switch (newstring)
                    {
                        case "1":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S1 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S1);
                                vs.Clear();
                            }
                            break;
                        case "2":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S2 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S2);
                                vs.Clear();
                            }
                            break;
                        case "3":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S3 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S3);
                                vs.Clear();
                            }
                            break;
                        case "4":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S4 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S4);
                                vs.Clear();
                            }
                            break;
                        case "5":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S5 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S5);
                                vs.Clear();
                            }
                            break;
                        case "6":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S6 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S6);
                                vs.Clear();
                            }
                            break;
                        case "7":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S7 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S7);
                                vs.Clear();
                            }
                            break;
                        case "8":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S8 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S8);
                                vs.Clear();
                            }
                            break;
                    }

                }
            }
            listBox1.Items.AddRange(WorldLevelDict[0].ToArray());
            comboBox2.SelectedIndex = 0;
        }
    

        private void FrmBgmMain_Load(object sender, EventArgs e)
        {
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(Properties.Settings.Default.UseLayeredFs ? Properties.Settings.Default.LayeredFSPath + "\\SoundData\\BgmTable.szs" : @"BgmTable.szs")));
            foreach (LibEveryFileExplorer.Files.SimpleFileSystem.SFSFile file in SzsArch.ToFileSystem().Files) SzsFiles.Add(file.FileName, file.Data);

            string ConvertedXml = BymlConverter.GetXml(SzsFiles["StageDefaultBgmList.byml"]);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(ConvertedXml);
            XmlNode nodes = xml.SelectSingleNode("/Root/C1/C0");
            foreach (XmlNode no in nodes.ChildNodes)
            {
                int scenario = 0;
                string name = "";
                string music = "";
                foreach (XmlNode n in no.ChildNodes)
                {
                    if (n.Attributes["Name"].Value == "Scenario") scenario = Int32.Parse(n.Attributes["StringValue"].Value);
                    else if (n.Attributes["Name"].Value == "StageName") name = n.Attributes["StringValue"].Value;
                    else if (n.Attributes["Name"].Value == "BgmLabel") music = n.Attributes["StringValue"].Value;
                }
                if (name != "SkyChikuwaSp2Stage")
                {
                    name = name + "Map" + scenario.ToString() + ".szs";
                }
                Levels.Add(name, music);
                if (!Music.Contains(music)) Music.Add(music);
            }

            //foreach (string k in LevelsNum.Keys.ToArray()) listBox1.Items.Add(k + " (" + LevelsNum[k] + ")");
            string[] Mus = new string [Properties.Settings.Default.Music.Count];//{ "STM_BGM_TRUCK_B_FAST", "STM_BGM_WORLD1_A", "STM_BGM_WORLD2_A" };
            Properties.Settings.Default.Music.CopyTo(Mus, 0);
            Music.AddRange(Mus);
            if (Properties.Settings.Default.CustomMusic != null)
            {
            string[] custmus = new string[Properties.Settings.Default.CustomMusic.Count];
                

            Properties.Settings.Default.CustomMusic.CopyTo(custmus, 0);

            Music.AddRange(custmus);
            }
            comboBox1.Items.AddRange(Music.ToArray());
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                comboBox1.Enabled = false;
                return;
            }
            comboBox1.Enabled = true;
            if (LevelsNum[WorldLevelDict[comboBox2.SelectedIndex][listBox1.SelectedIndex]].Equals("SkyChikuwaStageMap2.szs"))
            {
                comboBox1.Text = Levels["SkyChikuwaSp2Stage"];
            }
            else
            {
                comboBox1.Text = Levels[LevelsNum[WorldLevelDict[comboBox2.SelectedIndex][listBox1.SelectedIndex]]];
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                comboBox1.Enabled = false;
                return;
            }
            else
            {
                Levels[LevelsNum[WorldLevelDict[comboBox2.SelectedIndex][listBox1.SelectedIndex]]] = comboBox1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = Form1.DefEnc }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("Root");
                    xr.WriteStartElement("isBigEndian");
                    xr.WriteAttributeString("Value", "False");
                    xr.WriteEndElement();
                    xr.WriteStartElement("BymlFormatVersion");
                    xr.WriteAttributeString("Value", ((uint)1).ToString());
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1");
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", "StageDefaultBgmList");
                    foreach (string k in Levels.Keys.ToArray())
                    {
                        xr.WriteStartElement("C1");
                        xr.WriteStartElement("A0");
                        xr.WriteAttributeString("Name", "BgmLabel");
                        xr.WriteAttributeString("StringValue", Levels[k]);
                        if (k.Contains("SkyChikuwaSp2Stage"))
                        {
                            xr.WriteEndElement();
                            xr.WriteStartElement("D1");
                            xr.WriteAttributeString("Name", "Scenario");
                            xr.WriteAttributeString("StringValue", 1.ToString());
                            xr.WriteEndElement();
                            xr.WriteStartElement("A0");
                            xr.WriteAttributeString("Name", "StageName");
                            xr.WriteAttributeString("StringValue", "SkyChikuwaSp2Stage");
                            xr.WriteEndElement();
                            xr.WriteEndElement();
                        }
                        else if (k.Equals("SkyChikuwa"))
                        {
                            xr.WriteEndElement();
                            xr.WriteStartElement("D1");
                            xr.WriteAttributeString("Name", "Scenario");
                            xr.WriteAttributeString("StringValue", 1.ToString());
                            xr.WriteEndElement();
                            xr.WriteStartElement("A0");
                            xr.WriteAttributeString("Name", "StageName");
                            xr.WriteAttributeString("StringValue", "SkyChikuwaSp2Stage");
                            xr.WriteEndElement();
                            xr.WriteEndElement();
                        }
                        else if (k.Contains("SkyChikuwaStageMap2"))
                        {
                            xr.WriteEndElement();
                            xr.WriteStartElement("D1");
                            xr.WriteAttributeString("Name", "Scenario");
                            xr.WriteAttributeString("StringValue", 1.ToString());
                            xr.WriteEndElement();
                            xr.WriteStartElement("A0");
                            xr.WriteAttributeString("Name", "StageName");
                            xr.WriteAttributeString("StringValue", "SkyChikuwaSp2Stages");
                            xr.WriteEndElement();
                            xr.WriteEndElement();
                        }
                        else
                        {
                            xr.WriteEndElement();
                            xr.WriteStartElement("D1");
                            xr.WriteAttributeString("Name", "Scenario");
                            xr.WriteAttributeString("StringValue", k.Substring(k.Length - 5, 1));
                            xr.WriteEndElement();
                            xr.WriteStartElement("A0");
                            xr.WriteAttributeString("Name", "StageName");
                            xr.WriteAttributeString("StringValue", k.Substring(0, k.Length - 8));
                            xr.WriteEndElement();
                            xr.WriteEndElement();
                        }
                    }
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.Close();
                }
                Clipboard.SetText(Form1.DefEnc.GetString(stream.ToArray()));
                SzsFiles["StageDefaultBgmList.byml"] = BymlConverter.GetByml(Form1.DefEnc.GetString(stream.ToArray()));
            }
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);
            for (int i = 0; i < SzsFiles.Count; i++)
            {
                SFSFile file = new SFSFile(i, SzsFiles.Keys.ToArray()[i], dir);
                file.Data = SzsFiles.Values.ToArray()[i];
                dir.Files.Add(file);
            }
            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(Properties.Settings.Default.UseLayeredFs ? Properties.Settings.Default.LayeredFSPath + "\\SoundData\\BgmTable.szs" : @"BgmTable.szs", y.Compress(SzsArch.Write()));
            MessageBox.Show("File was saved properly in " + (Properties.Settings.Default.UseLayeredFs ? Properties.Settings.Default.LayeredFSPath + "\\SoundData\\." : "this program's folder."));
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(WorldLevelDict[comboBox2.SelectedIndex].ToArray());
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                Levels[LevelsNum[WorldLevelDict[comboBox2.SelectedIndex][listBox1.SelectedIndex]]] = comboBox1.Text;
            }
        }
    }
}
