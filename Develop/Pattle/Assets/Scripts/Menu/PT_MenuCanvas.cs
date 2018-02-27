using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PT_Global;

public class PT_MenuCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonAdventure () {
		NetworkManager.singleton.matchSize = 1;
		NetworkManager.singleton.onlineScene = Constants.SCENE_ADVENTURE;
		TransitionManager.Instance.StartTransition (Constants.SCENE_ADVENTUREMENU);
	}

	public void OnButtonArena () {
		NetworkManager.singleton.matchSize = 2;
		NetworkManager.singleton.onlineScene = Constants.SCENE_BATTLE;
		TransitionManager.Instance.StartTransition (Constants.SCENE_LOBBY);
	}
}
