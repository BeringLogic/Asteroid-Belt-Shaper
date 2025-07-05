using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ResourcePanel : MonoBehaviour {



	// NOTE: less than 1h remaining, this class has the model and the UI and it's getting pretty bad!!!
	protected Vector3 position;
	public Vector3 GetPosition()
	{
		return position;
	}
	public void SetPosition(Vector3 newPosition)
	{
		position = newPosition;
	}



	public UnityEvent IsFullEvent;


	[SerializeField] protected RectTransform guage;
	[SerializeField] protected Text label;

	protected List<GameObject> contents;
	protected int maxCapacity;


	public void SetLabel(string newLabel)
	{
		label.text = newLabel;
	}

	public void SetCapacity(int newCapacity)
	{
		maxCapacity = newCapacity;
		UpdateUI ();
	}


	public int GetLevel()
	{
		return contents.Count;
	}


	public bool isFull()
	{
		return (GetLevel() >= maxCapacity);
	}


	public void Store(GameObject can)
	{
		if (isFull()) {
			return;
		}

		contents.Add(can);
		// TODO: hide the can?

		if (isFull()) {
			guage.GetComponent <Image> ().color = Color.green;
			IsFullEvent.Invoke ();
		}

		UpdateUI ();
	}

	/*
	public GameObject Extract(Material requestedMaterial)
	{
		
	}
	*/

	public void UpdateUI()
	{
		//label.text = GetLevel().ToString() + " / " + maxCapacity.ToString();

		float percent = GetLevel() * 1f / maxCapacity;
		guage.sizeDelta = new Vector2 (percent*100, guage.sizeDelta.y);
	}
	
	
	void Awake()
	{
		contents = new List<GameObject> ();
	}


	void Start()
	{
		transform.SetParent (Services.Find<ResourcePanelsList>().transform);
		UpdateUI ();
	}

}
