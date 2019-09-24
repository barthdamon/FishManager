using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviourSingleton<Tutorial>
{
	[SerializeField]
	private bool has_dragged_worker = false;

	[SerializeField]
	private bool has_assigned_worker_to_boat = false;

	[SerializeField]
	private bool has_assigned_equipment_to_boat = false;

	[SerializeField]
	private bool has_processed_fish = false;

	public bool HasDraggedWorker
	{
		get { return this.has_dragged_worker; }
		set { this.has_dragged_worker = value; }
	}

	public bool HasAssignedWorkerToBoat
	{
		get { return this.has_assigned_worker_to_boat; }
		set { this.has_assigned_worker_to_boat = value; }
	}

	public bool HasAssignedEquipmentToBoat
	{
		get { return this.has_assigned_equipment_to_boat; }
		set { this.has_assigned_equipment_to_boat = value; }
	}

	public bool HasProcessedFish
	{
		get { return this.has_processed_fish; }
		set { this.has_processed_fish = value; }
	}

	public GameObject WorkerBob;
	public GameObject WorkerJoe;

	public void ShowStartingTutorial()
	{
		DialogManager.singleton.Say_3D("Well this is a fine mess...", this.WorkerBob);
		DialogManager.singleton.Say_3D("I better DRAG myself over to the boat and go fishing.", this.WorkerBob);

		StartCoroutine(StartReminderTimer());
	}

	public void ShowBoatSizeTutorial(int num_slots, FMWorker worker)
	{
		if (num_slots > 0)
		{
			GameObject worker_go = worker?.gameObject;
			if (!worker_go)
			{
				worker_go = this.WorkerBob;
			}

			switch (Random.Range(0, 4))
			{
				case 0:
					DialogManager.singleton.Say_3D("Woah, this is a big boat...", worker_go);
					DialogManager.singleton.Say_3D("... I Cod do with another pair of fins to help out.", worker_go);
					break;

				case 1:
					DialogManager.singleton.Say_3D("I could live in here!", worker_go);
					DialogManager.singleton.Say_3D("... I think I'll need a skipper to drive this thing.", worker_go);
					break;

				case 2:
					DialogManager.singleton.Say_3D("Oh my! I could get lost below decks.", worker_go);
					DialogManager.singleton.Say_3D("... Any chance I could get a bit of help with this thing?", worker_go);
					break;

				case 3:
					DialogManager.singleton.Say_3D("Nope! I nearly got caught in the propeller!", worker_go);
					DialogManager.singleton.Say_3D("... I'm not going back down there again, I need a safety buddy!", worker_go);
					break;
			}
		}
	}

	public void ShowBoatEquipmentTutorial(FMWorker worker)
	{
		if (!has_assigned_equipment_to_boat)
		{
			GameObject worker_go = worker?.gameObject;
			if (!worker_go)
			{
				worker_go = this.WorkerBob;
			}
			
			DialogManager.singleton.Say_3D("Now to get fishing...", worker_go);
			DialogManager.singleton.Say_3D("... But first I'm going to need some bait.", worker_go);
			DialogManager.singleton.Say_3D("Drag over one of those fishing lures.", worker_go);
			DialogManager.singleton.Say_3D("Each one is for a different catch.", worker_go);
		}
	}

	public void ShowProcessFishTutorial(FMWorker worker)
	{
		GameObject worker_go = worker?.gameObject;
		if (!worker_go)
		{
			worker_go = this.WorkerBob;
		}

		DialogManager.singleton.Say_3D("Woo, we made it!", worker_go);
		DialogManager.singleton.Say_3D("... But that fish isn't going to process itself.", worker_go);
		DialogManager.singleton.Say_3D("Drag a worker over to the processing plant.", worker_go);

		StartCoroutine(ProcessingReminderTimer(worker_go));
	}

	private IEnumerator StartReminderTimer()
	{
		while (!HasAssignedWorkerToBoat)
		{
			if (!HasDraggedWorker)
			{
				DialogManager.singleton.Say_3D("You can drag around workers...", this.WorkerBob);
				DialogManager.singleton.Say_3D("... just click on the circle under one and drag.", this.WorkerBob);
				yield return new WaitForSeconds(30f);
			}
			else if (!HasAssignedWorkerToBoat)
			{
				DialogManager.singleton.Say_3D("Now you should drag one of the workers over to the boats.", this.WorkerBob);
				yield return new WaitForSeconds(20f);
			}
			// Wait for an amount of time after the text has finished playing out.
			yield return new WaitForSeconds(Random.Range(25f, 45f));
		}
	}

	private IEnumerator ProcessingReminderTimer(GameObject worker_go)
	{
		while (!HasProcessedFish)
		{
			DialogManager.singleton.Say_3D("Quick! The fish is already going off.", worker_go);
			DialogManager.singleton.Say_3D("People are going to get sick if we leave it too long.", worker_go);
			yield return new WaitForSeconds(30f);
		}
	}
}
