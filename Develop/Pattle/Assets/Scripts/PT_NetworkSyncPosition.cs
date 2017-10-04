using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_NetworkSyncPosition : NetworkBehaviour {

	[Range (0, 60)]
	[SerializeField] float myLerpRate = 15;

	[SyncVar] private Vector3 mySyncPosition;
	private Transform myTransform;

	void Awake () {
		myTransform = this.GetComponent<Transform> ();
	}

	void FixedUpdate () {
		if (isServer) {
			mySyncPosition = myTransform.position;
		}

		if (isClient) {
			myTransform.position = Vector3.Lerp (myTransform.position, mySyncPosition, Time.fixedDeltaTime * myLerpRate);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
