using HarmonyLib;
using UnityEngine;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(scrHitTextMesh), "Init")]
    public static class sHTM_FontPatch
    {
        public static void Postfix(scrHitTextMesh __instance, HitMargin hitMargin, TextMesh ___text)
        {
            if (Settings.settings.FontSize >= 0)
                ___text.fontSize = (int)Settings.settings.FontSize;
            if (FontManager.TryGetFont(Settings.settings.Fonts[(int)hitMargin], out var fontData))
            {
                if (!(fontData.font?.dynamic ?? false)) return;
                //if (!fontData.font.dynamic)
                //{
                //    ___text.fontSize = 0;
                //    ___text.fontStyle = FontStyle.Normal;
                //}
                ___text.font = fontData.font;
                ___text.GetComponent<MeshRenderer>().material = fontData.font.material;
            }
        }
    }
}
