
using UnityEngine;

public class FishSpecimenScaler : MonoBehaviour
{
    [Header("Fish Visual")]
    [SerializeField] private Transform scalableVisual;

    [Header("Scaling Settings")]
    [SerializeField] private float scaleStep = 0.25f;
    [SerializeField] private float minimumScale = 0.5f;
    [SerializeField] private float maximumScale = 2.5f;

    private Vector3 originalScale;
    private float currentMultiplier = 1f;

    private void Awake()
    {
        if (scalableVisual == null)
        {
            Debug.LogWarning(
                "FishSpecimenScaler: Scalable visual not assigned!",
                this
            );
            enabled = false;
            return;
        }

        originalScale = scalableVisual.localScale;
    }

    public void EnlargeFish()
    {
        SetScale(currentMultiplier + scaleStep);
    }

    public void ShrinkFish()
    {
        SetScale(currentMultiplier - scaleStep);
    }

    public void ResetFishScale()
    {
        SetScale(1f);
    }

    private void SetScale(float multiplier)
    {
        currentMultiplier = Mathf.Clamp(
            multiplier,
            minimumScale,
            maximumScale
        );

        scalableVisual.localScale =
            originalScale * currentMultiplier;
    }
}
