using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_ServerManager : NetworkBehaviour {

	[SerializeField] GameObject enemyPrefab;
	[SerializeField] int numberOfEnemies;

	public override void OnStartServer ()
	{

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}