using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_PlayerController : NetworkBehaviour {
	// the id of this controller, used for networking
	[SyncVar] int myID = -1;

	private bool wasInit = false;

	// control feed back prefabs
	[SerializeField] GameObject mySelect;
	[SerializeField] GameObject myLine;

	// the targets records
	private GameObject myGameObject_X;
	private GameObject myGameObject_T;
	private Vector2 myPosition_T;
	private GameObject myGameObject_Y;
	private Vector2 myPosition_Y;

	// input
	private bool isMouseDown;
	private Vector3 myMouseDownPosition;
	private bool isMouseDrag;

	// target display for controlling
	private List<PT_PlayerController_TargetDisplay> myTargetDisplays = new List<PT_PlayerController_TargetDisplay> ();
	private GameObject myTargetSignPrefab;
	private GameObject myTargetLinePrefab;

	void Awake () {
		// find basic sign and line prefabs for target display
		myTargetSignPrefab = Resources.Load<GameObject> (Constants.PATH_TARGET_SIGN);
		myTargetLinePrefab = Resources.Load<GameObject> (Constants.PATH_TARGET_LINE);

		// register on network game manager
		if (PT_NetworkGameManager.Instance.myPlayerList[0] != null &&
			PT_NetworkGameManager.Instance.myPlayerList[0].enabled == true) {
			PT_NetworkGameManager.Instance.myPlayerList[1] = this;
		} else {
			PT_NetworkGameManager.Instance.myPlayerList[0] = this;
		}
	}

	void Start () {

		if (!base.isLocalPlayer) {
			return;
		}

		// create a selected effect
		mySelect = Instantiate (mySelect, Vector3.zero, Quaternion.identity) as GameObject;
		mySelect.transform.SetParent (this.transform);
		// create a drag line effect
		myLine = Instantiate (myLine) as GameObject;
		myLine.transform.SetParent (this.transform);
		// hide effects
		HideSelect ();
		HideLine ();

		//		if (PT_NetworkGameManager.Instance != null) {
		//			//we MAY be awake late (see comment on _wasInit above), so if the instance is already there we init
		//			Init();
		//		}
	}

	/// <summary>
	/// Init player control
	/// </summary>
	public void Init () {
		// if this player controller is not registered on the network game manager, do the initialization 1 sec later
		if (System.Array.IndexOf (PT_NetworkGameManager.Instance.myPlayerList, this) == -1) {
			Invoke ("Init", 1);
			Debug.Log ("Invoke");
			return;
		}

		// if it's the second player, rotate the camera for this player
		if (System.Array.IndexOf (PT_NetworkGameManager.Instance.myPlayerList, this) == 1) {
			Camera.main.transform.rotation = Quaternion.Euler (0, 0, 180);
		} else {
			Camera.main.transform.rotation = Quaternion.Euler (0, 0, 0);
		}

		// if it's already init, return
		if (wasInit)
			return;

		// set init to true
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

		// create chess
		CmdCreateChess (PT_DeckManager.Instance.GetChessTypes (), PT_DeckManager.Instance.GetChessPositions ());

	}

	void Update () {
		// only local player will do the update
		if (!base.isLocalPlayer)
			return;

		// update the target display
		foreach (PT_PlayerController_TargetDisplay f_targetDisplay in myTargetDisplays) {
			f_targetDisplay.UpdateTarget ();
		}

		// if its selecting an object, set the select effect poisiton to the object
		if (myGameObject_X != null)
			mySelect.transform.position = myGameObject_X.transform.position;

		// if mouse down
		if (Input.GetMouseButtonDown (0)) {
			// set isMouseDown to true
			isMouseDown = true;
			// set the myMouseDownPosition to the mouse position
			myMouseDownPosition = Input.mousePosition;
			// do a raycast to the currently clicking position
			Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D t_hit = Physics2D.GetRayIntersection (t_ray, 100, 1024 + 2048);
			if (t_hit.collider != null) {
				// if hit something, update the temp gameObject and position
				myGameObject_T = t_hit.collider.gameObject;
				myPosition_T = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			} else {
				// set isMouseDown back to false if didn't click on anything
				isMouseDown = false;
			}
		}

		// get the chess script from the temp game object
		PT_BaseChess t_BaseChess_T = null;
		if (myGameObject_T && myGameObject_T.GetComponent<PT_BaseChess> ())
			t_BaseChess_T = myGameObject_T.GetComponent<PT_BaseChess> ();

		// if mouse down, click on my own chess and it's not dead, and mouse moving distance is far enough
		if (isMouseDown == true &&
			t_BaseChess_T != null &&
			t_BaseChess_T.GetProcess () != Process.Dead &&
			t_BaseChess_T.GetMyOwnerID () == myID &&
			Vector3.SqrMagnitude (myMouseDownPosition - Input.mousePosition) > Constants.DISTANCE_DRAG) {
			// set dragging to true
			isMouseDrag = true;
			// show the dragging line display
			ShowLine ();
		}

		// if is dragging
		if (isMouseDrag == true) {
			// udpate the line display
			UpdateLine (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		}

		// if mouse up
		if (Input.GetMouseButtonUp (0)) {
			// hide the line drag display
			HideLine ();
			// set mouse down
			isMouseDown = false;
			// do a raycast to the currently mouse position
			Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D t_hit = Physics2D.GetRayIntersection (t_ray, 100, 1024 + 2048);
			GameObject t_gameObject;
			if (t_hit.collider != null) {
				// get the hit object
				t_gameObject = t_hit.collider.gameObject;
				// if its dragging
				if (isMouseDrag == true) {
					// set dragging back to false
					isMouseDrag = false;
					// set temp to X
					myGameObject_X = myGameObject_T;
					// set current hitting object to Y
					myGameObject_Y = t_gameObject;
					// set Y position
					myPosition_Y = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					// send message to chess X
					CmdChessAction (myGameObject_X, myGameObject_Y, myPosition_Y);
				} else {
					// if its not dragging
					// if X is empty
					if (myGameObject_X == null) {
						// if temp is my active chess
						if (t_BaseChess_T != null &&
						   t_BaseChess_T.GetProcess () != Process.Dead &&
						   t_BaseChess_T.GetMyOwnerID () == myID) {
							// set temp to X
							myGameObject_X = myGameObject_T;
							// show selecting effect
							ShowSelect ();
						}
					} else {
						// if X is not empty
						// set temp to Y
						myGameObject_Y = myGameObject_T;
						myPosition_Y = myPosition_T;
						// send message to chess X
						CmdChessAction (myGameObject_X, myGameObject_Y, myPosition_Y);
					}
				}
			}
		}
	}

	/// <summary>
	/// the chess action is done
	/// </summary>
	public void Done () {
		myGameObject_X = null;
		myGameObject_Y = null;
		myPosition_Y = Vector2.zero;

		HideSelect ();
	}

	/// <summary>
	/// the chess action is undone
	/// </summary>
	public void Undone () {
		if (myGameObject_Y.GetComponent<PT_BaseChess> () != null &&
			myGameObject_Y.GetComponent<PT_BaseChess> ().GetMyOwnerID () == myID) {
			myGameObject_X = myGameObject_Y;
			myGameObject_Y = null;
			myPosition_Y = Vector2.zero;

			ShowSelect ();
		} else
			Done ();
	}

	/// <summary>
	/// Shows the selecting effect.
	/// </summary>
	private void ShowSelect () {
		mySelect.transform.position = myGameObject_X.transform.position;
		mySelect.SetActive (true);
	}

	/// <summary>
	/// Hides the selecting effect.
	/// </summary>
	private void HideSelect () {
		mySelect.SetActive (false);
	}

	/// <summary>
	/// Shows the line effect.
	/// </summary>
	private void ShowLine () {
		myLine.GetComponent<LineRenderer> ().SetPosition (0, myGameObject_T.transform.position);
		myLine.GetComponent<LineRenderer> ().SetPosition (1, myPosition_T);
		myLine.SetActive (true);
	}

	/// <summary>
	/// Updates the line effect.
	/// </summary>
	/// <param name="t_pos">target end position.</param>
	private void UpdateLine (Vector3 t_pos) {
		myLine.GetComponent<LineRenderer> ().SetPosition (0, myGameObject_T.transform.position);
		myLine.GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (t_pos.x, t_pos.y, 0));
	}

	/// <summary>
	/// Hides the line effect.
	/// </summary>
	private void HideLine () {
		myLine.SetActive (false);
	}

	/// <summary>
	/// Checks if this player controller lose
	/// </summary>
	public void CheckLose () {
		List<GameObject> t_chessList = PT_NetworkGameManager.Instance.GetChessList (myID);
		Debug.Log (t_chessList.Count);
		// if any of the chess is not dead, return
		for (int i = 0; i < t_chessList.Count; i++) {
			if (t_chessList[i].GetComponent<PT_BaseChess> ().GetProcess () != Process.Dead) {
				return;
			}
		}
		// tell other players that I lose
		RpcLose ();
	}

	// used by ui, surrender
	public void Lose () {
		if (isLocalPlayer)
			CmdLose ();
	}

	#region Network
	// create chess
	[Command]
	public void CmdCreateChess (ChessType[] g_chessTypes, Vector2[] g_positions) {
		myID = System.Array.IndexOf (PT_NetworkGameManager.Instance.myPlayerList, this);
		//		Debug.Log (myID);

		if (myID == -1) {
			Invoke ("CmdCreateChess", 1);
			return;
		}

		for (int i = 0; i < g_chessTypes.Length; i++) {
			// create server-side instance
			GameObject t_chessObject = (GameObject)Instantiate (
										   PT_DeckManager.Instance.myChessBank.GetChessPrefab (g_chessTypes[i]),
										   Random.insideUnitCircle * 4,
										   Quaternion.identity
									   );
			PT_BaseChess t_chess = t_chessObject.GetComponent<PT_BaseChess> ();

			//add the chess to the network game manager
			//PT_NetworkGameManager.myChessList [myID].Add (t_chess);

			//set the player id to the chess
			t_chess.SetMyOwnerID (myID);
			t_chess.SetMyPlayerController (this);

			//myChessCount++;

			t_chess.transform.position = g_positions[i];

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

	// do action for chess
	[Command]
	public void CmdChessAction (GameObject g_active, GameObject g_target, Vector2 g_targetPos) {
		if (g_active.GetComponent<PT_BaseChess> () && g_active.GetComponent<PT_BaseChess> ().Action (g_target, g_targetPos))
			RpcDone ();
		else
			RpcUndone ();
	}

	// check lose
	[Command]
	public void CmdCheckLose () {
		List<GameObject> t_chessList = PT_NetworkGameManager.Instance.GetChessList (myID);
		Debug.Log (t_chessList.Count);
		for (int i = 0; i < t_chessList.Count; i++) {
			if (t_chessList[i].GetComponent<PT_BaseChess> ().GetProcess () != Process.Dead) {
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

		myTargetDisplays[g_ID].SetOwner (myID, g_ID);

		if (g_targetOwnerID == -1 || g_targetID == -1) {
			myTargetDisplays[g_ID].ShowTarget (g_TargetPosition);
		} else
			myTargetDisplays[g_ID].ShowTarget (g_targetOwnerID, g_targetID);
	}

	[ClientRpc]
	public void RpcHideTarget (int g_ID) {
		if (!isLocalPlayer)
			return;

		myTargetDisplays[g_ID].HideTarget ();
	}

	[ClientRpc]
	public void RpcInit () {
		if (isLocalPlayer) {
			Init ();
		}
	}
	#endregion
}

/// <summary>
/// used for target display
/// </summary>
public class PT_PlayerController_TargetDisplay {
	private GameObject myChess;
	private GameObject myTargetSign;
	private GameObject myTargetLine;

	private GameObject myTargetGameObject;
	private bool myTargetIsSingle = false;
	private Vector2 myTargetPosition;

	// init the target display
	public PT_PlayerController_TargetDisplay (string g_name, GameObject g_sign, GameObject g_line) {
		myTargetSign = g_sign;
		myTargetSign.name = "TargetSign_" + g_name;
		myTargetSign.SetActive (false);

		myTargetLine = g_line;
		myTargetLine.name = "TargetLine_" + g_name;
		myTargetLine.SetActive (false);
	}

	// set the owner
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

	// update the target display
	public void UpdateTarget () {
		if (myTargetLine == null || myTargetSign == null)
			return;

		if (myTargetLine.activeSelf == false || myTargetSign.activeSelf == false)
			return;

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
