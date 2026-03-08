# Airport CEO - PanoramaCEO Plugin

Plugin that extends camera zoom and pan in Airport CEO. Lets you zoom out further than the default and configure zoom min/max and pan speed. Other plugins can use the public API to set camera settings at runtime (e.g. when using a custom map size).

## Use plugin in Airport CEO

### Manual installation

1. Download the dll from [GitHub releases](https://github.com/iSamity/AirportCEO-PanoramaCEO/releases).
2. Reference the dll in your plugin project, or copy it to your Airport CEO plugins folder.

### Steam

Go to the [Steam workshop page](https://steamcommunity.com/sharedfiles/filedetails/?id=YOUR_STEAM_ID) and subscribe. Don't forget to install the required [AirportCEO Mod Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=3109136766).

## Dev

### Local installation

1. Clone the repository.
2. Open the project in Visual Studio and Build → Rebuild.
3. Copy the generated `PanoramaCEO.dll` to your Airport CEO plugins directory, typically located at `AirportCEO/BepInEx/plugins/`.

### Installation

How to use the plugin in your own plugin:

#### Manual installation

1. Download the dll from [releases](https://github.com/iSamity/AirportCEO-PanoramaCEO/releases).
2. Reference the dll in your plugin project.

Don't forget to add an optional dependency so your mod works with or without PanoramaCEO:

```csharp
[BepInDependency(PanoramaCEO.Api.PluginId, BepInDependency.DependencyFlags.SoftDependency)]
```

### API reference – configurable settings

All of these are on `PanoramaCEO.Api`. You can **read** (get) or **set** them; when you set zoom min/max, the camera updates immediately.

| Member | Type | Description | Default |
|--------|------|-------------|---------|
| `PluginId` | `const string` | BepInEx plugin ID. Use for `[BepInDependency(...)]` and `Chainloader.Plugins.TryGetValue(Api.PluginId, ...)`. | `"org.iSamity.PanoramaCEO"` |
| `CameraZoomMin` | `float` (get/set) | Closest zoom level (smaller = closer). Must be less than `CameraZoomMax`. | `6f` |
| `CameraZoomMax` | `float` (get/set) | Furthest zoom level (larger = further out). Must be greater than `CameraZoomMin`. | `350f` |
| `CameraPanSpeedMultiplier` | `float` (get/set) | Multiplier for camera panning speed. Higher = faster panning. | `1f` |
| `SetRecommendedZoomMaxForMapSize(float mapWidth, float mapHeight)` | method | Sets a recommended zoom max from map dimensions (e.g. after loading a larger map). Uses linear scaling; call when your mod changes map size. | — |

### Usage

Use the public API to read or set camera zoom and pan from your mod. Settings apply immediately and are persisted in BepInEx config.

```csharp
using BepInEx;
using BepInEx.Bootstrap;

[BepInPlugin(/* your plugin id, name, version */)]
[BepInDependency(PanoramaCEO.Api.PluginId, BepInDependency.DependencyFlags.SoftDependency)]
public class YourPlugin : BaseUnityPlugin
{
    private void Start()
    {
        if (!Chainloader.Plugins.TryGetValue(PanoramaCEO.Api.PluginId, out _))
            return;

        // Set zoom max so the camera can zoom out further (e.g. for a larger map)
        PanoramaCEO.Api.CameraZoomMax = 500f;

        // Or set a recommended zoom max from your map dimensions
        PanoramaCEO.Api.SetRecommendedZoomMaxForMapSize(mapWidth, mapHeight);

        // Optional: adjust zoom min or pan speed
        PanoramaCEO.Api.CameraZoomMin = 6f;
        PanoramaCEO.Api.CameraPanSpeedMultiplier = 1.2f;
    }
}
```
