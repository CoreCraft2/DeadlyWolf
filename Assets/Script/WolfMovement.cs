using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public float forwardDistance = 3.0f;  // Distance forward from initial position
    public float backwardDistance = 3.0f;  // Distance backward from initial position

    private bool movingForward = true;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;  // Record the initial position
    }

    void Update()
    {
        Vector3 moveDirection;

        if (movingForward)
        {
            // Move forward along the local Z-axis
            moveDirection = transform.forward;
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // Calculate the distance traveled from the initial position
            float distanceTraveled = Vector3.Distance(initialPosition, transform.position);

            // Debugging current position and bounds
            Debug.Log("Moving Forward | Distance Traveled: " + distanceTraveled + " | Forward Distance: " + forwardDistance);

            // If the distance traveled exceeds the forward distance, change direction
            if (distanceTraveled >= forwardDistance)
            {
                movingForward = false;
                // Rotate the wolf to face backward
                transform.Rotate(0f, 180f, 0f);
                Debug.Log("Switching to Moving Backward");
            }
        }
        else
        {
            // Move backward along the local Z-axis
            moveDirection = transform.forward;
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // Calculate the distance traveled from the initial position
            float distanceTraveled = Vector3.Distance(initialPosition, transform.position);

            // Debugging current position and bounds
            Debug.Log("Moving Backward | Distance Traveled: " + distanceTraveled + " | Backward Distance: " + backwardDistance);

            // If the distance traveled exceeds the backward distance, change direction
            if (distanceTraveled >= backwardDistance)
            {
                movingForward = true;
                // Rotate the wolf to face forward
                transform.Rotate(0f, 180f, 0f);
                Debug.Log("Switching to Moving Forward");
            }
        }
    }
}
