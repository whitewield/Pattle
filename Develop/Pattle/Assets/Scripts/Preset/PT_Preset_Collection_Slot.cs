using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Collection_Slot : PT_Preset_Slot {
	private bool inUse;

	public void Init (ChessInfo g_info) {

		SetupSpriteRenderer ();
		
		myChessInfo = g_info;
		if (g_info.prefab == null)
			mySpriteRenderer.sprite = null;
		else
			mySpriteRenderer.sprite = myChessInfo.prefab.GetComponent<SpriteRenderer> ().sprite;
	}

	public bool GetInUse () {
		return inUse;
	}

	public void SetInUse (bool g_inUse) {
		inUse = g_inUse;
		if (inUse) {
			mySpriteRenderer.color = PT_Global.Constants.COLOR_CHESS_INACTIVE;
		} else {
			mySpriteRenderer.color = PT_Global.Constants.COLOR_CHESS_ACTIVE;
		}
	}

}
