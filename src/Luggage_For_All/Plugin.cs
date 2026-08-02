using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

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

    private void Awake()
    {
        Log = Logger;
        _harmony = new Harmony("legocool.Luggage_For_All");
        _harmony.PatchAll();

        // Config Setting Stuff
        RefillDuration = Config.Bind("General", "Refill Duration", 60f, "The number of seconds it takes for luggage to refill after being opened.");
        TimerFontSize = Config.Bind("General", "Timer Font Size", 5f, "The font size of the countdown timer displayed above luggage.");
        ShowTimer = Config.Bind("General", "Show Timer", true, "Whether to display the countdown timer above luggage.");
        
        Log.LogInfo($"Plugin {Name} is loaded!");
    }
} 
