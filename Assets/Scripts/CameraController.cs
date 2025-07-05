using UnityEngine;

public class CameraController : MonoBehaviour {


	// TODO: have those in the options menu
	public float scaleX;
	public float scaleY;
	public float scaleZ;


	public void OnArrival()
	{
		GetComponent<Animator> ().enabled = false;
	}


	void Update()
	{
		GetComponent<Rigidbody> ().AddForce (Vector3.right * Input.GetAxis ("Horizontal") * scaleX);
		GetComponent<Rigidbody> ().AddForce (Vector3.up * Input.GetAxis ("Vertical") * scaleY);
		GetComponent<Rigidbody> ().AddForce (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel") * scaleZ);

		if (transform.position.z > -10) {
			GetComponent<Rigidbody> ().AddForce (-Vector3.forward * 0.5f * scaleZ);
		}
	}

}
