using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System.Linq;
using System.Collections;
using Luggage_For_All;

// Displays a countdown above luggage and refills it when the timer expires.
public class LuggageTimer : MonoBehaviourPun
{
    // The network time when the luggage was opened, used to calculate the remaining countdown.
    public double lastOpenedTime;

    // How long the luggage stays open before it refills.
    public float duration = 0f;

    // The text object shown above the luggage.
    private TextMeshPro text = null!;

    // ===== Font Management =====
    private static TMP_FontAsset? _gameFontAsset = null;
    private static bool _fontSearchAttempted = false;

    // Creates the timer UI text and initializes its visual style.
    void Start()
    {
        text = new GameObject("TimerText").AddComponent<TextMeshPro>();
        text.transform.SetParent(transform);
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = Plugin.TimerFontSize.Value;
        text.color = Color.green;

        var font = GameFont;
        if (font != null)
        {
            text.font = font;
        }
        else
        {
            Plugin.Log.LogWarning("Using fallback TMP font (Darumadrop One not found).");
        }

        text.fontMaterial.EnableKeyword("OUTLINE_ON");
        text.outlineWidth = 0.05f;
        text.outlineColor = new Color32(0, 0, 0, 255);
    }

    // Called over the network to begin the countdown for a luggage object.
    [PunRPC]
    public void RPC_SetOpenedTime(double time, float SetTime)
    {
        text.gameObject.SetActive(Plugin.ShowTimer.Value);
        lastOpenedTime = time;
        duration = SetTime;
    }

    // Updates the countdown each frame and refills luggage once time runs out.
    void Update()
    {
        if (lastOpenedTime <= 0) return;

        float timeLeft = duration - (float)(PhotonNetwork.Time - lastOpenedTime);

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            text.gameObject.SetActive(false);

            // Only the master client should trigger the refill so the state stays synchronized.
            if (PhotonNetwork.IsMasterClient)
            {
                Vector3 luggagepos = transform.position;
                Quaternion luggagerot = transform.rotation;

                string spawnName = name;
                string[] luggageTypes = { "LuggageSmall", "LuggageBig", "LuggageEpic", "LuggageAncient" };

                foreach (var luggageType in luggageTypes)
                {
                    if (spawnName.Contains(luggageType, System.StringComparison.OrdinalIgnoreCase))
                    {
                        spawnName = luggageType;
                        break;
                    }
                }

                PhotonNetwork.Instantiate($"0_Items/{spawnName}", luggagepos, luggagerot);
                PhotonNetwork.Destroy(base.gameObject);

                //luggage.photonView.RPC("RefillLuggage", RpcTarget.All);
                lastOpenedTime = 0; // reset
            }
        }

        // Update the displayed countdown string.
        int totalSeconds = Mathf.CeilToInt(timeLeft);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        text.text = $"{minutes}:{seconds:00}";

        // Position the timer above the luggage object.
        text.transform.position = transform.position + Vector3.up * 2f;

        // Make the text face the camera for readability.
        if (Camera.main != null)
        {
            text.transform.forward = Camera.main.transform.forward;
        }
    }

    // Lazily finds the game's TMP font once and caches it for later use.
    public static TMP_FontAsset? GameFont
    {
        get
        {
            if (_gameFontAsset == null && !_fontSearchAttempted)
            {
                _gameFontAsset = FindGameFont();
                _fontSearchAttempted = true;
            }
            return _gameFontAsset;
        }
    }

    // Searches loaded resources for a TMP font matching the game's font family.
    private static TMP_FontAsset? FindGameFont()
    {
        try
        {
            var allFonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            Plugin.Log?.LogInfo($"Found {allFonts.Length} TMP fonts in resources");

            // Try the exact family names used by the game.
            var font = allFonts.FirstOrDefault(fontAsset =>
                fontAsset.faceInfo.familyName == "Darumadrop One" ||
                fontAsset.faceInfo.familyName == "Daruma Drop One"
            );
            if (font != null)
            {
                Plugin.Log?.LogInfo($"Found game font: {font.name} (Family: {font.faceInfo.familyName})");
                return font;
            }

            // Log available fonts for debugging when the expected font is missing.
            Plugin.Log?.LogWarning("Available TMP fonts:");
            foreach (var f in allFonts)
            {
                Plugin.Log?.LogWarning($" - {f.name} (Family: {f.faceInfo.familyName})");
            }

            return null;
        }
        catch (System.Exception e)
        {
            Plugin.Log?.LogWarning($"Error finding game font: {e.Message}");
            return null;
        }
    }

    // Resets luggage to its closed state so it can be opened again.
    [PunRPC]
    private void RefillLuggage()
    {
        var luggage = GetComponent<Luggage>();

        // Reset the gameplay state to closed.
        luggage.state = Luggage.LuggageState.Closed;

        // Reset the animator to the closed animation frame.
        luggage.anim.Play("Closed", 0, 0f);

        // Add the luggage back to the global list so it can be opened again.
        if (!Luggage.ALL_LUGGAGE.Contains(luggage))
        {
            Luggage.ALL_LUGGAGE.Add(luggage);
        }
    }
}
