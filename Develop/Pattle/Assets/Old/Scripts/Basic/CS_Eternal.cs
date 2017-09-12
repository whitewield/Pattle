using UnityEngine;
using System.Collections;

public class CS_Eternal : MonoBehaviour {
	void Awake() {
		//do not destroy the object when loading new level
		DontDestroyOnLoad(transform.gameObject);
	}
}
