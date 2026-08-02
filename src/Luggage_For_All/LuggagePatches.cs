using HarmonyLib;
using Photon.Pun;
using BepInEx.Logging;

namespace Luggage_For_All;

// Harmony patches that attach a luggage timer and sync its state across the network.
[HarmonyPatch(typeof(Luggage))]
public static class LuggagePatches
{
    private static ManualLogSource Log => Plugin.Log;

    // Ensures every luggage object receives its timer component when it wakes up.
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    public static void AddLuggageTimer(Luggage __instance)
    {
        string[] luggageTypes = Plugin.GetLuggageTypes();
        foreach (var luggageType in luggageTypes)
        {
            if (__instance.name.Contains(luggageType))
            {
                if (__instance.GetComponent<LuggageTimer>() == null)
                {
                    __instance.gameObject.AddComponent<LuggageTimer>();
                }
                return;
            }
        }
    }

    // When luggage is opened, tell all clients to start the shared countdown timer.
    [HarmonyPostfix]
    [HarmonyPatch("OpenLuggageRPC")]
    public static void SetLuggageTimer(Luggage __instance)
    {
        var timer = __instance.GetComponent<LuggageTimer>();

        if (timer == null)
            timer = __instance.gameObject.AddComponent<LuggageTimer>();

        if (PhotonNetwork.IsMasterClient)
        {
            __instance.photonView.RPC(
                "RPC_SetOpenedTime",
                RpcTarget.All,
                PhotonNetwork.Time,
                Plugin.RefillDuration.Value
            );
        }
    }
}
