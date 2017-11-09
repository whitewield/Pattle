using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Deck : MonoBehaviour {
	[SerializeField] PT_Preset_Deck_Slot[] mySlots;
	// Use this for initialization
	void Start () {
		SetFromDeckManager ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetFromDeckManager () {
		PT_Global.ChessType[] g_chessTypes = PT_DeckManager.Instance.GetChessTypes ();
		for (int i = 0; i < mySlots.Length; i++) {
			mySlots [i].SetChessInfo (PT_DeckManager.Instance.myChessBank.GetChessInfo (g_chessTypes [i]));
		}

//		PT_Preset.Instance.myCollection
	}
}
