using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;
using Pattle.Action;
using Hang.AiryAudio;

public class PT_Boss_FireDragon : PT_BaseBoss {

	[SerializeField] SO_ActionWeightSettings myActionWeights_Normal;
	[SerializeField] SO_ActionWeightSettings myActionWeights_Low;

	[SerializeField] protected GameObject mySkill_1_Fireball;
	[SerializeField] protected GameObject mySkill_2_FireShoot;

	[SerializeField] AiryAudioData myAiryAudioData_Attack;
	[SerializeField] AiryAudioData myAiryAudioData_Fireball;
	[SerializeField] AiryAudioData myAiryAudioData_FireShoot;

	protected override void ActionAI () {
		for (int f_loopTime = 0; f_loopTime < 1000; f_loopTime++) {

			if (GetCurHP () >= (myAttributes.HP / 2)) {
				myActionType = myActionWeights_Normal.GetRandomAction ();
			} else {
				myActionType = myActionWeights_Low.GetRandomAction ();
			}

			if (myActionType != myLastActionType) {
				break;
			}

			if (f_loopTime == 1000) {
				Debug.LogError ("I Spend Too Much Time In This Loop!");
			}
		}

		myLastActionType = myActionType;

		//ActionNumber = 10;
		switch (myActionType) {
		case ActionType.Move:
			Move ();
			break;
		case ActionType.Attack:
			Attack ();
			myAiryAudioData_Attack.Play ();
			break;
		case ActionType.Skill_1:
			Cast ();
			myAiryAudioData_Fireball.Play ();
			break;
		case ActionType.Skill_2:
			Cast ();
			myAiryAudioData_FireShoot.Play ();
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
	/// skill 1 is fireball
	/// </summary>
	protected override void Skill_1 () {
		//get enemy position
		myTargetPosition = GetEnemy_Random ().transform.position;

		//create skill
		GameObject t_skill = Instantiate (mySkill_1_Fireball, myTargetPosition, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myAttributes.MD);

		CoolDown ();
	}

	/// <summary>
	/// skill 2 is fire shoot
	/// </summary>
	protected override void Skill_2 () {
		//get enemy position
		myTargetPosition = GetEnemy_Random ().transform.position;

		//create skill
		GameObject t_skill = Instantiate (mySkill_2_FireShoot, this.transform.position, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myAttributes.MD);
		t_skill.GetComponent<PT_Skill_LightMage> ().SetDirection (myTargetPosition);

		CoolDown ();
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
}
