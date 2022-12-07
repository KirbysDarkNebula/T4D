using ModelViewer;
using HelixToolkit;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.ListBox;

namespace ModelViewer
{
    public partial class UserControl1 : UserControl
    {
        Dictionary<string, Model3D> ImportedModels = new Dictionary<string, Model3D>();
        Dictionary<string, List<ModelVisual3D>> Models = new Dictionary<string, List<ModelVisual3D>>();
        public Dictionary<string, List<Vector3D>> Positions = new Dictionary<string, List<Vector3D>>();
        Dictionary<string, ModelVisual3D> AxisDict = new Dictionary<string, ModelVisual3D>();
        ModelImporter Importer = new ModelImporter();
        SortingVisual3D ModelViewer = new SortingVisual3D();
        System.Windows.Media.Media3D.PerspectiveCamera fovycam = new System.Windows.Media.Media3D.PerspectiveCamera();
        Vector3D CameraTarget = new Vector3D(0, 0, 0);
        bool IsPreview = false;
        string CameraType = "";
        public bool BackfaceCull = false;
        public double CameraInertiaFactor
        {
            get { return ModelView.CameraInertiaFactor; }
            set { ModelView.CameraInertiaFactor = value; }
        }

        public bool ShowFps
        {
            get { return ModelView.ShowFrameRate; }
            set { ModelView.ShowFrameRate = value; }
        }
        public bool HasAA
        {
            get { if (RenderOptions.EdgeModeProperty.ToString() == "Unspecified") return true; else return false; }
            set { if (value == true) RenderOptions.SetEdgeMode(ModelView,(EdgeMode)0); else RenderOptions.SetEdgeMode(ModelView, (EdgeMode)1); }
        }

        public int TextureFilter
        {
            get {return (int)RenderOptions.GetBitmapScalingMode(ModelView);}
            set { RenderOptions.SetBitmapScalingMode(ModelView, (BitmapScalingMode)value+1); string desc =  RenderOptions.GetBitmapScalingMode(ModelView).ToString(); } 
        }

        public bool ShowTriangleCount
        {
            get { return ModelView.ShowTriangleCountInfo; }
            set { ModelView.ShowTriangleCountInfo = value; }
        }

        public bool ShowDebugInfo
        {
            get { return ModelView.ShowCameraInfo; }
            set { ModelView.ShowCameraInfo = value; }
        }

        public HelixToolkit.Wpf.CameraMode CamMode
        {
            get { return ModelView.CameraMode; }
            set { ModelView.CameraMode = value; }
        }

        public double ZoomSensitivity
        {
            get { return ModelView.ZoomSensitivity; }
            set { ModelView.ZoomSensitivity = value; }
        }

        public double RotationSensitivity
        {
            get { return ModelView.RotationSensitivity; }
            set { ModelView.RotationSensitivity = value; }
        }
        private double CameraTime;

        public double Camspeed
        {
            get { return CameraTime; }
            set { CameraTime = value; }

        }
        private double CameraDist;
        public double CamDistance
        {
            get { return CameraDist; }
            set { CameraDist = value; }

        }

        public bool AutoCam = true;

        void test()
        {
        }

        public UserControl1()
        {
            InitializeComponent();
            ModelView.Camera.NearPlaneDistance = 80;
            ModelViewer.SortingFrequency = 0.5;
            ModelView.Children.Add(ModelViewer);
            ModelViewer.CheckForOpaqueVisuals = true;
            ModelViewer.Method = SortingMethod.BoundingBoxCenter;
            //ModelView.
        }


        public void Clone(UserControl1 old)
        {
            BackfaceCull = old.BackfaceCull;
            ModelView.ShowViewCube = false;
            ImportedModels = new Dictionary<string, Model3D>();
            foreach (string s in old.ImportedModels.Keys)
            {
                ImportedModels.Add(s, old.ImportedModels[s].Clone());
            }

            Models = new Dictionary<string, List<ModelVisual3D>>();

            foreach (string s in old.Models.Keys)
            {
                if (s == "SelectionLayer") continue;
                List<ModelVisual3D> tempmodellist = new List<ModelVisual3D>();
                foreach (ModelVisual3D model in old.Models[s])
                {
                    tempmodellist.Add(new ModelVisual3D());
                }
                Models.Add(s, tempmodellist);
                for (int i = 0; i< old.Models[s].Count; i++)
                {
                    List<GeometryModel3D> NewModel = new List<GeometryModel3D>();
                    GeometryModel3D gmod = old.Models[s][i].Content.Clone() as GeometryModel3D;
                    if (gmod == null)
                    {
                        Model3DGroup group3d = new Model3DGroup();
                        foreach (GeometryModel3D gd in (old.Models[s][i].Content.Clone() as Model3DGroup).Children)
                        {
                            gmod = new GeometryModel3D();
                            gmod.Geometry = gd.Geometry.Clone();
                            gmod.Material = gd.Material.Clone();
                            if (!BackfaceCull)
                            {
                                gmod.BackMaterial = gd.Material.Clone();
                            }
                            gmod.Transform = gd.Transform;
                            group3d.Children.Add(gmod);
                        }
                        Models[s][i].Content = group3d;
                        //Models[s][i].Transform = old.Models[s][i].Transform;
                        Transform3DGroup t = new Transform3DGroup();
                        t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90))); // for some reason the object rotates 90 degrees back
                        t.Children.Add(old.Models[s][i].Transform);
                        Models[s][i].Transform = t;
                    }
                    else
                    {
                        NewModel.Add(gmod);
                        Model3DGroup group3d = new Model3DGroup();
                        foreach (GeometryModel3D gd in NewModel)
                        {
                            group3d.Children.Add(gd);
                        }
                        Models[s][i].Content = group3d;
                        Models[s][i].Transform = old.Models[s][i].Transform.Clone();
                    }
                }

            }
            foreach (string s in old.Positions.Keys)
            {
                List<Vector3D> tempvectorlist = new List<Vector3D>();
                foreach (Vector3D vect in old.Positions[s])
                {
                    tempvectorlist.Add( new Vector3D(vect.X, vect.Y, vect.Z));
                }
                Positions.Add(s, tempvectorlist);
            }
            //Importer
            if (Models.ContainsKey("ObjInfo")) foreach (ModelVisual3D model in Models["ObjInfo"]) ModelViewer.Children.Add(model);
            if (Models.ContainsKey("DemoSceneObjInfo")) foreach (ModelVisual3D model in Models["DemoSceneObjInfo"]) ModelViewer.Children.Add(model);
            if (Models.ContainsKey("GoalObjInfo")) foreach (ModelVisual3D model in Models["GoalObjInfo"]) ModelViewer.Children.Add(model);    
            IsPreview = true;
            ModelView.UpdateLayout(); 
            //ModelView.
        }

        public void Clear()
        {
            ImportedModels = null;
            Models = null;
            Positions = null;
            Importer = null;
            ModelView = null;
        }

        public void SetSortFrequency(double t)
        {
            ModelViewer.SortingFrequency = t;
        }

        public void AddKey(string Type)
        {
            if (!Models.ContainsKey(Type)) Models.Add(Type, new List<ModelVisual3D>());
            if (!Positions.ContainsKey(Type)) Positions.Add(Type, new List<Vector3D>());
        }

        public void AddTmpObjects(List<Vector3D> positions, List<Vector3D> scale, List<Vector3D> rot, List<string> Paths,string type)
        {
            if (Models[type].Count != 0) ClearTmpObjects();
            for (int i = 0; i < positions.Count; i++) addModel(Paths[i], type, positions[i], scale[i], (float)rot[i].X, (float)rot[i].Y, (float)rot[i].Z);
        }

        public void ClearTmpObjects(bool updatelayout = true)
        {
            ClearType("TmpChildrenObjs");
            ClearType("TmpAreaChildrenObjs");
            if (updatelayout) ModelView.UpdateLayout();
        }

        public void ClearC0Objects()
        {
            ClearType("C0EditingListObjs");
            ModelView.UpdateLayout();
        }

        void ClearType(string type)
        {
            while (Models[type].Count != 0) RemoveModel(type, 0);
        }

        public void addRail(List<Point3D[]> Points, bool Closed = false, int Thickness = 5, int at = -1)
        {
            string Type = "AllRailInfos";
            LinesVisual3D l = new LinesVisual3D();
            List<Point3D> BezierCurve = new List<Point3D>();
            for (int i = 0; i < Points.Count; i++)
            {
                Single t = 0;
                int CurrentPoint = i; // for this we will use this point[0] and the control for next curve [2]
                int NextPoint = i + 1 < Points.Count ? i + 1 : 0; // for this we will use this point[0] and the control for prev curve [1]
                int PreviousPoint = i - 1 < 0 ? Points.Count - 1 : i - 1;

                if (i == Points.Count - 1 && !Closed)
                {
                    BezierCurve.Add(Points[i][0]);
                }
                else
                {
                    if ((Points[CurrentPoint][0] != Points[CurrentPoint][1] || Points[CurrentPoint][0] != Points[CurrentPoint][2]) || (Points[PreviousPoint][0] != Points[PreviousPoint][1] || Points[PreviousPoint][0] != Points[PreviousPoint][2]) || (Points[NextPoint][0] != Points[NextPoint][1] || Points[NextPoint][0] != Points[NextPoint][2]))
                        while (t < 1)
                        {
                            Point3D P0 = Points[CurrentPoint][0];
                            Point3D P1 = Points[CurrentPoint][2];
                            Point3D P2 = Points[NextPoint][1];
                            Point3D P3 = Points[NextPoint][0];
                            P0 = new Point3D(Math.Pow((1 - t), 3) * P0.X, Math.Pow((1 - t), 3) * P0.Y, Math.Pow((1 - t), 3) * P0.Z);
                            P1 = new Point3D(3 * Math.Pow((1 - t), 2) * t * P1.X, (Math.Pow((1 - t), 2) * 3 * t * P1.Y), Math.Pow((1 - t), 2) * 3 * t * P1.Z);
                            P2 = new Point3D(3 * (1 - t) * Math.Pow(t, 2) * P2.X, (3 * (1 - t) * Math.Pow(t, 2) * P2.Y), 3 * (1 - t) * Math.Pow(t, 2) * P2.Z);
                            P3 = new Point3D(Math.Pow(t, 3) * P3.X, (Math.Pow(t, 3) * P3.Y), Math.Pow(t, 3) * P3.Z);
                            Point3D bPoint = P0 + P1.ToVector3D() + P2.ToVector3D() + P3.ToVector3D();
                            BezierCurve.Add(bPoint);
                            t += (float)0.1;
                        }
                    else
                    {
                        BezierCurve.Add(Points[i][0]);
                    }
                }
            }

            if (at == -1) Models[Type].Add(l); else Models[Type].Insert(at, l);
            if (at == -1) Positions[Type].Add(BezierCurve[0].ToVector3D()); else Positions[Type].Insert(at, BezierCurve[0].ToVector3D());
            if (at == -1) ModelViewer.Children.Add(Models[Type][Models[Type].Count - 1]); else ModelViewer.Children.Insert(at, Models[Type][at]);
            if (BezierCurve.Count < 2) return;
            l.Color = Color.FromRgb(255, 255, 255);
            l.Thickness = Thickness;
            AddRailpoints(l, Closed, BezierCurve, Thickness);
        }
        public void addAxis(Point3D Point, string axis)
        {
            Point3D point2 = new Point3D(Point.X,Point.Y,Point.Z);
            LinesVisual3D l = new LinesVisual3D();
            l.Color = Color.FromRgb(0,0,0);
            switch (axis) 
            {
                case "X":
                    point2.X += 5000;
                    Point.X -= 5000;
                    l.Color = Color.FromRgb(255,0,0);
                    break;
                case "Y":
                    point2.Z += 5000;
                    Point.Z -= 5000;
                    l.Color = Color.FromRgb(0, 0, 255);
                    break;
                case "Z":
                    point2.Y += 5000;
                    Point.Y -= 5000;
                    l.Color = Color.FromRgb(0, 255, 0);
                    break;
            }
            Point3D[] Points = new Point3D[2] {Point,point2 };
            int at = -1;
            string Type = "Axis";
            l.SetName(axis);
            AxisDict.Add(axis, l);
            if (at == -1) Models[Type].Add(l); else Models[Type].Insert(at, l);
            if (at == -1) Positions[Type].Add(Points[0].ToVector3D()); else Positions[Type].Insert(at, Points[0].ToVector3D());
            if (at == -1) ModelViewer.Children.Add(Models[Type][Models[Type].Count - 1]); else ModelViewer.Children.Insert(at, Models[Type][at]);
            if (Points.Length < 2) return;
            l.Thickness = 2.9;
            AddRailpoints(l, false, Points.ToList(), 5);
        }
        public void removeAxis(string axis = "All")
        {

            if (axis == "All" && AxisDict.ContainsKey("X"))
            {
                RemoveModel("Axis", Models["Axis"].IndexOf(AxisDict["X"]));
                AxisDict.Remove("X");
                RemoveModel("Axis", Models["Axis"].IndexOf(AxisDict["Y"]));
                AxisDict.Remove("Y");
                RemoveModel("Axis", Models["Axis"].IndexOf(AxisDict["Z"]));
                AxisDict.Remove("Z");
            }
            else if (axis != "All" && AxisDict.ContainsKey(axis)) 
            {
                RemoveModel("Axis", Models["Axis"].IndexOf(AxisDict[axis]));
                AxisDict.Remove(axis);
            }

            ModelView.UpdateLayout();
            //can use X Y and Z

        }
        public void SelectRail(List<Point3D[]> Points)
        {
            UnselectRail();
            List<Point3D[]> BezierList = new List<Point3D[]>();
            List<Point3D> BezierCurve = new List<Point3D>();
            List<Point3D> NPoints = new List<Point3D>();
            for (int i = 0; i < Points.Count; i++)
            {
                NPoints.Add(Points[i][0]);
                NPoints.Add(Points[i][1]);
                NPoints.Add(Points[i][2]);

            }

            foreach (Point3D[] p in Points)
            {
                addModel(@"models\Point.obj", "SelectedRail", p[0].ToVector3D(), new Vector3D(.5f, .5f, .5f), 0, 0, 0);
                if (Points.Count > 1 && (Points[0] != Points[1] || Points[0] != Points[2])) 
                {
                    LinesVisual3D l = new LinesVisual3D();

                    l.Color = Color.FromRgb(209, 209, 209);
                    l.Thickness = 2;
                    List<Point3D> handle = new List<Point3D>() { p[1], p[0], p[2]};
                    l.Points.Add(p[0]);
                    Models["RailHandleRail"].Add(l);
                    Positions["RailHandleRail"].Add(p[0].ToVector3D());
                    ModelViewer.Children.Add(Models["RailHandleRail"][Models["RailHandleRail"].Count - 1]);
                    AddRailpoints(l, false, handle, 2);

                }
                //new linevisual3d that goes from railHandle0 to point to railhandle1
                addModel(@"models\PntHandle.obj", "RailHandle", p[1].ToVector3D(), new Vector3D(.5f, .5f, .5f), 0, 0, 0);
                addModel(@"models\PntHandle.obj", "RailHandle", p[2].ToVector3D(), new Vector3D(.5f, .5f, .5f), 0, 0, 0);
            }
        }

        public void UnselectRail()
        {

            ClearType("SelectedRail");
            ClearType("RailHandle");
            ClearType("RailHandleRail");
            ModelView.UpdateLayout();
        }

        public void AddRailpoints(LinesVisual3D l, bool Closed, List<Point3D[]> Points, int Thickness)
        {
            Point3D oldPoint = Points[1][0];
            l.Points.Add(Points[0][0]);
            l.Points.Add(Points[1][0]);
            for (int i = 2; i < Points.Count; i++)
            {
                int chidIndex = l.Children.Count;
                l.Children.Add(new LinesVisual3D());
                ((LinesVisual3D)l.Children[chidIndex]).Color = Color.FromRgb(47, 47, 47);
                ((LinesVisual3D)l.Children[chidIndex]).Thickness = Thickness;
                ((LinesVisual3D)l.Children[chidIndex]).Points.Add(oldPoint);
                ((LinesVisual3D)l.Children[chidIndex]).Points.Add(Points[i][0]);
                oldPoint = Points[i][0];
                if (Closed && i == Points.Count - 1)
                {
                    chidIndex = l.Children.Count;
                    l.Children.Add(new LinesVisual3D());
                    ((LinesVisual3D)l.Children[chidIndex]).Color = Color.FromRgb(245, 245, 245);
                    ((LinesVisual3D)l.Children[chidIndex]).Thickness = Thickness-1;
                    ((LinesVisual3D)l.Children[chidIndex]).Points.Add(oldPoint);
                    ((LinesVisual3D)l.Children[chidIndex]).Points.Add(Points[0][0]);
                    oldPoint = Points[i][0];
                }
            }
        }

        public void AddRailpoints(LinesVisual3D l, bool Closed, List<Point3D> Points, int Thickness)
        {
            Point3D oldPoint = Points[1];
            l.Points.Add(Points[0]);
            l.Points.Add(Points[1]);
            for (int i = 2; i < Points.Count; i++)
            {
                int chidIndex = l.Children.Count;
                l.Children.Add(new LinesVisual3D());
                ((LinesVisual3D)l.Children[chidIndex]).Color = l.Color;
                ((LinesVisual3D)l.Children[chidIndex]).Thickness = Thickness;
                ((LinesVisual3D)l.Children[chidIndex]).Points.Add(oldPoint);
                ((LinesVisual3D)l.Children[chidIndex]).Points.Add(Points[i]);
                oldPoint = Points[i];
                if (Closed && i == Points.Count - 1)
                {
                    chidIndex = l.Children.Count;
                    l.Children.Add(new LinesVisual3D());
                    ((LinesVisual3D)l.Children[chidIndex]).Color = l.Color;
                    ((LinesVisual3D)l.Children[chidIndex]).Thickness = Thickness;
                    ((LinesVisual3D)l.Children[chidIndex]).Points.Add(oldPoint);
                    ((LinesVisual3D)l.Children[chidIndex]).Points.Add(Points[0]);
                    oldPoint = Points[i];
                }
            }
        }

        public void UpdateRailpos(int id, List<Point3D[]> Points, bool Closed)
        {
            RemoveRailPoints(((LinesVisual3D)Models["AllRailInfos"][id]));
            if (Points.Count < 2) return;
            List<Point3D> BezierCurve = new List<Point3D>();
            for (int i = 0; i < Points.Count; i++)
            {
                Single t = 0;
                int CurrentPoint = i; // for this we will use this point[0] and the control for next curve [2]
                int NextPoint = i + 1 < Points.Count ? i + 1 : 0; // for this we will use this point[0] and the control for prev curve [1]
                int PreviousPoint = i - 1 < 0 ? Points.Count - 1 : i - 1;

                if (i == Points.Count-1 && !Closed)
                {
                    BezierCurve.Add(Points[i][0]);
                }
                else
                {
                    if ((Points[CurrentPoint][0] != Points[CurrentPoint][1] || Points[CurrentPoint][0] != Points[CurrentPoint][2]) || (Points[PreviousPoint][0] != Points[PreviousPoint][1] || Points[PreviousPoint][0] != Points[PreviousPoint][2]) || (Points[NextPoint][0] != Points[NextPoint][1] || Points[NextPoint][0] != Points[NextPoint][2]))
                        while (t < 1)
                        {
                            Point3D P0 = Points[CurrentPoint][0];
                            Point3D P1 = Points[CurrentPoint][2];
                            Point3D P2 = Points[NextPoint][1];
                            Point3D P3 = Points[NextPoint][0];
                            P0 = new Point3D(Math.Pow((1 - t), 3) * P0.X, Math.Pow((1 - t), 3) * P0.Y, Math.Pow((1 - t), 3) * P0.Z);
                            P1 = new Point3D(3 * Math.Pow((1 - t), 2) * t * P1.X, (Math.Pow((1 - t), 2) * 3 * t * P1.Y), Math.Pow((1 - t), 2) * 3 * t * P1.Z);
                            P2 = new Point3D(3 * (1 - t) * Math.Pow(t, 2) * P2.X, (3 * (1 - t) * Math.Pow(t, 2) * P2.Y), 3 * (1 - t) * Math.Pow(t, 2) * P2.Z);
                            P3 = new Point3D(Math.Pow(t, 3) * P3.X, (Math.Pow(t, 3) * P3.Y), Math.Pow(t, 3) * P3.Z);
                            Point3D bPoint = P0 + P1.ToVector3D() + P2.ToVector3D() + P3.ToVector3D();
                            BezierCurve.Add(bPoint);
                            t += (float)0.1;
                        }
                    else
                    {
                        BezierCurve.Add(Points[i][0]);
                    }
                }
            }

            AddRailpoints((LinesVisual3D)Models["AllRailInfos"][id], Closed, BezierCurve, 5);
            Positions["AllRailInfos"][id] = Points[0][0].ToVector3D();
            ModelView.UpdateLayout();
        }

        public void addModel(string path, string Type, Vector3D pos, Vector3D scale, Single RotX, Single RotY, Single RotZ, int at = -1)
        {           
            if (at == -1) Models[Type].Add(new ModelVisual3D()); else Models[Type].Insert(at,new ModelVisual3D());
            if (at == -1) ModelViewer.Children.Add(Models[Type][Models[Type].Count-1]); else ModelViewer.Children.Insert(at,Models[Type][at]);
            Model3D Model;
            List<GeometryModel3D> NewModel = new List<GeometryModel3D>();
            GeometryGroup ModelG = new GeometryGroup();
            if (!ImportedModels.ContainsKey(path))
            {
                Model = Importer.Load(path);
                foreach (Model3D mod in Importer.Load(path).Children)
                {
                    var gmod = mod as GeometryModel3D;
                    if (BackfaceCull)
                    {
                        DiffuseMaterial a = new DiffuseMaterial();
                        a.Color = Color.FromArgb(255, 2, 2, 20);
                        gmod.BackMaterial = a;
                    }
                    NewModel.Add(gmod);
                }
                Model3DGroup group3d = new Model3DGroup();
                foreach (GeometryModel3D gd in NewModel)
                {
                    group3d.Children.Add(gd);
                }
                Model = group3d;
                ImportedModels.Add(path, Model);
            }
            else Model = ImportedModels[path];
            Model.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));
            Models[Type][at == -1 ? Models[Type].Count - 1: at].Content = Model;
            Transform3DGroup t = new Transform3DGroup();
            t.Children.Add(new ScaleTransform3D(scale));
            t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), RotX)));
            t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), RotY)));
            t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), RotZ)));
            t.Children.Add(new TranslateTransform3D(pos));
            if (at == -1) Positions[Type].Add(pos); else Positions[Type].Insert(at, pos);
            Models[Type][at == -1 ? Models[Type].Count - 1 : at].Transform = t;
        }

        public void RemoveModel(string Type, int index)
        {
            ModelViewer.Children.Remove(Models[Type][index]);
            Models[Type][index].Content = null;
            if (Type == "AllRailInfos")
            {
                RemoveRailPoints(((LinesVisual3D)Models[Type][index]));
            }
            Models[Type].RemoveAt(index);
            if (Type != "SelectionLayer") Positions[Type].RemoveAt(index);
            ModelView.UpdateLayout();
        }

        public void RemoveRailPoints(LinesVisual3D rail)
        {
            foreach (LinesVisual3D r in rail.Children) RemoveRailPoints(r);
            for (int i = 0; i< rail.Children.Count; i++)
            {
                rail.Children.RemoveAt(i);
            }
            rail.Points.Clear();
        }

        public void HideGroup(string Type)
        {
            for (int i = 0; i < Positions[Type].Count; i++)
            {
                ChangeTransform(Type, i, Positions[Type][i], new Vector3D(0, 0, 0), 0, 0, 0, false);
            }
        }

        public void HideScenario(string Layer)
        {
            for (int i = 0; i < Positions[Layer].Count; i++)
            {
                ChangeTransform(Layer, i, Positions[Layer][i], new Vector3D(0, 0, 0), 0, 0, 0, false);
            }
        }

        public void LookAt(Vector3D p)
        {
            ModelView.Camera.LookAt(p.ToPoint3D(), CameraDist, CameraTime);
            CameraTarget = p;
        }

        public void CameraToObj(string Type, int index)
        {
            if (Positions[Type].Count <= index) return;
            Vector3D pos = Positions[Type][index];
            Point3D point3 = new Point3D(Math.Truncate(pos.X * 100) / 100, Math.Truncate(pos.Y * 100) / 100, Math.Truncate(pos.Z * 100) / 100);
            
            if (Type == "AllRailInfos" && (Models["RailHandleRail"].Count == 0 || Models["SelectedRail"].Count == 1))
            {
                Type = "SelectedRail";
                index = 0;
            }
            point3 = Models[Type][index].FindBounds(Transform3D.Identity).Location;
            point3.X = point3.X + Models[Type][index].FindBounds(Transform3D.Identity).SizeX / 2;
            point3.Y = point3.Y + Models[Type][index].FindBounds(Transform3D.Identity).SizeY / 2;
            point3.Z = point3.Z + Models[Type][index].FindBounds(Transform3D.Identity).SizeZ / 2;
            if (AutoCam)
            {
                CameraDist = Math.Sqrt(Math.Pow(Models[Type][index].FindBounds(Transform3D.Identity).SizeX, 2) + Math.Pow(Models[Type][index].FindBounds(Transform3D.Identity).SizeY, 2) + Math.Pow(Models[Type][index].FindBounds(Transform3D.Identity).SizeZ, 2));
                if (CameraDist < 10)
                {
                    CameraDist = 10;
                }
                if (CameraDist < 400)
                {
                    CameraDist *= 5;
                }
                else if (CameraDist < 1000)
                {
                    CameraDist *= 3;
                }
                else if (CameraDist < 5000)
                {
                    CameraDist *= 2;
                }
                else if (CameraDist < 6000)
                {
                    CameraDist *= 1.5;
                }
                else if (CameraDist < 12000)
                {
                    CameraDist /= 2;
                }
                else
                {
                    CameraDist /= 4;
                }

                double percent = ActualWidth / 506;
                CameraDist *= percent;
                CameraTime = 200;
            }
            ModelView.Camera.LookAt(point3, CameraDist, CameraTime);
            Vector3D vec3 = new Vector3D(Math.Truncate(pos.X * 100) / 100, Math.Truncate(pos.Y * 100) / 100, Math.Truncate(pos.Z * 100) / 100);
            CameraTarget = vec3;
        }

        public void CameraToPoint()
        {

        }

        public void CameraFrom3DLCamera(Dictionary<string, object> properties)
        {
            //render the current scene, position the camera the same as it would be from the params, add buttons like the ones ingame
            double newX = 0;
            double newY = 0;
            double newZ = 0;
            double FOV = 45;
            if (properties.ContainsKey("Distance")) CameraDist = (Single)properties["Distance"]; //distance *from player*
            if (properties.ContainsKey("UpOffset")) newZ = (Single)properties["UpOffset"];
            if (properties.ContainsKey("VisionParam"))
            {
                if (((Dictionary<string, object>)properties["VisionParam"]).ContainsKey("FovyDegree"))
                {
                    //49,51753
                    //37,8493
                    //15
                    //6,5 => 11
                    //10 => 17
                    //16 => 28
                    //28 => 45
                    //30 => 48
                    //35 => 55
                    //40 => 63
                    //45 => 70
                    //54 => 81
                    //60 => 88
                    //75 => 104
                    //90 => 118
                    //-0.0052x^{2}+1.78x-0.168 where x is the game FovyDegree param
                    FOV = (Single)((Dictionary<string, object>)properties["VisionParam"])["FovyDegree"];
                    FOV = -0.0052 * Math.Pow(FOV, 2) + 1.78 * FOV - 0.168;
                }
                
            }
            double AngleV = 0;
            double AngleH = 0;

            if (properties.ContainsKey("AngleV")) { AngleV = (Single)properties["AngleV"]; }//trigonometry stuff to get the new camera position from the angle
            if (properties.ContainsKey("AngleH")) { AngleH = -(Single)properties["AngleH"]; }
            AngleH = AngleH / 180 * Math.PI;
            AngleV = AngleV / 180 * Math.PI;

            //render mario at StartInfo[0] position (can be changed later within the same window)


            if (properties.ContainsKey("Class"))
            {
                string type = (string)properties["Class"];
                CameraType = type;
                switch (type)
                {
                    case "Parallel":
                        if (true == true)
                        {
                            Vector3D MarioPos = Positions["StartInfo"][0];

                            ModelView.CameraController.ActualCamera.Position = MarioPos.ToPoint3D();//new Point3D(ModelView.CameraController.ActualCamera.Position.X, ModelView.CameraController.ActualCamera.Position.Y, newZ);

                            ModelView.CameraController.CameraUpDirection = new Vector3D(0, 0, 1);

                            ModelView.CameraController.ChangeDirection(new Vector3D( Math.Sin(AngleH) * Math.Cos(AngleV), Math.Cos(AngleH) * Math.Cos(AngleV), -Math.Sin(AngleV)), 100);

                            LookAt(MarioPos);


                            Vector3D CamPos = new Vector3D(ModelView.CameraController.ActualCamera.Position.X + newX, ModelView.CameraController.ActualCamera.Position.Y + newY, ModelView.CameraController.ActualCamera.Position.Z + newZ);


                            fovycam = (System.Windows.Media.Media3D.PerspectiveCamera)ModelView.Camera;
                            fovycam.FieldOfView = FOV;
                            ModelView.Camera = fovycam;
                            ModelView.CameraController.MaximumFieldOfView = FOV;
                            ModelView.CameraController.MinimumFieldOfView = FOV;
                            ModelView.CameraController.ActualCamera.Position = CamPos.ToPoint3D();//new Point3D(ModelView.CameraController.ActualCamera.Position.X, ModelView.CameraController.ActualCamera.Position.Y, newZ);

                        }
                        break;
                    case "Tower":
                        if (properties.ContainsKey("Position"))
                        {
                            //apparently this camera type doesn't really care about Y value 
                            //get the look at position (rotation center), the mario position will be the camera position, direction will be vector between Center to Mario (or backwards)
                            Vector3D MarioPos = Positions["StartInfo"][0];
                            Vector3D RotCenter = new Vector3D((Single)((Dictionary<string, object>)properties["Position"])["X"], -(Single)((Dictionary<string, object>)properties["Position"])["Z"], 0); //(Single)((Dictionary<string, object>)properties["Position"])["Y"]); ignores Y value for Position?
                            Vector3D ResultVec = (RotCenter.Length < MarioPos.Length) ? Vector3D.Subtract(RotCenter, MarioPos) : Vector3D.Subtract(MarioPos, RotCenter);

                            if (ResultVec.X == 0 && ResultVec.Y == 0 && ResultVec.Z != 0)
                            {
                                ModelView.CameraController.CameraUpDirection = new Vector3D(0, 1, 0);
                            }
                            else
                            {
                                ModelView.CameraController.CameraUpDirection = new Vector3D(0, 0, 1);
                            }
                            ModelView.CameraController.ChangeDirection(ResultVec, 100);
                            ModelView.CameraController.FixedRotationPoint = RotCenter.ToPoint3D();
                            CameraDist += ((Positions.ContainsKey("StartInfo")) ? (Math.Sqrt(Math.Pow(ResultVec.X, 2) + Math.Pow(ResultVec.Y, 2))) : 1500);// camera distance would be wherever mario is in the level + the Distance property?
                            LookAt(RotCenter);
                            fovycam = (System.Windows.Media.Media3D.PerspectiveCamera)ModelView.Camera;
                            fovycam.FieldOfView = FOV;
                            ModelView.Camera = fovycam;
                            ModelView.CameraController.MaximumFieldOfView = FOV;
                            ModelView.CameraController.MinimumFieldOfView = FOV;
                            ModelView.CameraController.ActualCamera.Position = new Point3D(ModelView.CameraController.ActualCamera.Position.X, ModelView.CameraController.ActualCamera.Position.Y, newZ);


                        }
                        break;
                    case "FixAll": //campos + lookatpos
                        if (properties.ContainsKey("CameraPos") && properties.ContainsKey("LookAtPos")) //LookAtPos
                        {
                            //Doesn't care about UpOffset or Distance
                            //get the look at position (rotation center), the mario position will be the camera position, direction will be vector between Center to Mario (or backwards)
                            Vector3D CamPos = new Vector3D((Single)((Dictionary<string, object>)properties["CameraPos"])["X"], -(Single)((Dictionary<string, object>)properties["CameraPos"])["Z"], (Single)((Dictionary<string, object>)properties["CameraPos"])["Y"]);
                            Vector3D Center = new Vector3D((Single)((Dictionary<string, object>)properties["LookAtPos"])["X"], -(Single)((Dictionary<string, object>)properties["LookAtPos"])["Z"], (Single)((Dictionary<string, object>)properties["LookAtPos"])["Y"]);
                            Vector3D ResultVec = (Center.Length < CamPos.Length) ? Vector3D.Subtract(Center, CamPos) : Vector3D.Subtract(CamPos, Center);

                            if (ResultVec.X == 0 && ResultVec.Y == 0 && ResultVec.Z != 0)
                            {
                                ModelView.CameraController.CameraUpDirection = new Vector3D(0, 1, 0);
                            }
                            else
                            {
                                ModelView.CameraController.CameraUpDirection = new Vector3D(0, 0, 1);
                            }
                            ModelView.CameraController.ChangeDirection(ResultVec, 100);
                            ModelView.CameraController.FixedRotationPoint = Center.ToPoint3D();
                            CameraDist =  ((Math.Sqrt(Math.Pow(ResultVec.X, 2) + Math.Pow(ResultVec.Y, 2) + Math.Pow(ResultVec.Z,2))));// camera distance would be wherever mario is in the level + the Distance property?
                            LookAt(Center);
                            fovycam = (System.Windows.Media.Media3D.PerspectiveCamera)ModelView.Camera;
                            fovycam.FieldOfView = FOV;
                            ModelView.Camera = fovycam;
                            ModelView.CameraController.MaximumFieldOfView = FOV;
                            ModelView.CameraController.MinimumFieldOfView = FOV;
                        }
                        break;
                    case "FixAllSpot": // no properties, camera is stuck, doesn't rotate
                        break;
                    case "FixPos": // CamPos
                        if (properties.ContainsKey("CameraPos")) //LookAtPos
                        {
                            //Doesn't care about Distance
                            //get the look at position (rotation center), the mario position will be the camera position, direction will be vector between Center to Mario (or backwards)
                            Vector3D CamPos = new Vector3D((Single)((Dictionary<string, object>)properties["CameraPos"])["X"], -(Single)((Dictionary<string, object>)properties["CameraPos"])["Z"], (Single)((Dictionary<string, object>)properties["CameraPos"])["Y"]);
                            Vector3D MarioPos = Positions["StartInfo"][0];
                            MarioPos.Z += newZ;
                            Vector3D ResultVec = (MarioPos.Length < CamPos.Length) ? Vector3D.Subtract(MarioPos, CamPos) : Vector3D.Subtract(CamPos, MarioPos);

                            if(ResultVec.X == 0 && ResultVec.Y == 0 && ResultVec.Z != 0)
                            {
                                ModelView.CameraController.CameraUpDirection = new Vector3D(0, 1, 0);
                            }
                            else
                            {
                                ModelView.CameraController.CameraUpDirection = new Vector3D(0, 0, 1);
                            }
                            ModelView.CameraController.ChangeDirection(new Vector3D(ResultVec.X, ResultVec.Y , ResultVec.Z), 100);
                            ModelView.CameraController.FixedRotationPoint = CamPos.ToPoint3D();
                            ModelView.CameraController.FixedRotationPointEnabled = true;
                            CameraDist = ((Math.Sqrt(Math.Pow(ResultVec.X, 2) + Math.Pow(ResultVec.Y, 2) + Math.Pow(ResultVec.Z, 2))));// camera distance would be wherever mario is in the level + the Distance property?
                            LookAt(MarioPos);
                            fovycam = (System.Windows.Media.Media3D.PerspectiveCamera)ModelView.Camera;
                            fovycam.FieldOfView = FOV;
                            ModelView.Camera = fovycam;
                            ModelView.CameraController.MaximumFieldOfView = FOV;
                            ModelView.CameraController.MinimumFieldOfView = FOV;
                        }
                        break;
                    case "FixPosSpot": // no properties, camera is stuck, rotates 
                        break;
                    case "Rail":// uses rail to move 
                        break;
                    case "ParallelTarget": //??
                        break;
                    case "ParallelVersus": //??
                        break;
                    case "DemoTarget": // ??
                        break;
                    case "Anim":// ??
                        break;
                    case "Follow":// follows mario, similar to mario cam in sm64
                        break;
                }

            }



        }

        public void SetCameraDirection(int x, int y, int z)
        {
            ModelView.Camera.UpDirection = new Vector3D(x, y, z);
        }

        public Vector3D Drag(object[] DragArgs, System.Windows.Input.MouseEventArgs e, double roundTo)
        {
            Point p = e.GetPosition(ModelView);
            Vector3D v = (Vector3D)DragArgs[2];
            Point3D? pos = ModelView.Viewport.UnProject(p, new Point3D(v.X,v.Y,v.Z), ModelView.Camera.LookDirection);
            if (pos.HasValue)
            {
                Vector3D vec = pos.Value.ToVector3D();
                if (roundTo != 0)
                {
                    vec.X = Math.Round(vec.X / roundTo, 0) * roundTo;
                    vec.Y = Math.Round(vec.Y / roundTo, 0) * roundTo;
                    vec.Z = Math.Round(vec.Z / roundTo, 0) * roundTo;
                    return vec;
                }
                else
                {
                    vec.X = Math.Round(vec.X, 3, MidpointRounding.AwayFromZero);
                    vec.Y = Math.Round(vec.Y, 3, MidpointRounding.AwayFromZero);
                    vec.Z = Math.Round(vec.Z, 3, MidpointRounding.AwayFromZero);
                    return vec;
                }
            }
            return pos.Value.ToVector3D();
        }

        public void ChangeTransform(string Type, int index, Vector3D pos, Vector3D scale, Single RotX, Single RotY, Single RotZ, bool SelectedObj)
        {
            Transform3DGroup t = new Transform3DGroup();
            t.Children.Add(new ScaleTransform3D(scale));
            t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), RotX)));
            t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), RotY)));
            t.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), RotZ)));
            t.Children.Add(new TranslateTransform3D(pos));
            Positions[Type][index] = pos;
            Models[Type][index].Transform = t;
            if (SelectedObj)
            {
                if (Models["SelectionLayer"].Count == 0) return;
                if (Models["SelectionLayer"].Count == 1)((BoundingBoxVisual3D)Models["SelectionLayer"][0]).BoundingBox = Models[Type][index].FindBounds(Transform3D.Identity);

            }
        }

        public void ChangeModel(string Type, int index, string path)
        {
            Model3D Model;
            if (!ImportedModels.ContainsKey(path))
            {
                Model = Importer.Load(path);
                ImportedModels.Add(path, Model);
            }
            else Model = ImportedModels[path];
            Model.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));
            Models[Type][index].Content = Model;
            ModelView.UpdateLayout();
        }

        public object[] GetOBJ(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(ModelView);
            object[] res = new object[3] { null, null, null };
            ModelVisual3D result = GetHitResult(p);
            if (result == null) return res;
            foreach (string k in Models.Keys)
            {
                if (k != "TmpAreaChildrenObjs" && Models[k].Contains(result))
                {
                    res[0] = k;
                    res[1] = Models[k].IndexOf(result);
                    res[2] = Positions[k][(int)res[1]];
                    return res;
                }
                if (k == "AllRailInfos")
                {
                    foreach(LinesVisual3D l in Models[k])
                    {
                        if (l.Children.Contains(result))
                        {
                            res[0] = k;
                            res[1] = Models[k].IndexOf(l);
                            res[2] = Positions[k][(int)res[1]];
                            return res;
                        }
                    }
                }
            }
            return new object[3] { null, null, null };
        }

        ModelVisual3D GetHitResult(Point location)
        {
            System.Windows.Media.HitTestResult result = VisualTreeHelper.HitTest(ModelView, location);
            result = VisualTreeHelper.HitTest(ModelView, location);
            if (result != null && result.VisualHit is ModelVisual3D)
            {
                //we transform back to a geometry 3d object and check if the normal of the selected face faces the camera or not, if it does we set the result as the object, if it isn't we check for hit again?
                //
                ModelVisual3D visual = (ModelVisual3D)result.VisualHit;
                return visual;
            }

            return null;
        }

        public double TooCloseCheck() 
        {
            double X =  Math.Abs(CameraTarget.X) - Math.Abs(ModelView.Camera.Position.X) ;
            double Y =  Math.Abs(CameraTarget.Y) - Math.Abs(ModelView.Camera.Position.Y) ;
            double Z =  Math.Abs(CameraTarget.Z) - Math.Abs(ModelView.Camera.Position.Z) ;

            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }

        public Vector3D GetPositionInView()
        {
            FrameworkElement pnlClient = this.Content as FrameworkElement;
            Point3D p = (Point3D)ModelView.Viewport.UnProject(new Point(pnlClient.ActualWidth / 2, pnlClient.ActualHeight / 2), ModelView.Camera.Position, ModelView.Camera.LookDirection);
            return new Vector3D(Math.Truncate(p.X), Math.Truncate(p.Y), Math.Truncate(p.Z));
        }

        public void UnloadLevel()
        {
            ModelView.Children.Remove(ModelViewer);
            ModelViewer.Children.Clear();
            //ImportedModels = new Dictionary<string, Model3D>();
            Models = new Dictionary<string, List<ModelVisual3D>>();
            Positions = new Dictionary<string, List<Vector3D>>();
            ModelViewer = new SortingVisual3D();
            ModelViewer.SortingFrequency = 0.5;
            ModelView.Children.Add(ModelViewer);
            AddKey("TmpChildrenObjs");
            AddKey("SelectedRail");
            AddKey("RailHandle");
            AddKey("RailHandleRail");
            AddKey("Axis");
            AddKey("TmpAreaChildrenObjs");
            AddKey("C0EditingListObjs");
            AddKey("SelectionLayer");
            ModelView.Camera.LookAt(new Point3D(0,0,0), CameraDist, CameraTime);
            CameraTarget = new Vector3D(0, 0, 0);
        }
        
        public void SelectObjs(string type, SelectedIndexCollection IDs)
        {
            ClearSelection();
            int count = 0;
            foreach (int i in IDs)
            {
                BoundingBoxVisual3D box = new BoundingBoxVisual3D();
                Models["SelectionLayer"].Add(box);
                ModelViewer.Children.Add(Models["SelectionLayer"][count++]);
                box.BoundingBox = Models[type][i].FindBounds(Transform3D.Identity);
                box.Diameter = 10;
            }
            ModelView.UpdateLayout();
        }

        public void SelectObjs(string type, int[] IDs)
        {
            ClearSelection();
            int count = 0;
            foreach (int i in IDs)
            {
                BoundingBoxVisual3D box = new BoundingBoxVisual3D();
                Models["SelectionLayer"].Add(box);
                ModelViewer.Children.Add(Models["SelectionLayer"][count++]);
                box.BoundingBox = Models[type][i].FindBounds(Transform3D.Identity);
                box.Diameter = 10;
            }
            ModelView.UpdateLayout();
        }

        public void ClearSelection()
        {
            ClearType("SelectionLayer");
            ClearTmpObjects(false);
            ModelView.UpdateLayout();
        }

        public void UpdateSelected(int selectedindex, int modelindex,  string Type)
        {
            ((BoundingBoxVisual3D)Models["SelectionLayer"][selectedindex]).BoundingBox = Models[Type][modelindex].FindBounds(Transform3D.Identity);
        }

        private void ModelView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsPreview) e.Handled = true;
            if (ModelView.Camera.LookDirection.Length >= 2000000 && e.Delta < 0) e.Handled = true;
            if (ModelView.Camera.LookDirection.Length <= 380 && e.Delta > 0) e.Handled = true; //should change it to use the same distance as cameratoobj
        }

        private void ModelView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           if (IsPreview && (CameraType != "Tower" && CameraType != "FixPos")) e.Handled = true;
        }

        private void ModelView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (CameraType != "Tower" || CameraType != "FixPos") return;
            if (IsPreview && e.MiddleButton == MouseButtonState.Pressed && (CameraType != "Parallel")) e.Handled = true;
            //if (IsPreview) e.Handled = true;
        }

        public void ShowRail(int selectedIndex)
        {
            if (Models.ContainsKey("AllRailInfos") && selectedIndex < Models["AllRailInfos"].Count)
            {

            }
        }
    }
}
