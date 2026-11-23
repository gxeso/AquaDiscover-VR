using UnityEngine;

public class FishSwim : MonoBehaviour
{
    public float width = 2.5f;
    public float depth = 0.7f;
    public float speed = 0.6f;
    public float bobHeight = 0.15f;

    private Vector3 centre;
    private float angle;

    void Start()
    {
        centre = transform.position;
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        Vector3 nextPosition = centre + new Vector3(
            Mathf.Sin(angle) * width,
            Mathf.Sin(angle * 2f) * bobHeight,
            Mathf.Cos(angle) * depth
        );

        Vector3 direction = new Vector3(
            Mathf.Cos(angle) * width,
            0f,
            -Mathf.Sin(angle) * depth
        );

        transform.position = nextPosition;

        if (direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}