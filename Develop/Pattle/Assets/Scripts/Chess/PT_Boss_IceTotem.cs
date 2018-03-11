using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;
using Pattle.Action;

public class PT_Boss_IceTotem : PT_BaseBoss {

	[SerializeField] SO_ActionWeightSettings myActionWeights;

	[SerializeField] protected GameObject mySkill_1_IceRain;
	[SerializeField] int mySkill_1_IceRain_Count = 5;

	private PT_BaseChess myMaster;
	private float myCDScale;

	protected override void ActionAI () {

		myCDScale = (float)(myMaster.GetCurHP ()) / myMaster.GetAttributes ().HP;
		
		if (GetCurHP () < GetAttributes().HP || myMaster.GetCurHP () < myMaster.GetAttributes().HP) {
			myActionType = myActionWeights.GetRandomAction ();
		} else {
			myActionType = ActionType.Skill_1;
		}

		//ActionNumber = 10;
		switch (myActionType) {
		case ActionType.Move:
			Move ();
			break;
		case ActionType.Attack:
			Attack ();
//			PlayMySFX (myAttackSFX);
			break;
		case ActionType.Skill_1:
			Cast ();
//			PlayMySFX (mySkill1SFX);
			break;
		case ActionType.Skill_2:
			Cast ();
//			PlayMySFX (mySkill2SFX);
			break;
		}
	}

	protected override void Attack () {
		//get enemy position
		myTargetPosition = GetEnemy_LowestHP ().transform.position;

		//update my position for attack back
		myPosition = this.transform.position;

		base.Attack ();
	}

	/// <summary>
	/// skill 1 is ice rain
	/// </summary>
	protected override void Skill_1 () {
		float t_angle = 180 / (mySkill_1_IceRain_Count + 1);

		for (int i = 0; i < mySkill_1_IceRain_Count; i++) {

			Vector2 t_targetPos = Quaternion.Euler (0, 0, -t_angle * (i + 1)) * this.transform.right;

			//create skill
			GameObject t_skill = Instantiate (mySkill_1_IceRain, this.transform.position, Quaternion.identity) as GameObject;

			t_skill.GetComponent<PT_Skill_IceRain> ().SetDirection (t_targetPos);
			t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);

			//spawn the bullet on Clients
			NetworkServer.Spawn (t_skill);
		}



		CoolDown (myCDScale);
	}

	/// <summary>
	/// skill 2 is fire shoot
	/// </summary>
	protected override void Skill_2 () {

		//create skill
//		GameObject t_skill = Instantiate (mySkill_2_IceBird, this.transform.position, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
//		NetworkServer.Spawn (t_skill);
//
//		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
//		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myAttributes.MD);
//		t_skill.GetComponent<PT_Skill_LightMage> ().SetDirection (myTargetPosition);

		myMaster.GetComponent<PT_BaseChess> ().HPModify (HPModifierType.Healing, myAttributes.MD * 2);
		this.HPModify (HPModifierType.Healing, myAttributes.MD);

		CoolDown (myCDScale);
	}

	protected override void CollisionAction (GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == Process.Attack &&
		    g_GO_Collision.GetComponent<PT_BaseChess> () && g_GO_Collision.GetComponent<PT_BaseChess> ().GetMyOwnerID () != myOwnerID) {
			g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (HPModifierType.PhysicalDamage, myAttributes.PD);
			AttackBack ();
		}
	}

	public void SetMyMaster (PT_BaseChess g_master) {
		myMaster = g_master;
	}
}
