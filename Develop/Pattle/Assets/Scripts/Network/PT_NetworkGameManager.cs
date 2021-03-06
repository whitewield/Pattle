﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_NetworkGameManager : NetworkBehaviour {

	protected static PT_NetworkGameManager instance = null;
	public static PT_NetworkGameManager Instance { get { return instance; } }

	protected bool isStart = false;
	public PT_PlayerController[] myPlayerList = new PT_PlayerController[2];
	public List<List<GameObject>> myChessList = new List<List<GameObject>> ();
	public PT_NetworkGameCanvas myGameCanvas;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
			//		DontDestroyOnLoad(this.gameObject);
			myChessList.Add (new List<GameObject> ());
			myChessList.Add (new List<GameObject> ());
		}
	}


	// Use this for initialization
	protected virtual void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

	protected virtual void OnStart () {
		
	}

	public List<GameObject> GetChessList (int g_ID) {
		return myChessList [g_ID];
	}

	[ClientRpc]
	public virtual void RpcAddChessToList(int g_playerID, GameObject g_chess) {
		myChessList [g_playerID].Add (g_chess);
		g_chess.GetComponent<PT_BaseChess> ().SetMyID (myChessList [g_playerID].Count - 1);
	}
}


