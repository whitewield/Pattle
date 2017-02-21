using UnityEngine;
using System.Collections;

public class CS_Chess_AI_Fish : CS_Chess {
	private GameObject myCenter;
	private float myCenterRotateAngleSpeed = 10.0f;
	private float myCenterDistance = 1.8f;

	private float myCenterRotateAngleSpeed_Normal = 30;
	private float myCenterRotateAngleSpeed_Move = 180;
	private float myCenterRotateAngleSpeed_Attack = 180;

	protected int ActionNumber = -1;
	protected int ActionNumber_Last = -1;

	private Vector2[] presetPosition = {
		new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)};
	private Vector2[] presetPositionInDanger = {
		new Vector2(2, 0), new Vector2(-2, 0), new Vector2(0, 2), new Vector2(0, -2),
		new Vector2(1, 0)};

	public GameObject myYin;
	public GameObject myYinSkill;

	public override void CustomInitialize () {
		myCenter = new GameObject ("Fish_Center");
		myCenter.transform.position = Vector3.up * 5;
		myCenterRotateAngleSpeed = myCenterRotateAngleSpeed_Normal;
		//myCenterDistance = 2.0f;

		this.transform.SetParent (myCenter.transform);
		this.transform.localPosition = Vector3.left * CS_Global.CHESS_SIZE_S;

		myYin = Instantiate (myYin);
		myYin.transform.SetParent (myCenter.transform);
		myYin.transform.localPosition = this.transform.localPosition * -1;
		myYin.SendMessage ("SetMyYang", this.gameObject);
		myYin.SendMessage ("SetCurHp", at_CurHP);
	}

	public override void CustomUpdate () {
		if (process == CS_Global.PS_DEAD)
			return;
		myCenter.transform.Rotate (Vector3.forward, myCenterRotateAngleSpeed * Time.deltaTime);
		this.transform.rotation = Camera.main.transform.rotation;
		myYin.transform.rotation = Camera.main.transform.rotation;
		myYin.transform.localPosition = this.transform.localPosition * -1;
	}

	public override void CollisionAction (GameObject g_GO_Collision) {

		if (process == CS_Global.PS_DEAD)
			return;

		if (process == CS_Global.PS_ATTACK && 
			g_GO_Collision.tag == CS_Global.GetMyEnemyTag(this.tag)) {
			g_GO_Collision.SendMessage("DamageP",at_PDM);
		}
	}

	public override void DamageM (int g_MDM) {
		return;
	}

	public override void DoOnDamage () {
		myYin.SendMessage ("SetCurHp", at_CurHP);
	}

	public override void DoOnHeal () {
		myYin.SendMessage ("SetCurHp", at_CurHP);
	}

	public void Action () {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");

		if (at_CurHP >= (at_HP / 2)) {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;

				if (t_Number < 0.5f)
					ActionNumber = 0;
				else
					ActionNumber = 10;

				if (ActionNumber != ActionNumber_Last)
					break;

				if (t_Time == 0)
					Debug.LogError ("I Spend Too Much Time In This!");
			}
		}
		else {
			for (int t_Time = 999; t_Time >= 0; t_Time--) {
				float t_Number = Random.value;

				if (t_Number < 0.4f)
					ActionNumber = 9;
				else if (t_Number < 0.6f)
					ActionNumber = 10;
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
			myYin.SendMessage ("Move");
			PlayMySFX (myMoveSFX);
			break;
		case 10:
			Attack ();
			myYin.SendMessage ("Attack");
			PlayMySFX (myAttackSFX);
			break;
		case 1:
			Cast ();
			myYin.SendMessage ("Cast");
			PlayMySFX (myCastSFX);
			break;
		case 9:
			MoveInDanger ();
			myYin.SendMessage ("Move");
			PlayMySFX (myMoveSFX);
			break;
		}
	}

	public override void Idle () {
		SetProcess (CS_Global.PS_IDLE);
		myYin.SendMessage ("Idle");

		// if the Boss is idle, take action
		Action ();

	}

	public override void CoolDown (float g_time) {
		myCenterRotateAngleSpeed = myCenterRotateAngleSpeed_Normal;

		SetProcess (CS_Global.PS_CD);
		timer = g_time;
	}

	public override void Move () {
		myCenterRotateAngleSpeed = myCenterRotateAngleSpeed_Move;

		SetProcess (CS_Global.PS_MOVE);

		//Set Move Tatget Position
		int t_Number = 0;
		Vector2 t_myPos = this.transform.localPosition;

		int t_DoWhileBreakTime = 1000;
		do {
			t_DoWhileBreakTime--;
			if (t_DoWhileBreakTime <= 0) {
				Debug.LogError ("Break, I Spend Too Much Time In This Do While!");
				break;
			}

			t_Number = Random.Range (0, presetPosition.Length); 
		} while(presetPosition [t_Number] * myCenterDistance == t_myPos);

		myTargetPosition = presetPosition [t_Number] * myCenterDistance;

	}

	public void MoveInDanger () {
		myCenterRotateAngleSpeed = myCenterRotateAngleSpeed_Move;

		SetProcess (CS_Global.PS_MOVE);

		//Set Move Tatget Position
		int t_Number = 0;
		Vector2 t_myPos = this.transform.localPosition;

		int t_DoWhileBreakTime = 1000;
		do {
			t_DoWhileBreakTime--;
			if (t_DoWhileBreakTime <= 0) {
				Debug.LogError ("Break, I Spend Too Much Time In This Do While!");
				break;
			}

			t_Number = Random.Range (0, presetPositionInDanger.Length); 
		} while(presetPositionInDanger [t_Number] * myCenterDistance == t_myPos);

		myTargetPosition = presetPositionInDanger [t_Number] * myCenterDistance;

	}

	public override void Attack () {
		myCenterRotateAngleSpeed = myCenterRotateAngleSpeed_Attack;
		SetProcess (CS_Global.PS_ATTACK);

		myPosition = myCenter.transform.position;

		myTargetPosition = Vector3.down * 5;

	}

	public virtual void Skill1 () {
		//Shoot
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(CS_Global.GetMyEnemyTag(this.tag));
		if (Enemies.Length == 0) {
			CoolDown (at_CD);
			myYin.SendMessage ("CoolDown", at_CD);
			return;
		}
		myTargetGameObject = Enemies[0];

		bool t_haveAlive = false;

		//get an undead enemy
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD) {
				t_haveAlive = true;
				myTargetGameObject = Enemy;
			}
		}

		//choose the enemy with lowest HP
		foreach (GameObject Enemy in Enemies) {
			if (Enemy.GetComponent<CS_Chess>().GetProcess() != CS_Global.PS_DEAD &&
				Enemy.GetComponent<CS_Chess>().GetCurHP() < myTargetGameObject.GetComponent<CS_Chess>().GetCurHP()) {
				myTargetGameObject = Enemy;
			}
		}

		//set myTargetPosition
		if (t_haveAlive) {
			myTargetPosition = myTargetGameObject.transform.position;
		}  else {
			myTargetPosition = Enemies [Random.Range (0, Enemies.Length)].transform.position;
		}
			
		GameObject t_Skill = Instantiate (mySkill, this.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetTargetPosition", myTargetPosition);
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);

		GameObject t_yinSkill = Instantiate (myYinSkill, myYin.transform.position + CS_Global.POSITION_SKILL, Quaternion.identity) as GameObject;
		t_yinSkill.SendMessage ("SetTargetPosition", myTargetPosition);
		t_yinSkill.SendMessage ("SetMyCaster", myYin);

		CoolDown (at_CD);
		myYin.SendMessage ("CoolDown", at_CD);
	}

	public override void UpdateAction_Move () {
		//move the chess
		this.transform.localPosition = 
			Vector2.Lerp (this.transform.localPosition, myTargetPosition, CS_Global.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.localPosition, myTargetPosition) <= CS_Global.DISTANCE_RESET) {
			//set my position
			this.transform.localPosition = myTargetPosition;
			myPosition = myTargetPosition;

			//start cool down
			CoolDown (at_CD / 2);
			myYin.SendMessage ("CoolDown", at_CD / 2);
		}
	}

	public override void UpdateAction_Attack () {
		//different in different character
		//move the chess
		myCenter.transform.position = 
			Vector2.Lerp (myCenter.transform.position, myTargetPosition, CS_Global.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (myCenter.transform.position, myTargetPosition) <= CS_Global.DISTANCE_RESET) {
			//Reset and come back
			AttackBack();
		}
	}

	public override void UpdateAction_AttackBack () {
		//move the chess
		myCenter.transform.position = 
			Vector2.Lerp (myCenter.transform.position, myPosition, CS_Global.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (myCenter.transform.position, myPosition) <= CS_Global.DISTANCE_RESET) {
			//set my position
			myCenter.transform.position = myPosition;

			//start cool down
			CoolDown (at_CD);
			myYin.SendMessage ("CoolDown", at_CD);
		}
	}

	public override void UpdateAction_CT () {
		//different in different character
		if (timer <= 0) {
			switch(ActionNumber)
			{
			case 1 : Skill1(); break;
			}
		}

	}
}
