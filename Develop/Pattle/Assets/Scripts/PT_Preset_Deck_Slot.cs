using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Deck_Slot : MonoBehaviour {
	[SerializeField] int myIndex;
	public ChessInfo myChessInfo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetChessInfo (ChessInfo g_info) {
		myChessInfo = g_info;
		if (g_info.prefab == null)
			this.GetComponent<SpriteRenderer> ().sprite = null;
		else
			this.GetComponent<SpriteRenderer> ().sprite = myChessInfo.prefab.GetComponent<SpriteRenderer> ().sprite;
	}
}
