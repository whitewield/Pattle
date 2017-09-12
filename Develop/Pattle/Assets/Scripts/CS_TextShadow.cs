using UnityEngine;
using System.Collections;

public class CS_TextShadow : MonoBehaviour {
	void Start () {
		UpdateShadow ();
	}

	public void UpdateShadow () {
		TextMesh t_myTextMesh = this.GetComponent<TextMesh> ();
		TextMesh t_myParentTextMesh = this.transform.parent.GetComponent<TextMesh> ();
		//set text
		t_myTextMesh.text = t_myParentTextMesh.text;
		//set font size
		t_myTextMesh.fontSize = t_myParentTextMesh.fontSize;
		//set font style
		t_myTextMesh.fontStyle = t_myParentTextMesh.fontStyle;
		//set archor
		t_myTextMesh.anchor = t_myParentTextMesh.anchor;
	}
}
