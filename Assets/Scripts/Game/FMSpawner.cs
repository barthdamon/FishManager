using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public int numberToSpawn = 10;

    public Vector3 randomPosRange = new Vector3(5, 0, 5);

    void Start()
    {
        for(int i=0; i<numberToSpawn; i++)
        {
            Vector3 offset = Vector3.Scale(Random.insideUnitSphere, randomPosRange);
            GameObject go = Instantiate(prefab, parent.position + offset, parent.rotation, parent);
            go.name = "spawn" + i;
        }
    }

    void Update()
    {
        
    }
}
