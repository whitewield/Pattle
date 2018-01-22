using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;

public class PT_DeckManager : MonoBehaviour {
	
	private static PT_DeckManager instance = null;

	//========================================================================
	public static PT_DeckManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			LoadChess ();
		}
	}
	//========================================================================

	public SO_ChessBank myChessBank;

	private ChessType[] myChessTypes;
	private ChessType[] myDefaultChessTypes = {
		ChessType.Warrior, ChessType.Archer, ChessType.IceMage
	}; 
	private Vector2[] myChessPositions;
	private Vector2[] myDefaultChessPositions = { new Vector2 (-3, -3), new Vector2 (3, -3), new Vector2 (0, -7) };

	private bool isUsingAdventureChess;
	private ChessType[] myAdventure_ChessTypes;
	private Vector2[] myAdventure_ChessPositions;
	private ChessType myAdventure_BossTypes;
	private Vector2 myAdventure_BossPositions;

	// Use this for initialization
	void Start () {


	}

	private void LoadChess () {
		myChessPositions = new Vector2[Constants.DECK_SIZE];
		myChessTypes = new ChessType[Constants.DECK_SIZE];

		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			//get preset position
			string t_positionString = ShabbySave.LoadGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_POSITION [i]
			);

			if (t_positionString != "0") {
				myChessPositions [i] = Constants.StringToVector2 (t_positionString);
			} else {
				myChessPositions [i] = myDefaultChessPositions [i];
				ShabbySave.SaveGame (
					Constants.SAVE_CATEGORY_PRESET,
					Constants.SAVE_TITLE_PRESET_POSITION [i],
					Constants.Vector2ToString (myChessPositions [i])
				);
			}

			//get preset chess
			string t_chessTypeString = ShabbySave.LoadGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_CHESS [i]
			);
			ChessType t_chessType = (ChessType)(int.Parse (t_chessTypeString));

			if (t_chessType != ChessType.none && myChessTypes.Contains (t_chessType) == false) {
				myChessTypes [i] = t_chessType;
			} else {
				foreach (ChessType f_type in myDefaultChessTypes) {
					t_chessType = f_type;
					if (myChessTypes.Contains (t_chessType) == false) {
						break;
					}
				}
				myChessTypes [i] = t_chessType;
				ShabbySave.SaveGame (
					Constants.SAVE_CATEGORY_PRESET,
					Constants.SAVE_TITLE_PRESET_CHESS [i],
					((int)myChessTypes [i]).ToString ("0")
				);
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetChessTypes (ChessType[] g_types) {
		myChessTypes = g_types;
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			ShabbySave.SaveGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_CHESS [i],
				((int)myChessTypes [i]).ToString ("0")
			);
		}
	}

	public ChessType[] GetChessTypes () {
		return myChessTypes;
	}

	public void SetChessPositions (Vector2[] g_positions) {
		myChessPositions = g_positions;
		for (int i = 0; i < Constants.DECK_SIZE; i++) {
			ShabbySave.SaveGame (
				Constants.SAVE_CATEGORY_PRESET,
				Constants.SAVE_TITLE_PRESET_POSITION [i],
				Constants.Vector2ToString (myChessPositions [i])
			);
		}
	}

	public Vector2[] GetChessPositions () {
		return myChessPositions;
	}
}
