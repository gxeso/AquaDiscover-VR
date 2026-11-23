
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    [Header("Swimming Area - Local Coordinates")]
    [SerializeField] private Vector3 swimCenter =
        new Vector3(0f, 1.5f, 0f);

    [SerializeField] private Vector3 swimExtents =
        new Vector3(0.7f, 0.25f, 0.35f);

    [Header("Swimming")]
    [SerializeField] private float swimSpeed = 0.4f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float arrivalDistance = 0.15f;

    [Header("Tail")]
    [SerializeField] private Transform tailPivot;
    [SerializeField] private float tailSpeed = 4f;
    [SerializeField] private float tailAngle = 20f;

    private Vector3 targetPosition;
    private Quaternion originalTailRotation;

    private void Start()
    {
        if (tailPivot != null)
            originalTailRotation = tailPivot.localRotation;

        // Start inside the allowed swimming area.
        transform.localPosition = ClampToSwimArea(
            transform.localPosition
        );

        PickNewTarget();
    }

    private void Update()
    {
        Swim();
        AnimateTail();
    }

    private void Swim()
    {
        Vector3 current = transform.localPosition;
        Vector3 direction = targetPosition - current;

        if (direction.magnitude <= arrivalDistance)
        {
            PickNewTarget();
            return;
        }

        // Rotate in the same local coordinate space.
        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion desiredRotation =
                Quaternion.LookRotation(direction.normalized, Vector3.up);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                desiredRotation,
                turnSpeed * Time.deltaTime
            );
        }

        // Move toward the target in local space.
        Vector3 nextPosition = Vector3.MoveTowards(
            current,
            targetPosition,
            swimSpeed * Time.deltaTime
        );

        // Never let the shark's origin leave the patrol area.
        transform.localPosition =
            ClampToSwimArea(nextPosition);
    }

    private Vector3 ClampToSwimArea(Vector3 position)
    {
        Vector3 min = swimCenter - swimExtents;
        Vector3 max = swimCenter + swimExtents;

        return new Vector3(
            Mathf.Clamp(position.x, min.x, max.x),
            Mathf.Clamp(position.y, min.y, max.y),
            Mathf.Clamp(position.z, min.z, max.z)
        );
    }

    private void PickNewTarget()
    {
        targetPosition = swimCenter + new Vector3(
            Random.Range(-swimExtents.x, swimExtents.x),
            Random.Range(-swimExtents.y, swimExtents.y),
            Random.Range(-swimExtents.z, swimExtents.z)
        );
    }

    private void AnimateTail()
    {
        if (tailPivot == null)
            return;

        float swing =
            Mathf.Sin(Time.time * tailSpeed) * tailAngle;

        tailPivot.localRotation =
            originalTailRotation *
            Quaternion.Euler(0f, swing, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Matrix4x4 previousMatrix = Gizmos.matrix;

        Gizmos.matrix = transform.parent != null
            ? transform.parent.localToWorldMatrix
            : Matrix4x4.identity;

        Gizmos.DrawWireCube(
            swimCenter,
            swimExtents * 2f
        );

        Gizmos.matrix = previousMatrix;
    }
}
