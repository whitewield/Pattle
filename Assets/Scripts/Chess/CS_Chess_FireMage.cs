using UnityEngine;
using System.Collections;

public class CS_Chess_FireMage : CS_Chess {

	public override void IndividualAction (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");

		if (myTargetGameObject.tag == ("F" + this.tag)) {
			PreMove ();
			g_Input.SendMessage ("Done");
		} else if (myTargetGameObject.tag != this.tag &&
		           myTargetGameObject.tag != CS_Global.TAG_MAP){
			//Attack ();
			PreCast ();
			g_Input.SendMessage ("Done");
		} else {
			PreIdle ();
			g_Input.SendMessage ("Undone");
		}
	}

	public override void Attack()
	{
		Vector3 t_position = myTargetPosition;
		t_position += CS_Global.POSITION_SKILL;
		GameObject t_Skill = Instantiate (mySkill, t_position, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetMDM", at_MDM);

		CoolDown (at_CD);
	}

}
