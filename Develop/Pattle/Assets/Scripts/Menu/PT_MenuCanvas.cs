using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;
using Hang.AiryAudio;

public class PT_MenuCanvas : MonoBehaviour {

	[SerializeField] AiryAudioData myButtonAiryAudioData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonAdventure () {
//		if (myButtonAiryAudioData != null)
//			myButtonAiryAudioData.Play ();

		NetworkManager.singleton.matchSize = 1;
		NetworkManager.singleton.onlineScene = Constants.SCENE_ADVENTURE;
		NetworkManager.singleton.offlineScene = Constants.SCENE_ENDGAME;
		TransitionManager.Instance.StartTransition (Constants.SCENE_ADVENTUREMENU);
	}

	public void OnButtonArena () {
//		if (myButtonAiryAudioData != null)
//			myButtonAiryAudioData.Play ();
		
		NetworkManager.singleton.matchSize = 2;
		NetworkManager.singleton.onlineScene = Constants.SCENE_BATTLE;
		NetworkManager.singleton.offlineScene = Constants.SCENE_ENDGAME;
		TransitionManager.Instance.StartTransition (Constants.SCENE_LOBBY);
	}

	public void OnButtonWebsite () {
		//		if (myButtonAiryAudioData != null)
		//			myButtonAiryAudioData.Play ();

		Application.OpenURL ("https://ruanhang.itch.io/pattle");
	}
}
