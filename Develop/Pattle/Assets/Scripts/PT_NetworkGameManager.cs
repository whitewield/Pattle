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
		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	public static PT_PlayerController[] myPlayerList = new PT_PlayerController[2];
	public static PT_BaseChess[,] myChessList = new PT_BaseChess[2, 3];

	// Use this for initialization
	void Start () {
//		for (int i = 0; i < myPlayerList.Length; ++i) {
//			myPlayerList [i].Init ();
//		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
