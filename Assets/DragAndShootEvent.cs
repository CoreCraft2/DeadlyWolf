using UnityEngine;

public class DragAndShoot3D : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab of the arrow
    public Transform shootPoint; // Point from where the arrow is shot
    public float shootForce = 10f; // Force applied to the arrow

    private Vector3 startPos;
    private Vector3 endPos;
    private Camera cam;
    private GameObject currentArrow;

    void Start()
    {
        cam = Camera.main;
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
        currentArrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
    }

    void OnDrag()
    {
        Vector3 currentPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 direction = startPos - currentPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentArrow.transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }

    void OnDragEnd()
    {
        endPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 direction = startPos - endPos;
        currentArrow.GetComponent<Rigidbody>().velocity = direction * shootForce;
        currentArrow = null;
    }
}
