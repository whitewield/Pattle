//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM

using UnityEngine;
using System.Collections;

public class CS_Skill : MonoBehaviour {

	public GameObject myCaster;

	public int at_MDM = 0;
	public int at_PDM = 0;
	public int at_Heal = 0;

	public bool isFriendlyFire = true;

	public GameObject subSkill;
	public GameObject subParticle;

	//===============================================================================================
	//COLLISION

	void OnTriggerEnter2D (Collider2D other) {
		CollisionAction (other.gameObject);
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		CollisionAction (collision.gameObject);
	}
	
	public virtual void CollisionAction (GameObject g_GO_Collision) {

		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;
		//if hit my caster , return
		if (g_GO_Collision == myCaster)
			return;

		if (isFriendlyFire == false && g_GO_Collision.tag == myCaster.tag) {
			return;
		}

		if (at_PDM != 0) {
			g_GO_Collision.SendMessage("DamageP" ,at_PDM);
		}
		if (at_MDM != 0) {
			g_GO_Collision.SendMessage("DamageM" ,at_MDM);
		}
		
		if (at_Heal != 0) {
			g_GO_Collision.SendMessage("Heal" ,at_Heal);
		}
	}

	public void SetMyCaster (GameObject g_Caster) {
		myCaster = g_Caster;
	}
	
	public void SetPDM (int g_PDM) {
		at_PDM = g_PDM;
	}

	public void SetMDM (int g_MDM) {
		at_MDM = g_MDM;
	}

	public void SetHeal (int g_Heal) {
		at_Heal = g_Heal;
	}

	public void Kill () {
		if (subSkill != null) {
			GameObject t_subSkill = Instantiate (subSkill, this.transform.position, Quaternion.identity) as GameObject;
			t_subSkill.SendMessage ("SetMyCaster", myCaster);
		}
		if (subParticle != null) {
			Instantiate (subParticle, this.transform.position, this.transform.rotation);
		}
		DoOnKill ();
		Destroy (this.gameObject);
	}

	public virtual void DoOnKill () {

	}
}
