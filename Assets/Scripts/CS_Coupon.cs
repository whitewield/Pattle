using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CS_Coupon : MonoBehaviour {

	public InputField IF_coupon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Verify () {
		string t_coupon = IF_coupon.text;
		int t_coinsAmount = CS_StoreList.GetCoupon (t_coupon);
		//if doesn't exist, return
		if (t_coinsAmount == -1) {
			Debug.Log ("Coupon doesn't exist : " + t_coupon);
			return;
		}

		//if already have, return
		if (CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_COUPON, t_coupon) == "1") {
			Debug.Log ("Already have coupon : " + t_coupon);
			return;
		}

		CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_COUPON, t_coupon, "1");
		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("SaveAddCoins", t_coinsAmount);
		Debug.Log ("Get Coupon : " + t_coupon);

		GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("UpdateBag");
	}
}
