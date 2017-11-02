using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChessBank", menuName = "Wield/ChessBank", order = 2)]
public class SO_ChessBank : ScriptableObject {
	public List<ChessInfo> myBank;

	public GameObject GetChessPrefab (PT_Global.ChessType g_chessType) {
		foreach (ChessInfo f_info in myBank) {
			if (f_info.chessType == g_chessType)
				return f_info.prefab;
		}
		return null;
	}
}

[System.Serializable]
public struct ChessInfo {
	public PT_Global.ChessType chessType;
	public GameObject prefab;
}