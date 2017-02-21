using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_CoinManager : MonoBehaviour {

	public GameObject coin;	
	public GameObject PT_Coin;
	public Vector2[] presetPosition;
	public List<GameObject> coinList = new List<GameObject>();

	void Start () {
		for (int i = 0; i < presetPosition.Length; i++) {
			GameObject t_coin = Instantiate(coin, presetPosition[i], Quaternion.identity) as GameObject;
			coinList.Add (t_coin);
			t_coin.SendMessage("SetMyManager", this.gameObject);
		}
	}

	void Update () {
	
	}

	public void RemoveCoin (GameObject g_coin) {
		coinList.Remove (g_coin);
		Instantiate (PT_Coin, g_coin.transform.position, Quaternion.identity);
		Destroy (g_coin);
		if(coinList.Count == 0)
			GameObject.Find(CS_Global.NAME_AFTERBATTLE).SendMessage("Win");
	}
}
