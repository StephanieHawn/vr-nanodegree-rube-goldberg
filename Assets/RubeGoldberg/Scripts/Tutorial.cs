
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	#region Properties
	public AudioManager audioManager;
	public GameObject metalPlank;
	public bool isMetalPlankPositioned;

	public Teleporter teleporter;
	public ObjectMenuManager objectMenuManager;

	public bool hasPressedStart;
	public LevelTitle levelTitle;
	public StartButton startButton;
	public GameObject star;
	public GameObject ball;

	public LineRenderer laserPointer;
	public GameObject laserTarget;
	public LayerMask laserMask;
	#endregion

	#region Frame Update
	void Update () {
		if (!hasPressedStart) {
			UpdateLaserTrajectory ();
		}
	}
	#endregion

	#region Laser Pointer
	private void UpdateLaserTrajectory () {
		laserPointer.numPositions = 2;
		laserPointer.SetPosition (0, laserPointer.transform.position);
		laserPointer.SetPosition (1, laserTarget.transform.position);

		RaycastHit hit;
		if (Physics.Raycast (laserPointer.transform.position, laserPointer.transform.forward, out hit, 10, laserMask)) {
			startButton.Highlight ();
		} else {
			startButton.Normalize ();
		}
	}
	#endregion

	#region Welcome
	public void PressStart () {
		startButton.isHighlighted = false;
		startButton.gameObject.SetActive (false);
		laserPointer.gameObject.SetActive (false);
		hasPressedStart = true;
		levelTitle.enabled = true;
		star.SetActive (true);
		ball.SetActive (true);
		StartCoroutine (PlayWelcome ());
	}

	private IEnumerator PlayWelcome () {
		audioManager.source.clip = audioManager.welcome;
		audioManager.source.Play ();

		yield return new WaitForSeconds (15);
		TeleportPartOne ();
	}
	#endregion

	#region Teleport
	private void TeleportPartOne () {
		// Enable Teleporter
		teleporter.isEnabled = true;

		// Play Audio Clip 1
		audioManager.source.clip = audioManager.teleport1;
		audioManager.source.Play ();
	}

	public IEnumerator TeleportPartTwo () {
		// Play Audio Clip 2
		audioManager.source.clip = audioManager.teleport2;
		audioManager.source.Play ();

		yield return new WaitForSeconds (8);

		// Play Audio Clip 3
		audioManager.source.clip = audioManager.teleport3;
		audioManager.source.Play ();
	}

	public IEnumerator  PlayerHasTeleported () {
		audioManager.source.clip = audioManager.expletive;
		audioManager.source.Play ();

		yield return new WaitForSeconds (4);

		ObjectMenuPartOne ();
	}
	#endregion

	#region Object Menu
	private void ObjectMenuPartOne () {
		objectMenuManager.isEnabled = true;

		// Play Audio Clip 1
		audioManager.source.clip = audioManager.objectMenu1;
		audioManager.source.Play ();
	}

	public void ObjectMenuPartTwo () {
		// Play Audio Clip 2
		audioManager.source.clip = audioManager.objectMenu2;
		audioManager.source.Play ();
	}

	public void ObjectMenuPartThree () {
		// Play Audio Clip 3
		audioManager.source.clip = audioManager.objectMenu3;
		audioManager.source.Play ();
	}

	public void PlayerHasSpawnedObject () {
		audioManager.source.clip = audioManager.positionObject;
		audioManager.source.Play ();
	}

	public void MetalPlankInPosition () {
		audioManager.source.clip = audioManager.excellent;
		audioManager.source.Play ();
	}
	#endregion
}
