using UnityEngine;
using System.Collections;

public class CS_InputBag : CS_Control {

	public GameObject GO_X;
	public GameObject myInfoBook;

	void Start () {
		this.name = CS_Global.NAME_INPUTMANAGER;
		if (myInfoBook == null)
			myInfoBook = GameObject.Find (CS_Global.NAME_INFOBOOK);
	}
	
	void Update () {
	
	}

	public override void SetGO (GameObject g_GO) {
		myInfoBook.SetActive (true);
		myInfoBook.SendMessage ("Show", g_GO);

//		Debug.Log("on btn chess");
	}

	private void Action () {

	}

	public void SwitchPage () {
		//used by CS_BagPageManager : do not delete 
	}
}
