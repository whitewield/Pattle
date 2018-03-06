using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_NetworkGameCanvas : MonoBehaviour {

	public void OnButtonQuit () {
		PT_PlayerController[] t_players = PT_NetworkGameManager.Instance.myPlayerList;
		for (int i = 0; i < t_players.Length; i++) {
			if (t_players [i] != null) {
				t_players [i].Lose ();
			}
		}
	}
}
