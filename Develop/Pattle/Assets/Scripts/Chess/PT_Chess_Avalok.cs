using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_Chess_Avalok : PT_BaseChess {
	[SerializeField] float myGoldTime;

	protected override bool IndividualAction (GameObject g_target, Vector2 g_targetPos) {
		if (myProcess == Process.Dead)
			return false;
		
		if (g_target.name == (Constants.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			QueueMove ();
			return true;
		} else if (g_target.GetComponent<PT_BaseChess> ()) {
			isSingleTarget = true;
			myTargetGameObject = g_target;
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
//		GameObject t_skill = Instantiate (mySkillPrefab, this.transform.position, Quaternion.identity) as GameObject;

//		t_skill.GetComponent<PT_BaseSkill> ().SetMyCaster (this);
//		t_skill.GetComponent<PT_BaseSkill> ().SetPhysicalDamage (myAttributes.PD);
//		t_skill.GetComponent<PT_Skill_Archer> ().SetDirection (myTargetPosition);

		//spawn the bullet on Clients
//		NetworkServer.Spawn (t_skill);

		PT_BaseChess t_chess = myTargetGameObject.GetComponent<PT_BaseChess> ();

		//if hit not chess , return
		if (t_chess == null)
			return;

		t_chess.SetStatus (Status.Gold, myGoldTime);

		CoolDown ();
	}
}
