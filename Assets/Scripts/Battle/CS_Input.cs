using UnityEngine;
using System.Collections;

public class CS_Input : CS_Control {
	public GameObject GO_X;
	public Vector2 Pos_X;
	public GameObject GO_Y;
	public Vector2 Pos_Y;

	private GameObject GO_T;
	private Vector2 Pos_T;

	[SerializeField] GameObject Select;
	[SerializeField] GameObject myLine;

	//new input
	private bool isMouseDown;
	private Vector3 myMouseDownPosition;
	private bool isMouseDrag;

	void Start () {
		Select = Instantiate (Select, Vector3.zero, Quaternion.identity) as GameObject;
		myLine = Instantiate (myLine) as GameObject;
		//if (Select == null)
		//	Select = GameObject.Find (CS_Global.NAME_SELECT);
		HideSelect ();
		HideLine ();
	}

	void Update () {
		if (GO_X != null)
			Select.transform.position = GO_X.transform.position;

		if (Input.GetMouseButtonDown (0)) {
			isMouseDown = true;
//			Debug.Log ("MouseDown" + Input.mousePosition);

			myMouseDownPosition = Input.mousePosition;

			Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D t_hit = Physics2D.GetRayIntersection (t_ray, 100, 1024 + 2048);
			if (t_hit.collider != null) {
//				Debug.Log (t_hit.transform.name);

				GO_T = t_hit.collider.gameObject;
				Pos_T = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			} else
				isMouseDown = false;
		}

		//Debug.Log (Vector3.SqrMagnitude (myMouseDownPosition - Input.mousePosition));

		if (isMouseDown == true && GO_T.tag == myTeamTag &&
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

					GO_X = GO_T;
					Pos_X = Pos_T;

					GO_Y = t_GO;
					Pos_Y = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					//Send Message to chess X
					GO_X.SendMessage ("SetTargetGameObject", GO_Y);
					GO_X.SendMessage ("SetTargetPosition", Pos_Y);
					GO_X.SendMessage ("Action", this.gameObject);
				} else {
					if (GO_X == null) {
						if(GO_T.tag == myTeamTag){
							GO_X = GO_T;
							Pos_X = Pos_T;

							ShowSelect();
						}
					}
					else {
						GO_Y = GO_T;
						Pos_Y = Pos_T;
						//Send Message to chess X
						GO_X.SendMessage("SetTargetGameObject",GO_Y);
						GO_X.SendMessage("SetTargetPosition",Pos_Y);
						GO_X.SendMessage("Action",this.gameObject);
					}
				}
			}



//			Debug.Log ("MouseDown" + Input.mousePosition);
		}
	}

	public override void SetGO (GameObject g_GO) {
//		if (GO_X == null) {
//			if(g_GO.tag == myTeamTag){
//				GO_X = g_GO;
//				Pos_X = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//
//				ShowSelect();
//			}
//		}
//		else {
//			GO_Y = g_GO;
//			Pos_Y = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			//Send Message to chess X
//			GO_X.SendMessage("SetTargetGameObject",GO_Y);
//			GO_X.SendMessage("SetTargetPosition",Pos_Y);
//			GO_X.SendMessage("Action",this.gameObject);
//		}
	}

	public void Done () {
		GO_X = null;
		GO_Y = null;
		Pos_X = Vector2.zero;
		Pos_Y = Vector2.zero;

		HideSelect ();
	}

	public void Undone () {
		if (GO_Y.tag == myTeamTag) {
			GO_X = GO_Y;
			GO_Y = null;
			Pos_X = Pos_Y;
			Pos_Y = Vector2.zero;

			ShowSelect();
		} else
			Done ();
	}

	private void ShowSelect () {
		Select.transform.position = GO_X.transform.position;
		Select.SetActive (true);
	}

	private void HideSelect () {
		Select.SetActive (false);
	}

	private void ShowLine () {
		myLine.GetComponent<LineRenderer> ().SetPosition (0, GO_T.transform.position);
		myLine.GetComponent<LineRenderer> ().SetPosition (1, Pos_T);
		myLine.SetActive (true);
	}

	private void UpdateLine (Vector3 t_pos) {
		myLine.GetComponent<LineRenderer> ().SetPosition (0, GO_T.transform.position);
		myLine.GetComponent<LineRenderer> ().SetPosition (1, new Vector3(t_pos.x, t_pos.y, 0));
	}

	private void HideLine () {
		myLine.SetActive (false);
	}

}
