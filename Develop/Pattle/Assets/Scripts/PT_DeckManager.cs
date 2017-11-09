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

	[SerializeField] PT_Global.ChessType[] myChessTypes = new PT_Global.ChessType[3];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetChessTypes (PT_Global.ChessType[] g_types) {
		myChessTypes = g_types;
	}

	public PT_Global.ChessType[] GetChessTypes () {
		return myChessTypes;
	}
}
