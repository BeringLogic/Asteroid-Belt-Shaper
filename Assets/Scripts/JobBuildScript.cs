using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class JobBuildScript : MonoBehaviour
{
	
	protected Material jobMaterial;
	protected Material material;
	protected Renderer r;

	[System.Serializable] public class JobCompleteEventClass : UnityEvent<JobBuildScript> {}
	public JobCompleteEventClass JobCompleteEvent;

	public Material GetMaterial()
	{
		return material;
	}

	public void SetMaterial(Material newMaterial)
	{
		material = newMaterial;
	}


	public void CompleteJob()
	{
		JobCompleteEvent.Invoke (this);
	}


	void Awake()
	{
		r = GetComponentInChildren<Renderer> ();
	}


	void OnMouseEnter()
	{
		jobMaterial = r.material;
		r.material = material;
	}


	void OnMouseExit()
	{
		r.material = jobMaterial;
	}


	// TODO: There was a bug with cancelling the build job but I'm out of time so.. you can't cancel a build job :-)
}

