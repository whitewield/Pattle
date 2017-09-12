using UnityEngine;
using System.Collections;

public class CS_Chess_Sword : CS_Chess {

	private GameObject mySword;

	public override void CustomInitialize () {
		mySword = Instantiate (mySkill, this.transform.position, Quaternion.identity) as GameObject;
		mySword.transform.SetParent (this.transform);
	}

	public override void DoOnDead () {
		Destroy (mySword);
	}
	
	public override void CollisionAction (GameObject g_GO_Collision) {
		
		if (process == CS_Global.PS_DEAD)
			return;
		
		//run after collision
		if (process == CS_Global.PS_ATTACK && 
		    g_GO_Collision.tag == CS_Global.GetMyEnemyTag(this.tag)) {
			g_GO_Collision.SendMessage("DamageP",at_PDM);
			AttackBack();
		}
	}
	
	public override void IndividualAction (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		
		if (myTargetGameObject.tag == ("F" + this.tag)) {
			PreMove ();
			g_Input.SendMessage ("Done");
		} else if (myTargetGameObject.tag != this.tag){
			PreAttack ();
			g_Input.SendMessage ("Done");
		} else {
			PreIdle ();
			g_Input.SendMessage ("Undone");
			//Casting();
		}
	}
	
	public override void Attack () {
		SetProcess (CS_Global.PS_ATTACK);
		myPosition = this.transform.position;
	}

}
