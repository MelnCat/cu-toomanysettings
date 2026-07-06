using System.Collections.Generic;

namespace TooManySettings.settings;

public static class CustomSettings
{
    private static readonly Dictionary<string, CustomSetting> Settings = new();

    public static void Init()
    {
        Register(new RunSettingFloat(Plugin.Keyed("minibarrel_multiplier"))
        {
            limits = new RangeF(0f, 100f),
            postfix = "x"
        }, 1f);
        Register(new RunSettingFloat(Plugin.Keyed("enemy_multiplier"))
        {
            limits = new RangeF(0f, 20f),
            postfix = "x"
        }, 1f);
        Register(new RunSettingFloat(Plugin.Keyed("geyser_multiplier"))
        {
            limits = new RangeF(0f, 20f),
            postfix = "x"
        }, 1f);
        Register(new RunSettingFloat(Plugin.Keyed("plant_multiplier"))
        {
            limits = new RangeF(0f, 20f),
            postfix = "x"
        }, 1f);
        Register(new RunSettingFloat(Plugin.Keyed("fluid_multiplier"))
        {
            limits = new RangeF(0f, 20f),
            postfix = "x"
        }, 1f);
        Register(new RunSettingFloat(Plugin.Keyed("climbable_multiplier"))
        {
            limits = new RangeF(0f, 20f),
            postfix = "x"
        }, 1f);
        Register(new RunSettingBool(Plugin.Keyed("spawn_random_loot"))
        {
        }, false);
        Register(new RunSettingFloat(Plugin.Keyed("random_loot_multiplier"))
        {
            limits = new RangeF(0f, 20f),
            postfix = "x"
        }, 1f);

        foreach (var setting in Settings.Values)
        {
            RunSettings.GetPreset("normal").presetValues[setting.Name] = setting.Default;
            RunSettings.settingTypes.Add(setting.Setting);
        }
    }

    private static void Register(RunSetting setting, object defaultValue)
    {
        Settings.Add(setting.name, new CustomSetting
        {
            Setting = setting,
            Default = defaultValue,
        });
    }
}