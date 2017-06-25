using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

	#region Properties
	public Puzzle puzzle;
	public AudioManager audioManager;
	#endregion

	#region Physics
	void OnTriggerStay (Collider other) {
		if (other.CompareTag("Throwable")) {
			Rigidbody rigidBody = other.GetComponent<Rigidbody> ();
			if (!rigidBody.isKinematic) {
				audioManager.collectableSoundSource.transform.position = transform.position;
				audioManager.collectableSoundSource.Play ();
				gameObject.SetActive (false);
				puzzle.CollectStar ();
			}
		}
	}
	#endregion
}
