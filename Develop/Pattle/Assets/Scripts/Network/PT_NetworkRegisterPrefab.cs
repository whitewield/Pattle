using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_NetworkRegisterPrefab : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GameObject[] t_networkPrefabs = Resources.LoadAll<GameObject> (Constants.PATH_NETWORK);
		foreach (GameObject f_prefab in t_networkPrefabs) {
			NetworkManager.singleton.spawnPrefabs.Add (f_prefab);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
