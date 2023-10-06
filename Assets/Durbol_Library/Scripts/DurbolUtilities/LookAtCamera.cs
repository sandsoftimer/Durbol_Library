using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool useCustomCamera;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
            camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
    }
}
