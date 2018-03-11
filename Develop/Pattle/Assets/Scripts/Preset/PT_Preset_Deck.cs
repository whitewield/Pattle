using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;
using UnityEngine.UI;

public class PT_Preset_Deck : MonoBehaviour {
	[SerializeField] PT_Preset_Deck_Slot[] mySlots;
	[SerializeField] GameObject myButtonFace_Edit;
	[SerializeField] GameObject myButtonFace_Confirm;

	public enum ButtonFace {
		Edit,
		Confirm
	}

	// Use this for initialization
	void Start () {
		SetFromDeckManager ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetFromDeckManager () {
		ChessType[] t_chessTypes = PT_DeckManager.Instance.GetArenaChessTypes ();
		for (int i = 0; i < mySlots.Length; i++) {
			mySlots [i].SetChessInfo (PT_DeckManager.Instance.myChessBank.GetChessInfo (t_chessTypes [i]));
		}
	}

	public void ApplyChessType () {
		ChessType[] t_typeArray = new ChessType[Constants.DECK_SIZE];
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			t_typeArray [i] = mySlots [i].GetChessType ();
		}
		PT_DeckManager.Instance.SetChessTypes (t_typeArray);
	}

	public Sprite[] GetSprites () {
		Sprite[] t_sprites = new Sprite[Constants.DECK_SIZE];
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			t_sprites [i] = mySlots [i].GetSprite ();
		}
		return t_sprites;
	}

	public bool CheckReady () {
		foreach (PT_Preset_Deck_Slot f_slot in mySlots) {
			if (f_slot.GetChessType () == ChessType.none)
				return false;
		}
		return true;
	}

	public void SetButtonFace (ButtonFace g_face) {
		switch (g_face) {
		case ButtonFace.Edit:
			myButtonFace_Confirm.SetActive (false);
			myButtonFace_Edit.SetActive (true);
			break;
		case ButtonFace.Confirm:
			myButtonFace_Confirm.SetActive (true);
			myButtonFace_Edit.SetActive (false);
			break;
		}
	}
}
