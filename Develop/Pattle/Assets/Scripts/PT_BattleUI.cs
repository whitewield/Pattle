using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PT_Global;

public class PT_BattleUI : PT_GameUI {
	[SerializeField] GameObject myPage_Wait;
	[SerializeField] TextMesh myText_Name;
	[SerializeField] TextMesh myText_Password;
	[SerializeField] TextMesh myText_IP;
	// Use this for initialization
	void Start () {
		myText_Name.text = "Room: " +
		ShabbySave.LoadGame (
			Constants.SAVE_CATEGORY_NETWORK, 
			Constants.SAVE_TITLE_NETWORK_NAME
		);

		string t_pwd = 
			ShabbySave.LoadGame (
				Constants.SAVE_CATEGORY_NETWORK, 
				Constants.SAVE_TITLE_NETWORK_PASSWORD
			);
		if (t_pwd == "0")
			myText_Password.text = "";
		else
			myText_Password.text = "Password: " + t_pwd;
		myText_IP.text = "IP: " + Network.player.ipAddress;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowWait () {
		myPage_Wait.SetActive (true);
	}

	public void HideWait () {
		Debug.Log ("HideWait");
		myPage_Wait.SetActive (false);
	}
}
