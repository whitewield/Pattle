using UnityEngine;
using System.Collections;

public class CS_Chess_Tutorial_JackStraw_FireBall : CS_Chess {

	//For move

	public Vector2[] presetPosition = {new Vector2(0, 5), new Vector2(3, 5), new Vector2(-3, 5)};

	private bool isTimerOn = true;
	private float myTimer = 30;
	[SerializeField] TextMesh TX_Timer; 

	private GameObject myEnemy;

	public override void CustomInitialize () {
		myEnemy = GameObject.FindGameObjectWithTag (CS_Global.GetMyEnemyTag (this.tag));
		ShowHP ();
	}

	public override void CustomUpdate () {
		if (isTimerOn == false)
			return;

		if (myEnemy.GetComponent<CS_Chess> ().GetProcess () == CS_Global.PS_DEAD) {
			isTimerOn = false;
			GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("Lose");
		}

		if (myTimer > 0) {
			myTimer -= Time.deltaTime;
		} else {
			myTimer = 0;
			isTimerOn = false;
			GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("Win");
		}

		TX_Timer.text = myTimer.ToString ("f1");
	}

	public override void DoOnDead () {
		GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("Win");
	}

	public void Action () {
		Attack ();
		PlayMySFX (myAttackSFX);
	}

	public override void Idle () {
		SetProcess (CS_Global.PS_IDLE);
		
		// if the Boss is idle, take action
		Cast ();
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

	public override void UpdateAction_CT () {
		//different in different character

		if (timer <= 0)
			Skill ();
	}

	public void Skill () {
		//different in different character
		//Debug.Log (CS_Global.GetMyEnemyTag (this.tag));

		if (myEnemy == null) {
			myEnemy = GameObject.FindGameObjectWithTag (CS_Global.GetMyEnemyTag (this.tag));

			if (myEnemy == null) {
				CoolDown (at_CD);
				return;
			}
		}

		myTargetPosition = myEnemy.transform.position;

		Vector3 t_position = myTargetPosition;
		t_position += CS_Global.POSITION_SKILL;
		GameObject t_Skill = Instantiate (mySkill, t_position, Quaternion.identity) as GameObject;
		t_Skill.SendMessage ("SetMyCaster", this.gameObject);

		CoolDown (at_CD);
	}

//	private bool GetEnemy () {
//		
//	}
}
