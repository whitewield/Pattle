using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;
using Pattle.Action;
using Hang.AiryAudio;

public class PT_Boss_IceMage : PT_BaseBoss {

	[SerializeField] SO_ActionWeightSettings myActionWeights_Normal;
	[SerializeField] SO_ActionWeightSettings myActionWeights_Alone;

	[SerializeField] protected GameObject mySkill_1_Iceball;
	[SerializeField] protected GameObject mySkill_2_IceBird;

	[SerializeField] AiryAudioData myAiryAudioData_Iceball;
	[SerializeField] AiryAudioData myAiryAudioData_IceBird;

	[SerializeField] GameObject myTotemPrefab;
	private PT_Boss_IceTotem[] myTotems;
	private bool isAlone = false;

	protected override void CustomInitialize () {

		// init the totems

		myTotems = new PT_Boss_IceTotem[2];

		for (int i = 0; i < myTotems.Length; i++) {

			//create totem
			GameObject t_totem = Instantiate (myTotemPrefab, myMoveSettings.Moves[3 + i * 2], Quaternion.identity) as GameObject;

			myTotems [i] = t_totem.GetComponent<PT_Boss_IceTotem> ();
			myTotems [i].SetMyMaster (this);


			PT_BaseBoss t_totemScript = t_totem.GetComponent<PT_BaseBoss> ();

			//add the chess to the network game manager
			//			PT_NetworkGameManager.myChessList [myID].Add (t_chess);

			//set the boss id to the chess
			t_totemScript.SetMyOwnerID (1);

			t_totemScript.SetMyManager (myManager);

//			t_totemScript.Initialize ();

			//spawn the totem on Clients
			NetworkServer.Spawn (t_totem);

			PT_NetworkGameManager.Instance.RpcAddChessToList (1, t_totem);
		}
	}

	protected override void ActionAI () {

		// alone check
		if (!isAlone) {
			bool t_hasTotemAlive = false;
			foreach (PT_Boss_IceTotem f_totem in myTotems) {
				if (f_totem.GetProcess () != Process.Dead) {
					t_hasTotemAlive = true;
					break;
				}
			}

			if (t_hasTotemAlive == false) {
				isAlone = true;
			}
		}

		for (int f_loopTime = 0; f_loopTime < 1000; f_loopTime++) {

			if (isAlone) {
				myActionType = myActionWeights_Alone.GetRandomAction ();
			} else {
				myActionType = myActionWeights_Normal.GetRandomAction ();
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
//			PlayMySFX (myAttackSFX);
			break;
		case ActionType.Skill_1:
			Cast ();
			myAiryAudioData_Iceball.Play ();
//			PlayMySFX (mySkill1SFX);
			break;
		case ActionType.Skill_2:
			Cast ();
			myAiryAudioData_IceBird.Play ();
//			PlayMySFX (mySkill2SFX);
			break;
		}
	}

//	protected override void Attack () {
//		//get enemy position
//		myTargetPosition = GetEnemy_LowestHP ().transform.position;
//
//		//update my position for attack back
//		myPosition = this.transform.position;
//
//		base.Attack ();
//	}

	/// <summary>
	/// skill 1 is iceball
	/// </summary>
	protected override void Skill_1 () {
		//get enemy position
		myTargetPosition = GetEnemy_Random ().transform.position;

		//create skill
		GameObject t_skill = Instantiate (mySkill_1_Iceball, myTargetPosition, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myAttributes.MD);

		if (isAlone) {
			CoolDown (0.5f);
		} else {
			CoolDown (1);
		}
	}

	/// <summary>
	/// skill 2 is ice bird
	/// </summary>
	protected override void Skill_2 () {
		//get enemy position
		myTargetPosition = GetEnemy_LowestHP ().transform.position;

		//create skill
		GameObject t_skill = Instantiate (mySkill_2_IceBird, this.transform.position, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
		t_skill.GetComponent<PT_BaseSkill> ().SetPhysicalDamage (myAttributes.PD);
		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myAttributes.MD);
		t_skill.GetComponent<PT_Skill_IceBird> ().SetDirection (myTargetPosition);

		if (isAlone) {
			CoolDown (0.5f);
		} else {
			CoolDown (1);
		}
	}

//	protected override void CollisionAction (GameObject g_GO_Collision) {
//		if (!isServer)
//			return;
//		if (myProcess == Process.Dead)
//			return;
//
//		//need to be rewrite in different chess
//		if (myProcess == Process.Attack &&
//		    g_GO_Collision.GetComponent<PT_BaseChess> () && g_GO_Collision.GetComponent<PT_BaseChess> ().GetMyOwnerID () != myOwnerID) {
//			g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (HPModifierType.PhysicalDamage, myAttributes.PD);
//			AttackBack ();
//		}
//	}
}
