using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;

public class PT_Preset_Field : MonoBehaviour {
	[SerializeField] PT_Preset_Field_Chess[] myChesses;

	// Use this for initialization
	void Start () {
		SetPositionFromDeckManager ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSprites (Sprite[] g_sprites) {
		for (int i = 0; i < myChesses.Length; i++) {
			myChesses [i].SetSprite (g_sprites [i]);
		}
	}

	public void SetPositionFromDeckManager () {
		Vector2[] t_chessPositions = PT_DeckManager.Instance.GetArenaChessPositions ();
		for (int i = 0; i < myChesses.Length; i++) {
			myChesses [i].transform.transform.localPosition = 
				new Vector3 (t_chessPositions [i].x, -Mathf.Abs (t_chessPositions [i].y));
		}
	}

	public void ApplyChessPosition () {
		Vector2[] t_posArray = new Vector2[Constants.DECK_SIZE];
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			t_posArray [i] = myChesses [i].transform.localPosition;
		}
		PT_DeckManager.Instance.SetChessPositions (t_posArray);
	}
}
