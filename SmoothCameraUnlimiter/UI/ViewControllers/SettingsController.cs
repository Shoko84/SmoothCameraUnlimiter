using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace SmoothCameraUnlimiter.UI.ViewControllers
{
    public class SettingsController : PersistentSingleton<SettingsController>
    {
        [UIParams] private BSMLParserParams parserParams;

        [UIValue("enable-tps-bool")]
        public bool enabledTpsValue {
            get => _mainSettingsModel.smoothCameraThirdPersonEnabled;
            set {
                var bSo = ScriptableObject.CreateInstance<BoolSO>();
                bSo.value = value;
                _mainSettingsModel.smoothCameraThirdPersonEnabled = bSo;
            }
        }

        [UIValue("tps-x-pos-float")]
        public float tpsXPos {
            get => _mainSettingsModel.smoothCameraThirdPersonPosition.value.x;
            set
            {
                var bSo = ScriptableObject.CreateInstance<Vector3SO>();
                bSo.value = new Vector3(value,
                                        _mainSettingsModel.smoothCameraThirdPersonPosition.value.y,
                                        _mainSettingsModel.smoothCameraThirdPersonPosition.value.z);
                _mainSettingsModel.smoothCameraThirdPersonPosition = bSo;
            }
        }

        [UIValue("tps-y-pos-float")]
        public float tpsYPos {
            get => _mainSettingsModel.smoothCameraThirdPersonPosition.value.y;
            set
            {
                var bSo = ScriptableObject.CreateInstance<Vector3SO>();
                bSo.value = new Vector3(_mainSettingsModel.smoothCameraThirdPersonPosition.value.x,
                                        value,
                                        _mainSettingsModel.smoothCameraThirdPersonPosition.value.z);
                _mainSettingsModel.smoothCameraThirdPersonPosition = bSo;
            }
        }

        [UIValue("tps-z-pos-float")]
        public float tpsZPos {
            get => _mainSettingsModel.smoothCameraThirdPersonPosition.value.z;
            set
            {
                var bSo = ScriptableObject.CreateInstance<Vector3SO>();
                bSo.value = new Vector3(_mainSettingsModel.smoothCameraThirdPersonPosition.value.x,
                                        _mainSettingsModel.smoothCameraThirdPersonPosition.value.y,
                                        value);
                _mainSettingsModel.smoothCameraThirdPersonPosition = bSo;
            }
        }

        [UIValue("tps-x-rot-float")]
        public float tpsXRot {
            get => _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.x;
            set
            {
                var bSo = ScriptableObject.CreateInstance<Vector3SO>();
                bSo.value = new Vector3(value,
                                        _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.y,
                                        _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.z);
                _mainSettingsModel.smoothCameraThirdPersonEulerAngles = bSo;
            }
        }

        [UIValue("tps-y-rot-float")]
        public float tpsYRot {
            get => _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.y;
            set
            {
                var bSo = ScriptableObject.CreateInstance<Vector3SO>();
                bSo.value = new Vector3(_mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.x,
                                        value,
                                        _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.z);
                _mainSettingsModel.smoothCameraThirdPersonEulerAngles = bSo;
            }
        }

        [UIValue("tps-z-rot-float")]
        public float tpsZRot {
            get => _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.z;
            set
            {
                var bSo = ScriptableObject.CreateInstance<Vector3SO>();
                bSo.value = new Vector3(_mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.x,
                                        _mainSettingsModel.smoothCameraThirdPersonEulerAngles.value.y,
                                        value);
                _mainSettingsModel.smoothCameraThirdPersonEulerAngles = bSo;
            }
        }

        [UIObject("pos-x-field")] public GameObject posXField;
        [UIObject("pos-y-field")] public GameObject posYField;
        [UIObject("pos-z-field")] public GameObject posZField;
        [UIObject("rot-x-field")] public GameObject rotXField;
        [UIObject("rot-y-field")] public GameObject rotYField;
        [UIObject("rot-z-field")] public GameObject rotZField;

        private MainSettingsModelSO _mainSettingsModel;

        private void Awake()
        {
            _mainSettingsModel = Resources.FindObjectsOfTypeAll<MainSettingsModelSO>().FirstOrDefault();
        }

        private static void ResizeValuePicker(GameObject go)
        {
            if (go == null) return;
            var rectPicker = go.transform.Find("ValuePicker")?.GetComponent<RectTransform>();
            if (rectPicker)
                rectPicker.sizeDelta = new Vector2(25, rectPicker.sizeDelta.y);
        }

        [UIAction("#post-parse")]
        internal void Setup()
        {
            var list = new List<GameObject> {
                posXField, posYField, posZField, rotXField, rotYField, rotZField
            };
            foreach (var go in list)
                ResizeValuePicker(go);
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            _mainSettingsModel = Resources.FindObjectsOfTypeAll<MainSettingsModelSO>().FirstOrDefault();
            if (_mainSettingsModel == null) return;
            var bSo = ScriptableObject.CreateInstance<BoolSO>();
            bSo.value = enabledTpsValue;
            var vPosSo = ScriptableObject.CreateInstance<Vector3SO>();
            vPosSo.value = new Vector3(tpsXPos, tpsYPos, tpsZPos);
            var vRotSo = ScriptableObject.CreateInstance<Vector3SO>();
            vRotSo.value = new Vector3(tpsXRot, tpsYRot, tpsZRot);
            _mainSettingsModel.smoothCameraThirdPersonEnabled = bSo;
            _mainSettingsModel.smoothCameraThirdPersonPosition = vPosSo;
            _mainSettingsModel.smoothCameraThirdPersonEulerAngles = vRotSo;
            _mainSettingsModel.Save();
        }
    }
}