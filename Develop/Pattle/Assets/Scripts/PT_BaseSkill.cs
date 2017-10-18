using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_BaseSkill : NetworkBehaviour {

	private PT_BaseChess myCaster;

	private int myPD = 0;
	private int myMD = 0;
	private int myHeal = 0;

	[SerializeField] bool isFriendlyFire = true;

	[SerializeField] GameObject mySubSkill;
	[SerializeField] GameObject mySubParticle;

	//===============================================================================================
	//COLLISION

	void OnTriggerEnter2D (Collider2D other) {
		CollisionAction (other.gameObject);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		CollisionAction (collision.gameObject);
	}

	protected virtual void CollisionAction (GameObject g_GO_Collision) {
		if (!isServer)
			return;

		PT_BaseChess t_chess = g_GO_Collision.GetComponent<PT_BaseChess> ();

		//if hit not chess , return
		if (t_chess == null)
			return;

		//if hit my caster , return
		if (t_chess == myCaster)
			return;

		if (isFriendlyFire == false && t_chess.GetMyOwnerID () == myCaster.GetMyOwnerID ()) {
			return;
		}

		if (myPD != 0) {
			t_chess.HPModify (PT_Global.HPModifierType.PhysicalDamage, myPD);
		}
		if (myMD != 0) {
			t_chess.HPModify (PT_Global.HPModifierType.MagicDamage, myMD);
		}
		if (myHeal != 0) {
			t_chess.HPModify (PT_Global.HPModifierType.Healing, myHeal);
		}
	}

	public void SetMyCaster (PT_BaseChess g_Caster) {
		myCaster = g_Caster;
	}

	public void SetPhysicalDamage (int g_PD) {
		myPD = g_PD;
	}

	public void SetMagicDamage (int g_MD) {
		myMD = g_MD;
	}

	public void SetHeal (int g_Heal) {
		myHeal = g_Heal;
	}

	public void Kill () {
		if (mySubSkill != null) {

			//create skill
			GameObject t_subSkill = Instantiate (mySubSkill, this.transform.position, Quaternion.identity) as GameObject;

			//spawn the skill on Clients
			NetworkServer.Spawn (t_subSkill);

			t_subSkill.GetComponent<PT_BaseSkill> ().SetMyCaster (myCaster);

		}
		if (mySubParticle != null) {
			//create particle
			GameObject t_subParticle = Instantiate (mySubParticle, this.transform.position, Quaternion.identity) as GameObject;

			//spawn the particle on Clients
			NetworkServer.Spawn (t_subParticle);
		}
		DoOnKill ();
		Destroy (this.gameObject);
	}

	public virtual void DoOnKill () {

	}
}
