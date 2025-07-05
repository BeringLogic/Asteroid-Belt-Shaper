using UnityEngine;
using System.Collections;
using System;

public class BlueprintPlacementProbe : MonoBehaviour {

	// TODO: OUT OF TIME!

	protected Blueprint bp;
	protected Color originalColor;

	void Start()
	{
		bp = GetComponentInParent<Blueprint> ();
		originalColor = GetComponent<Renderer>().material.color;
	}


	void Update()
	{
		// TODO: Do some raycasting under this
		// TODO: if raycast sees stuff, change color and bp.canBeBuiltHere = false;
	}

}
