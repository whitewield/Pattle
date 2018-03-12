using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;
using Hang.AiryAudio;

public class PT_EndGameUI : MonoBehaviour {

	[SerializeField] GameObject myVictoryDisplay;
	[SerializeField] GameObject myDefeatDisplay;
	[SerializeField] SpriteRenderer[] myChessDisplays;

	[SerializeField] AiryAudioData myAiryAudioData_Win;
	[SerializeField] AiryAudioData myAiryAudioData_Lose;

	void Start () {

		if (PT_DeckManager.Instance.IsWinning) {
			myAiryAudioData_Win.Play ();

			myVictoryDisplay.SetActive (true);
			myDefeatDisplay.SetActive (false);
		} else {
			myAiryAudioData_Lose.Play ();

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
