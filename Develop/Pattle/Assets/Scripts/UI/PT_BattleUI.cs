using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PT_Global;

public class PT_BattleUI : PT_GameUI {
	[SerializeField] GameObject myPage_Wait;
	[SerializeField] GameObject mySet_Password;
	[SerializeField] TextMesh myText_Room;
	[SerializeField] TextMesh myText_Password;
	[SerializeField] TextMesh myText_IP;
	// Use this for initialization
	void Start () {
		myText_Room.text = ShabbySave.LoadGame (
			Constants.SAVE_CATEGORY_NETWORK, 
			Constants.SAVE_TITLE_NETWORK_NAME
		);

		string t_pwd = 
			ShabbySave.LoadGame (
				Constants.SAVE_CATEGORY_NETWORK, 
				Constants.SAVE_TITLE_NETWORK_PASSWORD
			);
		if (t_pwd == "0")
			mySet_Password.SetActive (false);
		else
			myText_Password.text = t_pwd;
		myText_IP.text = Network.player.ipAddress;
	}

	public void ShowWait () {
		myPage_Wait.SetActive (true);
	}

	public void HideWait () {
		Debug.Log ("HideWait");
		myPage_Wait.SetActive (false);
	}
}
