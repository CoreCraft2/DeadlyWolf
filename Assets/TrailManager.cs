using UnityEngine;

public class TrailManager : MonoBehaviour
{
    [SerializeField] private Transform leftHand;      // Reference to the player's left hand transform
    [SerializeField] private Transform targetPoint;   // Reference to the target point transform
    [SerializeField] private LineRenderer lineRenderer; // Reference to the Line Renderer component

    private void Start()
    {
        if (lineRenderer != null)
        {
            // Set the width of the line renderer
            lineRenderer.startWidth = 0.5f; // Adjust as needed
            lineRenderer.endWidth = 0.5f;   // Adjust as needed
        }
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            // Set the positions of the Line Renderer
            lineRenderer.SetPosition(0, leftHand.position);
            lineRenderer.SetPosition(1, targetPoint.position);
        }
    }
}
