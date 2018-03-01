using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;

public class PT_EndGameUI : MonoBehaviour {

	[SerializeField] GameObject myVictoryDisplay;
	[SerializeField] GameObject myDefeatDisplay;
	[SerializeField] SpriteRenderer[] myChessDisplays;

	void Start () {

		if (PT_DeckManager.Instance.IsWinning) {
			myVictoryDisplay.SetActive (true);
			myDefeatDisplay.SetActive (false);
		} else {
			myVictoryDisplay.SetActive (false);
			myDefeatDisplay.SetActive (true);
		}
			

		// show chess sprites
		ChessType[] t_chessTypes = PT_DeckManager.Instance.GetChessTypes ();
		for (int i = 0; i < t_chessTypes.Length; i++) {
			myChessDisplays [i].sprite = 
				PT_DeckManager.Instance.myChessBank.GetChessPrefab (t_chessTypes [i]).GetComponent<SpriteRenderer> ().sprite;
		}
	}

	public void OnButtonConfirm () {
		GameMode t_mode = PT_DeckManager.Instance.GetGameMode ();

		switch (t_mode){
		case GameMode.Adventure:
			TransitionManager.Instance.StartTransition (Constants.SCENE_ADVENTUREMENU);
			break;
		case GameMode.Arena:
			TransitionManager.Instance.StartTransition (Constants.SCENE_LOBBY);
			break;
		}
	}
}
