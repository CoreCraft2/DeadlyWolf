using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2.0f;
    public float forwardBound = 3.0f;  // Maximum Z value for forward movement
    public float backwardBound = -3.0f;  // Minimum Z value for backward movement

    private bool movingForward = true;

    void Update()
    {
        Vector3 moveDirection;

        if (movingForward)
        {
            // Move forward along the local Z-axis
            moveDirection = transform.forward;
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // Debugging current position and bounds
            Debug.Log("Moving Forward | Z Position: " + transform.position.z + " | Forward Bound: " + forwardBound);

            // If the wolf's position in Z reaches or exceeds the forward bound, change direction
            if (transform.position.z >= forwardBound)
            {
                movingForward = false;
                Debug.Log("Switching to Moving Backward");
            }
        }
        else
        {
            // Move backward along the local Z-axis
            moveDirection = -transform.forward;
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // Debugging current position and bounds
            Debug.Log("Moving Backward | Z Position: " + transform.position.z + " | Backward Bound: " + backwardBound);

            // If the wolf's position in Z reaches or is less than the backward bound, change direction
            if (transform.position.z <= backwardBound)
            {
                movingForward = true;
                Debug.Log("Switching to Moving Forward");
            }
        }
    }
}
