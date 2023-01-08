using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace JudgeTextBeautifier
{
    public static class sHTM_FontPatch
    {
        [HarmonyPatch(typeof(scrHitTextMesh), "Init")]
        public static class InitPatch
        {
            public static void Postfix(HitMargin hitMargin, TextMesh ___text)
            {
                if (FontManager.TryGetFont(Settings.settings.Fonts[(int)hitMargin], out var fontData))
                {
                    if (!fontData.font.dynamic) return;
                    ___text.font = fontData.font;
                    ___text.GetComponent<MeshRenderer>().material = fontData.font.material;
                }
            }
        }
    }
}
