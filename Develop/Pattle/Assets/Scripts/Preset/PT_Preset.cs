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

	[SerializeField] PT_Preset_Collection myCollection;
	[SerializeField] PT_Preset_Deck myDeck;
	[SerializeField] PT_Preset_Field myField;

	[SerializeField] Camera myPresetCamera;
	[SerializeField] SpriteRenderer myDragChess;
	private bool isMouseDown = false;
	private PT_Preset_Collection_Slot myInput_Collection_Slot;
	private PT_Preset_Deck_Slot myInput_Deck_Slot;
	private PT_Preset_Field_Chess myInput_Field_Chess;

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
		UpdateInput ();
	}

	private void UpdateInput () {
		Debug.Log (myState);
		switch (myState) {
		case PresetState.ShowCollection:
			UpdateInput_Collection ();
			break;
		case PresetState.ShowFormation:
			UpdateInput_Formation ();
			break;
		}
	}

	private void UpdateInput_Collection () {

		if (Input.GetMouseButtonDown (0) && isMouseDown == false) {
//			Debug.Log ("GetMouseButtonDown");

			isMouseDown = false;

			Collider2D t_collider = 
				Physics2D.OverlapPoint (myPresetCamera.ScreenToWorldPoint (Input.mousePosition), (int)Mathf.Pow (2, 8));

			if (t_collider != null) {
				myInput_Collection_Slot = t_collider.GetComponent<PT_Preset_Collection_Slot> ();
				if (myInput_Collection_Slot != null &&
				    myInput_Collection_Slot.GetChessType () != PT_Global.ChessType.none &&
				    myInput_Collection_Slot.GetInUse () == false) {
					myInput_Collection_Slot.SetInUse (true);
					myDragChess.sprite = myInput_Collection_Slot.GetSprite ();
					isMouseDown = true;
				} else {
					myInput_Collection_Slot = null;
				}

				myInput_Deck_Slot = t_collider.GetComponent<PT_Preset_Deck_Slot> ();
				if (myInput_Deck_Slot != null &&
					myInput_Deck_Slot.GetChessType () != PT_Global.ChessType.none) {
					myInput_Deck_Slot.RemoveSprite ();
					myDragChess.sprite = myInput_Deck_Slot.GetSprite ();
					isMouseDown = true;
				} else {
					myInput_Deck_Slot = null;
				}
			}
				
		}

		if (isMouseDown) {
//			Debug.Log ("isMouseDown");
			myDragChess.transform.position = (Vector2)myPresetCamera.ScreenToWorldPoint (Input.mousePosition);
		}

		if (Input.GetMouseButtonUp (0) && isMouseDown) {
//			Debug.Log ("GetMouseButtonUp");
			isMouseDown = false;

			Collider2D t_collider = 
				Physics2D.OverlapPoint (myPresetCamera.ScreenToWorldPoint (Input.mousePosition), (int)Mathf.Pow (2, 8));

			PT_Preset_Deck_Slot t_deckSlot = null;
			if (t_collider != null)
				t_deckSlot = t_collider.GetComponent<PT_Preset_Deck_Slot> ();


			if (myInput_Collection_Slot != null && t_deckSlot != null) {
				//replace a chess from collection
				//active the replaced chess
				myCollection.SetChessInUse (t_deckSlot.GetChessType (), false);
				//change the chess in the deck
				t_deckSlot.SetChessInfo (myInput_Collection_Slot.GetChessInfo ());
				UpdateInput_Collection_Reset ();
			} else if (myInput_Deck_Slot != null && t_deckSlot != null ) {
				//change the chess in the deck
				ChessInfo t_chessInfo = myInput_Deck_Slot.GetChessInfo ();
				myInput_Deck_Slot.SetChessInfo (t_deckSlot.GetChessInfo ());
				t_deckSlot.SetChessInfo (t_chessInfo);
				UpdateInput_Collection_Reset ();
			} else if (myInput_Deck_Slot != null) {
				// put the chess back
				myCollection.SetChessInUse (myInput_Deck_Slot.GetChessType (), false);
				// empty the slots
				myInput_Deck_Slot.SetChessInfo (PT_DeckManager.Instance.myChessBank.emptyInfo);
				UpdateInput_Collection_Reset ();
			}

			if (myInput_Collection_Slot != null) {
				myInput_Collection_Slot.SetInUse (false);
			}
			UpdateInput_Collection_Reset ();
		}
	}

	private void UpdateInput_Collection_Reset () {
		//remove the sprite of the drag chess
		myDragChess.sprite = null;
		myInput_Collection_Slot = null;
		myInput_Deck_Slot = null;
	}

	private void UpdateInput_Formation () {

		if (Input.GetMouseButtonDown (0) && isMouseDown == false) {
			//			Debug.Log ("GetMouseButtonDown");

			isMouseDown = false;

			Collider2D t_collider = 
				Physics2D.OverlapPoint (myPresetCamera.ScreenToWorldPoint (Input.mousePosition), (int)Mathf.Pow (2, 8));

			if (t_collider != null) {
				myInput_Field_Chess = t_collider.GetComponent<PT_Preset_Field_Chess> ();
				if (myInput_Field_Chess != null) {
					myDragChess.sprite = myInput_Field_Chess.GetSprite ();
					myInput_Field_Chess.SetSprite (null);
					isMouseDown = true;
				}
			}

		}

		if (isMouseDown) {
			//			Debug.Log ("isMouseDown");
			myDragChess.transform.position = (Vector2)myPresetCamera.ScreenToWorldPoint (Input.mousePosition);
		}

		if (Input.GetMouseButtonUp (0) && isMouseDown) {
			//			Debug.Log ("GetMouseButtonUp");
			isMouseDown = false;

			Collider2D t_collider = 
				Physics2D.OverlapPoint (myPresetCamera.ScreenToWorldPoint (Input.mousePosition), (int)Mathf.Pow (2, 8));

			PT_Preset_Field t_field = null;
			if (t_collider != null)
				t_field = t_collider.GetComponent<PT_Preset_Field> ();

			if (t_field != null) {
				myInput_Field_Chess.transform.position = myDragChess.transform.position;
				Vector3 t_pos = myInput_Field_Chess.transform.localPosition;
				t_pos.z = 0;
				myInput_Field_Chess.transform.localPosition = t_pos;
			}

			PT_Preset_Field_Chess t_chess = null;
			if (t_collider != null)
				t_chess = t_collider.GetComponent<PT_Preset_Field_Chess> ();

			if (t_chess != null) {
				Vector2 t_position = t_chess.transform.position;
				t_chess.transform.position = myInput_Field_Chess.transform.position;
				myInput_Field_Chess.transform.position = t_position;
			}

			myInput_Field_Chess.SetSprite (myDragChess.sprite);
			myDragChess.sprite = null;
			myInput_Field_Chess = null;
		}
	}

	#region Buttons
	public void OnButtonBack () {
		myCollection.SetChessInUse (PT_DeckManager.Instance.GetChessTypes ());
		myDeck.SetFromDeckManager ();
		myField.SetPositionFromDeckManager ();
		myState = PresetState.ShowDeck;
		SetAnimation (myState);
	}

	public void OnButtonDeck () {
		if (myState == PresetState.ShowDeck || myState == PresetState.ShowFormation)
			myState = PresetState.ShowCollection;
		else if (myState == PresetState.ShowCollection) {
			if (myDeck.CheckReady ()) {
				myField.SetSprites (myDeck.GetSprites ());
				myState = PresetState.ShowFormation;
			}
		}
		SetAnimation (myState);
	}

	public void OnButtonDone () {
		//save 
		myDeck.ApplyChessType();
		myField.ApplyChessPosition();

		myState = PresetState.ShowDeck;
		SetAnimation (myState);
	}
	#endregion

	#region Animations
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
					Time.unscaledDeltaTime * PT_Global.Constants.SPEED_UI_LERP);
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
	#endregion

}
