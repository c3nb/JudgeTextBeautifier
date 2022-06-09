using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityModManagerNet.UnityModManager.UI;

namespace JudgeTextBeautifier
{
    public static class Utils
    {
        public static bool DrawBool(this bool value, string text)
            => GUILayout.Toggle(value, text);
        public static void DrawTextArea(ref string str, Action onChange)
        {
            string cache = str;
            str = GUILayout.TextArea(str);
            if (str != cache)
                onChange();
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
    }
}
