using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;

public class PT_Preset_Info : MonoBehaviour {
	[SerializeField] SpriteRenderer mySpriteRenderer_Chess;
	[SerializeField] TextMesh myTextMesh_Info;
	[SerializeField] TextMesh myTextMesh_HP;
	[SerializeField] TextMesh myTextMesh_PD;
	[SerializeField] TextMesh myTextMesh_PR;
	[SerializeField] TextMesh myTextMesh_MD;
	[SerializeField] TextMesh myTextMesh_CT;
	[SerializeField] TextMesh myTextMesh_CD;
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
		myTextMesh_Info.text = PT_Caption.Instance.LoadCaption (Constants.CAPTION_CHESSABILITY, g_chessInfo.chessType.ToString ());

		SO_Attributes t_attributes = g_chessInfo.prefab.GetComponent<PT_BaseChess> ().GetAttributes ();
		myTextMesh_HP.text = t_attributes.HP.ToString ("0");
		myTextMesh_PD.text = t_attributes.PD.ToString ("0");
		myTextMesh_PR.text = t_attributes.PR.ToString ("0");
		myTextMesh_MD.text = t_attributes.MD.ToString ("0");
		myTextMesh_CT.text = t_attributes.CT.ToString ("0");
		myTextMesh_CD.text = t_attributes.CD.ToString ("0");
	}
}
