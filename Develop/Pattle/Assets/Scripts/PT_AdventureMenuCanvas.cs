﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_AdventureMenuCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonBack () {
		TransitionManager.Instance.StartTransition ("NetworkMenu");
	}
}
