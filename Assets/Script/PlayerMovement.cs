using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    private SphereGenerator.Sphere[] spheres;
    private Vector3 center;


    public SphereGenerator.Sphere[] SpheresArr
    {
        get { return spheres; }
        set { spheres = value; }
    }

    public Vector3 Center
    {
        get { return center; }
        set { center = value; }
    }


    void Update()
    {
        Vector3 totalMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            totalMovement += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalMovement -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalMovement -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalMovement += transform.right * moveSpeed * Time.deltaTime;
        }

        // Apply total movement to the transform position
        transform.position = MoveAroundSphere(transform.position + totalMovement);
    }
    // Function to move the object around the sphere
    Vector3 MoveAroundSphere(Vector3 newPosition)
    {
        // Calculate the direction vector from the center of the sphere to the new position
        Vector3 direction = newPosition - center;

        // Normalize the direction vector
        direction.Normalize();

        // Multiply the direction vector by the radius of the sphere to get the final position
        Vector3 finalPosition = center + direction * (spheres[0].Radius + 0.5f);

        return finalPosition;
    }
}