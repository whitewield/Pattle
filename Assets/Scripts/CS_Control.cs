using UnityEngine;
using System.Collections;

public class CS_Control : MonoBehaviour {

	// team number of the chess
	public int myTeamNumber;
	public string myTeamTag;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			
		}
	}

	public virtual void SetGO (GameObject g_GO) {
	}

	public virtual void UnsetGO (GameObject g_GO) {
	}


}
