using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChessBank", menuName = "Wield/ChessBank", order = 2)]
public class SO_ChessBank : ScriptableObject {
	public List<ChessInfo> Blood;
	public List<ChessInfo> Magic;
	public List<ChessInfo> Earth;
	public List<ChessInfo> Light;

	public List<ChessInfo> Boss;

	public ChessInfo emptyInfo;

	public List<ChessInfo> GetList (PT_Global.ChessClass g_class) {
		switch (g_class) {
		case PT_Global.ChessClass.Blood:
			return Blood;
		case PT_Global.ChessClass.Magic:
			return Magic;
		case PT_Global.ChessClass.Earth:
			return Earth;
		case PT_Global.ChessClass.Light:
			return Light;
		case PT_Global.ChessClass.Boss:
			return Boss;
		}
		Debug.LogError ("cannot find the list");
		return null;
	}

	public GameObject GetChessPrefab (PT_Global.ChessType g_chessType) {
		ChessInfo t_chessInfo = GetChessInfo (g_chessType);

		if (t_chessInfo.chessType == emptyInfo.chessType)
			return null;
		
		return t_chessInfo.prefab;
	}

	public ChessInfo GetChessInfo (PT_Global.ChessType g_chessType) {
		foreach (ChessInfo f_info in Blood) {
			if (f_info.chessType == g_chessType)
				return f_info;
		}

		foreach (ChessInfo f_info in Magic) {
			if (f_info.chessType == g_chessType)
				return f_info;
		}

		foreach (ChessInfo f_info in Earth) {
			if (f_info.chessType == g_chessType)
				return f_info;
		}

		foreach (ChessInfo f_info in Light) {
			if (f_info.chessType == g_chessType)
				return f_info;
		}

		foreach (ChessInfo f_info in Boss) {
			if (f_info.chessType == g_chessType)
				return f_info;
		}

		return emptyInfo;
	}
}

[System.Serializable]
public struct ChessInfo {
	public PT_Global.ChessType chessType;
	public GameObject prefab;
}