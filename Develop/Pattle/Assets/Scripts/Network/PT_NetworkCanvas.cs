using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PT_NetworkCanvas : MonoBehaviour {

	private NetworkManager myNetworkManager;
	private NetworkDiscovery myNetworkDiscovery;
	public Dictionary<string,NetworkBroadcastResult> myBroadcastsReceived;
	private string[] mySplitStringArray = new string[1];
	[SerializeField] RectTransform myPageAll;
	[SerializeField] RectTransform myPageCreate;
	[SerializeField] InputField myPageCreate_Input_Name;
	[SerializeField] InputField myPageCreate_Input_Password;
	[SerializeField] RectTransform myPageSearch;
	[SerializeField] Text myPageSearch_Info;
	[SerializeField] GameObject myPageSearch_ButtonPrefab;
	[SerializeField] Transform myPageSearch_Contents;
	[SerializeField] RectTransform myPagePassword;
	[SerializeField] InputField myPagePassword_Input;
	[SerializeField] Text myPagePassword_Name;
	[SerializeField] Text myPagePassword_Info;
	// Use this for initialization
	void Start () {
		myNetworkManager = NetworkManager.singleton;
		myNetworkDiscovery = GameObject.Find (PT_Global.Constants.NAME_NETWORK_DISCOVERY).GetComponent<NetworkDiscovery> ();
		string t_name = ShabbySave.LoadGame (PT_Global.Constants.SAVE_CATEGORY_NETWORK, PT_Global.Constants.SAVE_TITLE_NETWORK_NAME);
		if (t_name != "0")
			myPageCreate_Input_Name.text = t_name;
		mySplitStringArray [0] = PT_Global.Constants.SYMBOL_PASSWORD;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnButtonCreate () {
		string t_name = myPageCreate_Input_Name.text;
		if (t_name == "0" || t_name == "" || t_name.Contains (PT_Global.Constants.SYMBOL_PASSWORD)) {
			Debug.LogError ("error: you should't use this name");
			return;
		}

		string t_password = myPageCreate_Input_Password.text;
		if (t_password.Contains (PT_Global.Constants.SYMBOL_PASSWORD)) {
			Debug.LogError ("error: you should't use this password");
			return;
		}

		string t_data = "0";
		if (t_password != "") {
			t_data = t_name + PT_Global.Constants.SYMBOL_PASSWORD + t_password;
		} else {
			t_data = t_name;
		}

		myNetworkDiscovery.Initialize ();
		myNetworkDiscovery.broadcastData = t_data;
		myNetworkDiscovery.StartAsServer ();
		myNetworkManager.StartHost ();
	}

	public void OnButtonSearch () {
		ShowPage (myPageSearch);
		StartCoroutine (SearchRooms ());
	}

	public void JoinRoom (string g_ip) {
		myNetworkManager.networkAddress = g_ip;
		if (GetRoomPassword (g_ip) == "")
			myNetworkManager.StartClient ();
		else {
			myPagePassword_Info.text = "Please enter password.";
			myPagePassword_Name.text = GetRoomName (g_ip);
			ShowPage (myPagePassword);
		}
	}

	public void OnButtonJoin () {
		if (GetRoomPassword (myNetworkManager.networkAddress) == myPagePassword_Input.text)
			myNetworkManager.StartClient ();
		else {
			myPagePassword_Info.text = "Wrong password!";
		}
	}

	IEnumerator SearchRooms () {
		for (int i = myPageSearch_Contents.childCount - 1; i >= 0; i--) {
			Destroy (myPageSearch_Contents.GetChild (i).gameObject);
		}

		myPageSearch_Info.text = "Searching...";

		myNetworkDiscovery.Initialize ();
		myNetworkDiscovery.StartAsClient ();

		while (myNetworkDiscovery.running && myNetworkDiscovery.broadcastsReceived.Count == 0) {
			Debug.Log ("finding rooms");
			yield return new WaitForSeconds (1f);
		}

		if (myNetworkDiscovery.running == false) {
			yield return null;
		}

		myPageSearch_Info.text = "";
		myBroadcastsReceived = myNetworkDiscovery.broadcastsReceived;

		if (myBroadcastsReceived == null) {
			yield return null;
		}

		foreach (string f_ip in myBroadcastsReceived.Keys) {
			GameObject t_button = Instantiate (myPageSearch_ButtonPrefab, myPageSearch_Contents); 
			string t_name = GetRoomName (f_ip);
			t_button.GetComponent<PT_NetworkRoomButton> ().Init (this, f_ip, t_name);
		}

		myNetworkDiscovery.StopBroadcast ();

		yield return null;
	}

	public void OnButtonSearchBack () {
		if (myNetworkDiscovery.running)
			myNetworkDiscovery.StopBroadcast ();
		ShowPage (myPageCreate);
	}

	public void OnButtonPasswordBack () {
		ShowPage (myPageSearch);
	}

	private void ShowPage (RectTransform g_page) {
		myPageAll.anchoredPosition = new Vector2 (-g_page.anchoredPosition.x, myPageAll.anchoredPosition.y);
	}

	private string[] GetRoomData (string g_ip) {
		string t_data = System.Text.Encoding.Unicode.GetString (myBroadcastsReceived [g_ip].broadcastData);
		return t_data.Split (mySplitStringArray, System.StringSplitOptions.RemoveEmptyEntries);
	}

	private string GetRoomName (string g_ip) {
		string[] t_dataArray = GetRoomData (g_ip);
		if (t_dataArray.Length > 0 && t_dataArray [0] != "")
			return t_dataArray [0];
		return "ERROR: NO ROOM NAME!";
	}

	private string GetRoomPassword (string g_ip) {
		string[] t_dataArray = GetRoomData (g_ip);
		if (t_dataArray.Length > 1)
			return t_dataArray [1];
		return "";
	}
}
