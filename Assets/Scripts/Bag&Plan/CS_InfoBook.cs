using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_InfoBook : MonoBehaviour {

	public GameObject btn_Buy;

	public SpriteRenderer sr_Chess;
	public TextMesh tm_HP;
	public TextMesh tm_Atk;
	public TextMesh tm_Def;
	public TextMesh tm_SP;
	public TextMesh tm_CT;
	public TextMesh tm_CD;
	public GameObject TX_Info;
	public GameObject TX_Story;


	// Use this for initialization
	void Start () {
		Hide ();
	}

	void OnMouseDown() {
		Hide ();
	}

	public void Hide () {
		btn_Buy.SetActive (false);
		this.gameObject.SetActive (false);
	}

	public void Show (GameObject g_subChess) {
		GameObject t_chess = g_subChess.GetComponent<CS_SubChess> ().myChess;
		sr_Chess.sprite = t_chess.GetComponent<SpriteRenderer> ().sprite;
		tm_HP.text = t_chess.GetComponent<CS_Chess> ().at_HP.ToString ();
		tm_Atk.text = t_chess.GetComponent<CS_Chess> ().at_PDM.ToString ();
		tm_Def.text = t_chess.GetComponent<CS_Chess> ().at_PDF.ToString ();
		tm_SP.text = t_chess.GetComponent<CS_Chess> ().at_MDM.ToString ();
		tm_CT.text = t_chess.GetComponent<CS_Chess> ().at_CT.ToString ();
		tm_CD.text = t_chess.GetComponent<CS_Chess> ().at_CD.ToString ();
		TX_Info.SendMessage ("SetTitle", g_subChess.name);
		TX_Story.SendMessage ("SetTitle", g_subChess.name);
		//don't have the chess, you can buy
		if (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_CHESS, g_subChess.name) == "0") {
			btn_Buy.SetActive (true);
			int t_coinsAmount = int.Parse (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_BAG, CS_Global.SAVE_TITLE_COINS));
			int t_price = CS_StoreList.GetChessPrice (g_subChess.name);
			btn_Buy.SendMessage ("SetPrice", t_price);
			//if don't have the money can't buy
			if (t_coinsAmount < t_price) {
				btn_Buy.SendMessage ("Off");
			} else {
				btn_Buy.SendMessage ("On", g_subChess.name);
			}
		}
	}


}
