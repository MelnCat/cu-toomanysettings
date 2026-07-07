using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

namespace TooManySettings.patches;

[HarmonyPatch(typeof(WorldGeneration))]
internal static class WorldGenerationPatch
{
    private static HashSet<string> plants =
    [
        "glowplant",
        "stoneplant",
        "ceilingrye",
        "geotree",
        "hydreed",
        "leadbush",
        "cactus",
        "sandrose",
        "drybush",
        "brownshroom"
    ];

    private static List<string> categories = ["medical", "drug", "food", "water", "tool", "utility", "container", "trash"];

    [HarmonyPatch(nameof(WorldGeneration.DistributeEntities))]
    [HarmonyPrefix]
    private static void DistributeEntities(GameObject basObj,
        ref float minPerChunk,
        ref float maxPerChunk,
        float spawnYOffset,
        float randomRotation,
        float spawnYOffsetDeviation,
        bool spawnInGround,
        bool randomFlip,
        WorldGeneration.PlaceCheckDelegate checkFunc,
        bool isTrap,
        Vector2 dir,
        bool forceFlip)
    {
        float? multiplier = null;
        if (basObj.TryGetComponent<BuildingEntity>(out var component))
        {
            if (component.animal)
            {
                multiplier = WorldGeneration.GetRunSettingFloat(Plugin.Keyed("enemy_multiplier"));
            }
        }

        if (basObj.TryGetComponent<CaveTickSpawner>(out var component2))
        {
            multiplier = WorldGeneration.GetRunSettingFloat(Plugin.Keyed("enemy_multiplier"));
        }

        if (basObj.name == "geyser")
        {
            multiplier = WorldGeneration.GetRunSettingFloat(Plugin.Keyed("geyser_multiplier"));
        }

        if (plants.Contains(basObj.name))
        {
            multiplier = WorldGeneration.GetRunSettingFloat(Plugin.Keyed("plant_multiplier"));
        }

        if (multiplier != null)
        {
            minPerChunk *= multiplier.Value;
            maxPerChunk *= multiplier.Value;
        }
    }


    [HarmonyPatch(nameof(WorldGeneration.WorldPlaceEntities))]
    [HarmonyPostfix]
    public static void WorldPlaceEntities(WorldGeneration __instance)
    {
        if (!WorldGeneration.GetRunSettingBool(Plugin.Keyed("spawn_random_loot"))) return;
        float num = (float)(__instance.chunkWidth * __instance.chunkHeight) * UnityEngine.Random.Range(1f, 2f) *
                    __instance.lootRarityMultiplier * WorldGeneration.GetRunSettingFloat(Plugin.Keyed("random_loot_multiplier"));
        for (int i = 0; (float)i < num; i++)
        {
            Vector2 vector = new Vector2(UnityEngine.Random.Range(-__instance.halfWidth, __instance.halfWidth),
                UnityEngine.Random.Range(-__instance.halfHeight, PlayerCamera.main.body.transform.position.y - 5f));
            if (Physics2D.OverlapPoint(vector, LayerMask.GetMask("Ground")))
            {
                continue;
            }

            RaycastHit2D raycastHit2D =
                Physics2D.Raycast(vector, Vector2.down, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
            if (raycastHit2D)
            {
                var pool = ItemLootPool.pool[categories[Random.Range(0, categories.Count)]];
                if (pool == null) continue;
                var item = Resources.Load(pool[Random.Range(0, pool.Count)]);

                Plugin.Logger.LogInfo($"GOT HERE {item.name}");
                GameObject obj = (GameObject)UnityEngine.Object.Instantiate(item,
                    raycastHit2D.point + Vector2.up, Quaternion.identity);
                obj.GetComponent<Item>().SetCondition(Random.Range(0f, 1f));
            }
        }
    }

    [HarmonyPatch(nameof(WorldGeneration.PlaceLiquids))]
    [HarmonyPrefix]
    public static void PlaceLiquids(ref float perChunk, byte type, ref int maxFill)
    {
        perChunk *= WorldGeneration.GetRunSettingFloat(Plugin.Keyed("fluid_multiplier"));
        maxFill = (int)(maxFill * WorldGeneration.GetRunSettingFloat(Plugin.Keyed("fluid_multiplier")));
    }
}