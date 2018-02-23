using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Adventure;
using UnityEngine.UI;

public class PT_AdventureButton : MonoBehaviour {

	[SerializeField] AdventureSetup mySetup;
	[SerializeField] RectTransform myButtonFace;

	private PT_AdventureMenuCanvas myCanvas;

	// Use this for initialization
	void Start () {
		myCanvas = PT_AdventureMenuCanvas.Instance;

		GameObject t_display = Instantiate (mySetup.myMenuDisplayPrefab, myButtonFace);

		GameObject t_medals = Instantiate (myCanvas.MedalsPrefab, myButtonFace);
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void OnButtonPressed () {
		myCanvas.OnButtonAdventure (mySetup);
	}
}

