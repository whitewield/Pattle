using UnityEngine;
using System.Collections;

public class CS_InitBattle : MonoBehaviour {
	
	void Start () {
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("InitBattle");
	}

}
