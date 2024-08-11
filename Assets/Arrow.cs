using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the arrow has collided with an object tagged "Rope"
        if (other.CompareTag("Rope"))
        {
            // Get the stone GameObject, assuming it's a child of the rope
            Transform stoneTransform = other.transform.Find("Stone"); // "Stone" is the name of the child GameObject

            if (stoneTransform != null)
            {
                Rigidbody stoneRb = stoneTransform.GetComponent<Rigidbody>();
                if (stoneRb != null)
                {
                    stoneRb.useGravity = true;
                }
                else
                {
                    Debug.LogError("Stone does not have a Rigidbody component.");
                }
            }
            else
            {
                Debug.LogError("Stone child not found on the rope.");
            }
        }
    }
}
