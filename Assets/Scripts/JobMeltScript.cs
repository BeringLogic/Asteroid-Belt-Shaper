using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class JobMeltScript : MonoBehaviour {


	void OnMouseOver()
	{
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		if (Input.GetMouseButton (1)) {
			Services.Find<JobsController>().CancelJob(gameObject);
		}
	}


}
