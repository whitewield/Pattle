using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Collection : MonoBehaviour {

	[System.Serializable]
	public struct CollectionPage {
		public Transform tagTransform;
		public GameObject collections;
		public PT_Global.ChessClass chessClass;
	}

	[SerializeField] CollectionPage myCollectionPage_Blood;
	public PT_Preset_Collection_Slot[] myCollectionPage_Blood_SlotArray;
	[SerializeField] CollectionPage myCollectionPage_Magic;
	public PT_Preset_Collection_Slot[] myCollectionPage_Magic_SlotArray;
	[SerializeField] CollectionPage myCollectionPage_Earth;
	public PT_Preset_Collection_Slot[] myCollectionPage_Earth_SlotArray;
	[SerializeField] CollectionPage myCollectionPage_Light;
	public PT_Preset_Collection_Slot[] myCollectionPage_Light_SlotArray;

	// Use this for initialization
	void Start () {
		InitPage (myCollectionPage_Blood, ref myCollectionPage_Blood_SlotArray);
		InitPage (myCollectionPage_Magic, ref myCollectionPage_Magic_SlotArray);
		InitPage (myCollectionPage_Earth, ref myCollectionPage_Earth_SlotArray);
		InitPage (myCollectionPage_Light, ref myCollectionPage_Light_SlotArray);
	}

	public void InitPage (CollectionPage g_page, ref PT_Preset_Collection_Slot[] g_array) {
		Transform t_slots = g_page.collections.transform.Find (PT_Global.Constants.NAME_SLOTS);
		List<ChessInfo> t_info = PT_DeckManager.Instance.myChessBank.GetList (g_page.chessClass);
		if (t_slots == null) {
			return;
		}

		g_array = t_slots.GetComponentsInChildren<PT_Preset_Collection_Slot> ();
		if (g_array == null || g_array.Length == 0) {
			return;
		}

		int t_chessCount = Mathf.Min (t_info.Count, g_array.Length);
		for (int i = 0; i < t_chessCount; i++) {
			g_array [i].Init (t_info [i]);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void ShowClass (string g_class) {
		ShowClass_Hide (myCollectionPage_Blood);
		ShowClass_Hide (myCollectionPage_Magic);
		ShowClass_Hide (myCollectionPage_Earth);
		ShowClass_Hide (myCollectionPage_Light);

		switch ((PT_Global.ChessClass)System.Enum.Parse (typeof(PT_Global.ChessClass), g_class)) {
		case PT_Global.ChessClass.Blood:
			ShowClass_Show (myCollectionPage_Blood);
			break;
		case PT_Global.ChessClass.Magic:
			ShowClass_Show (myCollectionPage_Magic);
			break;
		case PT_Global.ChessClass.Earth:
			ShowClass_Show (myCollectionPage_Earth);
			break;
		case PT_Global.ChessClass.Light:
			ShowClass_Show (myCollectionPage_Light);
			break;
		}
	}

	private void ShowClass_Show (CollectionPage g_page) {
		if (g_page.collections.activeSelf == false) {
			g_page.collections.SetActive (true);
			g_page.tagTransform.localPosition = new Vector2 (g_page.tagTransform.localPosition.x, PT_Global.Constants.UI_COLLECTION_TAG_ON);
		}
	}

	private void ShowClass_Hide (CollectionPage g_page) {
		if (g_page.collections.activeSelf == true) {
			g_page.collections.SetActive (false);
			g_page.tagTransform.localPosition = new Vector2 (g_page.tagTransform.localPosition.x, 0);
		}
	}
}
