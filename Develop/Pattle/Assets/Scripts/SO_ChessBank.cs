using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PT_Global;

[CreateAssetMenu(fileName = "ChessBank", menuName = "Wield/ChessBank", order = 2)]
public class SO_ChessBank : ScriptableObject {
	[Header ("Chess")]
	public List<ChessInfo> Blood;
	public List<ChessInfo> Magic;
	public List<ChessInfo> Earth;
	public List<ChessInfo> Light;
	public ChessInfo emptyInfo;

	[Header ("Boss")]
	public List<BossInfo> BossList;
	public BossInfo emptyBossInfo;

	public List<ChessInfo> GetList (ChessClass g_class) {
		switch (g_class) {
		case ChessClass.Blood:
			return Blood;
		case ChessClass.Magic:
			return Magic;
		case ChessClass.Earth:
			return Earth;
		case ChessClass.Light:
			return Light;
		}
		Debug.LogError ("cannot find the list");
		return null;
	}

	public List<BossInfo> GetBossList () {
		return BossList;
	}

	public GameObject GetChessPrefab (ChessType g_chessType) {
		ChessInfo t_chessInfo = GetChessInfo (g_chessType);

		if (t_chessInfo.chessType == emptyInfo.chessType)
			return null;
		
		return t_chessInfo.prefab;
	}

	public GameObject GetBossPrefab (BossType g_bossType, BossDifficulty g_bossDifficulty) {
		BossInfo t_bossInfo = GetBossInfo (g_bossType);

		if (t_bossInfo.bossType == emptyBossInfo.bossType)
			return null;

		switch (g_bossDifficulty) {
		case BossDifficulty.Easy:
			return t_bossInfo.prefabEasy;
		case BossDifficulty.Normal:
			return t_bossInfo.prefabNormal;
		case BossDifficulty.Hard:
			return t_bossInfo.prefabHard;
		default:
			Debug.LogError ("Boss Difficulty not found!");
			return t_bossInfo.prefabEasy;
		}
	}

	public ChessInfo GetChessInfo (ChessType g_chessType) {
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

		return emptyInfo;
	}

	public BossInfo GetBossInfo (BossType g_bossType) {
		foreach (BossInfo f_info in BossList) {
			if (f_info.bossType == g_bossType)
				return f_info;
		}

		return emptyBossInfo;
	}

	public SO_AdventureChessSettings GetAdventureChessSettings (BossType g_bossType) {
		BossInfo t_bossInfo = GetBossInfo (g_bossType);

		return t_bossInfo.chessSettings;
	}
}

[System.Serializable]
public struct ChessInfo {
	public ChessType chessType;
	public GameObject prefab;
}

[System.Serializable]
public struct BossInfo {
	public BossType bossType;
	public GameObject prefabEasy;
	public GameObject prefabNormal;
	public GameObject prefabHard;
	public SO_AdventureChessSettings chessSettings;
}