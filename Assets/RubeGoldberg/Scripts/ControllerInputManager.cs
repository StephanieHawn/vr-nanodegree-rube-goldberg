using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {

	#region Properties (SteamVR)
	public SteamVR_TrackedObject trackedObject;
	public SteamVR_Controller.Device device;
	#endregion

	#region Initialization
	void Start () {
		if (trackedObject == null) {
			trackedObject = GetComponent<SteamVR_TrackedObject> ();
		}
	}
	#endregion
	
	#region Frame Update
	void Update () {
		if (device == null) {
			device = SteamVR_Controller.Input ((int)trackedObject.index);
		}
	}
	#endregion
}
