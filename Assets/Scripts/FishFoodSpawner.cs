using System.Collections;
using UnityEngine;

public class FishFoodSpawner : MonoBehaviour
{
    [Header("Food Pellet Prefab")]
    [SerializeField] private GameObject foodPelletPrefab;

    [Header("Spawning Settings")]
    [SerializeField] private int pelletCount = 12;
    [SerializeField] private float spawnRadius = 0.4f;
    [SerializeField] private float spawnInterval = 0.08f;

    [Header("Sinking Settings")]
    [SerializeField] private float sinkingSpeed = 0.15f;
    [SerializeField] private float pelletLifetime = 6f;

    private bool hasSpawned = false;

    public void SpawnFood(Vector3 spawnPosition)
    {
        if (hasSpawned || foodPelletPrefab == null)
            return;

        hasSpawned = true;

        StartCoroutine(SpawnPellets(spawnPosition));
    }

    private IEnumerator SpawnPellets(Vector3 spawnPosition)
    {
        for (int i = 0; i < pelletCount; i++)
        {
            Vector2 randomOffset =
                Random.insideUnitCircle * spawnRadius;

            Vector3 pelletPosition = spawnPosition + new Vector3(
                randomOffset.x,
                Random.Range(-0.05f, 0.05f),
                randomOffset.y
            );

            GameObject pellet = Instantiate(
                foodPelletPrefab,
                pelletPosition,
                Quaternion.identity
            );

            StartCoroutine(SinkPellet(pellet));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SinkPellet(GameObject pellet)
    {
        float elapsed = 0f;

        while (pellet != null && elapsed < pelletLifetime)
        {
            pellet.transform.position +=
                Vector3.down * sinkingSpeed * Time.deltaTime;

            elapsed += Time.deltaTime;

            yield return null;
        }

        if (pellet != null)
        {
            Destroy(pellet);
        }
    }
}