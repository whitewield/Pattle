using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PT_Global;

public class PT_GameUI : MonoBehaviour {
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
	public void OnButtonQuit () {
		PT_NetworkGameManager.Instance.Quit ();
	}
}
