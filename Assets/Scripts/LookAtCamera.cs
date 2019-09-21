using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool useCameraForward = true;

    void LateUpdate()
    {
        transform.rotation = useCameraForward ? Quaternion.LookRotation(-Camera.main.transform.forward)
                                                : Quaternion.LookRotation(Camera.main.transform.position - this.transform.position);
    }
}
