using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {

	#region Properties
	// Level
	public int number = 1;

	// Contraption Components
	public GameObject fanPrefab;
	public int numberOfFans = 0;
	public List<GameObject> fans;
	private int fansUsed = 0;

	public GameObject metalPlankPrefab;
	public int numberOfMetalPlanks = 0;
	public List<GameObject> metalPlanks;
	private int metalPlanksUsed = 0;

	public GameObject trampolinePrefab;
	public int numberOfTrampolines = 0;
	public List<GameObject> trampolines;
	private int trampolinesUsed = 0;

	public GameObject woodenPlankPrefab;
	public int numberOfWoodenPlanks = 0;
	public List<GameObject> woodenPlanks;
	private int woodenPlanksUsed = 0;

	// Collectables
	public List<Star> stars;
	public int starsCollected = 0;
	#endregion

	#region Computed Properties
	public int MetalPlanksAvailable {
		get { return numberOfMetalPlanks - metalPlanksUsed; }
	}

	public int WoodenPlanksAvailable {
		get { return numberOfWoodenPlanks - woodenPlanksUsed; }
	}

	public int TrampolinesAvailable {
		get { return numberOfTrampolines - trampolinesUsed; }
	}

	public int FansAvailable {
		get { return numberOfFans - fansUsed; }
	}
	#endregion

	#region Initialization
	void Start () {
		// Fill Fan Pool
		for (int i = 0; i < numberOfFans; i++) {
			GameObject fan = Instantiate (fanPrefab);
			fan.SetActive (false);
			fans.Add (fan);
		}

		// Fill Metal Plank Pool
		for (int i = 0; i < numberOfMetalPlanks; i++) {
			GameObject metalPlank = Instantiate (metalPlankPrefab);
			metalPlank.SetActive (false);
			metalPlanks.Add (metalPlank);
		}

		// Fill Trampoline Pool
		for (int i = 0; i < numberOfTrampolines; i++) {
			GameObject trampoline = Instantiate (trampolinePrefab);
			trampoline.SetActive (false);
			trampolines.Add (trampoline);
		}

		// Fill Wooden Plank Pool
		for (int i = 0; i < numberOfWoodenPlanks; i++) {
			GameObject woodenPlank = Instantiate (woodenPlankPrefab);
			woodenPlank.SetActive (false);
			woodenPlanks.Add (woodenPlank);
		}
	}
	#endregion

	#region Contraption Components
	public GameObject UseFan () {
		if (numberOfFans > 0 && fansUsed < numberOfFans) {
			GameObject fan = fans [fansUsed];
			fan.SetActive (true);
			fansUsed++;
			return fan;
		}
		return null;
	}

	public GameObject UseMetalPlank () {
		if (numberOfMetalPlanks > 0 && metalPlanksUsed < numberOfMetalPlanks) {
			GameObject metalPlank = metalPlanks [metalPlanksUsed];
			metalPlank.SetActive (true);
			metalPlanksUsed++;
			return metalPlank;
		}
		return null;
	}

	public GameObject UseTrampoline () {
		if (numberOfTrampolines > 0 && trampolinesUsed < numberOfTrampolines) {
			GameObject trampoline = trampolines [trampolinesUsed];
			trampoline.SetActive (true);
			trampolinesUsed++;
			return trampoline;
		}
		return null;
	}

	public GameObject UseWoodenPlank () {
		if (numberOfWoodenPlanks > 0 && woodenPlanksUsed < numberOfWoodenPlanks) {
			GameObject woodenPlank = woodenPlanks [woodenPlanksUsed];
			woodenPlank.SetActive (true);
			woodenPlanksUsed++;
			return woodenPlank;
		}
		return null;
	}
	#endregion

	#region Collectibles
	public void CollectStar () {
		starsCollected++;
	}

	public void ResetStars () {
		foreach (Star star in stars) {
			star.gameObject.SetActive (true);
			starsCollected = 0;
		}
	}
	#endregion
}
