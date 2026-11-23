using TMPro;
using UnityEngine;

public class AquariumMissionManager : MonoBehaviour
{
    [Header("Mission Board UI")]
    [SerializeField] private TMP_Text missionBoardText;

    [Header("Mission Requirements")]
    [SerializeField] private int requiredSpecies = 2;
    [SerializeField] private int requiredLitter = 3;

    [Header("Victory Celebration")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private AudioSource victoryAudio;

    private int discoveredSpecies = 0;
    private int collectedLitter = 0;
    private bool feedingCompleted = false;
    private bool allMissionsCompleted = false;

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        UpdateMissionBoard();
    }

    public void UpdateDiscoveryProgress(int speciesCount)
    {
        discoveredSpecies = Mathf.Max(0, speciesCount);
        CheckAllMissions();
    }

    public void UpdateCleanupProgress(int litterCount)
    {
        collectedLitter = Mathf.Max(0, litterCount);
        CheckAllMissions();
    }

    public void CompleteFeedingMission()
    {
        feedingCompleted = true;
        CheckAllMissions();
    }

    private void CheckAllMissions()
    {
        bool discoveryDone = discoveredSpecies >= requiredSpecies;
        bool cleanupDone = collectedLitter >= requiredLitter;
        bool feedingDone = feedingCompleted;

        if (discoveryDone && cleanupDone && feedingDone)
        {
            if (!allMissionsCompleted)
            {
                allMissionsCompleted = true;
                TriggerVictoryCelebration();
            }
        }

        UpdateMissionBoard();
    }

    private void TriggerVictoryCelebration()
    {
        Debug.Log("AquaDiscover: OCEAN GUARDIAN UNLOCKED!");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (victoryAudio != null && victoryAudio.clip != null)
        {
            victoryAudio.PlayOneShot(victoryAudio.clip);
        }
    }

    private void UpdateMissionBoard()
    {
        if (missionBoardText == null)
            return;

        bool discoveryDone = discoveredSpecies >= requiredSpecies;
        bool cleanupDone = collectedLitter >= requiredLitter;
        bool feedingDone = feedingCompleted;

        int completedCount = 0;

        if (discoveryDone) completedCount++;
        if (cleanupDone) completedCount++;
        if (feedingDone) completedCount++;

        string discoverySymbol = discoveryDone ? "[X]" : "[ ]";
        string cleanupSymbol = cleanupDone ? "[X]" : "[ ]";
        string feedingSymbol = feedingDone ? "[X]" : "[ ]";

        missionBoardText.text =
            "AQUADISCOVER\n" +
            "OCEAN GUARDIAN MISSIONS\n\n" +
            $"{discoverySymbol} Discover {requiredSpecies} Fish Species " +
            $"({discoveredSpecies}/{requiredSpecies})\n" +
            $"{cleanupSymbol} Collect {requiredLitter} Pieces of Litter " +
            $"({collectedLitter}/{requiredLitter})\n" +
            $"{feedingSymbol} Feed the Aquarium Fish\n\n" +
            $"Missions Completed: {completedCount}/3\n\n";

        if (allMissionsCompleted)
        {
            missionBoardText.text +=
                "OCEAN GUARDIAN UNLOCKED!\n" +
                "Congratulations!";
        }
        else
        {
            missionBoardText.text +=
                "Complete all missions to become\n" +
                "an Ocean Guardian!";
        }
    }
}