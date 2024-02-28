using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;

public class shader_manager : MonoBehaviour
{
    //idk any other way thats easy
    private SphereGenerator sphereGenerator;
    private PlayerMovement playerMovement;

    public ComputeShader RayTracingShader;

    private RenderTexture _target;

    private Camera _camera;


    private int numSpheres;
    private SphereGenerator.Sphere[] spheres;


    private ComputeBuffer computeBuffer;




    private void Awake()
    {
        sphereGenerator = GetComponent<SphereGenerator>();
        playerMovement = GetComponent<PlayerMovement>();

        //make a list of spheres on a larger sphere
        SphereGenerator.Sphere earth = new SphereGenerator.Sphere();
        earth.Radius = 2000.0f;
        Vector3 center = new Vector3(0, -earth.Radius, 0);
        earth.Position = center;
        earth.Colour = new Vector4(0.76f, 0.76f, 0.76f, 0.2f);
        numSpheres = sphereGenerator.getNumSpheres;
        spheres = sphereGenerator.GenerateSpheresOnEarth(numSpheres, earth);

        //Initialise data for player camera
        playerMovement.SpheresArr = spheres;
        playerMovement.Center = center;


        _camera = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();

        // Set shader parameters
        SetShaderParameters();

        computeBuffer = new ComputeBuffer(numSpheres, Marshal.SizeOf(typeof(SphereGenerator.Sphere)));
        computeBuffer.SetData(spheres);
        RayTracingShader.SetInt("NumSpheres", numSpheres);
        RayTracingShader.SetBuffer(0, "Spheres", computeBuffer);
        RayTracingShader.SetTexture(0, "Result", _target);

        //dispatch it
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // Blit the result texture to the screen
        Graphics.Blit(_target, destination);

        computeBuffer.Release();
    }

    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();

            // Get a render target for Ray Tracing
            _target = new RenderTexture(
                Screen.width,
                 Screen.height,
                  0,
                   RenderTextureFormat.ARGBFloat,
                    RenderTextureReadWrite.Linear);

            _target.enableRandomWrite = true;
            _target.Create();
        }
    }




    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
    }
}

