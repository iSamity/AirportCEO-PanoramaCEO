using AirportCEOModLoader.WatermarkUtils;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using PanoramaCEO.Config;

namespace PanoramaCEO;

[BepInPlugin($"org.iSamity.{MyPluginInfo.PLUGIN_GUID}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("org.airportceomodloader.humoresque")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static ConfigFile ConfigReference { get; private set; }

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        ConfigReference = base.Config;

        DefaultConfig.Setup();

        SetupHarmony();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Start()
    {
        SetupModLoader();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} - Finished start");
    }


    private void SetupHarmony()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} - Setting up Harmony.");

        var harmony = new HarmonyLib.Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} - Finished up Harmony.");
    }

    private void SetupModLoader()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} - Setting up Mod Loader.");

#if DEBUG
        WatermarkUtils.Register(new WatermarkInfo(MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION, false));
#else
        WatermarkUtils.Register(new WatermarkInfo("PC", MyPluginInfo.PLUGIN_VERSION, true));
#endif
    }
}
