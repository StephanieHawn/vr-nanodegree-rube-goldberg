using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	#region Properties
	public GameManager gameManager;
	#endregion

	#region Physics
	void OnTriggerStay(Collider other) {
		if ((gameManager.puzzle.starsCollected == gameManager.puzzle.stars.Count) && (!gameManager.isCheating)) {
			print ("goal reached");
			if (gameManager.puzzle.number < 4) {
				GameManager.LoadNextStage (gameManager.puzzle.number);
			} else {
				gameManager.EndGame ();
			}
		}
	}
	#endregion
}
