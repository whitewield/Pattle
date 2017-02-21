//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM
// 3. SetTargetPosition

using UnityEngine;
using System.Collections;

public class CS_Skill_Fish : CS_Skill {

	//private Vector2 direction;
	private Vector2 targetPosition;
	public bool isYin;

	public void SetTargetPosition (Vector2 g_targetPosition) {
		targetPosition = g_targetPosition;

		//update
		//set direction
		Vector2 myPosition = this.transform.position;
		Vector2 t_direction = targetPosition - myPosition;
		t_direction = t_direction.normalized;

		if(t_direction.x > 0)
			this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, Vector2.Angle (Vector2.up, t_direction) * -1);
		else 
			this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, Vector2.Angle (Vector2.up, t_direction));
	}

	public override void CollisionAction (GameObject g_GO_Collision) {

		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;
		//if hit my caster , return
		if (g_GO_Collision.tag == myCaster.tag)
			return;

		if (at_PDM != 0) {
			g_GO_Collision.SendMessage("DamageP" ,at_PDM);
		}
		if (at_MDM != 0) {
			g_GO_Collision.SendMessage("DamageM" ,at_MDM);
		}

		if (at_Heal != 0) {
			g_GO_Collision.SendMessage("Heal" ,at_Heal);
		}

		if (isYin == true)
			g_GO_Collision.SendMessage("ST_FISH" ,CS_Global.ST_FISH_YIN);
		else 
			g_GO_Collision.SendMessage("ST_FISH" ,CS_Global.ST_FISH_YANG);
	}

	void Update () {
		//set pos
		this.transform.position = myCaster.transform.position;

		//set direction
		Vector2 myPosition = this.transform.position;
		Vector2 t_direction = targetPosition - myPosition;
		t_direction = t_direction.normalized;

		if(t_direction.x > 0)
			this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, Vector2.Angle (Vector2.up, t_direction) * -1);
		else 
			this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, Vector2.Angle (Vector2.up, t_direction));

//		if (t_direction.x > 0)
//			this.transform.Rotate (0.0f, 0.0f, -(Vector2.Angle (Vector2.up, t_direction)));
//		else this.transform.Rotate (0.0f, 0.0f, Vector2.Angle (Vector2.up, t_direction));
	}
}
