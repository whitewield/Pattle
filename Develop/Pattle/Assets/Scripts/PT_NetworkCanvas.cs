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
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnButtonHost () {
		myNetworkManager.StartHost ();
	}

	public void OnButtonJoin () {
		myNetworkManager.networkAddress = myInputField_ServerIP.text;
		myNetworkManager.StartClient ();
	}

}
