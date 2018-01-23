using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TransitionManager.Instance.EndTransition ();
	}

}
