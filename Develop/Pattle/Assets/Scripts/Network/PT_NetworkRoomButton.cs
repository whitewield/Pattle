using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PT_NetworkRoomButton : MonoBehaviour {
	[SerializeField] Text myText_Name;
	private PT_NetworkCanvas myCanvas;
	private string myIP;

	public void Init (PT_NetworkCanvas t_canvas, string t_ip, string t_name) {
		myCanvas = t_canvas;
		myIP = t_ip;
		myText_Name.text = t_name;
	}

	public void JoinRoom () {
		myCanvas.JoinRoom (myIP);
	}

}
