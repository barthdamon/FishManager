using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 mouseSensitivity = Vector3.one;
    public Vector2 distanceMinMax;

    public Transform zoomer;
    public float distance;

    void Update()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");
        float ms = Input.GetAxisRaw("Mouse ScrollWheel");

        distance = -(Quaternion.Inverse(zoomer.localRotation) * zoomer.localPosition).z;

        if (Input.GetMouseButton(1))
        {
            transform.position += transform.right * mx * mouseSensitivity.x * distance;
            transform.position += transform.forward * my * mouseSensitivity.y * distance;
        }

        if (ms != 0.0f)
        {
            distance = distance * 1.0f + ms * mouseSensitivity.z;
            distance = Mathf.Clamp(distance, distanceMinMax.x, distanceMinMax.y);
            zoomer.localPosition = zoomer.localRotation * Vector3.forward * -distance;
        }
    }
}
