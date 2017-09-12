using UnityEngine;
using System.Collections;

public class CS_Skill_Wave : CS_Skill {
	public float moveSpeed = 1.0f;
	public Vector3 startPosition;
	public float endY = 12.5f;
	public float endWaitTime = 5.0f;

	private float timer;

	void Start () {
		timer = endWaitTime;
	}

	void Update () {
		if (this.transform.position.y <= endY && this.transform.position.y >= -endY) {
			Vector3 t_deltaPosition = Vector3.down * moveSpeed * Time.deltaTime;
			this.transform.position += t_deltaPosition;
		} else {
			timer -= Time.deltaTime;
			if (timer < 0)
				Kill ();
		}
	}

	public override void CollisionAction (GameObject g_GO_Collision) {
		
		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;

		//if hit my caster , heal
		if (g_GO_Collision == myCaster) {
			//g_GO_Collision.SendMessage("Heal", 1);
			return;
		}

		if (isFriendlyFire == false && g_GO_Collision.tag == myCaster.tag) {
			return;
		}

		if (at_MDM != 0) {
			g_GO_Collision.SendMessage("DamageM", at_MDM);
		}
	}
}
