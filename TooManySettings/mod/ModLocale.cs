namespace TooManySettings.mod;

public static class ModLocale
{
    public static void Init()
    {
        void SetRunSettingText(string key, string text, string desc)
        {
            Locale.currentLang.other[$"runset{Plugin.Keyed(key)}"] = text;
            Locale.currentLang.other[$"runset{Plugin.Keyed(key)}dsc"] = desc;
        }

        SetRunSettingText("minibarrel_multiplier", "Minibarrel Multiplier", "The amount of minibarrels to spawn.");
        SetRunSettingText("enemy_multiplier", "Enemy Multiplier", "The amount of enemies to spawn.");
        SetRunSettingText("geyser_multiplier", "Geyser Multiplier", "The amount of geysers to spawn.");
        SetRunSettingText("plant_multiplier", "Plant Multiplier", "The amount of plants to spawn (glowplants, stoneplants, etc.).");
        SetRunSettingText("fluid_multiplier", "Fluid Multiplier", "The amount of fluid to place in each pool.");
        SetRunSettingText("spawn_random_loot", "Spawn Random Loot", "Spawns random items around each layer.");
        SetRunSettingText("random_loot_multiplier", "Random Loot Multiplier", "The amount of random items to spawn (only active if Spawn Random Loot is active).");
        SetRunSettingText("climbable_multiplier", "Climbable Multiplier", "The amount of climbable objects (rope, sandvine) to spawn.");
    }
}