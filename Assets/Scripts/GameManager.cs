using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	[SerializeField] protected BotControllerMelter BotMelterPrefab;
	[SerializeField] protected BotControllerHauler BotHaulerPrefab;
	[SerializeField] protected BotControllerBuilder BotBuilderPrefab;

	[SerializeField] protected GameObject blueprintSingleCubePrefab;
	[SerializeField] protected GameObject blueprintStoragePrefab;
	[SerializeField] protected GameObject blueprintFuelTankPrefab;
	[SerializeField] protected GameObject blueprintEnginePrefab;

	[SerializeField] protected GameObject resourcePanelPrefab;
	[SerializeField] protected int capacityOfSingleStorage;
	[SerializeField] protected int capacityOfSingleFuelTank;

	public List<ResourcePanel> listOfStorages;
	public List<ResourcePanel> listOfFuelTanks;
	Objectives objectives;
	ShipController ship;


	// TODO: Make sure there is AT LEAST enough resources on the map to be able to win


	void Awake()
	{
		Services.Register<GameManager> (this);
		listOfStorages = new List<ResourcePanel> ();
		listOfFuelTanks = new List<ResourcePanel> ();
	}


	void Start ()
	{
		ship = Services.Find<ShipController> ();
		objectives = Services.Find<Objectives> ();
	}


	public void BuildMelterBot() {
		Instantiate (BotMelterPrefab, ship.transform.position, Quaternion.identity);
	}

	public void BuildHaulerBot()
	{
		Instantiate (BotHaulerPrefab, ship.transform.position, Quaternion.identity);
	}

	public void BuildBuilderBot()
	{
		Instantiate (BotBuilderPrefab, ship.transform.position, Quaternion.identity);
	}


	public void PlaceSingleCubeBlueprint()
	{
		GameObject go = (GameObject) Instantiate (blueprintSingleCubePrefab);
		go.GetComponent<Blueprint> ().BuildingCompleteEvent.AddListener (OnSingleCubeBuilt);
	}
	protected void OnSingleCubeBuilt(Blueprint bp)
	{
		Destroy(bp.gameObject);
	}


	public void PlaceFuelTankBlueprint()
	{
		GameObject go = (GameObject) Instantiate (blueprintFuelTankPrefab);
		go.GetComponent<Blueprint> ().BuildingCompleteEvent.AddListener (OnFuelTankBuilt);
	}
	protected void OnFuelTankBuilt(Blueprint bp)
	{
		GameObject go = Instantiate (resourcePanelPrefab);
		ResourcePanel panel = go.GetComponent<ResourcePanel> ();
		panel.IsFullEvent.AddListener (OnFuelTankFull);
		panel.SetCapacity(capacityOfSingleFuelTank);
		panel.SetPosition (bp.transform.position);
		panel.SetLabel("Fuel Tank");

		listOfFuelTanks.Add (panel);
		objectives.FuelTankBuilt ();
		Destroy (bp.gameObject);
	}
	protected void OnFuelTankFull()
	{
		objectives.FuelTankFull ();
	}


	public void PlaceStorageBlueprint()
	{
		GameObject go = (GameObject) Instantiate (blueprintStoragePrefab);
		go.GetComponent<Blueprint> ().BuildingCompleteEvent.AddListener (OnStorageBuilt);
	}
	protected void OnStorageBuilt(Blueprint bp)
	{
		GameObject go = Instantiate (resourcePanelPrefab);
		ResourcePanel panel = go.GetComponent<ResourcePanel> ();
		panel.IsFullEvent.AddListener (OnStorageFull);
		panel.SetCapacity(capacityOfSingleStorage);
		panel.SetPosition (bp.transform.position);
		panel.SetLabel("Storage");

		listOfStorages.Add (panel);
		objectives.StorageBuilt ();
		Destroy (bp.gameObject);
	}
	protected void OnStorageFull()
	{
		objectives.StorageFull();
	}


	public void PlaceEngineBlueprint()
	{
		GameObject go = (GameObject) Instantiate (blueprintEnginePrefab);
		go.GetComponent<Blueprint> ().BuildingCompleteEvent.AddListener (OnEngineBuilt);
	}
	protected void OnEngineBuilt(Blueprint bp)
	{
		objectives.EngineBuilt ();
		Destroy (bp.gameObject);
	}


	public void OnExitButtonClicked()
	{
		Application.Quit ();
	}

}
