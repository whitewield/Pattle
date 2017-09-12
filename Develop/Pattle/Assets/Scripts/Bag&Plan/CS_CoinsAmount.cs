using UnityEngine;
using System.Collections;

public class CS_CoinsAmount : MonoBehaviour {
	
	public TextMesh TX_Price;

	void Start () {
		UpdateSprite ();
	}

	public void UpdateSprite () {
		TX_Price.text = CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_BAG, CS_Global.SAVE_TITLE_COINS);
	}
}
