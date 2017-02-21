using UnityEngine;
using System.Collections;

public class CS_Coin : MonoBehaviour {

	private GameObject myCoinManager;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == CS_Global.TAG_A)
			myCoinManager.SendMessage ("RemoveCoin", this.gameObject);
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == CS_Global.TAG_A)
			myCoinManager.SendMessage ("RemoveCoin", this.gameObject);
	}

	public void SetMyManager (GameObject g_coinManager) {
		myCoinManager = g_coinManager;
	}
}
