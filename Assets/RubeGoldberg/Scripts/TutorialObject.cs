using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour {

	#region Properties
	public Tutorial tutorial;
	#endregion

	#region Physics
	void OnTriggerStay (Collider other) {
		if (other.CompareTag("Structure")) {
			gameObject.SetActive (false);
		}
	}
	#endregion
}
