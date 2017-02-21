using UnityEngine;
using System.Collections;

public class CS_Text : MonoBehaviour {

	public string myCategory;
	public string myTitle;
	private Transform myTextShadow;

	// Use this for initialization
	void Start () {
		ShowText ();
	}

	public void ShowText () {
		if (myTitle == "")
			return;
		
		this.GetComponent<TextMesh> ().text = 
			CS_Caption.Instance.GetComponent<CS_Caption> ().LoadCaption (myCategory, myTitle);

		myTextShadow = this.transform.FindChild ("TX_Shadow");
		if (myTextShadow != null)
			myTextShadow.gameObject.SendMessage ("UpdateShadow");
	}

	public void SetTitle (string g_title) {
		myTitle = g_title;
		ShowText ();
	}

}
