using PanoramaCEO.Config;
using PanoramaCEO.CameraZoom;
using UnityEngine;

namespace PanoramaCEO;

/// <summary>
/// Public API for other mods to read or set PanoramaCEO camera settings (e.g. zoom and pan).
/// When PanoramaCEO is loaded, use this class to set zoom min/max or pan speed so the camera updates immediately.
/// </summary>
/// <remarks>
/// <para>Add an optional dependency so your mod works with or without PanoramaCEO:</para>
/// <code>[BepInDependency(PanoramaCEO.Api.PluginId, BepInDependency.DependencyFlags.SoftDependency)]</code>
/// <para>Check if PanoramaCEO is loaded with <c>BepInEx.Bootstrap.Chainloader.Plugins.TryGetValue(Api.PluginId, out _)</c>.</para>
/// <para>Then use the static properties and methods below to get/set camera settings.</para>
/// </remarks>
public static class Api
{
    /// <summary>
    /// BepInEx plugin ID for PanoramaCEO. Use for optional dependency and Chainloader lookup.
    /// </summary>
    public const string PluginId = "org.iSamity." + MyPluginInfo.PLUGIN_GUID;

    private const float DefaultZoomMax = 350f;
    private const float ReferenceMapSize = 200f;

    /// <summary>
    /// Closest zoom level (smaller value = closer). Default 6. Minimum 6 (nothing renders below that). Min must be less than Max.
    /// </summary>
    public static float CameraZoomMin
    {
        get => DefaultConfig.CameraZoomMin.Value;
        set
        {
            value = Mathf.Max(value, CameraZoomService.MinZoomMin);
            var max = DefaultConfig.CameraZoomMax.Value;
            if (value >= max)
                value = max - 1f;
            DefaultConfig.CameraZoomMin.Value = value;
            CameraZoomService.ApplyZoomToActiveCamera();
        }
    }

    /// <summary>
    /// Furthest zoom level (larger value = further out). Default 350. Max must be greater than Min.
    /// </summary>
    public static float CameraZoomMax
    {
        get => DefaultConfig.CameraZoomMax.Value;
        set
        {
            var min = DefaultConfig.CameraZoomMin.Value;
            if (value <= min)
                value = min + 1f;
            DefaultConfig.CameraZoomMax.Value = value;
            CameraZoomService.ApplyZoomToActiveCamera();
        }
    }

    /// <summary>
    /// Multiplier for camera panning speed. Higher values = faster panning. Maximum 50 (higher values are too fast). Default 1.0.
    /// </summary>
    public static float CameraPanSpeedMultiplier
    {
        get => DefaultConfig.CameraPanSpeedMultiplier.Value;
        set => DefaultConfig.CameraPanSpeedMultiplier.Value = Mathf.Min(value, CameraZoomService.MaxPanSpeedMultiplier);
    }

    /// <summary>
    /// Sets a recommended zoom max based on map dimensions so the camera can zoom out to fit the map.
    /// Uses linear scaling from a reference map size (default zoom max 350 at reference size 200).
    /// Call this when your mod changes map size (e.g. after loading a larger map).
    /// </summary>
    /// <param name="mapWidth">Map width (e.g. in tiles or units).</param>
    /// <param name="mapHeight">Map height (e.g. in tiles or units).</param>
    public static void SetRecommendedZoomMaxForMapSize(float mapWidth, float mapHeight)
    {
        float maxDimension = Mathf.Max(mapWidth, mapHeight);
        float suggestedZoomMax = DefaultZoomMax * (maxDimension / ReferenceMapSize);
        suggestedZoomMax = Mathf.Clamp(suggestedZoomMax, DefaultConfig.CameraZoomMin.Value + 1f, 2000f);
        CameraZoomMax = suggestedZoomMax;
    }
}
