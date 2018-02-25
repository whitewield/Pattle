using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_BattleCanvas : MonoBehaviour {

	public void OnButtonQuit () {
		PT_NetworkGameManager.Instance.Quit ();
	}
}
