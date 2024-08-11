using UnityEngine;

public class AnimateTextureOnLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; // Reference to the Line Renderer
    [SerializeField] private float scrollSpeed = 1.0f;   // Speed of the texture scrolling
    private Material material;                          // Material used by the Line Renderer

    private void Start()
    {
        // Get the material from the Line Renderer
        material = lineRenderer.material;
    }

    private void Update()
    {
        // Calculate the offset based on time
        float offset = Time.time * scrollSpeed;

        // Update the texture offset
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
