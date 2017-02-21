using UnityEngine;
using System.Collections;

public class CS_Chess_AI_IceMage : CS_Chess {

	public float at_CDInDanger;

	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;
	
	//For move
	public Vector2[] presetPosition = {new Vector2(0, 7), new Vector2(3, 7), new Vector2(-3, 7)};
	public Vector2[] presetIceTotemPosition = {new Vector2(-3.5f, 2), new Vector2(3.5f, 2)};
	
	public GameObject mySkill1;
	public GameObject mySkill2;
	public GameObject mySkill3;

	public GameObject myTotem;
	private GameObject[] myTotems;
	private int myTotemNumber;

	public CS_AudioClip mySkill1SFX;
	public CS_AudioClip mySkill2SFX;

	public override void CustomInitialize () {
		GameObject t_Totem1 = Instantiate (myTotem, presetIceTotemPosition [0], Quaternion.identity) as GameObject;
		GameObject t_Totem2 = Instantiate (myTotem, presetIceTotemPosition [1], Quaternion.identity) as GameObject;

		t_Totem1.SendMessage ("SetMyMaster", this.gameObject);
		t_Totem2.SendMessage ("SetMyMaster", this.gameObject);

		//Add to after battle
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("AddChess", t_Totem1);
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("AddChess", t_Totem2);

		myTotemNumber = 2;
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
	
	public void Action () {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");

		if (myTotemNumber <= 0) {
			//when totem all die, reduce cool down time
			at_CD = at_CDInDanger;

			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.2f)
					ActionNumber = 0;
				else if(t_Number < 0.6f)
					ActionNumber = 1;
				else
					ActionNumber = 2;

				if (ActionNumber != ActionNumber_Last)
					break;

				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		}
		else {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.5f)
					ActionNumber = 1;
				else
					ActionNumber = 2;
				
				if (ActionNumber != ActionNumber_Last)
					break;

				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
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
		case 3:
			Cast ();
			PlayMySFX (mySkill1SFX);
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
		SetProcess (CS_Global.PS_IDLE);
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
			case 3 : Skill2(); break;
			}
		}
		
	}
	
	public virtual void Skill1 () {
		//different in different character
		//iceball

		//Debug.Log (CS_Global.GetMyEnemyTag (this.tag));
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		foreach (GameObject Enemy in Enemies) {
			//Debug.Log (Enemy);
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
	
	public virtual void Skill2 () {
		//different in different character
		//Ice bird

		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		bool t_haveAlive = false;
		foreach (GameObject Enemy in Enemies) {
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

	public virtual void Skill3 () {
		//different in different character

		//Ice Arrow shoot each character
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		//bool t_haveAlive = false;
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				GameObject t_Skill = Instantiate (mySkill3, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
				t_Skill.SendMessage ("SetDirection", Enemy.transform.position);
				t_Skill.SendMessage ("SetMyCaster", this.gameObject);
				t_Skill.SendMessage ("SetMDM", 1);
			}
		}

		CoolDown (at_CD);
	}

	public void TotemIsDead () {
		myTotemNumber--; 
	}

	public bool IsDamaged () {
		if (at_CurHP < at_HP) {
			return true;
		} else
			return false;
	}

	public bool IsHalfDead () {
		if (at_CurHP <= (at_HP / 2)) {
			return true;
		} else
			return false;
	}
}