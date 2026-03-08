using System;
using BepInEx.Configuration;
using PanoramaCEO.Config;
using UnityEngine;

namespace PanoramaCEO.CameraZoom;

internal static class CameraZoomService
{
    private const float DefaultZoomMax = 350f;
    private const float DefaultWheelMouseMultiplier = 10000f;

    /// <summary>
    /// Applies camera zoom settings to the given GenericMoveCamera instance.
    /// Called from CameraZoomPatch on initialization and from ApplyZoomToActiveCamera for live updates.
    /// Note: Movement speed scaling is handled by CameraSpeedPatch in the Update method.
    /// </summary>
    internal static void ApplyCameraSettings(GenericMoveCamera genericMoveCamera, Camera mainCamera)
    {
        // Convert positive display values to negative values for the camera system
        genericMoveCamera.ZRangeMax = -DefaultConfig.CameraZoomMin.Value;
        genericMoveCamera.ZRangeMin = -DefaultConfig.CameraZoomMax.Value;

        // Always refresh wheel multiplier so reset/default values are applied immediately.
        float zoomRatio = DefaultConfig.CameraZoomMax.Value / DefaultZoomMax;
        float effectiveZoomRatio = Mathf.Max(1f, zoomRatio);
        genericMoveCamera.WheelMouseMultiplier = DefaultWheelMouseMultiplier * effectiveZoomRatio;
        Plugin.Logger.LogInfo($"[CameraZoomService] Wheel multiplier set to: {genericMoveCamera.WheelMouseMultiplier}");

        // Adjust far clip plane to accommodate extreme zoom out
        if (mainCamera != null)
        {
            float requiredFarClip = DefaultConfig.CameraZoomMax.Value + 100f;
            if (mainCamera.farClipPlane < requiredFarClip)
            {
                mainCamera.farClipPlane = requiredFarClip;
                Plugin.Logger.LogInfo($"[CameraZoomService] Camera far clip plane adjusted to: {requiredFarClip}");
            }
        }

        Plugin.Logger.LogInfo($"[CameraZoomService] Camera zoom set - Min: {DefaultConfig.CameraZoomMin.Value}, Max: {DefaultConfig.CameraZoomMax.Value}");
    }

    internal static void OnCameraZoomMinChanged(object sender, EventArgs e)
    {
        var min = DefaultConfig.CameraZoomMin.Value;
        var max = DefaultConfig.CameraZoomMax.Value;

        // Min should be less than max (when positive, e.g., 6 < 350)
        // This ensures when converted to negative: -6 > -350
        if (min >= max)
        {
            // Set min to be slightly below max
            DefaultConfig.CameraZoomMin.Value = max - 1f;
            Plugin.Logger.LogWarning($"[CameraZoomService] Camera Zoom Min must be less than Max. Adjusted to: {DefaultConfig.CameraZoomMin.Value}");
        }

        ApplyZoomToActiveCamera();
    }

    internal static void OnCameraZoomMaxChanged(object sender, EventArgs e)
    {
        var min = DefaultConfig.CameraZoomMin.Value;
        var max = DefaultConfig.CameraZoomMax.Value;

        // Max should be greater than min (when positive, e.g., 350 > 6)
        // This ensures when converted to negative: -350 < -6
        if (max <= min)
        {
            // Set max to be slightly above min
            DefaultConfig.CameraZoomMax.Value = min + 1f;
            Plugin.Logger.LogWarning($"[CameraZoomService] Camera Zoom Max must be greater than Min. Adjusted to: {DefaultConfig.CameraZoomMax.Value}");
        }

        ApplyZoomToActiveCamera();
    }

    private static void ApplyZoomToActiveCamera()
    {
        var cameraController = Singleton<CameraController>.Instance;
        if (cameraController == null)
        {
            return;
        }

        var genericMoveCamera = cameraController.GetComponent<GenericMoveCamera>();
        if (genericMoveCamera == null)
        {
            return;
        }

        ApplyCameraSettings(genericMoveCamera, cameraController.mainCamera);
    }
}

