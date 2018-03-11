using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PT_MessageManager_Box : MonoBehaviour {
	[SerializeField] Text myText;

	void Awake () {
		if (myText == null)
			myText = this.GetComponentInChildren<Text> ();
	}

	public void ShowText (string g_text) {
		myText.text = g_text;
		this.gameObject.SetActive (true);
	}

	public void Hide () {
		this.gameObject.SetActive (false);
	}
}
