using UnityEngine;
using System.Collections;

public class CS_Unlock : MonoBehaviour {

	[SerializeField] string myCategory;
	[SerializeField] string[] myTitles;

	// Use this for initialization
	void Start () {
		foreach (string t_title in myTitles) {
			if (CS_GameSave.LoadGame (myCategory, t_title) == "0") {
				this.gameObject.SetActive (false);
				return;
			}
		}
	}
}
