using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_Skill_IceBird : PT_BaseSkill {


	[SerializeField] float myMoveSpeed;
	[SerializeField] float myMoveAcceleration;
	[SyncVar] Vector2 myDirection;

	[SerializeField] float myFreezeTime = 1;

	[SerializeField] float myRateByDistance = 2;
	private float myDistanceCounter;

	protected override void Update () {
		myMoveSpeed += Time.deltaTime * myMoveAcceleration;
		float t_distance = myMoveSpeed * Time.deltaTime;
		Vector3 t_deltaPosition = myDirection * t_distance;
		this.transform.position += t_deltaPosition;

		if (myRateByDistance > 0)
			Update_Drop (t_distance);

		base.Update ();
	}

	private void Update_Drop (float g_deltaPosition) {
		//		if (lastPosotion == null) {
		//			lastPosotion = this.transform.position;
		//			return;
		//		}

		myDistanceCounter += g_deltaPosition;
		if (myDistanceCounter > myRateByDistance) {
			myDistanceCounter -= myRateByDistance;
			Drop ();
		}
	}

	private void Drop () {
		//create skill
		GameObject t_skill = Instantiate (mySubSkill, this.transform.position, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (myCaster);
		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myMD);
	}

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
//		if (myMD != 0) {
//			t_chess.HPModify (HPModifierType.MagicDamage, myMD);
//		}
//		if (myHeal != 0) {
//			t_chess.HPModify (HPModifierType.Healing, myHeal);
//		}

		t_chess.SetStatus (Status.Freeze, myFreezeTime);
	}

	public void SetDirection (Vector2 g_targetPosition) {
		myDirection = (g_targetPosition - (Vector2)this.transform.position).normalized;

		Quaternion t_quaternion = Quaternion.Euler (0, 0, 
			Vector2.Angle (Vector2.up, myDirection) * Mathf.Sign (myDirection.x * -1));

		this.transform.rotation = t_quaternion;
	}
}
