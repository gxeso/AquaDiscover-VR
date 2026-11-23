using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FishFeedingZone : MonoBehaviour
{
    [Header("Food Container")]
    [SerializeField] private XRGrabInteractable foodContainer;

    [Header("Fish")]
    [SerializeField] private FishFeedingMovement[] fishToFeed;

    [Header("Feeding Target")]
    [SerializeField] private Transform feedingTarget;

    [Header("Food Spawner")]
    [SerializeField] private FishFoodSpawner foodSpawner;

    [Header("Feeding Progress")]
    [SerializeField] private FishFeedingProgress feedingProgress;

    [Header("UI")]
    [SerializeField] private TMP_Text feedingStatusText;

    [Header("Audio")]
    [SerializeField] private AudioSource feedingAudio;

    private bool feedingCompleted = false;

    private void Start()
    {
        UpdateStatus(
            "Feed the fish: Bring the food container to the tank."
        );
    }

    private void OnTriggerStay(Collider other)
    {
        if (feedingCompleted)
            return;

        XRGrabInteractable detectedObject =
            other.GetComponentInParent<XRGrabInteractable>();

        if (detectedObject == null)
            return;

        if (detectedObject != foodContainer)
            return;

        if (detectedObject.isSelected)
            return;

        CompleteFeeding();
    }

    private void CompleteFeeding()
    {
        feedingCompleted = true;

        UpdateStatus("Fish feeding complete!");

        Vector3 targetPosition =
            feedingTarget != null
                ? feedingTarget.position
                : transform.position;

        // Start tracking pellets eaten.
        if (feedingProgress != null)
        {
            feedingProgress.BeginFeeding();
        }

        // Tell fish to swim toward the food.
        if (fishToFeed != null)
        {
            foreach (FishFeedingMovement fish in fishToFeed)
            {
                if (fish != null)
                {
                    fish.StartFeeding(targetPosition);
                }
            }
        }

        // Spawn visible food pellets.
        if (foodSpawner != null)
        {
            foodSpawner.SpawnFood(targetPosition);
        }

        // Play the feeding sound once.
        if (feedingAudio != null && feedingAudio.clip != null)
        {
            feedingAudio.PlayOneShot(feedingAudio.clip);
        }

        Debug.Log("AquaDiscover: Fish feeding completed!");
    }

    private void UpdateStatus(string message)
    {
        if (feedingStatusText != null)
        {
            feedingStatusText.text = message;
        }
    }
}