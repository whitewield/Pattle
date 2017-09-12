using UnityEngine;
using System.Collections;

public class CS_BagButtonBuy : MonoBehaviour {

	private bool status = false;
	private string chessName;
	private int price;

	public SpriteRenderer backGround;
	public TextMesh TX_Price;

	public Color color_on = new Color (0.64f, 0.72f, 0.8f);
	public Color color_off = Color.gray;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (status == false)
			return;

		//buy chess
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("SaveLoseCoins", price);
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("SaveAddChess", chessName);

		//update bag
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("UpdateBag");


		this.gameObject.SetActive (false);
	}

	public void SetPrice (int g_price) {
		TX_Price.text = g_price.ToString ();
		price = g_price;
	}

	public void On (string g_chessName) {
		backGround.color = color_on;
		status = true;
		chessName = g_chessName;
	}

	public void Off () {
		backGround.color = color_off;
		status = false;
	}


}
