using UnityEngine;
using System.Collections;


public class BotControllerBuilder : MonoBehaviour {

	MoveTowards movementController;
	GameObject currentJob;
	JobsController jc;


	void Start () {
		movementController = GetComponent <MoveTowards> ();
		jc = Services.Find<JobsController> ();

		currentJob = null;
		StartCoroutine ("AI");
	}


	IEnumerator AI()
	{
		while(true)
		{
			
			while (currentJob == null) {
				currentJob = jc.TakeBuildingJob ();
				yield return new WaitForSeconds (1);
			}

			if (currentJob != null) {
				movementController.SetDestination(currentJob.transform.position);
			}

			while (currentJob != null && movementController.isEnRoute) {
				yield return new WaitForSeconds (1);
			}
			movementController.isEnRoute = false; // to make sure you stop if the job you were going towards to got cancelled

			if (currentJob != null) {
				yield return new WaitForSeconds (3);
			}

			if (currentJob != null) {
				jc.CompleteBuildingJob (currentJob);
				currentJob = null;
			}

		}
	}

}
