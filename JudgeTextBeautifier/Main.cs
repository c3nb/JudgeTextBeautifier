using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using static UnityModManagerNet.UnityModManager;
using Object = UnityEngine.Object;
using System.IO;
using System.Diagnostics;
using TinyJson;

namespace JudgeTextBeautifier
{
    public static class Main
    {
        public static ModEntry mod;
        public static Harmony har;
        public static readonly string TalmoPath = Path.Combine("Mods", "JudgeTextBeautifier", "Talmos.json");
        public static Text[] Talmos;
        public static bool HasOverlayer = false;
        public static void Load(ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnToggle = (mod, value) =>
            {
                if (value)
                {
                    if (File.Exists(TalmoPath))
                        Talmos = File.ReadAllText(TalmoPath).FromJson<Text[]>();
                    else Talmos = Text.GetNewTalmos();
                    har = new Harmony(mod.Info.Id);
                    har.PatchAll(Assembly.GetExecutingAssembly());
                    _ = Settings.settings;
                }
                else
                {
                    har.UnpatchAll(har.Id);
                    har = null;
                }
                return true;
            };
            modEntry.OnGUI = (mod) =>
            {
                Settings settings = Settings.settings;
                Language lang = Language.Current;
                bool newIsTalmo = settings.IsTalmo.DrawBool(lang.ColorMode);
                if (newIsTalmo)
                    foreach (Text talmo in Talmos)
                        talmo.TalmoGUI();
                else
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(lang.Reset))
                        settings.Reset(true);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.TooEarly);
                    Utils.DrawTextArea(ref settings.TooEarly, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.VeryEarly);
                    Utils.DrawTextArea(ref settings.VeryEarly, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.EarlyPerfect);
                    Utils.DrawTextArea(ref settings.EarlyPerfect, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.Perfect);
                    Utils.DrawTextArea(ref settings.Perfect, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.LatePerfect);
                    Utils.DrawTextArea(ref settings.LatePerfect, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.VeryLate);
                    Utils.DrawTextArea(ref settings.VeryLate, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.TooLate);
                    Utils.DrawTextArea(ref settings.TooLate, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.Multipress);
                    Utils.DrawTextArea(ref settings.Multipress, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.FailMiss);
                    Utils.DrawTextArea(ref settings.FailMiss, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.FailOverload);
                    Utils.DrawTextArea(ref settings.FailOverload, settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                if (newIsTalmo != settings.IsTalmo)
                    settings.IsTalmo = newIsTalmo;
            };
            modEntry.OnSaveGUI = mod =>
            {
                File.WriteAllText(TalmoPath, Talmos.ToJson());
                Settings.settings.Save();
            };
        }
        public static void StartSHTMP()
        {
            scrController ctrl = scrController.instance;
            if (ctrl != null && ctrl)
            {
                Talmos.ForEach(talmo => talmo.title = RDString.Get("HitMargin." + talmo.hitMargin.ToString()));
                if (ctrl.gameworld)
                {
                    var hitTexts = sHTMtosHTMP.ctrlCachedHitTexts;
                    hitTexts.ForEach(kvp => kvp.Value.ForEach(shtm => Object.Destroy(shtm)));
                    hitTexts.Clear();
                    sHTMtosHTMP.InitSHTMP(ctrl);
                }
            }
        }
        public static void StopSHTMP()
        {
            var arr = sHTMtosHTMP.cachedHitTexts;
            Array.ForEach(Talmos, t => t.Terminate());
            scrController ctrl = scrController.instance;
            if (ctrl != null && ctrl && ctrl.gameworld)
            {
                arr.ForEach(ary => ary.ForEach(shtm => Object.Destroy(shtm)));
                Array.Clear(arr, 0, 10);
                sHTMtosHTMP.InitSHTM(ctrl);
            }
        }
    }
}
