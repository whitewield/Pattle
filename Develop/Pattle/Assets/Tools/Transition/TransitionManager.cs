using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TransitionManager : MonoBehaviour {

	private static TransitionManager instance = null;

	//========================================================================
	public static TransitionManager Instance {
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
		this.transform.position = Vector3.zero;
	}
	//========================================================================

//	[SerializeField] Animator myAnimator;
//	[SerializeField] Animator myGrapeAnimator;

//	[SerializeField] float myLoadingWaitTime = 5;
	private string myNextScene;

	private enum Status {
		Idle,
		TransitionIn,
		TransitionOut
	}

	public enum TransitionMode {
		Normal,
		StartHost,
		StopHost,
		StartClient,
		StopClient,
	}

	[SerializeField] Collider2D myCollider2D;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] Color myTransitionColor = Color.black;
	[SerializeField] float myAnimationTime = 0.5f;
	private float myAnimationTimer;
	private Status myStatus = Status.Idle;
	private TransitionMode myTransitionMode = TransitionMode.Normal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTransitionIn ();
		UpdateTransitionOut ();

//		Debug.Log (myAnimationTimer);
	}

	public void UpdateTransitionIn () {
		if (myStatus == Status.TransitionIn) {
			myAnimationTimer -= Time.unscaledDeltaTime;
			if (myAnimationTimer <= 0) {
				myAnimationTimer = 0;
				myStatus = Status.Idle;


			}

			//change the color
			Color t_color = myTransitionColor;
			t_color.a = (myAnimationTimer / myAnimationTime);
			mySpriteRenderer.color = t_color;
		}
	}

	public void UpdateTransitionOut () {
//		Debug.Log (myAnimationTimer);
		if (myStatus == Status.TransitionOut) {
			myAnimationTimer -= Time.unscaledDeltaTime;
			if (myAnimationTimer <= 0) {
				myAnimationTimer = 0;
				myStatus = Status.Idle;
				StartLoading ();
			}

			//change the color
			Color t_color = myTransitionColor;
			t_color.a = 1 - (myAnimationTimer / myAnimationTime);
			mySpriteRenderer.color = t_color;
		}
	}

	public void StartTransition (string g_scene) {
		myTransitionMode = TransitionMode.Normal;

		myNextScene = g_scene;
		//		myGrapeAnimator.SetBool ("isGrape", true);
		TransitionOut ();
	}

	public void StartTransition (TransitionMode g_trasitionMode) {
		myTransitionMode = g_trasitionMode;

		myNextScene = "";
		//		myGrapeAnimator.SetBool ("isGrape", true);
		TransitionOut ();
	}

	public void EndTransition () {
		TransitionIn ();
	}

	public void TransitionIn () {
		
		myAnimationTimer = myAnimationTime;
		myStatus = Status.TransitionIn;
		//		myAnimator.SetBool ("isTransitioning", false);
		myCollider2D.enabled = false;
	}

	public void TransitionOut () {
		Debug.Log ("TransitionOut");
		myAnimationTimer = myAnimationTime;
		myStatus = Status.TransitionOut;
//		myAnimator.SetBool ("isTransitioning", true);
		myCollider2D.enabled = true;
	}

	public void StartLoading () {
		switch (myTransitionMode) {
		case TransitionMode.Normal:
			SceneManager.LoadSceneAsync (myNextScene);
			break;
		case TransitionMode.StartHost:
			NetworkManager.singleton.StartHost ();
			break;
		case TransitionMode.StopHost:
			NetworkManager.singleton.StopHost ();
			break;
		case TransitionMode.StartClient:
			NetworkManager.singleton.StartClient ();
			break;
		case TransitionMode.StopClient:
			NetworkManager.singleton.StopClient ();
			break;
		}
	}
}
