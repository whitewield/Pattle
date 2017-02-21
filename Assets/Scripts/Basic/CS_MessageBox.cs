using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_MessageBox : MonoBehaviour {

	private static CS_MessageBox instance = null;

	public string myTeamTag = "A";

	public List<GameObject> SubChessList = new List<GameObject>();

	public Vector3 lastAdventurePosition = Vector3.zero;

	public GameObject BOSS;
	public GameObject TutorialChess;
	public bool isHard;
	public GameObject map;

	public string adventureName;

	public bool planIntroWatched;

	//========================================================================
	public static CS_MessageBox Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	void Start () {
		CS_GameSave.CreatGameSave ();
	}

	public void InitBattle () {
		//create BOSS
		GameObject t_BOSS = Instantiate (BOSS) as GameObject;
		string t_TagBOSS = CS_Global.GetMyEnemyTag (myTeamTag);
		t_BOSS.SendMessage ("SetTag", t_TagBOSS);
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).GetComponent<CS_AfterBattle> ().AddChess (t_BOSS);

		//create Chess
		for (int i = 0; i < SubChessList.Count; i++) {
			SubChessList [i].SendMessage ("InitChess", myTeamTag);
		} 
		HideSubChessList ();

		GameObject.Find (CS_Global.NAME_AFTERBATTLE).GetComponent<CS_AfterBattle> ().On (myTeamTag);
	}

	public void InitTutorial () {
		//create Chess
		//GameObject t_myChess = Instantiate (TutorialChess, CS_Global.POS_DOWN_CENTER, Quaternion.identity) as GameObject;
		Instantiate (TutorialChess, CS_Global.POS_DOWN_CENTER, Quaternion.identity);

		//t_myChess.SendMessage ("SetTag", myTeamTag);
		//GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("AddChess", t_myChess);

		//GameObject.Find(CS_Global.NAME_AFTERBATTLE).SendMessage("On", myTeamTag);
	}

	public void SetBOSS (GameObject g_BOSS) {
		BOSS = g_BOSS;
	}

	public void SetTutorialChess (GameObject g_chess) {
		TutorialChess = g_chess;
	}

	public void SetMap (GameObject g_map) {
		map = g_map;
	}

	public void SetIsHard (bool g_isHard) {
		isHard = g_isHard;
	}

	public void GetMap () {
		Instantiate (map);
	}

	public void GetSubChessList (GameObject g_InputPlan) {
		g_InputPlan.SendMessage ("SetSubChessList", SubChessList);
		ShowSubChessList ();
	}

	public void ShowSubChessList () {
		for (int i = 0; i < SubChessList.Count; i++) {
			SubChessList[i].SetActive (true);
		}
	}

	public void HideSubChessList () {
		for (int i = 0; i < SubChessList.Count; i++) {
			SubChessList[i].SetActive (false);
		}
	}

	public void GetAdventurePosition (GameObject g_AdventureMap) {
		g_AdventureMap.SendMessage ("SetPosition", lastAdventurePosition);
	}

	public void SaveAdventurePosition (Vector3 g_lastAdventurePosition) {
		lastAdventurePosition = g_lastAdventurePosition;
	}

	public void SetAdventureName (string g_name) {
		adventureName = g_name;
	}

	////////////////////////////////////////
	/// Adventure Victory
	public void AdventureVictory (string g_data) {
		string t_data = CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_ADVENTURE, adventureName);
		int t_dataInt = int.Parse (t_data);
		int g_dataInt = int.Parse (g_data);
		if (isHard)
			g_dataInt += 2;

		Debug.Log (t_dataInt + "=-=" + g_dataInt);
		
		if (t_dataInt >= g_dataInt) {
			if (isHard)
				AdventureRewardCoins (CS_AdventureReward.GetRewardCoins (adventureName) / 5);
			else 
				AdventureRewardCoins (CS_AdventureReward.GetRewardCoins (adventureName) / 10);
		} else {
			if (CS_AdventureReward.GetRewardType (adventureName) == CS_AdventureReward.REWARD_CHESS) {
				//get chess reward
				AdventureRewardChess ();
			} else {
				//get coins reward
				AdventureRewardCoins (CS_AdventureReward.GetRewardCoins (adventureName) * (g_dataInt - t_dataInt));
			}

			//save
			CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_ADVENTURE, adventureName, g_dataInt.ToString ());
		}
	}

	private void AdventureRewardChess () {
		//get chess reward
		string t_chessReward = CS_AdventureReward.GetRewardChess (adventureName);
		
		if (SaveAddChess (t_chessReward)) {
			//show chess in after battle
			GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage ("ShowRewardChess", t_chessReward);
		} else {
			//if already have the chess
			//get coins reward
			AdventureRewardCoins (CS_AdventureReward.GetRewardCoins (adventureName) / 10);
		}
	}

	private void AdventureRewardCoins (int g_coinsReward) {
		//get coins reward
		SaveAddCoins (g_coinsReward);
		//show coins reward in afterBattle
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage ("ShowRewardCoins", g_coinsReward);
	}

	////////////////////////////////////////
	/// Save
	public bool SaveAddChess (string g_chess) {
		if (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_CHESS, g_chess) == "1")
			return false;

		Debug.Log ("Save + chess : " + g_chess);
		//save chess
		CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_CHESS, g_chess, "1");
		return true;
	}

	public void UpdateBag () {
		//run after save:addChess at bag page
		GameObject[] t_BtnChessArray = GameObject.FindGameObjectsWithTag (CS_Global.TAG_BTNCHESS);
		foreach (GameObject t_BtnChess in t_BtnChessArray) {
			t_BtnChess.SendMessage ("UpdateSprite");
		}
		//including update coins amount
	}

	public bool SaveAddCoins (int g_coinsReward) {
		Debug.Log ("Save + coins : " + g_coinsReward);
		//get coins amount
		int t_coinsAmount = int.Parse (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_BAG, CS_Global.SAVE_TITLE_COINS));
		t_coinsAmount += g_coinsReward;
		//save coins amount
		CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_BAG, CS_Global.SAVE_TITLE_COINS, t_coinsAmount.ToString());
		return true;
	}

	public bool SaveLoseCoins (int g_coinsReward) {
		Debug.Log ("Save - coins : " + g_coinsReward);
		//get coins amount
		int t_coinsAmount = int.Parse (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_BAG, CS_Global.SAVE_TITLE_COINS));

		if (t_coinsAmount < g_coinsReward)
			return false;

		t_coinsAmount -= g_coinsReward;
		//save coins amount
		CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_BAG, CS_Global.SAVE_TITLE_COINS, t_coinsAmount.ToString());
		return true;
	}

	////////////////////////////////////////
	/// Load Scene
	public void LoadScene (string g_name) {
		GameObject.Find (CS_Global.NAME_TRANSITIONMANAGER).SendMessage (CS_Global.NAME_TRANSITIONMANAGER_SAO, g_name);
//		Debug.Log ("LoadScene");
	}
}
