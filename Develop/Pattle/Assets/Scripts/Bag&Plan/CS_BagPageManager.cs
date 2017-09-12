using UnityEngine;
using System.Collections;

public class CS_BagPageManager : MonoBehaviour {
	public int myPage;
	private Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SwitchPage (string myPageName)
	{
		if (myPageName == CS_Global.CLASS_BLOOD)
			myPage = 1;
		else if (myPageName == CS_Global.CLASS_MAGIC)
			myPage = 2;
		else if (myPageName == CS_Global.CLASS_NATURE)
			myPage = 3;
		else if (myPageName == CS_Global.CLASS_LIGHT)
			myPage = 4;

		myAnimator.SetInteger ("myPage", myPage);

		//deselect when selecting BtnChess
		GameObject.Find(CS_Global.NAME_INPUTMANAGER).SendMessage("SwitchPage");
	}
}
