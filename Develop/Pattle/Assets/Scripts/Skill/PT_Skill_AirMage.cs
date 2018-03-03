using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Global;

public class PT_Skill_AirMage : PT_BaseSkill {

	protected override void CollisionAction (GameObject g_GO_Collision) {
		if (!isServer)
			return;

		PT_BaseChess t_chess = g_GO_Collision.GetComponent<PT_BaseChess> ();

		//if hit not chess , return
		if (t_chess == null)
			return;

		//if hit my caster , return
		if (t_chess == myCaster)
			return;

		if (isFriendlyFire == false && t_chess.GetMyOwnerID () == myCaster.GetMyOwnerID ()) {
			return;
		}

		if (myPD != 0) {
			t_chess.HPModify (HPModifierType.PhysicalDamage, myPD);
		}
		if (myMD != 0) {
			t_chess.HPModify (HPModifierType.MagicDamage, myMD);
		}
		if (myHeal != 0) {
			t_chess.HPModify (HPModifierType.Healing, myHeal);
		}

	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.GetComponent<PT_BaseChess> ()) {
			other.transform.position = 
				Vector2.Lerp (other.transform.position, this.transform.position, Constants.SPEED_MOVE * Time.deltaTime);
			//Debug.Log ("Suck!" + other.tag);
		}
	}
}
