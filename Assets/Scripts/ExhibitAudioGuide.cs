
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExhibitAudioGuide : MonoBehaviour
{
    [Header("Audio Guide")]
    [SerializeField] private AudioClip narrationClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    public void PlayNarration()
    {
        if (narrationClip == null)
        {
            Debug.LogWarning(
                "Audio guide has no narration clip assigned!",
                this
            );
            return;
        }

        audioSource.Stop();
        audioSource.clip = narrationClip;
        audioSource.Play();
    }

    public void StopNarration()
    {
        audioSource.Stop();
    }

    public void ToggleNarration()
    {
        if (audioSource.isPlaying)
        {
            StopNarration();
        }
        else
        {
            PlayNarration();
        }
    }
}
