using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_Skill_LightMage : PT_BaseSkill {

	[SyncVar] Vector2 myDirection;

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

	public void SetDirection (Vector2 g_targetPosition) {
		myDirection = (g_targetPosition - (Vector2)this.transform.position).normalized;

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, myDirection) * Mathf.Sign (myDirection.x * -1));

		this.transform.rotation = t_quaternion;

//		RpcSetDirection (t_quaternion.eulerAngles);
	}

	[ClientRpc]
	void RpcSetDirection (Vector3 g_euler) {
		this.transform.rotation = Quaternion.Euler (g_euler);
	}
}
