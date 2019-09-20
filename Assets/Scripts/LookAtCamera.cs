using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }
}
