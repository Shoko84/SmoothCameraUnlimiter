using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using SmoothCameraUnlimiter.UI.ViewControllers;
using UnityEngine;
using Config = IPA.Config.Config;
using Logger = IPA.Logging.Logger;
using ReflectionUtil = BS_Utils.Utilities.ReflectionUtil;

namespace SmoothCameraUnlimiter
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        #region Properties

        public static Logger Log { get; private set; }

        #endregion

        #region BSIPA Events

        [Init]
        public Plugin(Logger logger, Config conf)
        {
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            BSEvents.menuSceneLoadedFresh += AddSettingsSubMenu;
            BSEvents.menuSceneLoadedFresh += UnlimitSmoothCamera;
            var harmony = new Harmony("com.Shoko84.beatsaber.SmoothCameraUnlimiter");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Events

        private static void AddSettingsSubMenu()
        {
            BSMLSettings.instance.AddSettingsMenu("<size=75%>Smooth Camera Unlimiter</size>", "SmoothCameraUnlimiter.UI.Views.settings.bsml", SettingsController.instance);
        }

        private static void UnlimitSmoothCamera()
        {
            var scssc = Resources.FindObjectsOfTypeAll<SmoothCameraSmoothnessSettingsController>().FirstOrDefault();
            if (scssc == null) return;
            var fovSettings = scssc.transform?.parent?.Find("FieldOfView");
            if (fovSettings == null) return;
            var fflsvc = fovSettings.GetComponent<FormattedFloatListSettingsValueController>();
            if (fflsvc == null) return;
            var fs = ReflectionUtil.GetPrivateField<float[]>(fflsvc, "_values")?.ToList();
            if (fs == null) return;
            for (var f = fs[fs.Count - 1] + 5; f <= 200; f += 5)
                fs.Add(f);
            ReflectionUtil.SetPrivateField(fflsvc, "_values", fs.ToArray());
        }

        #endregion
    }
}