using HarmonyLib;
using UnityEngine;

namespace PanoramaCEO.CameraZoom;

[HarmonyPatch(typeof(GenericMoveCamera), nameof(GenericMoveCamera.Awake))]
internal class CameraZoomPatch
{
    [HarmonyPostfix]
    static void Postfix(GenericMoveCamera __instance)
    {
        var mainCamera = __instance.GetComponent<Camera>();
        CameraZoomService.ApplyCameraSettings(__instance, mainCamera);
    }
}

