using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_PlayerController : NetworkBehaviour {
	[SyncVar] int myID = -1;
	[SerializeField] GameObject[] myChessPrefabs;
	[SerializeField] Vector2[] myChessPositions;
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

	void Awake()
	{
		//register the spaceship in the gamemanager, that will allow to loop on it.
		if (PT_NetworkGameManager.myPlayerList [0] != null && PT_NetworkGameManager.myPlayerList [0].enabled == true) {
			PT_NetworkGameManager.myPlayerList [1] = this;
		} else {
			PT_NetworkGameManager.myPlayerList [0] = this;
		}
	}

	// Use this for initialization
	void Start () {
		if (!base.isLocalPlayer) {
			return;
		}
			
		mySelect = Instantiate (mySelect, Vector3.zero, Quaternion.identity) as GameObject;
		mySelect.transform.SetParent (this.transform);
		myLine = Instantiate (myLine) as GameObject;
		myLine.transform.SetParent (this.transform);
		HideSelect ();
		HideLine ();
		
		if (PT_NetworkGameManager.Instance != null) {
			//we MAY be awake late (see comment on _wasInit above), so if the instance is already there we init
			Init();
		}
	}

	public void Init () {
		//		Debug.Log (GetInstanceID ());
		if (System.Array.IndexOf (PT_NetworkGameManager.myPlayerList, this) == -1) {
			Invoke ("Init", 1);
			Debug.Log ("Invoke");
			return;
		}


		//rotate the camera
//		Debug.Log ("do" + System.Array.IndexOf (PT_NetworkGameManager.myPlayerList, this));
		if (System.Array.IndexOf (PT_NetworkGameManager.myPlayerList, this) == 1)
			Camera.main.transform.rotation = Quaternion.Euler (0, 0, 180);
		else
			Camera.main.transform.rotation = Quaternion.Euler (0, 0, 0);

		if (wasInit)
			return;

		wasInit = true;

		CmdCreateChess ();

		//		Debug.Log (GetInstanceID ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!base.isLocalPlayer)
			return;

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
			t_BaseChess_T.GetProcess() != PT_Global.Process.Dead &&
			t_BaseChess_T.GetMyOwnerID() == myID &&
			Vector3.SqrMagnitude (myMouseDownPosition - Input.mousePosition) > PT_Global.DISTANCE_DRAG) {
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
						   t_BaseChess_T.GetProcess () != PT_Global.Process.Dead &&
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
		
	[Command]
	public void CmdCreateChess () {
		myID = System.Array.IndexOf (PT_NetworkGameManager.myPlayerList, this);
		Debug.Log (myID);

		if (myID == -1) {
			Invoke ("CmdCreateChess", 1);
			return;
		}

		for(int i = 0; i < myChessPrefabs.Length; i++) {
			// create server-side instance

			GameObject t_chessObject = (GameObject)Instantiate (myChessPrefabs[i], Random.insideUnitCircle * 4, Quaternion.identity);
			PT_BaseChess t_chess = t_chessObject.GetComponent<PT_BaseChess> ();

			//add the chess to the network game manager
//			PT_NetworkGameManager.myChessList [myID].Add (t_chess);

			//set the player id to the chess
			t_chess.SetMyOwnerID (myID);

//			myChessCount++;

			t_chess.transform.position = myChessPositions [i];

			if (myID == 1) {
				t_chess.transform.position *= -1;
			}

			//test
//			if(myID == 0)t_chessObject.GetComponent<SpriteRenderer>().color = Color.red;

			// spawn on the clients
			NetworkServer.Spawn (t_chessObject);
		}
	}

	[Command]
	public void CmdChessAction (GameObject g_active, GameObject g_target, Vector2 g_targetPos) {
		if (g_active.GetComponent<PT_BaseChess> ().Action (g_target, g_targetPos))
			RpcDone ();
		else 
			RpcUndone ();
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
}
