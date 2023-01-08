using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityModManagerNet;

namespace JudgeTextBeautifier
{
    [HarmonyPatch(typeof(UnityModManager.ModEntry), "Active", MethodType.Setter)]
    public static class CheckIfHasOverlayer
    {
        public static void Postfix(UnityModManager.ModEntry __instance, bool value)
        {
            try
            {
                if (__instance.Info.Id == "Overlayer")
                {
                    if (Main.HasOverlayer = value)
                    {
                        Tags.Enable();
                        OnChange(Settings.settings);
                    }
                    else Tags.Disable();
                }
            }
            catch { }
        }
        public static readonly Action<Settings> OnChange = (Action<Settings>)typeof(Settings).GetMethod("OnChange").CreateDelegate(typeof(Action<Settings>));
    }
}
