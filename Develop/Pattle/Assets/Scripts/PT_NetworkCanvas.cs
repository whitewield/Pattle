using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PT_NetworkCanvas : MonoBehaviour {
	private NetworkManager myNetworkManager;
	[SerializeField] Text myText_IP;
	[SerializeField] InputField myInputField_ServerIP;
	// Use this for initialization
	void Start () {
		myNetworkManager = NetworkManager.singleton;
		myText_IP.text = "IP: " + Network.player.ipAddress;
		string t_ip = ShabbySave.LoadGame (PT_Global.Constants.SAVE_CATEGORY_PRESET, PT_Global.Constants.SAVE_TITLE_PRESET_IP);
		if (t_ip != "0")
			myInputField_ServerIP.text = t_ip;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnButtonHost () {
		myNetworkManager.StartHost ();
	}

	public void OnButtonJoin () {
		myNetworkManager.networkAddress = myInputField_ServerIP.text;
		ShabbySave.SaveGame (
			PT_Global.Constants.SAVE_CATEGORY_PRESET, 
			PT_Global.Constants.SAVE_TITLE_PRESET_IP,
			myInputField_ServerIP.text
		);
		myNetworkManager.StartClient ();
	}

}
