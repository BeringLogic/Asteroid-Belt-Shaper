using UnityEngine;

public class ResourcePanelsList : MonoBehaviour {


	void Awake() {
		Services.Register<ResourcePanelsList>(this);
	}


}
