using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;

    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            float rotationAroundXAxis = cam.transform.position.y * 5; // camera moves vertically

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);

            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (float)0.1, cam.transform.position.z);
        }
        if (Input.GetKeyDown("down"))
        {
            float rotationAroundXAxis = cam.transform.position.y * -5; // camera moves vertically

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);

            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - (float)0.1, cam.transform.position.z);
        }
    }
}
