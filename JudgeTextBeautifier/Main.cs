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
        public static Talmo[] Talmos;
        public static bool HasOverlayer = false;
        public static void Load(ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnToggle = (mod, value) =>
            {
                if (value)
                {
                    if (File.Exists(TalmoPath))
                        Talmos = File.ReadAllText(TalmoPath).FromJson<Talmo[]>();
                    else Talmos = Talmo.GetNewTalmos();
                    har = new Harmony(mod.Info.Id);
                    har.PatchAll(Assembly.GetExecutingAssembly());
                    _ = Settings.settings;
                    HasOverlayer = modEntries.FirstOrDefault(m => m.Info.Id == "Overlayer" && m.Enabled) != null;
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
                GUILayout.Label($"Overlayer{(HasOverlayer ? "" : " Not")} Detected");
                Settings settings = Settings.settings;
                Language lang = Language.Current;
                bool newIsTalmo = settings.IsTalmo.DrawBool(lang.ColorMode);
                if (newIsTalmo)
                    foreach (Talmo talmo in Talmos)
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
                if (fontSetting = GUILayout.Toggle(fontSetting, lang.FontSetting))
                {
                    GUILayout.Label(lang.FontWarning);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.TooEarly);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.TooEarly], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.VeryEarly);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.VeryEarly], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.EarlyPerfect);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.EarlyPerfect], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.Perfect);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.Perfect], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.LatePerfect);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.LatePerfect], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.VeryLate);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.VeryLate], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.TooLate);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.TooLate], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.Multipress);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.Multipress], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.FailMiss);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.FailMiss], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lang.FailOverload);
                    Utils.DrawTextArea(ref settings.Fonts[(int)HitMargin.FailOverload], settings.OnChange);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            };
            modEntry.OnSaveGUI = mod =>
            {
                File.WriteAllText(TalmoPath, Talmos.ToJson());
                Settings.settings.Save();
            };
        }
        static bool fontSetting = false;
        public static void StartSHTMP()
        {
            scrController ctrl = scrController.instance;
            if (ctrl != null && ctrl)
            {
                Talmos.ForEach(talmo => talmo.title = RDString.Get("HitMargin." + talmo.hitMargin.ToString()));
                if (ctrl.gameworld)
                {
                    var hitTexts = sHTMtosHTMP.ctrlCachedHitTexts;
                    hitTexts.ForEach(kvp => kvp.Value.ForEach(DestroyIfExist));
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
                arr.ForEach(ary => ary.ForEach(DestroyIfExist));
                Array.Clear(arr, 0, 10);
                sHTMtosHTMP.InitSHTM(ctrl);
            }
        }
        static void DestroyIfExist(Object obj)
        {
            if (obj)
                Object.Destroy(obj);
        }
    }
}
