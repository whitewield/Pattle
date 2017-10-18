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

	private float myPauseTime;

	[SerializeField] TextMesh myHPTextMesh;

//	[SerializeField] Transform myIdleTransform;

	#region Process
	public void HideProces () {
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

	public void PauseProcess (float g_time) {
		//		Debug.Log ("ShowCD: " + g_time);

		myPauseTime = Mathf.Max (myPauseTime, g_time);
	}
		
	private void Update () {
		if (myPauseTime > 0) {
			myPauseTime -= Time.deltaTime;
		} else {
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

	public void ShowHP (int g_HP) {
		if (g_HP == 0)
			myHPTextMesh.text = "0";
		else
			myHPTextMesh.text = g_HP.ToString ("#");
	}

	public void HideHP () {
		myHPTextMesh.text = "";
	}
}
