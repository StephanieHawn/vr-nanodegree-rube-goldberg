using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour {

	#region Properties (Game Manager)
	public GameManager gameManager;
	#endregion

	#region Properties (Controller Input Manager)
	public ControllerInputManager controllerInputManager;
	private SteamVR_Controller.Device controller;
	#endregion

	#region Properties
	private bool isHoldingBall;
	#endregion

	public float throwForce = 1.5f;

	#region Initialization
	private void InitializeController () {
		//Controller Input Manager
		if (controllerInputManager == null) {
			controllerInputManager = GetComponent<ControllerInputManager> ();
		}

		if (controller == null && controllerInputManager != null) {
			controller = controllerInputManager.device;
		}
	}
	#endregion

	#region Frame Update
	void Update () {
		if (controllerInputManager == null || controller == null) {
			InitializeController ();
		} else if (gameManager.tutorial != null && gameManager.tutorial.startButton.isHighlighted) {
			StartButton ();
		} else if (gameManager.endGame != null && gameManager.endGame.restartButton.isHighlighted) {
			RestartButton ();
		} else {
			CheckForDebugMode ();
		}

		if (isHoldingBall) {
			gameManager.NoCheating ();
		}
	}
	#endregion

	#region Controller Button (Trigger)
	void OnTriggerStay(Collider other) {

		if (other.gameObject.CompareTag ("Throwable")) {
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				GrabObject (other);
				isHoldingBall = true;
			} else if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
				ThrowObject (other);
				isHoldingBall = false;
			}
		}
		else if (other.gameObject.CompareTag ("Structure") || other.gameObject.CompareTag("Trampoline")) {
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				GrabObject (other);
				ShowTutorialObject ();
			} else if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
				PlaceObject (other);
				PlaceTutorialObject ();
			}
		}
	}
	#endregion

	#region Throwable Objects
	private void GrabObject(Collider collider) {
		collider.transform.SetParent (transform);
		collider.GetComponent<Rigidbody> ().isKinematic = true;
		controller.TriggerHapticPulse (2000);
	}

	private void ThrowObject(Collider collider) {
		collider.transform.SetParent (null);
		Rigidbody rigidBody = collider.GetComponent<Rigidbody> ();
		rigidBody.isKinematic = false;
		rigidBody.velocity = controller.velocity * throwForce;
		rigidBody.angularVelocity = controller.angularVelocity;
	}
	#endregion

	#region Structure Objects
	private void PlaceObject(Collider collider) {
		collider.transform.SetParent (null);
		if (collider.CompareTag("Trampoline")) {
			collider.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}
	#endregion

	#region Tutorial Objects
	private void StartButton () {
		if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			gameManager.tutorial.PressStart ();
		}
	}

	private void PlaceTutorialObject () {
		if (gameManager.tutorial != null) {
			if (!gameManager.tutorial.metalPlank.activeInHierarchy && !gameManager.tutorial.isMetalPlankPositioned) {
				gameManager.tutorial.MetalPlankInPosition ();
				gameManager.tutorial.isMetalPlankPositioned = true;
			} else {
				HideTutorialObject ();
			}
		}
	}

	private void ShowTutorialObject () {
		if (gameManager.tutorial != null) {
			if (!gameManager.tutorial.metalPlank.activeInHierarchy && !gameManager.tutorial.isMetalPlankPositioned) {
				gameManager.tutorial.metalPlank.SetActive (true);
			}
		}
	}

	private void HideTutorialObject () {
		if (gameManager.tutorial != null) {
			if (gameManager.tutorial.metalPlank.activeInHierarchy && !gameManager.tutorial.isMetalPlankPositioned) {
				gameManager.tutorial.metalPlank.SetActive (false);
			}
		}
	}
	#endregion

	#region End Game Objects
	private void RestartButton () {
		if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			gameManager.endGame.PressRestart ();
		}
	}
	#endregion

	#region Debug Mode
	private void CheckForDebugMode () {
		if (controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
				gameManager.ToggleDebugMode ();
			}
		}
	}
	#endregion
}
