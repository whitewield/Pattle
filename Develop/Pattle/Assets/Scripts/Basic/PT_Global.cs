using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pattle{
	namespace Global{

		public enum GameMode {
			Adventure,
			Arena,
		}

		public enum ChessClass {
			Start = 0,

			Blood = 1,

			Magic = 2,

			Earth = 3,

			Light = 4,

			End,
		}

		public enum ChessType {
			none = 0,
			___Blood___ = 100,
			Warrior = 101,
			Vampire = 102,
			Berserker = 103,
			Sword = 104,
			___Magic___ = 200,
			IceMage = 201,
			FireMage = 202,
			LightMage = 203,
			AirMage = 204,
			___Earth___ = 300,
			Archer = 301,
			Hunter = 302,
			Soprano = 303,
			Jam = 304,
			___Light___ = 400,
			Paladin = 401,
			Angel = 402,
			Avalok = 403,
			Neptu = 404,
		}

		public enum BossType {
			none = 0,
			FireDragon = 1,
		}

		public enum MedalType {
			None = 0,
			Bronze = 1,
			Silver = 2,
			Gold = 3,
		}

		public enum BossDifficulty {
			Easy = 0,
			Normal = 1,
			Hard = 2
		}

		public enum Process {
			None = -1,
			Idle,
			Move,
			CD,
			CT,
			Attack,
			AttackBack,
			Dead
		}

		public enum Status {
			Freeze,
			Gold,
			SpellImmune,
			Bubble,

			FishYin,
			FishYang,
			End
		}

		public enum HPModifierType {
			PhysicalDamage,
			MagicDamage,
			Healing
		}


		public static class Constants {

			public const string SCENE_MENU = "NetworkMenu";
			public const string SCENE_LOBBY = "NetworkLobby";
			public const string SCENE_BATTLE = "NetworkBattle";
			public const string SCENE_ADVENTUREMENU = "NetworkAdventureMenu";
			public const string SCENE_ADVENTURE = "NetworkAdventure";
			public const string SCENE_ENDGAME = "NetworkEndGame";

			public const int DECK_SIZE = 3;

			public const string PATH_TARGET_SIGN = "PrefabsLoad/Target/TargetSign";
			public const string PATH_TARGET_LINE = "PrefabsLoad/Target/Targetline";
			public const string PATH_NETWORK = "PrefabsLoad/Network";

			public const float DISTANCE_DRAG = 100.0f;
			public const float DISTANCE_RESET = 0.1f;
			public const float DISTANCE_UI_RESET = 0.0025f;

			public const float UI_COLLECTION_TAG_ON = 0.2f;

			public const float SPEED_MOVE = 4.0f;
			public const float SPEED_UI_LERP = 10.0f;

			public const string NAME_MAP_FIELD = "MapField_";
			public const string NAME_SLOTS = "Slots";
			public const string NAME_NETWORK_DISCOVERY = "NetworkDiscovery";

			public const string SYMBOL_PASSWORD = "-/*/-";

			public const string SORTINGLAYER_DEADBODY = "DeadBody";
			public const string SORTINGLAYER_CHESS = "Chess";

			public const string SAVE_CATEGORY_SETTINGS = "Settings";
			public const string SAVE_TITLE_LANGUAGE = "Language";
			public const string SAVE_CATEGORY_ADVENTURE = "Adventure";
			public const string SAVE_CATEGORY_BAG = "Bag";
			public const string SAVE_CATEGORY_COUPON = "Coupon";
			public const string SAVE_TITLE_COINS = "Coins";

			public const string SAVE_CATEGORY_NETWORK = "Network";
			public const string SAVE_TITLE_NETWORK_NAME = "Name";
			public const string SAVE_TITLE_NETWORK_PASSWORD = "Password";
			public const string SAVE_TITLE_NETWORK_JOIN_PASSWORD = "JoinPassword";
			public const string SAVE_CATEGORY_PRESET = "Preset";
			public static readonly string[] SAVE_TITLE_PRESET_CHESS = { "C0", "C1", "C2" };
			public static readonly string[] SAVE_TITLE_PRESET_POSITION = { "P0", "P1", "P2" };

			public const string LANGUAGE_DEFAULT = "CHS";
			public const string LANGUAGE_CHS = "CHS";
			public const string LANGUAGE_CHT = "CHT";
			public const string LANGUAGE_EN = "EN";

			public const string CAPTION_CHESSABILITY = "ChessAbility";
			public const string CAPTION_CHESSNAME = "ChessName";

			public static Color COLOR_DEADBODY = new Color (0.2f, 0.2f, 0.2f, 0.5f);
			public static Color COLOR_BUSY = new Color (0.8f, 0.8f, 0.9f, 1);
			public static Color COLOR_CHESS_ACTIVE = Color.white;
			public static Color COLOR_CHESS_INACTIVE = new Color (1, 1, 1, 0.5f);

			public static Vector2 StringToVector2 (string t_string) {
				//			public Vector3 stringToVec(string s) {
				//				string[] temp = s.Substring (1, s.Length-2).Split (',');
				//				return new Vector3 (float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
				//			}

				string[] t_numbers = t_string.Split (',');
				return new Vector2 (float.Parse (t_numbers [0]), float.Parse (t_numbers [1]));
			}

			public static string Vector2ToString (Vector2 t_vector2) {
				string t_string = t_vector2.x.ToString ("0.###") + "," + t_vector2.y.ToString ("0.###");
				return t_string;
			}

			public static T GetRandomEnum<T> () {
				System.Array A = System.Enum.GetValues (typeof(T));
				T V = (T)A.GetValue (Random.Range (0, A.Length));
				return V;
			}
		}
	}
}
