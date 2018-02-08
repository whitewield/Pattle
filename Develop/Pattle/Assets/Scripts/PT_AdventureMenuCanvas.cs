using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_AdventureMenuCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonDragon () {
		PT_DeckManager.Instance.SetAdventureBossType (PT_Global.ChessType.FireDragon);

		NetworkManager.singleton.matchSize = 1;
		NetworkManager.singleton.onlineScene = "NetworkAdventure";
		NetworkManager.singleton.StartHost ();
	}

	public void OnButtonBack () {
		TransitionManager.Instance.StartTransition ("NetworkMenu");
	}
}
