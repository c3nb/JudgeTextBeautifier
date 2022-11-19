using System;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
#pragma warning disable

namespace JudgeTextBeautifier
{
    public class Talmo
    {
        public static Language lang => Language.Current;
        bool Saved = false; 
        public static readonly Dictionary<HitMargin, Color> hitMarginColors = new Dictionary<HitMargin, Color>()
        {
            { HitMargin.TooEarly, new Color(1, 0, 0, 1) },
            { HitMargin.VeryEarly, new Color(1, 0.4352941f, 0.3058824f, 1) },
            { HitMargin.EarlyPerfect, new Color(0.627451f, 1, 0.3058824f, 1) },
            { HitMargin.Perfect, new Color(0.3764706f, 1, 0.3066038f, 1) },
            { HitMargin.LatePerfect, new Color(0.627451f, 1, 0.3058824f, 1) },
            { HitMargin.VeryLate, new Color(1, 0.4352941f, 0.3058824f, 1) },
            { HitMargin.TooLate, new Color(1, 0, 0, 1) },
            { HitMargin.Multipress, new Color(0, 1, 0.9301295f, 1) },
            { HitMargin.FailMiss, new Color(0.851175f, 0.3459119f, 1, 1) },
            { HitMargin.FailOverload, new Color(0.851175f, 0.3459119f, 1, 1) },
            { HitMargin.Auto, new Color(0.3764706f, 1, 0.3066038f, 1) },
        };
        public static readonly HitMargin[] redMargins = new HitMargin[] { HitMargin.TooEarly, HitMargin.VeryEarly, HitMargin.VeryLate, HitMargin.TooLate, HitMargin.FailMiss, HitMargin.FailOverload };
        public static readonly HitMargin[] hitMargins = (HitMargin[])Enum.GetValues(typeof(HitMargin));
        public static Talmo[] GetNewTalmos()
        {
            Talmo[] talmos = new Talmo[hitMargins.Length];
            for (int i = 0; i < hitMargins.Length; i++)
                talmos[i] = new Talmo(hitMargins[i]);
            return talmos;
        }
        public Talmo() => Saved = true;
        public Talmo(HitMargin hitMargin)
            => this.hitMargin = hitMargin;
        public HitMargin hitMargin
        {
            get => hm;
            set
            {
                hm = value;
                if (!Saved)
                {
                    var col = hitMarginColors[value];
                    Color = new[] { col.r, col.g, col.b, col.a };
                    Reset(false);
                }
            }
        }
        internal string title;
        HitMargin hm;
        public float[] Color = new float[4] { 1, 1, 1, 1 };
        public bool IsGradient = false;
        public bool IsEditing = false;
        public float[][] Gradient = new float[4][]
        {
            new float[] { 1, 1, 1, 1 },
            new float[] { 1, 1, 1, 1 },
            new float[] { 1, 1, 1, 1 },
            new float[] { 1, 1, 1, 1 },
        };
        bool Initialized = false;
        public void Initialize()
        {
            Initialized = true;
            Apply();
        }
        public void Terminate()
            => Initialized = false;
        public void TalmoGUI()
        {
            if (IsEditing = GUILayout.Toggle(IsEditing, title))
            {
                Utils.IndentGUI(() =>
                {
                    Utils.IndentGUI(() =>
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(lang.TextColor);
                        GUILayout.Space(1);
                        if (Utils.DrawColor(ref Color)) Apply();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    });
                    bool newIsGradient = GUILayout.Toggle(IsGradient, lang.Gradient);
                    if (newIsGradient)
                    {
                        Utils.IndentGUI(() =>
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(lang.Reset))
                                Reset(true);
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label(lang.TopLeft);
                            GUILayout.Space(1);
                            if (Utils.DrawColor(ref Gradient[0])) Apply();
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label(lang.TopRight);
                            GUILayout.Space(1);
                            if (Utils.DrawColor(ref Gradient[1])) Apply();
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label(lang.BottomLeft);
                            GUILayout.Space(1);
                            if (Utils.DrawColor(ref Gradient[2])) Apply();
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label(lang.BottomRight);
                            GUILayout.Space(1);
                            if (Utils.DrawColor(ref Gradient[3])) Apply();
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        });
                    }
                    if (IsGradient != newIsGradient)
                    {
                        IsGradient = newIsGradient;
                        Apply();
                    }
                });
            }
        }
        public void Reset(bool withApply)
        {
            IsGradient = true;
            if (redMargins.Contains(hitMargin))
            {
                SetMagenta(0);
                SetYellow(3);
            }
            else
            {
                SetYellow(0);
                SetCyan(3);
            }
            Gradient[1] = (float[])Color.Clone();
            Gradient[2] = (float[])Color.Clone();
            if (withApply)
                Apply();
        }
        public void Apply()
        {
            if (!Initialized || !scrConductor.instance.isGameWorld) return;
            Color col = new Color(Color[0], Color[1], Color[2], Color[3]);
            Color col1 = new Color(Gradient[0][0], Gradient[0][1], Gradient[0][2], Gradient[0][3]);
            Color col2 = new Color(Gradient[1][0], Gradient[1][1], Gradient[1][2], Gradient[1][3]);
            Color col3 = new Color(Gradient[2][0], Gradient[2][1], Gradient[2][2], Gradient[2][3]);
            Color col4 = new Color(Gradient[3][0], Gradient[3][1], Gradient[3][2], Gradient[3][3]);
            Array.ForEach(sHTMtosHTMP.cachedHitTexts[(int)hitMargin], tmp =>
            {
                if (IsGradient)
                    tmp.text.colorGradient = new VertexGradient(col1, col2, col3, col4);
                else tmp.text.colorGradient = new VertexGradient(col);
            });
        }
        public void SetMagenta(int index)
        {
            Gradient[index][0] = 1;
            Gradient[index][1] = 0;
            Gradient[index][2] = 1;
            Gradient[index][3] = 1;
        }
        public void SetYellow(int index)
        {
            Gradient[index][0] = 1;
            Gradient[index][1] = 1;
            Gradient[index][2] = 0;
            Gradient[index][3] = 1;
        }
        public void SetCyan(int index)
        {
            Gradient[index][0] = 0;
            Gradient[index][1] = 1;
            Gradient[index][2] = 1;
            Gradient[index][3] = 1;
        }
    }
}
