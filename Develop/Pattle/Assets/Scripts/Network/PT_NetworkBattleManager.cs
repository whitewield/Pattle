using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PT_Global;

public class PT_NetworkBattleManager : PT_NetworkGameManager {

	// Use this for initialization
	protected override void Start () {
		((PT_NetworkBattleCanvas)myGameCanvas).HideWait ();

		if (!isServer)
			return;
		
		Time.timeScale = 0;
		((PT_NetworkBattleCanvas)myGameCanvas).ShowWait ();
//		for (int i = 0; i < myPlayerList.Length; ++i) {
//			myPlayerList [i].Init ();
//		}
	}
	
	// Update is called once per frame
	protected override void Update () {
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			Debug.Log (myChessList [0].Count + ":" + myChessList [1].Count);
//			foreach (GameObject f_chess in myChessList[0]) {
//				Debug.Log (f_chess);
//			}
//
//			foreach (GameObject f_chess in myChessList[1]) {
//				Debug.Log (f_chess);
//			}
//		}

		if (!isServer)
			return;

		if (Time.timeScale == 0 || isStart == false)
		if (myPlayerList [0] != null && myPlayerList [1] != null) {
			Time.timeScale = 1;
			((PT_NetworkBattleCanvas)myGameCanvas).HideWait ();
			OnStart ();
//			isStart = true;
		}
	}

	protected override void OnStart () {
		if (!isServer) {
			return;
		}
		Debug.Log ("OnStart");
		myPlayerList [0].RpcInit ();
		myPlayerList [1].RpcInit ();

		isStart = true;
	}


	[ClientRpc]
	public override void RpcAddChessToList(int g_playerID, GameObject g_chess) {
		myChessList [g_playerID].Add (g_chess);
		g_chess.GetComponent<PT_BaseChess> ().SetMyID (myChessList [g_playerID].Count - 1);
	}
}


