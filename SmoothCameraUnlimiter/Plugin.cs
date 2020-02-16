using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Utilities;
using Harmony;
using IPA;
using IPA.Config;
using IPA.Utilities;
using SmoothCameraUnlimiter.UI.ViewControllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Config = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;
using ReflectionUtil = BS_Utils.Utilities.ReflectionUtil;

namespace SmoothCameraUnlimiter
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static Ref<PluginConfig> config;
        internal static IConfigProvider   configProvider;

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            BSEvents.menuSceneLoadedFresh += AddSettingsSubMenu;
            BSEvents.menuSceneLoadedFresh += UnlimitSmoothCamera;
            configProvider = cfgProvider;

            config = cfgProvider.MakeLink<PluginConfig>((p, v) => {
                if (v.Value == null || v.Value.RegenerateConfig)
                    p.Store(v.Value = new PluginConfig { RegenerateConfig = false });
                config = v;
            });
        }

        private void AddSettingsSubMenu()
        {
            BSMLSettings.instance.AddSettingsMenu("<size=75%>Smooth Camera Unlimiter</size>", "SmoothCameraUnlimiter.UI.Views.settings.bsml", SettingsController.instance);
        }

        private void UnlimitSmoothCamera()
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

        public void OnApplicationStart()
        {
            var harmony = HarmonyInstance.Create("com.Shoko84.beatsaber.SmoothCameraUnlimiter");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnApplicationQuit() { }

        public void OnFixedUpdate() { }

        public void OnUpdate() { }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }

        public void OnSceneUnloaded(Scene scene) { }
    }
}