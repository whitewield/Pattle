using UnityEngine;
using System.Collections;

public class CS_Button : MonoBehaviour {
	public GameObject messageReceiver;
	public string messageFunction;
	public string myMessage;

	void OnMouseDown () {
		if (messageReceiver == null)
			messageReceiver = GameObject.Find (CS_Global.NAME_MESSAGEBOX);

		if (myMessage == "")
			messageReceiver.SendMessage (messageFunction);
		else
			messageReceiver.SendMessage (messageFunction, myMessage);

		//Debug.Log ("MouseDown");
	}
}
