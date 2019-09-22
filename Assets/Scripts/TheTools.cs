using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTools : MonoBehaviour
{
    [InspectorButton("Clear","FindSprites", "SetThemUp")]
    public bool _;

    public Material material;
    public List<SpriteRenderer> sprites;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void Clear()
    {
        sprites.Clear();
    }

    void FindSprites()
    {
        sprites.AddRange(GetComponents<SpriteRenderer>());
        sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    void SetThemUp()
    {
        foreach(SpriteRenderer sr in sprites)
        {
            if (sr.material != material)
            {
                sr.material = material;

                sr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                sr.receiveShadows = true;
            }
        }
    }
}
