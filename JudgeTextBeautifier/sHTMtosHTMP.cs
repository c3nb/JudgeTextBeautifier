using HarmonyLib;
using RDTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(scrController))]
    public static class sHTMtosHTMP
    {
		static readonly MethodInfo saveDataSetup = typeof(scrController).GetMethod("SaveDataSetup", AccessTools.all);
        [HarmonyPrefix]
        [HarmonyPatch("Awake")]
        public static bool CtrlAwakePrefix(scrController __instance, ref scrDpadInputChecker ___dpadInputChecker, ref List<scrPlanet> ___availablePlanets, ref bool ___benchmarkMode)
        {
			Settings.settings.Reset();
			if (!Settings.settings.IsTalmo) return true;
			ADOBase.Startup();
			RDUtils.SetGarbageCollectionEnabled(true);
			if (__instance.gameworld)
			{
				__instance.MakeNewFontDictionary();
			}
			if (!__instance.gameworld || GCS.standaloneLevelMode)
			{
				__instance.FlushUnusedMemory();
			}
			___dpadInputChecker = __instance.gameObject.GetOrAddComponent<scrDpadInputChecker>();
			__instance.redPlanet = GameObject.Find("PlanetRed").GetComponent<scrPlanet>();
			__instance.bluePlanet = __instance.redPlanet.other;
			__instance.planetList = new List<scrPlanet>();
			__instance.planetList.Add(__instance.redPlanet);
			__instance.planetList.Add(__instance.bluePlanet);
			GameObject gameObject = __instance.redPlanet.transform.parent.gameObject;
			for (int i = 0; i < __instance.planetList.Count; i++)
			{
				__instance.planetList[i].planetIndex = i;
				__instance.planetList[i].next = __instance.GetMultiPlanet(i, 1);
				__instance.planetList[i].prev = __instance.GetMultiPlanet(i, -1);
			}
			___availablePlanets = new List<scrPlanet>();
			__instance.canExitLevel = true;
			__instance.planetGreen = UnityEngine.Object.Instantiate<scrPlanet>(__instance.bluePlanet, gameObject.transform);
			__instance.planetGreen.name = "PlanetGreen";
			__instance.planetGreen.EnableCustomColor();
			__instance.planetGreen.SetPlanetColor(new Color(0.3f, 0.7f, 0f, 1f));
			__instance.planetGreen.SetTailColor(new Color(0.3f, 0.7f, 0f, 1f));
			__instance.planetGreen.isExtra = true;
			__instance.planetGreen.Destroy();
			__instance.planetYellow = UnityEngine.Object.Instantiate<scrPlanet>(__instance.bluePlanet, gameObject.transform);
			__instance.planetYellow.name = "PlanetYellow";
			__instance.planetYellow.EnableCustomColor();
			__instance.planetYellow.SetPlanetColor(new Color(1f, 0.8f, 0f, 1f));
			__instance.planetYellow.SetTailColor(new Color(1f, 0.8f, 0f, 1f));
			__instance.planetYellow.isExtra = true;
			__instance.planetYellow.Destroy();
			__instance.planetPurple = UnityEngine.Object.Instantiate<scrPlanet>(__instance.bluePlanet, gameObject.transform);
			__instance.planetPurple.name = "PlanetPurple";
			__instance.planetPurple.EnableCustomColor();
			__instance.planetPurple.SetPlanetColor(new Color(0.7f, 0.1f, 1f, 1f));
			__instance.planetPurple.SetTailColor(new Color(0.7f, 0.1f, 1f, 1f));
			__instance.planetPurple.isExtra = true;
			__instance.planetPurple.Destroy();
			__instance.planetPink = UnityEngine.Object.Instantiate<scrPlanet>(__instance.bluePlanet, gameObject.transform);
			__instance.planetPink.name = "PlanetPink";
			__instance.planetPink.EnableCustomColor();
			__instance.planetPink.SetPlanetColor(new Color(1f, 0.1f, 0.7f, 1f));
			__instance.planetPink.SetTailColor(new Color(1f, 0.1f, 0.7f, 1f));
			__instance.planetPink.isExtra = true;
			__instance.planetPink.Destroy();
			__instance.planetOrange = UnityEngine.Object.Instantiate<scrPlanet>(__instance.bluePlanet, gameObject.transform);
			__instance.planetOrange.name = "PlanetOrange";
			__instance.planetOrange.EnableCustomColor();
			__instance.planetOrange.SetPlanetColor(new Color(1f, 0.4f, 0.1f, 1f));
			__instance.planetOrange.SetTailColor(new Color(1f, 0.4f, 0.1f, 1f));
			__instance.planetOrange.isExtra = true;
			__instance.planetOrange.Destroy();
			__instance.planetCyan = UnityEngine.Object.Instantiate<scrPlanet>(__instance.bluePlanet, gameObject.transform);
			__instance.planetCyan.name = "PlanetCyan";
			__instance.planetCyan.EnableCustomColor();
			__instance.planetCyan.SetPlanetColor(new Color(0.1f, 0.8f, 0.9f, 1f));
			__instance.planetCyan.SetTailColor(new Color(0.1f, 0.8f, 0.9f, 1f));
			__instance.planetCyan.isExtra = true;
			__instance.planetCyan.Destroy();
			___availablePlanets.Add(__instance.planetGreen);
			___availablePlanets.Add(__instance.planetYellow);
			___availablePlanets.Add(__instance.planetPurple);
			___availablePlanets.Add(__instance.planetPink);
			___availablePlanets.Add(__instance.planetOrange);
			___availablePlanets.Add(__instance.planetCyan);
			__instance.allPlanets = new List<scrPlanet>();
			__instance.allPlanets.Add(__instance.redPlanet);
			__instance.allPlanets.Add(__instance.bluePlanet);
			__instance.allPlanets.Add(__instance.planetGreen);
			__instance.allPlanets.Add(__instance.planetYellow);
			__instance.allPlanets.Add(__instance.planetPurple);
			__instance.allPlanets.Add(__instance.planetPink);
			__instance.allPlanets.Add(__instance.planetOrange);
			__instance.allPlanets.Add(__instance.planetCyan);
			__instance.dummyPlanets = new List<scrPlanet>();
			__instance.multiPlanetLines = new List<LineRenderer>();
			__instance.lineMaterial = new Material(Shader.Find("ADOFAI/ScrollingSprite"));
			__instance.lineMaterial.SetTexture("_MainTex", __instance.gc.planetPolygonTex);
			__instance.lineMaterial.SetVector("_ScrollSpeed", new Vector2(-0.4f, 0f));
			__instance.lineMaterial.SetFloat("_Time0", 0f);
			__instance.lineColor = new Color(1f, 1f, 1f, 0.5f);
			Application.runInBackground = true;
			if (RDC.customCheckpoint && __instance.isUnityEditor)
			{
				RDC.customCheckpoint = false;
				GCS.checkpointNum = RDC.customCheckpointPos;
			}
			else
			{
				bool practiceMode = GCS.practiceMode;
			}
			if (GCS.previousScene == null)
			{
				GCS.previousScene = __instance.gameObject.scene.name;
			}
			if (GCS.previousScene != __instance.gameObject.scene.name)
			{
				GCS.checkpointNum = 0;
				GCS.previousScene = GCS.sceneToLoad;
			}
			__instance.levelName = __instance.gameObject.scene.name;
			__instance.responsive = true;
			__instance.lockInput = 0f;
			if (GCS.turnOnBenchmarkMode)
			{
				___benchmarkMode = true;
				GCS.turnOnBenchmarkMode = false;
			}
			RDC.auto = false;
			if (__instance.mobileMenu)
			{
				GCNS.sceneLevelSelect = "scnMobileMenu";
			}
			__instance.stateMachine.Initialize<scrController.States>(__instance);
			if (__instance.gameworld)
			{
				if (__instance.background == null)
				{
					__instance.background = GameObject.Find("BG");
					if (__instance.background == null)
					{
						__instance.background = GameObject.Find("Tutorial BG");
					}
				}
				if (ADOBase.IsCurrentLevelBossLevel())
				{
					if (__instance.visualQuality == VisualQuality.High)
					{
						__instance.background.SetActive(true);
					}
					else if (__instance.lofiBackground != null)
					{
						Sprite sprite = __instance.lofiBackground.GetComponent<SpriteRenderer>().sprite;
						if (sprite)
						{
							Rect rect = sprite.rect;
							float num = 1f / rect.height * 10f;
							float x = (float)Screen.width * 1f / (float)Screen.height / (rect.width * 1f / rect.height) * 1.0005f * num;
							__instance.printe(string.Format("texture [{0} x {1}] height: {2}, Screen {3} x {4}", new object[]
							{
						rect.width,
						rect.height,
						num,
						Screen.width,
						Screen.height
							}));
							__instance.lofiBackground.transform.localScale = new Vector3(x, num, 1f);
							__instance.lofiBackground.SetActive(true);
						}
						else
						{
							MonoBehaviour.print("oh no sprite is null, lets just ignore the sprite entirely");
						}
					}
				}
			}
			if (!__instance.gameworld || GCS.speedTrialMode)
			{
				GCS.checkpointNum = 0;
			}
			if (__instance.levelName.Contains("-"))
			{
				scrUIController.instance.canvas.enabled = true;
				__instance.txtCaption = scrUIController.instance.txtLevelName;
				__instance.txtCaption.SetLocalizedFont();
				__instance.caption = ADOBase.GetLocalizedLevelName(__instance.levelName);
				string text = __instance.levelName.Substring(0, __instance.levelName.IndexOf('-'));
				bool flag = __instance.worldData.ContainsKey(text);
				if (!flag)
				{
					__instance.printe(scrController.currentWorldString + " is not present in WorldData...");
				}
				scrController.currentWorldString = (flag ? text : "Template");
				RDBaseDll.printem("currentWorldString is " + scrController.currentWorldString);
			}
			if (ADOBase.isMobile)
			{
				Button pauseButton = scrUIController.instance.pauseButton;
				pauseButton.gameObject.SetActive(true);
				pauseButton.onClick.AddListener(delegate ()
				{
					__instance.TogglePauseGame();
				});
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(__instance.gc.canvasPrefab);
			if (!GCS.lofiVersion || ADOBase.isMobile)
			{
				__instance.pauseMenu = gameObject2.transform.Find("RDPauseMenu").GetComponent<PauseMenu>();
				if ((float)Screen.width / (float)Screen.height < 1.5f)
				{
					gameObject2.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.2f;
					RectTransform component = __instance.pauseMenu.settingsMenu.GetComponent<RectTransform>();
					component.offsetMax = component.offsetMax.WithY(-20f);
				}
				__instance.takeScreenshot = __instance.camy.GetOrAddComponent<TakeScreenshot>();
			}
			__instance.mistakesManager = new scrMistakesManager(__instance.controller, __instance.lm);
			if (GCS.checkpointNum == 0)
			{
				__instance.mistakesManager.Reset();
				scrController.checkpointsUsed = 0;
			}
			__instance.mistakesManager.RevertToLastCheckpoint();
			if (__instance.gameworld)
			{
				__instance.errorMeter = UnityEngine.Object.Instantiate(__instance.gc.errorMeterPrefab).GetComponent<scrHitErrorMeter>();
				__instance.errorMeter.gameObject.SetActive(false);
				__instance.errorMeter.UpdateLayout(Persistence.GetHitErrorMeterSize(), Persistence.GetHitErrorMeterShape());
			}
			saveDataSetup.Invoke(__instance, null);
			__instance.Awake_Rewind();
			if (__instance.gameworld)
			{
				scrUIController.instance.PrepareWipeFromBlack();
				InitSHTMP(__instance);
			}
			Main.Talmos.ForEach(talmo => talmo.title = RDString.Get("HitMargin." + talmo.hitMargin.ToString()));
			if (___benchmarkMode)
			{
				__instance.gameObject.AddComponent<scrBenchmark>();
			}
			if (ADOBase.IsHalloweenWeek() && __instance.conductor != null && __instance.conductor.song != null && __instance.conductor.song.clip != null && __instance.conductor.song.clip.name.StartsWith("1-X"))
			{
				__instance.conductor.song.clip = __instance.gc.halloweenMusic;
				RDBaseDll.printem("it's Halloween! conductor song is now: " + __instance.conductor.song.clip.name);
			}
			ctrlLastTimeStatsUploaded = Time.unscaledTime;
			if (!__instance.gameworld)
			{
				SteamIntegration.EditorEntered();
			}
			__instance.noFail = GCS.useNoFail;
			return false;
        }
		[HarmonyPrefix]
		[HarmonyPatch("ShowHitText")]
		public static bool CtrlShowHitTextPrefix(scrController __instance, HitMargin hitMargin, Vector3 position, float angle)
        {
			if (!Settings.settings.IsTalmo)
            {
				foreach (var shtm in cht(__instance)[hitMargin])
                {
					if (shtm.dead)
                    {
						TextMesh text = sHTMtosHTMP.text(shtm);
						text.richText = false;
						if (Main.HasOverlayer)
							text.text = Overlayer.GetResult(hitMargin);
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
        public static float ctrlLastTimeStatsUploaded
        {
			get => ltsu();
			set => ltsu() = value;
        }
		public static Dictionary<HitMargin, scrHitTextMesh[]> ctrlCachedHitTexts
		{
			get => cht(scrController.instance);
			set => cht(scrController.instance) = value;
		}
		public static AccessTools.FieldRef<scrController, Dictionary<HitMargin, scrHitTextMesh[]>> cht = AccessTools.FieldRefAccess<scrController, Dictionary<HitMargin, scrHitTextMesh[]>>("cachedHitTexts");
		public static AccessTools.FieldRef<scrHitTextMesh, TextMesh> text = AccessTools.FieldRefAccess<scrHitTextMesh, TextMesh>("text");
		static AccessTools.FieldRef<float> ltsu = AccessTools.StaticFieldRefAccess<float>(typeof(scrController).GetField("lastTimeStatsUploaded", AccessTools.all));
		public static scrHitTextMeshPro[][] cachedHitTexts = new scrHitTextMeshPro[10][];
		public static void InitSHTMP(scrController ctrl)
        {
			ctrl.hitTextContainer = new GameObject("HitTexts");
			Transform transform = ctrl.hitTextContainer.transform;
			cachedHitTexts = new scrHitTextMeshPro[10][];
			foreach (HitMargin hitMargin in Talmo.hitMargins)
			{
				scrHitTextMeshPro[] array3 = new scrHitTextMeshPro[100];
				for (int k = 0; k < 100; k++)
				{
					GameObject go = new GameObject();
					scrHitTextMeshPro shtmp = go.AddComponent<scrHitTextMeshPro>();
					go.transform.SetParent(transform);
					shtmp.Init(hitMargin);
					array3[k] = shtmp;
				}
				cachedHitTexts[(int)hitMargin] = array3;
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
				scrHitTextMesh[] array3 = new scrHitTextMesh[100];
				for (int k = 0; k < 100; k++)
				{
					scrHitTextMesh componentInChildren = UnityEngine.Object.Instantiate(ctrl.gc.hitTextPrefab, transform)
						.GetComponentInChildren<scrHitTextMesh>();
					componentInChildren.Init(hitMargin);
					array3[k] = componentInChildren;
				}
				ctrlCachedHitTexts[hitMargin] = array3;
			}
		}
    }
}
