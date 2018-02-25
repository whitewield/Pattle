using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;
using UnityEngine.UI;

public class PT_NetworkBattleCanvas : PT_BattleCanvas {

	[SerializeField] GameObject myPage_Wait;
	[SerializeField] GameObject mySet_Password;
	[SerializeField] Text myText_Room;
	[SerializeField] Text myText_Password;
	[SerializeField] Text myText_IP;

	// Use this for initialization
	void Start () {
		myText_Room.text = 
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
			mySet_Password.SetActive (false);
		else
			myText_Password.text = t_pwd;
		myText_IP.text = Network.player.ipAddress;
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
