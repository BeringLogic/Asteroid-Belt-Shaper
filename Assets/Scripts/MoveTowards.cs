using UnityEngine;
using System.Collections;

public class MoveTowards : MonoBehaviour {

	public Vector3 destination;
	public float maxTurnSpeed;
	public float maxSpeed;
	public bool isEnRoute;

	// TODO: [SerializeField] SFX EngineSound;

	public void SetDestination(Vector3 newDestination)
	{
		destination = newDestination;
		isEnRoute = true;
	}


	void Update()
	{
		if (!isEnRoute) {
			return;
		}

		if (Vector3.Distance (transform.position, destination) < 0.05f) {
			// TODO: Stop Engine Sound
			isEnRoute = false;
			return;
		}

		// TODO: Fix rotation?
		//Quaternion destinationHeading = Quaternion.LookRotation (destination - transform.position);
		//transform.rotation = Quaternion.RotateTowards (transform.rotation, destinationHeading, maxTurnSpeed * Time.deltaTime);

		transform.position = Vector3.MoveTowards(transform.position, destination, maxSpeed * Time.deltaTime);
	}
}
