using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_NetworkBattleManager : PT_NetworkGameManager {

	// Use this for initialization
	protected override void Start () {
		((PT_BattleUI)myGameUI).HideWait ();

		if (!isServer)
			return;
		
		Time.timeScale = 0;
		((PT_BattleUI)myGameUI).ShowWait ();
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
			((PT_BattleUI)myGameUI).HideWait ();
			OnStart ();
//			isStart = true;
		}
	}

	public override void Quit () {
		GameObject t_NetworkDiscoveryGameObject = GameObject.Find (PT_Global.Constants.NAME_NETWORK_DISCOVERY);
		if (t_NetworkDiscoveryGameObject != null) {
			NetworkDiscovery t_NetworkDiscovery = t_NetworkDiscoveryGameObject.GetComponent<NetworkDiscovery> ();
			if (t_NetworkDiscovery != null &&
			    t_NetworkDiscovery.running) {
				t_NetworkDiscovery.StopBroadcast ();
			}
		}

		Time.timeScale = 1;
		if (isServer) {
			NetworkManager.singleton.StopHost ();
		} else if (isClient) {
//			NetworkManager.singleton.StopClient ();
			CmdQuit ();
		}
	}

	protected override void OnStart () {
		if (!isServer)
			return;
		Debug.Log ("OnStart");
		myPlayerList [0].RpcInit ();
		myPlayerList [1].RpcInit ();

		isStart = true;
	}

	[Command]
	public override void CmdQuit () {
		Debug.Log ("CmdQuit");
		Quit ();
	}

	[ClientRpc]
	public override void RpcAddChessToList(int g_playerID, GameObject g_chess) {
		myChessList [g_playerID].Add (g_chess);
		g_chess.GetComponent<PT_BaseChess> ().SetMyID (myChessList [g_playerID].Count - 1);
	}
}


