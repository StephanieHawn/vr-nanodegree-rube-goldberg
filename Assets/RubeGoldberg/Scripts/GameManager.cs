using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	#region Properties
	public AudioManager audioManager;
	public int level = 0;
	public Tutorial tutorial;
	public EndGame endGame;
	public Puzzle puzzle;
	public GameObject solution;
	public Ball ball;
	public bool debugMode = false;
	public bool isCheating;
	#endregion

	#region Computed Properties
	private Vector3 playerPosition {
		get {
			return SteamVR_Render.Top ().head.position;
		}
	}
	#endregion

	#region Initialization
	void Start () {
		// Debug Mode
		if (debugMode) {
			solution.SetActive (true);
		} else {
			solution.SetActive (false);
		}

		// Level Titles
		StageLoaded ();
	}
	#endregion

	#region Cheat Prevention
	public void NoCheating () {
		if (puzzle.starsCollected > 0) {
			puzzle.ResetStars ();
		}

		LookForPlatform ();
	}

	private void LookForPlatform () {
		RaycastHit groundRay;
		if (Physics.Raycast (playerPosition, -Vector3.up, out groundRay)) {
			if (groundRay.collider.CompareTag ("Platform")) {
				isCheating = false;
				ball.SetActive (true);
			} else if (!groundRay.collider.CompareTag ("Throwable")) {
				isCheating = true;
				ball.SetActive (false);
			}
		}
	}
	#endregion

	#region Stage Audio
	private void PlayAudio () {
		if (audioManager.source.clip != null) {
			audioManager.source.Play ();
		}
	}
	#endregion

	#region Stage Management
	private void StageLoaded () {
		switch (level) {
		case 1:
			audioManager.source.clip = audioManager.level1;
			break;
		case 2:
			audioManager.source.clip = audioManager.level2;
			break;
		case 3:
			audioManager.source.clip = audioManager.level3;
			break;
		case 4:
			audioManager.source.clip = audioManager.level4;
			break;
		}

		PlayAudio ();
	}

	public static void LoadNextStage (int currentLevel) {
		string nextLevel = "";
		switch (currentLevel) {
		case 0:
			nextLevel = "Level01";
			break;
		case 1:
			nextLevel = "Level02";
			break;
		case 2:
			nextLevel = "Level03";
			break;
		case 3:
			nextLevel = "Level04";
			break;
		}

		SteamVR_LoadLevel.Begin (nextLevel);
	}

	public void EndGame () {
		if (endGame != null) {
			endGame.hasCompletedGame = true;
			endGame.endTitle.gameObject.SetActive (true);
			endGame.laserPointer.gameObject.SetActive (true);
			endGame.teleporter.isEnabled = false;
			endGame.objectMenuManager.isEnabled = false;
			endGame.PlayAudio ();

			foreach (Star star in puzzle.stars) {
				star.gameObject.SetActive (false);
			}
		}
	}
	#endregion

	#region Debug Mode
	public void ToggleDebugMode () {
		debugMode = !debugMode;

		if (debugMode) {
			solution.SetActive (true);
		} else {
			solution.SetActive (false);
		}
	}
	#endregion
}
