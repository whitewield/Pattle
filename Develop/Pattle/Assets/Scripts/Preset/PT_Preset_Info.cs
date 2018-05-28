using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pattle.Global;

public class PT_Preset_Info : MonoBehaviour {
	[SerializeField] SpriteRenderer mySpriteRenderer_Chess;
	[SerializeField] PT_Text myText_Name;
	[SerializeField] PT_Text myText_Info;
	[SerializeField] Text myText_HP;
	[SerializeField] Text myText_PD;
	[SerializeField] Text myText_PR;
	[SerializeField] Text myText_MD;
	[SerializeField] Text myText_CT;
	[SerializeField] Text myText_CD;
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void ShowInfo (ChessInfo g_chessInfo) {
		mySpriteRenderer_Chess.sprite = g_chessInfo.prefab.GetComponent<SpriteRenderer> ().sprite;
		myText_Name.SetText (
			PT_Caption.Instance.LoadCaption (Constants.CAPTION_CHESSNAME, g_chessInfo.chessType.ToString ())
		);
		myText_Info.SetText (
			PT_Caption.Instance.LoadCaption (Constants.CAPTION_CHESSABILITY, g_chessInfo.chessType.ToString ())
		);

		SO_Attributes t_attributes = g_chessInfo.prefab.GetComponent<PT_BaseChess> ().GetAttributes ();
		myText_HP.text = t_attributes.HP.ToString ("0");
		myText_PD.text = t_attributes.PD.ToString ("0");
		myText_PR.text = t_attributes.PR.ToString ("0");
		myText_MD.text = t_attributes.MD.ToString ("0");
		myText_CT.text = t_attributes.CT.ToString ("0");
		myText_CD.text = t_attributes.CD.ToString ("0");
	}
}
