using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour {

	#region Properties (Game Manager)
	public GameManager gameManager;
	#endregion

	#region Properties (Controller Input Manager)
	public ControllerInputManager controllerInputManager;
	private SteamVR_Controller.Device controller;
	#endregion

	#region Properties (Laser Pointer)
	public LineRenderer laserEmitter;
	public GameObject laserTarget;
	public LayerMask laserMask;

	private Material laserMaterial;
	public Color validColor;
	public Color invalidColor;
	#endregion

	#region Properties (Player and Play Area)
	public Transform playArea;
	private Vector3 playerPosition {
		get {
			return new Vector3 (SteamVR_Render.Top ().head.localPosition.x, 0, SteamVR_Render.Top ().head.localPosition.z);
		}
	}
	#endregion
	
	#region Propeties (Teleport)
	public bool isEnabled;
	public bool isTargetValid;
	#endregion

	#region Properties (Dash)
	public float dashSpeed = 0.1f;
	private bool isDashing;
	private float lerpTime;

	private Vector3 startPosition;
	private Vector3 targetPosition;
	#endregion

	#region Properties (Tutorial)
	public bool isPlayingTutorial;
	public bool isTeleporting;
	public bool hasTeleported;
	#endregion

	#region Initialization
	void Start () {
		// Laser Pointer
		laserMaterial = laserEmitter.material;
		laserMaterial.color = invalidColor;
	}

	private void InitializeController () {
		// Controller Input Manager
		if (controllerInputManager == null) {
			controllerInputManager = GetComponent<ControllerInputManager> ();
		}

		// Controller
		if (controller == null && controllerInputManager != null) {
			controller = controllerInputManager.device;
		}
	}
	#endregion
	
	#region Frame Update
	void Update () {
		if (controllerInputManager == null || controller == null) {
			InitializeController ();
		}

		if (isDashing) {
			Dash ();
			TutorialDash ();

		} else if (controllerInputManager != null && controller != null && isEnabled) {
			if (controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
				TurnLaserPointerOn ();
				UpdateLaserTrajectory ();
				TutorialGetPress ();
			}

			if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
				TurnLaserPointerOff ();

				if (isTargetValid) {
					startPosition = playArea.position;
					isDashing = true;
				}
			}
		}
	}
	#endregion

	#region Locomotion
	private void Dash () {
		lerpTime += 1 * dashSpeed;
		playArea.position = Vector3.Lerp (startPosition, targetPosition - playerPosition, lerpTime);

		if (lerpTime >= 1)
		{
			isDashing = false;
			lerpTime = 0;

			// Make sure the player isn't cheating
			gameManager.NoCheating ();
		}
	}
	#endregion

	#region Laser Pointer
	private void TurnLaserPointerOn () {
		laserEmitter.gameObject.SetActive (true);
		laserTarget.SetActive (true);
	}

	private void TurnLaserPointerOff () {
		laserEmitter.gameObject.SetActive (false);
		laserTarget.SetActive (false);
	}

	private void UpdateLaserTrajectory () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 10, laserMask))
		{
			targetPosition = hit.point;
			Vector3[] points = InterpolateCurve (GeneratePoints (), 8);
			laserEmitter.numPositions = points.Length;

			for (int i = 0; i < points.Length; i++) {
				laserEmitter.SetPosition (i, points [i]);
			}

			// aimer position
			laserTarget.transform.position = new Vector3 (targetPosition.x, targetPosition.y, targetPosition.z);
			if (hit.collider.CompareTag("Platform")) {
				SetTargetToValid ();
			} else {
				CheckForNavMesh ();
			}

		} else {
			targetPosition = transform.position + transform.forward * 10;
			RaycastHit groundRay;
			if (Physics.Raycast (targetPosition, -Vector3.up, out groundRay, 10, laserMask)) {
				targetPosition = new Vector3 (transform.forward.x * 10 + transform.position.x, hit.point.y, transform.forward.z * 10 + transform.position.z);
				if (groundRay.collider.CompareTag("Platform")) {
					SetTargetToValid ();
				} else {
					CheckForNavMesh ();
				}
			} else {
				targetPosition = new Vector3 (transform.forward.x * 10 + transform.position.x, hit.point.y, transform.forward.z * 10 + transform.position.z);
				SetTargetToInvalid ();
			}

			Vector3[] points = InterpolateCurve (GeneratePoints (), 8);
			laserEmitter.numPositions = points.Length;

			for (int i = 0; i < points.Length; i++) {
				laserEmitter.SetPosition (i, points [i]);
			}

			// aimer position
			laserTarget.transform.position = targetPosition;
		}
	}

	private void CheckForNavMesh () {
		NavMeshHit navHit;
		if (NavMesh.SamplePosition (targetPosition, out navHit, 0.5f, -1)) {
			SetTargetToValid ();
		} else {
			SetTargetToInvalid ();
		}
	}

	private void CheckForPlatform (RaycastHit hit) {
		if (hit.collider.CompareTag("Teleport")) {
			SetTargetToValid ();
		}
	}

	private void SetTargetToValid () {
		laserMaterial.SetVector ("_EmissionColor", validColor);
		isTargetValid = true;
	}

	private void SetTargetToInvalid () {
		laserMaterial.SetVector ("_EmissionColor", invalidColor);
		isTargetValid = false;
	}
	#endregion

	#region Curved LineRenderer
	private Vector3[] GeneratePoints () {
		List<Vector3> points = new List<Vector3> {};
		points.Add (laserEmitter.transform.position);
		points.Add (((laserEmitter.transform.position + targetPosition) / 2) + new Vector3 (0, 1.5f, 0));
		points.Add (targetPosition);

		return points.ToArray ();
	}

	private static Vector3[] InterpolateCurve (Vector3[] basePoints, int interpolations) {
		List<Vector3> points;
		List<Vector3> interpolatedPoints;

		int numberOfBasePoints = 0;
		int numberOfInterpolatedPoints = 0;

		if (interpolations < 1) {
			interpolations = 1;
		}

		numberOfBasePoints = basePoints.Length;
		numberOfInterpolatedPoints = (numberOfBasePoints * interpolations) - 1;
		interpolatedPoints = new List<Vector3> (numberOfInterpolatedPoints);

		float time = 0;

		for (int i = 0; i < numberOfInterpolatedPoints + 1; i++) {
			time = Mathf.InverseLerp (0, numberOfInterpolatedPoints, i);
			points = new List<Vector3> (basePoints);

			for (int j = numberOfBasePoints - 1; j > 0; j--) {
				for (int k = 0; k < j; k++) {
					points [k] = ((1 - time) * points [k]) + (time * points [k + 1]);
				}
			}
			interpolatedPoints.Add (points [0]);
		}
		return (interpolatedPoints.ToArray ());
	}
	#endregion

	#region Teleport Tutorial
	private void TutorialGetPress () {
		if (isPlayingTutorial && !isTeleporting && !hasTeleported) {
			StartCoroutine (gameManager.tutorial.TeleportPartTwo ());
			isTeleporting = true;
		}
	}

	private void TutorialDash () {
		if (isPlayingTutorial && isTeleporting && !hasTeleported) {
			StopAllCoroutines ();
			StartCoroutine (gameManager.tutorial.PlayerHasTeleported ());
			isTeleporting = false;
			hasTeleported = true;
		}
	}
	#endregion
}
