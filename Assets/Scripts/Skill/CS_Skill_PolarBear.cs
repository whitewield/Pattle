//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM
// 3. (SetFreezeTime)

using UnityEngine;
using System.Collections;

public class CS_Skill_PolarBear : CS_Skill {

	public float myFreezeTime;

	public override void CollisionAction (GameObject g_GO_Collision) {
		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;
		//if hit my caster , return
		if (g_GO_Collision == myCaster)
			return;

		if (isFriendlyFire == false && g_GO_Collision.tag == myCaster.tag) {
			return;
		}

		if (at_PDM != 0) {
			g_GO_Collision.SendMessage("DamageP" ,at_PDM);
		}

		g_GO_Collision.SendMessage ("ST_Freeze", myFreezeTime);

		Kill ();
	}

}
