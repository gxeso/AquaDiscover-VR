using TMPro;
using UnityEngine;

public class FishFeedingProgress : MonoBehaviour
{
    [Header("Progress UI")]
    [SerializeField] private TMP_Text progressText;

    [Header("Feeding Settings")]
    [SerializeField] private int totalPellets = 12;

    [Header("Mission Board")]
    [SerializeField] private AquariumMissionManager missionManager;

    private int pelletsEaten = 0;
    private bool feedingStarted = false;
    private bool feedingCompleted = false;

    private void OnEnable()
    {
        FishFoodPellet.OnPelletEaten += HandlePelletEaten;
    }

    private void OnDisable()
    {
        FishFoodPellet.OnPelletEaten -= HandlePelletEaten;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    public void BeginFeeding()
    {
        if (feedingStarted)
            return;

        pelletsEaten = 0;
        feedingStarted = true;
        feedingCompleted = false;

        UpdateDisplay();

        Debug.Log("AquaDiscover: Feeding mission started!");
    }

    private void HandlePelletEaten()
    {
        if (!feedingStarted || feedingCompleted)
            return;

        pelletsEaten++;

        UpdateDisplay();

        Debug.Log(
            $"AquaDiscover: Feeding progress {pelletsEaten}/{totalPellets}"
        );

        // Check whether every pellet has been eaten.
        if (totalPellets > 0 && pelletsEaten >= totalPellets)
        {
            CompleteFeeding();
        }
    }

    private void CompleteFeeding()
    {
        if (feedingCompleted)
            return;

        feedingCompleted = true;

        Debug.Log(
            "AquaDiscover: Feeding mission completed!"
        );

        // Notify the Aquarium Mission Manager.
        if (missionManager != null)
        {
            missionManager.CompleteFeedingMission();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (progressText == null)
            return;

        progressText.text =
            "Fish Feeding Progress\n" +
            $"Pellets eaten: {pelletsEaten} / {totalPellets}";

        if (feedingCompleted)
        {
            progressText.text +=
                "\nFeeding successful!";
        }
    }
}