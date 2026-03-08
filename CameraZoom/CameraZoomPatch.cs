using HarmonyLib;
using UnityEngine;

namespace PanoramaCEO.CameraZoom;

[HarmonyPatch(typeof(GenericMoveCamera), nameof(GenericMoveCamera.Awake))]
internal class CameraZoomPatch
{
    [HarmonyPostfix]
    static void Postfix(GenericMoveCamera __instance)
    {
        var cameraController = __instance.GetComponent<CameraController>();
        var mainCamera = cameraController?.mainCamera ?? __instance.GetComponent<Camera>();
        CameraZoomService.ApplyCameraSettings(__instance, mainCamera);
    }
}

