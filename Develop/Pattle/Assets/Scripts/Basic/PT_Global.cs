﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PT_Global{

	public enum ChessClass {
		Start = 0,

		Blood = 1,

		Magic = 2,

		Earth = 3,

		Light = 4,
	}

	public enum ChessType {
		none = 0,

		Warrior = 101,

		IceMage = 201,

		Archer = 301,

		Paladin = 401,
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

		public const float DISTANCE_DRAG = 100.0f;
		public const float DISTANCE_RESET = 0.1f;
		public const float DISTANCE_UI_RESET = 0.0025f;

		public const float UI_COLLECTION_TAG_ON = 0.2f;

		public const float SPEED_MOVE = 4.0f;
		public const float SPEED_UI_LERP = 6.0f;

		public const string NAME_MAP_FIELD = "MapField_";
		public const string NAME_SLOTS = "Slots";

		public const string SORTINGLAYER_DEADBODY = "DeadBody";
		public const string SORTINGLAYER_CHESS = "Chess";

		public const string SAVE_CATEGORY_SETTINGS = "Settings";
		public const string SAVE_TITLE_LANGUAGE = "Language";
		public const string SAVE_CATEGORY_ADVENTURE = "Adventure";
		public const string SAVE_CATEGORY_CHESS = "Chess";
		public const string SAVE_CATEGORY_BAG = "Bag";
		public const string SAVE_CATEGORY_COUPON = "Coupon";
		public const string SAVE_TITLE_COINS = "Coins";

		public const string LANGUAGE_DEFAULT = "CHS";
		public const string LANGUAGE_CHS = "CHS";
		public const string LANGUAGE_CHT = "CHT";
		public const string LANGUAGE_EN = "EN";

		public static Color COLOR_DEADBODY = new Color(0.2f, 0.2f, 0.2f, 0.5f);
	}
}
