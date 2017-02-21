using UnityEngine;
using System.Collections;

public class CS_Chess_LightMage : CS_Chess {
	
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
		GameObject t_Skill = Instantiate (mySkill, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetMDM", at_MDM);
		
		CoolDown (at_CD);
	}
	
}
	

