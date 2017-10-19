using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_Suicide : NetworkBehaviour {

	public float mySuicideTime;

	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}

		mySuicideTime -= Time.deltaTime; 
		if (mySuicideTime < 0)
			Kill ();
	}

	public void Kill () {
		Destroy (this.gameObject);
	}
}
