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

	public const float DISTANCE_DRAG = 100.0f;
	public const float DISTANCE_RESET = 0.1f;

	public const float SPEED_MOVE = 4.0f;

	public const string NAME_MAP_FIELD = "MapField_";
}
