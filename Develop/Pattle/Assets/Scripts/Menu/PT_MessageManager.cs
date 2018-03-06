using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_MessageManager : MonoBehaviour {
	
	protected static PT_MessageManager instance = null;
	public static PT_MessageManager Instance { get { return instance; } }



	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowMessage () {
		
	}

	public void OnButtonHide () {
		
	}
}
