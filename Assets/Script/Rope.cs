using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Rigidbody stoneRigidbody;

    public void Break()
    {
        Destroy(gameObject);
        if (stoneRigidbody != null)
        {
            stoneRigidbody.isKinematic = false;
        }
    }
}
