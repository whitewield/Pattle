using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Deck_Slot : PT_Preset_Slot {
	[SerializeField] int myIndex;

	void Awake () {
		SetupSpriteRenderer ();
	}

	public void SetChessInfo (ChessInfo g_info) {
		myChessInfo = g_info;
		if (g_info.prefab == null)
			mySpriteRenderer.sprite = null;
		else
			mySpriteRenderer.sprite = myChessInfo.prefab.GetComponent<SpriteRenderer> ().sprite;
	}

	public void RemoveSprite () {
		mySpriteRenderer.sprite = null;
	}
}
