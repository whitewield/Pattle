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
	private BossDifficulty myCurrentDifficulty;

	[Header ("AdventureList")]
	[SerializeField] RectTransform myPageAdventureList;
	[SerializeField] RectTransform myPageAdventureList_Content;
	[SerializeField] Vector2 myPageAdventureList_Content_Height;//x -> the base height, y -> the spacing
	[SerializeField] GameObject[] myPageAdventureList_ButtonPrefabs;
	[SerializeField] GameObject myMedalsPrefab;
	public GameObject MedalsPrefab { get { return myMedalsPrefab; } }

	[Header ("Difficulty")]
	[SerializeField] RectTransform myPageDifficulty;
	[SerializeField] Text myPageDifficulty_Name;
	[SerializeField] RectTransform myPageDifficulty_Image;
	private GameObject myPageDifficulty_CurrentImage;

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
		Init_AdventureList ();

		SetState (AdventureMenuState.AdventureList);
	}

	private void Init_AdventureList () {
		float t_height = myPageAdventureList_Content_Height.x;

		foreach (GameObject f_prefab in myPageAdventureList_ButtonPrefabs) {
			RectTransform t_rectTransform = Instantiate (f_prefab, myPageAdventureList_Content).transform as RectTransform;
			t_height += t_rectTransform.sizeDelta.y;
			t_height += myPageAdventureList_Content_Height.y;
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

		// update the Adventure Chess settings
		PT_DeckManager.Instance.SetAdventureChess (g_setup.myBossType);

		SetState (AdventureMenuState.Difficulty);
	}

	public void OnButtonDifficulty (BossDifficulty g_difficulty, Color g_color) {
		// store difficulty
		myCurrentDifficulty = g_difficulty;

		// change difficulty display
		myPageDeck_DifficultyText.text = g_difficulty.ToString ();
		myPageDeck_DifficultyImage.color = g_color;

		// show deck
		if (myCurrentDifficulty == BossDifficulty.Hard) {
			PT_Preset.Instance.Show ();
			PT_DeckManager.Instance.UseAdventureChess (false);
		}else {
			PT_Preset.Instance.ShowAdventure ();
			PT_DeckManager.Instance.UseAdventureChess (true);
		}

		// update the boss in deck manager
		PT_DeckManager.Instance.SetAdventureBoss (myCurrentSetup.myBossType, g_difficulty);

		SetState (AdventureMenuState.Deck);
	}

	public void OnButtonStart () {
		NetworkManager.singleton.matchSize = 1;
		NetworkManager.singleton.onlineScene = "NetworkAdventure";
		NetworkManager.singleton.offlineScene = "NetworkAdventureMenu";
		NetworkManager.singleton.StartHost ();
	}

	public void OnButtonBack () {
		switch (myState) {
		case AdventureMenuState.AdventureList:
			TransitionManager.Instance.StartTransition ("NetworkMenu");
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

}
