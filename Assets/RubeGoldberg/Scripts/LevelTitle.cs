using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTitle : MonoBehaviour {

	#region Properties
	public Text level;
	public Text title;
	private Color zeroAlpha = new Color(1, 1, 1, 0);
	#endregion

	#region Initialization
	void Start () {
		StartCoroutine (FadeTitle ());
	}
	#endregion

	#region Fade Alpha
	private IEnumerator FadeTitle () {
		yield return new WaitForSeconds (1.5f);
		level.CrossFadeColor (zeroAlpha, 1.5f, true, true);
		title.CrossFadeColor (zeroAlpha, 1.5f, true, true);
	}
	#endregion
}
