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
                    Utils.DrawHorizontalFlexibleButton(lang.Reset, () =>
                    {
                        settings.Reset(true);
                        settings.FontSize = 50;
                        UpdateFontSize();
                    });
                    Utils.DrawHorizontalFlexibleTextArea(lang.TooEarly, ref settings.TooEarly, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.VeryEarly, ref settings.VeryEarly, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.EarlyPerfect, ref settings.EarlyPerfect, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.LatePerfect, ref settings.LatePerfect, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.VeryLate, ref settings.VeryLate, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.TooLate, ref settings.TooLate, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.Multipress, ref settings.Multipress, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.FailMiss, ref settings.FailMiss, settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.FailOverload, ref settings.FailOverload, settings.OnChange);
                }
                if (newIsTalmo != settings.IsTalmo)
                    settings.IsTalmo = newIsTalmo;
                if (fontSetting = GUILayout.Toggle(fontSetting, lang.FontSetting))
                {
                    GUILayout.Label(lang.FontWarning);
                    Utils.DrawHorizontalFlexibleTextArea(lang.TooEarly, ref settings.Fonts[(int)HitMargin.TooEarly], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.VeryEarly, ref settings.Fonts[(int)HitMargin.VeryEarly], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.EarlyPerfect, ref settings.Fonts[(int)HitMargin.EarlyPerfect], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.Perfect, ref settings.Fonts[(int)HitMargin.Perfect], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.LatePerfect, ref settings.Fonts[(int)HitMargin.LatePerfect], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.VeryLate, ref settings.Fonts[(int)HitMargin.VeryLate], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.TooLate, ref settings.Fonts[(int)HitMargin.TooLate], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.Multipress, ref settings.Fonts[(int)HitMargin.Multipress], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.FailMiss, ref settings.Fonts[(int)HitMargin.FailMiss], settings.OnChange);
                    Utils.DrawHorizontalFlexibleTextArea(lang.FailOverload, ref settings.Fonts[(int)HitMargin.FailOverload], settings.OnChange);
                }
                Utils.DrawHorizontalFlexibleFloatField(lang.FontSize, ref settings.FontSize, () =>
                {
                    if (settings.FontSize >= 0)
                        UpdateFontSize();
                });
                Utils.DrawHorizontalFlexibleFloatField(lang.TextDuration, ref settings.TextDuration);
                Utils.DrawHorizontalFlexibleFloatField(lang.TextPunchDuration, ref settings.TextPunchDuration);
                Utils.DrawHorizontalFlexibleIntField(lang.CachedTextCount, ref settings.CachedTextCount);
                Utils.DrawHorizontalFlexibleFloatArrayField(new[] { "X", "Y" }, settings.TextOffset);
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
        public static void UpdateFontSize()
        {
            if (!(scrController.instance?.gameworld ?? false)) return;
            if (Settings.settings.IsTalmo)
                foreach (var text in sHTMtosHTMP.cachedHitTexts.SelectMany(t => t))
                    text.text.fontSize = Settings.settings.FontSize;
            else
                foreach (var text in sHTMtosHTMP.ctrlCachedHitTexts.Values.SelectMany(t => t))
                    sHTMtosHTMP.text(text).fontSize = (int)Settings.settings.FontSize;
        }
    }
}
