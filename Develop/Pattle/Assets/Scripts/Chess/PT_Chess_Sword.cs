using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_Chess_Sword : PT_BaseChess {
	[SerializeField] GameObject mySwordPrefab;
	private GameObject mySword;


	protected override void CustomInitialize () {
		if (!isServer)
			return;

		//create sword
		mySword = Instantiate (mySwordPrefab, this.transform.position, Quaternion.identity) as GameObject;

		PT_BaseSkill t_baseSkill = mySword.GetComponentInChildren<PT_BaseSkill> ();
		t_baseSkill.SetMyCaster (this);
		t_baseSkill.SetMagicDamage (myAttributes.MD);

		//spawn the sword on Clients
		NetworkServer.Spawn (mySword);
	}

	protected override void CollisionAction (GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == Process.Attack &&
		    g_GO_Collision.GetComponent<PT_BaseChess> () && g_GO_Collision.GetComponent<PT_BaseChess> ().GetMyOwnerID () != myOwnerID) {
			g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (HPModifierType.PhysicalDamage, myAttributes.PD);
			AttackBack ();
		}
	}

	protected override void DoOnDead () {
		mySword.GetComponent<PT_BaseSkill> ().Kill ();
		base.DoOnDead ();
	}
}
