using UnityEngine;
using System.Collections;

public class CS_MouseDown : MonoBehaviour {
	private GameObject myManager;
	// Use this for initialization
	void Start () {
		myManager = GameObject.Find (CS_Global.NAME_INPUTMANAGER);
	}

	void OnMouseDown() {
		if (myManager == null)
			myManager = GameObject.Find (CS_Global.NAME_INPUTMANAGER);
		myManager.SendMessage ("SetGO", this.gameObject);
	}

	void OnMouseUp () {
		if (myManager == null)
			myManager = GameObject.Find (CS_Global.NAME_INPUTMANAGER);
		myManager.SendMessage ("UnsetGO", this.gameObject);
	}
}
