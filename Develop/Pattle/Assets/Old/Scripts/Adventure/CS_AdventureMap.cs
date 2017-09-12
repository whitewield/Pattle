using UnityEngine;
using System.Collections;

public class CS_AdventureMap : MonoBehaviour {

	public Camera myCamera;
	public GameObject map;
	public float zoomRate = 2.0f;
	public float moveRate = 2.0f;
	public Vector2 maxPosition = new Vector2 (20, 20);

	//private int onMouseDown = 0;			//0:down 1:up,1stTime 2:up,not1stTime
	private Vector3 preMousePosition;
	
	private GameObject myMessageBox;

	// Use this for initialization
	void Start () {
		myMessageBox = GameObject.Find (CS_Global.NAME_MESSAGEBOX) as GameObject;
		myMessageBox.SendMessage("GetAdventurePosition",this.gameObject);
	}

	void OnMouseDown() {
		//set previous mouse position
		preMousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseDrag() {
		//Get mouse position (World Point)
		Vector3 t_Postion = myCamera.ScreenToWorldPoint(Input.mousePosition);

		Vector3 t_deltaPosition = t_Postion - preMousePosition;
		//float t_distance = t_deltaPosition.magnitude;

		//increase x delta position
		t_deltaPosition *= moveRate;
		t_deltaPosition += t_deltaPosition.x * Vector3.right;
		
		//reset camera size according to distance
		//Camera.main.SendMessage("SetSize", (CS_Global.CAMERA_SIZE + t_distance * 2));
		Camera.main.SendMessage("SetSize", (CS_Global.CAMERA_SIZE * zoomRate));

		//Move map
		map.transform.position += t_deltaPosition;
		
		//record previous mouse position
		preMousePosition = t_Postion;

		//set max position
		if (map.transform.position.x > maxPosition.x) {
			map.transform.position = new Vector2 (maxPosition.x, map.transform.position.y);
		} else if (map.transform.position.x < maxPosition.x * -1) {
			map.transform.position = new Vector2 (maxPosition.x * -1, map.transform.position.y);
		}

		if (map.transform.position.y > maxPosition.y) {
			map.transform.position = new Vector2 (map.transform.position.x, maxPosition.y);
		} else if (map.transform.position.y < maxPosition.y * -1) {
			map.transform.position = new Vector2 (map.transform.position.x, maxPosition.y * -1);
		}
	}

	void OnMouseUp() {
		//Save Adventure Position
		myMessageBox.SendMessage ("SaveAdventurePosition", map.transform.position);
		
		//reset camera size
		Camera.main.SendMessage("SetSize", (CS_Global.CAMERA_SIZE));
		//Debug.Log("reset camera size");
	}
	
	// Update is called once per frame
//	void Update () {
//		if (onMouseDown > 0) {
//			//Get mouse position (World Point)
//			Vector3 t_Postion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//
//			//if 1st time, set previous mouse position
//			if (onMouseDown == 1) {
//				preMousePosition = t_Postion;
//				onMouseDown = 2;
//			}
//
//			//Move this map
//			Vector3 t_deltaPosition = t_Postion - preMousePosition;
//			float t_distance = t_deltaPosition.magnitude;
//			//t_deltaPosition *= t_distance;
//			//Debug.Log(t_distance);
//			
//			//reset camera size according to distance
//			Camera.main.SendMessage("SetSize", (CS_Global.CAMERA_SIZE + t_distance * 2));
//			//Camera.main.SendMessage("SetSize", (CS_Global.CAMERA_SIZE * 2));
//			map.transform.position += t_deltaPosition;
//
//			//record previous mouse position
//			preMousePosition = t_Postion;
//
//			//if mouse up, update onMouseDown
//			if (Input.GetMouseButtonUp (0)) {
//				onMouseDown = 0;
//
//				//Save Adventure Position
//				myMessageBox.SendMessage ("SaveAdventurePosition", map.transform.position);
//
//				//reset camera size
//				Camera.main.SendMessage("SetSize", (CS_Global.CAMERA_SIZE));
//				Debug.Log("reset camera size");
//			}
//		} else {
//			//if mouse down, update onMouseDown
//			if (Input.GetMouseButtonDown (0))
//				onMouseDown = 1;
//		}
//	}

	public void SetPosition (Vector3 g_position) {
		map.transform.position = g_position;
	}

}
