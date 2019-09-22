using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMObjectSlot
{
	public Transform m_Transform;
	[HideInInspector]
	public GameObject m_AssignedObject;
}

public class FMWorkerSlotHelper : MonoBehaviour
{
	public List<FMObjectSlot> m_WorkerSlots = new List<FMObjectSlot>();

	public FMObjectSlot m_EquipmentSlot;
	public FMObjectSlot m_ResourceSlot;

	public void AssignWorker(FMWorker worker)
	{
		for (int i = 0; i < m_WorkerSlots.Count; i++)
		{
			var slot = m_WorkerSlots[i];
			if (slot.m_AssignedObject == null)
			{
				slot.m_AssignedObject = worker.gameObject;
				worker.transform.SetParent(slot.m_Transform, false);
				worker.transform.localPosition = Vector3.zero;
				return;
			}
		}
	}

	public void AssignEquipment(FMEquipment equipment)
	{
		if (equipment)
		{
			equipment.transform.SetParent(m_EquipmentSlot.m_Transform, false);
			equipment.transform.localPosition = Vector3.zero;
		}
	}

	public void AssignResource(FMResource resource)
	{
		resource.transform.SetParent(m_ResourceSlot.m_Transform, false);
		resource.transform.localPosition = Vector3.zero;
	}

	public void UnassignWorker(FMWorker worker)
	{
		for (int i = 0; i < m_WorkerSlots.Count; i++)
		{
			var slot = m_WorkerSlots[i];
			if (slot.m_AssignedObject == worker.gameObject)
			{
				slot.m_AssignedObject = null;
			}
		}
	}

	public void UnassignEquipment(FMEquipment equipment)
	{
		if (equipment != null)
		{
			var stagingArea = FMBoardReferences.GetOrCreateInstance().m_EquipmentStagingArea;
			stagingArea.AddToStaging(equipment.transform);
		}
		m_EquipmentSlot.m_AssignedObject = null;
	}


	public void UnassignResource(FMResource resource)
	{
		m_ResourceSlot.m_AssignedObject = null;
	}
}