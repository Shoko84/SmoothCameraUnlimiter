using System;
using BS_Utils.Utilities;
using HarmonyLib;

namespace SmoothCameraUnlimiter.Patches
{
    [HarmonyPatch(typeof(SmoothCameraSmoothnessSettingsController))]
    [HarmonyPatch("TextForValue")]
    [HarmonyPatch(new Type[] { typeof(int) })]
    internal class ScsscTextForValuePatch
    {
        public static bool Prefix(ref string __result, SmoothCameraSmoothnessSettingsController __instance, int idx)
        {
            var smoothnesses = __instance.GetPrivateField<float[]>("_smoothnesses");
            __result = string.Format("{0:0.00}", (object)(float)(1.0 - ((double)smoothnesses[idx] - (double)smoothnesses[smoothnesses.Length - 1]) / ((double)smoothnesses[0] - (double)smoothnesses[smoothnesses.Length - 1])));
            return false;
        }
    }
}
