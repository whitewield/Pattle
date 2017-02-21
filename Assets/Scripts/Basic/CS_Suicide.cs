using UnityEngine;
using System.Collections;

public class CS_Suicide : MonoBehaviour {
	public float SuicideTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SuicideTime -= Time.deltaTime; 

		if (SuicideTime < 0)
			Kill ();
	}

	public void Kill () {
		Destroy (this.gameObject);
	}
}
