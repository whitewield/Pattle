using UnityEngine;
using System.Collections;

namespace PT_Action {
	
	[CreateAssetMenu(fileName = "ActionWeightSettings", menuName = "Wield/ActionWeightSettings", order = 1)]
	public class SO_ActionWeightSettings : ScriptableObject {

		/// <summary>
		/// Comment, what is skill_1/skill_2/...
		/// </summary>
		[TextArea(1,10)]
		public string Comment;

		/// <summary>
		/// An array of ActionWeight.
		/// </summary>
		public ActionWeight[] ActionWeights;

		public ActionType GetRandomAction () {
			int f_randomNumber = 0;
			foreach (ActionWeight f_weight in ActionWeights) {
				f_randomNumber += f_weight.myWeight;
			}
			f_randomNumber = Random.Range (0, f_randomNumber);

			for (int i = 0; i < ActionWeights.Length; i++) {
				f_randomNumber -= ActionWeights [i].myWeight;
				if (f_randomNumber < 0)
					return ActionWeights [i].myActionType;
			}

			Debug.LogError ("didn't find the random action!");
			return ActionType.Move;
		}
	}



	public enum ActionType {
		Move,
		Attack,
		Skill_1,
		Skill_2,
		Skill_3,
		Skill_4,
		Skill_5,
	}

	[System.Serializable]
	public struct ActionWeight {
		public ActionType myActionType;
		public int myWeight;
	}

}