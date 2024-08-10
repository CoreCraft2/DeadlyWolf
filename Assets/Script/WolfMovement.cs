using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovement : MonoBehaviour
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

            // If the wolf's position in Z exceeds or equals the forward bound, change direction
            if (transform.position.z >= forwardBound)
            {
                movingForward = false; // Switch to moving backward
            }
        }
        else
        {
            // Move backward along the local Z-axis
            moveDirection = -transform.forward;
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // If the wolf's position in Z is less than or equal to the backward bound, change direction
            if (transform.position.z <= backwardBound)
            {
                movingForward = true; // Switch to moving forward
            }
        }

        Debug.Log("Current Z Position: " + transform.position.z + " | Moving Forward: " + movingForward);
    }
}
