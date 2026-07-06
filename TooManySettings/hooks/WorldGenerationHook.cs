using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TooManySettings.hooks;

using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Mono.Cecil.Cil;

public class WorldGenerationHook
{
    static ILHook hook;
    static ILHook hook2;

    private static readonly MethodInfo GetRunSettingFloat =
        AccessTools.Method(typeof(WorldGeneration), "GetRunSettingFloat", [typeof(string)]);

    public static void Apply()
    {
        hook = new ILHook(AccessTools.Method(typeof(WorldGeneration), "DistributeMiniBarrels"),
            DistributeMiniBarrelsManipulator);
        hook2 = new ILHook(
            AccessTools.Method(typeof(WorldGeneration).GetNestedTypes(BindingFlags.NonPublic)
                .First(x => x.Name.StartsWith("<WorldPlaceEntities>d__")), "MoveNext"),
            WorldPlaceEntitiesManipulator);
    }

    static void DistributeMiniBarrelsManipulator(ILContext il)
    {
        ILCursor c = new ILCursor(il);

        if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(0)))
        {
            Plugin.Logger.LogError("DistributeMiniBarrelsManipulator failed");
            return;
        }

        c.Emit(OpCodes.Ldloc_0);
        c.Emit(OpCodes.Ldstr, Plugin.Keyed("minibarrel_multiplier"));
        c.Emit(OpCodes.Call, GetRunSettingFloat);
        c.Emit(OpCodes.Mul);
        c.Emit(OpCodes.Stloc_0);
    }

    static void WorldPlaceEntitiesManipulator(ILContext il)
    {
        ILCursor c = new ILCursor(il);

        if (!c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(0.8f),
                x => x.MatchLdcR4(1f),
                x => x.MatchCall("UnityEngine.Random", "Range")
            ))
        {
            Plugin.Logger.LogError("WorldPlaceEntitiesManipulator failed 1");
        }

        c.EmitDelegate((float x) =>
            x * WorldGeneration.GetRunSettingFloat(Plugin.Keyed("climbable_multiplier")));


        c.Index = 0;

        if (!c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(6f),
                x => x.MatchLdcR4(7f),
                x => x.MatchCall("UnityEngine.Random", "Range")
            ))
        {
            Plugin.Logger.LogError("WorldPlaceEntitiesManipulator failed 2");
        }

        c.EmitDelegate((float x) =>
            x * WorldGeneration.GetRunSettingFloat(Plugin.Keyed("climbable_multiplier")));
    }
}