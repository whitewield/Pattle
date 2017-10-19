using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_ProcessDisplay : MonoBehaviour {
	private PT_Global.Process myProcess = PT_Global.Process.None;

	[SerializeField] Transform myTimerTransform;
	[SerializeField] SpriteRenderer myTimerSpriteRenderer;
	[SerializeField] Color myColor_CD;
	[SerializeField] Color myColor_CT;

	private float myDeltaTimeRatio;
	private float myTimer;

	[SerializeField] TextMesh myHPTextMesh;

	[SerializeField] GameObject myStatus_Freeze;
	private float myStatus_Freeze_EndTime;

	[SerializeField] GameObject myStatus_Gold;
	private float myStatus_Gold_EndTime;


//	[SerializeField] Transform myIdleTransform;

	#region Process
	public void HideProcess () {
		myTimerTransform.localScale = Vector3.zero;
	}

	public void ShowCD (float g_time) {
//		Debug.Log ("ShowCD: " + g_time);

		myProcess = PT_Global.Process.CD;
		myTimerSpriteRenderer.color = myColor_CD;

		if (g_time != 0) {
			myTimer = 0;
			myDeltaTimeRatio = 1 / g_time;
		} else {
			myTimer = 1;
		}

		myTimerTransform.localScale = Vector3.one;
	}

	public void ShowCT (float g_time) {
//		Debug.Log ("ShowCT");

		myProcess = PT_Global.Process.CT;
		myTimerSpriteRenderer.color = myColor_CT;

		if (g_time != 0) {
			myTimer = 0;
			myDeltaTimeRatio = 1 / g_time;
		} else {
			myTimer = 1;
		}

		myTimerTransform.localScale = Vector3.zero;
	}

	public void ShowStatus_Freeze (float g_time) {
		//		Debug.Log ("ShowCD: " + g_time);
		myStatus_Freeze_EndTime = Mathf.Max (myStatus_Freeze_EndTime, g_time + Time.timeSinceLevelLoad);
		if (myStatus_Freeze.activeSelf == false) {
			myStatus_Freeze.SetActive (true);
		}
	}

	public void ShowStatus_Gold (float g_time) {
		//		Debug.Log ("ShowCD: " + g_time);
		myStatus_Gold_EndTime = Mathf.Max (myStatus_Gold_EndTime, g_time + Time.timeSinceLevelLoad);
		if (myStatus_Gold.activeSelf == false) {
			myStatus_Gold.SetActive (true);
		}
	}
		
	private void Update () {
		if (myStatus_Freeze.activeSelf == true && 
			myStatus_Freeze_EndTime < Time.timeSinceLevelLoad) {
			myStatus_Freeze.SetActive (false);
		}

		if (myStatus_Gold.activeSelf == true && 
			myStatus_Gold_EndTime < Time.timeSinceLevelLoad) {
			myStatus_Gold.SetActive (false);
		}


		if (myStatus_Freeze_EndTime > Time.timeSinceLevelLoad || 
			myStatus_Gold_EndTime > Time.timeSinceLevelLoad) {
			return;
		}

		switch (myProcess) {
		case PT_Global.Process.CT:
			Update_CT ();
			break;
		case PT_Global.Process.CD:
			Update_CD ();
			break;
		default:
			break;
		}
	}

	private void Update_CD () {

		myTimer += Time.deltaTime * myDeltaTimeRatio;

		if (myTimer > 1) {
			myTimerTransform.localScale = Vector3.zero;
			myProcess = PT_Global.Process.None;
			return;
		}

		myTimerTransform.localScale = Vector3.one * (1 - myTimer);
	}

	private void Update_CT () {

		myTimer += Time.deltaTime * myDeltaTimeRatio;

		if (myTimer > 1) {
			myTimerTransform.localScale = Vector3.one;
			myProcess = PT_Global.Process.None;
			return;
		}

		myTimerTransform.localScale = Vector3.one * myTimer;
	}
	#endregion

	#region HP

	public void ShowHP (int g_HP) {
		if (g_HP == 0)
			myHPTextMesh.text = "0";
		else
			myHPTextMesh.text = g_HP.ToString ("#");
	}

	public void HideHP () {
		myHPTextMesh.text = "";
	}

	#endregion

	[SerializeField] float myRandomRadius = 0.5f;
	private Vector2 lastRandomPosition;
	[SerializeField] GameObject myDamagePrefab;
	[SerializeField] GameObject myHealingPrefab;

	public void ShowDamage (int g_number) {
		//Show Damage GameObject
		GameObject t_go_damage = Instantiate (myDamagePrefab, this.transform) as GameObject;
		t_go_damage.transform.rotation = Camera.main.transform.rotation;
		t_go_damage.GetComponent<TextMesh> ().text = "-" + g_number.ToString();
		SetRandomPosition (t_go_damage);
	}

	public void ShowHealing (int g_number) {
		//Show Heal GameObject
		GameObject t_go_heal = Instantiate (myHealingPrefab, this.transform) as GameObject;
		t_go_heal.transform.rotation = Camera.main.transform.rotation;
		t_go_heal.GetComponent<TextMesh> ().text = "+" + g_number.ToString();
		SetRandomPosition (t_go_heal);
	}

	private void SetRandomPosition (GameObject g_object) {
		//t_go_damage.transform.localScale = Vector3.one * scale;

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
		g_object.transform.localPosition = t_randomPosition * myRandomRadius;
	}
}
