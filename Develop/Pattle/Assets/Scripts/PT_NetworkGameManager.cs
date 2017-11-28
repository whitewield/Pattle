using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_NetworkGameManager : NetworkBehaviour {

	private static PT_NetworkGameManager instance = null;

	[SyncVar (hook = "OnStart")] bool isStart = false;
	public PT_PlayerController[] myPlayerList = new PT_PlayerController[2];
	public List<List<GameObject>> myChessList = new List<List<GameObject>> ();
	public PT_BattleUI myBattleUI;

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

		myChessList.Add (new List<GameObject> ());
		myChessList.Add (new List<GameObject> ());
	}
	//========================================================================


	// Use this for initialization
	void Start () {
		myBattleUI.HideWait ();

		if (!isServer)
			return;
		
		Time.timeScale = 0;
		myBattleUI.ShowWait ();
//		for (int i = 0; i < myPlayerList.Length; ++i) {
//			myPlayerList [i].Init ();
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log (myChessList [0].Count + ":" + myChessList [1].Count);
			foreach (GameObject f_chess in myChessList[0]) {
				Debug.Log (f_chess);
			}

			foreach (GameObject f_chess in myChessList[1]) {
				Debug.Log (f_chess);
			}
		}

		if (!isServer)
			return;

		if (Time.timeScale == 0)
		if (myPlayerList [0] != null && myPlayerList [1] != null) {
			Time.timeScale = 1;
			myBattleUI.HideWait ();
			isStart = true;
		}
	}

	public void Quit () {
		Time.timeScale = 1;
		if (isServer)
			NetworkManager.singleton.StopHost ();
		else {
			NetworkManager.singleton.StopClient ();
			CmdQuit ();
		}
	}

	void OnStart (bool g_isStart) {
		if (!isServer)
			return;
		Debug.Log ("OnStart");
		if (g_isStart == true) {
			myPlayerList [0].RpcInit ();
			myPlayerList [1].RpcInit ();
		}
	}

	[Command]
	public void CmdQuit () {
		Debug.Log ("CmdQuit");
		Quit ();
	}

	[ClientRpc]
	public void RpcAddChessToList(int g_playerID, GameObject g_chess) {
		myChessList [g_playerID].Add (g_chess);
		g_chess.GetComponent<PT_BaseChess> ().SetMyID (myChessList [g_playerID].Count - 1);
	}
}


