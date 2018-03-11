using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PT_Text : MonoBehaviour {

	[SerializeField] Text myText;
	[SerializeField] string myCategory;
	[SerializeField] string myTitle;
	[SerializeField] PT_TextShadow myTextShadow;

	// Use this for initialization
	void Awake () {
		if (myText == null)
			myText = this.GetComponentInChildren<Text> ();
	}

	void Start () {
		ShowText ();
	}

	public void ShowText () {
		if (myCategory == "")
			return;

		if (myTitle == "")
			return;
		
		ShowText (PT_Caption.Instance.LoadCaption (myCategory, myTitle));
	}

	public void ShowText (string g_text) {
		myText.text = g_text;

		//		myTextShadow = this.GetComponentInChildren<PT_TextShadow> ();
		if (myTextShadow != null)
			myTextShadow.UpdateShadow (myText);
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
