using UnityEngine;

[RequireComponent(typeof(Light))]
public class UnderwaterLightAnimation : MonoBehaviour
{
    [Header("Light Intensity")]
    [SerializeField] private float minimumIntensity = 1.5f;
    [SerializeField] private float maximumIntensity = 2.5f;

    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 0.7f;

    private Light underwaterLight;

    private void Awake()
    {
        underwaterLight = GetComponent<Light>();
    }

    private void Update()
    {
        // Smoothly brighten and dim the light.
        float wave = Mathf.Sin(Time.time * animationSpeed);

        // Convert the wave from -1..1 to 0..1.
        float normalizedWave = (wave + 1f) * 0.5f;

        // Calculate the new intensity.
        underwaterLight.intensity = Mathf.Lerp(
            minimumIntensity,
            maximumIntensity,
            normalizedWave
        );
    }
}