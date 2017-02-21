//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;

public class CS_AudioManager : MonoBehaviour {
	
	private static CS_AudioManager instance = null;

	public GameObject myPrefabSFX;

	public AudioSource myAudioSource;

	//========================================================================
	public static CS_AudioManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	public void PlaySFX (CS_AudioClip g_SFX) {
		AudioClip t_AudioClip = g_SFX.GetMyAudioClip ();

		if (t_AudioClip == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}

		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + t_AudioClip.name;
		t_SFX.GetComponent<AudioSource> ().clip = t_AudioClip;
		t_SFX.GetComponent<AudioSource> ().volume = g_SFX.myVolum;
		t_SFX.GetComponent<AudioSource> ().pitch = g_SFX.GetMyPitch ();
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, t_AudioClip.length);
	}

	public void PlaySFX (AudioClip g_SFX) {
		if (g_SFX == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlayBGM (AudioClip g_BGM) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.Play ();
			return;
		}

		if (g_BGM == myAudioSource.clip)
			return;

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.Play ();
	}

	public void PlayBGM (AudioClip g_BGM, float g_Volume) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.volume = g_Volume;
			myAudioSource.Play ();
			return;
		} else if (g_BGM == myAudioSource.clip) {
			myAudioSource.volume = g_Volume;
			return;
		}

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.volume = g_Volume;
		myAudioSource.Play ();
	}

	public void StopBGM () {
		myAudioSource.Stop ();
	}

}
