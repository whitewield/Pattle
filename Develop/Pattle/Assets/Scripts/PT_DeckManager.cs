using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_DeckManager : MonoBehaviour {
	
	private static PT_DeckManager instance = null;

	//========================================================================
	public static PT_DeckManager Instance {
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
	}
	//========================================================================

	public SO_ChessBank myChessBank;

	public PT_Global.ChessType[] myChessPrefabs;

	[SerializeField] PT_Global.ChessType[] myChessPrefabsSet1;
	[SerializeField] PT_Global.ChessType[] myChessPrefabsSet2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			Debug.Log ("Set1");
			myChessPrefabs = myChessPrefabsSet1;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			Debug.Log ("Set2");
			myChessPrefabs = myChessPrefabsSet2;
		}
	}
}
