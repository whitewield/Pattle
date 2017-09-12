using UnityEngine;
using System.Collections;

public class CS_Anima_ZoomIn : MonoBehaviour {

	[SerializeField] float speed = 1;
	[SerializeField] bool isOnce = true;
	[SerializeField] float delay = 0;
	[SerializeField] bool distanceDealy_isOn = false;
	[SerializeField] Vector3 distanceDealy_Center;
	[SerializeField] float distanceDealy_Ratio = 1;

	private bool isDone = false;

	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.zero;

		if (distanceDealy_isOn)
			delay = Vector2.Distance (distanceDealy_Center, this.transform.position) * distanceDealy_Ratio;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDone)
			return;

		if (delay > 0) {
			delay -= Time.deltaTime;
			return;
		}
			
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * speed);

		if (Vector3.Distance (transform.localScale, Vector3.one) < 0.01) {
			transform.localScale = Vector3.one;
			if (isOnce)
				Destroy (this);
			else
				isDone = true;
		}
	}
}
