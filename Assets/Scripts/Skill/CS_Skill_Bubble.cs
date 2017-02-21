using UnityEngine;
using System.Collections;

public class CS_Skill_Bubble : CS_Skill {


	public override void CollisionAction (GameObject g_GO_Collision) {

	}

	void OnTriggerStay2D (Collider2D other) {
		CollisionStayAction (other.gameObject);
	}
	
	void OnCollisionStay2D (Collision2D collision) {
		CollisionStayAction (collision.gameObject);
	}

	public void CollisionStayAction (GameObject g_GO_Collision) {
		
		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;
		//if hit my caster , return
		if (g_GO_Collision == myCaster)
			return;
		
		g_GO_Collision.SendMessage ("ST_Bubble", 0.2f);
	}
}
