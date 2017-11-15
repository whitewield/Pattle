using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Slot : MonoBehaviour {
	protected ChessInfo myChessInfo;

	protected SpriteRenderer mySpriteRenderer;

	public Sprite GetSprite () {
		if (myChessInfo.chessType == PT_Global.ChessType.none)
			return null;
		return myChessInfo.prefab.GetComponent<SpriteRenderer> ().sprite;
	}

	public PT_Global.ChessType GetChessType () {
		return myChessInfo.chessType;
	}

	public ChessInfo GetChessInfo () {
		return myChessInfo;
	}

	protected void SetupSpriteRenderer () {
		if (mySpriteRenderer == null) {
			mySpriteRenderer = this.GetComponent<SpriteRenderer> ();
		}
	}
}
