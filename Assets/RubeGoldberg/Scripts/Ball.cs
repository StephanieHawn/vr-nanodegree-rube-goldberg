using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	#region Properties
	public GameManager gameManager;
	public AudioSource audioSource;
	public Rigidbody rigidBody;
	public Renderer ballRenderer;
	public Material activeMaterial;
	public Material inactiveMaterial;
	public Transform ballStart;
	public float resetInSeconds;
	#endregion

	#region Initialization
	void Start () {
		ballRenderer = GetComponent<Renderer> ();
		ResetPosition ();
	}
	#endregion

	#region Physics
	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag("Ground")) {
			PlayAudio ();
			if (gameManager.endGame == null) {
				StartCoroutine (Reset ());
			} else {
				if (!gameManager.endGame.hasCompletedGame) {
					StartCoroutine (Reset ());
				}
			}
		}
	}
	#endregion

	#region Change Materials
	public void SetActive (bool isActive) {
		if (isActive) {
			ballRenderer.material = activeMaterial;
		} else {
			ballRenderer.material = inactiveMaterial;
		}
	}
	#endregion

	#region Audio
	private void PlayAudio () {
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}
	}
	#endregion

	#region Reset Position
	private IEnumerator Reset () {
		// Wait for n seconds
		yield return new WaitForSeconds (resetInSeconds);

		// Reset physics
		ResetPhysics ();

		// Reset position and rotation
		ResetPosition ();

		// Reset Puzzle Collectibles
		gameManager.puzzle.ResetStars ();
	}

	private void ResetPhysics () {
		rigidBody.angularVelocity = Vector3.zero;
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = false;
	}

	private void ResetPosition () {
		transform.position = ballStart.position;
		transform.rotation = Quaternion.identity;
	}
	#endregion
}
