using UnityEngine;
using System.Collections;

namespace Pattle {
	namespace Action {

		[CreateAssetMenu(fileName = "MoveSettings", menuName = "Wield/MoveSettings", order = 1)]
		public class SO_MoveSettings : ScriptableObject {

			/// <summary>
			/// Comment
			/// </summary>
			[TextArea(1,10)]
			public string Comment;

			/// <summary>
			/// An array of ActionWeight.
			/// </summary>
			public Vector3[] Moves;

			public Vector3 GetStartPosition () {
				return Moves [0];
			}

			public Vector3 GetRandomMovePosition () {
				return Moves [Random.Range (0, Moves.Length)];
			}

			public Vector3 GetOtherRandomMovePosition (Vector3 g_position, float g_sqrMinDistance = 0.01f) {

				Vector3 f_pos;

				for (int f_loopTime = 0; f_loopTime < 1000; f_loopTime++) {

					f_pos = GetRandomMovePosition ();

					//if the distance between old and new pos is big enough, return the value
					if (Vector3.SqrMagnitude(f_pos - g_position) > g_sqrMinDistance) {
						return f_pos;
					}

					if (f_loopTime == 1000) {
						Debug.LogError ("I Spend Too Much Time In This Loop!");
					}
				}

				return Moves [0];
			}
		}
	}
}