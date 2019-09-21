using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMObjectSlot
{
	public Transform m_Transform;
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
				worker.transform.SetParent(slot.m_Transform);
			}
		}
	}

	public void AssignEquipment(FMEquipment equipment)
	{
		equipment.transform.SetParent(m_EquipmentSlot.m_Transform);
	}

	public void AssignResource(FMResource resource)
	{
		resource.transform.SetParent(m_ResourceSlot.m_Transform);
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
		m_EquipmentSlot.m_AssignedObject = null;
	}


	public void UnassignResource(FMResource resource)
	{
		m_ResourceSlot.m_AssignedObject = null;
	}
}