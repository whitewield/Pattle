using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Chameleon : CS_Chess {
	
	protected int ActionNumber = -1;

	private int myStatus = 1;	//1:normal 2:P 3:M 4:H

	public Sprite SP_G;
	public Sprite SP_R;
	public Sprite SP_B;
	public Sprite SP_Y;

	public GameObject mySkill_G;
	public GameObject mySkill_R;
	public GameObject mySkill_B;
	public GameObject mySkill_Y;
	
	//For move
	public Vector2[] presetPosition = 
	{
		new Vector2 (0, 6), new Vector2 (3, 3), new Vector2 (-3, 3)
	};

	public CS_AudioClip mySkill_G_SFX;
	public CS_AudioClip mySkill_R_SFX;
	public CS_AudioClip mySkill_B_SFX;
	public CS_AudioClip mySkill_Y_SFX;

	public override void CustomInitialize ()
	{
		//base.CustomInitialize ();
		StatusToNormal ();
	}

	public void StatusToNormal () {
		myStatus = 1;
		this.GetComponent<SpriteRenderer> ().sprite = SP_G;
	}
	
	public override void DoOnPDM () {
		if (myStatus == 2)
			return;

		myStatus = 2;
		this.GetComponent<SpriteRenderer> ().sprite = SP_R;
		Idle ();
	}
	
	public override void DoOnMDM () {
		if (myStatus == 3)
			return;

		myStatus = 3;
		this.GetComponent<SpriteRenderer> ().sprite = SP_B;
		Idle ();
	}
	
	public override void DoOnHeal () {
		if (myStatus == 4)
			return;

		myStatus = 4;
		this.GetComponent<SpriteRenderer> ().sprite = SP_Y;
		Idle ();
	}
	
	public override void CollisionAction (GameObject g_GO_Collision) {
		
		if (process == CS_Global.PS_DEAD)
			return;
		
		//run after collision
		if (process == CS_Global.PS_ATTACK && 
		    g_GO_Collision.tag == CS_Global.GetMyEnemyTag(this.tag)) {
			g_GO_Collision.SendMessage("DamageM",at_MDM);
			AttackBack();
		}
	}
	
	public void Action () {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		
		if (myStatus == 1) {
			//Normal
			ActionNumber = 1;
		} else if (myStatus == 2) {
			//PDM
			ActionNumber = 2;
		} else if (myStatus == 3) {
			//MDM
			ActionNumber = 3;
		} else if (myStatus == 4) {
			//Heal
			ActionNumber = 4;
		}
		
		//Debug.Log(ActionNumber);
		switch (ActionNumber) {
		case 0:
			Move ();
			PlayMySFX (myMoveSFX);
			break;
		case 10:
			Attack ();
			PlayMySFX (myAttackSFX);
			break;
		case 1:
			Cast ();
			PlayMySFX (mySkill_G_SFX);
			break;
		case 2:
			Cast ();
			PlayMySFX (mySkill_R_SFX);
			break;
		case 3:
			Cast ();
			PlayMySFX (mySkill_B_SFX);
			break;
		case 4:
			Cast ();
			PlayMySFX (mySkill_Y_SFX);
			break;
		default :
			Move ();
			break;
		}
	}
	
	public override void Idle () {
		SetProcess (CS_Global.PS_IDLE);
		//myAnimatorProcess.PlayInFixedTime ("Idle");
		
		// if the Boss is idle, take action
		Action ();
	}
	
	public override void Move () {
		SetProcess (CS_Global.PS_MOVE);
		
		//Set Move Tatget Position
		int t_Number = 0;
		Vector2 t_myPos = this.transform.position;
		
		int t_DoWhileBreakTime = 1000;
		do {
			t_DoWhileBreakTime --;
			if(t_DoWhileBreakTime <= 0) {
				Debug.LogError("Break, I Spend Too Much Time In This Do While!");
				break;
			}
			
			t_Number = Random.Range (0, presetPosition.Length); 
		} while(presetPosition[t_Number] == t_myPos);
		
		myTargetPosition = presetPosition [t_Number];
		
		myCollider.isTrigger = true;
		
		
		Debug.Log("Move:" + myTargetPosition);
	}
	
	public override void Attack () {
		//need to be rewrite in different chess
		SetProcess (CS_Global.PS_ATTACK);
		myPosition = this.transform.position;
		
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		GameObject targetEnemy = null;
		foreach (GameObject Enemy in Enemies) {
			//Debug.Log (Enemy);
			if(Enemy.GetComponent<CS_Chess>().GetProcess() == CS_Global.PS_DEAD)
				continue;
			
			if(targetEnemy == null)targetEnemy = Enemy;
			else if(Enemy.GetComponent<CS_Chess>().GetCurHP() < targetEnemy.GetComponent<CS_Chess>().GetCurHP())
				targetEnemy = Enemy;
		}
		
		if (targetEnemy == null) {
			Action ();
			return;
		}
		
		myTargetPosition = targetEnemy.transform.position;
	}
	
	public override void UpdateAction_CT () {
		//different in different character
		if (timer <= 0) {
			switch(ActionNumber)
			{
			case 1 : Skill1(); break;
			case 2 : Skill2(); break;
			case 3 : Skill3(); break;
			case 4 : Skill4(); break;
			}
		}
		
	}
	
	public virtual void Skill1 () {
		//different in different character
		
		// Normal shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		myTargetGameObject = Enemies[0];
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}
		
		if (t_haveAlive) {
			//find alive one with lowest hp
			foreach (GameObject Enemy in Enemies) {
				if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD &&
				    Enemy.GetComponent<CS_Chess>().GetCurHP() < myTargetGameObject.GetComponent<CS_Chess>().GetCurHP()) {
					myTargetGameObject = Enemy;
				}
			}

			myTargetPosition = myTargetGameObject.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		
		GameObject t_Skill = Instantiate (mySkill_G, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetPDM", at_PDM);
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		
		CoolDown (at_CD);
	}
	
	public virtual void Skill2 () {
		//different in different character
		
		//PDM : All Shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				myTargetPosition = Enemy.transform.position;
				GameObject t_Skill = Instantiate (mySkill_R, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
				t_Skill.SendMessage ("SetMyCaster", this.gameObject);
				t_Skill.SendMessage ("SetPDM", at_PDM);
				t_Skill.SendMessage ("SetDirection", myTargetPosition);
			}
		}
		
		//StatusToNormal ();
		Move ();
	}
	
	public virtual void Skill3 () {
		//different in different character
		
		// MDM shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		myTargetGameObject = Enemies[0];
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}
		
		if (t_haveAlive) {
			//find alive one with lowest hp
			foreach (GameObject Enemy in Enemies) {
				if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD &&
				    Enemy.GetComponent<CS_Chess>().GetCurHP() < myTargetGameObject.GetComponent<CS_Chess>().GetCurHP()) {
					myTargetGameObject = Enemy;
				}
			}
			
			myTargetPosition = myTargetGameObject.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		
		GameObject t_Skill = Instantiate (mySkill_B, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetPDM", at_PDM);
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		
		//StatusToNormal ();
		Move ();
	}

	public virtual void Skill4 () {
		//different in different character
		
		// Heal shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		myTargetGameObject = Enemies[0];
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}
		
		if (t_haveAlive) {
			//find alive one with lowest hp
			foreach (GameObject Enemy in Enemies) {
				if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD &&
				    Enemy.GetComponent<CS_Chess>().GetCurHP() < myTargetGameObject.GetComponent<CS_Chess>().GetCurHP()) {
					myTargetGameObject = Enemy;
				}
			}
			
			myTargetPosition = myTargetGameObject.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		
		GameObject t_Skill = Instantiate (mySkill_Y, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetHeal", 1);
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		
		//StatusToNormal ();
		Move ();
	}
}