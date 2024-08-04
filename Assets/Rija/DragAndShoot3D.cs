using UnityEngine;

public class DragAndShoot3D : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab of the ball
    public Transform shootPoint; // Point from where the ball is shot
    public float shootForce = 10f; // Force applied to the ball
    public LineRenderer trajectoryRenderer; // LineRenderer for showing trajectory

    private Vector3 startPos;
    private Camera cam;
    private GameObject currentBall;

    void Start()
    {
        cam = Camera.main;
        if (trajectoryRenderer == null)
        {
            Debug.LogError("Trajectory Renderer is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDragStart();
        }
        else if (Input.GetMouseButton(0))
        {
            OnDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnDragEnd();
        }
    }

    void OnDragStart()
    {
        startPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        currentBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        DrawTrajectory();
    }

    void OnDrag()
    {
        Vector3 currentPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 direction = startPos - currentPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentBall.transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
        DrawTrajectory();
    }

    void OnDragEnd()
    {
        Vector3 endPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 direction = startPos - endPos;
        currentBall.GetComponent<Rigidbody>().velocity = direction * shootForce;
        currentBall = null;
        ClearTrajectory();
    }

    void DrawTrajectory()
    {
        if (trajectoryRenderer == null) return;

        // Example points for the trajectory
        Vector3[] points = new Vector3[30];
        for (int i = 0; i < points.Length; i++)
        {
            float t = i / (float)(points.Length - 1);
            points[i] = shootPoint.position + (startPos - shootPoint.position) * t;
        }

        trajectoryRenderer.positionCount = points.Length;
        trajectoryRenderer.SetPositions(points);
    }

    void ClearTrajectory()
    {
        if (trajectoryRenderer == null) return;
        trajectoryRenderer.positionCount = 0;
    }
}
