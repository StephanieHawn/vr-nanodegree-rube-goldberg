using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {

	#region Properties
	public int strength = 50;
	#endregion

	#region Physics
	void OnTriggerStay (Collider other) {
		Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody> ();
		if (rigidBody != null) {
			rigidBody.AddForce (transform.forward * strength);
		}
	}
	#endregion
}
