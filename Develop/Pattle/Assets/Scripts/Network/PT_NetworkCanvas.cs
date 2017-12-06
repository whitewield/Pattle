using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PT_NetworkCanvas : MonoBehaviour {

	private NetworkManager myNetworkManager;
	private NetworkDiscovery myNetworkDiscovery;
	public Dictionary<string,NetworkBroadcastResult> myBroadcastsReceived;
	[SerializeField] RectTransform myPageAll;
	[SerializeField] RectTransform myPageCreate;
	[SerializeField] InputField myPageCreate_Input_Name;
	[SerializeField] InputField myPageCreate_Input_Password;
	[SerializeField] RectTransform myPageSearch;
	[SerializeField] GameObject myPageSearch_ButtonPrefab;
	[SerializeField] Transform myPageSearch_Contents;
	[SerializeField] RectTransform myPagePassword;
	// Use this for initialization
	void Start () {
		myNetworkManager = NetworkManager.singleton;
		myNetworkDiscovery = GameObject.Find (PT_Global.Constants.NAME_NETWORK_DISCOVERY).GetComponent<NetworkDiscovery> ();
		string t_name = ShabbySave.LoadGame (PT_Global.Constants.SAVE_CATEGORY_NETWORK, PT_Global.Constants.SAVE_TITLE_NETWORK_NAME);
		if (t_name != "0")
			myPageCreate_Input_Name.text = t_name;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnButtonCreate () {
		string t_name = myPageCreate_Input_Name.text;
		if (t_name == "0" || t_name == "" || t_name.Contains (PT_Global.Constants.SIGN_PASSWORD)) {
			Debug.LogError ("error: you should't use this name");
			return;
		}

		string t_password = myPageCreate_Input_Password.text;
		if (t_password.Contains (PT_Global.Constants.SIGN_PASSWORD)) {
			Debug.LogError ("error: you should't use this password");
			return;
		}

		string t_data = "0";
		if (t_password != "") {
			t_data = t_name + PT_Global.Constants.SIGN_PASSWORD + t_password;
		} else {
			t_data = t_name;
		}

		myNetworkDiscovery.Initialize ();
		myNetworkDiscovery.broadcastData = t_data;
		myNetworkDiscovery.StartAsServer ();
		myNetworkManager.StartHost ();
	}

	public void OnButtonSearch () {
		myPageAll.anchoredPosition = new Vector2 (-myPageSearch.anchoredPosition.x, myPageAll.anchoredPosition.y);
		StartCoroutine (SearchRooms ());
	}

	public void JoinRoom (string t_ip) {
		myNetworkManager.networkAddress = t_ip;
		myNetworkManager.StartClient ();
	}

	IEnumerator SearchRooms () {
		myNetworkDiscovery.Initialize ();
		myNetworkDiscovery.StartAsClient ();

		while (myNetworkDiscovery.broadcastsReceived.Count == 0) {
			Debug.Log ("finding rooms");
			yield return new WaitForSeconds (1f);
		}
		Debug.Log ("got rooms!");
		myBroadcastsReceived = myNetworkDiscovery.broadcastsReceived;

		for (int i = myPageSearch_Contents.childCount - 1; i >= 0; i--) {
			Destroy (myPageSearch_Contents.GetChild (i).gameObject);
		}

		foreach (string f_ip in myBroadcastsReceived.Keys) {
			GameObject t_button = Instantiate (myPageSearch_ButtonPrefab, myPageSearch_Contents); 
			string t_name = System.Text.Encoding.Unicode.GetString (myBroadcastsReceived [f_ip].broadcastData);
			t_button.GetComponent<PT_NetworkRoomButton> ().Init (this, f_ip, t_name);
		}


		myNetworkDiscovery.StopBroadcast ();

		yield return null;
	}

	public void OnButtonSearchBack () {
		myPageAll.anchoredPosition = new Vector2 (-myPageCreate.anchoredPosition.x, myPageAll.anchoredPosition.y);
	}

}
