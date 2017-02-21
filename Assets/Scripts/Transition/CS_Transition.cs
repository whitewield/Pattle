using UnityEngine;
using System.Collections;

public class CS_Transition : MonoBehaviour {
	public GameObject myTranstionManager;

	public void GoToNextScene () {
		if (myTranstionManager == null)
			myTranstionManager = GameObject.Find (CS_Global.NAME_TRANSITIONMANAGER);
		myTranstionManager.SendMessage ("GoToNextScene");
	}
}
