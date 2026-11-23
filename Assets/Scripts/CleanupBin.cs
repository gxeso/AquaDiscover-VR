using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class CleanupBin : MonoBehaviour
{
    [Header("Cleanup Progress")]
    public TMP_Text progressText;
    public int totalLitter = 3;

    [Header("Mission Board")]
    [SerializeField] private AquariumMissionManager missionManager;

    [Header("Collection Audio")]
    public AudioSource collectionAudio;

    [Header("Completion Audio")]
    public AudioSource completionAudio;

    [Header("Collection Haptics")]
    public HapticImpulsePlayer leftHaptics;
    public HapticImpulsePlayer rightHaptics;

    [Range(0f, 1f)]
    public float vibrationStrength = 0.7f;

    public float vibrationDuration = 0.15f;

    private readonly HashSet<int> collected = new();

    private bool completionPlayed = false;

    private void Start()
    {
        if (collectionAudio == null)
        {
            collectionAudio = GetComponent<AudioSource>();
        }

        UpdateDisplay();
        UpdateMissionProgress();
    }

    private void OnTriggerStay(Collider other)
    {
        XRGrabInteractable litter =
            other.GetComponentInParent<XRGrabInteractable>();

        if (litter == null)
            return;

        if (!litter.CompareTag("Litter"))
            return;

        // Do not collect while the object is held.
        if (litter.isSelected)
            return;

        // Prevent duplicate collections.
        if (!collected.Add(litter.GetInstanceID()))
            return;

        // Remove litter.
        Destroy(litter.gameObject);

        // Play collection sound.
        if (collectionAudio != null &&
            collectionAudio.clip != null)
        {
            collectionAudio.PlayOneShot(collectionAudio.clip);
        }

        // Vibrate left controller.
        if (leftHaptics != null)
        {
            leftHaptics.SendHapticImpulse(
                vibrationStrength,
                vibrationDuration
            );
        }

        // Vibrate right controller.
        if (rightHaptics != null)
        {
            rightHaptics.SendHapticImpulse(
                vibrationStrength,
                vibrationDuration
            );
        }

        // Update the original cleanup UI.
        UpdateDisplay();

        // Update the Mission Board.
        UpdateMissionProgress();
    }

    private void UpdateDisplay()
    {
        if (progressText != null)
        {
            progressText.text =
                $"Litter collected: {collected.Count} / {totalLitter}";

            if (totalLitter > 0 &&
                collected.Count >= totalLitter)
            {
                progressText.text += "\nCleanup complete!";
            }
        }

        // Play the completion sound once.
        if (totalLitter > 0 &&
            collected.Count >= totalLitter &&
            !completionPlayed)
        {
            completionPlayed = true;

            if (completionAudio != null &&
                completionAudio.clip != null)
            {
                completionAudio.PlayOneShot(
                    completionAudio.clip
                );
            }
        }
    }

    private void UpdateMissionProgress()
    {
        if (missionManager != null)
        {
            missionManager.UpdateCleanupProgress(
                collected.Count
            );
        }
    }
}