using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AsteroidCubeScript : MonoBehaviour {

	GameObject jobAtThisPosition; // TODO: I could use the transform.GetComponentInChildren<JobMeltScript>() instead of using that var
	JobsController jc;


	void Start () {
		jc = Services.Find<JobsController> ();
	}


	public void SetMaterial(Material newMaterial)
	{
		GetComponentInChildren<Renderer> ().material = newMaterial;
	}


	void OnMouseOver()
	{
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		if (Input.GetMouseButton (0)) {
			CreateMeltingJob();
		}

		if (Input.GetMouseButton (1)) {
			CancelMeltingJob ();
			
		}
	}


	protected void CreateMeltingJob()
	{
		if (jobAtThisPosition != null) {
			return;
		}

		jobAtThisPosition = jc.CreateMeltingJob (transform);
		jobAtThisPosition.transform.SetParent (transform);
	}


	protected void CancelMeltingJob()
	{
		if (jobAtThisPosition == null) {
			return;
		}

		jc.CancelJob (jobAtThisPosition);
		jobAtThisPosition = null;
	}

}
