using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;

public class PT_Chess_Berserker : PT_BaseChess {
	private int myCurPD;

	private int GetPD () {
		if (GetCurHP () <= 2)
			return 3;
		else if (GetCurHP () <= 5)
			return 2;
		else
			return 1;
	}

	protected override void CollisionAction(GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == Process.Attack && 
			g_GO_Collision.GetComponent<PT_BaseChess>() && g_GO_Collision.GetComponent<PT_BaseChess>().GetMyOwnerID() != myOwnerID) {
			g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (HPModifierType.PhysicalDamage, GetPD ());
			AttackBack ();
		}
	}
}
