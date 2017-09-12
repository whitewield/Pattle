using UnityEngine;
using System.Collections;

public class CS_SubChess : MonoBehaviour {

	public string ID;
	public GameObject myChess;

	void Start () {
		UpdateSprite ();
	}

	public void UpdateSprite () {
		if (this.name == CS_Global.NAME_SUBCHESS) {
			this.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_CHESS, this.name) == "1") {
			this.GetComponent<SpriteRenderer>().color = Color.white;
		} else {
			this.GetComponent<SpriteRenderer>().color = Color.gray;
		}
	}

	public void InitChess (string g_tag) {
		//create Chess
		//Debug.Log (g_tag);
		GameObject t_chess = Instantiate (myChess, this.transform.position, Quaternion.identity) as GameObject;
		t_chess.SendMessage ("SetTag", g_tag);
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("AddChess", t_chess);
	}
}
