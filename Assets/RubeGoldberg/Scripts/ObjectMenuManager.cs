using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMenuManager : MonoBehaviour {
	#region Properties (Game Manager)
	private GameManager gameManager;
	#endregion

	#region Properties (Controller Input Manager)
	public ControllerInputManager controllerInputManager;
	private SteamVR_Controller.Device controller;
	#endregion

	#region Properties (Swipe)
	private float swipeSum;
	private float touchLast;
	private float touchCurrent;
	private float distance;
	private bool hasSwipedLeft;
	private bool hasSwipedRight;
	#endregion

	#region Properties (Objects)
	public List<GameObject> objectList; // handled automatically at start
	public List<GameObject> objectPrefabList; // set manually in inspector and MUST match order of scene menu objects
	#endregion

	#region Properties (Object Displays)
	public bool isEnabled;
	public GameObject objectDisplay;

	public Text objectTitle;
	public int currentObject = 0;

	public Text objectQuantity;
	public int currentQuantity = 0;
	#endregion

	#region Properties (Tutorial)
	public bool isPlayingTutorial;
	public bool isUsingMenu;
	public bool hasSpawnedObject;
	#endregion

	#region Initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();

		// Generate the list of objects
		foreach (Transform child in transform) {
			objectList.Add (child.gameObject);
		}

		// Setup the object title and quantity displays
		UpdateTitleDisplay ();
		UpdateQuantityDisplay ();
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

		if (controllerInputManager != null && controller != null && isEnabled) {
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				objectDisplay.SetActive (true);
				objectList [currentObject].SetActive (true);
				TutorialSwipe ();
			}
	
			if (controller.GetPress (SteamVR_Controller.ButtonMask.Grip)) {
				if (controller.GetTouchDown (SteamVR_Controller.ButtonMask.Touchpad)) {
					touchLast = controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
				}
	
				if (controller.GetTouch (SteamVR_Controller.ButtonMask.Touchpad)) {
					touchCurrent = controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
					distance = touchCurrent - touchLast;
					touchLast = touchCurrent;
					swipeSum += distance;
	
					if (!hasSwipedRight) {
						if (swipeSum > 0.5f) {
							swipeSum = 0;
							SwipeRight ();
							hasSwipedRight = true;
							hasSwipedLeft = false;
						}
					}
	
					if (!hasSwipedLeft) {
						if (swipeSum < -0.5f) {
							swipeSum = 0;
							SwipeLeft ();
							hasSwipedLeft = true;
							hasSwipedRight = false;
						}
					}
				}
	
				if (controller.GetTouchUp (SteamVR_Controller.ButtonMask.Touchpad)) {
					SwipeReset ();
				}
	
				if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
					SpawnCurrentObject ();
				}
			}
	
			if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Grip)) {
				objectDisplay.SetActive (false);
				objectList [currentObject].SetActive (false);
				TutorialSelectReset ();
			}
		}
	}

	#endregion

	#region Swipe
	private void SwipeLeft () {
		objectList [currentObject].SetActive (false);
		currentObject--;
		if (currentObject < 0) {
			currentObject = objectList.Count - 1;
		}
		objectList [currentObject].SetActive (true);
		UpdateTitleDisplay ();
		UpdateQuantityDisplay ();
		TutorialSelectPlank ();
	}

	private void SwipeRight () {
		objectList [currentObject].SetActive (false);
		currentObject++;
		if (currentObject > objectList.Count - 1) {
			currentObject = 0;
		}
		objectList [currentObject].SetActive (true);
		UpdateTitleDisplay ();
		UpdateQuantityDisplay ();
		TutorialSelectPlank ();
	}

	private void SwipeReset () {
		swipeSum = 0;
		touchCurrent = 0;
		touchLast = 0;
		hasSwipedRight = false;
		hasSwipedLeft = false;
	}
	#endregion

	#region Spawn Object
	public void SpawnCurrentObject () {
		Puzzle puzzle = gameManager.puzzle; 
		if (currentQuantity > 0) {
			switch (currentObject) {
			case 0:
				GameObject fan = puzzle.UseFan ();
				if (fan != null) {
					fan.transform.position = transform.position;
				}
				break;
			case 1:
				GameObject metalPlank = puzzle.UseMetalPlank ();
				if (metalPlank != null) {
					metalPlank.transform.position = transform.position;
					TutorialSpawn ();
				}
				break;
			case 2:
				GameObject trampoline = puzzle.UseTrampoline ();
				if (trampoline != null) {
					trampoline.transform.position = transform.position;
				}
				break;
			case 3:
				GameObject woodenPlank = puzzle.UseWoodenPlank ();
				if (woodenPlank != null) {
					woodenPlank.transform.position = transform.position;
				}
				break;
			}
			UpdateQuantityDisplay ();
		}
	}
	#endregion

	#region Object Displays
	private void UpdateTitleDisplay () {
		objectTitle.text = objectList [currentObject].name;
	}

	private void UpdateQuantityDisplay () {
		Puzzle puzzle = gameManager.puzzle;

		switch (currentObject) {
		case 0:
			currentQuantity = puzzle.FansAvailable;
			break;
		case 1:
			currentQuantity = puzzle.MetalPlanksAvailable;
			break;
		case 2:
			currentQuantity = puzzle.TrampolinesAvailable;
			break;
		case 3:
			currentQuantity = puzzle.WoodenPlanksAvailable;
			break;
		}

		objectQuantity.text = currentQuantity.ToString ();
	}
	#endregion

	#region Tutorial
	private void TutorialSwipe () {
		if (isPlayingTutorial && !isUsingMenu && !hasSpawnedObject) {
			gameManager.tutorial.ObjectMenuPartTwo ();
		}
	}

	private void TutorialSelectPlank () {
		if (isPlayingTutorial && !isUsingMenu && !hasSpawnedObject) {
			gameManager.tutorial.ObjectMenuPartThree ();
			isUsingMenu = true;
		}
	}

	private void TutorialSelectReset () {
		if (isPlayingTutorial && isUsingMenu && !hasSpawnedObject) {
			isUsingMenu = false;
		}
	}

	private void TutorialSpawn () {
		if (isPlayingTutorial && !hasSpawnedObject) {
			gameManager.tutorial.PlayerHasSpawnedObject ();
			hasSpawnedObject = true;
		}
	}
	#endregion
}
