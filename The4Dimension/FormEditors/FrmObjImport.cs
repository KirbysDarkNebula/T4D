﻿using _3DS.NintendoWare.GFX;
using CommonFiles;
using LibEveryFileExplorer.Files.SimpleFileSystem;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModelViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static The4Dimension.Ohana.RenderBase;

namespace The4Dimension.FormEditors
{
    public partial class FrmObjImport : Form
    {
        string ffile = "";
        string filename = "";
        string modelPath = "";
        string ObjModelPath = "";
        bool IsObj;
        public UserControl1 render = new UserControl1();
        string tmpPath = "";
        string KclPath = "";
        string PaPath = "";
        public FrmObjImport(string file = "")
        { 
            InitializeComponent();
            comboBox1.SelectedIndex = 5;
            elementHost1.Child = render;
            render.AddKey("Model");
            ffile = file;
        }

        private void FrmObjImport_Load(object sender, EventArgs e)
        {
            string opnfile;
            bool ok = true;
            if (ffile != "")
            {
                opnfile = ffile;
            }
            else
            {
                if (Directory.Exists(Path.GetTempPath() + "TmpT4D")) { try { Directory.Delete(Path.GetTempPath() + "TmpT4D", true); } catch { } }
                OpenFileDialog opn = new OpenFileDialog();
                opn.Title = "Open a model file";
                opn.Filter = "Supported formats |*.bcmdl; *.bcres; *.obj";
                
                if (opn.ShowDialog() != DialogResult.OK) ok = false;
                opnfile = opn.FileName;

            }
            if (Path.GetExtension(opnfile).ToLower() == ".obj")
            {
                if (MessageBox.Show("The obj will be converted to bcmdl with Every File Explorer's method, this is known to have problems, especially with models made in sketchup.\r\nDo you want to continue ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    modelPath = opnfile;
                    ObjModelPath = modelPath;
                    IsObj = true;
                    render.addModel(modelPath, "Model", new System.Windows.Media.Media3D.Vector3D(0, 0, 0), new System.Windows.Media.Media3D.Vector3D(1, 1, 1), 0, 0, 0);
                }
                else
                {
                    MessageBox.Show("You can convert the model to bcmdl with the leaked tools");
                    this.Close();
                }
            }
            else if (Path.GetExtension(opnfile).ToLower() == ".bcmdl")
            {
                tmpPath = Path.GetTempPath() + "TmpT4D";
                filename = Path.GetFileNameWithoutExtension(opnfile);
                string Name = tmpPath + "\\"+Path.GetFileNameWithoutExtension(opnfile) +".obj";
                Directory.CreateDirectory(tmpPath);
                CGFX mod = null;
                mod = new _3DS.NintendoWare.GFX.CGFX(File.ReadAllBytes(opnfile));
                CommonFiles.OBJ o = mod.Data.Models[0].ToOBJ();
                o.MTLPath = Path.GetFileNameWithoutExtension(Name) + ".mtl";
                MTL m = mod.Data.Models[0].ToMTL("Tex");
                byte[] d = o.Write();
                byte[] d2 = m.Write();
                File.Create(Name).Close();
                File.WriteAllBytes(Name, d);
                File.Create(Path.ChangeExtension(Name, "mtl")).Close();
                File.WriteAllBytes(Path.ChangeExtension(Name, "mtl"), d2);
                Directory.CreateDirectory(tmpPath + "\\Tex");
                if (mod.Data.Textures != null)
                {
                    foreach (var v in mod.Data.Textures)
                    {
                        if (!(v is ImageTextureCtr)) continue;
                        if (File.Exists(tmpPath + "\\Tex\\" + v.Name + ".png")) continue;
                        ((ImageTextureCtr)v).GetBitmap().Save(tmpPath + "\\Tex\\" + v.Name + ".png");
                    }
                }

                modelPath = opnfile;
                ObjModelPath = Name;
                IsObj = false;
                render.addModel(Name, "Model", new System.Windows.Media.Media3D.Vector3D(0, 0, 0), new System.Windows.Media.Media3D.Vector3D(1, 1, 1), 0, 0, 0);
                textBox1.Text = mod.Data.Models[0].Name;
                textBox1.Enabled = false;
                render.SetSortFrequency(0);
            }
            else if (Path.GetExtension(opnfile).ToLower() == ".bcres")
            {
                tmpPath = Path.GetTempPath() + "TmpT4D";
                filename = Path.GetFileNameWithoutExtension(opnfile);
                string Name = tmpPath + "\\" + Path.GetFileNameWithoutExtension(opnfile) + ".obj";
                Directory.CreateDirectory(tmpPath);
                CGFX mod = null;
                mod = new _3DS.NintendoWare.GFX.CGFX(File.ReadAllBytes(opnfile));
                CommonFiles.OBJ o = mod.Data.Models[0].ToOBJ();
                o.MTLPath = Path.GetFileNameWithoutExtension(Name) + ".mtl";
                MTL m = mod.Data.Models[0].ToMTL("Tex");
                byte[] d = o.Write();
                byte[] d2 = m.Write();
                File.Create(Name).Close();
                File.WriteAllBytes(Name, d);
                File.Create(Path.ChangeExtension(Name, "mtl")).Close();
                File.WriteAllBytes(Path.ChangeExtension(Name, "mtl"), d2);
                Directory.CreateDirectory(tmpPath + "\\Tex");
                if (mod.Data.Textures != null)
                {
                    foreach (var v in mod.Data.Textures)
                    {
                        if (!(v is ImageTextureCtr)) continue;
                        if (File.Exists(tmpPath + "\\Tex\\" + v.Name + ".png")) continue;
                        ((ImageTextureCtr)v).GetBitmap().Save(tmpPath + "\\Tex\\" + v.Name + ".png");
                    }
                }

                modelPath = opnfile;
                ObjModelPath = Name;
                IsObj = false;
                render.addModel(Name, "Model", new System.Windows.Media.Media3D.Vector3D(0, 0, 0), new System.Windows.Media.Media3D.Vector3D(1, 1, 1), 0, 0, 0);
                textBox1.Text = mod.Data.Models[0].Name;
                textBox1.Enabled = false;
                render.SetSortFrequency(0);
            }
            else
            {
                if (ok) MessageBox.Show("File not supported");
                else MessageBox.Show("You must select your model file to use this function");
                this.Close();
            }
        }

        private void f_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ObjModelPath == "") return;
            render.RemoveModel("Model", 0);
            render.Clear();
            elementHost1.Dispose();
            try { Directory.Delete(tmpPath, true); } catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Kcl file|*.kcl";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                KclPath = opn.FileName;
                label4.Text = Path.GetFileName(KclPath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Pa file|*.pa";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                PaPath = opn.FileName;
                label5.Text = Path.GetFileName(PaPath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists("CollisionsMng.exe"))
            {
                MessageBox.Show("CollisionsMng.exe not found!");
                return;
            }
            button2.Enabled = false;
            Process p = new Process();
            ProcessStartInfo s = new ProcessStartInfo();
            s.FileName = "CollisionsMng.exe";
            s.Arguments = (!ObjModelPath.StartsWith("\"")) ? "\"" + ObjModelPath + "\"" : ObjModelPath;
            p.StartInfo = s;
            p.Start();
            p.WaitForExit();
            if (File.Exists(ObjModelPath + ".kcl") && File.Exists(ObjModelPath + ".pa"))
            {
                PaPath = ObjModelPath + ".pa";
                label5.Text = "PA: " + textBox1.Text + ".pa";
                KclPath = ObjModelPath + ".kcl";
                label4.Text = "KCL: " + textBox1.Text + ".kcl";
                button7.Enabled = true;
            }
            else MessageBox.Show("Files not found, something went wrong!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(PaPath)) { MessageBox.Show("Pa file not found!"); return; }
            if (!File.Exists(KclPath)) { MessageBox.Show("Kcl file not found!"); return; }
            if (!File.Exists(modelPath)) { MessageBox.Show("Model file not found!"); return; }
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Szs files|*.szs";
            s.FileName = textBox1.Text;
            if (s.ShowDialog() != DialogResult.OK) return;
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);

            //Model
            SFSFile Model = new SFSFile(0, textBox1.Text + ".bcmdl", dir);
            if (IsObj)
            {
                CGFX mod = null;
                mod = new CGFX();
                CGFXGenerator.FromOBJ(mod, modelPath, textBox1.Text);
                Model.Data = mod.Write();
            }
            else Model.Data = File.ReadAllBytes(modelPath);
            dir.Files.Add(Model);
            //Collisions
            SFSFile KclFile = new SFSFile(1, textBox1.Text + ".kcl", dir);
            KclFile.Data = File.ReadAllBytes(KclPath);
            dir.Files.Add(KclFile);
            SFSFile PaFile = new SFSFile(2, textBox1.Text + ".pa", dir);
            PaFile.Data = File.ReadAllBytes(PaPath);
            dir.Files.Add(PaFile);
            //InitSensor
            SFSFile Sensor = new SFSFile(3, "InitSensor.byml", dir);
            Sensor.Data = BymlConverter.GetByml(Properties.Resources.Sensor);
            dir.Files.Add(Sensor);
            //InitActor
            string actor = "<?xml version =\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n    <C1 Name=\"Collision\">\r\n      <A0 Name=\"Sensor\" StringValue=\"Collision\" />\r\n    </C1>\r\n    <C1 Name=\"Debug\">\r\n      <A0 Name=\"NodeName\" StringValue=\"地形\" />\r\n    </C1>\r\n    <C1 Name=\"Executor\">\r\n      <A0 Name=\"CategoryName\" StringValue=\"" + comboBox1.SelectedItem + "\" />\r\n    </C1>\r\n    <C1 Name=\"GroupClipping\">\r\n      <D1 Name=\"MaxCount\" StringValue=\"64\" />\r\n    </C1>\r\n    <FF Name=\"Model\" StringValue=\"00000000\" />\r\n  </C1>\r\n</Root>";
            SFSFile Actor = new SFSFile(4, "InitActor.byml", dir);
            Actor.Data = BymlConverter.GetByml(actor);//Properties.Resources.Actor);
            dir.Files.Add(Actor);
            //InitClipping
            string clip = "<?xml version=\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n    <D2 Name=\"Radius\" StringValue=\"" + numericUpDown1.Value.ToString() + "\" />\r\n  </C1>\r\n</Root>";
            SFSFile Clipping = new SFSFile(5, "InitClipping.byml", dir);
            Clipping.Data = BymlConverter.GetByml(clip);
            dir.Files.Add(Clipping);

            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(s.FileName, y.Compress(SzsArch.Write()));
            MessageBox.Show("Done!");
            MessageBox.Show("Remember you need to add the object to the CreatorClassNameTable to use the object in-game (Other modding -> CreatorClassNameTable editor)");
            MessageBox.Show("To view the model in the editor you must copy it in the models folder with the name " + textBox1.Text + ".obj or else you will see a blue box");
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Resources.ExecName);
            MessageBox.Show("If unsure leave the default option (collision terrain/option no. 6)");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Szs files|*.szs";
            s.FileName = textBox1.Text;
            if (s.ShowDialog() != DialogResult.OK) return;
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);

            //Model
            SFSFile Model = new SFSFile(0, textBox1.Text + ".bcmdl", dir);
            if (IsObj)
            {
                CGFX mod = null;
                mod = new CGFX();
                CGFXGenerator.FromOBJ(mod, modelPath, textBox1.Text);
                Model.Data = mod.Write();
            }
            else Model.Data = File.ReadAllBytes(modelPath);
            dir.Files.Add(Model);
            //Collisions
            /*SFSFile KclFile = new SFSFile(1, textBox1.Text + ".kcl", dir);
            KclFile.Data = File.ReadAllBytes(KclPath);
            dir.Files.Add(KclFile);
            SFSFile PaFile = new SFSFile(2, textBox1.Text + ".pa", dir);
            PaFile.Data = File.ReadAllBytes(PaPath);
            dir.Files.Add(PaFile);*/
            //InitSensor
            SFSFile Sensor = new SFSFile(3, "InitSensor.byml", dir);
            Sensor.Data = BymlConverter.GetByml(Properties.Resources.SensorNPC);
            dir.Files.Add(Sensor);
            //InitActor
            string actor = "<?xml version =\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n        <C1 Name=\"Debug\">\r\n      <A0 Name=\"NodeName\" StringValue=\"地形\" />\r\n    </C1>\r\n    <C1 Name=\"Executor\">\r\n      <A0 Name=\"CategoryName\" StringValue=\"" + comboBox1.SelectedItem + "\" />\r\n    </C1>\r\n    <C1 Name=\"GroupClipping\">\r\n      <D1 Name=\"MaxCount\" StringValue=\"64\" />\r\n    </C1>\r\n    <FF Name=\"Model\" StringValue=\"00000000\" />\r\n  </C1>\r\n</Root>";
            SFSFile Actor = new SFSFile(4, "InitActor.byml", dir);
            Actor.Data = BymlConverter.GetByml(actor);//Properties.Resources.Actor);
            dir.Files.Add(Actor);
            //InitClipping
            string clip = "<?xml version=\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n    <D2 Name=\"Radius\" StringValue=\"" + numericUpDown1.Value.ToString() + "\" />\r\n  </C1>\r\n</Root>";
            SFSFile Clipping = new SFSFile(5, "InitClipping.byml", dir);
            Clipping.Data = BymlConverter.GetByml(clip);
            dir.Files.Add(Clipping);

            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(s.FileName, y.Compress(SzsArch.Write()));
            MessageBox.Show("Done!");
            MessageBox.Show("Remember you need to add the object to the CreatorClassNameTable to use the object in-game (Other modding -> CreatorClassNameTable editor)");
            MessageBox.Show("To view the model in the editor you must copy it in the models folder with the name " + textBox1.Text + ".obj or else you will see a blue box");
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog fld = new CommonOpenFileDialog { IsFolderPicker = true };
            fld.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (fld.ShowDialog() != CommonFileDialogResult.Ok) return;
            if (File.Exists(fld.FileName + "\\"+ textBox1.Text + ".pa")!=true)
            { 
            File.Copy(PaPath, fld.FileName+"\\"+ textBox1.Text + ".pa");
            File.Copy(KclPath, fld.FileName+ "\\"+ textBox1.Text +".kcl");
            }
            else 
            {
                File.Delete(fld.FileName + "\\"+ textBox1.Text + ".pa");
                File.Delete(fld.FileName + "\\"+ textBox1.Text+".kcl");
                File.Copy(PaPath, fld.FileName + "\\"+ textBox1.Text+".pa");
                File.Copy(KclPath, fld.FileName + "\\"+ textBox1.Text + ".kcl");
            }
            MessageBox.Show("The kcl and pa files were saved in:\r\n" + fld.FileName);
        }
    }
}