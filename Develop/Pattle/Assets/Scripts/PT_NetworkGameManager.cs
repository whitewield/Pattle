using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_NetworkGameManager : NetworkBehaviour {

	private static PT_NetworkGameManager instance = null;

	//========================================================================
	public static PT_NetworkGameManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	public PT_PlayerController[] myPlayerList = new PT_PlayerController[2];
	public PT_BaseChess[,] myChessList = new PT_BaseChess[2, 3];

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;
		
		Time.timeScale = 0;
//		for (int i = 0; i < myPlayerList.Length; ++i) {
//			myPlayerList [i].Init ();
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

		if (Time.timeScale == 0)
		if (myPlayerList [0] != null && myPlayerList [1] != null) {
			Time.timeScale = 1;
		}
	}
}
