using UnityEngine;

public class WolfMovement : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the wolf moves

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("walk"); // Ensure the walking animation is playing
    }

    void Update()
    {
        // Move the wolf forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
