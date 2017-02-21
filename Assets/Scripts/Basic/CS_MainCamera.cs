using UnityEngine;
using System.Collections;

public class CS_MainCamera : MonoBehaviour {
	private float targetSize = 10.24f;
	private float ratio = 0.2f;						//0-1	0: don't change	1:fast change 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.orthographicSize = (1 - ratio) * Camera.main.orthographicSize + ratio * targetSize;
	}

	public void SetSize (float t_size) {
		targetSize = t_size;
	}
}
