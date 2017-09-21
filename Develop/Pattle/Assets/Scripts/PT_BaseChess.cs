using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_BaseChess : NetworkBehaviour {
	[SyncVar] int myOwnerID = -1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetMyOwnerID () {
		return myOwnerID;
	}

	public void SetMyOwnerID (int g_ID) {
		myOwnerID = g_ID;
	}

}
