using UnityEngine;
using UnityEngine.Events;


public class JobHaulScript : MonoBehaviour {


	protected Material material;
	protected Vector3 destination;
	protected GameObject destinationMarker;

	[System.Serializable] public class JobCompleteEventClass : UnityEvent<JobHaulScript> {}
	public JobCompleteEventClass JobCompleteEvent;


	public Material GetMaterial()
	{
		return material;
	}


	public void SetMaterial(Material newMaterial)
	{
		material = newMaterial;
		GetComponentInChildren<Renderer> ().material = material;
	}


	public Vector3 GetDestination()
	{
		// NOTE: destination can be left as null when creating the job. That way the bot will find the best place for it when it gets here.
		return destination;
	}


	public void SetDestination(Vector3 newDestination)
	{
		// NOTE: destination can be left as null when creating the job. That way the bot will find the best place for it when it gets here.
		destination = newDestination;

		if (destinationMarker == null) {
			destinationMarker = (GameObject)Instantiate (Services.Find<JobsController> ().destinationMarkerPrefab);
		}

		destinationMarker.transform.position = destination;

		// TODO: Reserve a slot at the destination (to prevent overcapacity in storages)
	}


	public void CompleteJob()
	{
		// The destinationMarker is not linked to this GameObject so we need to destroy it ourselves
		if (destinationMarker != null) {
			Destroy(destinationMarker);
		}


		foreach (var item in Services.Find<GameManager>().listOfStorages) {
			Vector3 p = transform.parent.transform.position;
			if (Mathf.Round (p.x) != Mathf.Round (item.GetPosition().x)) {
				continue;
			}
			if (Mathf.Round (p.y) != Mathf.Round (item.GetPosition().y)) {
				continue;
			}

			// TODO: create another job if the store is full?


			item.Store (transform.GetChild (0).gameObject);
		}

		foreach (var item in Services.Find<GameManager>().listOfFuelTanks) {
			Vector3 p = transform.parent.transform.position;
			if (Mathf.Round (p.x) != Mathf.Round (item.GetPosition().x)) {
				continue;
			}
			if (Mathf.Round (p.y) != Mathf.Round (item.GetPosition().y)) {
				continue;
			}

			// TODO: create another job if the store is full?


			item.Store (transform.GetChild (0).gameObject);
		}

		// TODO: Increment ressources counter: GameObject.Find (job.GetComponent<Renderer> ().material.name + " Guage").GetComponent<RessourcePanel> ().count++;
		// TODO: put the can in the storage or fuel tank

		JobCompleteEvent.Invoke (this);
	}

}
