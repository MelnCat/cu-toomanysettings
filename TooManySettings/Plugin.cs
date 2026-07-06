using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using TooManySettings.hooks;
using TooManySettings.settings;
using UnityEngine;

namespace TooManySettings
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "melncat.toomanysettings";
        public const string ModName = "TooManySettings";
        public const string ModVersion = "1.0.0";

        internal static new ManualLogSource Logger;
        private readonly Harmony _harmony = new(ModGUID);
        public static Plugin Instance { get; private set; } = null!;

        public static string Keyed(string str)
        {
            return $"toomanysettings:{str}";
        }

        void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            _harmony.PatchAll();
            Logger.LogInfo($"Plugin {ModName} is loaded!");

            try
            {
                WorldGenerationHook.Apply();
                CustomSettings.Init();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        void OnDestroy()
        {
            _harmony?.UnpatchSelf();
            Instance = null!;
        }
    }
}