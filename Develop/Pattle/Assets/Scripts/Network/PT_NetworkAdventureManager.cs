using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_NetworkAdventureManager : PT_NetworkGameManager {

	// Use this for initialization
	protected override void Start () {

	}
	
	// Update is called once per frame
	protected override void Update () {
		if (!isServer)
			return;

		if (isStart == false)
		if (myPlayerList [0] != null) {
			OnStart ();
		}
	}

	protected override void OnStart () {
		if (!isServer)
			return;
		Debug.Log ("OnStart");

		myPlayerList [0].RpcInit ();

		CreateBoss ();

		isStart = true;
	}

	private void CreateBoss () {
		//create boss
		GameObject t_bossObject = Instantiate (PT_DeckManager.Instance.GetAdventureBossPrefab(), this.transform) as GameObject;

		PT_BaseBoss t_boss = t_bossObject.GetComponent<PT_BaseBoss> ();

		//add the chess to the network game manager
		//			PT_NetworkGameManager.myChessList [myID].Add (t_chess);

		//set the boss id to the chess
		t_boss.SetMyOwnerID (1);

		t_boss.InitPosition ();

		t_boss.SetMyManager (this);

		// spawn on the clients
		NetworkServer.Spawn (t_bossObject);

		PT_NetworkGameManager.Instance.RpcAddChessToList (1, t_bossObject);
	}

	public List<GameObject> GetPlayerChessList () {
		return myChessList [0];
	}

	public void CheckBossLose () {
		List<GameObject> t_chessList = PT_NetworkGameManager.Instance.GetChessList (1);
		Debug.Log (t_chessList.Count);
		for (int i = 0; i < t_chessList.Count; i++) {
			if (t_chessList [i].GetComponent<PT_BaseChess> ().GetProcess () != PT_Global.Process.Dead) {
				return;
			}
		}
		BossLose ();
	}

	private void BossLose () {
		Time.timeScale = 1;

		PT_DeckManager.Instance.IsWinning = true;
			
		TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.StopHost);
	}

	[ClientRpc]
	public override void RpcAddChessToList(int g_playerID, GameObject g_chess) {
		myChessList [g_playerID].Add (g_chess);
		g_chess.GetComponent<PT_BaseChess> ().SetMyID (myChessList [g_playerID].Count - 1);
	}


}


