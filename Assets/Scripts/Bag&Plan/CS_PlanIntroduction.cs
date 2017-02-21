using UnityEngine;
using System.Collections;

public class CS_PlanIntroduction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GameObject.Find (CS_Global.NAME_MESSAGEBOX).GetComponent<CS_MessageBox> ().planIntroWatched == true)
			Hide ();
	}
	
	void OnMouseDown () {
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).GetComponent<CS_MessageBox> ().planIntroWatched = true;
		Hide ();
	}

	public void Show () {
		this.gameObject.SetActive (true);
	}

	public void Hide () {
		this.gameObject.SetActive (false);
	}
}
