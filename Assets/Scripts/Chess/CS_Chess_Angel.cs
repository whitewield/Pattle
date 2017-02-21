using UnityEngine;
using System.Collections;

public class CS_Chess_Angel : CS_Chess {
	
	public override void IndividualAction (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		
		if (myTargetGameObject.tag == ("F" + this.tag)) {
			PreMove ();
			g_Input.SendMessage ("Done");
		} else if (myTargetGameObject == this.gameObject){
			//Heal ();
			PreCast ();
			g_Input.SendMessage ("Done");
		} else {
			PreIdle ();
			g_Input.SendMessage ("Undone");
		}
	}
	
	public override void Attack()
	{
		GameObject t_Skill = Instantiate (mySkill, this.transform.position, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetHeal", at_MDM);
		
		CoolDown (at_CD);
	}
	
}
