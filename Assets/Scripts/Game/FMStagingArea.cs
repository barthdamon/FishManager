using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMStagingArea : MonoBehaviour
{
    [SerializeField]
    private float m_DropRadius = 3f;

    public void AddToStaging(Transform child_transform)
    {
		var rect_transform = GetComponent<RectTransform>();
		Vector2 random_position = new Vector2(Random.Range(-this.m_DropRadius, this.m_DropRadius), Random.Range(-this.m_DropRadius, this.m_DropRadius));
		Vector3 position = rect_transform.position + random_position.ToXZVector3();

		child_transform.SetParent(this.transform, false);
		child_transform.position = position;
    }

	//private void OnDrawGizmosSelected()
	//{
	//	var rect_transform = GetComponent<RectTransform>();
    //
	//	UnityEditor.Handles.color = Color.red;
	//	UnityEditor.Handles.DrawWireDisc(rect_transform.position, Vector3.up, m_DropRadius);
	//}
}
