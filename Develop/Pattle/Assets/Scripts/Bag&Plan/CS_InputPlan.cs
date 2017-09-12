using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_InputPlan : CS_Control {

	public GameObject GO_X;
	public Vector2 Pos_X;
	public GameObject GO_Y;
	public Vector2 Pos_Y;
	
	public GameObject Select;
	public GameObject SubChess;
	public List<GameObject> SubChessList;

	private float timerCountDown = CS_Global.TIME_COUNTDOWN;
	public GameObject TX_TimerCountDown;

	public GameObject Btn_Start;

	public bool quit = false;

	public CS_InfoBook_S myInfoBook_S;
	public CS_PlanIntroduction myPlanIntroduction;

	void Start () {
		this.name = CS_Global.NAME_INPUTMANAGER;
		
		Select = Instantiate (Select, Vector3.zero, Quaternion.identity) as GameObject;
		HideSelect ();

		InitSubChessList ();

		//ResetTimerCountDown ();
		HideTimerCountDown ();

		HideBtnStart ();
	}
	
	void Update () {
		if (quit)
			return;

		if (timerCountDown <= 0)
			return;

		//press btn to start
		if (SubChessList.Count == CS_Global.NUMBER_CHESS) {
			//if there are 3 chesses on board, show start btn;
			ShowBtnStart ();
		} else
			HideBtnStart ();
		
		//count down to start
//		if (SubChessList.Count == CS_Global.NUMBER_CHESS) {
//			//if there are 3 chesses on board, start to count down;
//			ShowTimerCountDown ();
//			timerCountDown -= Time.deltaTime;
//			if (timerCountDown <= 0)
//			{
//				timerCountDown = 0;
//				StartBattle ();
//			}
//		} else
//			HideTimerCountDown ();
	}

	public override void SetGO (GameObject g_GO) {
		//if the you don't have the chess, return
		if (g_GO.tag == CS_Global.TAG_BTNCHESS && CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_CHESS, g_GO.name) == "0")
			return;

		if (GO_X == null) {
			//Cannot select map as the first object
			if (g_GO.tag == CS_Global.TAG_MAP)
				return;

			//if(g_GO.tag == myTeamTag){
			GO_X = g_GO;
			Pos_X = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			PreAction ();
			//}
		}
		else {
			GO_Y = g_GO;
			Pos_Y = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Action ();
		}

		//ResetTimerCountDown ();

		if (g_GO.tag == CS_Global.TAG_BTNCHESS) {
			//Show Info book Small
			myInfoBook_S.Show (g_GO);
		}
	}

	public override void UnsetGO (GameObject g_GO) {
		//Hide Info book Small
		myInfoBook_S.Hide ();
	}

	private void PreAction () {
		//change by Sep 3

		if (GO_X.tag == CS_Global.TAG_BTNCHESS) {
			if (SubChessListCheckSame (GO_X)) {
				//already has the chess on stage and remove it
				Done ();
			} else 
				ShowSelect ();
		} else
			ShowSelect ();
	}
	
	private void Action () {
		if (GO_X.tag == CS_Global.TAG_BTNCHESS && 
			GO_Y.tag == CS_Global.TAG_MAP &&
			SubChessList.Count < CS_Global.NUMBER_CHESS) {
			//new sub chess
			GameObject t_SubChess = Instantiate (SubChess, Pos_Y, Quaternion.identity) as GameObject;
			t_SubChess.name = CS_Global.NAME_SUBCHESS;
			t_SubChess.GetComponent<CS_SubChess> ().myChess = GO_X.GetComponent<CS_SubChess> ().myChess;
			t_SubChess.GetComponent<SpriteRenderer> ().sprite = GO_X.GetComponent<SpriteRenderer> ().sprite;

			SubChessList.Add (t_SubChess);
			t_SubChess.transform.SetParent(GameObject.Find (CS_Global.NAME_MESSAGEBOX).transform);

			Done();
		} else if (GO_X.tag == CS_Global.TAG_BTNCHESS && GO_Y.tag == CS_Global.TAG_SUBCHESS) {
			//delete the sub chess if same as btn chess
			SubChessListRemove(GO_X);
			//change subchess
			GO_Y.GetComponent<CS_SubChess> ().myChess = GO_X.GetComponent<CS_SubChess> ().myChess;
			GO_Y.GetComponent<SpriteRenderer> ().sprite = GO_X.GetComponent<SpriteRenderer> ().sprite;
			Done();
		} else if (GO_Y.tag == CS_Global.TAG_BTNCHESS && GO_X.tag == CS_Global.TAG_SUBCHESS) {
			//delete the sub chess if same as btn chess
			SubChessListRemove(GO_Y);
			//change subchess
			GO_X.GetComponent<CS_SubChess> ().myChess = GO_Y.GetComponent<CS_SubChess> ().myChess;
			GO_X.GetComponent<SpriteRenderer> ().sprite = GO_Y.GetComponent<SpriteRenderer> ().sprite;
			Done();
		} else if (GO_X.tag == CS_Global.TAG_SUBCHESS && GO_Y.tag == CS_Global.TAG_MAP) {
			//move sub chess
			GO_X.transform.position = Pos_Y;
			Done();
		}  else if (GO_X.tag == CS_Global.TAG_SUBCHESS && GO_Y.tag == CS_Global.TAG_SUBCHESS) {
			if (GO_X == GO_Y) {
				//delete the sub chess
				SubChessListRemove(GO_X);
			} else {
				//switch sub chess
				Pos_Y = GO_X.transform.position;
				GO_X.transform.position = GO_Y.transform.position;
				GO_Y.transform.position = Pos_Y;
			}
			Done();
		} else
			Undone ();
	}
	
	public void Done () {
		GO_X = null;
		GO_Y = null;
		Pos_X = Vector2.zero;
		Pos_Y = Vector2.zero;
		
		HideSelect ();
	}
	
	public void Undone () {
		if (GO_Y.tag == CS_Global.TAG_SUBCHESS || 
		    GO_Y.tag == CS_Global.TAG_BTNCHESS) {
			GO_X = GO_Y;
			GO_Y = null;
			Pos_X = Pos_Y;
			Pos_Y = Vector2.zero;

			PreAction ();
			//ShowSelect();
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

	public void SwitchPage () {
		//deselect when selecting BtnChess
		if (GO_X == null)
			return;
		if (GO_X.tag == CS_Global.TAG_BTNCHESS)
			Done ();
	}

	//ShowPlanIntroduction 
	private void ShowPlanIntroduction () {
		myPlanIntroduction.Show ();
	}
		
	//BtnStart 
	private void ShowBtnStart () {
		Btn_Start.SetActive (true);
	}

	private void HideBtnStart () {
		Btn_Start.SetActive (false);
	}

	private void StartBattle () {
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage (CS_Global.NAME_MESSAGEBOX_LOADSCENE, CS_Global.SCENE_BATTLE);
		//GameObject.Find (CS_Global.NAME_TRANSITIONMANAGER).SendMessage ("StartAnimationOut", CS_Global.SCENE_BATTLE);
	}

	//TimerCountDown
	private void ResetTimerCountDown () {
		timerCountDown = CS_Global.TIME_COUNTDOWN;
	}

	private void ShowTimerCountDown () {
		TX_TimerCountDown.SetActive (true);
		TX_TimerCountDown.GetComponent<TextMesh> ().text = timerCountDown.ToString ("f1");
	}

	private void HideTimerCountDown () {
		TX_TimerCountDown.SetActive (false);
	}

	//SubChessList
	public bool SubChessListRemove (GameObject g_btnChess) {
		GameObject TheSameChess = null;
		for (int i = 0; i < SubChessList.Count; i++) {
			if(SubChessList[i].GetComponent<SpriteRenderer>().sprite == g_btnChess.GetComponent<SpriteRenderer>().sprite) {
				TheSameChess = SubChessList[i];
				break;
			}
		}
		if (TheSameChess != null) {
			SubChessList.Remove (TheSameChess);
			Destroy(TheSameChess);
			return true;
		}
		return false;
	}

	//SubChessList
	public bool SubChessListCheckSame (GameObject g_btnChess) {
		GameObject TheSameChess = null;
		for (int i = 0; i < SubChessList.Count; i++) {
			if(SubChessList[i].GetComponent<SpriteRenderer>().sprite == g_btnChess.GetComponent<SpriteRenderer>().sprite) {
				TheSameChess = SubChessList[i];
				break;
			}
		}
		if (TheSameChess != null) {
			return true;
		}
		return false;
	}

	public void InitSubChessList () {
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("GetSubChessList", this.gameObject);
	}

	public void SetSubChessList (List<GameObject> g_SubChessList) {
		SubChessList = g_SubChessList;
	}

	public void HideSubChessList () {
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("HideSubChessList");
	}

	public void Quit () {
		quit = true;
		HideSubChessList ();
		HideTimerCountDown ();
	}
}
