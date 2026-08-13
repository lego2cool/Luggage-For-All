using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace Luggage_For_All;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    private Harmony _harmony = null!;

    // Config stuff
    internal static ConfigEntry<float> RefillDuration = null!;
    internal static ConfigEntry<float> TimerFontSize = null!;
    internal static ConfigEntry<bool> ShowTimer = null!;
    internal static ConfigEntry<int> MaxReopens = null!;

    internal static ConfigEntry<bool> LuggageSmall = null!;
    internal static ConfigEntry<bool> LuggageBig = null!;
    internal static ConfigEntry<bool> LuggageEpic = null!;
    internal static ConfigEntry<bool> LuggageAncient = null!;
    internal static ConfigEntry<bool> LuggageClown = null!;

    private void Awake()
    {
        Log = Logger;
        _harmony = new Harmony("legocool.Luggage_For_All");
        _harmony.PatchAll();

        // Config Setting Stuff
        RefillDuration = Config.Bind("General", "Refill Duration", 60f, "The number of seconds it takes for luggage to refill after being opened.");
        TimerFontSize = Config.Bind("General", "Timer Font Size", 5f, "The font size of the countdown timer displayed above luggage.");
        ShowTimer = Config.Bind("General", "Show Timer", true, "Whether to display the countdown timer above luggage.");
        MaxReopens = Config.Bind("General", "Max Reopens", -1, "The maximum number of times a luggage can be reopened before it stops refilling. Set to -1 for unlimited reopens.");
        
        LuggageSmall = Config.Bind("Luggages", "Luggage Small", true, "Whether the small luggage should refill.");
        LuggageBig = Config.Bind("Luggages", "Luggage Big", true, "Whether the big luggage should refill.");
        LuggageEpic = Config.Bind("Luggages", "Luggage Epic", true, "Whether the explorer luggage should refill.");
        LuggageAncient = Config.Bind("Luggages", "Luggage Ancient", true, "Whether the ancient luggage should refill.");
        LuggageClown = Config.Bind("Luggages", "Luggage Clown", true, "Whether the clown luggage should refill.");

        Log.LogInfo($"Plugin {Name} is loaded!");
    }

    public static string[] GetLuggageTypes()
    {
        var types = new List<string>();
        if (LuggageSmall.Value) types.Add("LuggageSmall");
        if (LuggageBig.Value) types.Add("LuggageBig");
        if (LuggageEpic.Value) types.Add("LuggageEpic");
        if (LuggageAncient.Value) types.Add("LuggageAncient");
        if (LuggageClown.Value) types.Add("LuggageClown");
        return types.ToArray();
    }
} 
