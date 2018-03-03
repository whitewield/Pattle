using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;

public class PT_Chess_Warrior : PT_BaseChess {
	protected override void CollisionAction(GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == Process.Attack && 
			g_GO_Collision.GetComponent<PT_BaseChess>() && g_GO_Collision.GetComponent<PT_BaseChess>().GetMyOwnerID() != myOwnerID) {
			g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (HPModifierType.PhysicalDamage, myAttributes.PD);
			AttackBack ();
		}
	}
}
