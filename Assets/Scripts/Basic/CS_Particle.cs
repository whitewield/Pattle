using UnityEngine;
using System.Collections;

public class CS_Particle : MonoBehaviour {
//	public bool particlePlay;
	private ParticleSystem myParticleSystem;
	private GameObject myIdol;
	
	void Start () {
		GetMyParticleSystem ();
	}
	
	void Update () {
//		if (particlePlay)
//			On ();
//		else
//			Off ();
		if (myIdol != null) {
			this.transform.position = myIdol.transform.position;
		}
	}

	public void SetMyIdol (GameObject g_idol) {
		myIdol = g_idol;
	}

	private void GetMyParticleSystem () {
		myParticleSystem = GetComponent<ParticleSystem> ();
	}
	
	public void On () {
		if (myParticleSystem == null)
			GetMyParticleSystem ();
		if (myParticleSystem.isStopped)
			myParticleSystem.Play ();
	}
	
	public void Off () {
		if (myParticleSystem == null)
			GetMyParticleSystem ();
		if (myParticleSystem.isPlaying)
			myParticleSystem.Stop ();
	}
}
