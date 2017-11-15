using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Preset_Field_Chess : MonoBehaviour {

	private SpriteRenderer mySpriteRenderer;
	private Collider2D myCollider2D;

	void Awake () {
		mySpriteRenderer = this.GetComponent<SpriteRenderer> ();
		myCollider2D = this.GetComponent<CircleCollider2D> ();
	}

	public void SetSprite (Sprite g_sprite) {
		mySpriteRenderer.sprite = g_sprite;
		if (g_sprite == null) {
			myCollider2D.enabled = false;
		} else {
			myCollider2D.enabled = true;
		}
	}

	public Sprite GetSprite () {
		return mySpriteRenderer.sprite;
	}
}
