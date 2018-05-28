using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PT_Text : MonoBehaviour {

	[SerializeField] Text myText;
	[SerializeField] string myCategory;
	[SerializeField] string myTitle;
	[SerializeField] PT_TextShadow myTextShadow;
	[SerializeField] bool isAllCaps;

	// Use this for initialization
	void Awake () {
		if (myText == null)
			myText = this.GetComponentInChildren<Text> ();
	}

	void Start () {
		myText.font = PT_Caption.Instance.GetFont ();
		SetText ();
	}

	public void SetText () {
		if (myCategory == "")
			return;

		if (myTitle == "")
			return;
		
		SetText (PT_Caption.Instance.LoadCaption (myCategory, myTitle));
	}

	public void SetColor (Color g_color) {
		myText.color = g_color;
	}

	public void SetText (string g_text) {
		if (isAllCaps)
			myText.text = g_text.ToUpper ();
		else
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
		SetText ();
	}

}
