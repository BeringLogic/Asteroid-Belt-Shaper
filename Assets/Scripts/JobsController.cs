using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class JobsController : MonoBehaviour
{
	List<GameObject> ListOfAllJobs;
	public List<GameObject> ListOfMeltingJobs;
	public List<GameObject> ListOfHaulingJobs;
	public List<GameObject> ListOfBuildingJobs;

	// TODO: This is probably not the right place for that
	public GameObject destinationMarkerPrefab;

	[SerializeField] protected GameObject meltingJobPrefab;
	[SerializeField] protected GameObject haulingJobPrefab;
	[SerializeField] protected GameObject buildingJobPrefab;


	void Awake ()
	{
		Services.Register<JobsController> (this);

		ListOfAllJobs = new List<GameObject> ();
		ListOfMeltingJobs = new List<GameObject> ();
		ListOfHaulingJobs = new List<GameObject> ();
		ListOfBuildingJobs = new List<GameObject> ();
	}


	// TODO: Refactor all the helpers (CreateXJob(), CompleteXJob(), etc) to use their respective type instead of GameObject.
	// TODO: THEN, refactor this whole system to call the script's type instead (X.CreateJob(), X.CompleteJob(), etc.)
	// TODO: Probably also refactor the jobs lists to be part of the job script

	public GameObject CreateMeltingJob(Transform transform)	{ return CreateJob (meltingJobPrefab, transform, ListOfMeltingJobs); }
	public GameObject CreateHaulingJob(Transform transform) { return CreateJob (haulingJobPrefab, transform, ListOfHaulingJobs); }
	public GameObject CreateBuildingJob(Transform transform) { return CreateJob (buildingJobPrefab, transform, ListOfBuildingJobs); }
	protected GameObject CreateJob(GameObject prefab, Transform transform, List<GameObject> list)
	{
		GameObject job = (GameObject) Instantiate (prefab, transform.position, transform.rotation);
		ListOfAllJobs.Add (job);
		list.Add(job);
		return job;
	}


	public void CancelJob(GameObject job)
	{
		if (job.GetComponent<JobMeltScript> () != null) {
			ListOfMeltingJobs.Remove (job);
		}
		if (job.GetComponent<JobHaulScript> () != null) {;
			ListOfHaulingJobs.Remove (job);
		}
		if (job.GetComponent<JobBuildScript> () != null) {
			ListOfBuildingJobs.Remove (job);
		}

		ListOfAllJobs.Remove (job);
		Destroy (job);
	}


	public bool isJobAvailable(GameObject job)
	{
		if (job == null) {
			return false;
		}
		if (job.tag == "JobTaken") {
			return false;
		}
		return true;
	}
	protected GameObject TakeJob(GameObject job)
	{
		job.tag = "JobTaken";
		return job;
	}
	

	public GameObject TakeMeltingJob() { return GiveJobInList (ListOfMeltingJobs); }
	public GameObject TakeHaulingJob() { return GiveJobInList (ListOfHaulingJobs); }
	public GameObject TakeBuildingJob() { return GiveJobInList (ListOfBuildingJobs); }
	protected GameObject GiveJobInList(List<GameObject> list)
	{
		foreach(var job in list) {
			if (!isJobAvailable(job)) {
				continue;
			}

			return TakeJob(job);
		}

		return null;
	}


	public void CompleteMeltingJob(GameObject job) {
		AsteroidCubeScript acs = job.transform.parent.GetComponent<AsteroidCubeScript> ();

		GameObject newHaulingJob = CreateHaulingJob (acs.transform);
		newHaulingJob.GetComponent<JobHaulScript> ().SetMaterial(acs.GetComponent<Renderer>().material);

		Destroy (acs.gameObject);
		CompleteJob (job);
	}
	public void CompleteHaulingJob(GameObject job) {
		job.GetComponent<JobHaulScript>().CompleteJob();
		CompleteJob (job);
	}
	public void CompleteBuildingJob(GameObject job) {
		job.GetComponent<JobBuildScript>().CompleteJob();
		CompleteJob (job);
	}
	protected void CompleteJob(GameObject job)
	{
		CancelJob (job);
	}

}

