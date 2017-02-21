using UnityEngine;
using System.Collections;

public class CS_Chess_Tutorial_JackStraw : CS_Chess {
	public bool isDamaged = false;

	//For move
	public Vector2[] presetPosition = {new Vector2(0, 5), new Vector2(3, 5), new Vector2(-3, 5)};

	public override void CustomInitialize () {
		if (isDamaged == true)
			at_CurHP = 1;
		ShowHP ();
	}

	public override void DoOnHeal () {
		if (isDamaged == true && at_CurHP == at_HP) {
			GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("Win");
		}
	}

	public override void DoOnDead () {
		if (isDamaged == false) {
			GameObject.Find (CS_Global.NAME_AFTERBATTLE).SendMessage("Win");
		}
	}

	public void Action () {
		Move ();
		PlayMySFX (myMoveSFX);
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
}
