using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_Chess_Soprano : PT_BaseChess {
	[SerializeField] GameObject mySkillPrefab;

	protected override bool IndividualAction (GameObject g_target, Vector2 g_targetPos) {
		if (myProcess == Process.Dead)
			return false;

		if (g_target.name == (Constants.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			QueueMove ();
			return true;
		} else if (g_target != this.gameObject && g_target.GetComponent<PT_BaseChess> ()) {
			isSingleTarget = true;
			myTargetGameObject = g_target;
			myTargetPosition = g_targetPos;
			myPosition = this.transform.position;
			QueueCast ();
			return true;
		}
		QueueIdle();
		return false;
	}

	protected override void Attack () {
		//		Vector3 t_position = myTargetPosition;
		//		t_position += CS_Global.POSITION_SKILL;

		//create skill
		GameObject t_skill = Instantiate (mySkillPrefab, myTargetPosition, Quaternion.identity) as GameObject;

		//spawn the bullet on Clients
		NetworkServer.Spawn (t_skill);

		//if the chess is in cd or casting, make it ready
		PT_BaseChess t_baseChess = myTargetGameObject.GetComponent<PT_BaseChess> ();
		if (t_baseChess.GetProcess () == Process.CD ||
		    t_baseChess.GetProcess () == Process.CT)
			t_baseChess.BeReady ();

		CoolDown ();
	}
}
