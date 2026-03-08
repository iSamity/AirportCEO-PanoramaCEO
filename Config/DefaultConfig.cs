using BepInEx.Configuration;
using UnityEngine;
using PanoramaCEO.CameraZoom;

namespace PanoramaCEO.Config;

static class DefaultConfig
{
    internal static ConfigEntry<float> CameraZoomMin { get; private set; }
    internal static ConfigEntry<float> CameraZoomMax { get; private set; }
    internal static ConfigEntry<float> CameraPanSpeedMultiplier { get; private set; }

    public static void Setup()
    {
        CameraZoomMin = ConfigReference.Bind("Camera", "Camera Zoom Min", 6f, "Closest zoom level (smaller value = closer). Default: 6");
        CameraZoomMin.SettingChanged += CameraZoomService.OnCameraZoomMinChanged;
        CameraZoomMax = ConfigReference.Bind("Camera", "Camera Zoom Max", 350f, "Furthest zoom level (larger value = further out). Default: 350");
        CameraZoomMax.SettingChanged += CameraZoomService.OnCameraZoomMaxChanged;
        CameraPanSpeedMultiplier = ConfigReference.Bind("Camera", "Camera Pan Speed Multiplier", 1.0f, "Multiplier for camera panning speed. Higher values = faster panning. Default: 1.0");
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
