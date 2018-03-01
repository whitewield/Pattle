using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PT_Global;
using Pattle.Adventure;
using UnityEngine.UI;

namespace Pattle {
	namespace Adventure {
		[System.Serializable]
		public struct AdventureSetup {
			public string myMenuDisplayName;
			public GameObject myMenuDisplayPrefab;

			public BossType myBossType;
			public GameObject myMapPrefab;

			public int myUnlockMedalCount;
		}

		public static class AdventureSave {
			public static MedalType[] LoadMedalTypes (BossType g_bossType) {
				string t_loadString = ShabbySave.LoadGame (Constants.SAVE_CATEGORY_ADVENTURE, g_bossType.ToString ());

				int t_load = int.Parse (t_loadString);

				//left to right: easy, normal, hard
				MedalType[] t_medalTypes = new MedalType[3];
				t_medalTypes [0] = (MedalType)(t_load / 100);
				t_medalTypes [1] = (MedalType)((t_load % 100) / 10);
				t_medalTypes [2] = (MedalType)(t_load % 10);

				return t_medalTypes;
			}

			public static void SaveMedalTypes (BossType g_bossType, MedalType[] g_medalTypes) {
				//left to right: easy, normal, hard
				string t_saveString = 
					((int)g_medalTypes [0]).ToString () +
					((int)g_medalTypes [1]).ToString () +
					((int)g_medalTypes [2]).ToString ();

				ShabbySave.SaveGame (Constants.SAVE_CATEGORY_ADVENTURE, g_bossType.ToString (), t_saveString);
			}

			public static MedalType LoadMedalType (BossType g_bossType, BossDifficulty g_difficulty) {
				return LoadMedalTypes (g_bossType) [(int)(g_difficulty)];
			}

			public static void SaveMedalType (BossType g_bossType, BossDifficulty g_difficulty, MedalType g_medalType) {
				MedalType t_currentMedalType = LoadMedalType (g_bossType, g_difficulty);

				if ((int)t_currentMedalType < (int)g_medalType) {
					//save

					MedalType[] t_medalTypes = LoadMedalTypes (g_bossType);
					t_medalTypes [(int)g_difficulty] = g_medalType;
					SaveMedalTypes (g_bossType, t_medalTypes);
				}
			}
		}
	}
}

public class PT_AdventureMenuCanvas : MonoBehaviour {

	enum AdventureMenuState {
		AdventureList,
		Difficulty,
		Deck,
	}

	private static PT_AdventureMenuCanvas instance = null;
	public static PT_AdventureMenuCanvas Instance { get { return instance; } }

	private AdventureMenuState myState = AdventureMenuState.AdventureList;

	private RectTransform myCurrentPage;
	[SerializeField] RectTransform myPageAll;

	private AdventureSetup myCurrentSetup;
//	private BossDifficulty myCurrentDifficulty;

	[Header ("AdventureList")]
	[SerializeField] RectTransform myPageAdventureList;
	[SerializeField] RectTransform myPageAdventureList_Content;
	private List<PT_AdventureButton> myPageAdventureList_Buttons = new List<PT_AdventureButton> ();
	[SerializeField] Vector2 myPageAdventureList_Content_Height;//x -> the base height, y -> the spacing
	[SerializeField] GameObject[] myPageAdventureList_ButtonPrefabs;
	[SerializeField] GameObject myMedalsPrefab;
	public GameObject MedalsPrefab { get { return myMedalsPrefab; } }
	private int myMedalCount = 0;
	[SerializeField] Color myMedalColor_None = Color.clear;
	[SerializeField] Color myMedalColor_Bronze = Color.black;
	[SerializeField] Color myMedalColor_Silver = Color.gray;
	[SerializeField] Color myMedalColor_Gold = Color.yellow;

	[Header ("Difficulty")]
	[SerializeField] RectTransform myPageDifficulty;
	[SerializeField] Text myPageDifficulty_Name;
	[SerializeField] RectTransform myPageDifficulty_Image;
	private GameObject myPageDifficulty_CurrentImage;
	[SerializeField] Image[] myPageDifficulty_MedalImages;

	[Header ("Deck")]
	[SerializeField] RectTransform myPageDeck;
	[SerializeField] Text myPageDeck_Name;
	[SerializeField] RectTransform myPageDeck_Image;
	[SerializeField] Text myPageDeck_DifficultyText;
	[SerializeField] Image myPageDeck_DifficultyImage;
	private GameObject myPageDeck_CurrentImage;


	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		this.transform.position = Vector3.zero;
	}

	// Use this for initialization
	void Start () {
		if (NetworkManager.singleton.onlineScene != Constants.SCENE_ADVENTURE)
			NetworkManager.singleton.onlineScene = Constants.SCENE_ADVENTURE;
		//set game mode to adventure
		PT_DeckManager.Instance.SetGameMode (GameMode.Adventure);

		//saving test
		MedalType[] t_saveTest = {MedalType.Gold, MedalType.Silver, MedalType.Bronze};
		AdventureSave.SaveMedalTypes (BossType.FireDragon, t_saveTest);
		Debug.Log (AdventureSave.LoadMedalType (BossType.FireDragon, BossDifficulty.Easy));
		Debug.Log (AdventureSave.LoadMedalType (BossType.FireDragon, BossDifficulty.Normal));
		Debug.Log (AdventureSave.LoadMedalType (BossType.FireDragon, BossDifficulty.Hard));

		Init_AdventureList ();

		SetState (AdventureMenuState.AdventureList);

		Debug.Log ("medal count: " + myMedalCount);
	}

	private void Init_AdventureList () {
		float t_height = myPageAdventureList_Content_Height.x;

		foreach (GameObject f_prefab in myPageAdventureList_ButtonPrefabs) {
			GameObject f_gameObject = Instantiate (f_prefab, myPageAdventureList_Content);

			// add the button to the button list
			PT_AdventureButton f_buttonScript = f_gameObject.GetComponent<PT_AdventureButton> ();
			myPageAdventureList_Buttons.Add (f_buttonScript);

			//show medals
			MedalType[] f_medals = AdventureSave.LoadMedalTypes (f_buttonScript.GetBossType ());
			f_buttonScript.Init (myMedalsPrefab, f_medals);

			//update medal count
			foreach (MedalType f_type in f_medals) {
				if (f_type != MedalType.None) {
					myMedalCount++;
				}
			}

			// change the height in content object
			RectTransform f_rectTransform = f_gameObject.transform as RectTransform;
			t_height += f_rectTransform.sizeDelta.y;
			t_height += myPageAdventureList_Content_Height.y;
		}

		//check buttons unlock or lock
		foreach (PT_AdventureButton f_button in myPageAdventureList_Buttons) {
			f_button.CheckLock (myMedalCount);
		}

		t_height -= myPageAdventureList_Content_Height.y;
		myPageAdventureList_Content.sizeDelta = new Vector2 (myPageAdventureList_Content.sizeDelta.x, t_height);
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePage ();
	}

	private void UpdatePage () {
		Vector2 t_targetPos = new Vector2 (-myCurrentPage.anchoredPosition.x, myPageAll.anchoredPosition.y);
		myPageAll.anchoredPosition = Vector2.Lerp (myPageAll.anchoredPosition, t_targetPos, Time.unscaledDeltaTime * PT_Global.Constants.SPEED_UI_LERP);
	}


//	public void OnButtonDragon (int boss) {
//		PT_DeckManager.Instance.SetAdventureBossType (PT_Global.ChessType.FireDragon);
//
//		NetworkManager.singleton.matchSize = 1;
//		NetworkManager.singleton.onlineScene = "NetworkAdventure";
////		NetworkManager.singleton.StartHost ();
//
//		PT_Preset.Instance.ShowAdventure ();
//	}

	public void OnButtonAdventure (AdventureSetup g_setup) {
		myCurrentSetup = g_setup;

		// update the boss avator
		if (myPageDifficulty_CurrentImage != null)
			Destroy (myPageDifficulty_CurrentImage);
		myPageDifficulty_CurrentImage = Instantiate (g_setup.myMenuDisplayPrefab, myPageDifficulty_Image);

		if (myPageDeck_CurrentImage != null)
			Destroy (myPageDeck_CurrentImage);
		myPageDeck_CurrentImage = Instantiate (g_setup.myMenuDisplayPrefab, myPageDeck_Image);

		// update the boss name
		myPageDifficulty_Name.text = g_setup.myMenuDisplayName;
		myPageDeck_Name.text = g_setup.myMenuDisplayName;

		//show medals
		MedalType[] t_medals = AdventureSave.LoadMedalTypes (g_setup.myBossType);
		for (int i = 0; i < t_medals.Length; i++) {
			myPageDifficulty_MedalImages [i].color = PT_AdventureMenuCanvas.Instance.GetMedalColor (t_medals [i]);
		}

		// update the Adventure Chess settings
		PT_DeckManager.Instance.SetAdventureChess (g_setup.myBossType);

		SetState (AdventureMenuState.Difficulty);
	}

	public void OnButtonDifficulty (BossDifficulty g_difficulty, Color g_color) {
		// store difficulty
//		myCurrentDifficulty = g_difficulty;

		// change difficulty display
		myPageDeck_DifficultyText.text = g_difficulty.ToString ();
		myPageDeck_DifficultyText.color = g_color;
//		myPageDeck_DifficultyImage.color = g_color;

		// update the boss in deck manager
		PT_DeckManager.Instance.SetAdventureBoss (myCurrentSetup.myBossType, g_difficulty);

		// show the deck
		if (g_difficulty == BossDifficulty.Hard)
			PT_Preset.Instance.Show ();
		else
			PT_Preset.Instance.ShowAdventure ();

		SetState (AdventureMenuState.Deck);
	}

	public void OnButtonStart () {
		TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.StartHost);
	}

	public void OnButtonBack () {
		switch (myState) {
		case AdventureMenuState.AdventureList:
			TransitionManager.Instance.StartTransition (Constants.SCENE_MENU);
			break;
		case AdventureMenuState.Difficulty:
			SetState (AdventureMenuState.AdventureList);
			break;
		case AdventureMenuState.Deck:
			PT_Preset.Instance.Hide ();
			SetState (AdventureMenuState.Difficulty);
			break;
		}
	}

	private void SetState (AdventureMenuState g_state) {
		myState = g_state;
		switch (myState) {
		case AdventureMenuState.AdventureList:
			myCurrentPage = myPageAdventureList;
			break;
		case AdventureMenuState.Difficulty:
			myCurrentPage = myPageDifficulty;
			break;
		case AdventureMenuState.Deck:
			myCurrentPage = myPageDeck;
			break;
		}
	}

	public Color GetMedalColor (MedalType g_medalType) {
		switch (g_medalType) {
		case MedalType.Gold:
			return myMedalColor_Gold;
		case MedalType.Silver:
			return myMedalColor_Silver;
		case MedalType.Bronze:
			return myMedalColor_Bronze;

		case MedalType.None:
		default:
			return myMedalColor_None;
		}
	}

}
