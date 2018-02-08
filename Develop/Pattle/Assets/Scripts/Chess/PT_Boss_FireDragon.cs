using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;
using PT_Action;

public class PT_Boss_FireDragon : PT_BaseBoss {

	[SerializeField] SO_ActionWeightSettings myActionWeights_Normal;
	[SerializeField] SO_ActionWeightSettings myActionWeights_Low;

	[SerializeField] protected GameObject mySkill_1_Fireball;
	[SerializeField] protected GameObject mySkill_2_FireShoot;



	protected override void ActionAI () {
		for (int f_loopTime = 0; f_loopTime < 1000; f_loopTime++) {

			if (GetCurHP () >= (myAttributes.HP / 2)) {
				myActionType = myActionWeights_Normal.GetRandomAction ();
			} else {
				myActionType = myActionWeights_Low.GetRandomAction ();
			}

			if (myActionType != myLastActionType) {
				break;
			}

			if (f_loopTime == 1000) {
				Debug.LogError ("I Spend Too Much Time In This Loop!");
			}
		}

		myLastActionType = myActionType;

		//ActionNumber = 10;
		switch (myActionType) {
		case ActionType.Move:
			Move ();
			break;
		case ActionType.Attack:
			Attack ();
//			PlayMySFX (myAttackSFX);
			break;
		case ActionType.Skill_1:
			Cast ();
//			PlayMySFX (mySkill1SFX);
			break;
		case ActionType.Skill_2:
			Cast ();
//			PlayMySFX (mySkill2SFX);
			break;
		}
	}

}
