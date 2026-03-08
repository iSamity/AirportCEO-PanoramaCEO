using AirportCEOModLoader.Core;
using HarmonyLib;
using PanoramaCEO.Config;

namespace PanoramaCEO.Welcome;

[HarmonyPatch(typeof(UpdatePanelUI), nameof(UpdatePanelUI.DisplayOnlyUpdateButtons))]
internal static class PanoramaWelcomeMessage
{
    private const string WelcomeMessageText =
        "PanoramaCEO extends the camera zoom range and lets you adjust pan speed. " +
        "Defaults are the same as the normal game—you can change Zoom Min/Max and Pan Speed in the configuration menu (F1) if you want.";

    [HarmonyPostfix]
    static void ShowWelcomeIfFirstRun()
    {
        if (!DefaultConfig.ShowPanoramaWelcomeMessage.Value)
            return;

        DialogUtils.QueueDialog(WelcomeMessageText);
        DefaultConfig.ShowPanoramaWelcomeMessage.Value = false;
    }
}
