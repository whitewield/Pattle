using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_BattleUI : MonoBehaviour {
	[SerializeField] GameObject myPage_Wait;
	[SerializeField] TextMesh myText_IP;
	// Use this for initialization
	void Start () {
		myText_IP.text = "IP: " + Network.player.ipAddress;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonJoin () {
		PT_NetworkGameManager.Instance.Quit ();
	}

	public void ShowWait () {
		myPage_Wait.SetActive (true);
	}

	public void HideWait () {
		Debug.Log ("HideWait");
		myPage_Wait.SetActive (false);
	}
}
