using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pattle.Global;
using Hang.AiryAudio;

public class PT_EndGameUI : MonoBehaviour {

	[SerializeField] GameObject myVictoryDisplay;
	[SerializeField] GameObject myDefeatDisplay;
	[SerializeField] SpriteRenderer[] myChessDisplays;
	[SerializeField] Button myButton_Restart;

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

		if (PT_DeckManager.Instance.GetGameMode () == GameMode.Arena) {
			myButton_Restart.interactable = false;
		} else {
			myButton_Restart.interactable = true;
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

	public void OnButtonRestart () {
		TransitionManager.Instance.StartTransition (TransitionManager.TransitionMode.StartHost);
	}
}
