//CheesyCamera ver.1.0
//made by Hang Ruan, Kevin Zeng
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheesyCanvas : MonoBehaviour {
	private CanvasScaler myCanvasScaler;

	private Vector2 myLastScreenSize = Vector2.zero;

	private Vector2 myDefaultSize;

	// Use this for initialization
	void Start () {
		myCanvasScaler = this.GetComponent<CanvasScaler> ();
		myDefaultSize = myCanvasScaler.referenceResolution;
		UpdateResize ();
		myLastScreenSize = new Vector2 (Screen.width, Screen.height);
	}

	// Update is called once per frame
	void Update () {
		if (Screen.width != myLastScreenSize.x || Screen.height != myLastScreenSize.y) {
			UpdateResize ();
			myLastScreenSize = new Vector2 (Screen.width, Screen.height);
		}
	}

	private void UpdateResize () {

		if ((float)Screen.height / (float)Screen.width == myDefaultSize.y / myDefaultSize.x) {
			myCanvasScaler.referenceResolution = myDefaultSize;
		} else if ((float)Screen.height / (float)Screen.width > myDefaultSize.y / myDefaultSize.x) {
			float t_height = (float)Screen.height / (float)Screen.width * myDefaultSize.x;
			myCanvasScaler.referenceResolution = new Vector2 (myDefaultSize.x, t_height);
		} else {
			float t_width = (float)Screen.width / (float)Screen.height * myDefaultSize.y;
			myCanvasScaler.referenceResolution = new Vector2 (t_width, myDefaultSize.y);
		}

	}

}
