using DG.Tweening;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(scrHitTextMesh), "Update")]
    public static class sHTM_UpdatePatch
    {
        public static bool Prefix(scrHitTextMesh __instance, ref bool ___forceOnScreen, ref float ___timer, ref TextMesh ___text, ref Camera ___gameCam, ref Vector3 ___textPos)
        {
            if (__instance.dead) return false;
            if (___forceOnScreen)
            {
                float num = ___gameCam.orthographicSize * 2f;
                float num2 = num * Screen.width / Screen.height;
                Vector3 position = ___gameCam.transform.position;
                Vector3 vector = ___textPos - position;
                Vector3 vector2 = ___textPos;
                vector2.x = position.x + Mathf.Clamp(vector.x, -num2 / 2f + scrController.instance.hitTextMinBorderDistance, num2 / 2f - scrController.instance.hitTextMinBorderDistance);
                vector2.y = position.y + Mathf.Clamp(vector.y, -num / 2f + scrController.instance.hitTextMinBorderDistance, num / 2f - scrController.instance.hitTextMinBorderDistance);
                __instance.transform.localPosition = vector2;
            }
            ___timer += Time.deltaTime;
            if (___timer > Settings.settings.TextDuration.GraterThan(0, 1.25f))
            {
                __instance.dead = true;
                __instance.transform.DOKill(false);
                ___text.DOKill(false);
                __instance.gameObject.SetActive(false);
            }
            return false;
        }
    }
}
