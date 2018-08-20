using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// commit suicide 
/// </summary>
public class PT_Suicide : NetworkBehaviour {

	public float mySuicideTime = -1;

	void Start () {
		if (!isServer) {
			return;
		}

		if (mySuicideTime > 0)
			Destroy (this.gameObject, mySuicideTime);
	}

	public void Kill () {
		Destroy (this.gameObject);
	}
}
