using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PT_TextShadow : MonoBehaviour {

	private Text myText;

	void Awake () {
		myText = this.GetComponent<Text> ();
	}
	
	public void UpdateShadow (Text g_target) {
		//set text
		myText.text = g_target.text;
		//set font size
		myText.fontSize = g_target.fontSize;
		//set font style
		myText.fontStyle = g_target.fontStyle;
		//set alignment
		myText.alignment = g_target.alignment;
	}
}
