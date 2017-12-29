using UnityEngine;
using System.Collections;

public class PT_Text : MonoBehaviour {

	private TextMesh myTextMesh;
	[SerializeField] string myCategory;
	[SerializeField] string myTitle;
	private Transform myTextShadow;

	// Use this for initialization
	void Awake () {
		myTextMesh = this.GetComponent<TextMesh> ();
	}

	void Start () {
		ShowText ();
	}

	public void ShowText () {
		if (myCategory == "")
			return;

		if (myTitle == "")
			return;
		
		myTextMesh.text = 
			PT_Caption.Instance.LoadCaption (myCategory, myTitle);

//		myTextShadow = this.transform.Find ("TX_Shadow");
//		if (myTextShadow != null)
//			myTextShadow.gameObject.SendMessage ("UpdateShadow");
	}

	public void SetCategory (string g_title) {
		myCategory = g_title;
//		ShowText ();
	}

	public void SetTitle (string g_title) {
		myTitle = g_title;
		ShowText ();
	}

}
