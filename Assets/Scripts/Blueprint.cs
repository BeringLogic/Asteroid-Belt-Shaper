using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Blueprint : MonoBehaviour {

	Map map;
	Camera cam;
	Vector3 position;

	[System.Serializable] public class BuildingCompleteEventClass : UnityEvent<Blueprint> {}
	public BuildingCompleteEventClass BuildingCompleteEvent;

	public bool canBeBuiltHere;

	List<GameObject> jobs;
	JobsController jc;
	bool isInPosition;


	void Awake()
	{
		cam = Camera.main;
		jobs = new List<GameObject>();
		canBeBuiltHere = true;
	}


	void Start()
	{
		map = Services.Find<Map> ();
		jc = Services.Find<JobsController> ();
		transform.SetParent (jc.transform);
	}


	void Update () {
		if (isInPosition) {
			return;
		}

		FollowMouse ();

		if (canBeBuiltHere && Input.GetMouseButtonUp (0)) {
			PlaceBlueprint ();
			return;
		}

		if (Input.GetMouseButtonUp (1)) {
			Destroy (gameObject);
			return;
		}


		// Reset the flag. It will be flipped back to false if needed
		canBeBuiltHere = true;
	}


	void FollowMouse()
	{
		position = Input.mousePosition;
		position.z = -cam.transform.position.z;
		position = cam.ScreenToWorldPoint (position);

		position.x = Mathf.Round (position.x);
		position.y = Mathf.Round (position.y);

		transform.position = position;
	}


	void PlaceBlueprint()
	{
		Transform t;
		JobHaulScript job;
		Transform shipTransform = Services.Find<ShipController> ().transform;


		// Convert the blueprint's cubes into hauling jobs
		for (int i = 0; i < transform.childCount; i++) {
			t = transform.GetChild (i);

			job = jc.CreateHaulingJob (shipTransform).GetComponent<JobHaulScript> ();
			job.transform.rotation = Quaternion.identity;
			job.SetMaterial (t.GetComponent<Renderer> ().material);
			job.SetDestination (t.position);
			job.JobCompleteEvent.AddListener (CompleteHaulingJob);
			jobs.Add (job.gameObject);

			Destroy (t.gameObject);
		}

		// Out of the previous loop because changing children while looping using .childCount and .GetChild(i) is not a good plan
		foreach (var j in jobs) {
			j.transform.SetParent (transform);
		}

		isInPosition = true;
	}


	public void CompleteHaulingJob(JobHaulScript haulingJob)
	{
		JobBuildScript buildingJob;

		buildingJob = jc.CreateBuildingJob (haulingJob.transform).GetComponent<JobBuildScript>();
		buildingJob.SetMaterial (haulingJob.GetMaterial ());
		buildingJob.JobCompleteEvent.AddListener (CompleteBuildingJob);

		jobs.Remove (haulingJob.gameObject);
		jobs.Add (buildingJob.gameObject);
	}


	public void CompleteBuildingJob(JobBuildScript job)
	{
		jobs.Remove (job.gameObject);

		map.CreateCube (job.transform, job.GetMaterial());

		// TODO: maybe disable the AsteroidCubeScript, to prevent melting this new cube? Or just let the Building class destroy the building if you melt one of it's cubes?

		if (jobs.Count == 0) {
			// TODO: Instantiate the finish building prefab
			// TODO: Activate the finished building (play sounds, update hud, etc.)
			BuildingCompleteEvent.Invoke (this);
		}
	}


	public void CancelJob()
	{
		foreach (var job in jobs) {
			jc.CancelJob (job.gameObject);
		}

		jobs.Clear ();
		Destroy (gameObject);
	}


}
