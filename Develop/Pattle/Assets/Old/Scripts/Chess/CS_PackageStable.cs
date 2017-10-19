using UnityEngine;
using System.Collections;

public class CS_PackageStable : MonoBehaviour {

	public float randomRadius = 1.0f;
	private Vector2 lastRandomPosition;
	//public float scale = 1.0f;
	public GameObject GO_Damage;
	public GameObject GO_Heal;

	public void ShowDamage (int g_number) {
		//Show Damage GameObject
		GameObject t_go_damage = Instantiate (GO_Damage) as GameObject;
		t_go_damage.GetComponent<TextMesh> ().text = g_number.ToString();
		SetRandomPosition (t_go_damage);
	}

	public void ShowHeal (int g_number) {
		//Show Heal GameObject
		GameObject t_go_heal = Instantiate (GO_Heal) as GameObject;
		t_go_heal.GetComponent<TextMesh> ().text = "+" + g_number.ToString();
		SetRandomPosition (t_go_heal);
	}

	private void SetRandomPosition (GameObject g_object) {
		//t_go_damage.transform.localScale = Vector3.one * scale;

		//apply to the chess
		g_object.transform.SetParent (this.transform);

		//get random position
		Vector2 t_randomPosition = Random.insideUnitCircle;

		//reset position
//		if (lastRandomPosition == null)
//			lastRandomPosition = t_randomPosition;
//		else {
//			Vector2 t_sameDirection = lastRandomPosition.normalized + t_randomPosition.normalized;
//			Debug.Log (t_sameDirection);
//			if (Mathf.Abs(t_sameDirection.x) < 0.5f) {
//				t_randomPosition += (t_randomPosition.x * Vector2.left) * 2;
//				Debug.Log ("SetRandomPosition x");
//			}
//			if (Mathf.Abs(t_sameDirection.y) < 0.5f) {
//				t_randomPosition += (t_randomPosition.y * Vector2.down) * 2;
//				Debug.Log ("SetRandomPosition y");
//			}
//		}

		//set random position
		g_object.transform.localPosition = t_randomPosition * randomRadius;
	}
}
