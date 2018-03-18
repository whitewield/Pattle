using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_PlayerController : NetworkBehaviour {
	[SyncVar] int myID = -1;
//	[SerializeField] GameObject[] myChessPrefabs;
//	[SerializeField] Vector2[] myChessPositions;
//	[SyncVar] int myChessCount = 0;

	private bool wasInit = false;

	[SerializeField] GameObject mySelect;
	[SerializeField] GameObject myLine;
	private GameObject myGameObject_X;
	private GameObject myGameObject_T;
	private Vector2 myPosition_T;
	private GameObject myGameObject_Y;
	private Vector2 myPosition_Y;
	//new input
	private bool isMouseDown;
	private Vector3 myMouseDownPosition;
	private bool isMouseDrag;

	private List<PT_PlayerController_TargetDisplay> myTargetDisplays = new List<PT_PlayerController_TargetDisplay> ();
	private GameObject myTargetSignPrefab;
	private GameObject myTargetLinePrefab;

	void Awake () {
		myTargetSignPrefab = Resources.Load<GameObject> (Constants.PATH_TARGET_SIGN);
		myTargetLinePrefab = Resources.Load<GameObject> (Constants.PATH_TARGET_LINE);

		if (PT_NetworkGameManager.Instance.myPlayerList [0] != null && 
			PT_NetworkGameManager.Instance.myPlayerList [0].enabled == true) {
			PT_NetworkGameManager.Instance.myPlayerList [1] = this;
		} else {
			PT_NetworkGameManager.Instance.myPlayerList [0] = this;
		}
//		ClientScene.RegisterPrefab
	}

	// Use this for initialization
	void Start () {
		// register on network game manager
		if (!base.isLocalPlayer) {
			return;
		}
			
		mySelect = Instantiate (mySelect, Vector3.zero, Quaternion.identity) as GameObject;
		mySelect.transform.SetParent (this.transform);
		myLine = Instantiate (myLine) as GameObject;
		myLine.transform.SetParent (this.transform);
		HideSelect ();
		HideLine ();
		
//		if (PT_NetworkGameManager.Instance != null) {
//			//we MAY be awake late (see comment on _wasInit above), so if the instance is already there we init
//			Init();
//		}
	}

	public void Init () {

		//		Debug.Log (GetInstanceID ());
		if (System.Array.IndexOf (PT_NetworkGameManager.Instance.myPlayerList, this) == -1) {
			Invoke ("Init", 1);
			Debug.Log ("Invoke");
			return;
		}


		//rotate the camera
//		Debug.Log ("do" + System.Array.IndexOf (PT_NetworkGameManager.myPlayerList, this));
		if (System.Array.IndexOf (PT_NetworkGameManager.Instance.myPlayerList, this) == 1) {
			Camera.main.transform.rotation = Quaternion.Euler (0, 0, 180);
		} else {
			Camera.main.transform.rotation = Quaternion.Euler (0, 0, 0);
		}

		if (wasInit)
			return;

		wasInit = true;

		// init target display list
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			myTargetDisplays.Add (
				new PT_PlayerController_TargetDisplay (
					"(" + i.ToString () + ")",
					Instantiate (myTargetSignPrefab, this.transform),
					Instantiate (myTargetLinePrefab, this.transform)
				)
			);
		}

		CmdCreateChess (PT_DeckManager.Instance.GetChessTypes (), PT_DeckManager.Instance.GetChessPositions ());

		//		Debug.Log (GetInstanceID ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!base.isLocalPlayer)
			return;

		foreach (PT_PlayerController_TargetDisplay f_targetDisplay in myTargetDisplays) {
			f_targetDisplay.UpdateTarget ();
		}

		if (myGameObject_X != null)
			mySelect.transform.position = myGameObject_X.transform.position;

		if (Input.GetMouseButtonDown (0)) {
			isMouseDown = true;
			//			Debug.Log ("MouseDown" + Input.mousePosition);

			myMouseDownPosition = Input.mousePosition;

			Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D t_hit = Physics2D.GetRayIntersection (t_ray, 100, 1024 + 2048);
			if (t_hit.collider != null) {
				//				Debug.Log (t_hit.transform.name);

				myGameObject_T = t_hit.collider.gameObject;
				myPosition_T = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			} else
				isMouseDown = false;
		}

		//Debug.Log (Vector3.SqrMagnitude (myMouseDownPosition - Input.mousePosition));
		PT_BaseChess t_BaseChess_T = null;
		if (myGameObject_T && myGameObject_T.GetComponent<PT_BaseChess> ())
			t_BaseChess_T = myGameObject_T.GetComponent<PT_BaseChess> ();

		if (isMouseDown == true && 
			t_BaseChess_T != null && 
			t_BaseChess_T.GetProcess() != Process.Dead &&
			t_BaseChess_T.GetMyOwnerID() == myID &&
			Vector3.SqrMagnitude (myMouseDownPosition - Input.mousePosition) > Constants.DISTANCE_DRAG) {
			isMouseDrag = true;

			ShowLine ();
		}

		if (isMouseDrag == true) {
			UpdateLine (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		}

		if (Input.GetMouseButtonUp (0)) {

			HideLine ();

			isMouseDown = false;

			Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D t_hit = Physics2D.GetRayIntersection (t_ray, 100, 1024 + 2048);
			GameObject t_gameObject;
			if (t_hit.collider != null) {
				t_gameObject = t_hit.collider.gameObject;

				if (isMouseDrag == true) {
					isMouseDrag = false;

					myGameObject_X = myGameObject_T;

					myGameObject_Y = t_gameObject;
					myPosition_Y = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					//Send Message to chess X
					CmdChessAction (myGameObject_X, myGameObject_Y, myPosition_Y);
				} else {
					if (myGameObject_X == null) {
						if (t_BaseChess_T != null &&
						   t_BaseChess_T.GetProcess () != Process.Dead &&
						   t_BaseChess_T.GetMyOwnerID () == myID) {
							myGameObject_X = myGameObject_T;

							ShowSelect ();
						}
					}
					else {
						myGameObject_Y = myGameObject_T;
						myPosition_Y = myPosition_T;
						//Send Message to chess X
						CmdChessAction (myGameObject_X, myGameObject_Y, myPosition_Y);
					}
				}
			}



			//			Debug.Log ("MouseDown" + Input.mousePosition);
		}
	}

	public void Done () {
		myGameObject_X = null;
		myGameObject_Y = null;
		myPosition_Y = Vector2.zero;

		HideSelect ();
	}

	public void Undone () {
		if (myGameObject_Y.GetComponent<PT_BaseChess>()!= null && 
			myGameObject_Y.GetComponent<PT_BaseChess>().GetMyOwnerID() == myID) {
			myGameObject_X = myGameObject_Y;
			myGameObject_Y = null;
			myPosition_Y = Vector2.zero;

			ShowSelect();
		} else
			Done ();
	}

	private void ShowSelect () {
		mySelect.transform.position = myGameObject_X.transform.position;
		mySelect.SetActive (true);
	}

	private void HideSelect () {
		mySelect.SetActive (false);
	}

	private void ShowLine () {
		myLine.GetComponent<LineRenderer> ().SetPosition (0, myGameObject_T.transform.position);
		myLine.GetComponent<LineRenderer> ().SetPosition (1, myPosition_T);
		myLine.SetActive (true);
	}

	private void UpdateLine (Vector3 t_pos) {
		myLine.GetComponent<LineRenderer> ().SetPosition (0, myGameObject_T.transform.position);
		myLine.GetComponent<LineRenderer> ().SetPosition (1, new Vector3(t_pos.x, t_pos.y, 0));
	}

	private void HideLine () {
		myLine.SetActive (false);
	}

	public void CheckLose () {
		List<GameObject> t_chessList = PT_NetworkGameManager.Instance.GetChessList (myID);
		Debug.Log (t_chessList.Count);
		for (int i = 0; i < t_chessList.Count; i++) {
			if (t_chessList [i].GetComponent<PT_BaseChess> ().GetProcess () != Process.Dead) {
				return;
			}
		}

		RpcLose ();
	}

	public void Lose () {
		if (isLocalPlayer)
			CmdLose ();
	}
		
	[Command]
	public void CmdCreateChess (ChessType[] g_chessTypes, Vector2[] g_positions) {
		myID = System.Array.IndexOf (PT_NetworkGameManager.Instance.myPlayerList, this);
//		Debug.Log (myID);

		if (myID == -1) {
			Invoke ("CmdCreateChess", 1);
			return;
		}

		for(int i = 0; i < g_chessTypes.Length; i++) {
			// create server-side instance

			GameObject t_chessObject = (GameObject)Instantiate (
				                           PT_DeckManager.Instance.myChessBank.GetChessPrefab (g_chessTypes [i]), 
				                           Random.insideUnitCircle * 4, 
				                           Quaternion.identity
			                           );
			PT_BaseChess t_chess = t_chessObject.GetComponent<PT_BaseChess> ();

			//add the chess to the network game manager
//			PT_NetworkGameManager.myChessList [myID].Add (t_chess);

			//set the player id to the chess
			t_chess.SetMyOwnerID (myID);
			t_chess.SetMyPlayerController (this);

//			myChessCount++;

			t_chess.transform.position = g_positions [i];

			if (myID == 1) {
				t_chess.transform.position *= -1;
			}

			//test
//			if(myID == 0)t_chessObject.GetComponent<SpriteRenderer>().color = Color.red;
//			t_chess.Initialize();

			// spawn on the clients
			NetworkServer.Spawn (t_chessObject);

			PT_NetworkGameManager.Instance.RpcAddChessToList (myID, t_chessObject);

		}
	}


	[Command]
	public void CmdChessAction (GameObject g_active, GameObject g_target, Vector2 g_targetPos) {
		if (g_active.GetComponent<PT_BaseChess> () && g_active.GetComponent<PT_BaseChess> ().Action (g_target, g_targetPos))
			RpcDone ();
		else 
			RpcUndone ();
	}

	[Command]
	public void CmdCheckLose () {
		List<GameObject> t_chessList = PT_NetworkGameManager.Instance.GetChessList (myID);
		Debug.Log (t_chessList.Count);
		for (int i = 0; i < t_chessList.Count; i++) {
			if (t_chessList [i].GetComponent<PT_BaseChess> ().GetProcess () != Process.Dead) {
				return;
			}
		}

		RpcLose ();
	}

	[Command]
	public void CmdLose () {
		RpcLose ();
	}

	[ClientRpc]
	void RpcLose () {
		GameObject t_NetworkDiscoveryGameObject = GameObject.Find (Constants.NAME_NETWORK_DISCOVERY);
		if (t_NetworkDiscoveryGameObject != null) {
			NetworkDiscovery t_NetworkDiscovery = t_NetworkDiscoveryGameObject.GetComponent<NetworkDiscovery> ();
			if (t_NetworkDiscovery != null &&
				t_NetworkDiscovery.running) {
				t_NetworkDiscovery.StopBroadcast ();
			}
		}

		Time.timeScale = 1;

		if (isLocalPlayer) {
			PT_DeckManager.Instance.IsWinning = false;
		} else {
			PT_DeckManager.Instance.IsWinning = true;
		}

		if (isServer) {
			TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.StopHost);
		} else {
			TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.StopClient);
		}
	}

	[ClientRpc]
	void RpcDone () {
		if (isLocalPlayer) {
			Done ();
		}
	}

	[ClientRpc]
	void RpcUndone () {
		if (isLocalPlayer) {
			Undone ();
		}
	}

	[ClientRpc]
	public void RpcShowTarget (int g_ID, int g_targetOwnerID, int g_targetID, Vector2 g_TargetPosition) {
		if (!isLocalPlayer)
			return;

//		Debug.Log (g_ID + ", " + g_targetOwnerID + ", " + g_targetID + ", " + g_TargetPosition);

		myTargetDisplays [g_ID].SetOwner (myID, g_ID);

		if (g_targetOwnerID == -1 || g_targetID == -1) {
			myTargetDisplays [g_ID].ShowTarget (g_TargetPosition);
		} else
			myTargetDisplays [g_ID].ShowTarget (g_targetOwnerID, g_targetID);
	}

	[ClientRpc]
	public void RpcHideTarget (int g_ID) {
		if (!isLocalPlayer)
			return;

		myTargetDisplays [g_ID].HideTarget ();
	}

	[ClientRpc]
	public void RpcInit () {
		if (isLocalPlayer) {
			Init ();
		}
	}
}

public class PT_PlayerController_TargetDisplay {
	private GameObject myChess;
	private GameObject myTargetSign;
	private GameObject myTargetLine;

	private GameObject myTargetGameObject;
	private bool myTargetIsSingle = false;
	private Vector2 myTargetPosition;

	public PT_PlayerController_TargetDisplay (string g_name, GameObject g_sign, GameObject g_line) {
		myTargetSign = g_sign;
		myTargetSign.name = "TargetSign_" + g_name;
		myTargetSign.SetActive (false);

		myTargetLine = g_line;
		myTargetLine.name = "TargetLine_" + g_name;
		myTargetLine.SetActive (false);
	}

	public void SetOwner (int g_chessOwnerID, int g_chessID) {
		if (myChess == null) {
			myChess = PT_NetworkGameManager.Instance.myChessList [g_chessOwnerID] [g_chessID];
		}
	}

	public void ShowTarget (int g_targetOwnerID, int g_targetID) {
		myTargetIsSingle = true;
		myTargetGameObject = PT_NetworkGameManager.Instance.myChessList [g_targetOwnerID] [g_targetID];

		myTargetSign.SetActive (true);
		myTargetLine.SetActive (true);

		UpdateTarget ();
	}

	public void ShowTarget (Vector2 g_TargetPosition) {
		myTargetIsSingle = false;
		myTargetPosition = g_TargetPosition;

		myTargetSign.SetActive (true);
		myTargetLine.SetActive (true);

		UpdateTarget ();
	}

	public void UpdateTarget () {
		if (myTargetLine == null || myTargetSign == null)
			return;

		if (myTargetLine.activeSelf == false || myTargetSign.activeSelf == false)
			return;

//		Debug.Log ("UpdateTarget");

		if (myTargetIsSingle) {
			myTargetSign.transform.position = myTargetGameObject.transform.position;
		} else
			myTargetSign.transform.position = myTargetPosition;
		myTargetLine.GetComponent<LineRenderer> ().SetPosition (0, myChess.transform.position);
		myTargetLine.GetComponent<LineRenderer> ().SetPosition (1, myTargetSign.transform.position);
	}

	public void HideTarget () {
		myTargetLine.SetActive (false);
		myTargetSign.SetActive (false);
	}
}
