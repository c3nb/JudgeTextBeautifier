using HarmonyLib;
using RDTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Reflection.Emit;
using UnityEngine.UI;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(scrController))]
    public static class sHTMtosHTMP
    {
        [HarmonyTranspiler]
        [HarmonyPatch("Awake")]
        public static IEnumerable<CodeInstruction> CtrlAwakePrefix(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> insts = new List<CodeInstruction>(instructions);
            insts.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Call, typeof(sHTMtosHTMP).GetMethod("SetTalmoTitle")),
                new CodeInstruction(OpCodes.Call, typeof(Settings).GetProperty("settings").GetGetMethod()),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Callvirt, typeof(Settings).GetMethod("Reset"))
            });
			int chtIndex = insts.FindIndex(inst => inst.opcode == OpCodes.Stfld && inst.operand is FieldInfo fi && fi.Name == "cachedHitTexts");
			insts.RemoveAt(chtIndex);
			insts.RemoveAt(chtIndex - 1);
			insts.Insert(chtIndex - 1, new CodeInstruction(OpCodes.Call, typeof(sHTMtosHTMP).GetMethod("InitSHT")));
			int bchmrkIndex = insts.FindIndex(inst => inst.opcode == OpCodes.Ldfld && inst.operand is FieldInfo fi && fi.Name == "benchmarkMode");
			chtIndex++;
			Main.mod.Logger.Log($"chtIndex:{insts[chtIndex]}, bchmarkIndex:{insts[bchmrkIndex]}");
			insts.RemoveRange(chtIndex, bchmrkIndex - chtIndex - 1);
            Main.mod.Logger.Log($"chtIndex:{insts[chtIndex]}, bchmarkIndex:{insts[bchmrkIndex]}");
			return insts;
        }
		public static void SetTalmoTitle()
		{
            Main.Talmos.ForEach(talmo => talmo.title = RDString.Get("HitMargin." + talmo.hitMargin.ToString()));
        }
		[HarmonyPrefix]
		[HarmonyPatch("ShowHitText")]
		public static bool CtrlShowHitTextPrefix(scrController __instance, HitMargin hitMargin, Vector3 position, float angle)
        {
			if (!Settings.settings.IsTalmo)
            {
				foreach (var shtm in chtRef(__instance)[hitMargin])
                {
					if (shtm.dead)
                    {
						TextMesh text = sHTMtosHTMP.text(shtm);
						text.richText = false;
						if (Main.HasOverlayer)
							text.text = Tags.GetResult(hitMargin);
						else
							switch (hitMargin)
							{
								case HitMargin.TooEarly:
									text.text = Settings.settings.TooEarly;
									break;
								case HitMargin.VeryEarly:
									text.text = Settings.settings.VeryEarly;
									break;
								case HitMargin.EarlyPerfect:
									text.text = Settings.settings.EarlyPerfect;
									break;
								case HitMargin.Perfect:
									text.text = Settings.settings.Perfect;
									break;
								case HitMargin.LatePerfect:
									text.text = Settings.settings.LatePerfect;
									break;
								case HitMargin.VeryLate:
									text.text = Settings.settings.VeryLate;
									break;
								case HitMargin.TooLate:
									text.text = Settings.settings.TooLate;
									break;
								case HitMargin.Multipress:
									text.text = Settings.settings.Multipress;
									break;
								case HitMargin.FailMiss:
									text.text = Settings.settings.FailMiss;
									break;
								case HitMargin.FailOverload:
									text.text = Settings.settings.FailOverload;
									break;
							}
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
					shtmp.Init(hitMargin);
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
					scrHitTextMesh shtm = UnityEngine.Object.Instantiate(RDConstants.data.hitTextPrefab, transform)
						.GetComponentInChildren<scrHitTextMesh>();
					shtm.Init(hitMargin);
					texts[i] = shtm;
				}
				ctrlCachedHitTexts[hitMargin] = texts;
			}
		}
    }
}
