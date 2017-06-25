using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {
	
	#region Properties
	public AudioManager audioManager;

	public bool hasCompletedGame;
	public LevelTitle endTitle;
	public StartButton restartButton;

	public Teleporter teleporter;
	public ObjectMenuManager objectMenuManager;

	public LineRenderer laserPointer;
	public GameObject laserTarget;
	public LayerMask laserMask;
	#endregion
	
	#region Frame Update
	void Update () {
		if (hasCompletedGame) {
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
			restartButton.Highlight ();
		} else {
			restartButton.Normalize ();
		}
	}
	#endregion

	#region End Game
	public void PressRestart () {
		SteamVR_LoadLevel.Begin ("Level01");
	}

	public void PlayAudio () {
		audioManager.source.clip = audioManager.gameover;
		audioManager.source.Play ();
	}
	#endregion
}
