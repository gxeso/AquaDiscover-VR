using System;
using UnityEngine;

public class FishFoodPellet : MonoBehaviour
{
    public static event Action OnPelletEaten;

    private bool hasBeenEaten = false;
    private FishFeedingMovement reservedBy;

    public bool IsAvailable
    {
        get { return !hasBeenEaten; }
    }

    public bool IsAvailableFor(FishFeedingMovement fish)
    {
        return !hasBeenEaten &&
               (reservedBy == null || reservedBy == fish);
    }

    public bool TryReserve(FishFeedingMovement fish)
    {
        if (fish == null || !IsAvailableFor(fish))
            return false;

        reservedBy = fish;
        return true;
    }

    public void ReleaseReservation(FishFeedingMovement fish)
    {
        if (reservedBy == fish)
        {
            reservedBy = null;
        }
    }

    public bool TryEat(FishFeedingMovement fish)
    {
        if (fish == null || hasBeenEaten || reservedBy != fish)
            return false;

        hasBeenEaten = true;
        reservedBy = null;

        OnPelletEaten?.Invoke();

        Destroy(gameObject);

        return true;
    }
}