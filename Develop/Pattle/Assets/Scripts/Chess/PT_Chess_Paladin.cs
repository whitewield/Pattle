using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_Chess_Paladin : PT_BaseChess {
//	[SerializeField] GameObject mySkillPrefab;

	protected override bool IndividualAction (GameObject g_target, Vector2 g_targetPos) {
		if (g_target.name == (PT_Global.Constants.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			Move ();
			return true;
		} else if (g_target.GetComponent<PT_BaseChess> ()) {
			myTargetGameObject = g_target;
			myTargetPosition = g_targetPos;
			myPosition = this.transform.position;
			Cast ();
		}
		return false;
	}

	protected override void Attack () {
		//		Vector3 t_position = myTargetPosition;
		//		t_position += CS_Global.POSITION_SKILL;

		//create skill
//		GameObject t_skill = Instantiate (mySkillPrefab, this.transform.position, Quaternion.identity) as GameObject;

//		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
//		t_skill.GetComponent<PT_BaseSkill> ().SetPhysicalDamage (myAttributes.PD);
//		t_skill.GetComponent<PT_Skill_Archer> ().SetDirection (myTargetPosition);

		//spawn the bullet on Clients
//		NetworkServer.Spawn (t_skill);

		myTargetGameObject.GetComponent<PT_BaseChess> ().HPModify (PT_Global.HPModifierType.Healing, myAttributes.MD);

		CoolDown ();
	}
}
