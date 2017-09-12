using UnityEngine;
using System.Collections;

public static class CS_Global {

	//the max chess for each side in battle
	public static int NUMBER_CHESS = 3;
	public static int NUMBER_ADVENTUREPAGE = 4;

	public static float TIME_COUNTDOWN = 5.0f;
	public static float TIME_SHOWINFO = 0.5f;

	public static float DISTANCE_RESET = 0.1f;
	public static float DISTANCE_DRAG = 100.0f;
	
	public static float SPEED_MOVE = 4.0f;

	public static int PS_NULL = -1;
	public static int PS_IDLE = 0;
	public static int PS_MOVE = 1;
	public static int PS_CD = 4;
	public static int PS_CT = 5;
	public static int PS_ATTACK = 2;
	public static int PS_ATTACKBACK = 3;
	public static int PS_DEAD = 9;

	public static string NAME_MESSAGEBOX = "MessageBox";
	public static string NAME_MESSAGEBOX_LOADSCENE = "LoadScene";
	public static string NAME_MESSAGEBOX_ADVENTUREVICTORY = "AdventureVictory";
	public static string NAME_CAPTION = "Caption";
	public static string NAME_INPUTMANAGER = "InputManager";
	public static string NAME_TRANSITIONMANAGER = "TransitionManager";
	public static string NAME_TRANSITIONMANAGER_SAO = "StartAnimationOut";
	public static string NAME_SELECT = "Select";
	public static string NAME_TEXT_HP = "TX_HP";
	public static string NAME_TEXT_DAMAGE = "TX_Damage";
	public static string NAME_AFTERBATTLE = "AfterBattle";
	public static string NAME_INFOBOOK = "InfoBook";
	public static string NAME_SUBCHESS = "SubChess";
	public static string NAME_BAGCOINSAMOUNT = "BagCoinsAmount";
	public static string NAME_ADVENTUREBOOK = "AdventureBook";

	public static string SCENE_BATTLE = "Battle";

	public static string SL_DEADBODY = "DeadBody";
	public static string SL_CHESS = "Chess";

	public static string TAG_MAP = "Map";
	public static string TAG_FB = "FB";
	public static string TAG_FA = "FA";
	public static string TAG_B = "B";
	public static string TAG_A = "A";
	public static string TAG_SUBCHESS = "SubChess";
	public static string TAG_BTNCHESS = "BtnChess";

	public static string CLASS_BLOOD = "Blood";
	public static string CLASS_MAGIC = "Magic";
	public static string CLASS_NATURE = "Nature";
	public static string CLASS_LIGHT = "Light";

	public static Vector3 POS_DOWN_CENTER = new Vector3 (0, -5, 0);

	public static float CAMERA_SIZE = 10.24f;
	public static float CHESS_SIZE_S = 1.28f;
	public static float CHESS_SIZE_M = 1.92f;
	public static float CHESS_SIZE_L = 2.56f;

	public static string STAR_UNDONE = "0";
	public static string STAR_DONE = "1";
	public static string STAR_PERFECT = "2";
	public static string STAR_DONE_HARD = "3";
	public static string STAR_PERFECT_HARD = "4";

	public static string SAVE_CATEGORY_SETTINGS = "Settings";
	public static string SAVE_TITLE_LANGUAGE = "language";
	public static string SAVE_CATEGORY_ADVENTURE = "Adventure";
	public static string SAVE_CATEGORY_CHESS = "Chess";
	public static string SAVE_CATEGORY_BAG = "Bag";
	public static string SAVE_CATEGORY_COUPON = "Coupon";
	public static string SAVE_TITLE_COINS = "Coins";

	public static string LANGUAGE_DEFAULT = "CHS";
	public static string LANGUAGE_CHS = "CHS";
	public static string LANGUAGE_CHT = "CHT";
	public static string LANGUAGE_EN = "EN";

	public static Vector3 POSITION_SKILL = Vector3.forward * 5;

	public static int DAMAGE_FISH = 4;
	public static int ST_FISH_YIN = 1;
	public static int ST_FISH_YANG = 2;

	public static string GetMyEnemyTag (string g_myTag) {
		int t_myNum = 0;
		if (g_myTag == TAG_A)
			t_myNum = 1;
		else if (g_myTag == TAG_B)
			t_myNum = 2;

		int t_myEnemyNum = 3 - t_myNum;
		if (t_myEnemyNum == 1)
			return TAG_A;
		else if (t_myEnemyNum == 2)
			return TAG_B;
		else
			return "";
	}

}


