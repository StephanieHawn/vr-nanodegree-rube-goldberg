using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

	#region Properties
	public Button button;
	public Image buttonImage;
	public bool isHighlighted;
	#endregion

	#region Color
	public void Highlight () {
		buttonImage.color = button.colors.highlightedColor;
		isHighlighted = true;
	}

	public void Normalize () {
		buttonImage.color = button.colors.normalColor;
		isHighlighted = false;
	}
	#endregion
}
