using System;
using BS_Utils.Utilities;
using HarmonyLib;

namespace SmoothCameraUnlimiter.Patches
{
    [HarmonyPatch(typeof(SmoothCameraSmoothnessSettingsController))]
    [HarmonyPatch("GetInitValues")]
    [HarmonyPatch(new Type[] { typeof(int), typeof(int) }, new ArgumentType[] {ArgumentType.Out, ArgumentType.Out })]
    internal class ScsscGetInitValuesPatch
    {
        public static bool Prefix(ref bool __result, SmoothCameraSmoothnessSettingsController __instance, ref int idx, ref int numberOfElements)
        {
            var smoothnesses = new float[]
            {
                20f,
                18f,
                16f,
                14f,
                12f,
                10f,
                8f,
                6f,
                4f,
                2f,
                1f,
                0.75f,
                0.5f,
                0.25f,
                0.125f
            };
            __instance.SetPrivateField("_smoothnesses", smoothnesses);
            FloatSO cameraPositionSmooth = __instance.GetPrivateField<FloatSO>("_smoothCameraPositionSmooth");
            idx = 2;
            numberOfElements = smoothnesses.Length;
            for (int index = 0; index < smoothnesses.Length; ++index)
            {
                if ((double)(float)((ObservableVariableSO<float>)cameraPositionSmooth) == (double)smoothnesses[index])
                {
                    idx = index;
                    break;
                }
            }
            __result = true;
            return false;
        }
    }
}
