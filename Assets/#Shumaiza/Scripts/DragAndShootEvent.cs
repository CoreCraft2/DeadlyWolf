using UnityEngine;
using System.Collections;

public class DragAndShootEvent : MonoBehaviour
{
    [Header("Movement")]
    public float maxPower;
    public float gravity = 1;
    [Range(0f, 0.1f)] public float slowMotion;
    public bool forwardDraging = true;
    public bool showLineOnScreen = false;
    public GameObject arrowPrefab; // Reference to the Arrow prefab
    public Transform arrowSpawnPoint; // Instantiation point for the arrow

    private Transform direction;
    private Animator animator;
    private LineRenderer line;
    private LineRenderer screenLine;

    private Vector3 startMousePos;
    private Vector3 currentMousePos;
    private float shootPower;
    private bool canShoot = true;
    private bool isAiming = false;
    private GameObject currentArrow;

    void Start()
    {
        // Get components from the child "Archer" GameObject
        animator = GetComponentInChildren<Animator>();
        line = GetComponentInChildren<LineRenderer>();
        direction = transform.GetChild(0); // Assume the first child is the direction indicator
        screenLine = direction.GetComponent<LineRenderer>();

        // Check if components are missing and log errors
        if (animator == null)
        {
            Debug.LogError("Animator component is missing. Please add it to the child 'Archer' object.");
        }
        if (line == null || screenLine == null)
        {
            Debug.LogError("LineRenderer component is missing. Please add it to the child 'Archer' object.");
        }

        // Instantiate the first arrow
        SpawnArrow();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            MouseClick();
        }
        if (Input.GetMouseButton(0) && isAiming)
        {
            MouseDrag();
        }
        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            MouseRelease();
        }
    }

    private void MouseClick()
    {
        isAiming = true;
        if (animator != null)
        {
            animator.SetTrigger("ReadyToAttack");
        }
        startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startMousePos.z = 0;
    }

    private void MouseDrag()
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentMousePos.z = 0;
        LookAtShootDirection();
        DrawLine();
    }

    private void MouseRelease()
    {
        Shoot();
        isAiming = false;
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }

    private void LookAtShootDirection()
    {
        Vector3 dir = startMousePos - currentMousePos;
        transform.right = forwardDraging ? -dir : dir;

        float distance = Vector3.Distance(startMousePos, currentMousePos);
        shootPower = Mathf.Clamp(distance * 4, 0, maxPower);

        direction.localPosition = new Vector3(shootPower / 6, 0, 0);
    }

    private void Shoot()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (currentArrow != null)
        {
            currentArrow.transform.SetParent(null); // Detach from bow
            Rigidbody arrowRb = currentArrow.GetComponent<Rigidbody>();
            if (arrowRb != null)
            {
                arrowRb.velocity = transform.right * shootPower;
                canShoot = false;
                StartCoroutine(ResetShoot());
            }
            else
            {
                Debug.LogError("Arrow prefab is missing Rigidbody component.");
            }
        }
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(1f); // Adjust the delay as needed
        canShoot = true;
        SpawnArrow(); // Automatically spawn a new arrow after shooting
    }

    private void DrawLine()
    {
        if (line != null)
        {
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, direction.position);
        }
    }

    private void SpawnArrow()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, transform.rotation);
            currentArrow.transform.SetParent(arrowSpawnPoint);
        }
    }
}
