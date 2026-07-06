using System;
using HarmonyLib;
using TooManySettings.mod;

namespace TooManySettings.patches;

[HarmonyPatch(typeof(Locale))]
internal static class LocalePatch
{
    [HarmonyPatch(nameof(Locale.LoadLanguage))]
    [HarmonyPostfix]
    private static void LoadLanguage()
    {
        try
        {
            ModLocale.Init();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e.ToString());
        }
    }
}