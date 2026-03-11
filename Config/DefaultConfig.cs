using BepInEx.Configuration;
using UnityEngine;
using PanoramaCEO.CameraZoom;

namespace PanoramaCEO.Config;

static class DefaultConfig
{
    internal static ConfigEntry<float> CameraZoomMin { get; private set; }
    internal static ConfigEntry<float> CameraZoomMax { get; private set; }
    internal static ConfigEntry<float> CameraPanSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ShowPanoramaWelcomeMessage { get; private set; }

    public static void Setup()
    {
        ShowPanoramaWelcomeMessage = ConfigReference.Bind("General", "Show PanoramaCEO Welcome Message", true, "Show welcome message explaining what the mod does on first run.");
        CameraZoomMin = ConfigReference.Bind("Camera", "Camera Zoom Min", 6f, "Closest zoom level (smaller value = closer). Minimum 6 because nothing renders below that. Default: 6");
        CameraZoomMin.SettingChanged += CameraZoomService.OnCameraZoomMinChanged;
        CameraZoomMax = ConfigReference.Bind("Camera", "Camera Zoom Max", 350f, "Furthest zoom level (larger value = further out). Default: 350");
        CameraZoomMax.SettingChanged += CameraZoomService.OnCameraZoomMaxChanged;
        CameraPanSpeedMultiplier = ConfigReference.Bind("Camera", "Camera Pan Speed Multiplier", 1.0f, "Multiplier for camera panning speed. Higher values = faster panning. Maximum 30 because higher values are too fast. Default: 1.0");
        CameraPanSpeedMultiplier.SettingChanged += CameraZoomService.OnCameraPanSpeedMultiplierChanged;
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
