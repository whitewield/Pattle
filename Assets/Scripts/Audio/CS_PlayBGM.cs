using UnityEngine;
using System.Collections;

public class CS_PlayBGM : MonoBehaviour {
	public AudioClip myBGM;
	public float myVolume;
	// Use this for initialization
	void Start () {
		if (myBGM == null)
			CS_AudioManager.Instance.StopBGM ();
		CS_AudioManager.Instance.PlayBGM (myBGM, myVolume);
	}
}
