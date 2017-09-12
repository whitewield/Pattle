using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_InfoBook_S : MonoBehaviour {

	public GameObject info_Page;

	public SpriteRenderer sr_Chess;
	public TextMesh tm_HP;
	public TextMesh tm_Atk;
	public TextMesh tm_Def;
	public TextMesh tm_SP;
	public TextMesh tm_CT;
	public TextMesh tm_CD;
	public CS_Text TX_Info;

	private float timer = CS_Global.TIME_SHOWINFO;


	// Use this for initialization
	void Start () {
		Hide ();
	}

	void Update () {
		
		if (timer <= 0)
			return;

		//update timer
		timer -= Time.deltaTime;

		//if timer first time < 0
		if(timer <= 0)
			info_Page.gameObject.SetActive (true);
	}

	public void Hide () {
		//set timer, not to update
		timer = 0;
		//hide info page
		info_Page.gameObject.SetActive (false);
	}

	public void Show (GameObject g_subChess) {
		//set timer, start update
		timer = CS_Global.TIME_SHOWINFO;

		//set infomation
		GameObject t_chess = g_subChess.GetComponent<CS_SubChess> ().myChess;
		sr_Chess.sprite = t_chess.GetComponent<SpriteRenderer> ().sprite;
		tm_HP.text = t_chess.GetComponent<CS_Chess> ().at_HP.ToString ();
		tm_Atk.text = t_chess.GetComponent<CS_Chess> ().at_PDM.ToString ();
		tm_Def.text = t_chess.GetComponent<CS_Chess> ().at_PDF.ToString ();
		tm_SP.text = t_chess.GetComponent<CS_Chess> ().at_MDM.ToString ();
		tm_CT.text = t_chess.GetComponent<CS_Chess> ().at_CT.ToString ();
		tm_CD.text = t_chess.GetComponent<CS_Chess> ().at_CD.ToString ();
		TX_Info.SetTitle (g_subChess.name);
	}


}
