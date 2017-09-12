using UnityEngine;
using System.Collections;

public class CS_AdventureBook : MonoBehaviour {
	
	public GameObject GO_Chess;

	public GameObject TX_Info;

	public GameObject Btn_Easy;
	public GameObject Btn_Hard;

	private CS_AdventureChess myAdventureChess;


	// Use this for initialization
	void Start () {
		Hide ();
	}

	void OnMouseDown() {
		Hide ();
	}

	public void Hide () {
		this.gameObject.SetActive (false);
	}

	public void EasyMode () {
		CS_MessageBox.Instance.SetIsHard (false);

		if (myAdventureChess.isTutorial) {
			CS_MessageBox.Instance.SetTutorialChess (myAdventureChess.myChess);
			CS_MessageBox.Instance.SetIsHard (true);
		}
		else
			CS_MessageBox.Instance.SetBOSS (myAdventureChess.myBoss);

		InitBattle ();
	}

	public void HardMode () {
		CS_MessageBox.Instance.SetIsHard (true);

		if (myAdventureChess.isTutorial)
			CS_MessageBox.Instance.SetTutorialChess (myAdventureChess.myChess);
		else
			CS_MessageBox.Instance.SetBOSS (myAdventureChess.myBossHard);
		
		InitBattle ();
	}

	public void InitBattle () {
		CS_MessageBox.Instance.SetMap (myAdventureChess.myMap);
		CS_MessageBox.Instance.SetAdventureName (myAdventureChess.myName);

		string t_name = myAdventureChess.myName;
		switch (t_name) {
		case "TutorialMove":
			CS_MessageBox.Instance.LoadScene ("TutorialMove");
			break;
		case "TutorialSingle":
			CS_MessageBox.Instance.LoadScene ("TutorialHeal");
			break;
		case "TutorialClose":
			CS_MessageBox.Instance.LoadScene ("Tutorial");
			break;
		case "TutorialSpell":
			CS_MessageBox.Instance.LoadScene ("Tutorial");
			break;
		case "TutorialDistance":
			CS_MessageBox.Instance.LoadScene ("Tutorial");
			break;
		default: 
			CS_MessageBox.Instance.LoadScene ("Plan");
			break;
		}

	}

	public void Show (GameObject g_adventureChess) {
		myAdventureChess = g_adventureChess.GetComponent<CS_AdventureChess> ();
//		Debug.Log ("sprite : " +  g_adventureChess.GetComponent<SpriteRenderer> ().sprite);
		GO_Chess.GetComponent<SpriteRenderer>().sprite = g_adventureChess.GetComponent<SpriteRenderer> ().sprite;
		GO_Chess.transform.localScale = Vector3.one * 2 / myAdventureChess.mySpriteSize;
		TX_Info.SendMessage ("SetTitle", myAdventureChess.myName);

		if (int.Parse (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_ADVENTURE, myAdventureChess.myName)) >= 
			int.Parse (CS_Global.STAR_PERFECT) && !myAdventureChess.isTutorial)
			Btn_Hard.SetActive (true);
		else
			Btn_Hard.SetActive (false);
	}


}