
using BepInEx;
using HarmonyLib;

namespace ukmod_improvedcamera
{
    public struct PluginInfo
    {
        public const string PLUGIN_GUID = "com.gunnanthapop.ukmod_improvedcamera";
        public const string PLUGIN_NAME = "Improved Camera Movement";
        public const string PLUGIN_VERSION = "1.0.0";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class MainPluginModule : BaseUnityPlugin
    {
        void Awake()
        {
            Harmony ham = new Harmony(PluginInfo.PLUGIN_GUID);
            ham.PatchAll();
        }
    }
}
