using UnityEngine;
using System.Collections;

public class CS_Chess_AI_FireDragon : CS_Chess {
	
	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;

	//For move
	public Vector2[] presetPosition = {new Vector2(0, 5), new Vector2(3, 7), new Vector2(-3, 7)};

	public GameObject mySkill1;
	public GameObject mySkill2;

	public CS_AudioClip mySkill1SFX;
	public CS_AudioClip mySkill2SFX;

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

	public void Action () {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		
		if (at_CurHP >= (at_HP / 2)) {

			int t_DoWhileBreakTime = 1000;
			do {
				t_DoWhileBreakTime --;
				if(t_DoWhileBreakTime <= 0) {
					Debug.LogError("Break, I Spend Too Much Time In This Do While!");
					break;
				}

				float t_Number = Random.value;
				
				if (t_Number < 0.25f)
					ActionNumber = 0;
				else if (t_Number < 0.6f)
					ActionNumber = 10;
				else
					ActionNumber = 1;
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
				
				if (t_Number < 0.25f)
					ActionNumber = 0;
				else if (t_Number < 0.6f)
					ActionNumber = 2;
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
			PlayMySFX (mySkill1SFX);
			break;
		case 2:
			Cast ();
			PlayMySFX (mySkill2SFX);
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
	}

	public override void Attack () {
		//need to be rewrite in different chess
		SetProcess (CS_Global.PS_ATTACK);
		myPosition = this.transform.position;

		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		GameObject targetEnemy = null;
		foreach (GameObject Enemy in Enemies) {
			//Debug.Log (Enemy);
			if (Enemy.GetComponent<CS_Chess>()==null) {
				Debug.LogError("Can not find CS_Chess!");
				continue;
			}

			if (Enemy.GetComponent<CS_Chess>().GetProcess() == CS_Global.PS_DEAD)
				continue;
			
			if (targetEnemy == null) targetEnemy = Enemy;
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

	public void Skill1 () {
		//different in different character
		//Debug.Log (CS_Global.GetMyEnemyTag (this.tag));
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		if (Enemies.Length == 0) {
			CoolDown (at_CD);
			return;
		}

		bool t_haveAlive = false;
		foreach (GameObject Enemy in Enemies) {
			//Debug.Log (Enemy);
			if (Enemy.GetComponent<CS_Chess>()==null) {
				Debug.LogError("Can not find CS_Chess!");
				continue;
			}

			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
			}
		}
		
		if (t_haveAlive) {
			GameObject targetEnemy = null;

			int t_DoWhileBreakTime = 1000;
			do {
				t_DoWhileBreakTime --;
				if(t_DoWhileBreakTime <= 0) {
					Debug.LogError("Break, I Spend Too Much Time In This Do While!");
					break;
				}

				targetEnemy = Enemies [Random.Range (0, Enemies.Length)];
			} while(targetEnemy.GetComponent<CS_Chess>().GetProcess() == CS_Global.PS_DEAD);
			myTargetPosition = targetEnemy.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		
		Vector3 t_position = myTargetPosition;
		t_position += CS_Global.POSITION_SKILL;
		GameObject t_Skill = Instantiate (mySkill1, t_position, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		
		CoolDown (at_CD);
	}

	public void Skill2 () {
		//Fire Shoot
		//different in different character
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		if (Enemies.Length == 0) {
			CoolDown (at_CD);
			return;
		}

		bool t_haveAlive = false;
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>()==null) {
				Debug.LogError("Can not find CS_Chess!");
				continue;
			}

			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
			}
		}
		
		if (t_haveAlive) {
			GameObject targetEnemy = null;

			int t_DoWhileBreakTime = 1000;
			do {
				t_DoWhileBreakTime --;
				if(t_DoWhileBreakTime <= 0) {
					Debug.LogError("Break, I Spend Too Much Time In This Do While!");
					break;
				}
				
				targetEnemy = Enemies [Random.Range (0, Enemies.Length)];
			} while(targetEnemy.GetComponent<CS_Chess>().GetProcess() == CS_Global.PS_DEAD);
			myTargetPosition = targetEnemy.transform.position;
		} else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		
		
		GameObject t_Skill = Instantiate (mySkill2, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetDirection", myTargetPosition);
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);

		CoolDown (at_CD);
	}
}
