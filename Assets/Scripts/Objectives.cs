using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Objectives : MonoBehaviour {


	[SerializeField] protected Toggle BuildFuelTanksToggle;
	[SerializeField] protected Toggle BuildEngineToggle;
	[SerializeField] protected Toggle BuildStorageToggle;
	[SerializeField] protected Toggle FillFuelTanksToggle;
	[SerializeField] protected Toggle FillStoragesToggle;
	[SerializeField] protected Button WinButton;

	int fuelTanksCount;
	int fuelTanksFullCount;
	int storagesCount;
	int storagesFullCount;

	void Awake () {
		Services.Register<Objectives> (this);
	}


	// TODO: Track destruction of buildings too


	public void FuelTankBuilt()
	{
		fuelTanksCount++;

		if (fuelTanksCount >= 2) {
			BuildFuelTanksToggle.isOn = true;
		}
		CheckWinConditions();
	}


	public void StorageBuilt()
	{
		storagesCount++;

		BuildStorageToggle.isOn = true;
		CheckWinConditions();
	}


	public void EngineBuilt()
	{
		BuildEngineToggle.isOn = true;
		CheckWinConditions();
	}


	public void FuelTankFull()
	{
		fuelTanksFullCount++;
		if (fuelTanksFullCount != fuelTanksCount) {
			return;
		}

		FillFuelTanksToggle.isOn = true;
		CheckWinConditions();
	}


	public void StorageFull()
	{
		storagesFullCount++;
		if (storagesFullCount != storagesCount) {
			return;
		}

		FillStoragesToggle.isOn = true;
		CheckWinConditions();
	}


	protected void CheckWinConditions()
	{
		if (
			BuildFuelTanksToggle.isOn
			&&
			BuildStorageToggle.isOn
			&&
			BuildEngineToggle.isOn
			&&
			FillFuelTanksToggle.isOn
			&&
			FillStoragesToggle.isOn
		) {
			WinButton.gameObject.SetActive(true);
		}
	}

}
