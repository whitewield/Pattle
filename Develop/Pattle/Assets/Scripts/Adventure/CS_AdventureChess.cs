using UnityEngine;
using System.Collections;

public class CS_AdventureChess : MonoBehaviour {

	public string myName;
	public bool isTutorial;
	public float mySpriteSize = 1;
	public GameObject myChess;
	public GameObject myBoss;
	public GameObject myBossHard;
	public GameObject myMap;
	[SerializeField] GameObject myStar;
	private GameObject myPrivateStar;

	public GameObject myAdventureBook;

	void Start () {
		myPrivateStar = Instantiate (myStar) as GameObject;

		myPrivateStar.GetComponent<CS_AdventureStar> ().SetStarStatus (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_ADVENTURE, myName));
		myPrivateStar.GetComponent<CS_AdventureStar> ().SetTransform (this.transform);
		myPrivateStar.SetActive (true);
	}

	void OnMouseDown() {
//		Debug.Log("onBtn");
		myAdventureBook.SetActive (true);
		myAdventureBook.GetComponent<CS_AdventureBook> ().Show (this.gameObject);
	}
}
