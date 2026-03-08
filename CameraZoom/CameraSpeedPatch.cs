using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PanoramaCEO.Config;
using UnityEngine;

namespace PanoramaCEO.CameraZoom;

/// <summary>
/// Patches GenericMoveCamera.Update to adjust MovementSpeedMagnification
/// AFTER it's calculated (line 191) but BEFORE it's used for WASD panning (lines 231+).
/// When zoom range is extended beyond 350, recomputes magnification from a normalized zoom
/// parameter (0 = zoomed in, 1 = zoomed out) using the game's MovementSpeed curve, so WASD
/// feels like vanilla across the full range. Otherwise applies only the user pan speed multiplier.
/// </summary>
[HarmonyPatch(typeof(GenericMoveCamera), nameof(GenericMoveCamera.Update))]
internal class CameraSpeedPatch
{
    private const float DefaultZoomMax = 350f;

    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var found = false;

        // Find where MovementSpeedMagnification is assigned (game's line 191)
        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode != OpCodes.Stfld || codes[i].operand is not FieldInfo field)
                continue;
            if (field.Name != "MovementSpeedMagnification")
                continue;

            // Insert: load 'this', then call our multiplier (runs right after the assignment)
            codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldarg_0));
            codes.Insert(i + 2, new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(CameraSpeedPatch), nameof(ApplySpeedMultiplier))));
            found = true;
            break;
        }

        if (!found)
            Plugin.Logger.LogWarning("[CameraSpeedPatch] Could not find MovementSpeedMagnification assignment in Update.");

        return codes;
    }

    static void ApplySpeedMultiplier(GenericMoveCamera instance)
    {
        var currentSpeed = instance.MovementSpeedMagnification;
        var configuredMax = DefaultConfig.CameraZoomMax.Value;
        var userMultiplier = DefaultConfig.CameraPanSpeedMultiplier.Value;

        if (configuredMax > DefaultZoomMax)
        {
            // Extended zoom range: recompute magnification from normalized zoom so WASD feels like vanilla
            var z = instance.transform.position.z;
            var rangeSpan = instance.ZRangeMin - instance.ZRangeMax;
            var t = rangeSpan != 0f
                ? Mathf.Clamp01((z - instance.ZRangeMax) / rangeSpan)
                : 0f;
            var curveMin = instance.MovementSpeed.Evaluate(0f);
            var curveMax = instance.MovementSpeed.Evaluate(1f);
            var magnification = Mathf.Lerp(curveMin, curveMax, t) * instance.MovementSpeedMultiplier;
            instance.MovementSpeedMagnification = magnification * userMultiplier;
        }
        else
        {
            instance.MovementSpeedMagnification = currentSpeed * userMultiplier;
        }
    }
}
