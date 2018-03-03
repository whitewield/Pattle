using UnityEngine;
using System.Collections;
using Pattle.Global;

[CreateAssetMenu(fileName = "AdventureChessSettings", menuName = "Wield/AdventureChessSettings", order = 1)]
public class SO_AdventureChessSettings : ScriptableObject {

	public ChessType[] chessTypes = new ChessType[3];
	public Vector2[] chessPositions = new Vector2[3];

}