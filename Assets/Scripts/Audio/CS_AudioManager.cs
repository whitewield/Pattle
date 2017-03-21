//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_AudioManager : MonoBehaviour {
	
	private static CS_AudioManager instance = null;

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

	[SerializeField] GameObject mySFXPrefab;
	[SerializeField] int mySFXMaxNumber = 50;
	private List<GameObject> mySFXList = new List<GameObject> ();

	[SerializeField] AudioSource myAudioSource;

	void Start () {
		for (int i = 0; i < mySFXMaxNumber; i++) {
			mySFXList.Add (Instantiate (mySFXPrefab, this.transform));
		}
	}

	private AudioSource GetIdleAudioSource () {
		for (int i = 0; i < mySFXList.Count; i++) {
			if (mySFXList [i].GetComponent<AudioSource> ().isPlaying == false)
				return mySFXList [i].GetComponent<AudioSource> ();
		}
		Debug.LogError ("Can not find idle audio source!");
		return null;
	}

	public void PlaySFX (CS_AudioClip g_SFX) {
		AudioClip t_AudioClip = g_SFX.GetMyAudioClip ();

		if (t_AudioClip == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}

		AudioSource t_SFXAudioSource = GetIdleAudioSource ();
		if (t_SFXAudioSource == null) {
			Debug.LogError ("Can not find idle audio source!");
			return;
		}

		t_SFXAudioSource.clip = t_AudioClip;
		t_SFXAudioSource.volume = g_SFX.myVolum;
		t_SFXAudioSource.pitch = g_SFX.GetMyPitch ();
		t_SFXAudioSource.Play ();

//		GameObject t_SFX = Instantiate (mySFXPrefab) as GameObject;
//		t_SFX.name = "SFX_" + t_AudioClip.name;
//		t_SFX.GetComponent<AudioSource> ().clip = t_AudioClip;
//		t_SFX.GetComponent<AudioSource> ().volume = g_SFX.myVolum;
//		t_SFX.GetComponent<AudioSource> ().pitch = g_SFX.GetMyPitch ();
//		t_SFX.GetComponent<AudioSource> ().Play ();
//		DestroyObject(t_SFX, t_AudioClip.length);
	}

	public void PlaySFX (AudioClip g_SFX, float g_volum = 1) {
		if (g_SFX == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}

		AudioSource t_SFXAudioSource = GetIdleAudioSource ();
		if (t_SFXAudioSource == null) {
			Debug.LogError ("Can not find idle audio source!");
			return;
		}

		t_SFXAudioSource.clip = g_SFX;
		t_SFXAudioSource.volume = g_volum;
		t_SFXAudioSource.Play ();

//		GameObject t_SFX = Instantiate (mySFXPrefab) as GameObject;
//		t_SFX.name = "SFX_" + g_SFX.name;
//		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
//		t_SFX.GetComponent<AudioSource> ().Play ();
//		DestroyObject(t_SFX, g_SFX.length);
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
