using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CS_AdventureStar : MonoBehaviour {

	[SerializeField] Vector3 deltaPosition;
	[SerializeField] float myScale = 1;
	[SerializeField] GameObject star_Done;
	[SerializeField] GameObject star_Perfect;
	[SerializeField] GameObject star_Done_Hard;
	[SerializeField] GameObject star_Perfect_Hard;

	void Start () {
		this.transform.localScale = Vector3.one * myScale;
	}

	public void SetStarStatus (string g_status) {
		star_Done.SetActive (false);
		star_Perfect.SetActive (false);
		star_Done_Hard.SetActive (false);
		star_Perfect_Hard.SetActive (false);

		if (g_status == CS_Global.STAR_UNDONE) {
//			Debug.Log("star undone");
		} else if (g_status == CS_Global.STAR_DONE) {
//			Debug.Log("star done");
			star_Done.SetActive (true);
		} else if (g_status == CS_Global.STAR_PERFECT) {
//			Debug.Log("star perfect");
			star_Perfect.SetActive (true);
		} else if (g_status == CS_Global.STAR_DONE_HARD) {
//			Debug.Log("star done hard");
			star_Done_Hard.SetActive (true);
		} else if (g_status == CS_Global.STAR_PERFECT_HARD) {
//			Debug.Log("star perfect hard");
			star_Perfect_Hard.SetActive (true);
		} else
			Debug.LogError ("What are you sending to me? " + g_status + "?");
	}

	public void SetTransform (Transform g_chess_transform) {
		this.transform.SetParent (g_chess_transform);
		this.transform.localPosition = deltaPosition;
		this.transform.localScale = Vector3.one * myScale;
//		Debug.Log (this.transform.name + " mylocalscale:" + this.transform.localScale + ":" + myScale);
	}
}
