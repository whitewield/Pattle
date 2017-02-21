//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM

using UnityEngine;
using System.Collections;

public class CS_Skill_AirMage : CS_Skill {

	public override void CollisionAction (GameObject g_GO_Collision) {
		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;
		//if hit my caster , return
		if (g_GO_Collision == myCaster)
			return;
		
		if (at_PDM != 0) {
			g_GO_Collision.SendMessage("DamageP" ,at_PDM);
		}
		if (at_MDM != 0) {
			g_GO_Collision.SendMessage("DamageM" ,at_MDM);
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == CS_Global.TAG_A || other.tag == CS_Global.TAG_B) {
			other.transform.position = 
				Vector2.Lerp (other.transform.position, this.transform.position, CS_Global.SPEED_MOVE * Time.deltaTime);
			//Debug.Log ("Suck!" + other.tag);
		}
	}
}
