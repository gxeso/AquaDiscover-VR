using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UnderwaterCausticsAnimation : MonoBehaviour
{
    [Header("Caustics Movement")]
    [SerializeField] private Vector2 scrollSpeed =
        new Vector2(0.02f, 0.015f);

    [Header("Caustics Appearance")]
    [SerializeField] private Vector2 textureTiling =
        new Vector2(2f, 2f);

    private Renderer causticsRenderer;
    private MaterialPropertyBlock propertyBlock;

    private static readonly int BaseMapST =
        Shader.PropertyToID("_BaseMap_ST");

    private void Awake()
    {
        causticsRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        float time = Time.time;

        // Slowly move the texture across the sand.
        Vector2 offset = new Vector2(
            Mathf.Repeat(time * scrollSpeed.x, 1f),
            Mathf.Repeat(time * scrollSpeed.y, 1f)
        );

        // Apply tiling and scrolling without modifying
        // the shared material asset.
        causticsRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetVector(
            BaseMapST,
            new Vector4(
                textureTiling.x,
                textureTiling.y,
                offset.x,
                offset.y
            )
        );

        causticsRenderer.SetPropertyBlock(propertyBlock);
    }
}