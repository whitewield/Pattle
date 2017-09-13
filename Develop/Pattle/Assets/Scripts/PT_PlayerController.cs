using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_PlayerController : NetworkBehaviour {
	[SerializeField] GameObject myChessPrefab;
	[SyncVar] int myChessCount = 0;
	int[] numbers = new int[5];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!base.isLocalPlayer)
			return;

		if (Input.GetKeyDown (KeyCode.Space))
			Cmd_CreateChess ();
	}

	[Command]
	public void Cmd_CreateChess()
	{
		// create server-side instance

		GameObject t_chess = (GameObject)Instantiate (myChessPrefab, Random.insideUnitCircle * 4, Quaternion.identity);

		myChessCount++;

		// spawn on the clients

		NetworkServer.Spawn (t_chess);
	}
}
