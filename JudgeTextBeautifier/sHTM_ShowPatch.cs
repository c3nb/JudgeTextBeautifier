using DG.Tweening;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(scrHitTextMesh), "Show")]
    public static class sHTM_ShowPatch
    {
        public static bool Prefix(scrHitTextMesh __instance, Vector3 position, float angle, ref int ___frameShown, ref float ___timer, ref Renderer ___meshRenderer, ref Vector3 ___textPos)
        {
            ___frameShown = Time.frameCount;
            ___timer = 0f;
            __instance.transform.localPosition = position;
            __instance.transform.gameObject.SetActive(true);
            __instance.dead = false;
            Material material = ___meshRenderer.material;
            material.DOKill(false);
            material.color = Color.white;
            if (Settings.settings.TextDuration >= 0)
                material.DOFade(0f, 0.7f).SetDelay(Settings.settings.TextDuration).SetEase(Ease.OutQuad);
            else material.DOFade(0f, 0.7f).SetDelay(0.5f).SetEase(Ease.OutQuad);
            scrMisc.Rotate2D(__instance.transform, scrController.instance.camy.transform.rotation.eulerAngles.z);
            __instance.transform.DOKill(false);
            __instance.transform.localScale = new Vector3(__instance.startingSize, __instance.startingSize, 1f);
            __instance.transform.DOPunchScale(new Vector3(__instance.sizeUp, __instance.sizeUp, 1f), __instance.duration, __instance.vibrato, __instance.elasticity);
            if (__instance.hitMargin != HitMargin.Perfect)
                __instance.transform.DOLocalRotate(new Vector3(0f, 0f, angle * 20f), 2f, RotateMode.LocalAxisAdd);
            ___textPos = position;
            return false;
        }
    }
}
