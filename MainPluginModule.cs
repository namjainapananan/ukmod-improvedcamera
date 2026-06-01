
using BepInEx;
using HarmonyLib;
using PluginConfig.API;
using PluginConfig.API.Fields;
using ULTRAKILL.Enemy;

namespace ukmod_improvedcamera
{
    public struct PluginInfo
    {
        public const string PLUGIN_GUID = "com.gunnanthapop.ukmod_improvedcamera";
        public const string PLUGIN_NAME = "Improved Camera Movement";
        public const string PLUGIN_VERSION = "1.0.0";
    }

    public class PluginConfigVar
    {
        public BoolField enabledScreenshake;
        public FloatField screenshakeMaster;
        public FloatField screenshakeFrequency;
        public FloatField screenshakePitch;
        public FloatField screenshakeYaw;
        public FloatField screenshakeRoll;
        public BoolField enabledViewtilt;
        public FloatField viewtiltMaster;
        public FloatField viewtiltPitch;
        public FloatField viewtiltYaw;
        public FloatField viewtiltRoll;
        public FloatField viewtiltRecoveryFrequency;
        public FloatField viewtiltRecoveryDamping;

        public PluginConfigVar(PluginConfigurator config)
        {
            enabledScreenshake = new BoolField(config.rootPanel, "Enabled Screenshake", "enabledScreenshake", true);
            ConfigDivision screenshakeDivision = new ConfigDivision(config.rootPanel, "screenshakeDivision");
            screenshakeMaster = new FloatField(screenshakeDivision, "Master", "screenshakeMaster", 1.0f, 0f, 999f);
            screenshakeFrequency = new FloatField(screenshakeDivision, "Frequency", "screenshakeFrequency", 150f, 1f, 999f);
            screenshakePitch = new FloatField(screenshakeDivision, "Pitch", "screenshakePitch", 1.0f, 0f, 1.0f);
            screenshakeYaw = new FloatField(screenshakeDivision, "Yaw", "screenshakeYaw", 1.0f, 0f, 1.0f);
            screenshakeRoll = new FloatField(screenshakeDivision, "Roll", "screenshakeRoll", 1.0f, 0f, 1.0f);
            enabledScreenshake.onValueChange += (BoolField.BoolValueChangeEvent e) =>
            {
                screenshakeDivision.interactable = e.value;
            };

            enabledViewtilt = new BoolField(config.rootPanel, "Enabled Camera tilt", "enabledViewtilt", true);
            ConfigDivision viewtiltDivision = new ConfigDivision(config.rootPanel, "viewtiltDivision");
            viewtiltMaster = new FloatField(viewtiltDivision, "Master", "viewtiltMaster", 1.0f, 0f, 999f);
            viewtiltPitch = new FloatField(viewtiltDivision, "Pitch", "viewtiltPitch", 1.0f, 0f, 1.0f);
            viewtiltYaw = new FloatField(viewtiltDivision, "Yaw", "viewtiltYaw", 1.0f, 0f, 1.0f);
            viewtiltRoll = new FloatField(viewtiltDivision, "Roll", "viewtiltRoll", 1.0f, 0f, 1.0f);
            viewtiltRecoveryFrequency = new FloatField(viewtiltDivision, "Recovery Frequency", "viewtiltRecoveryFrequency", 2.5f, 0f, 999f);
            viewtiltRecoveryDamping = new FloatField(viewtiltDivision, "Damping Ratio", "viewtiltRecoveryDamping", 0.5f, 0f, 1.0f);
            enabledViewtilt.onValueChange += (BoolField.BoolValueChangeEvent e) =>
            {
                viewtiltDivision.interactable = e.value;
            };
        }
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class MainPluginModule : BaseUnityPlugin
    {
        public static PluginConfigVar pluginConfigVar;
        void Awake()
        {
            Harmony ham = new Harmony(PluginInfo.PLUGIN_GUID);
            ham.PatchAll();
            PluginConfigurator config = PluginConfigurator.Create(PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_GUID);
            pluginConfigVar = new PluginConfigVar(config);
        }    
    }
}
