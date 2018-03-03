using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_GameUI : MonoBehaviour {
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
	public void OnButtonQuit () {
		PT_PlayerController[] t_players = PT_NetworkGameManager.Instance.myPlayerList;
		for (int i = 0; i < t_players.Length; i++) {
			if (t_players [i] != null) {
				t_players [i].Lose ();
			}
		}
	}
}
