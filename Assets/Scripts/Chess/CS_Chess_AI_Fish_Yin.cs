using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Fish_Yin : CS_Chess {

	public GameObject myYang;

	public void SetMyYang (GameObject g_myYang) {
		myYang = g_myYang;
	}

	public override void CollisionAction (GameObject g_GO_Collision) {

		if (process == CS_Global.PS_DEAD)
			return;

		if (g_GO_Collision.tag == CS_Global.GetMyEnemyTag(this.tag)) {
			g_GO_Collision.SendMessage("DamageM",at_MDM);
		}
	}
		
	public override void UpdateAction () {
		return;
	}

	public override void Idle () {
		SetProcess (CS_Global.PS_IDLE);
	}

	public override void Move () {
		SetProcess (CS_Global.PS_MOVE);
	}

	public override void CoolDown (float g_time) {
//		SetProcess (CS_Global.PS_CD);
//		timer = g_time;
		SetProcess (CS_Global.PS_MOVE);
	}

	public override void Cast () {
//		SetProcess (CS_Global.PS_CT);
//		timer = at_CT;
		SetProcess (CS_Global.PS_MOVE);
	}

	public override void Attack () {
//		SetProcess (CS_Global.PS_ATTACK);
		SetProcess (CS_Global.PS_MOVE);
	}

	public override void AttackBack () {
//		SetProcess (CS_Global.PS_ATTACKBACK);
		SetProcess (CS_Global.PS_MOVE);
	}
		
	public override void DamageP (int g_PDM) {
		return;
	}

	public override void DamageM (int g_MDM) {
		myYang.SendMessage ("DamageP", g_MDM);
	}

	public void SetCurHp (int g_at_CurHP) {
		at_CurHP = g_at_CurHP;
		CheckIsDead ();
	}

}
