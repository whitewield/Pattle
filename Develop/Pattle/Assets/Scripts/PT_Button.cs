using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PT_Button : MonoBehaviour {

	[SerializeField] bool doPositionChange = false;
	[SerializeField] Transform myButtonTransform;
	[SerializeField] Vector3 myNormalPosition;
	[SerializeField] Vector3 myPressedPosition;

	//my event
	[Serializable]
	public class PT_ButtonEvent : UnityEvent { }

	[SerializeField]
	private PT_ButtonEvent onClick = new PT_ButtonEvent();
	public PT_ButtonEvent onClickEvent { get { return onClick; } set { onClick = value; } }

	private bool isPressed;

	public void OnMouseDown () {
		if (doPositionChange)
			myButtonTransform.localPosition = myPressedPosition;

		isPressed = true;
	}

	public void OnMouseUp () {
		if (doPositionChange)
			myButtonTransform.localPosition = myNormalPosition;

		if (isPressed)
			onClickEvent.Invoke ();
	}

	public void OnMouseExit() {
		if (doPositionChange)
			myButtonTransform.localPosition = myNormalPosition;
		
		isPressed = false;
	}

}
///add a collider to the object as well so the OnPointerClick can work

