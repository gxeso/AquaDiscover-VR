using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscoveryTracker : MonoBehaviour
{
    [Header("Discovery UI")]
    public TMP_Text progressText;

    [Header("Species Settings")]
    public int totalSpecies = 1;

    [Header("Mission Board")]
    [SerializeField] private AquariumMissionManager missionManager;

    private readonly HashSet<string> discovered = new();

    private void Start()
    {
        UpdateDisplay();
        UpdateMissionProgress();
    }

    public void DiscoverSpecies(string speciesId)
    {
        if (string.IsNullOrWhiteSpace(speciesId))
            return;

        // Only count each species once.
        if (discovered.Add(speciesId))
        {
            UpdateDisplay();
            UpdateMissionProgress();

            Debug.Log(
                $"AquaDiscover: Species discovered! " +
                $"Total: {discovered.Count}/{totalSpecies}"
            );
        }
    }

    private void UpdateDisplay()
    {
        if (progressText == null)
            return;

        progressText.text =
            $"Species discovered: {discovered.Count} / {totalSpecies}";

        if (totalSpecies > 0 && discovered.Count >= totalSpecies)
        {
            progressText.text +=
                "\n\nDiscovery complete!\nYou inspected every species.";
        }
    }

    private void UpdateMissionProgress()
    {
        if (missionManager != null)
        {
            missionManager.UpdateDiscoveryProgress(discovered.Count);
        }
    }
}