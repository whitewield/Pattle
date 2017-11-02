using UnityEngine;
using System.Collections;

public class PT_Text : MonoBehaviour {

	[SerializeField] string myCategory;
	[SerializeField] string myTitle;
	private Transform myTextShadow;

	// Use this for initialization
	void Start () {
		ShowText ();
	}

	public void ShowText () {
		if (myTitle == "")
			return;
		
		this.GetComponent<TextMesh> ().text = 
			PT_Caption.Instance.LoadCaption (myCategory, myTitle);

		myTextShadow = this.transform.Find ("TX_Shadow");
		if (myTextShadow != null)
			myTextShadow.gameObject.SendMessage ("UpdateShadow");
	}

	public void SetTitle (string g_title) {
		myTitle = g_title;
		ShowText ();
	}

}
