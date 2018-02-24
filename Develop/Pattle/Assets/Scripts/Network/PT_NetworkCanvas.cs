using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using PT_Global;

public class PT_NetworkCanvas : MonoBehaviour {

	enum NetworkMenuState {
		Create,
		Search,
		Password,
	}

	private NetworkMenuState myState = NetworkMenuState.Create;
	private RectTransform myCurrentPage;

	private NetworkManager myNetworkManager;
	private NetworkDiscovery myNetworkDiscovery;
	public Dictionary<string,NetworkBroadcastResult> myBroadcastsReceived;
	private string[] mySplitStringArray = new string[1];
	[SerializeField] RectTransform myPageAll;
	[SerializeField] RectTransform myPageCreate;
	[SerializeField] InputField myPageCreate_Input_Name;
	[SerializeField] InputField myPageCreate_Input_Password;
	[SerializeField] Text myPageCreate_Info;
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
		//do not use the adventure chess
		PT_DeckManager.Instance.UseAdventureChess (false);

		myNetworkManager = NetworkManager.singleton;
		myNetworkDiscovery = 
			GameObject.Find (PT_Global.Constants.NAME_NETWORK_DISCOVERY).GetComponent<NetworkDiscovery> ();
		
		mySplitStringArray [0] = PT_Global.Constants.SYMBOL_PASSWORD;

		string t_load = ShabbySave.LoadGame (Constants.SAVE_CATEGORY_NETWORK, Constants.SAVE_TITLE_NETWORK_NAME);
		if (t_load != "0")
			myPageCreate_Input_Name.text = t_load;
		t_load = ShabbySave.LoadGame (Constants.SAVE_CATEGORY_NETWORK, Constants.SAVE_TITLE_NETWORK_PASSWORD);
		if (t_load != "0")
			myPageCreate_Input_Password.text = t_load;

		SetState (NetworkMenuState.Create);

		PT_Preset.Instance.Show ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePage ();
	}

	private void UpdatePage () {
		Vector2 t_targetPos = new Vector2 (-myCurrentPage.anchoredPosition.x, -myCurrentPage.anchoredPosition.y);
		myPageAll.anchoredPosition = Vector2.Lerp (myPageAll.anchoredPosition, t_targetPos, Time.unscaledDeltaTime * PT_Global.Constants.SPEED_UI_LERP);
	}

	public void OnButtonCreate () {
		string t_name = myPageCreate_Input_Name.text;
		if (t_name == "0" || t_name == "" || t_name.Contains (PT_Global.Constants.SYMBOL_PASSWORD)) {
//			Debug.LogError ("error: you should't use this name");
			myPageCreate_Info.text = "name is not allowed";
			return;
		}

		string t_password = myPageCreate_Input_Password.text;
		if (t_password == "0" || t_password.Contains (PT_Global.Constants.SYMBOL_PASSWORD)) {
//			Debug.LogError ("error: you should't use this password");
			myPageCreate_Info.text = "password is not allowed";
			return;
		}

		ShabbySave.SaveGame (Constants.SAVE_CATEGORY_NETWORK, Constants.SAVE_TITLE_NETWORK_NAME, t_name);
		if (t_password == "")
			ShabbySave.SaveGame (Constants.SAVE_CATEGORY_NETWORK, Constants.SAVE_TITLE_NETWORK_PASSWORD, "0");
		else
			ShabbySave.SaveGame (Constants.SAVE_CATEGORY_NETWORK, Constants.SAVE_TITLE_NETWORK_PASSWORD, t_password);

		string t_data = "0";
		if (t_password != "") {
			t_data = t_name + PT_Global.Constants.SYMBOL_PASSWORD + t_password;
		} else {
			t_data = t_name;
		}

//		myNetworkDiscovery.
		myNetworkDiscovery.Initialize ();
		myNetworkDiscovery.broadcastData = t_data;
		myNetworkDiscovery.StartAsServer ();
		TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.Host);
	}

	public void OnButtonSearch () {
		SetState (NetworkMenuState.Search);
		StartCoroutine (SearchRooms ());
	}

	public void JoinRoom (string g_ip) {
		myNetworkManager.networkAddress = g_ip;
		if (GetRoomPassword (g_ip) == "")
			TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.Client);
		else {
			myPagePassword_Info.text = "Please enter password.";
			myPagePassword_Name.text = GetRoomName (g_ip);
			string t_password = 
				ShabbySave.LoadGame (
					Constants.SAVE_CATEGORY_NETWORK, 
					Constants.SAVE_TITLE_NETWORK_JOIN_PASSWORD
				);
			if (t_password != "0")
				myPagePassword_Input.text = t_password;
			SetState (NetworkMenuState.Password);
		}
	}

	public void OnButtonJoin () {
		if (GetRoomPassword (myNetworkManager.networkAddress) == myPagePassword_Input.text) {
			ShabbySave.SaveGame (
				Constants.SAVE_CATEGORY_NETWORK, 
				Constants.SAVE_TITLE_NETWORK_JOIN_PASSWORD, 
				myPagePassword_Input.text
			);
			TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.Client);
		} else {
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
//			Debug.Log ("finding rooms");
			yield return new WaitForSeconds (1f);
		}

		if (myNetworkDiscovery.running == false) {
			myPageSearch_Info.text = "ERROR: no myNetworkDiscovery";
			yield break;
		}

		myPageSearch_Info.text = "";
		myBroadcastsReceived = myNetworkDiscovery.broadcastsReceived;

		if (myBroadcastsReceived == null) {
			myPageSearch_Info.text = "ERROR: no myBroadcastsReceived";
			yield break;
		}

		foreach (string f_ip in myBroadcastsReceived.Keys) {
			GameObject t_button = Instantiate (myPageSearch_ButtonPrefab, myPageSearch_Contents); 
			string t_name = GetRoomName (f_ip);
			t_button.GetComponent<PT_NetworkRoomButton> ().Init (this, f_ip, t_name);
		}

		myNetworkDiscovery.StopBroadcast ();

		yield return null;
	}

	public void OnButtonBack () {
		switch (myState) {
		case NetworkMenuState.Create:
			TransitionManager.Instance.StartTransition ("NetworkMenu");
			break;
		case NetworkMenuState.Search:
			if (myNetworkDiscovery.running)
				myNetworkDiscovery.StopBroadcast ();
			SetState (NetworkMenuState.Create);
			break;
		case NetworkMenuState.Password:
			SetState (NetworkMenuState.Search);
			break;
		}
	}

	private void SetState (NetworkMenuState g_state) {
		myState = g_state;
		switch (myState) {
		case NetworkMenuState.Create:
			myPageCreate_Info.text = "";
			myCurrentPage = myPageCreate;
			break;
		case NetworkMenuState.Search:
			myPageSearch_Info.text = "";
			myCurrentPage = myPageSearch;
			break;
		case NetworkMenuState.Password:
			myPageSearch_Info.text = "";
			myCurrentPage = myPagePassword;
			break;
		}
	}

	private string[] GetRoomData (string g_ip) {
		if (myBroadcastsReceived == null || myBroadcastsReceived.ContainsKey (g_ip) == false)
			return null;
		string t_data = System.Text.Encoding.Unicode.GetString (myBroadcastsReceived [g_ip].broadcastData);
		return t_data.Split (mySplitStringArray, System.StringSplitOptions.RemoveEmptyEntries);
	}

	private string GetRoomName (string g_ip) {
		string[] t_dataArray = GetRoomData (g_ip);
		if (t_dataArray != null && t_dataArray.Length > 0 && t_dataArray [0] != "")
			return t_dataArray [0];
		return "ERROR: NO ROOM NAME!";
	}

	private string GetRoomPassword (string g_ip) {
		string[] t_dataArray = GetRoomData (g_ip);
		if (t_dataArray != null && t_dataArray.Length > 1)
			return t_dataArray [1];
		return "";
	}
}
