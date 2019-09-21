using UnityEngine;
using System.Collections;

public class SpriteShadows : MonoBehaviour
{
	public bool receiveShadows;
	public UnityEngine.Rendering.ShadowCastingMode shadowCastingMode;

	//[InspectorButton("Apply")]
	//public bool _;

	//void Start()
	//{
	//	Apply();
	//}

	private void OnValidate()
	{
		Apply();
	}

	void Apply()
	{
		Renderer r = GetComponent<Renderer>();
		r.receiveShadows = receiveShadows;// true;
		r.shadowCastingMode = shadowCastingMode;// UnityEngine.Rendering.ShadowCastingMode.On;
	}
	
}
