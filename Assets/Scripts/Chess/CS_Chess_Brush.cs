using UnityEngine;
using System.Collections;

public class CS_Chess_Brush : CS_Chess {

	public GameObject myBrush;

	public override void CustomInitialize () {
		//create my brush
		myBrush = Instantiate (myBrush, this.transform.position, Quaternion.identity) as GameObject;
		myBrush.transform.position += CS_Global.POSITION_SKILL;
		myBrush.SendMessage ("SetMyCaster", this.gameObject);
		myBrush.SendMessage ("SetPDM", at_PDM);
		myBrush.SendMessage ("SetMDM", at_MDM);
	}

	public override void DoOnDead () {
		//delete brush
		myBrush.SendMessage ("Kill");
	}

	public override void IndividualAction (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		
		if (myTargetGameObject.tag == ("FA") ||
		    myTargetGameObject.tag == ("FB") ||
		    myTargetGameObject.tag == ("A") ||
		    myTargetGameObject.tag == ("B")
		    ) {
			PreCast ();
			g_Input.SendMessage ("Done");
		} else {
			PreIdle ();
			g_Input.SendMessage ("Undone");
		}
	}
	
	public override void Attack()
	{
		myBrush.SendMessage ("SetTarget", myTargetPosition);
		
		CoolDown (at_CD);
	}
	
}
