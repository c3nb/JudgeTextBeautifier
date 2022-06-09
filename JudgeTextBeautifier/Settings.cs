﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TinyJson;

namespace JudgeTextBeautifier
{
    public class Settings
    {
        public static readonly string SettingsPath = Path.Combine("Mods", "JudgeTextBeautifier", "Settings.json");
        public static Settings settings
        {
            get
            {
                if (_settings == null)
                {
                    if (File.Exists(SettingsPath))
                        return _settings = File.ReadAllText(SettingsPath).FromJson<Settings>();
                    Settings newSettings = new Settings();
                    File.WriteAllText(SettingsPath, newSettings.ToJson());
                    return newSettings;
                }
                return _settings;
            }
        }
        public void Save()
            => File.WriteAllText(SettingsPath, this.ToJson());
        public void Reset(bool force = false)
        {
            if (force)
            {
                TooEarly = RDString.Get("HitMargin.TooEarly");
                VeryEarly = RDString.Get("HitMargin.VeryEarly");
                EarlyPerfect = RDString.Get("HitMargin.EarlyPerfect");
                Perfect = RDString.Get("HitMargin.Perfect");
                LatePerfect = RDString.Get("HitMargin.LatePerfect");
                VeryLate = RDString.Get("HitMargin.VeryLate");
                TooLate = RDString.Get("HitMargin.TooLate");
                Multipress = RDString.Get("HitMargin.Multipress");
                FailMiss = RDString.Get("HitMargin.FailMiss");
                FailOverload = RDString.Get("HitMargin.FailOverload");
                return;
            }
            TooEarly ??= RDString.Get("HitMargin.TooEarly");
            VeryEarly ??= RDString.Get("HitMargin.VeryEarly");
            EarlyPerfect ??= RDString.Get("HitMargin.EarlyPerfect");
            Perfect ??= RDString.Get("HitMargin.Perfect");
            LatePerfect ??= RDString.Get("HitMargin.LatePerfect");
            VeryLate ??= RDString.Get("HitMargin.VeryLate");
            TooLate ??= RDString.Get("HitMargin.TooLate");
            Multipress ??= RDString.Get("HitMargin.Multipress");
            FailMiss ??= RDString.Get("HitMargin.FailMiss");
            FailOverload ??= RDString.Get("HitMargin.FailOverload");
        }
        public void OnChange()
        {
            if (Main.HasOverlayer)
                Overlayer.CompileAll(this);
        }
        private static Settings _settings;
        public bool IsTalmo
        {
            get => isTalmo;
            set
            {
                isTalmo = value;
                if (value)
                    Main.StartSHTMP();
                else Main.StopSHTMP();
            }
        }
        bool isTalmo = false;
        public string TooEarly;
        public string VeryEarly;
        public string EarlyPerfect;
        public string Perfect;
        public string LatePerfect;
        public string VeryLate;
        public string TooLate;
        public string Multipress;
        public string FailMiss;
        public string FailOverload;
    }
}
