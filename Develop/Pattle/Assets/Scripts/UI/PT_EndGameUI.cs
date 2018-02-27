using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;

public class PT_EndGameUI : MonoBehaviour {

	private string myNextScene = Constants.SCENE_ADVENTUREMENU;

	public void OnButtonConfirm () {
		TransitionManager.Instance.StartTransition (myNextScene);
	}
}
