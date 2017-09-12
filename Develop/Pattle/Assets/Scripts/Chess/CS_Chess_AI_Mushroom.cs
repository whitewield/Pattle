using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Mushroom : CS_Chess {
	
	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;
	
	//For move
	public Vector2[] presetPosition = 
	{
		new Vector2 (0, 5), new Vector2 (3, 5), new Vector2 (-3, 5), 
		new Vector2 (0, 7), new Vector2 (3, 7), new Vector2 (-3, 7)
	};
	
	public GameObject me;
	public GameObject mySkill1;
	public GameObject mySkill2;
	
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
		
		if (at_CurHP < 2) {
			int t_DoWhileBreakTime = 1000;
			do {
				t_DoWhileBreakTime --;
				if(t_DoWhileBreakTime <= 0) {
					Debug.LogError("Break, I Spend Too Much Time In This Do While!");
					break;
				}
				
				float t_Number = Random.value;
				
				if (t_Number < 0.5f)
					ActionNumber = 0;
				else
					ActionNumber = 10;
			} while(ActionNumber == ActionNumber_Last);
		} else {
			int t_DoWhileBreakTime = 1000;
			do {
				t_DoWhileBreakTime --;
				if(t_DoWhileBreakTime <= 0) {
					Debug.LogError("Break, I Spend Too Much Time In This Do While!");
					break;
				}
				
				float t_Number = Random.value;
				
				if (t_Number < 0.1f)
					ActionNumber = 0;
				else if (t_Number < 0.5f)
					ActionNumber = 10;
				else
					ActionNumber = 1;
			} while(ActionNumber == ActionNumber_Last);
		}

		ActionNumber_Last = ActionNumber;
		
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
			PlayMySFX (myCastSFX);
			break;
		case 2:
			//Skill2 ();
			Cast ();
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
			}
		}
		
	}
	
	public virtual void Skill1 () {
		//different in different character

		//Split
		Vector3 t_D = new Vector3(Random.Range (-0.1f, 0.1f), Random.Range (-0.1f, 0.1f));

		GameObject t_copy = 
			Instantiate (me, (this.transform.position + t_D), this.transform.rotation) as GameObject;

		//Add to after battle
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("AddChess", t_copy);

		//set hp
		t_copy.SendMessage ("SetHP", (at_CurHP / 2));
		//t_copy.SendMessage ("CheckIsDead");

		//SetHP (at_CurHP - (at_CurHP / 2));
		at_CurHP = at_CurHP - (at_CurHP / 2);
		CheckIsDead ();
		
		//GameObject t_Skill = Instantiate (mySkill1, this.transform.position, Quaternion.identity) as GameObject;
		Instantiate (mySkill1, this.transform.position, Quaternion.identity);
		
		CoolDown (at_CD);
	}
	
	public virtual void Skill2 () {
		//different in different character

		//Heal
		Heal (at_MDM);

		//GameObject t_Skill = Instantiate (mySkill2, this.transform.position, Quaternion.identity) as GameObject;
		Instantiate (mySkill2, this.transform.position, Quaternion.identity);

		//Idle ();
		CoolDown (at_CD);
	}

	public override void DoOnPDM () {
		CheckIsDead ();

		if (process == CS_Global.PS_DEAD)
			return;
		
		Skill1 ();
	}

	public void SetHP (int g_HP) {
		//Debug.Log ("SetHP:" + g_HP);
		at_HP = g_HP;
		if (at_CurHP > at_HP)
			at_CurHP = at_HP;
		ShowHP ();
	}
}
