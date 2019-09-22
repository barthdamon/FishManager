using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool useCameraForward = true;
    [Range(0,1)]
    public float verticality = 0.5f;
    public bool backFace = true;

    void LateUpdate()
    {
        Vector3 fwd = useCameraForward ? -Camera.main.transform.forward : Camera.main.transform.position - this.transform.position;
            fwd.y *= (1.0f-verticality);
        if (backFace)
            fwd = -fwd;
        transform.rotation = Quaternion.LookRotation(fwd);
    }
}
