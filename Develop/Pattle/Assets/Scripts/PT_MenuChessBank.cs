using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Global;

public class PT_MenuChessBank : MonoBehaviour {
	[SerializeField] float myMoveSpeed = 0.2f;
	[SerializeField] float myMoveEnd = 12;

	private SpriteRenderer[] myChessSpriteRenderers;
	private List<Sprite> myChessSpriteList = new List<Sprite> ();
	private int myChessSpriteList_End;

	// Use this for initialization
	void Start () {

		//init the spite list

		for (int i = (int)ChessClass.Start + 1; i < (int)ChessClass.End; i++) {
			ChessClass t_chessClass = (ChessClass)i;
			List<ChessInfo> t_chessInfoList = PT_DeckManager.Instance.myChessBank.GetList (t_chessClass);

			if (t_chessInfoList == null)
				continue;
			
			for (int j = 0; j < t_chessInfoList.Count; j++) {
				SpriteRenderer t_spriteRenderer = t_chessInfoList [j].prefab.GetComponent<SpriteRenderer> ();
				if (t_spriteRenderer != null && t_spriteRenderer.sprite != null)
					myChessSpriteList.Add (t_spriteRenderer.sprite);
			}
		}

		myChessSpriteList_End = myChessSpriteList.Count;


		//init the chess on scene

		myChessSpriteRenderers = this.transform.GetComponentsInChildren<SpriteRenderer> ();
		for (int i = 0; i < myChessSpriteRenderers.Length; i++) {
			myChessSpriteRenderers [i].sprite = GetSprite ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Update_Move ();
	}

	private void Update_Move () {
		for (int i = 0; i < myChessSpriteRenderers.Length; i++) {
			Vector3 t_position = 
				myChessSpriteRenderers [i].transform.localPosition + Vector3.right * myMoveSpeed * Time.deltaTime;

			//if move to the end, change sprite

			if (t_position.x > myMoveEnd) {
				t_position.x -= myMoveEnd * 2;
				myChessSpriteRenderers [i].sprite = GetSprite ();
			}

			myChessSpriteRenderers [i].transform.localPosition = t_position;
		}
	}

	private Sprite GetSprite () {
		int t_index = Random.Range (0, myChessSpriteList_End);
		Sprite t_sprite = myChessSpriteList [t_index];

		//swap sprite

		myChessSpriteList [t_index] = myChessSpriteList [myChessSpriteList_End - 1];
		myChessSpriteList [myChessSpriteList_End - 1] = t_sprite;

		myChessSpriteList_End--;
		if (myChessSpriteList_End == 0) {
			myChessSpriteList_End = myChessSpriteList.Count;
		}

		return t_sprite;
	}
}
