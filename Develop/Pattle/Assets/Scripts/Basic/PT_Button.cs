using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Hang.AiryAudio;

public class PT_Button : MonoBehaviour {
	// for position change
	[SerializeField] bool doPositionChange = false;
	[SerializeField] Transform myButtonTransform;
	[SerializeField] Vector3 myNormalPosition;
	[SerializeField] Vector3 myPressedPosition;

	// button event
	[Serializable]
	public class PT_ButtonEvent : UnityEvent { }

	[SerializeField]
	private PT_ButtonEvent onClick = new PT_ButtonEvent();
	public PT_ButtonEvent onClickEvent { get { return onClick; } set { onClick = value; } }

	// check if it's pressed
	private bool isPressed;

	// sound
	[SerializeField] AiryAudioData myAiryAudioData_Down;

	/// <summary>
	/// if the mouse pressed down
	/// </summary>
	public void OnMouseDown () {
		// play sound
		if (myAiryAudioData_Down != null)
			myAiryAudioData_Down.Play ();
		// change position
		if (doPositionChange)
			myButtonTransform.localPosition = myPressedPosition;
		// set isPressed to true
		isPressed = true;
	}

	/// <summary>
	/// if the mouse back up
	/// </summary>
	public void OnMouseUp () {
		// change position
		if (doPositionChange)
			myButtonTransform.localPosition = myNormalPosition;
		// activate the pressing event
		if (isPressed)
			onClickEvent.Invoke ();
	}

	/// <summary>
	/// if the mouse exit from the button
	/// </summary>
	public void OnMouseExit() {
		// change position
		if (doPositionChange)
			myButtonTransform.localPosition = myNormalPosition;
		// set isPressed to false
		isPressed = false;
	}

}
///add a collider to the object as well so the OnPointerClick can work

