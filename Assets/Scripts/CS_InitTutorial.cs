using UnityEngine;
using System.Collections;

public class CS_InitTutorial : MonoBehaviour {

	void Start () {
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("InitTutorial");
	}
}
