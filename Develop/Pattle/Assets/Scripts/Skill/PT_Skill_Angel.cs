﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Skill_Angel : PT_BaseSkill {

	protected override void CollisionAction (GameObject g_GO_Collision) {
		if (!isServer)
			return;

		PT_BaseChess t_chess = g_GO_Collision.GetComponent<PT_BaseChess> ();

		//if hit not chess , return
		if (t_chess == null)
			return;

		if (isFriendlyFire == false && t_chess.GetMyOwnerID () == myCaster.GetMyOwnerID ()) {
			return;
		}

		if (myPD != 0) {
			t_chess.HPModify (PT_Global.HPModifierType.PhysicalDamage, myPD);
		}
		if (myMD != 0) {
			t_chess.HPModify (PT_Global.HPModifierType.MagicDamage, myMD);
		}
		if (myHeal != 0) {
			t_chess.HPModify (PT_Global.HPModifierType.Healing, myHeal);
		}

	}
}