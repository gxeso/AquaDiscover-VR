
using UnityEngine;

public class JellyfishMovement : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatSpeed = 1f;

    [Header("Bell Pulsing")]
    [SerializeField] private Transform jellyfishBell;
    [SerializeField] private float pulseAmount = 0.12f;
    [SerializeField] private float pulseSpeed = 2f;

    [Header("Tentacle Sway")]
    [SerializeField] private Transform[] tentacles;
    [SerializeField] private float swayAngle = 12f;
    [SerializeField] private float swaySpeed = 1.5f;

    private Vector3 startingPosition;
    private Vector3 originalBellScale;
    private Quaternion[] originalTentacleRotations;

    private void Start()
    {
        startingPosition = transform.localPosition;

        if (jellyfishBell != null)
            originalBellScale = jellyfishBell.localScale;

        originalTentacleRotations =
            new Quaternion[tentacles.Length];

        for (int i = 0; i < tentacles.Length; i++)
        {
            if (tentacles[i] != null)
                originalTentacleRotations[i] =
                    tentacles[i].localRotation;
        }
    }

    private void Update()
    {
        float time = Time.time;

        // Float gently up and down
        transform.localPosition = startingPosition +
            Vector3.up *
            Mathf.Sin(time * floatSpeed) *
            floatHeight;

        // Pulse the jellyfish bell
        if (jellyfishBell != null)
        {
            float pulse =
                1f + Mathf.Sin(time * pulseSpeed) *
                pulseAmount;

            jellyfishBell.localScale = new Vector3(
                originalBellScale.x * pulse,
                originalBellScale.y / pulse,
                originalBellScale.z * pulse
            );
        }

        // Sway tentacles independently
        for (int i = 0; i < tentacles.Length; i++)
        {
            if (tentacles[i] == null)
                continue;

            float sway =
                Mathf.Sin(time * swaySpeed + i) *
                swayAngle;

            tentacles[i].localRotation =
                originalTentacleRotations[i] *
                Quaternion.Euler(sway, 0f, sway * 0.5f);
        }
    }
}
