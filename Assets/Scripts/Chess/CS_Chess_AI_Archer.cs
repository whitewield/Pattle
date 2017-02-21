using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Archer : CS_Chess {
	
	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;

	//For move
	public Vector2[] presetPosition = 
	{
		new Vector2 (0, 8), new Vector2 (4, 1.5f), new Vector2 (-4, 1.5f)
	};

	public CS_AudioClip mySkill1SFX;
	public CS_AudioClip mySkill2SFX;
	public CS_AudioClip mySkill3SFX;
	
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

		if (at_CurHP > (at_HP / 2)) {
			//when totem all die, reduce cool down time
			//at_CD = at_CDInDanger;
			
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.4f)
					ActionNumber = 0;
				else
					ActionNumber = 1;
				
				if (ActionNumber != ActionNumber_Last)
					break;
				
				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		} else {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.5f)
					ActionNumber = 2;
				else
					ActionNumber = 3;
				
				if (ActionNumber != ActionNumber_Last)
					break;
				
				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		}
		
		ActionNumber_Last = ActionNumber;

		//Debug.Log(ActionNumber);
		//ActionNumber = 10;
		switch (ActionNumber) {
		case 0:
			Move ();
			PlayMySFX (myMoveSFX);
			break;
		case 1:
			Cast ();
			PlayMySFX (mySkill1SFX);
			break;
		case 2:
			Cast ();
			PlayMySFX (mySkill2SFX);
			break;
		case 3:
			Cast ();
			PlayMySFX (mySkill3SFX);
			break;
		}
	}
	
	public override void Idle () {
		SetProcess (CS_Global.PS_IDLE);
		
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
			}
		}
		
	}
	
	public virtual void Skill1 () {
		//different in different character

		// Normal shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		myTargetGameObject = Enemies[1];
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}
		
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD &&
			    Enemy.GetComponent<CS_Chess>().GetCurHP() < myTargetGameObject.GetComponent<CS_Chess>().GetCurHP()) {
				myTargetGameObject = Enemy;
			}
		}
	
		if (t_haveAlive) {
			myTargetPosition = myTargetGameObject.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}

		GameObject t_Skill = Instantiate (mySkill, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetPDM", at_PDM);
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		
		CoolDown (at_CD);
	}
	
	public virtual void Skill2 () {
		//different in different character
		// Moving Shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		myTargetGameObject = Enemies[1];
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}

		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD &&
			    Enemy.GetComponent<CS_Chess>().GetCurHP() < myTargetGameObject.GetComponent<CS_Chess>().GetCurHP()) {
				myTargetGameObject = Enemy;
			}
		}
		
		if (t_haveAlive) {
			myTargetPosition = myTargetGameObject.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		
		GameObject t_Skill = Instantiate (mySkill, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		t_Skill.SendMessage ("SetPDM", 1);
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		
		Move ();
	}

	public virtual void Skill3 () {
		//different in different character
		
		//All Shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				myTargetPosition = Enemy.transform.position;
				GameObject t_Skill = Instantiate (mySkill, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
				t_Skill.SendMessage ("SetMyCaster", this.gameObject);
				t_Skill.SendMessage ("SetPDM", at_PDM);
				t_Skill.SendMessage ("SetDirection", myTargetPosition);
			}
		}
		
		//Idle ();
		CoolDown (at_CD);
	}
}