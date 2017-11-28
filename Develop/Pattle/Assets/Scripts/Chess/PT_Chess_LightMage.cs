﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_Chess_LightMage : PT_BaseChess {
	[SerializeField] GameObject mySkillPrefab;

	protected override bool IndividualAction (GameObject g_target, Vector2 g_targetPos) {
		if (myProcess == PT_Global.Process.Dead)
			return false;
		
		if (g_target.name == (PT_Global.Constants.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			QueueMove ();
			return true;
		} else if (g_target.name == (PT_Global.Constants.NAME_MAP_FIELD + (1 - myOwnerID).ToString ()) ||
			(g_target.GetComponent<PT_BaseChess> () && g_target.GetComponent<PT_BaseChess> ().GetMyOwnerID () != myOwnerID)) {
			myTargetPosition = g_targetPos;
			myPosition = this.transform.position;
			QueueCast ();
			return true;
		}
		QueueIdle ();
		return false;
	}

	protected override void Attack () {
//		Vector3 t_position = myTargetPosition;
//		t_position += CS_Global.POSITION_SKILL;

		//create skill
		GameObject t_skill = Instantiate (mySkillPrefab, this.transform.position, Quaternion.identity) as GameObject;

		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
		t_skill.GetComponent<PT_BaseSkill> ().SetMagicDamage (myAttributes.MD);
		t_skill.GetComponent<PT_Skill_LightMage> ().SetDirection (myTargetPosition);

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		CoolDown ();
	}
}