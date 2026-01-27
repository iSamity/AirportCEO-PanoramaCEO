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

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
