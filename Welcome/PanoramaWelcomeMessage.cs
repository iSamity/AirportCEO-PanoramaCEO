using AirportCEOModLoader.Core;
using HarmonyLib;
using PanoramaCEO.Config;

namespace PanoramaCEO.Welcome;

[HarmonyPatch(typeof(UpdatePanelUI), nameof(UpdatePanelUI.DisplayOnlyUpdateButtons))]
internal static class PanoramaWelcomeMessage
{
    private const string WelcomeMessageText =
        "Welcome to PanoramaCEO!\n\n" +
        "This mod gives you more control over the camera:\n" +
        "• Zoom out further and in closer than the base game allows\n" +
        "• Adjust pan speed to move across your airport at your preferred pace\n\n" +
        "Default settings match the vanilla game. To customize Zoom Min/Max and Pan Speed, open the mod configuration menu (F1).";

    [HarmonyPostfix]
    static void ShowWelcomeIfFirstRun()
    {
        if (!DefaultConfig.ShowPanoramaWelcomeMessage.Value)
            return;

        DialogUtils.QueueDialog(WelcomeMessageText);
        DefaultConfig.ShowPanoramaWelcomeMessage.Value = false;
    }
}
