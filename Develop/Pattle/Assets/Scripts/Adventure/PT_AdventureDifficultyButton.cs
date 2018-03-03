using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pattle.Global;

public class PT_AdventureDifficultyButton : MonoBehaviour {

	[SerializeField] BossDifficulty myDifficulty;
	[SerializeField] Image myCenterImage;

	private PT_AdventureMenuCanvas myCanvas;

	// Use this for initialization
	void Start () {
		myCanvas = PT_AdventureMenuCanvas.Instance;
	}

	public void OnButtonPressed () {
		myCanvas.OnButtonDifficulty (myDifficulty, myCenterImage.color);
	}
}
