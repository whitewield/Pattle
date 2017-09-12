using UnityEngine;
using System.Collections;

public class CS_Chess_AI_IceTotem : CS_Chess {
	
	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;

	public float at_CDNormal = 3;
	public float at_CDInDanger = 1;
	
	//For move
	//public Vector2[] presetPosition = {new Vector2(0, 5), new Vector2(3, 7), new Vector2(-3, 7)};
	
	public GameObject mySkill1;
	public GameObject mySkill2;

	private GameObject myMaster;


	public CS_AudioClip mySkill1SFX;
	public CS_AudioClip mySkill2SFX;

	public override void CollisionAction (GameObject g_GO_Collision) {
		
		if (process == CS_Global.PS_DEAD)
			return;
	}
	
	public void Action () {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");
		if (myMaster.GetComponent<CS_Chess_AI_IceMage> ().IsHalfDead ()) {
			
			Debug.Log ("IceMage In Danger");
			at_CD = at_CDInDanger;
			
			float t_Number = Random.value;
			
			if (t_Number < 0.3f)
				ActionNumber = 1;
			else
				ActionNumber = 2;
		} else if (at_CurHP < at_HP || myMaster.GetComponent<CS_Chess_AI_IceMage> ().IsDamaged ()) {

			at_CD = at_CDNormal;
			
			float t_Number = Random.value;
			
			if (t_Number < 0.5f)
				ActionNumber = 1;
			else
				ActionNumber = 2;
		} else {
			at_CD = at_CDNormal;

			ActionNumber = 1;
		}
		
		ActionNumber_Last = ActionNumber;
		
		//ActionNumber = 10;
		switch (ActionNumber) {
//		case 0:
//			Move ();
//			break;
		case 10:
			Attack ();
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
	
//	public override void Move () {
//		SetProcess (CS_Global.PS_MOVE);
//		
//		//Set Move Tatget Position
//		int t_Number = 0;
//		Vector2 t_myPos = this.transform.position;
//		
//		int t_DoWhileBreakTime = 1000;
//		do {
//			t_DoWhileBreakTime --;
//			if(t_DoWhileBreakTime <= 0) {
//				Debug.LogError("Break, I Spend Too Much Time In This Do While!");
//				break;
//			}
//			
//			t_Number = Random.Range (0, presetPosition.Length); 
//		} while(presetPosition[t_Number] == t_myPos);
//		
//		myTargetPosition = presetPosition [t_Number];
//		
//		myCollider.isTrigger = true;
//	}
	
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
		//Debug.Log (CS_Global.GetMyEnemyTag (this.tag));

		//Shoot 7 Ice Arrow
		for (int i = 1; i < 8; i++) {
			//Debug.Log (i + ":" + (i-4) + "," + (Mathf.Abs(i - 4) - 4));
			Vector2 t_myPos = this.transform.position;
			Vector2 t_myTargetPos = 
				(i - 4) * Vector2.right + 
					(Mathf.Abs(i - 4) - 4) * Vector2.up + 
					t_myPos;
			GameObject t_Skill = Instantiate (mySkill1, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
			t_Skill.SendMessage("SetMyCaster", this.gameObject);
			t_Skill.SendMessage("SetDirection", t_myTargetPos);
		}
		
		CoolDown (at_CD);
	}
	
	public virtual void Skill2 () {
		//different in different character

		//Heal Self and Ice Mage
		Heal (at_MDM);
		if (myMaster != null)
			myMaster.SendMessage ("Heal", at_MDM);

		//Instantiate (mySkill2, this.transform.position, Quaternion.identity);
		//Instantiate (mySkill2, myMaster.transform.position, Quaternion.identity);

		CoolDown (at_CD);
	}

	public void SetMyMaster (GameObject g_myMaster) {
		myMaster = g_myMaster;
	}

	public override void DoOnDead () {
		myMaster.SendMessage ("TotemIsDead");
	}
}

