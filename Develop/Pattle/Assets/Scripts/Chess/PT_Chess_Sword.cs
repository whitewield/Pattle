﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

	protected override void CollisionAction(GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == PT_Global.Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == PT_Global.Process.Attack && 
			g_GO_Collision.GetComponent<PT_BaseChess>() && g_GO_Collision.GetComponent<PT_BaseChess>().GetMyOwnerID() != myOwnerID) {
			g_GO_Collision.GetComponent<PT_BaseChess> ().HPModify (PT_Global.HPModifierType.PhysicalDamage, myAttributes.PD);
			AttackBack ();
		}
	}

	protected override void DoOnDead () {
		Network.Destroy (mySword);
	}
}
