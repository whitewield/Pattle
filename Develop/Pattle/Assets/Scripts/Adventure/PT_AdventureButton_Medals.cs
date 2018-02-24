using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;
using UnityEngine.UI;

public class PT_AdventureButton_Medals : MonoBehaviour {

	[SerializeField] Image[] myMedalImages;
	public void Show (MedalType[] g_medals) {
		for (int i = 0; i < g_medals.Length; i++) {
			myMedalImages [i].color = PT_AdventureMenuCanvas.Instance.GetMedalColor(g_medals [i]);
		}
	}
}
