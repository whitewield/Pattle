using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PT_Global {
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

	public const float DISTANCE_DRAG = 100.0f;
	public const float DISTANCE_RESET = 0.1f;

	public const float SPEED_MOVE = 4.0f;

	public const string NAME_MAP_FIELD = "MapField_";

	public const string SORTINGLAYER_DEADBODY = "DeadBody";
	public const string SORTINGLAYER_CHESS = "Chess";

	public static Color COLOR_DEADBODY = new Color(0.2f, 0.2f, 0.2f, 0.5f);
}
