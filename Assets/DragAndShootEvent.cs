using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndShootEvent : MonoBehaviour
{
    [Header("Movement")]
    public float maxPower;
    [Tooltip("Set gravity to 0 if you want a top down ball game like billiardo.")]
    public float gravity = 1;
    [Tooltip("Slow the ball movement while aiming to make it easier to aim.")]
    [Range(0f, 0.1f)] public float slowMotion;

    [Tooltip("Allows you to aim and shoot even when the ball is still moving.")]
    public bool shootWhileMoving = false;
    [Tooltip("Drag forward to aim instead of reverse aiming.")]
    public bool forwardDragging = true;
    [Tooltip("Show the dragging line on the screen so you will not get confused where you are aiming")]
    public bool showLineOnScreen = false;
    [Tooltip("Allow you to click anywhere on the screen to start aiming, turn it off if you only want to start aiming while clicking on the ball")]
    public bool freeAim = true;

    private Transform direction;
    private Rigidbody rb;
    private LineRenderer line;
    private LineRenderer screenLine;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 startMousePos;
    private Vector3 currentMousePos;

    private float shootPower;
    private bool canShoot = true;

    private bool isAiming = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = gravity != 0;
        line = GetComponent<LineRenderer>();

        if (transform.childCount > 0)
        {
            direction = transform.GetChild(0);
            screenLine = direction.GetComponent<LineRenderer>();
            if (screenLine == null)
            {
                Debug.LogError("Child object does not have a Line Renderer component.");
            }
        }
        else
        {
            Debug.LogError("The GameObject does not have any children.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (freeAim)
                MouseClick();
            else
                BallClick();
        }

        if (Input.GetMouseButton(0) && isAiming)
        {
            MouseDrag();
            if (shootWhileMoving) rb.velocity /= (1 + slowMotion);
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            MouseRelease();
        }

        if (!shootWhileMoving && rb.velocity.magnitude < 0.7f)
        {
            rb.velocity = Vector3.zero; //ENABLE THIS IF YOU WANT THE BALL TO STOP IF ITS MOVING SO SLOW
            canShoot = true;
        }
    }

    private bool ObjectClicked()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void MouseClick()
    {
        isAiming = true;
        if (shootWhileMoving)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            startMousePos = Camera.main.ScreenToWorldPoint(screenPos);

            Vector3 dir = transform.position - startMousePos;
            if (dir != Vector3.zero)
                transform.forward = dir;
        }
        else
        {
            if (canShoot)
            {
                Vector3 screenPos = Input.mousePosition;
                screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
                startMousePos = Camera.main.ScreenToWorldPoint(screenPos);

                Vector3 dir = transform.position - startMousePos;
                if (dir != Vector3.zero)
                    transform.forward = dir;
            }
        }
    }

    void BallClick()
    {
        if (!ObjectClicked())
            return;

        isAiming = true;
        if (shootWhileMoving)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            startMousePos = Camera.main.ScreenToWorldPoint(screenPos);

            Vector3 dir = transform.position - startMousePos;
            if (dir != Vector3.zero)
                transform.forward = dir;
        }
        else
        {
            if (canShoot)
            {
                Vector3 screenPos = Input.mousePosition;
                screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
                startMousePos = Camera.main.ScreenToWorldPoint(screenPos);

                Vector3 dir = transform.position - startMousePos;
                if (dir != Vector3.zero)
                    transform.forward = dir;
            }
        }
    }

    void MouseDrag()
    {
        if (!freeAim)
            startMousePos = transform.position;

        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        currentMousePos = Camera.main.ScreenToWorldPoint(screenPos);

        LookAtShootDirection();
        DrawLine();

        if (showLineOnScreen)
            DrawScreenLine();

        float distance = Vector3.Distance(currentMousePos, startMousePos);

        if (distance > 1)
        {
            line.enabled = true;
            if (showLineOnScreen)
                screenLine.enabled = true;
        }
    }

    void MouseRelease()
    {
        if (shootWhileMoving)
        {
            Shoot();
            screenLine.enabled = false;
            line.enabled = false;
        }
        else
        {
            if (canShoot)
            {
                Shoot();
                screenLine.enabled = false;
                line.enabled = false;
            }
        }
        isAiming = false;
    }

    void LookAtShootDirection()
    {
        Vector3 dir = startMousePos - currentMousePos;

        if (forwardDragging)
        {
            if (dir != Vector3.zero)
                transform.forward = dir * -1;
        }
        else
        {
            if (dir != Vector3.zero)
                transform.forward = dir;
        }

        float dis = Vector3.Distance(startMousePos, currentMousePos);
        dis *= 4;

        if (dis < maxPower)
        {
            direction.localPosition = new Vector3(dis / 6, 0, 0);
            shootPower = dis;
        }
        else
        {
            shootPower = maxPower;
            direction.localPosition = new Vector3(maxPower / 6, 0, 0);
        }
    }

    public void Shoot()
    {
        canShoot = false;
        rb.velocity = transform.forward * shootPower;
    }

    void DrawScreenLine()
    {
        screenLine.positionCount = 2;
        screenLine.SetPosition(0, startMousePos);
        screenLine.SetPosition(1, currentMousePos);
    }

    void DrawLine()
    {
        startPosition = transform.position;

        line.positionCount = 2;
        line.SetPosition(0, startPosition);

        targetPosition = direction.transform.position;
        line.SetPosition(1, targetPosition);
    }
}
