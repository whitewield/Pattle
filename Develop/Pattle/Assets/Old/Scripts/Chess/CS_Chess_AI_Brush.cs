using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Brush : CS_Chess {

	public GameObject myBrush;

	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;

	public GameObject mySkillInk;
	
	//For move
	public Vector2[] presetPosition = 
	{
		new Vector2 (0, 7), new Vector2 (0, 3), new Vector2 (-3, 5), new Vector2 (3, 5)
	};

	public CS_AudioClip mySkill1SFX;
	public CS_AudioClip mySkill2SFX;
	public CS_AudioClip mySkill3SFX;

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
		
		if (at_CurHP > (at_HP / 1.5f)) {
			//when totem all die, reduce cool down time
			//at_CD = at_CDInDanger;
			
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.3f)
					ActionNumber = 4;	//ink
				else if(t_Number < 0.5f)
					ActionNumber = 0;	//move
				else
					ActionNumber = 1;	//brush 1 enemy
				
				if (ActionNumber != ActionNumber_Last)
					break;
				
				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		} else {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.3f)
					ActionNumber = 4;	//ink
				else if(t_Number < 0.6f)
					ActionNumber = 3;	//brush back
				else
					ActionNumber = 2;	//brush all

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
		case 10:
			Attack ();
			PlayMySFX (myAttackSFX);
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
			PlayMySFX (mySkill1SFX);
			break;
		case 4:
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
		
		
		//Debug.Log("Move:" + myTargetPosition);
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
		
		// Move Brush to enemy
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		myTargetGameObject = Enemies[0];

		//get an undead enemy
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}

		//random enemy
		if (t_haveAlive) {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				myTargetGameObject = Enemies [Random.Range (0, Enemies.Length)];
				if (myTargetGameObject.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
					myTargetPosition = myTargetGameObject.transform.position;
					//Debug.Log ("random enemy");
					break;
				}
			}
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}

		myBrush.SendMessage ("SetTarget", myTargetPosition);
		
		CoolDown (at_CD);
	}

	public virtual void Skill2 () {
		//different in different character
		
		// Move Brush to all enemy
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));

		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				myTargetPosition = Enemy.transform.position;
				myBrush.SendMessage ("SetTarget", myTargetPosition);
			}
		}
		
		CoolDown (at_CD);
	}
	
	public virtual void Skill3 () {
		//different in different character

		// Move Brush to back to myself
		myTargetPosition = this.transform.position;
		myBrush.SendMessage ("SetTarget", myTargetPosition);

		CoolDown (at_CD);
	}
	
	public virtual void Skill4 () {
		//different in different character
		
		//drop ink
		Instantiate (mySkillInk, myBrush.transform.position - CS_Global.POSITION_SKILL * 2, Quaternion.identity);
		
		//Idle ();
		Move ();
	}
}
