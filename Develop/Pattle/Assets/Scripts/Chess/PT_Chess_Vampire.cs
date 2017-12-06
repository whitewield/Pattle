using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Chess_Vampire : PT_BaseChess {
	protected override void CollisionAction(GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == PT_Global.Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == PT_Global.Process.Attack && 
			g_GO_Collision.GetComponent<PT_BaseChess>() && g_GO_Collision.GetComponent<PT_BaseChess>().GetMyOwnerID() != myOwnerID) {
			if (g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (PT_Global.HPModifierType.MagicDamage, myAttributes.MD))
				this.HPModify (PT_Global.HPModifierType.Healing, 1);
			AttackBack ();
		}
	}
}
