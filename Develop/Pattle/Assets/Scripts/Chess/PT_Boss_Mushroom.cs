using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;
using Pattle.Action;
using Hang.AiryAudio;

public class PT_Boss_Mushroom : PT_BaseBoss {

	[SerializeField] SO_ActionWeightSettings myActionWeights_Normal;
	[SerializeField] SO_ActionWeightSettings myActionWeights_Low;

	[SerializeField] AiryAudioData myAiryAudioData_Attack;
	[SerializeField] AiryAudioData myAiryAudioData_Clone;
	[SerializeField] AiryAudioData myAiryAudioData_Heal;

	[SerializeField] GameObject myPrefab;

	protected override void ActionAI () {
		if (myProcess == Process.Dead)
			return;

		for (int f_loopTime = 0; f_loopTime < 1000; f_loopTime++) {

			if (GetCurHP () >= (myAttributes.HP / 4)) {
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
			myAiryAudioData_Clone.Play ();
			break;
		case ActionType.Skill_2:
			Cast ();
			myAiryAudioData_Heal.Play ();
			break;
		}
	}

	protected override void Attack () {

		//get enemy position
		myTargetPosition = GetEnemy_Random ().transform.position;

		//update my position for attack back
		myPosition = this.transform.position;

		base.Attack ();
	}

	/// <summary>
	/// skill 1 is clone
	/// </summary>
	protected override void Skill_1 () {
		if (myProcess == Process.Dead)
			return;

		int t_hp1 = GetCurHP () / 2;
		int t_hp2 = GetCurHP () - t_hp1;

		if (t_hp1 < 1)
			t_hp1 = 1;
		if (t_hp2 < 1)
			t_hp2 = 1;

		SetCurHP (t_hp1);

		//create boss
		GameObject t_bossObject = Instantiate (PT_DeckManager.Instance.GetAdventureBossPrefab(), myManager.transform) as GameObject;

		PT_BaseBoss t_boss = t_bossObject.GetComponent<PT_BaseBoss> ();

		//add the chess to the network game manager
		//			PT_NetworkGameManager.myChessList [myID].Add (t_chess);

		//set the boss id to the chess
		t_boss.SetMyOwnerID (1);

		t_boss.transform.position = this.transform.position + (Vector3)Random.insideUnitCircle * 0.01f;

		t_boss.SetMyManager (myManager);

		//		t_boss.Initialize ();

		// spawn on the clients
		NetworkServer.Spawn (t_bossObject);

		PT_NetworkGameManager.Instance.RpcAddChessToList (1, t_bossObject);

		t_boss.RpcSetCurHP (t_hp2);

		CoolDown ();
	}

	/// <summary>
	/// skill 2 is heal
	/// </summary>
	protected override void Skill_2 () {

		HPModify (HPModifierType.Healing, myAttributes.MD);

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

	protected override void HPModify_PhysicalDamage (int g_value) {
		base.HPModify_PhysicalDamage (g_value);

		if (myProcess != Process.Attack && myProcess != Process.AttackBack)
			Skill_1 ();
	}

//	protected override void HPModify_MagicDamage (int g_value) {
//		base.HPModify_MagicDamage (g_value);
//
//		if (myProcess != Process.Attack || myProcess != Process.AttackBack)
//			Skill_1 ();
//	}
}
