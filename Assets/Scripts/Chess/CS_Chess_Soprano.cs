using UnityEngine;
using System.Collections;

public class CS_Chess_Soprano : CS_Chess {

	public override void IndividualAction (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		
		if (myTargetGameObject.tag == ("F" + this.tag)) {
			PreMove ();
			g_Input.SendMessage ("Done");
		} else if (myTargetGameObject.tag == "A" ||
		           myTargetGameObject.tag == "B") {
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
		//GameObject t_Skill = Instantiate (mySkill, myTargetGameObject.transform.position, Quaternion.identity) as GameObject;
		Instantiate (mySkill, myTargetGameObject.transform.position, Quaternion.identity);

		if (myTargetGameObject.GetComponent<CS_Chess>().GetProcess() == CS_Global.PS_CD ||
		    myTargetGameObject.GetComponent<CS_Chess>().GetProcess() == CS_Global.PS_CT)
			myTargetGameObject.SendMessage ("Idle");

		CoolDown (at_CD);
	}
}
