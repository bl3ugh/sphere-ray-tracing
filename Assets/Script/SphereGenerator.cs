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
        spheres[0] = earth;
        for (int i = 1; i < n; i++)
        {
            float latitude = Random.Range(-5f, 5f);
            float longitude = Random.Range(0f, 10f);

            float r = Random.Range(0f, 1.0f);
            float g = Random.Range(0f, 1.0f);
            float b = Random.Range(0f, 1.0f);
            float w = Random.Range(0f, 1.0f);

            float phi = Mathf.Deg2Rad * (90f - latitude);
            float theta = Mathf.Deg2Rad * longitude;
            float radius = Random.Range(0.5f, 5f);

            float x = earth.Position.x + (earth.Radius + radius) * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = earth.Position.y + (earth.Radius + radius) * Mathf.Cos(phi);
            float z = earth.Position.z + (earth.Radius + radius) * Mathf.Sin(phi) * Mathf.Sin(theta);

            spheres[i] = new Sphere
            {
                Position = new Vector3(x, y, z),
                Colour = new Vector4(r, g, b, w),
                Radius = radius
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
