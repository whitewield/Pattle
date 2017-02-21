using UnityEngine;
using System.Collections;

public class CS_ButtonLanguage : MonoBehaviour {

	public string myLanguage;
	public GameObject myBackGround;

	private Color color_on = new Color (0.64f, 0.72f, 0.8f);
	private Color color_off = Color.gray;

	void Start () {
		if (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_SETTINGS, CS_Global.SAVE_TITLE_LANGUAGE) == myLanguage) {
			myBackGround.GetComponent<SpriteRenderer> ().color = color_off;
			this.GetComponent<BoxCollider2D> ().enabled = false;
		}
		else
			myBackGround.GetComponent<SpriteRenderer> ().color = color_on;
	}

	void OnMouseDown () {
		CS_Caption.Instance.SetCaptionLanguage (myLanguage);
		CS_Caption.Instance.LoadCaptionLanguage ();

		CS_MessageBox.Instance.LoadScene ("Menu");
		//Debug.Log ("MouseDown");
	}
}
