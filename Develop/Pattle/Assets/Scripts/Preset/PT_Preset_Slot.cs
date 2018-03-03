using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;

public class PT_Preset_Slot : MonoBehaviour {
	protected ChessInfo myChessInfo;

	protected SpriteRenderer mySpriteRenderer;

	public Sprite GetSprite () {
		if (myChessInfo.chessType == ChessType.none)
			return null;
		return myChessInfo.prefab.GetComponent<SpriteRenderer> ().sprite;
	}

	public ChessType GetChessType () {
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
