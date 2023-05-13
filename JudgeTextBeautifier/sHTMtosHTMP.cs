using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Reflection.Emit;
using static System.Net.WebRequestMethods;
using Mono.Cecil.Cil;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(scrController))]
    public static class sHTMtosHTMP
    {
        [HarmonyPostfix]
        [HarmonyPatch("Awake")]
        public static void Postfix(scrController __instance)
		{
			SetTalmoTitle();
			Settings.settings.Reset();
			InitSHT(__instance);
        }
        public static void SetTalmoTitle()
		{
            Main.Talmos.ForEach(talmo => talmo.title = RDString.Get("HitMargin." + talmo.hitMargin.ToString()));
        }
		[HarmonyPrefix]
		[HarmonyPatch("ShowHitText")]
		public static bool CtrlShowHitTextPrefix(HitMargin hitMargin, Vector3 position, float angle)
        {
			var offset = Settings.settings.TextOffset;
			position += new Vector3(offset[0], offset[1]);
			if (!Settings.settings.IsTalmo)
            {
				foreach (var shtm in ctrlCachedHitTexts[hitMargin])
                {
					if (shtm.dead)
                    {
						TextMesh text = sHTMtosHTMP.text(shtm);
						text.richText = false;
						text.text = Settings.settings.GetString(hitMargin);
                        if (Settings.settings.TextPunchDuration >= 0)
                            shtm.duration = Settings.settings.TextPunchDuration;
                        shtm.Show(position, angle);
						return false;
                    }
                }
                return false;
            }
            var margin = (int)hitMargin;
			var arr = cachedHitTexts[margin];
			for (int i = 0; i < 100; i++)
            {
				var shtmp = arr[i];
				if (shtmp.dead)
                {
                    if (Settings.settings.TextDuration >= 0)
                        shtmp.appearDuration = Settings.settings.TextDuration;
                    if (Settings.settings.TextPunchDuration >= 0)
                        shtmp.duration = Settings.settings.TextPunchDuration;
                    shtmp.Show(position, angle);
					return false;
				}
            }
			return false;
        }
		public static Dictionary<HitMargin, scrHitTextMesh[]> ctrlCachedHitTexts
		{
			get => chtRef(scrController.instance);
			set => chtRef(scrController.instance) = value;
		}
        public static FieldInfo cht = AccessTools.Field(typeof(scrController), "cachedHitTexts");
        public static AccessTools.FieldRef<scrController, Dictionary<HitMargin, scrHitTextMesh[]>> chtRef = AccessTools.FieldRefAccess<scrController, Dictionary<HitMargin, scrHitTextMesh[]>>(cht);
		public static AccessTools.FieldRef<scrHitTextMesh, TextMesh> text = AccessTools.FieldRefAccess<scrHitTextMesh, TextMesh>("text");
		public static scrHitTextMeshPro[][] cachedHitTexts = new scrHitTextMeshPro[Talmo.hitMargins.Length][];
		public static void InitSHT(scrController ctrl)
		{
			if (Settings.settings.IsTalmo)
				InitSHTMP(ctrl);
			else InitSHTM(ctrl);
		}
		public static void InitSHTMP(scrController ctrl)
        {
			ctrl.hitTextContainer = new GameObject("HitTexts");
			Transform transform = ctrl.hitTextContainer.transform;
			cachedHitTexts = new scrHitTextMeshPro[Talmo.hitMargins.Length][];
			foreach (HitMargin hitMargin in Talmo.hitMargins)
			{
				scrHitTextMeshPro[] texts = new scrHitTextMeshPro[100];
				for (int i = 0; i < 100; i++)
				{
					GameObject go = new GameObject();
					scrHitTextMeshPro shtmp = go.AddComponent<scrHitTextMeshPro>();
					go.transform.SetParent(transform);
					shtmp.Init(hitMargin, Settings.settings.Fonts[(int)hitMargin]);
					if (Settings.settings.FontSize >= 0)
						shtmp.text.fontSize = Settings.settings.FontSize;
					texts[i] = shtmp;
				}
				cachedHitTexts[(int)hitMargin] = texts;
				Main.Talmos[(int)hitMargin].Initialize();
			}
		}
		public static void InitSHTM(scrController ctrl)
        {
			ctrl.hitTextContainer = new GameObject("HitTexts");
			Transform transform = ctrl.hitTextContainer.transform;
			ctrlCachedHitTexts = new Dictionary<HitMargin, scrHitTextMesh[]>();
			foreach (HitMargin hitMargin in Talmo.hitMargins)
			{
				scrHitTextMesh[] texts = new scrHitTextMesh[100];
				for (int i = 0; i < 100; i++)
				{
					scrHitTextMesh shtm = Object.Instantiate(RDConstants.data.hitTextPrefab, transform).GetComponentInChildren<scrHitTextMesh>();
					shtm.Init(hitMargin);
                    texts[i] = shtm;
				}
				ctrlCachedHitTexts[hitMargin] = texts;
			}
		}
    }
}
