﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;

public class PT_DeckManager : MonoBehaviour {
	// singleton
	private static PT_DeckManager instance = null;
	public static PT_DeckManager Instance { get { return instance; } }

	// game mode
	private GameMode myGameMode = GameMode.Arena;

	// the scriptable obejct for chess bank
	public SO_ChessBank myChessBank;

	// chess type and position for arena
	private ChessType[] myChessTypes;
	private ChessType[] myDefaultChessTypes = { ChessType.Warrior, ChessType.Archer, ChessType.IceMage };
	private Vector2[] myChessPositions;
	private Vector2[] myDefaultChessPositions = { new Vector2 (-3, -3), new Vector2 (3, -3), new Vector2 (0, -7) };

	// chess type and position for adventure mode
	private ChessType[] myAdventure_ChessTypes;
	private Vector2[] myAdventure_ChessPositions;
	private BossType myAdventure_BossType;
	private BossDifficulty myAdventure_BossDifficulty;

	// winning or not
	private bool isWinning = false;
	public bool IsWinning { get { return isWinning; } set { isWinning = value; } }

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
			LoadChess ();
		}
	}

	/// <summary>
	/// Loads the chess information from save file
	/// </summary>
	private void LoadChess () {
		myChessPositions = new Vector2[Constants.DECK_SIZE];
		myChessTypes = new ChessType[Constants.DECK_SIZE];

		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			//get preset position
			string t_positionString = ShabbySave.LoadGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_POSITION[i]
			);

			if (t_positionString != "0") {
				myChessPositions[i] = Constants.StringToVector2 (t_positionString);
			} else {
				myChessPositions[i] = myDefaultChessPositions[i];
				ShabbySave.SaveGame (
					Constants.SAVE_CATEGORY_PRESET,
					Constants.SAVE_TITLE_PRESET_POSITION[i],
					Constants.Vector2ToString (myChessPositions[i])
				);
			}

			//get preset chess
			string t_chessTypeString = ShabbySave.LoadGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_CHESS[i]
			);
			ChessType t_chessType = (ChessType)(int.Parse (t_chessTypeString));

			if (t_chessType != ChessType.none && myChessTypes.Contains (t_chessType) == false) {
				myChessTypes[i] = t_chessType;
			} else {
				foreach (ChessType f_type in myDefaultChessTypes) {
					t_chessType = f_type;
					if (myChessTypes.Contains (t_chessType) == false) {
						break;
					}
				}
				myChessTypes[i] = t_chessType;
				ShabbySave.SaveGame (
					Constants.SAVE_CATEGORY_PRESET,
					Constants.SAVE_TITLE_PRESET_CHESS[i],
					((int)myChessTypes[i]).ToString ("0")
				);
			}
		}
	}

	#region get set value 

	// set the game mode
	public void SetGameMode (GameMode g_mode) {
		myGameMode = g_mode;
		//if (myGameMode == GameMode.Arena) {
		//}
	}

	// get the game mode
	public GameMode GetGameMode () {
		return myGameMode;
	}

	// set chess type
	public void SetChessTypes (ChessType[] g_types) {
		myChessTypes = g_types;
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			ShabbySave.SaveGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_CHESS[i],
				((int)myChessTypes[i]).ToString ("0")
			);
		}
	}

	// get chess type
	public ChessType[] GetChessTypes () {
		if (myGameMode == GameMode.Adventure &&
			(myAdventure_BossDifficulty == BossDifficulty.Easy || myAdventure_BossDifficulty == BossDifficulty.Normal))
			return myAdventure_ChessTypes;

		return myChessTypes;
	}

	// set chess position
	public void SetChessPositions (Vector2[] g_positions) {
		myChessPositions = g_positions;
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			ShabbySave.SaveGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_POSITION[i],
				Constants.Vector2ToString (myChessPositions[i])
			);
		}
	}

	// get chess position
	public Vector2[] GetChessPositions () {
		if (myGameMode == GameMode.Adventure &&
			(myAdventure_BossDifficulty == BossDifficulty.Easy || myAdventure_BossDifficulty == BossDifficulty.Normal))
			return myAdventure_ChessPositions;

		//		Debug.LogWarning("normal chess pos");
		return myChessPositions;
	}

	// set adventure boss
	public void SetAdventureBoss (BossType g_type, BossDifficulty g_difficulty) {
		if (g_type == BossType.none) {
			Debug.LogWarning ("setting boss type to none?!");
		}
		myAdventure_BossType = g_type;
		myAdventure_BossDifficulty = g_difficulty;
	}

	// get adventure difficulty
	public BossDifficulty GetAdventureDifficulty () {
		return myAdventure_BossDifficulty;
	}

	// set adventure chess
	public void SetAdventureChess (BossType g_bossType) {
		SO_AdventureChessSettings t_setting = myChessBank.GetAdventureChessSettings (g_bossType);
		if (t_setting == null)
			return;

		myAdventure_ChessTypes = t_setting.chessTypes;
		myAdventure_ChessPositions = t_setting.chessPositions;
	}

	/// get adventure chess types.
	public ChessType[] GetAdventureChessTypes () {
		return myAdventure_ChessTypes;
	}

	// get adventure chess position
	public Vector2[] GetAdventureChessPositions () {
		return myAdventure_ChessPositions;
	}

	// get adventure boss type
	public BossType GetAdventureBossType () {
		return myAdventure_BossType;
	}

	// get adventure boss prefab
	public GameObject GetAdventureBossPrefab () {
		return myChessBank.GetBossPrefab (myAdventure_BossType, myAdventure_BossDifficulty);
	}

	// get adventure map prefab
	public GameObject GetAdventureMapPrefab () {
		return myChessBank.GetMapPrefab (myAdventure_BossType);
	}

	// get arena chess type 
	public ChessType[] GetArenaChessTypes () {
		return myChessTypes;
	}

	// get arena chess position
	public Vector2[] GetArenaChessPositions () {
		return myChessPositions;
	}

	#endregion
}