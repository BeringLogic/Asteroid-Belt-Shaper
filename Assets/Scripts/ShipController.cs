using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {


	void Awake()
	{
		Services.Register<ShipController> (this);
	}


	public void OnArrival()
	{
		GetComponent<Animator> ().enabled = false;
	}


}
