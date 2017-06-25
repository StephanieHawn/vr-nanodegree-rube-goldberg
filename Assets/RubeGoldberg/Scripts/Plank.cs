using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour {

	#region Properties
	public AudioSource audioSource;
	#endregion

	#region Physics
	void OnCollisionEnter (Collision collision) {
		if (!audioSource.isPlaying && collision.gameObject.CompareTag("Throwable")) {
			audioSource.Play ();
		}
	}
	#endregion
}
