using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityModManagerNet.UnityModManager.UI;

namespace JudgeTextBeautifier
{
    public static class Utils
    {
        public static readonly string[] ColorTitles = new string[] { "<color=#FF0000>R</color>", "<color=#00FF00>G</color>", "<color=#0000FF>B</color>", "A" };
        public static float GraterThan(this float f, float minValue, float ifLessThan) => f < minValue ? ifLessThan : f;
        public static (float, float) Distribute(this float f, float a, float b)
        {
            float total = a + b;
            float aFactor = a / total;
            float bFactor = b / total;
            return (f * aFactor, f * bFactor);
        }
        public static (float, float) DistributeRev(this float f, float a, float b)
        {
            float aFactor = f - b;
            float bFactor = f - a;
            return (aFactor / f, bFactor / f);
        }
        public static bool DrawBool(this bool value, string text)
            => GUILayout.Toggle(value, text);
        public static bool DrawTextArea(ref string str, Action onChange = null)
        {
            string cache = str;
            str = GUILayout.TextArea(str);
            if (str != cache)
            {
                onChange?.Invoke();
                return true;
            }
            return false;
        }
        public static bool DrawTextField(ref string str, Action onChange = null)
        {
            string cache = str;
            str = GUILayout.TextField(str);
            if (str != cache)
            {
                onChange?.Invoke();
                return true;
            }
            return false;
        }
        public static T MaxFloat<T>(this T[] array, Func<T, float> selector)
        {
            int index = 0;
            float maxValue = float.MinValue;
            for (int i = 0; i < array.Length; i++)
            {
                float value = selector(array[i]);
                if (maxValue < value)
                {
                    maxValue = value;
                    index = i;
                }
            }
            return array[index];
        }
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> forEach)
        {
            foreach (var item in enumerable)
                forEach(item);
        }
        public static void IndentGUI(Action GUI, float verticalSpace = 0f, float indentSize = 20f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indentSize);
            GUILayout.BeginVertical();
            GUILayout.Space(verticalSpace);
            GUI();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        public static bool DrawColor(ref float[] color, GUIStyle style = null, params GUILayoutOption[] option) => DrawFloatMultiField(ref color, new string[]
            {
        "<color=#FF0000>R</color>",
        "<color=#00FF00>G</color>",
        "<color=#0000FF>B</color>",
        "A"
            }, style, option);
        public static bool DrawVector2FloatArr(ref float[] vector, GUIStyle style = null, params GUILayoutOption[] option) => DrawFloatMultiField(ref vector, new string[] { "X", "Y" }, style, option);


        public static void DrawToggle(string title, ref bool toggle, Action onEnableState = null, Action onDisableState = null, Action onChange = null)
        {
            bool newToggle = GUILayout.Toggle(toggle, title);
            if (toggle != newToggle)
            {
                toggle = newToggle;
                onChange?.Invoke();
            }
            if (newToggle)
                onEnableState?.Invoke();
            else onDisableState?.Invoke();
        }

        public static void DrawHorizontalFlexibleTextArea(string title, ref string value, Action onChange = null)
            => DrawHorizontalFlexibleTextArea(title, 0, ref value, onChange);
        public static void DrawHorizontalFlexibleFloatField(string title, ref float value, Action onChange = null)
            => DrawHorizontalFlexibleFloatField(title, 0, ref value, onChange);
        public static void DrawHorizontalFlexibleIntField(string title, ref int value, Action onChange = null)
            => DrawHorizontalFlexibleIntField(title, 0, ref value, onChange);
        public static void DrawHorizontalFlexibleFloatArrayField(string[] titles, float[] values, Action onChange = null)
            => DrawHorizontalFlexibleFloatArrayField(titles, 0, values, onChange);

        public static void DrawHorizontalFlexibleTextArea(string title, float space, ref string value, Action onChange = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.Space(space);
            DrawTextArea(ref value, onChange);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static void DrawHorizontalFlexibleFloatField(string title, float space, ref float value, Action onChange = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.Space(space);
            string sValue = value.ToString("F6");
            bool changed = DrawTextField(ref sValue);
            if (float.TryParse(sValue, out float result))
            {
                value = result;
                onChange?.Invoke();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static void DrawHorizontalFlexibleIntField(string title, float space, ref int value, Action onChange = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.Space(space);
            string sValue = value.ToString();
            bool changed = DrawTextField(ref sValue);
            if (int.TryParse(sValue, out int result))
            {
                value = result;
                onChange?.Invoke();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static void DrawHorizontalFlexibleFloatArrayField(string[] titles, float space, float[] values, Action onChange = null)
        {
            int length = Math.Min(titles.Length, values.Length);
            GUILayout.BeginHorizontal();
            for (int i = 0; i < length; i++)
            {
                string title = titles[i];
                ref float value = ref values[i];
                GUILayout.Label(title);
                GUILayout.Space(space);
                string sValue = value.ToString("F6");
                bool changed = DrawTextField(ref sValue);
                if (float.TryParse(sValue, out float result))
                {
                    value = result;
                    onChange?.Invoke();
                }
                GUILayout.Space(1);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void DrawHorizontalFlexibleColor(string title, float[] color, Action onChange = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.Space(1);
            for (int i = 0; i < color.Length; i++)
            {
                ref float value = ref color[i];
                GUILayout.Label(ColorTitles[i]);
                string sValue = value.ToString("F6");
                bool changed = DrawTextField(ref sValue);
                if (float.TryParse(sValue, out float result))
                {
                    value = result;
                    onChange?.Invoke();
                }
                GUILayout.Space(1);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void DrawHorizontalFlexibleButton(string title, Action onPressed)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(title))
                onPressed();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
