using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_AdventureDeck : MonoBehaviour {
	[SerializeField] PT_Preset_Deck_Slot[] mySlots;
	// Use this for initialization
	void Start () {
		SetFromDeckManager ();
	}

	public void SetFromDeckManager () {
		PT_Global.ChessType[] t_chessTypes = PT_DeckManager.Instance.GetAdventureChessTypes ();
		if (t_chessTypes == null)
			return;
		for (int i = 0; i < mySlots.Length; i++) {
			mySlots [i].SetChessInfo (PT_DeckManager.Instance.myChessBank.GetChessInfo (t_chessTypes [i]));
		}
	}
}
