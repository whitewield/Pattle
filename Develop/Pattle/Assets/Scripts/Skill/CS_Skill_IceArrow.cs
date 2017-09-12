//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM
// 3. SetDirection
// 4. (SetMoveSpeed)

using UnityEngine;
using System.Collections;

public class CS_Skill_IceArrow : CS_Skill {
	
	public float moveSpeed;
	private Vector2 direction;
	
	private float timer;
	private float maxTime = 5.0f;

	public float myFreezeTime = 1.0f;
	
	// Use this for initialization
	void Start () {
		timer = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 t_deltaPosition = direction * moveSpeed * Time.deltaTime;
		this.transform.position += t_deltaPosition;
		
		
		timer -= Time.deltaTime;
		if (timer < 0)
			Kill ();
	}
	
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
		if (at_MDM != 0) {
			g_GO_Collision.SendMessage("DamageM" ,at_MDM);
		}

		if (at_Heal != 0) {
			g_GO_Collision.SendMessage("Heal" ,at_Heal);
		}

		g_GO_Collision.SendMessage ("ST_Freeze", myFreezeTime);

		Kill ();
	}
	
	public void SetDirection (Vector2 g_targetPosition) {
		Vector2 myPosition = this.transform.position;
		direction = g_targetPosition - myPosition;
		direction = direction.normalized;
		
		if(direction.x > 0)
			this.transform.Rotate (0.0f, 0.0f, -(Vector2.Angle (Vector2.up, direction)));
		else this.transform.Rotate (0.0f, 0.0f, Vector2.Angle (Vector2.up, direction));
	}
	
	public void SetMoveSpeed (float g_MoveSpeed) {
		moveSpeed = g_MoveSpeed;
	}
	
	public override void DoOnKill () {
	}
}
