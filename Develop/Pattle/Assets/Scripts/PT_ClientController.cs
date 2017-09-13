using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Pattle {
	public class PT_ClientController : NetworkBehaviour {

		[SerializeField] string myTeamTag;
		[SerializeField] GameObject[] myChessPrefabs;

		[SerializeField] List<GameObject> myChesses = new List<GameObject> ();

		private GameObject myGameObject_X;
//		private Vector2 myPosition_X;
		private GameObject myGameObject_Y;
		private Vector2 myPosition_Y;

		private GameObject myGameObject_T;
		private Vector2 myPosition_T;

		[SerializeField] GameObject Select;
		[SerializeField] GameObject myLine;

		//new input
		private bool isMouseDown;
		private Vector3 myMouseDownPosition;
		private bool isMouseDrag;

		void Start () {
		}

		public override void OnStartLocalPlayer () {
			//			Debug.Log (Network.player.ToString ());
			Invoke("CmdCreateChesses", 1);

			Select = Instantiate (Select, Vector3.zero, Quaternion.identity) as GameObject;
			myLine = Instantiate (myLine) as GameObject;
			//if (Select == null)
			//	Select = GameObject.Find (CS_Global.NAME_SELECT);
			HideSelect ();
			HideLine ();
		}

		void Update () {
			if (!isLocalPlayer) {
				return;
			}

			if (myGameObject_X != null)
				Select.transform.position = myGameObject_X.transform.position;

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

			if (isMouseDown == true && myGameObject_T.tag == myTeamTag &&
				Vector3.SqrMagnitude (myMouseDownPosition - Input.mousePosition) > CS_Global.DISTANCE_DRAG) {
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
				GameObject t_GO;
				if (t_hit.collider != null) {
					t_GO = t_hit.collider.gameObject;

					if (isMouseDrag == true) {
						isMouseDrag = false;

						myGameObject_X = myGameObject_T;
//						myPosition_X = myPosition_T;

						myGameObject_Y = t_GO;
						myPosition_Y = Camera.main.ScreenToWorldPoint (Input.mousePosition);

						//Send Message to chess X
						myGameObject_X.SendMessage ("SetTargetGameObject", myGameObject_Y);
						myGameObject_X.SendMessage ("SetTargetPosition", myPosition_Y);
						myGameObject_X.SendMessage ("Action", this.gameObject);
					} else {
						if (myGameObject_X == null) {
							if(myGameObject_T.tag == myTeamTag){
								myGameObject_X = myGameObject_T;
//								myPosition_X = myPosition_T;

								ShowSelect();
							}
						}
						else {
							myGameObject_Y = myGameObject_T;
							myPosition_Y = myPosition_T;
							//Send Message to chess X
							myGameObject_X.SendMessage("SetTargetGameObject",myGameObject_Y);
							myGameObject_X.SendMessage("SetTargetPosition",myPosition_Y);
							myGameObject_X.SendMessage("Action",this.gameObject);
						}
					}
				}



				//			Debug.Log ("MouseDown" + Input.mousePosition);
			}
		}

		[Command]
		private void CmdCreateChesses () {
			Debug.Log ("CmdCreateChesses: " + myChessPrefabs[0].name);
			foreach (GameObject t_chessPrefab in myChessPrefabs) {
				GameObject t_chess = Instantiate (t_chessPrefab, Random.insideUnitCircle, Quaternion.identity);
//				t_chess.GetComponent<Pattle.Chess.PT_Chess> ().SetMyNetworkPlayer (Network.player);
				NetworkServer.Spawn (t_chess);
				myChesses.Add (t_chess);
			}
		}

		[Command]
		private void CmdChessAction (GameObject g_caster, GameObject g_target, Vector2 g_targetPosition) {
			if (g_caster.GetComponent<Pattle.Chess.PT_Chess> ().Action (g_target, g_targetPosition)) {
				Done ();
			} else
				Undone ();
		}

		public void Done () {
			myGameObject_X = null;
			myGameObject_Y = null;
//			myPosition_X = Vector2.zero;
			myPosition_Y = Vector2.zero;

			HideSelect ();
		}

		public void Undone () {
			if (myGameObject_Y.tag == myTeamTag) {
				myGameObject_X = myGameObject_Y;
				myGameObject_Y = null;
//				myPosition_X = myPosition_Y;
				myPosition_Y = Vector2.zero;

				ShowSelect();
			} else
				Done ();
		}

		private void ShowSelect () {
			Select.transform.position = myGameObject_X.transform.position;
			Select.SetActive (true);
		}

		private void HideSelect () {
			Select.SetActive (false);
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
	}
}
