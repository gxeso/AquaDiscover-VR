using UnityEngine;

public class FishFeedingMovement : MonoBehaviour
{
    [Header("Feeding Movement")]
    public float feedingSpeed = 0.6f;
    public float stoppingDistance = 0.05f;
    public float feedingDuration = 20f;

    [Header("Natural Movement")]
    public float gatheringRadius = 0.5f;
    public float rotationSpeed = 2f;

    [Header("Pellet Eating")]
    public float eatingDistance = 0.15f;
    public float pelletSearchInterval = 0.3f;
    public float timeBetweenBites = 1.5f;

    private Vector3 feedingTarget;
    private bool isFeeding;

    private float feedingTimer;
    private float searchTimer;
    private float biteCooldown;

    private FishFoodPellet currentPellet;
    private FishSwim fishSwim;

    private int pelletsEaten;

    private void Awake()
    {
        fishSwim = GetComponent<FishSwim>();
    }

    private void Update()
    {
        if (!isFeeding)
            return;

        feedingTimer -= Time.deltaTime;
        searchTimer -= Time.deltaTime;

        if (biteCooldown > 0f)
        {
            biteCooldown -= Time.deltaTime;
        }

        if (feedingTimer <= 0f)
        {
            StopFeeding();
            return;
        }

        // Pause between bites.
        if (biteCooldown > 0f)
            return;

        // Keep an existing reservation until the pellet is eaten.
        if (currentPellet == null || !currentPellet.IsAvailable)
        {
            ReleaseCurrentPellet();
        }

        // Find another pellet when needed.
        if (currentPellet == null && searchTimer <= 0f)
        {
            searchTimer = pelletSearchInterval;
            FindNearestPellet();
        }

        if (currentPellet != null)
        {
            MoveTowards(currentPellet.transform.position);

            float distance = Vector3.Distance(
                transform.position,
                currentPellet.transform.position
            );

            if (distance <= eatingDistance)
            {
                if (currentPellet.TryEat(this))
                {
                    pelletsEaten++;
                    biteCooldown = timeBetweenBites;

                    Debug.Log(
                        $"{gameObject.name} ate a pellet! " +
                        $"Total eaten: {pelletsEaten}"
                    );
                }

                // The pellet has been eaten or is no longer usable.
                currentPellet = null;
                searchTimer = 0f;
            }
        }
        else
        {
            MoveTowards(feedingTarget);
        }
    }

    private void FindNearestPellet()
    {
        FishFoodPellet[] pellets =
            FindObjectsByType<FishFoodPellet>(
                FindObjectsSortMode.None
            );

        FishFoodPellet nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (FishFoodPellet pellet in pellets)
        {
            if (pellet == null || !pellet.IsAvailableFor(this))
                continue;

            float distance = Vector3.Distance(
                transform.position,
                pellet.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = pellet;
            }
        }

        if (nearest != null && nearest.TryReserve(this))
        {
            currentPellet = nearest;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (direction.magnitude <= stoppingDistance)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            feedingSpeed * Time.deltaTime
        );

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void StartFeeding(Vector3 targetPosition)
    {
        ReleaseCurrentPellet();

        Vector2 randomOffset =
            Random.insideUnitCircle * gatheringRadius;

        feedingTarget = targetPosition + new Vector3(
            randomOffset.x,
            Random.Range(-0.15f, 0.05f),
            randomOffset.y
        );

        feedingTimer = feedingDuration;
        searchTimer = 0f;
        biteCooldown = 0f;

        pelletsEaten = 0;
        isFeeding = true;

        if (fishSwim != null)
        {
            fishSwim.enabled = false;
        }
    }

    private void StopFeeding()
    {
        isFeeding = false;

        ReleaseCurrentPellet();

        if (fishSwim != null)
        {
            fishSwim.enabled = true;
        }

        Debug.Log(
            $"{gameObject.name} finished feeding. " +
            $"Pellets eaten: {pelletsEaten}"
        );
    }

    private void ReleaseCurrentPellet()
    {
        if (currentPellet != null)
        {
            currentPellet.ReleaseReservation(this);
            currentPellet = null;
        }
    }

    private void OnDisable()
    {
        ReleaseCurrentPellet();
    }
}