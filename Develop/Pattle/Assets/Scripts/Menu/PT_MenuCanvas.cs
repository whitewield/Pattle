using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_MenuCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonAdventure () {
		NetworkManager.singleton.matchSize = 1;
		TransitionManager.Instance.StartTransition ("NetworkAdventureMenu");
	}

	public void OnButtonArena () {
		NetworkManager.singleton.matchSize = 2;
	}
}
