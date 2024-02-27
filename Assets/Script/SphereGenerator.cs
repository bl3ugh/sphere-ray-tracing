using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SphereGenerator
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere
    {
        public Vector3 Position;
        public Vector4 Colour;
        public float Radius;
    }

    public Sphere[] GenerateSpheresOnEarth(int n, Sphere earth, float minDistance)
    {
        Sphere[] spheres = new Sphere[n];

        for (int i = 0; i < n; i++)
        {
            float latitude = Random.Range(-90f, 90f);
            float longitude = Random.Range(0f, 360f);

            float phi = Mathf.Deg2Rad * (90f - latitude);
            float theta = Mathf.Deg2Rad * longitude;

            float x = earth.Position.x + earth.Radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = earth.Position.y + earth.Radius * Mathf.Cos(phi);
            float z = earth.Position.z + earth.Radius * Mathf.Sin(phi) * Mathf.Sin(theta);

            spheres[i] = new Sphere
            {
                Position = new Vector3(x, y, z),
                Colour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f), // Default colour
                Radius = 1.0f // Default radius
            };

            // Apply clumping with minimum distance
            if (i > 0)
            {
                for (int j = 0; j < i; j++)
                {
                    float distance = Vector3.Distance(spheres[i].Position, spheres[j].Position);
                    if (distance < minDistance)
                    {
                        // Move the current sphere away from the other sphere
                        Vector3 direction = (spheres[i].Position - spheres[j].Position).normalized;
                        spheres[i].Position += direction * (minDistance - distance);
                    }
                }
            }
        }
        return spheres;
    }
}
