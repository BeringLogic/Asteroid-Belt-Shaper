using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BotControllerHauler : MonoBehaviour {

	MoveTowards movementController;
	JobHaulScript currentJob;
	JobsController jc;


	void Start () {
		movementController = GetComponent <MoveTowards> ();
		jc = Services.Find<JobsController> ();

		currentJob = null;
		StartCoroutine ("AI");
	}


	IEnumerator AI()
	{
		GameObject jobGO;

		while(true)
		{

			// Get a job
			while (currentJob == null) {
				jobGO = jc.TakeHaulingJob ();
				if (jobGO != null) {
					currentJob = jobGO.GetComponent<JobHaulScript> ();
				}
				yield return new WaitForSeconds (1);
			}


			// Go to the job's site
			if (currentJob != null) {
				movementController.SetDestination(currentJob.transform.position);
			}
			while (currentJob != null && movementController.isEnRoute) {
				yield return new WaitForSeconds (1);
			}
			movementController.isEnRoute = false; // to make sure you stop if the job you were going towards to got cancelled


			// Where to?
			// NOTE: destination can be left as null when creating the job. That way the bot will find the best place for it when it gets here.
			if (currentJob != null && currentJob.GetDestination() == Vector3.zero) {
				currentJob.SetDestination (FindBestDestination (currentJob));
			}

			// Haul to destination
			if (currentJob != null) {
				currentJob.transform.SetParent (transform);
				movementController.SetDestination(currentJob.GetComponent<JobHaulScript>().GetDestination ());
			}
			while (movementController.isEnRoute) {
				yield return new WaitForSeconds (1);
			}


			// Job's done!
			jc.CompleteHaulingJob (currentJob.gameObject);
			currentJob = null;

		}
	}


	protected Vector3 FindBestDestination(JobHaulScript job)
	{
		List<Vector3> destinations = new List<Vector3> ();
		Vector3 bestDestination;


		// TODO: Find build jobs requiring this material


		if (job.GetMaterial ().name == "Ice (Instance)") {
			// Find fuel tanks having room for this material
			foreach (var item in Services.Find<GameManager>().listOfFuelTanks) {
				if (!item.isFull ()) {
					// TODO: Reserve a spot there
					destinations.Add (item.GetPosition());
				}
			}
		}
		else {
			// Find storages having room for this material
			foreach (var item in Services.Find<GameManager>().listOfStorages) {
				if (!item.isFull ()) {
					// TODO: Reserve a spot there
					destinations.Add (item.GetPosition());
				}
			}
		}

		// Add the ship
		destinations.Add(Services.Find<ShipController> ().transform.position);


		// TODO: Figure out the best destination with a fancy algorythm. Closest is not always best. If Closest is a storage or ship, Closest might make the total trip to a job longuer
		// i.e.: solution for this example is not storage but buildjob:    buildjob ..... resource .. storage

		float d;
		float distanceToClosest = float.PositiveInfinity;
		bestDestination = Vector3.zero;
		foreach (var v in destinations) {
			d = Vector3.Distance (v, transform.position);
			if (d < distanceToClosest) {
				distanceToClosest = d;
				bestDestination = v;
			}
		}


		// TODO: No time even for closest, the first will be it LOL
		//bestDestination = destinations [0];

		return bestDestination;
	}


}
