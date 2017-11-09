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

	public void OnMouseDown () {
		if (doPositionChange)
			myButtonTransform.localPosition = myPressedPosition;
		onClickEvent.Invoke ();
	}

	public void OnMouseUp () {
		if (doPositionChange)
			myButtonTransform.localPosition = myNormalPosition;
	}

}
///add a collider to the object as well so the OnPointerClick can work

