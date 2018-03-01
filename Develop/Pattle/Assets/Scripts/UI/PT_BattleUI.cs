using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using PT_Global;

public class PT_BattleUI : PT_GameUI {
	[SerializeField] GameObject myPage_Wait;
	[SerializeField] GameObject mySet_Password;
	[SerializeField] Text myText_Room;
	[SerializeField] Text myText_Password;
	[SerializeField] Text myText_IP;

	[SerializeField] GameObject myButton_Surrender;
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
		Debug.Log ("ShowWait");
		myButton_Surrender.SetActive (false);
		myPage_Wait.SetActive (true);
	}

	public void HideWait () {
		Debug.Log ("HideWait");
		myButton_Surrender.SetActive (true);
		myPage_Wait.SetActive (false);
	}

	public void OnButtonCancelWait () {
		GameObject t_NetworkDiscoveryGameObject = GameObject.Find (PT_Global.Constants.NAME_NETWORK_DISCOVERY);
		if (t_NetworkDiscoveryGameObject != null) {
			NetworkDiscovery t_NetworkDiscovery = t_NetworkDiscoveryGameObject.GetComponent<NetworkDiscovery> ();
			if (t_NetworkDiscovery != null &&
				t_NetworkDiscovery.running) {
				t_NetworkDiscovery.StopBroadcast ();
			}
		}

		NetworkManager.singleton.offlineScene = Constants.SCENE_LOBBY;
		TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.StopHost);
	}
}
