﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace The4Dimension.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("The4Dimension.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C1&gt;
        ///    &lt;C1 Name=&quot;Collision&quot;&gt;
        ///      &lt;A0 Name=&quot;Sensor&quot; StringValue=&quot;Collision&quot; /&gt;
        ///    &lt;/C1&gt;
        ///    &lt;C1 Name=&quot;Debug&quot;&gt;
        ///      &lt;A0 Name=&quot;NodeName&quot; StringValue=&quot;地形&quot; /&gt;
        ///    &lt;/C1&gt;
        ///    &lt;C1 Name=&quot;Executor&quot;&gt;
        ///      &lt;A0 Name=&quot;CategoryName&quot; StringValue=&quot;地形オブジェ[Movement]&quot; /&gt;
        ///    &lt;/C1&gt;
        ///    &lt;C1 Name=&quot;GroupClipping&quot;&gt;
        ///      &lt;D1 Name=&quot;MaxCount&quot; StringValue=&quot;64&quot; /&gt;
        ///    &lt;/C1&gt;
        ///    &lt;FF Name=&quot;Model&quot; S [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string Actor {
            get {
                return ResourceManager.GetString("Actor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a FirstStageMap1.szs
        ///CaveInkStageMap1.szs
        ///DownHillStageMap1.szs
        ///BalanceTruckStageMap1.szs
        ///KoopaFirstStageMap1.szs
        ///FieldWalkStageMap1.szs
        ///CavePatanStageMap1.szs
        ///DotCoinStageMap1.szs
        ///JumpFlipGeneratorStageMap1.szs
        ///BattleShipKillerStageMap1.szs
        ///DesertRuinStageMap1.szs
        ///AquariumSwimStageMap1.szs
        ///SkyChikuwaStageMap1.szs
        ///MountainRopeStageMap1.szs
        ///SweetsScrollStageMap1.szs
        ///FortressSpikeStageMap1.szs
        ///UpUpLandStageMap1.szs
        ///CaveClimbStageMap1.szs
        ///SkyRotateBlockStageMap1.szs
        ///FlashTeresaStageMap1.szs
        ///S [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string AllStageList {
            get {
                return ResourceManager.GetString("AllStageList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca un recurso adaptado de tipo System.Byte[].
        /// </summary>
        internal static byte[] BaseModels {
            get {
                object obj = ResourceManager.GetObject("BaseModels", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Busca un recurso adaptado de tipo System.Byte[].
        /// </summary>
        internal static byte[] BCSTMtools {
            get {
                object obj = ResourceManager.GetObject("BCSTMtools", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a List:
        ///敵 = enemy
        ///地形オブジェ = terrain object
        ///固定地形 = fixed terrain 
        ///デモ = demo (cutscene)
        ///アイテム = item (e.g coin)
        ///コリジョン地形 = Collision terrain (e.g worldmap models)
        ///地形オブジェ[Movement] = Terrain object [Movement] 
        ///ＮＰＣ = NPC 
        ///地形オブジェ[キャラ後] = Terrain object [after character] (e.g level preview models)
        ///デモ[影なし] = Demo [no shadow]
        ///コリジョンアイテム = Collision item
        ///コリジョン地形装飾 = Collision terrain decoration (e.g Rope ends)
        ///空 = Sky
        ///敵[Movement] = Enemy Movement (e.g streetpass obj)
        ///遠景（固定地形） = Distant view (fixed terrain)  [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string ExecName {
            get {
                return ResourceManager.GetString("ExecName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca un recurso adaptado de tipo System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap gbatemp {
            get {
                object obj = ResourceManager.GetObject("gbatemp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C1&gt;
        ///    &lt;C0 Name=&quot;CameraParams&quot;&gt;
        ///      
        ///    &lt;/C0&gt;
        ///    &lt;C1 Name=&quot;VisionParam&quot;&gt;
        ///      &lt;D2 Name=&quot;FarClipDistance&quot; StringValue=&quot;80000&quot; /&gt;
        ///      &lt;D2 Name=&quot;FovyDegree&quot; StringValue=&quot;45&quot; /&gt;
        ///      &lt;D2 Name=&quot;NearClipDistacne&quot; StringValue=&quot;100&quot; /&gt;
        ///      &lt;D2 Name=&quot;StereovisionDepth&quot; StringValue=&quot;0,4&quot; /&gt;
        ///      &lt;D2 Name=&quot;StereovisionDistance&quot; StringValue=&quot;200&quot; /&gt;
        ///    &lt;/C1&gt;
        ///  &lt;/C1&gt;
        ///&lt;/Root&gt;.
        /// </summary>
        internal static string GenericCameraParam {
            get {
                return ResourceManager.GetString("GenericCameraParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C1&gt;
        ///    &lt;D2 Name=&quot;ColorB&quot; StringValue=&quot;1&quot; /&gt;
        ///    &lt;D2 Name=&quot;ColorG&quot; StringValue=&quot;1&quot; /&gt;
        ///    &lt;D2 Name=&quot;ColorR&quot; StringValue=&quot;1&quot; /&gt;
        ///    &lt;D2 Name=&quot;Density&quot; StringValue=&quot;0&quot; /&gt;
        ///    &lt;C0 Name=&quot;FogAreas&quot;&gt;
        ///		
        ///    &lt;/C0&gt;
        ///	&lt;A0 Name=&quot;FogType&quot; StringValue=&quot;&quot; /&gt;
        ///    &lt;D1 Name=&quot;InterpFrame&quot; StringValue=&quot;0&quot; /&gt;
        ///    &lt;D2 Name=&quot;MaxDepth&quot; StringValue=&quot;0&quot; /&gt;
        ///    &lt;D2 Name=&quot;MinDepth&quot; StringValue=&quot;35000&quot;  [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string GenericFogParam {
            get {
                return ResourceManager.GetString("GenericFogParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C0&gt;
        ///    &lt;C1&gt;
        ///      &lt;A0 Name=&quot;Name&quot; StringValue=&quot;SePmShootFire&quot; /&gt;
        ///      &lt;A0 Name=&quot;Type&quot; StringValue=&quot;SoundItem&quot; /&gt;
        ///    &lt;/C1&gt;
        ///  &lt;/C0&gt;
        ///&lt;/Root&gt;.
        /// </summary>
        internal static string GenericPreloadList {
            get {
                return ResourceManager.GetString("GenericPreloadList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C1&gt;
        ///    &lt;C0 Name=&quot;FootPrint&quot;&gt;
        ///      &lt;C1&gt;
        ///        &lt;A0 Name=&quot;Material&quot; StringValue=&quot;Sand&quot; /&gt;
        ///        &lt;A0 Name=&quot;Model&quot; StringValue=&quot;FootPrint&quot; /&gt;
        ///      &lt;/C1&gt;
        ///    &lt;/C0&gt;
        ///    &lt;D1 Name=&quot;StageTimer&quot; StringValue=&quot;500&quot; /&gt;
        ///    &lt;D1 Name=&quot;StageTimerRestart&quot; StringValue=&quot;300&quot; /&gt;
        ///  &lt;/C1&gt;
        ///&lt;/Root&gt;.
        /// </summary>
        internal static string GenericStageInfo {
            get {
                return ResourceManager.GetString("GenericStageInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a .
        /// </summary>
        internal static string Music {
            get {
                return ResourceManager.GetString("Music", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a FixMapParts
        ///ClockMapParts
        ///AoyagiEnemy
        ///JumpFlipBoard
        ///KeyMoveMapParts
        ///LavaConveyer
        ///LiftGeyserStep
        ///NutLift
        ///RailMoveMapParts
        ///RollingReflectMapParts
        ///RollingShakeMapParts
        ///RotateMapParts
        ///SeesawMapParts
        ///SlideMapParts
        ///SwingMapParts
        ///SwitchRotateMapParts
        ///SwitchSlideMapParts
        ///VisibleSwitchMapParts
        ///WheelMapParts
        ///WobbleMapParts
        ///AquariumSwimDebris
        ///VisibleSwitchMapParts
        ///AttackRevolvingPanel
        ///AudioEffectChangeArea
        ///AudioVolumeSettingArea
        ///BalanceTruck
        ///BeatBlock
        ///BeatBlockHolder
        ///BgmChangeArea
        ///Bird
        ///Bl [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string ObjectClasses {
            get {
                return ResourceManager.GetString("ObjectClasses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a @echo off
        ///Title CCNT patcher
        ///cls
        ///echo This will add the object entries to the CreatorClassNameTable.szs file.
        ///echo You should make a backup of this file.
        ///echo Press enter to continue
        ///pause &gt;nul
        ///Start /W Patcher.exe CCNTpatch Done, you are ready to go !
        ///del 3DS.dll
        ///del CCNTpatch.xml
        ///del CommonCompressors.dll
        ///del CommonFiles.dll
        ///del LibEveryFileExplorer.dll
        ///del NDS.dll
        ///del Patcher.exe
        ///del &quot;Patch script.bat&quot;.
        /// </summary>
        internal static string PatchScript {
            get {
                return ResourceManager.GetString("PatchScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;Shift-JIS&quot;?&gt;
        ///
        ///&lt;database version=&quot;0.0&quot;&gt;
        ///&lt;categories&gt;
        ///		&lt;category id=&quot;0&quot; name=&quot;Uncategorized / Unused&quot;/&gt;
        ///		&lt;category id=&quot;1&quot; name=&quot;Main stage elements&quot;/&gt;
        ///		&lt;category id=&quot;2&quot; name=&quot;Decorations and effects&quot;/&gt;
        ///		&lt;category id=&quot;3&quot; name=&quot;Enemies&quot;/&gt;
        ///		&lt;category id=&quot;4&quot; name=&quot;NPCs&quot;/&gt;
        ///		&lt;category id=&quot;5&quot; name=&quot;Areas&quot;/&gt;
        ///		&lt;category id=&quot;6&quot; name=&quot;Special events&quot;/&gt;
        ///		&lt;category id=&quot;7&quot; name=&quot;Misc.&quot;/&gt;
        ///&lt;/categories&gt;
        ///&lt;types&gt;
        ///	&lt;type id=&quot;1&quot; name=&quot;ObjInfo&quot;/&gt;
        ///	&lt;type id=&quot;2&quot; name=&quot;StartInfo&quot;/ [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string sampledb {
            get {
                return ResourceManager.GetString("sampledb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C0&gt;
        ///    &lt;C1&gt;
        ///      &lt;A0 Name=&quot;Name&quot; StringValue=&quot;Collision&quot; /&gt;
        ///      &lt;A0 Name=&quot;Type&quot; StringValue=&quot;CollisionParts&quot; /&gt;
        ///    &lt;/C1&gt;
        ///  &lt;/C0&gt;
        ///&lt;/Root&gt;.
        /// </summary>
        internal static string Sensor {
            get {
                return ResourceManager.GetString("Sensor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; encoding=&quot;shift_jis&quot;?&gt;
        ///&lt;Root&gt;
        ///  &lt;isBigEndian Value=&quot;False&quot; /&gt;
        ///  &lt;BymlFormatVersion Value=&quot;1&quot; /&gt;
        ///  &lt;C0&gt;
        ///    &lt;C1&gt;
        ///      &lt;A0 Name=&quot;Name&quot; StringValue=&quot;Body&quot; /&gt;
        ///	  &lt;D2 Name=&quot;Radius&quot; StringValue=&quot;50&quot; /&gt;
        ///      &lt;A0 Name=&quot;Type&quot; StringValue=&quot;Npc&quot; /&gt;
        ///	  &lt;D2 Name=&quot;Y&quot; StringValue=&quot;80&quot; /&gt;
        ///    &lt;/C1&gt;
        ///  &lt;/C0&gt;
        ///&lt;/Root&gt;.
        /// </summary>
        internal static string SensorNPC {
            get {
                return ResourceManager.GetString("SensorNPC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a DemoBeforeEndingStageMap1.szs
        ///DemoCourseKoopaRunawayStageMap1.szs
        ///DemoCourseSelectStartStageMap1.szs
        ///DemoDokanWorldMapStageMap1.szs
        ///DemoEndRollCompleteStageMap1.szs
        ///DemoEndRollStageMap1.szs
        ///DemoGameOverStageMap1.szs
        ///DemoKoopaDownStageMap1.szs
        ///DemoKoopaLastStageMap1.szs
        ///DemoKoopaLv3StageMap1.szs
        ///DemoLastLetterStageMap1.szs
        ///DemoLuigiLetterStageMap1.szs
        ///DemoLuigiRescueStageMap1.szs
        ///DemoOpeningStageMap1.szs
        ///DemoStartBattleShipEveningStageMap1.szs
        ///DemoStartBattleShipSpecialEveningStageMap1.szs
        ///De [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string SpecialStageList {
            get {
                return ResourceManager.GetString("SpecialStageList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca un recurso adaptado de tipo System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap t4d {
            get {
                object obj = ResourceManager.GetObject("t4d", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Busca un recurso adaptado de tipo System.Byte[].
        /// </summary>
        internal static byte[] Worldmap {
            get {
                object obj = ResourceManager.GetObject("Worldmap", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
