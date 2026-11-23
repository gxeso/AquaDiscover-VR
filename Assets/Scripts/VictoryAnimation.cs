using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class VictoryAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float floatDistance = 40f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Vector2 originalPosition;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            originalPosition = rectTransform.anchoredPosition;
        }
    }

    private void OnEnable()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalPosition;
        }

        canvasGroup.alpha = 0f;

        animationCoroutine = StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        float elapsed = 0f;

        // Fade in and float upward.
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;

            float progress = fadeInDuration > 0f
                ? Mathf.Clamp01(elapsed / fadeInDuration)
                : 1f;

            canvasGroup.alpha = progress;

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition =
                    originalPosition +
                    Vector2.up * (floatDistance * progress);
            }

            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Keep the achievement visible.
        yield return new WaitForSeconds(displayDuration);

        // Fade out.
        elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;

            float progress = fadeOutDuration > 0f
                ? Mathf.Clamp01(elapsed / fadeOutDuration)
                : 1f;

            canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        canvasGroup.alpha = 0f;

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }
}