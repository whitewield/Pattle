using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset : MonoBehaviour {

	enum PresetState {
		Hide,
		ShowDeck,
		ShowCollection,
		ShowFormation,
	}

	[System.Serializable]
	struct StatePosition {
		public PresetState state;
		public Vector3 position;
	}

	[System.Serializable]
	struct AnimatedSection {
		public Transform sectionTransform;
		public Vector3 defaultPosition;
		public StatePosition[] statePositions;
		public Vector3 targetPosition;
		public bool doAnimation;
	}

	private PresetState myState = PresetState.ShowDeck;
	[SerializeField] AnimatedSection[] myAnimatedSections;

	public PT_Preset_Collection myCollection;
	public PT_Preset_Deck myDeck;

	private static PT_Preset instance = null;

	//========================================================================
	public static PT_Preset Instance {
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
		//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	// Use this for initialization
	void Start () {
		InitAnimation ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAnimation ();
	}

	public void OnButtonBack () {
		myState = PresetState.ShowDeck;
//		UpdateAnimator ();
		SetAnimation (myState);
	}

	public void OnButtonDeck () {
		Debug.Log ("OnButtonDeck");
		if(myState == PresetState.ShowDeck || myState == PresetState.ShowFormation)
			myState = PresetState.ShowCollection;
		else if(myState == PresetState.ShowCollection)
			myState = PresetState.ShowFormation;
//		UpdateAnimator ();
		SetAnimation (myState);
	}

	public void OnButtonDone () {
		myState = PresetState.ShowDeck;
//		UpdateAnimator ();
		SetAnimation (myState);
	}

	private void InitAnimation () {
		for (int i = 0; i < myAnimatedSections.Length; i++) {
			myAnimatedSections [i].sectionTransform.localPosition = myAnimatedSections [i].defaultPosition;
			myAnimatedSections [i].targetPosition = myAnimatedSections [i].defaultPosition;
		}
	}

	private void SetAnimation (PresetState g_state) {
		for (int i = 0; i < myAnimatedSections.Length; i++) {
			bool f_isDefault = true;

			for (int j = 0; j < myAnimatedSections [i].statePositions.Length; j++) {
				if (myAnimatedSections [i].statePositions [j].state == g_state) {
					myAnimatedSections [i].targetPosition = myAnimatedSections [i].statePositions [j].position;
					f_isDefault = false;
					break;
				}
			}

			if (f_isDefault)
				myAnimatedSections [i].targetPosition = myAnimatedSections [i].defaultPosition;

			myAnimatedSections [i].doAnimation = true;
		}
	}

	private void UpdateAnimation () {
		for (int i = 0; i < myAnimatedSections.Length; i++) {
			if (myAnimatedSections [i].doAnimation == true) {
				myAnimatedSections [i].sectionTransform.localPosition = 
				Vector3.Lerp (myAnimatedSections [i].sectionTransform.localPosition, 
					myAnimatedSections [i].targetPosition, 
					Time.deltaTime * PT_Global.Constants.SPEED_UI_LERP);
				if (Vector3.SqrMagnitude (
					    myAnimatedSections [i].sectionTransform.localPosition - myAnimatedSections [i].targetPosition
				    ) <
					PT_Global.Constants.DISTANCE_UI_RESET) {
					myAnimatedSections [i].sectionTransform.localPosition = myAnimatedSections [i].targetPosition;
					myAnimatedSections [i].doAnimation = false;
				}
			}
			
		}
	}

//	public void UpdateAnimator () {
//		myAnimator.SetInteger ("State", (int)myState);
//	}
}
