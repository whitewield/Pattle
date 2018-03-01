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
		PT_Global.ChessType[] t_chessTypes = PT_DeckManager.Instance.GetArenaChessTypes ();
		for (int i = 0; i < mySlots.Length; i++) {
			mySlots [i].SetChessInfo (PT_DeckManager.Instance.myChessBank.GetChessInfo (t_chessTypes [i]));
		}
	}

	public void ApplyChessType () {
		PT_Global.ChessType[] t_typeArray = new PT_Global.ChessType[PT_Global.Constants.DECK_SIZE];
		for (int i = 0; i < PT_Global.Constants.DECK_SIZE; i++) {
			t_typeArray [i] = mySlots [i].GetChessType ();
		}
		PT_DeckManager.Instance.SetChessTypes (t_typeArray);
	}

	public Sprite[] GetSprites () {
		Sprite[] t_sprites = new Sprite[PT_Global.Constants.DECK_SIZE];
		for (int i = 0; i < PT_Global.Constants.DECK_SIZE; i++) {
			t_sprites [i] = mySlots [i].GetSprite ();
		}
		return t_sprites;
	}

	public bool CheckReady () {
		foreach (PT_Preset_Deck_Slot f_slot in mySlots) {
			if (f_slot.GetChessType () == PT_Global.ChessType.none)
				return false;
		}
		return true;
	}

}
