using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Neptu : CS_Chess {
	
	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;

	protected float startY = 12.0f;
	
	//For move
	public Vector2[] presetPosition = {
		new Vector2(0, 5), new Vector2(3, 7), new Vector2(-3, 7)
	};

	private Vector2[] presetBubblePosition = {
		new Vector2(-5, -2.5f), new Vector2(-5, -7.5f),
		new Vector2(0, -2.5f), new Vector2(0, -7.5f),
		new Vector2(5, -2.5f), new Vector2(5, -7.5f)
	};
	
	public GameObject mySkill1;
	public GameObject mySkill2;
	public GameObject mySkill3;

	public CS_AudioClip mySkill1SFX;
	public CS_AudioClip mySkill2SFX;
	public CS_AudioClip mySkill3SFX;

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
		
		if (at_CurHP > (at_HP * 2 / 3)) {
			//when totem all die, reduce cool down time
			//at_CD = at_CDInDanger;
			
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;
				
				if (t_Number < 0.3f)
					ActionNumber = 0;
				else if (t_Number < 0.6f)
					ActionNumber = 10;
				else
					ActionNumber = 1;
				
				if (ActionNumber != ActionNumber_Last)
					break;
				
				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		} else if (at_CurHP > (at_HP / 3)) {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				if (ActionNumber_Last == 2) {
					ActionNumber = 3;
					break;
				}
				
				float t_Number = Random.value;
				
				if (t_Number < 0.5f)
					ActionNumber = 2;
				else
					ActionNumber = 1;
				
				if (ActionNumber != ActionNumber_Last)
					break;
				
				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		} else {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				if (ActionNumber_Last == 2) {
					ActionNumber = 4;
					break;
				}
				
				float t_Number = Random.value;
				
				if (t_Number < 0.5f)
					ActionNumber = 2;
				else
					ActionNumber = 1;
				
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
			PlayMySFX (mySkill3SFX);
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
			switch (ActionNumber) {
			case 1 : Skill1(); break;
			case 2 : Skill2(); break;
			case 3 : Skill3(); break;
			case 4 : Skill4(); break;
			}
		}
		
	}
	
	public virtual void Skill1 () {
		//different in different character
		
		// Small Wave
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
			GameObject targetEnemy = null;
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				targetEnemy = Enemies [Random.Range (0, Enemies.Length)];
				if (targetEnemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD)
					break;
			}
			myTargetGameObject = targetEnemy;
		}
		
		//set myTargetPosition
		if (t_haveAlive) {
			myTargetPosition = myTargetGameObject.transform.position;
		}  else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
		//move to the top of the target
		Vector3 t_position = myTargetPosition.x * Vector3.right + startY * Vector3.up + CS_Global.POSITION_SKILL;
		
		GameObject t_Skill = Instantiate (mySkill1, t_position, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		
		CoolDown (at_CD);
	}
	
	public virtual void Skill2 () {
		//different in different character

		// Huge Wave
		Vector3 t_position = startY * Vector3.up + CS_Global.POSITION_SKILL;
		
		GameObject t_Skill = Instantiate (mySkill2, t_position, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);
		
		CoolDown (at_CD);
	}

	public virtual void Skill3 () {
		// Bubble 3
		
		int[] t_posNumberArray = {-1, -1, -1};
		for (int j = 0; j < t_posNumberArray.Length; j++) {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				//get a random number
				int t_posNumber = Random.Range (0, presetBubblePosition.Length);
				
				//check if the array has the same number
				bool hasSameNumber = false;
				for (int i = 0; i < t_posNumberArray.Length; i++) {
					if(t_posNumber == t_posNumberArray[i]) {
						hasSameNumber = true;
						break;
					}
				}
				
				//if doesn't have the same number, save number and break
				if (hasSameNumber == false) {
					t_posNumberArray[j] = t_posNumber;
					break;
				}
				
				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		}
		
		//create bubble
		for (int i = 0; i < t_posNumberArray.Length; i++) {
			Vector3 t_position = presetBubblePosition[t_posNumberArray[i]];
			Instantiate (mySkill3, t_position + CS_Global.POSITION_SKILL, Quaternion.identity);
		}
		
		CoolDown (at_CD);
	}

	public virtual void Skill4 () {
		// Bubble 2

		int[] t_posNumberArray = {-1, -1};
		for (int j = 0; j < t_posNumberArray.Length; j++) {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				//get a random number
				int t_posNumber = Random.Range (0, presetBubblePosition.Length);

				//check if the array has the same number
				bool hasSameNumber = false;
				for (int i = 0; i < t_posNumberArray.Length; i++) {
					if(t_posNumber == t_posNumberArray[i]) {
						hasSameNumber = true;
						break;
					}
				}

				//if doesn't have the same number, save number and break
				if (hasSameNumber == false) {
					t_posNumberArray[j] = t_posNumber;
					break;
				}

				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		}

		//create bubble
		for (int i = 0; i < t_posNumberArray.Length; i++) {
			Vector3 t_position = presetBubblePosition[t_posNumberArray[i]];
			Instantiate (mySkill3, t_position + CS_Global.POSITION_SKILL, Quaternion.identity);
		}
		
		CoolDown (at_CD);
	}
}