using UnityEngine;
using System.Collections;

public class CS_ShowMouseMaganer : MonoBehaviour {

	private static CS_ShowMouseMaganer instance = null;

	public GameObject showMouse;
	public AudioClip mySFX;

	//========================================================================
	public static CS_ShowMouseMaganer Instance {
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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector2 t_Postion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Instantiate (showMouse, t_Postion, Quaternion.identity);
			CS_AudioManager.Instance.PlaySFX (mySFX);
		}
	}
}
